using EmblemMagic.Components;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
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
        public string Name { get; }
        /// <summary>
        /// The name of the file used for a drop-down box
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// What category this property is to be placed in.
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// At what index in the data this property is located, in bits
        /// </summary>
        public int Position { get; }
        /// <summary>
        /// How many bits long this property is. (do ">> 3" for bytes)
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// What kind of Control should be loaded for this property
        /// </summary>
        public PropertyType ControlType { get; }
        /// <summary>
        /// The editor to which this property has a shortcut button attached (is null if there is none)
        /// </summary>
        public string EditorShortcut { get; }

        /// <summary>
        /// Gets the byte offset of this property within the struct
        /// </summary>
        public int Offset
        {
            get
            {
                return (Position >> 3);
            }
        }
        /// <summary>
        /// Gets the right-to-left bit index of this property
        /// </summary>
        public int BitIndex
        {
            get
            {
                return (7 - (Position % 8));
            }
        }



        public Property(
            int line,
            string name,
            string category,
            string propertyIndex,
            string propertyLength,
            string controlType,
            string fileName)
        {
            try
            {
                Name = name;
                Section = category;
                Position = Module.ReadNumber(propertyIndex);
                Length = Module.ReadNumber(propertyLength);
                ControlType = Module.ReadControl(controlType);
                EditorShortcut = Module.ReadShortcut(controlType);
                FileName = (fileName == "NULL") ? null : fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("\nError at line " + line + ":\n" + ex.Message);
            }
        }



        /// <summary>
        /// Gets the value for this property within the given entry byte array
        /// </summary>
        public object GetValue(byte[] entry)
        {

            if (Length % 8 == 0)
            {   // if this is a byte entry
                int length = Length >> 3;
                byte[] data = new byte[length];
                Array.Copy(entry, Position / 8, data, 0, length);

                switch (ControlType)
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
                if (Length == 1) return Util.GetBit(entry[Position / 8], BitIndex);
                else return Util.GetBits(entry, Position, Length);
            }
        }
        /// <summary>
        /// Returns the control associated with this module property
        /// </summary>
        public Control GetControl()
        {
            Control control;
            long max = (long)Math.Pow(2, Length);
            switch (ControlType)
            {
                case PropertyType.BOOL: control = new CheckBox() {
                    Size = new Size(20, 20),
                }; break;

                case PropertyType.TEXT: control = new TextBox() {
                    Size = new Size(100, 20),
                    MaxLength = Length >> 3,
                }; break;

                case PropertyType.HEXT: control = new HexBox() {
                    Size = new Size(200, 50),
                    VScrollBarVisible = true
                }; break;

                case PropertyType.HEXU: control = new NumericUpDown() {
                    Size = new Size(32 + Length, 20),
                    Minimum = 0,
                    Maximum = max,
                    Hexadecimal = true,
                }; break;

                case PropertyType.NUMU: control = new NumericUpDown() {
                    Size = new Size(40 + Length, 20),
                    Minimum = 0,
                    Maximum = max,
                    Hexadecimal = false,
                }; break;

                case PropertyType.NUMS: control = new NumericUpDown() {
                    Size = new Size(40 + Length, 20),
                    Minimum = (max / 2) * -1,
                    Maximum = (max / 2),
                    Hexadecimal = false,
                }; break;

                case PropertyType.LIST:
                    if (FileName == null) control = new ByteBox() { };
                    else {
                        control = new ByteArrayBox() { AutoSize = true };
                        ((ByteArrayBox)control).Load(FileName);
                    } break;

                case PropertyType.POIN:
                    if (FileName == null) control = new PointerBox() { };
                    else {
                        control = new PointerArrayBox() { AutoSize = true };
                        ((PointerArrayBox)control).Load(FileName);
                    } break;

                default: return null;
            }
            control.Anchor = AnchorStyles.Left;
            return control;
        }
    }
}