using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GBA;

namespace Magic.Editors
{
    /// <summary>
    /// A Module is a set of data which allows us to generate UI controls from a text file template
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Gets an array of all the values for this entry
        /// </summary>
        public Object[] this[Int32 index]
        {
            get
            {
                Byte[] entry = Core.ReadData(this.Address + index * this.EntryLength, this.EntryLength);
                Object[] values = new Object[this.Properties.Length];
                for (Int32 i = 0; i < this.Properties.Length; i++)
                {
                    values[i] = this.Properties[i].GetValue(entry);
                }
                return values;
            }
        }
        /// <summary>
        /// Gets an array of all the values for this entry
        /// </summary>
        public Object[] this[Pointer address]
        {
            get
            {
                Byte[] entry = Core.ReadData(address, this.EntryLength);
                List<Object> values = new List<Object>();
                for (Int32 i = 0; i < this.Properties.Length; i++)
                {
                    values.Add(this.Properties[i].GetValue(entry));
                }
                return values.ToArray();
            }
        }



        /// <summary>
        /// The name of this MMF module
        /// </summary>
        public String Name { get; }
        /// <summary>
        /// The author of this MMF module
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
                return this.Pointer.CurrentAddress;
            }
        }
        /// <summary>
        /// The 'Repoint' object tied to this game array, so as to track if it's been repointed
        /// </summary>
        public Repoint Pointer { get; }
        /// <summary>
        /// The amount of bytes per entry in this data array.
        /// </summary>
        public Int32 EntryLength { get; }
        /// <summary>
        /// The array of entry names for this module.
        /// </summary>
        public ArrayFile Entries { get; set; }
        /// <summary>
        /// The properties this module can edit.
        /// </summary>
        public Property[] Properties { get; set; }



        public Module(String game_identifier, String[] file)
        {
            if (file[0] != game_identifier)
                throw new Exception("this Magic Module is meant for another ROM: " + file[0]);

            if (file[7] != "NULL")
            {
                this.Entries = new ArrayFile(file[7]);
            }
            else this.Entries = null;
            this.Pointer = new Repoint(file[1], ReadAddress(file[3], this.Entries));
            this.EntryLength = Int32.Parse(file[5]);
            Property module = new Property(1,
                file[1],
                file[2],
                "0", "0",
                file[6],
                file[7]);

            this.Name = module.Name;
            this.Author = module.Section;
            this.Entry = module.GetControl();
            if (this.Entry is NumericUpDown) ((NumericUpDown)this.Entry).Maximum = ReadNumber(file[4]);
            this.Entry.Name = "EntrySelector";
            this.Entry.TabIndex = 2;
            this.Entry.Location = new Point(80, (this.Entries == null) ? 2 : 0);
            this.Entry.MaximumSize = new Size(10000, 26);
            this.Entry.MinimumSize = new Size(30, 26);
            this.Entry.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.Entry.AutoSize = true;

            List<Property> properties = new List<Property>();
            for (Int32 i = 8; i < file.Length; i += 6)
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
            this.Properties = properties.ToArray();
        }


        
        /// <summary>
        /// Reads a string and makes it into its corresponding PropertyType
        /// </summary>
        public static PropertyType ReadControl(String text)
        {
            text = text.Substring(0, 4);
            foreach (String controltype in Enum.GetNames(typeof(PropertyType)))
            {
                if (text == controltype) return (PropertyType)Enum.Parse(typeof(PropertyType), text);
            }
            throw new Exception("Invalid module entry type written: " + text);
        }
        /// <summary>
        /// Reads the editor shortcut between parentheses from the MMF file
        /// </summary>
        public static String ReadShortcut(String text)
        {
            if (text.Length > 4)
            {
                String result = null;
                result = text.GetBetween('(', ')', 4);
                if (result == "") result = null;
                return result;
            }
            else return null;
        }
        /// <summary>
        /// Reads a string number (potentially with 'b' indicating a bit index)
        /// </summary>
        public static Int32 ReadNumber(String input)
        {
            String number = input;
            Boolean negative = false;
            Int32 numberbase = 10;
            Int32 bitshift = 3;
            Int32 result = -1;

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
        static Pointer ReadAddress(String pointer, ArrayFile entries)
        {
            Pointer result = Util.StringToAddress(pointer);
            if (result == new Pointer())
            {
                String caption = "Specify Module Pointer";
                String text = "This module has no address specified.\n Please enter the address of the data to edit.";
                result = (entries == null) ?
                    Prompt.ShowPointerDialog(text, caption) :
                    Prompt.ShowPointerArrayBoxDialog(text, caption);
            }
            return result;
        }
    }
}
