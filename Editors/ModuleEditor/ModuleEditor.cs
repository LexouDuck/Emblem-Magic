using EmblemMagic.Components;
using EmblemMagic.Properties;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class ModuleEditor : Editor
    {
        /// <summary>
        /// The entry selector for the module - can be a ByteArrayBox or a PointerArrayBox
        /// </summary>
        Control EntrySelector;
        /// <summary>
        /// The current loaded nightmare module
        /// </summary>
        Module CurrentModule { get; set; }
        /// <summary>
        /// Gets the value of the current entry
        /// </summary>
        UInt32 CurrentValue
        {
            get
            {
                if (EntrySelector is ByteArrayBox)
                    return (UInt32)((ByteArrayBox)EntrySelector).Value;
                if (EntrySelector is PointerArrayBox)
                    return ((PointerArrayBox)EntrySelector).Value.Address;
                if (EntrySelector is NumericUpDown)
                    return (UInt32)((NumericUpDown)EntrySelector).Value;
                return 0;
            }
        }
        /// <summary>
        /// Returns the full name of the current entry: "[EntryType] 0x???? ([EntryText]) - "
        /// </summary>
        string CurrentEntry
        {
            get
            {
                if (EntrySelector is ByteArrayBox)
                    return Status_Module + " 0x" + Util.ByteToHex(((ByteArrayBox)EntrySelector).Value) + " (" + EntrySelector.Text + ") - ";
                if (EntrySelector is PointerArrayBox)
                    return Status_Module + " " + ((PointerArrayBox)EntrySelector).Value + " (" + EntrySelector.Text + ") - ";
                if (EntrySelector is NumericUpDown)
                    return Status_Module + " 0x" + Util.ByteToHex((byte)((NumericUpDown)EntrySelector).Value) + " - ";
                return Status_Module + " 0x??";
            }
        }
        /// <summary>
        /// Gets the address of the current array entry
        /// </summary>
        Pointer CurrentAddress
        {
            get
            {
                if (EntrySelector is ByteArrayBox)
                    return CurrentModule.Address + ((ByteArrayBox)EntrySelector).Value * CurrentModule.EntryLength;
                if (EntrySelector is PointerArrayBox)
                    return ((PointerArrayBox)EntrySelector).Value;
                if (EntrySelector is NumericUpDown)
                    return CurrentModule.Address + (int)((NumericUpDown)EntrySelector).Value * CurrentModule.EntryLength;
                return new Pointer();
            }
        }

        /// <summary>
        /// The controls that this module loads and deals with.
        /// </summary>
        public Control[] ModuleControls { get; set; }

        /// <summary>
        /// Tells whether or not writing events on the module's controls are currently disabled.
        /// </summary>
        public Boolean WriteDisabled { get; set; }

        

        public ModuleEditor()
        {
            InitializeComponent();

            WriteDisabled = true;

            File_OpenModule.LoadFiles(Core.Path_Modules, ".emm");

            LayoutPanel.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
        }


        
        public override void Core_Update()
        {
            Core_LoadValues(CurrentValue);
        }
        public override void Core_SetEntry(uint entry)
        {
            if (EntrySelector is ByteArrayBox)
                ((ByteArrayBox)EntrySelector).Value = (byte)entry;
            if (EntrySelector is PointerArrayBox)
                ((PointerArrayBox)EntrySelector).Value = new Pointer(entry);
            if (EntrySelector is NumericUpDown)
                ((NumericUpDown)EntrySelector).Value = entry;
        }

        /// <summary>
        /// Opens a module file and sets CurrentModule, as well as the status labels
        /// </summary>
        /// <param name="path"></param>
        public void Core_OpenFile(string path)
        {
            if (CurrentModule == null)
            {
                try
                {
                    Core_LoadModule(path);
                    Core_LoadLayout();
                    Core_LoadEvents();
                    if (EntrySelector is PointerArrayBox)
                    {
                        WriteDisabled = true;
                        ((PointerArrayBox)EntrySelector).Value = CurrentModule.Pointer.CurrentAddress;
                        EntrySelector.Location = new Point(EntrySelector.Location.X + 64, EntrySelector.Location.Y);
                        Offset_Minus_Button.Visible = true;
                        Offset_Plus_Button.Visible = true;
                        Core_LoadValues(CurrentModule.Pointer.CurrentAddress);
                    }
                    else Core_LoadValues(0);
                }
                catch (Exception ex)
                {
                    Program.ShowError("There has been an error while trying to load this Module.", ex);
                }
            }
            else
            {
                ModuleEditor module = new ModuleEditor();
                module.Core_OpenFile(path);
                Program.Core.Core_OpenEditor(module);
            }
        }
        /// <summary>
        /// Loads the EMM module at the given filepath into this window
        /// </summary>
        void Core_LoadModule(string path)
        {
            if (path == null || path.Length == 0)
                throw new Exception("The module file path given is invalid.");
            if (!File.Exists(path))
                throw new Exception("The module file was not found: " + path);

            string[] filedata = File.ReadAllLines(path);
            List<string> file = new List<string>();
            foreach (string line in filedata)
            {
                if (line == null) continue;
                else if (line == "" || line[0] == '#')
                {
                    continue;
                }
                else for (int i = 1; i < line.Length; i++)
                {
                    if (line[i] == '#') file.Add(line.Substring(0, i)); continue;
                }   // take out commented lines or parts of lines
                file.Add(line);
            }

            CurrentModule = new Module(file.ToArray());

            Status_Module.Text = CurrentModule.Name;
            Status_Author.Text = "Author of this module: " + CurrentModule.Author;

            EntrySelector = CurrentModule.Entry;

            if (CurrentModule.Entries == null)
            {
                ((NumericUpDown)EntrySelector).ValueChanged += EntryIndexChanged;
            }
            else
            {
                ((IArrayBox)EntrySelector).Load(CurrentModule.Entries);
                ((IArrayBox)EntrySelector).ValueChanged += EntryIndexChanged;
            }

            this.Controls.Add(EntrySelector);
            EntrySelector.BringToFront();
        }
        /// <summary>
        /// Sets up the sections and controls for the module upon opening
        /// </summary>
        void Core_LoadLayout()
        {
            LayoutPanel.SuspendLayout();

            bool unused = false;
            List<string> sectionNames = new List<string>();
            foreach (Property item in CurrentModule.Properties)
            {
                if (item.Section == "Unused") { unused = true; continue; }

                if (!sectionNames.Contains(item.Section))
                    sectionNames.Add(item.Section);
            }
            if (unused) sectionNames.Add("Unused");
            
            GroupBox[] sections = new GroupBox[sectionNames.Count];
            TableLayoutPanel[] layouts = new TableLayoutPanel[sections.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                sections[i] = new GroupBox()
                {
                    Text = sectionNames[i],
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                layouts[i] = new TableLayoutPanel()
                {
                    Location = new Point(10, 20),
                    ColumnCount = 3,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                sections[i].Controls.Add(layouts[i]);
                LayoutPanel.Controls.Add(sections[i]);
            }

            ModuleControls = new Control[CurrentModule.Properties.Length];

            int[] rows = new int[sections.Length];
            int total = 0;
            int index = 0;
            Label label;
            foreach (Property property in CurrentModule.Properties)
            {
                index = sectionNames.IndexOf(property.Section);

                label = new Label();
                label.Text = property.Name;
                label.AutoSize = true;
                label.Anchor = AnchorStyles.Right;

                ModuleControls[total] = property.GetControl();

                layouts[index].Controls.Add(label, 0, rows[index]);
                layouts[index].Controls.Add(ModuleControls[total], 2, rows[index]);
                if (property.EditorShortcut != null)
                {
                    layouts[index].Controls.Add(GetEditorShortcut(total, property), 1, rows[index]);
                }
                rows[index]++;
                total++;
            }

            LayoutPanel.ResumeLayout(true);
        }
        /// <summary>
        /// Loads the event functions for the controls so as to set data
        /// </summary>
        void Core_LoadEvents()
        {
            int i = 0;
            foreach (Property property in CurrentModule.Properties)
            {
                switch (property.ControlType)
                {
                    case PropertyType.BOOL: {
                            ((CheckBox)ModuleControls[i]).CheckedChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteByte(this,
                                    CurrentAddress + (property.Offset),
                                    Util.SetBit(
                                        Core.ReadByte(CurrentAddress + (property.Offset)),
                                        property.BitIndex,
                                        ((CheckBox)sender).Checked),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.TEXT: {
                            ((TextBox)ModuleControls[i]).TextChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    ByteArray.Make_ASCII(((TextBox)sender).Text),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.HEXT: {
                            ((HexBox)ModuleControls[i]).TextChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    ((HexBox)sender).Value,
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.HEXU:
                    case PropertyType.NUMU: {
                            ((NumericUpDown)ModuleControls[i]).ValueChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((long)((NumericUpDown)sender).Value, property.Length >> 3, false),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.NUMS: {
                            ((NumericUpDown)ModuleControls[i]).ValueChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((long)((NumericUpDown)sender).Value, property.Length >> 3, true),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.LIST: { if (property.FileName == null)
                            ((ByteBox)ModuleControls[i]).ValueChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteByte(this,
                                    CurrentAddress + (property.Offset),
                                    ((ByteBox)sender).Value,
                                    CurrentEntry + property.Name + " changed");
                            };
                            else ((ByteArrayBox)ModuleControls[i]).ValueChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((long)((NumericUpDown)sender).Value, property.Length >> 3, true),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.POIN: { if (property.FileName == null)
                            ((PointerBox)ModuleControls[i]).ValueChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    ((PointerBox)sender).Value.ToBytes(),
                                    CurrentEntry + property.Name + " changed");
                            };
                            else ((PointerArrayBox)ModuleControls[i]).ValueChanged += delegate (object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    ((PointerBox)sender).Value.ToBytes(),
                                    CurrentEntry + property.Name + " changed");
                            };} break;

                    default: break;
                }
                i++;
            }
        }
        /// <summary>
        /// Loads the correct values into the controls for the array entry at the given index
        /// </summary>
        void Core_LoadValues(uint entry)
        {
            WriteDisabled = true;
            object[] values;
            try
            {
                if (EntrySelector is PointerArrayBox)
                    values = (CurrentModule.Entries == null) ?
                        CurrentModule[new Pointer(entry)] :
                        CurrentModule[new Pointer(entry)];
                else
                    values = CurrentModule[(int)entry];
            }
            catch (Exception ex)
            {
                Program.ShowError("The current entry could not be properly loaded.", ex);
                return;
            }
            int i = 0;
            foreach (Property property in CurrentModule.Properties)
            {
                try
                {
                    switch (property.ControlType)
                    {
                        case PropertyType.BOOL: ((CheckBox)ModuleControls[i]).Checked = (bool)values[i]; break;

                        case PropertyType.TEXT: ((TextBox)ModuleControls[i]).Text = (string)values[i]; break;
                        case PropertyType.HEXT: ((HexBox)ModuleControls[i]).Value = (byte[])values[i]; break;

                        case PropertyType.HEXU: ((NumericUpDown)ModuleControls[i]).Value = Convert.ToUInt32(values[i]); break;
                        case PropertyType.NUMU: ((NumericUpDown)ModuleControls[i]).Value = Convert.ToUInt32(values[i]); break;
                        case PropertyType.NUMS: ((NumericUpDown)ModuleControls[i]).Value = Convert.ToInt32(values[i]); break;

                        case PropertyType.LIST:
                            if (property.FileName == null)
                                ((ByteBox)ModuleControls[i]).Value = Convert.ToByte(values[i]);
                            else ((ByteArrayBox)ModuleControls[i]).Value = Convert.ToByte(values[i]); break;
                        case PropertyType.POIN:
                            if (property.FileName == null)
                                ((PointerBox)ModuleControls[i]).Value = (GBA.Pointer)values[i];
                            else ((PointerArrayBox)ModuleControls[i]).Value = (GBA.Pointer)values[i]; break;
                        default: break;
                    }
                }
                catch (Exception ex)
                {
                    Program.ShowError("There has been an error while trying to load the value for property:\n" + property.Name, ex);
                }
                i++;
            }
            WriteDisabled = false;
        }



        MagicButton GetEditorShortcut(int index, Property property)
        {
            MagicButton result = new MagicButton()
            {
                //Location = new Point(ModuleControls[index].Width + 8, 0),
                Anchor = AnchorStyles.Left,
                EditorToOpen = property.EditorShortcut
            };
            switch (property.ControlType)
            {
                case PropertyType.BOOL:
                    ((CheckBox)ModuleControls[index]).CheckedChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = 0;
                    }; break;

                case PropertyType.TEXT:
                    ((TextBox)ModuleControls[index]).TextChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = 0;
                    }; break;

                case PropertyType.HEXT:
                    ((HexBox)ModuleControls[index]).TextChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = Util.BytesToUInt(((HexBox)ModuleControls[index]).Value, true);
                    }; break;

                case PropertyType.HEXU:
                case PropertyType.NUMU:
                    ((NumericUpDown)ModuleControls[index]).ValueChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = (uint)((NumericUpDown)ModuleControls[index]).Value;
                    }; break;

                case PropertyType.NUMS:
                    ((NumericUpDown)ModuleControls[index]).ValueChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = (uint)((NumericUpDown)ModuleControls[index]).Value;
                    }; break;

                case PropertyType.LIST:
                    if (property.FileName == null)
                        ((ByteBox)ModuleControls[index]).ValueChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = ((ByteBox)ModuleControls[index]).Value;
                    };
                    else ((ByteArrayBox)ModuleControls[index]).ValueChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = ((ByteArrayBox)ModuleControls[index]).Value;
                    };
                    break;

                case PropertyType.POIN:
                    if (property.FileName == null)
                        ((PointerBox)ModuleControls[index]).ValueChanged += delegate (object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((PointerBox)ModuleControls[index]).Value;
                        };
                    else ((PointerArrayBox)ModuleControls[index]).ValueChanged += delegate (object sender, EventArgs e)
                    {
                        result.EntryToSelect = ((PointerArrayBox)ModuleControls[index]).Value;
                    };
                    break;

                default: break;
            }
            return result;
        }



        void EntryIndexChanged(object sender, EventArgs e)
        {
            Core_Update();
        }

        void File_OpenModule_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            FolderViewMenuItem menu_item = (FolderViewMenuItem)e.ClickedItem;

            Core_OpenFile(menu_item.FilePath);
        }
        void File_OpenEMMFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "All files (*.*)|*.*|Emblem Magic Modules (*.emm)|*.emm";
            openWindow.FilterIndex = 1;
            openWindow.InitialDirectory = Settings.Default.PathModules;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                Core_OpenFile(openWindow.FileName);
            }
        }



        private void Offset_Plus_Button_Click(Object sender, EventArgs e)
        {
            if (EntrySelector is PointerArrayBox)
            {
                ((PointerArrayBox)EntrySelector).Value = new GBA.Pointer(((PointerArrayBox)EntrySelector).Value.Address + (uint)CurrentModule.EntryLength);
            }
        }
        private void Offset_Minus_Button_Click(Object sender, EventArgs e)
        {
            if (EntrySelector is PointerArrayBox)
            {
                ((PointerArrayBox)EntrySelector).Value = new GBA.Pointer(((PointerArrayBox)EntrySelector).Value.Address - (uint)CurrentModule.EntryLength);
            }
        }
    }
}
