using System;

namespace Magic
{
    public struct HackData
    {
        /// <summary>
        /// The 4-byte ID for what metadata we're dealing with
        /// </summary>
        public String TableString;
        /// <summary>
        /// How many bytes long is this table
        /// </summary>
        public UInt32 TableLength;

        /// <summary>
        /// How many entries there are in this metadata table
        /// </summary>
        public UInt32 EntryAmount;
        /// <summary>
        /// An array containing the length of each entry.
        /// </summary>
        public uint[] EntryLength;

        /// <summary>
        /// The data, in a jagged array so as to separate the different table entries.
        /// </summary>
        public Byte[][] Entries;



        public HackData(string identifier, byte[][] data)
        {
            TableString = identifier;
            EntryAmount = (uint)data.Length;
            EntryLength = new uint[EntryAmount];
            uint total = 12;
            for (int i = 0; i < EntryAmount; i++)
            {
                EntryLength[i] = (uint)data[i].Length;
                total += 4 + EntryLength[i];
            }
            TableLength = total;
            Entries = data;
        }

        public HackData(byte[] data)
        {
            uint parse = 0;
            TableString = data.GetASCII(parse, 4); parse += 4;
            TableLength = data.GetUInt32(parse, false); parse += 4;
            EntryAmount = data.GetUInt32(parse, false); parse += 4;
            if (TableLength != data.Length)
                throw new Exception("The constructor data given for this HackData (" + TableString + ") shows an invalid length.");
            EntryLength = new uint[EntryAmount];
            for (int i = 0; i < EntryAmount; i++)
            {
                EntryLength[i] = data.GetUInt32(parse, false);
                parse += 4;
            }
            Entries = new Byte[EntryAmount][];
            for (int i = 0; i < EntryAmount; i++)
            {
                Entries[i] = new byte[EntryLength[i]];
                Array.Copy(data, parse, Entries[i], 0, EntryLength[i]);
                parse += EntryLength[i];
            }
        }



        /// <summary>
        /// Returns this hackdata table as a byte array.
        /// </summary>
        public Byte[] ToBytes()
        {
            Byte[] result = new Byte[TableLength];
            Byte[] buffer = new Byte[4];
            int parse = 0;
            for (int i = 0; i < 3 + EntryAmount; i++)
            {
                switch (i)
                {
                    case 0: buffer = ByteArray.Make_ASCII(TableString); break;
                    case 1: buffer = ByteArray.Make_Int32(TableLength); break;
                    case 2: buffer = ByteArray.Make_Int32(EntryAmount); break;
                    default:buffer = ByteArray.Make_Int32(EntryLength[i - 3]); break;
                }
                Array.Copy(buffer, 0, result, parse, 4);
                parse += 4;
            }
            for (int i = 0; i < Entries.Length; i++)
            {
                buffer = Entries[i];
                Array.Copy(buffer, 0, result, parse, buffer.Length);
                parse += buffer.Length;
            }

            return result;
        }



        /// <summary>
        /// Combines several hackdata tables into one big one, where each hackdata table is an entry.
        /// </summary>
        public static Byte[] Combine(HackData[] tables)
        {
            uint length = 0;
            foreach (HackData table in tables)
            {
                length += table.TableLength;
            }
            byte[] result = new byte[length];
            uint parse = 0;

            foreach (HackData table in tables)
            {
                Array.Copy(table.ToBytes(), 0, result, parse, table.TableLength); parse += table.TableLength;
            }

            return result;
        }
    }
}