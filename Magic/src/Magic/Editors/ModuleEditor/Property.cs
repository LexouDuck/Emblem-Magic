using Magic.Components;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Magic.Editors
{
    /// <summary>
    /// Represents the type of cotnrol that will be loaded for this Property
    /// </summary>
    public enum PropertyType
    {
        NULL,   // NULL
        BOOL,   // BOOL -> CheckBox
        TEXT,   // TEXT -> TextBox
        HEXT,   // HEXA -> HexBox
        HEXU,   // NEHU -> Hex numeric updown
        NUMU,   // NEDU -> Unsigned decimal NUD
        NUMS,   // NEDS -> Signed decimal NUD
        LIST,   // NDHU -> ByteBox with dropdown
        POIN,   // STRUCT -> PointerBox, with a dropdown if the file is not "NULL"
    }

    public class Property
    {
        /// <summary>
        /// The name of this module property
        /// </summary>
        public String Name { get; }
        /// <summary>
        /// The name of the file used for a drop-down box
        /// </summary>
        public String FileName { get; }
        /// <summary>
        /// What category this property is to be placed in.
        /// </summary>
        public String Section { get; }
        /// <summary>
        /// At what index in the data this property is located, in bits
        /// </summary>
        public Int32 Position { get; }
        /// <summary>
        /// How many bits long this property is. (do ">> 3" for bytes)
        /// </summary>
        public Int32 Length { get; }
        /// <summary>
        /// What kind of Control should be loaded for this property
        /// </summary>
        public PropertyType ControlType { get; }
        /// <summary>
        /// The editor to which this property has a shortcut button attached (is null if there is none)
        /// </summary>
        public String EditorShortcut { get; }

        /// <summary>
        /// Gets the byte offset of this property within the struct
        /// </summary>
        public Int32 Offset
        {
            get
            {
                return (this.Position >> 3);
            }
        }
        /// <summary>
        /// Gets the right-to-left bit index of this property
        /// </summary>
        public Int32 BitIndex
        {
            get
            {
                return (7 - (this.Position % 8));
            }
        }



        public Property(
            Int32 line,
            String name,
            String category,
            String propertyIndex,
            String propertyLength,
            String controlType,
            String fileName)
        {
            try
            {
                this.Name = name;
                this.Section = category;
                this.Position = Module.ReadNumber(propertyIndex);
                this.Length = Module.ReadNumber(propertyLength);
                this.ControlType = Module.ReadControl(controlType);
                this.EditorShortcut = Module.ReadShortcut(controlType);
                this.FileName = (fileName == "NULL") ? null : fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("\nError at line " + line + ":\n" + ex.Message);
            }
        }



        /// <summary>
        /// Gets the value for this property within the given entry byte array
        /// </summary>
        public Object GetValue(Byte[] entry)
        {

            if (this.Length % 8 == 0)
            {   // if this is a byte entry
                Int32 length = this.Length >> 3;
                Byte[] data = new Byte[length];
                Array.Copy(entry, this.Position / 8, data, 0, length);

                switch (this.ControlType)
                {
                    case PropertyType.TEXT: return data.GetASCII(0, length);
                    case PropertyType.BOOL: return (data[0] == 0) ? false : true;
                    case PropertyType.HEXT: return data;
                    case PropertyType.LIST: return data[0];
                    case PropertyType.HEXU: return Util.BytesToUInt(data, true);
                    case PropertyType.NUMU: return Util.BytesToUInt(data, true);
                    case PropertyType.NUMS: return Util.BytesToSInt(data, true);
                    case PropertyType.POIN: return new GBA.Pointer(data);

                    default: return null;
                }
            }
            else
            {   // so its a bit entry
                if (this.Length == 1) return Util.GetBit(entry[this.Position / 8], this.BitIndex);
                else return Util.GetBits(entry, this.Position, this.Length);
            }
        }
        /// <summary>
        /// Returns the control associated with this module property
        /// </summary>
        public Control GetControl()
        {
            Control control;
            Int64 max = (Int64)Math.Pow(2, this.Length);
            switch (this.ControlType)
            {
                case PropertyType.BOOL: control = new CheckBox() {
                    Size = new Size(20, 20),
                }; break;

                case PropertyType.TEXT: control = new TextBox() {
                    Size = new Size(100, 20),
                    MaxLength = this.Length >> 3,
                }; break;

                case PropertyType.HEXT: control = new HexBox() {
                    Size = new Size(200, 50),
                    VScrollBarVisible = true
                }; break;

                case PropertyType.HEXU: control = new NumericUpDown() {
                    Size = new Size(32 + this.Length, 20),
                    Minimum = 0,
                    Maximum = max,
                    Hexadecimal = true,
                }; break;

                case PropertyType.NUMU: control = new NumericUpDown() {
                    Size = new Size(40 + this.Length, 20),
                    Minimum = 0,
                    Maximum = max,
                    Hexadecimal = false,
                }; break;

                case PropertyType.NUMS: control = new NumericUpDown() {
                    Size = new Size(40 + this.Length, 20),
                    Minimum = (max / 2) * -1,
                    Maximum = (max / 2),
                    Hexadecimal = false,
                }; break;

                case PropertyType.LIST:
                    if (this.FileName == null) control = new ByteBox() { };
                    else {
                        control = new ByteArrayBox() { AutoSize = true };
                        ((ByteArrayBox)control).Load(this.FileName);
                    } break;

                case PropertyType.POIN:
                    if (this.FileName == null) control = new PointerBox() { };
                    else {
                        control = new PointerArrayBox() { AutoSize = true };
                        ((PointerArrayBox)control).Load(this.FileName);
                    } break;

                default: return null;
            }
            control.Anchor = AnchorStyles.Left;
            return control;
        }
    }
}