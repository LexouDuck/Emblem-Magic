using System;
using System.Collections.Generic;

namespace Magic.Components
{
	/// <summary>
	/// Byte provider for a small amount of data.
	/// </summary>
	public class DataByteProvider : IByteProvider
	{
        /// <summary>
        /// Contains information about changes.
        /// </summary>
        Boolean _hasChanges;
		/// <summary>
		/// Contains a byte collection.
		/// </summary>
		List<Byte> _bytes;

		/// <summary>
		/// Initializes a new instance of the DataByteProvider class.
		/// </summary>
		/// <param name="data"></param>
		public DataByteProvider(Byte[] data) : this(new List<Byte>(data)) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the DataByteProvider class.
		/// </summary>
		/// <param name="bytes"></param>
		public DataByteProvider(List<Byte> bytes)
		{
			_bytes = bytes;
		}

		/// <summary>
		/// Raises the Changed event.
		/// </summary>
		void OnChanged(EventArgs e)
		{
			_hasChanges = true;

			if(Changed != null)
				Changed(this, e);
		}

		/// <summary>
		/// Raises the LengthChanged event.
		/// </summary>
		void OnLengthChanged(EventArgs e)
		{
			if(LengthChanged != null)
				LengthChanged(this, e);
		}

		/// <summary>
		/// Gets the byte collection.
		/// </summary>
		public List<Byte> Bytes
		{
			get { return _bytes; }
		}

		#region IByteProvider Members
		/// <summary>
		/// True, when changes are done.
		/// </summary>
		public Boolean HasChanges()
		{
			return _hasChanges;
		}

		/// <summary>
		/// Applies changes.
		/// </summary>
		public void ApplyChanges()
		{
			_hasChanges = false;
		}

		/// <summary>
		/// Occurs, when the write buffer contains new changes.
		/// </summary>
		public event EventHandler Changed;

		/// <summary>
		/// Occurs, when InsertBytes or DeleteBytes method is called.
		/// </summary>
		public event EventHandler LengthChanged;


		/// <summary>
		/// Reads a byte from the byte collection.
		/// </summary>
		/// <param name="index">the index of the byte to read</param>
		/// <returns>the byte</returns>
		public Byte ReadByte(Int64 index)
		{ return _bytes[(Int32)index]; }

		/// <summary>
		/// Write a byte into the byte collection.
		/// </summary>
		/// <param name="index">the index of the byte to write.</param>
		/// <param name="value">the byte</param>
		public void WriteByte(Int64 index, Byte value)
		{
			_bytes[(Int32)index] = value;
			OnChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Deletes bytes from the byte collection.
		/// </summary>
		/// <param name="index">the start index of the bytes to delete.</param>
		/// <param name="length">the length of bytes to delete.</param>
		public void DeleteBytes(Int64 index, Int64 length)
		{
            Int32 internal_index = (Int32)Math.Max(0, index);
            Int32 internal_length = (Int32)Math.Min((Int32)Length, length);
			_bytes.RemoveRange(internal_index, internal_length); 

			OnLengthChanged(EventArgs.Empty);
			OnChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Inserts byte into the byte collection.
		/// </summary>
		/// <param name="index">the start index of the bytes in the byte collection</param>
		/// <param name="bs">the byte array to insert</param>
		public void InsertBytes(Int64 index, Byte[] bs)
		{ 
			_bytes.InsertRange((Int32)index, bs); 

			OnLengthChanged(EventArgs.Empty);
			OnChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Gets the length of the bytes in the byte collection.
		/// </summary>
		public Int64 Length
		{
			get
			{
				return _bytes.Count;
			}
		}

		/// <summary>
		/// Returns true
		/// </summary>
		public Boolean SupportsWriteByte()
		{
			return true;
		}

		/// <summary>
		/// Returns true
		/// </summary>
		public Boolean SupportsInsertBytes()
		{
			return true;
		}

		/// <summary>
		/// Returns true
		/// </summary>
		public Boolean SupportsDeleteBytes()
		{
			return true;
		}
		#endregion

    }
}
