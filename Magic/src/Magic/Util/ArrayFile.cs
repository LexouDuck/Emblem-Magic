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
        public String this[UInt32 index]
        {
            get
            {
                String entry = "";
                if (this.Entries.TryGetValue(index, out entry))
                {
                    return entry;
                }
                return "";
            }
        }
        
        /// <summary>
        /// The index of the last entry in the list 
        /// </summary>
        public UInt32 LastEntry { get; private set; }
        /// <summary>
        /// The full path and name of the array file loaded
        /// </summary>
        String FilePath { get; }
        /// <summary>
        /// The actual string entry names and their indices
        /// </summary>
        Dictionary<UInt32, String> Entries { get; }
        
        public ArrayFile(String fileName)
        {
            this.FilePath = Core.Path_Arrays + fileName;
            this.Entries = new Dictionary<UInt32, String>();

            String[] file;
            try
            {
                file = File.ReadAllLines(this.FilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not open the array file:\n" + this.FilePath, ex);
            }
            this.Load(file);
        }
        public ArrayFile(String[] file)
        {
            this.FilePath = null;
            this.Entries = new Dictionary<UInt32, String>();

            this.Load(file);
        }
        void Load(String[] file)
        {
            UInt32 max = 0;
            UInt32 index;
            Int32 i;
            for (Int32 line = 0; line < file.Length; line++)
            {
                try
                {
                    i = 0;
                    while (i + 1 < file[line].Length && file[line][i] != ' ') i++;

                    if (file[line].StartsWith("0x"))
                         index = Util.HexToInt(file[line].Substring(0, i));
                    else index = UInt32.Parse(file[line].Substring(0, i));

                    if (index > max) max = index;

                    this.Entries.Add(index, file[line].Substring(i + 1));
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been an error while reading the array file:\n" + this.FilePath + "\nAt line: " + line, ex);
                }
            }
            this.LastEntry = max;
        }



        /// <summary>
        /// Returns the entry number corresponding to the given string- returns 0xFFFFFFFF if nothing is found.
        /// </summary>
        public UInt32 FindEntry(String name)
        {
            foreach (var entry in this.Entries)
            {
                if (entry.Value.Equals(name))
                    return entry.Key;
            }
            return 0xFFFFFFFF;
        }
        public void RenameEntry(UInt32 value, String text)
        {
            this.Entries[value] = text;
            String[] file = new String[this.LastEntry + 1];
            Int32 index = 0;
            foreach (var entry in this.Entries)
            {
                file[index++] = "0x" + Util.UInt32ToHex(entry.Key) + " " + entry.Value;
            }
            File.WriteAllLines(this.FilePath, file);
            this.Entries.Clear();
            this.Load(file);
        }



        /// <summary>
        /// Returns a regex for which every entry name in this array file will be a match
        /// </summary>
        public Regex GetRegex()
        {
            String result = "";
            foreach (var entry in this.Entries)
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
                return this.Entries[this.position];
            }
        }
        UInt32 position = 0;
        public void Reset()
        {
            this.position = 0;
        }
        public Boolean MoveNext()
        {
            this.position++;
            return (this.position < this.LastEntry);
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
        public List<KeyValuePair<UInt32, String>> ToList()
        {
            return this.Entries.ToList();
        }
    }
}
