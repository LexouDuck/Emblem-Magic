using GBA;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmblemMagic
{
    /// <summary>
    /// This class allows data to be read from the ROM by getting string "fields" of structs defined in txt files
    /// </summary>
    public class StructFile
    {
        public dynamic this[string entryField]
        {
            get
            {
                StructField field = GetField(entryField);

                byte[] data = Core.ReadData(Address + EntryIndex * EntryLength + field.Offset, field.Length);

                if (field.Type == typeof(Pointer))
                    return new Pointer((uint)Util.BytesToNumber(data, true), false, true);
                else if (field.Type == typeof(string))
                    return data.GetASCII(0, data.Length);
                else return Convert.ChangeType(Util.BytesToNumber(data, true), field.Type);
            }
        }



        /// <summary>
        /// The full path and name of the array file loaded
        /// </summary>
        public string FilePath { get; }
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
        public int EntryIndex { get; set; }
        /// <summary>
        /// The length (in bytes) of the struct described by the struct txt file
        /// </summary>
        public int EntryLength { get; }



        public StructFile(string fileName)
        {
            FilePath = Core.Path_Structs + fileName;
            string[] file;
            try
            {
                file = File.ReadAllLines(FilePath);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not open the struct file:\n" + FilePath, ex);
                return;
            }


            uint field_offset;
            uint field_length;
            Type field_type;
            string field_name;
            List<StructField> fields = new List<StructField>();
            string[] line;
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] == "") continue;
                try
                {
                    line = file[i].Trim(' ', '\t').Split(' ', '\t');

                    if (line.Length != 4 && i != 0)
                        throw new Exception("Missing struct field parameters, found only" + line.Length);

                    if (line[0].StartsWith("0x"))
                        field_offset = Util.HexToInt(line[0]);
                    else field_offset = uint.Parse(line[0]);

                    if (line[1].StartsWith("0x"))
                        field_length = Util.HexToInt(line[1]);
                    else field_length = uint.Parse(line[1]);

                    if (i == 0)
                    {
                        Address = new Pointer(field_offset);
                        EntryLength = (int)field_length;
                    }
                    else
                    {
                        field_type = (line[2] == "Pointer") ? typeof(Pointer) : Type.GetType("System." + line[2], true);
                        
                        field_name = line[3];
                        for (int j = 0; j < fields.Count; j++)
                        {
                            if (field_name == fields[j].Name) throw new Exception("Cannot have two fields of the same name.");
                        }

                        fields.Add(new StructField(field_name, field_type, field_offset, field_length));
                    }
                }
                catch (Exception ex)
                {
                    Program.ShowError("There has been an error while reading the struct file:\n" + FilePath + "\nAt line: " + i, ex);
                }
            }
            Fields = fields.ToArray();
        }



        /// <summary>
        /// Returns the field of the struct with the matching field name
        /// </summary>
        public StructField GetField(string name)
        {
            for (int i = 0; i < Fields.Length; i++)
            {
                if (Fields[i].Name == name)
                {
                    return Fields[i];
                }
            }
            throw new Exception("Couldn't find struct field named: " + name);
        }

        /// <summary>
        /// Returns the address for the given field according to the given struct index in the array
        /// </summary>
        public Pointer GetAddress(int entryIndex, string entryField = "")
        {
            Pointer address = Address + entryIndex * EntryLength;

            if (entryField == "") return address;
            else
            {
                StructField field = GetField(entryField);
                return address + field.Offset;
            }
        }
        /// <summary>
        /// Returns true if the struct entry at the given index is only composed of 0x00 bytes
        /// </summary>
        public bool EntryIsNull(int entryIndex)
        {
            byte[] entry = Core.ReadData(Address + entryIndex * EntryLength, EntryLength);
            for (int i = 0; i < entry.Length; i++)
            {
                if (entry[i] != 0x00)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Reads the pointer at the beginning of a struct file and returns it
        /// </summary>
        public static Pointer GetAddress(string fileName)
        {
            try
            {
                string line;
                using (StreamReader reader = new StreamReader(Core.Path_Structs + fileName))
                {
                    line = reader.ReadLine() ?? "";
                }
                if (line == "") throw new Exception("First line is empty.");

                line = line.TrimStart(' ');
                int length = 1;
                while (line[length] != ' ') length++;
                return new Pointer(Util.HexToInt(line.Substring(0, length)), false, false);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not read pointer from struct file:\n" + fileName, ex);
                return new Pointer();
            }
        }
    }



    public class StructField
    {
        /// <summary>
        /// The name of this struct field
        /// </summary>
        public string Name;
        /// <summary>
        /// The type of value for this struct field
        /// </summary>
        public Type Type;
        /// <summary>
        /// The offset (in bytes) of this struct field within the struct
        /// </summary>
        public int Offset;
        /// <summary>
        /// The length (in bytes) of this struct field within the struct
        /// </summary>
        public int Length;

        public StructField(string name, Type type, uint offset, uint length)
        {
            Name = name;
            Type = type;
            Offset = (int)offset;
            Length = (int)length;
        }
    }
}
