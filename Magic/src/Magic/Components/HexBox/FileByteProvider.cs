using System;
using System.IO;

namespace Magic.Components
{
    /// <summary>
    /// Implements a fully editable byte provider for file data of any size.
    /// </summary>
    /// <remarks>
    /// Only changes to the file are stored in memory with reads from the
    /// original data occurring as required.
    /// </remarks>
    public sealed class FileByteProvider : IByteProvider, IDisposable
    {
        const Int32 COPY_BLOCK_SIZE = 4096;

        String _fileName;
        Stream _stream;
        DataMap _dataMap;
        Int64 _totalLength;
        Boolean _readOnly;

        /// <summary>
        /// Constructs a new <see cref="FileByteProvider" /> instance.
        /// </summary>
        /// <param name="fileName">The name of the file from which bytes should be provided.</param>
        public FileByteProvider(String fileName) : this(fileName, false) { }

        /// <summary>
        /// Constructs a new <see cref="FileByteProvider" /> instance.
        /// </summary>
        /// <param name="fileName">The name of the file from which bytes should be provided.</param>
        /// <param name="readOnly">True, opens the file in read-only mode.</param>
        public FileByteProvider(String fileName, Boolean readOnly)
        {
            this._fileName = fileName;

            if (!readOnly)
            {
                this._stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            else
            {
                this._stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            this._readOnly = readOnly;

            this.ReInitialize();
        }

        /// <summary>
        /// Constructs a new <see cref="FileByteProvider" /> instance.
        /// </summary>
        /// <param name="stream">the stream containing the data.</param>
        /// <remarks>
        /// The stream must supported seek operations.
        /// </remarks>
        public FileByteProvider(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanSeek)
                throw new ArgumentException("stream must supported seek operations(CanSeek)");

            this._stream = stream;
            this._readOnly = !stream.CanWrite;
            this.ReInitialize();
        }

        #region IByteProvider Members
        /// <summary>
        /// See <see cref="IByteProvider.LengthChanged" /> for more information.
        /// </summary>
        public event EventHandler LengthChanged;

        /// <summary>
        /// See <see cref="IByteProvider.Changed" /> for more information.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// See <see cref="IByteProvider.ReadByte" /> for more information.
        /// </summary>
        public Byte ReadByte(Int64 index)
        {
            Int64 blockOffset;
            DataBlock block = this.GetDataBlock(index, out blockOffset);
            FileDataBlock fileBlock = block as FileDataBlock;
            if (fileBlock != null)
            {
                return this.ReadByteFromFile(fileBlock.FileOffset + index - blockOffset);
            }
            else
            {
                MemoryDataBlock memoryBlock = (MemoryDataBlock)block;
                return memoryBlock.Data[index - blockOffset];
            }
        }

        /// <summary>
        /// See <see cref="IByteProvider.WriteByte" /> for more information.
        /// </summary>
        public void WriteByte(Int64 index, Byte value)
        {
            try
            {
                // Find the block affected.
                Int64 blockOffset;
                DataBlock block = this.GetDataBlock(index, out blockOffset);

                // If the byte is already in a memory block, modify it.
                MemoryDataBlock memoryBlock = block as MemoryDataBlock;
                if (memoryBlock != null)
                {
                    memoryBlock.Data[index - blockOffset] = value;
                    return;
                }

                FileDataBlock fileBlock = (FileDataBlock)block;

                // If the byte changing is the first byte in the block and the previous block is a memory block, extend that.
                if (blockOffset == index && block.PreviousBlock != null)
                {
                    MemoryDataBlock previousMemoryBlock = block.PreviousBlock as MemoryDataBlock;
                    if (previousMemoryBlock != null)
                    {
                        previousMemoryBlock.AddByteToEnd(value);
                        fileBlock.RemoveBytesFromStart(1);
                        if (fileBlock.Length == 0)
                        {
                            this._dataMap.Remove(fileBlock);
                        }
                        return;
                    }
                }

                // If the byte changing is the last byte in the block and the next block is a memory block, extend that.
                if (blockOffset + fileBlock.Length - 1 == index && block.NextBlock != null)
                {
                    MemoryDataBlock nextMemoryBlock = block.NextBlock as MemoryDataBlock;
                    if (nextMemoryBlock != null)
                    {
                        nextMemoryBlock.AddByteToStart(value);
                        fileBlock.RemoveBytesFromEnd(1);
                        if (fileBlock.Length == 0)
                        {
                            this._dataMap.Remove(fileBlock);
                        }
                        return;
                    }
                }

                // Split the block into a prefix and a suffix and place a memory block in-between.
                FileDataBlock prefixBlock = null;
                if (index > blockOffset)
                {
                    prefixBlock = new FileDataBlock(fileBlock.FileOffset, index - blockOffset);
                }

                FileDataBlock suffixBlock = null;
                if (index < blockOffset + fileBlock.Length - 1)
                {
                    suffixBlock = new FileDataBlock(
                        fileBlock.FileOffset + index - blockOffset + 1,
                        fileBlock.Length - (index - blockOffset + 1));
                }

				block = this._dataMap.Replace(block, new MemoryDataBlock(value));

                if (prefixBlock != null)
                {
                    this._dataMap.AddBefore(block, prefixBlock);
                }

                if (suffixBlock != null)
                {
                    this._dataMap.AddAfter(block, suffixBlock);
                }
            }
            finally
            {
                this.OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// See <see cref="IByteProvider.InsertBytes" /> for more information.
        /// </summary>
        public void InsertBytes(Int64 index, Byte[] bs)
        {
            try
            {
                // Find the block affected.
                Int64 blockOffset;
                DataBlock block = this.GetDataBlock(index, out blockOffset);

                // If the insertion point is in a memory block, just insert it.
                MemoryDataBlock memoryBlock = block as MemoryDataBlock;
                if (memoryBlock != null)
                {
                    memoryBlock.InsertBytes(index - blockOffset, bs);
                    return;
                }

                FileDataBlock fileBlock = (FileDataBlock)block;

                // If the insertion point is at the start of a file block, and the previous block is a memory block, append it to that block.
                if (blockOffset == index && block.PreviousBlock != null)
                {
                    MemoryDataBlock previousMemoryBlock = block.PreviousBlock as MemoryDataBlock;
                    if (previousMemoryBlock != null)
                    {
                        previousMemoryBlock.InsertBytes(previousMemoryBlock.Length, bs);
                        return;
                    }
                }

                // Split the block into a prefix and a suffix and place a memory block in-between.
                FileDataBlock prefixBlock = null;
                if (index > blockOffset)
                {
                    prefixBlock = new FileDataBlock(fileBlock.FileOffset, index - blockOffset);
                }

                FileDataBlock suffixBlock = null;
                if (index < blockOffset + fileBlock.Length)
                {
                    suffixBlock = new FileDataBlock(
                        fileBlock.FileOffset + index - blockOffset,
                        fileBlock.Length - (index - blockOffset));
                }

				block = this._dataMap.Replace(block, new MemoryDataBlock(bs));

                if (prefixBlock != null)
                {
                    this._dataMap.AddBefore(block, prefixBlock);
                }

                if (suffixBlock != null)
                {
                    this._dataMap.AddAfter(block, suffixBlock);
                }
            }
            finally
            {
                this._totalLength += bs.Length;
                this.OnLengthChanged(EventArgs.Empty);
                this.OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// See <see cref="IByteProvider.DeleteBytes" /> for more information.
        /// </summary>
        public void DeleteBytes(Int64 index, Int64 length)
        {
            try
            {
                Int64 bytesToDelete = length;

                // Find the first block affected.
                Int64 blockOffset;
                DataBlock block = this.GetDataBlock(index, out blockOffset);

                // Truncate or remove each block as necessary.
                while ((bytesToDelete > 0) && (block != null))
                {
                    Int64 blockLength = block.Length;
                    DataBlock nextBlock = block.NextBlock;

                    // Delete the appropriate section from the block (this may result in two blocks or a zero length block).
                    Int64 count = Math.Min(bytesToDelete, blockLength - (index - blockOffset));
                    block.RemoveBytes(index - blockOffset, count);

                    if (block.Length == 0)
                    {
                        this._dataMap.Remove(block);
                        if (this._dataMap.FirstBlock == null)
                        {
                            this._dataMap.AddFirst(new MemoryDataBlock(new Byte[0]));
                        }
                    }
                    
                    bytesToDelete -= count;
                    blockOffset += block.Length;
                    block = (bytesToDelete > 0) ? nextBlock : null;
                }
            }
            finally
            {
                this._totalLength -= length;
                this.OnLengthChanged(EventArgs.Empty);
                this.OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// See <see cref="IByteProvider.Length" /> for more information.
        /// </summary>
        public Int64 Length
        {
            get
            {
                return this._totalLength;
            }
        }

        /// <summary>
        /// See <see cref="IByteProvider.HasChanges" /> for more information.
        /// </summary>
        public Boolean HasChanges()
        {
            if (this._readOnly)
                return false;

            if (this._totalLength != this._stream.Length)
            {
                return true;
            }

            Int64 offset = 0;
            for (DataBlock block = this._dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                FileDataBlock fileBlock = block as FileDataBlock;
                if (fileBlock == null)
                {
                    return true;
                }

                if (fileBlock.FileOffset != offset)
                {
                    return true;
                }

                offset += fileBlock.Length;
            }
            return (offset != this._stream.Length);
        }

        /// <summary>
        /// See <see cref="IByteProvider.ApplyChanges" /> for more information.
        /// </summary>
        public void ApplyChanges()
        {
            if (this._readOnly)
                throw new OperationCanceledException("File is in read-only mode");

            // This method is implemented to efficiently save the changes to the same file stream opened for reading.
            // Saving to a separate file would be a much simpler implementation.

            // Firstly, extend the file length (if necessary) to ensure that there is enough disk space.
            if (this._totalLength > this._stream.Length)
            {
                this._stream.SetLength(this._totalLength);
            }

            // Secondly, shift around any file sections that have moved.
            Int64 dataOffset = 0;
            for (DataBlock block = this._dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                FileDataBlock fileBlock = block as FileDataBlock;
                if (fileBlock != null && fileBlock.FileOffset != dataOffset)
                {
                    this.MoveFileBlock(fileBlock, dataOffset);
                }
                dataOffset += block.Length;
            }

            // Next, write in-memory changes.
            dataOffset = 0;
            for (DataBlock block = this._dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                MemoryDataBlock memoryBlock = block as MemoryDataBlock;
                if (memoryBlock != null)
                {
                    this._stream.Position = dataOffset;
                    for (Int32 memoryOffset = 0; memoryOffset < memoryBlock.Length; memoryOffset += COPY_BLOCK_SIZE)
                    {
                        this._stream.Write(memoryBlock.Data, memoryOffset, (Int32)Math.Min(COPY_BLOCK_SIZE, memoryBlock.Length - memoryOffset));
                    }
                }
                dataOffset += block.Length;
            }

            // Finally, if the file has shortened, truncate the stream.
            this._stream.SetLength(this._totalLength);
            this.ReInitialize();
        }

        /// <summary>
        /// See <see cref="IByteProvider.SupportsWriteByte" /> for more information.
        /// </summary>
        public Boolean SupportsWriteByte()
        {
            return !this._readOnly;
        }

        /// <summary>
        /// See <see cref="IByteProvider.SupportsInsertBytes" /> for more information.
        /// </summary>
        public Boolean SupportsInsertBytes()
        {
            return !this._readOnly;
        }

        /// <summary>
        /// See <see cref="IByteProvider.SupportsDeleteBytes" /> for more information.
        /// </summary>
        public Boolean SupportsDeleteBytes()
        {
            return !this._readOnly;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// See <see cref="Object.Finalize" /> for more information.
        /// </summary>
        ~FileByteProvider()
        {
            this.Dispose();
        }

        /// <summary>
        /// See <see cref="IDisposable.Dispose" /> for more information.
        /// </summary>
        public void Dispose()
        {
            if (this._stream != null)
            {
                this._stream.Close();
                this._stream = null;
            }
            this._fileName = null;
            this._dataMap = null;
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Gets a value, if the file is opened in read-only mode.
        /// </summary>
        public Boolean ReadOnly
        {
            get { return this._readOnly; }
        }

        void OnLengthChanged(EventArgs e)
        {
            if (LengthChanged != null)
                LengthChanged(this, e);
        }

        void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        DataBlock GetDataBlock(Int64 findOffset, out Int64 blockOffset)
        {
            if (findOffset < 0 || findOffset > this._totalLength)
            {
                throw new ArgumentOutOfRangeException("findOffset");
            }

            // Iterate over the blocks until the block containing the required offset is encountered.
            blockOffset = 0;
            for (DataBlock block = this._dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                if ((blockOffset <= findOffset && blockOffset + block.Length > findOffset) || block.NextBlock == null)
                {
                    return block;
                }
                blockOffset += block.Length;
            }
            return null;
        }

        FileDataBlock GetNextFileDataBlock(DataBlock block, Int64 dataOffset, out Int64 nextDataOffset)
        {
            // Iterate over the remaining blocks until a file block is encountered.
            nextDataOffset = dataOffset + block.Length;
            block = block.NextBlock;
            while (block != null)
            {
                FileDataBlock fileBlock = block as FileDataBlock;
                if (fileBlock != null)
                {
                    return fileBlock;
                }
                nextDataOffset += block.Length;
                block = block.NextBlock;
            }
            return null;
        }

        Byte ReadByteFromFile(Int64 fileOffset)
        {
            // Move to the correct position and read the byte.
            if (this._stream.Position != fileOffset)
            {
                this._stream.Position = fileOffset;
            }
            return (Byte)this._stream.ReadByte();
        }

        void MoveFileBlock(FileDataBlock fileBlock, Int64 dataOffset)
        {
            // First, determine whether the next file block needs to move before this one.
            Int64 nextDataOffset;
			FileDataBlock nextFileBlock = this.GetNextFileDataBlock(fileBlock, dataOffset, out nextDataOffset);
            if (nextFileBlock != null && dataOffset + fileBlock.Length > nextFileBlock.FileOffset)
            {
                // The next block needs to move first, so do that now.
                this.MoveFileBlock(nextFileBlock, nextDataOffset);
            }

            // Now, move the block.
            if (fileBlock.FileOffset > dataOffset)
            {
                // Move the section to earlier in the file stream (done in chunks starting at the beginning of the section).
                Byte[] buffer = new Byte[COPY_BLOCK_SIZE];
                for (Int64 relativeOffset = 0; relativeOffset < fileBlock.Length; relativeOffset += buffer.Length)
                {
                    Int64 readOffset = fileBlock.FileOffset + relativeOffset;
                    Int32 bytesToRead = (Int32)Math.Min(buffer.Length, fileBlock.Length - relativeOffset);
                    this._stream.Position = readOffset;
                    this._stream.Read(buffer, 0, bytesToRead);

                    Int64 writeOffset = dataOffset + relativeOffset;
                    this._stream.Position = writeOffset;
                    this._stream.Write(buffer, 0, bytesToRead);
                }
            }
            else
            {
                // Move the section to later in the file stream (done in chunks starting at the end of the section).
                Byte[] buffer = new Byte[COPY_BLOCK_SIZE];
                for (Int64 relativeOffset = 0; relativeOffset < fileBlock.Length; relativeOffset += buffer.Length)
                {
                    Int32 bytesToRead = (Int32)Math.Min(buffer.Length, fileBlock.Length - relativeOffset);
                    Int64 readOffset = fileBlock.FileOffset + fileBlock.Length - relativeOffset - bytesToRead;
                    this._stream.Position = readOffset;
                    this._stream.Read(buffer, 0, bytesToRead);

                    Int64 writeOffset = dataOffset + fileBlock.Length - relativeOffset - bytesToRead;
                    this._stream.Position = writeOffset;
                    this._stream.Write(buffer, 0, bytesToRead);
                }
            }

			// This block now points to a different position in the file.
			fileBlock.SetFileOffset(dataOffset);
        }

        void ReInitialize()
        {
            this._dataMap = new DataMap();
            this._dataMap.AddFirst(new FileDataBlock(0, this._stream.Length));
            this._totalLength = this._stream.Length;
        }
    }
}
