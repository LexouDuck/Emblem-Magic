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
        public UInt32[] EntryLength;

        /// <summary>
        /// The data, in a jagged array so as to separate the different table entries.
        /// </summary>
        public Byte[][] Entries;



        public HackData(String identifier, Byte[][] data)
        {
            this.TableString = identifier;
            this.EntryAmount = (UInt32)data.Length;
            this.EntryLength = new UInt32[this.EntryAmount];
            UInt32 total = 12;
            for (Int32 i = 0; i < this.EntryAmount; i++)
            {
                this.EntryLength[i] = (UInt32)data[i].Length;
                total += 4 + this.EntryLength[i];
            }
            this.TableLength = total;
            this.Entries = data;
        }

        public HackData(Byte[] data)
        {
            UInt32 parse = 0;
            this.TableString = data.GetASCII(parse, 4); parse += 4;
            this.TableLength = data.GetUInt32(parse, false); parse += 4;
            this.EntryAmount = data.GetUInt32(parse, false); parse += 4;
            if (this.TableLength != data.Length)
                throw new Exception("The constructor data given for this HackData (" + this.TableString + ") shows an invalid length.");
            this.EntryLength = new UInt32[this.EntryAmount];
            for (Int32 i = 0; i < this.EntryAmount; i++)
            {
                this.EntryLength[i] = data.GetUInt32(parse, false);
                parse += 4;
            }
            this.Entries = new Byte[this.EntryAmount][];
            for (Int32 i = 0; i < this.EntryAmount; i++)
            {
                this.Entries[i] = new Byte[this.EntryLength[i]];
                Array.Copy(data, parse, this.Entries[i], 0, this.EntryLength[i]);
                parse += this.EntryLength[i];
            }
        }



        /// <summary>
        /// Returns this hackdata table as a byte array.
        /// </summary>
        public Byte[] ToBytes()
        {
            Byte[] result = new Byte[this.TableLength];
            Byte[] buffer = new Byte[4];
            Int32 parse = 0;
            for (Int32 i = 0; i < 3 + this.EntryAmount; i++)
            {
                switch (i)
                {
                    case 0: buffer = ByteArray.Make_ASCII(this.TableString); break;
                    case 1: buffer = ByteArray.Make_Int32(this.TableLength); break;
                    case 2: buffer = ByteArray.Make_Int32(this.EntryAmount); break;
                    default:buffer = ByteArray.Make_Int32(this.EntryLength[i - 3]); break;
                }
                Array.Copy(buffer, 0, result, parse, 4);
                parse += 4;
            }
            for (Int32 i = 0; i < this.Entries.Length; i++)
            {
                buffer = this.Entries[i];
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
            UInt32 length = 0;
            foreach (HackData table in tables)
            {
                length += table.TableLength;
            }
            Byte[] result = new Byte[length];
            UInt32 parse = 0;

            foreach (HackData table in tables)
            {
                Array.Copy(table.ToBytes(), 0, result, parse, table.TableLength); parse += table.TableLength;
            }

            return result;
        }
    }
}