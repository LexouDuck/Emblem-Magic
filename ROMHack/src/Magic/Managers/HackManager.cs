using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Magic
{
    /// <summary>
    /// Manages everything relating to the Fire Emblem hack file (.FEH), opening and saving files.
    /// </summary>
    public class HackManager
    {
        public const Int32 TableTotal = 5;
        public const Int32 TableLength = 12 + TableTotal * 4;

        public const int LENGTH_Write_Author = 16;
        public const int LENGTH_Write_TimeOf = 8;
        public const int LENGTH_Write_Phrase = 64;
        public const int LENGTH_Write_Offset = 4;
        public static int LENGTH_Write
        {
            get
            {
                return LENGTH_Write_Author
                     + LENGTH_Write_TimeOf
                     + LENGTH_Write_Phrase
                     + LENGTH_Write_Offset;
            }
        }

        public const int LENGTH_Point_Name = 32;

        private Object Locked = new Object();

        /// <summary>
        /// Returns 'true' if this HackManager has no file path set to it
        /// </summary>
        public Boolean IsEmpty { get { return (FilePath.Length == 0); } }
        /// <summary>
        /// Returns 'true' if every write of this FEH HackManager is applied to the current ROM
        /// </summary>
        public Boolean IsApplied()
        {
            foreach (Write write in Write.History)
            {
                for (int i = 0; i < write.Data.Length; i++)
                {
                    if (Core.ReadByte(write.Address + i) != write.Data[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        public String FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }
        public String FilePath { get; private set; }
        public Byte[] FileData { get; private set; }
        public UInt32 FileSize { get; private set; }
        public Boolean Changed { get; set; }

        public String HackName;
        public String HackAuthor;
        public String HackDescription;

        public WriteManager Write;
        public PointManager Point;
        public MarksManager Marks;
        public SpaceManager Space;



        public IApp App;
        public HackManager(IApp app)
        {
            App = app;

            FilePath = "";
            FileSize = TableLength;
            FileData = new Byte[TableLength];
            HackName = "";
            HackAuthor = "";
            HackDescription = "";
            Write = new WriteManager(App);
            Point = new PointManager(App);
            Marks = new MarksManager(App);
            Space = new SpaceManager(App);
        }

        /// <summary>
        /// Opens a file from the system and writes its contents onto 'FileData', then calls LoadData()
        /// </summary>
        public void OpenFile(string path)
        {
            if (path == null || path.Length == 0)
                throw new Exception("The module file path given is invalid.");
            if (!File.Exists(path))
                throw new Exception("The module file was not found: " + path);

            lock (Locked)
            {
                FilePath = path;
                FileData = File.ReadAllBytes(FilePath);
                FileSize = (UInt32)FileData.Length;

                LoadData();

                this.Changed = false;
            }
        }
        /// <summary>
        /// Makes the byte array of HackData, then overwrites or creates an FEH file at the given path.
        /// </summary>
        public void SaveFile(string path)
        {
            lock (Locked)
            {
                FilePath = path;
                FileData = MakeData();
                FileSize = (UInt32)FileData.Length;

                File.WriteAllBytes(FilePath, FileData);

                this.Changed = false;
            }
        }



        /// <summary>
        /// Loads the hackdata stored in 'FileData' for use in the program
        /// </summary>
        public void LoadData()
        {
            uint parse = 0;
            HackData table = new HackData(FileData);
            parse += TableLength;
            // the first 2 bytes are always "FE" in ascii byte encoding - they serve as an indicator that the file is valid
            // byte 3 is the associated ROM: 6,7,8
            // byte 4 is the version of the associated ROM - "E" for PAL, "U" for NTSC, "J" for japanese
            App.Core_CheckROMIdentifier(table.TableString);

            uint length = 0;
            byte[] buffer;
            HackData subtable;
            List<Write>   write = new List<Write>();
            List<Repoint> point = new List<Repoint>();
            List<Mark>    marks = new List<Mark>();
            List<Space>   space = new List<Space>();
            for (int i = 0; i < TableTotal; i++)
            {
                length = table.EntryLength[i];
                buffer = new byte[length];
                Array.Copy(FileData, parse, buffer, 0, length);

                subtable = new HackData(buffer);
                switch (subtable.TableString)
                {
                    case "_FEH": LoadData_Properties(subtable); break;
                    case "_WRT": LoadData_Write(subtable); break;
                    case "_PNT": LoadData_Point(subtable); break;
                    case "_MRK": LoadData_Marks(subtable); break;
                    case "_SPC": LoadData_Space(subtable); break;
                }
                parse += length;
            }
        }
        void LoadData_Properties(HackData table)
        {
            HackName        = table.Entries[0].GetASCII(0, (int)table.EntryLength[0]);
            HackAuthor      = table.Entries[1].GetASCII(0, (int)table.EntryLength[1]);
            HackDescription = table.Entries[2].GetASCII(0, (int)table.EntryLength[2]);
        }
        void LoadData_Write(HackData table)
        {
            List<Write> result = new List<Write>();
            const uint index_Author = 0;
            const uint index_TimeOf = LENGTH_Write_Author;
            const uint index_Phrase = index_TimeOf + LENGTH_Write_TimeOf;
            const uint index_Offset = index_Phrase + LENGTH_Write_Phrase;
            const uint index_Data = index_Offset + LENGTH_Write_Offset;
            for (int i = 0; i < table.EntryAmount; i++)
            {
                result.Add(new Write(
                    table.Entries[i].GetASCII(index_Author, LENGTH_Write_Author).TrimEnd(' '),   // author of write
                    table.Entries[i].GetUInt64(index_TimeOf, false),                             // time of write
                    table.Entries[i].GetASCII(index_Phrase, LENGTH_Write_Phrase).TrimEnd(' '), // description
                    table.Entries[i].GetUInt32(index_Offset, false),                           // offset of write
                    table.Entries[i].GetBytes(index_Data, (int)table.EntryLength[i] - LENGTH_Write))); // data to write
            }
            Write.History = result;
        }
        void LoadData_Point(HackData table)
        {
            List<Repoint> result = new List<Repoint>();
            const uint index_Name = 0;
            const uint index_Default = LENGTH_Point_Name;
            const uint index_Current = index_Default + 4;
            for (int i = 0; i < table.EntryAmount; i++)
            {
                result.Add(new Repoint(
                    table.Entries[i].GetASCII(index_Name, LENGTH_Point_Name).TrimEnd(' '), // asset name
                    table.Entries[i].GetUInt32(index_Default, false),        // asset default address
                    table.Entries[i].GetUInt32(index_Current, false)));     // asset current address
            }
            Point.Repoints = result;
        }
        void LoadData_Marks(HackData table)
        {
            List<Mark> result = new List<Mark>();
            for (int i = 0; i < table.EntryAmount; i++)
            {
                result.Add(new Mark(
                    table.Entries[i].GetASCII(0, 4),        // marking type identifier
                    table.Entries[i].GetUInt32(4, false),   // marking type layer
                    table.Entries[i].GetUInt32(8, false))); // marking type color
            }
            Marks.MarkingTypes = result;
        }
        void LoadData_Space(HackData table)
        {
            List<Space> result = new List<Space>();
            for (int i = 0; i < table.EntryAmount; i++)
            {
                result.Add(new Space(Marks.Get(
                   table.Entries[i].GetASCII(0, 4)),       // marking type identifier
                   table.Entries[i].GetUInt32(4, false),   // offset of range start
                   table.Entries[i].GetUInt32(8, false))); // offset of range end
            }
            Space.MarkedRanges = result;
        }


        /// <summary>
        /// Returns a byte array in FEH format of the current hack
        /// </summary>
        public byte[] MakeData()
        {
            HackData tableProperties = new HackData("_FEH", new byte[3][]
            {
                ByteArray.Make_ASCII(HackName),
                ByteArray.Make_ASCII(HackAuthor),
                ByteArray.Make_ASCII(HackDescription),
            });
            HackData tableWrite = MakeData_Write();
            HackData tablePoint = MakeData_Point();
            HackData tableMarks = MakeData_Marks();
            HackData tableSpace = MakeData_Space();

            HackData table;
            byte[][] table_data = new byte[TableTotal][];
            table_data[0] = tableProperties.ToBytes();
            table_data[1] = tableWrite.ToBytes();
            table_data[2] = tablePoint.ToBytes();
            table_data[3] = tableMarks.ToBytes();
            table_data[4] = tableSpace.ToBytes();
            string game_identifier = Core.CurrentROM.GetIdentifier();
            table = new HackData(game_identifier, table_data);

            return table.ToBytes();
        }
        HackData MakeData_Write()
        {
            byte[][] writes = new byte[Write.History.Count][];

            byte[] entry;
            int entry_length;

            byte[] write_author;    // 16 for editor used,
            byte[] write_timeof;   // 8 for time (year 2038..?)
            byte[] write_phrase;  // 64 for description phrase
            byte[] write_offset; // 4 for offset of write
            byte[] write_data;  // and then a variable length of data written
            int index = 0;
            for (int i = 0; i < writes.Length; i++)
            {
                entry_length = (int)LENGTH_Write + Write.History[i].Data.Length;
                entry = new byte[entry_length];

                write_author = ByteArray.Make_ASCII(Write.History[i].GetEditorString());
                write_timeof = ByteArray.Make_Int64((UInt64)Write.History[i].Time.ToBinary());
                write_phrase = ByteArray.Make_ASCII(Write.History[i].GetPhraseString());
                write_offset = ByteArray.Make_Int32(Write.History[i].Address);
                write_data = Write.History[i].Data;
                
                Array.Copy(write_author, 0, entry, index, LENGTH_Write_Author); index += LENGTH_Write_Author;
                Array.Copy(write_timeof, 0, entry, index, LENGTH_Write_TimeOf); index += LENGTH_Write_TimeOf;
                Array.Copy(write_phrase, 0, entry, index, LENGTH_Write_Phrase); index += LENGTH_Write_Phrase;
                Array.Copy(write_offset, 0, entry, index, LENGTH_Write_Offset); index += LENGTH_Write_Offset;
                Array.Copy(write_data, 0, entry, index, write_data.Length); index = 0;
                
                writes[i] = entry;
            }

            return new HackData("_WRT", writes);
        }
        HackData MakeData_Point()
        {
            byte[][] repoints = new byte[Point.Repoints.Count][];
            byte[] entry;

            int entry_length = 40;
            byte[] asset_name;    // 32 bytes for asset name
            byte[] asset_default; // 4 bytes for default pointer for this asset
            byte[] asset_current; // 4 bytes for the repointed address for this asset

            for (int i = 0; i < repoints.Length; i++)
            {
                entry = new byte[entry_length];

                asset_name = ByteArray.Make_ASCII(Point.Repoints[i].AssetName.PadRight(LENGTH_Point_Name));
                asset_default = ByteArray.Make_Int32(Point.Repoints[i].DefaultAddress);
                asset_current = ByteArray.Make_Int32(Point.Repoints[i].CurrentAddress);

                Array.Copy(asset_name, 0, entry, 0, LENGTH_Point_Name);
                Array.Copy(asset_default, 0, entry, LENGTH_Point_Name, 4);
                Array.Copy(asset_current, 0, entry, LENGTH_Point_Name + 4, 4);

                repoints[i] = entry;
            }

            return new HackData("_PNT", repoints);
        }
        HackData MakeData_Marks()
        {
            byte[][] marks = new byte[Marks.MarkingTypes.Count][];
            byte[] entry;

            int entry_length = 12;
            byte[] mark_name;  // 4 bytes for marking name (FREE/USED/etc)
            byte[] mark_layer; // 4 bytes for marking type layer number
            byte[] mark_color; // 4 bytes for marking color

            for (int i = 0; i < marks.Length; i++)
            {
                entry = new byte[entry_length];

                mark_name  = ByteArray.Make_ASCII(Marks.MarkingTypes[i].Name);
                mark_layer = ByteArray.Make_Int32((UInt32)Marks.MarkingTypes[i].Layer);
                mark_color = ByteArray.Make_Int32((UInt32)Marks.MarkingTypes[i].Color.ToArgb());
                
                Array.Copy(mark_name,  0, entry, 0, 4);
                Array.Copy(mark_layer, 0, entry, 4, 4);
                Array.Copy(mark_color, 0, entry, 8, 4);

                marks[i] = entry;
            }

            return new HackData("_MRK", marks);
        }
        HackData MakeData_Space()
        {
            byte[][] spaces = new byte[Space.MarkedRanges.Count][];

            byte[] entry;
            int entry_length = 16;

            byte[] space_marked;  // 4 bytes for marking name (FREE/USED/etc)
            byte[] space_address; // 4 bytes for start offset
            byte[] space_endbyte; // 4 bytes for end offset

            for (int i = 0; i < spaces.Length; i++)
            {
                entry = new byte[entry_length];

                space_marked  = ByteArray.Make_ASCII(Space.MarkedRanges[i].Marked.Name);
                space_address = ByteArray.Make_Int32(Space.MarkedRanges[i].Address);
                space_endbyte = ByteArray.Make_Int32(Space.MarkedRanges[i].EndByte);
                
                Array.Copy(space_marked,  0, entry, 0, 4);
                Array.Copy(space_address, 0, entry, 4, 4);
                Array.Copy(space_endbyte, 0, entry, 8, 4);
                
                spaces[i] = entry;
            }

            return new HackData("_SPC", spaces);
        }
    }
}
