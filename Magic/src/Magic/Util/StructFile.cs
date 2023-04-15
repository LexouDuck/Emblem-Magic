using GBA;
using System;
using System.Collections.Generic;
using System.IO;

namespace Magic
{
    /// <summary>
    /// This class allows data to be read from the ROM by getting string "fields" of structs defined in txt files
    /// </summary>
    public class StructFile
    {
        public dynamic this[String entryField]
        {
            get
            {
                StructField field = this.GetField(entryField);

                Byte[] data = Core.ReadData(this.Address + this.EntryIndex * this.EntryLength + field.Offset, field.Length);

                if (field.Type == typeof(Pointer))
                    return new Pointer((UInt32)Util.BytesToNumber(data, true), false, true);
                else if (field.Type == typeof(String))
                    return data.GetASCII(0, data.Length);
                else return Convert.ChangeType(Util.BytesToNumber(data, true), field.Type);
            }
        }



        /// <summary>
        /// The full path and name of the array file loaded
        /// </summary>
        public String FilePath { get; }
        /// <summary>
        /// The actual string entry names and their indices
        /// </summary>
        public StructField[] Fields { get; }
        /// <summary>
        /// The address at which the array of these structs is located.
        /// </summary>
        public Pointer Address { get; set; }
        /// <summary>
        /// The index of the current struct in the array
        /// </summary>
        public Int32 EntryIndex { get; set; }
        /// <summary>
        /// The length (in bytes) of the struct described by the struct txt file
        /// </summary>
        public Int32 EntryLength { get; }



        public StructFile(String fileName)
        {
            this.FilePath = Core.Path_Structs + fileName;
            String[] file;
            try
            {
                file = File.ReadAllLines(this.FilePath);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not open the struct file:\n" + this.FilePath, ex);
                return;
            }


            UInt32 field_offset;
            UInt32 field_length;
            Type field_type;
            String field_name;
            List<StructField> fields = new List<StructField>();
            String[] line;
            for (Int32 i = 0; i < file.Length; i++)
            {
                if (file[i] == "") continue;
                try
                {
                    line = file[i].Trim(' ', '\t').Split(' ', '\t');

                    if (line.Length != 4 && i != 0)
                        throw new Exception("Missing struct field parameters, found only" + line.Length);

                    if (line[0].StartsWith("0x"))
                        field_offset = Util.HexToInt(line[0]);
                    else field_offset = UInt32.Parse(line[0]);

                    if (line[1].StartsWith("0x"))
                        field_length = Util.HexToInt(line[1]);
                    else field_length = UInt32.Parse(line[1]);

                    if (i == 0)
                    {
                        this.Address = new Pointer(field_offset);
                        this.EntryLength = (Int32)field_length;
                    }
                    else
                    {
                        field_type = (line[2] == "Pointer") ? typeof(Pointer) : Type.GetType("System." + line[2], true);
                        
                        field_name = line[3];
                        for (Int32 j = 0; j < fields.Count; j++)
                        {
                            if (field_name == fields[j].Name) throw new Exception("Cannot have two fields of the same name.");
                        }

                        fields.Add(new StructField(field_name, field_type, field_offset, field_length));
                    }
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been an error while reading the struct file:\n" + this.FilePath + "\nAt line: " + i, ex);
                }
            }
            this.Fields = fields.ToArray();
        }



        /// <summary>
        /// Returns the field of the struct with the matching field name
        /// </summary>
        public StructField GetField(String name)
        {
            for (Int32 i = 0; i < this.Fields.Length; i++)
            {
                if (this.Fields[i].Name == name)
                {
                    return this.Fields[i];
                }
            }
            throw new Exception("Couldn't find struct field named: " + name);
        }

        /// <summary>
        /// Returns the address for the given field according to the given struct index in the array
        /// </summary>
        public Pointer GetAddress(Int32 entryIndex, String entryField = "")
        {
            Pointer address = this.Address + entryIndex * this.EntryLength;

            if (entryField == "") return address;
            else
            {
                StructField field = this.GetField(entryField);
                return address + field.Offset;
            }
        }
        /// <summary>
        /// Returns true if the struct entry at the given index is only composed of 0x00 bytes
        /// </summary>
        public Boolean EntryIsNull(Int32 entryIndex)
        {
            Byte[] entry = Core.ReadData(this.Address + entryIndex * this.EntryLength, this.EntryLength);
            for (Int32 i = 0; i < entry.Length; i++)
            {
                if (entry[i] != 0x00)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Reads the pointer at the beginning of a struct file and returns it
        /// </summary>
        public static Pointer GetAddress(String fileName)
        {
            try
            {
                String line;
                using (StreamReader reader = new StreamReader(Core.Path_Structs + fileName))
                {
                    line = reader.ReadLine() ?? "";
                }
                if (line == "") throw new Exception("First line is empty.");

                line = line.TrimStart(' ');
                Int32 length = 1;
                while (line[length] != ' ') length++;
                return new Pointer(Util.HexToInt(line.Substring(0, length)), false, false);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not read pointer from struct file:\n" + fileName, ex);
                return new Pointer();
            }
        }
    }



    public class StructField
    {
        /// <summary>
        /// The name of this struct field
        /// </summary>
        public String Name;
        /// <summary>
        /// The type of value for this struct field
        /// </summary>
        public Type Type;
        /// <summary>
        /// The offset (in bytes) of this struct field within the struct
        /// </summary>
        public Int32 Offset;
        /// <summary>
        /// The length (in bytes) of this struct field within the struct
        /// </summary>
        public Int32 Length;

        public StructField(String name, Type type, UInt32 offset, UInt32 length)
        {
            this.Name = name;
            this.Type = type;
            this.Offset = (Int32)offset;
            this.Length = (Int32)length;
        }
    }
}
