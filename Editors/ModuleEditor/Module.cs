using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public class Module
    {
        /// <summary>
        /// Gets an array of all the values for this entry
        /// </summary>
        public object[] this[int index]
        {
            get
            {
                byte[] entry = Core.ReadData(Address + index * EntryLength, EntryLength);
                object[] values = new object[Properties.Length];
                for (int i = 0; i < Properties.Length; i++)
                {
                    values[i] = Properties[i].GetValue(entry);
                }
                return values;
            }
        }
        /// <summary>
        /// Gets an array of all the values for this entry
        /// </summary>
        public object[] this[Pointer address]
        {
            get
            {
                byte[] entry = Core.ReadData(address, EntryLength);
                List<object> values = new List<object>();
                for (int i = 0; i < Properties.Length; i++)
                {
                    values.Add(Properties[i].GetValue(entry));
                }
                return values.ToArray();
            }
        }



        /// <summary>
        /// The name of this EMM module
        /// </summary>
        public String Name { get; }
        /// <summary>
        /// The author of this EMM module
        /// </summary>
        public String Author { get; }
        /// <summary>
        /// What kind of entry selector this module has
        /// </summary>
        public Control Entry { get; set; }

        /// <summary>
        /// The location of this data array in the ROM
        /// </summary>
        public Pointer Address
        {
            get
            {
                return Pointer.CurrentAddress;
            }
        }
        /// <summary>
        /// The 'Repoint' object tied to this game array, so as to track if it's been repointed
        /// </summary>
        public Repoint Pointer { get; }
        /// <summary>
        /// The amount of bytes per entry in this data array.
        /// </summary>
        public int EntryLength { get; }
        /// <summary>
        /// The array of entry names for this module.
        /// </summary>
        public ArrayFile Entries { get; set; }
        /// <summary>
        /// The properties this module can edit.
        /// </summary>
        public Property[] Properties { get; set; }



        public Module(string[] file)
        {
            Pointer = new Repoint(file[1], ReadAddress(file[3]));
            EntryLength = int.Parse(file[5]);

            if (file[0] != Program.Core.CurrentROM.GetIdentifier())
                throw new Exception("this Emblem Magic Module is meant for another ROM: " + file[0]);
            
            if (file[7] != "NULL")
            {
                Entries = new ArrayFile(file[7]);
            }
            Property module = new Property(1,
                file[1],
                file[2],
                "0", "0",
                file[6],
                file[7]);

            Name = module.Name;
            Author = module.Section;
            Entry = module.GetControl();
            if (Entry is NumericUpDown) ((NumericUpDown)Entry).Maximum = ReadNumber(file[4]);
            Entry.Name = "EntrySelector";
            Entry.TabIndex = 2;
            Entry.Location = new Point(50, (Entries == null) ? 2 : 0);
            Entry.MaximumSize = new Size(10000, 26);
            Entry.MinimumSize = new Size(30, 26);
            Entry.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Entry.AutoSize = true;

            List<Property> properties = new List<Property>();
            for (int i = 8; i < file.Length; i += 6)
            {
                while (file[i] == "") i++;
                properties.Add(new Property(
                    i,
                    file[i],
                    file[i + 1],
                    file[i + 2],
                    file[i + 3],
                    file[i + 4],
                    file[i + 5]));
            }
            Properties = properties.ToArray();
        }


        
        /// <summary>
        /// Reads a string and makes it into its corresponding PropertyType
        /// </summary>
        public static PropertyType ReadControl(string text)
        {
            text = text.Substring(0, 4);
            foreach (string controltype in Enum.GetNames(typeof(PropertyType)))
            {
                if (text == controltype) return (PropertyType)Enum.Parse(typeof(PropertyType), text);
            }
            throw new Exception("Invalid module entry type written: " + text);
        }
        /// <summary>
        /// Reads the editor shortcut between parentheses from the EMM file
        /// </summary>
        public static string ReadShortcut(string text)
        {
            if (text.Length > 4)
            {
                string result = null;
                result = text.GetBetween('(', ')', 4);
                if (result == "") result = null;
                return result;
            }
            else return null;
        }
        /// <summary>
        /// Reads a string number (potentially with 'b' indicating a bit index)
        /// </summary>
        public static int ReadNumber(string input)
        {
            string number = input;
            bool negative = false;
            int numberbase = 10;
            int bitshift = 3;
            int result = -1;

            if (input.StartsWith("-"))
            {
                negative = true;
                number = input.Substring(1);
            }

            if (input.StartsWith("0x"))
            {
                numberbase = 16;
                number = input.Substring(2);
            }
            else if (input.EndsWith("b"))
            {
                bitshift = 0;
                number = input.Substring(0, input.Length - 1);
            }

            try
            {
                result = Convert.ToInt32(number, numberbase);
            }
            catch
            {
                throw new Exception("Module contains an invalid number: " + number);
            }
            result <<= bitshift;
            if (negative) result = 0 - result;
            return result;
        }
        /// <summary>
        /// Reads a string hex address and returns the corresponding GBA.Pointer
        /// </summary>
        static Pointer ReadAddress(string pointer)
        {
            Pointer result = Util.StringToAddress(pointer);
            if (result == new Pointer())
                result = Prompt.ShowPointerDialog(
                "This module has no address specified.\n Please enter the address at which the data to edit is located.",
                "Specify Module Pointer");
            return result;
        }
    }
}
