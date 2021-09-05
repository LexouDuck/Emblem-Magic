using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Magic
{
    /// <summary>
    /// This class is used by the program to easily read entry names from text array files
    /// </summary>
    public class ArrayFile : IEnumerable, IEnumerator
    {
        public string this[uint index]
        {
            get
            {
                string entry = "";
                if (Entries.TryGetValue(index, out entry))
                {
                    return entry;
                }
                return "";
            }
        }
        
        /// <summary>
        /// The index of the last entry in the list 
        /// </summary>
        public uint LastEntry { get; private set; }
        /// <summary>
        /// The full path and name of the array file loaded
        /// </summary>
        string FilePath { get; }
        /// <summary>
        /// The actual string entry names and their indices
        /// </summary>
        Dictionary<uint, string> Entries { get; }
        
        public ArrayFile(string fileName)
        {
            FilePath = Core.Path_Arrays + fileName;
            Entries = new Dictionary<uint, string>();

            string[] file;
            try
            {
                file = File.ReadAllLines(FilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not open the array file:\n" + FilePath, ex);
            }
            Load(file);
        }
        public ArrayFile(string[] file)
        {
            FilePath = null;
            Entries = new Dictionary<uint, string>();
            
            Load(file);
        }
        void Load(string[] file)
        {
            uint max = 0;
            uint index;
            int i;
            for (int line = 0; line < file.Length; line++)
            {
                try
                {
                    i = 0;
                    while (i + 1 < file[line].Length && file[line][i] != ' ') i++;

                    if (file[line].StartsWith("0x"))
                         index = Util.HexToInt(file[line].Substring(0, i));
                    else index = uint.Parse(file[line].Substring(0, i));

                    if (index > max) max = index;

                    Entries.Add(index, file[line].Substring(i + 1));
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been an error while reading the array file:\n" + FilePath + "\nAt line: " + line, ex);
                }
            }
            LastEntry = max;
        }



        /// <summary>
        /// Returns the entry number corresponding to the given string- returns 0xFFFFFFFF if nothing is found.
        /// </summary>
        public uint FindEntry(string name)
        {
            foreach (var entry in Entries)
            {
                if (entry.Value.Equals(name))
                    return entry.Key;
            }
            return 0xFFFFFFFF;
        }
        public void RenameEntry(uint value, string text)
        {
            Entries[value] = text;
            string[] file = new string[LastEntry + 1];
            int index = 0;
            foreach (var entry in Entries)
            {
                file[index++] = "0x" + Util.UInt32ToHex(entry.Key) + " " + entry.Value;
            }
            File.WriteAllLines(FilePath, file);
            Entries.Clear();
            Load(file);
        }



        /// <summary>
        /// Returns a regex for which every entry name in this array file will be a match
        /// </summary>
        public Regex GetRegex()
        {
            string result = "";
            foreach (var entry in Entries)
            {
                result += entry.Value;
                result += "|";
            }
            return new Regex(result.Substring(0, result.Length - 1));
        }

        public Object Current
        {
            get
            {
                return Entries[position];
            }
        } uint position = 0;
        public void Reset()
        {
            position = 0;
        }
        public Boolean MoveNext()
        {
            position++;
            return (position < LastEntry);
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
        public List<KeyValuePair<uint, string>> ToList()
        {
            return Entries.ToList();
        }
    }
}
