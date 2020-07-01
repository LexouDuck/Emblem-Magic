using GBA;
using System;
using System.Collections.Generic;
using System.IO;
using EA = Nintenlord.Event_Assembler.Core;

namespace EmblemMagic.Editors
{
    public class EA_Log : EA.IO.Logs.MessageLog
    {
        public override void PrintAll()
        {
            if (ErrorCount + MessageCount + WarningCount > 0)
                Prompt.ShowResult("Message logs:", "Event Assembler", this.GetText());
        }
    }

    public static class EventAssemblerIO
    {
        public static string ReplaceSpacesAndSpecialChars(string EMidentifier)
        {
            return EMidentifier.Replace('(', '_')
                               .Replace(")", "")
                               .Replace(") ", "")
                               .Replace(" ", "");
        }
    }

    public class ROM_Stream : Nintenlord.IO.ChangeStream
    {
        Editor owner;
        int position;
        Nintenlord.Collections.IDataChange<byte> changes;

        public ROM_Stream(Editor parent) : base()
        {
            owner = parent;
            changes = new Nintenlord.Collections.DataChange<byte>();
        }

        public string Description;

        public override bool CanRead { get { return false; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }
        public override long Length { get { return Core.CurrentROMSize; } }
        public override long Position
        {
            get { return position; }
            set { position = (int)value; }
        }

        public override void Flush() { }
        public override void SetLength(long value) { }
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new Exception("EA ROM_Stream: Reading not supported.");
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = (int)offset;
                    break;
                case SeekOrigin.Current:
                    position += (int)offset;
                    break;
                case SeekOrigin.End:
                    position = (int)Core.CurrentROMSize + (int)offset;
                    break;
            }
            return position;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                changes.AddChangedData(position, buffer, offset, count);
            }
            catch { }
            position += count;
        }

        public void WriteToROM(bool autoFreeSpace)
        {
            Pointer address = new Pointer();
            List<byte[]> writes = new List<byte[]>();
            int length = 0;
            foreach (var item in (IEnumerable<KeyValuePair<int, byte[]>>)changes)
            {
                if (length == 0)
                {
                    address = new Pointer((uint)item.Key);
                    writes.Add(item.Value);
                    length = item.Value.Length;
                }
                /*
                System.Windows.Forms.MessageBox.Show(
                    "address: " + address + ", length: " + length + "\r\n" +
                    "total: " + new Pointer(address + length) +
                    ", item.Key: " + new Pointer((uint)item.Key));
                */
                if (address + length == item.Key)
                {
                    writes.Add(item.Value);
                    length += item.Value.Length;
                }
                else
                {
                    WriteToROM_Concatenate(autoFreeSpace, address, writes, length);
                    writes.Clear();
                    address = new Pointer((uint)item.Key);
                    writes.Add(item.Value);
                    length = item.Value.Length;
                }
            }
            WriteToROM_Concatenate(autoFreeSpace, address, writes, length);
        }
        void WriteToROM_Concatenate(bool autoFreeSpace, Pointer address, List<byte[]> writes, int length)
        {
            byte[] data = new byte[length];
            length = 0;
            for (int index = 0; index < writes.Count; index++)
            {
                for (int i = 0; i < writes[index].Length; i++)
                {
                    data[length + i] = writes[index][i];
                }
                length += writes[index].Length;
            }

            string write_description = "???";
            if (length == 4 && data[0] == 0x08) write_description = "Repoint";
            else if (writes.Count > 1) write_description = "Compiled Events";

            if (autoFreeSpace)
            {
                address = Core.GetFreeSpace(data.Length);
                if (address % 4 != 0) address += (4 - (address % 4));
            }
            Core.WriteData(owner,
                address,
                data,
                Description + " - " + write_description);
        }
    }
}
