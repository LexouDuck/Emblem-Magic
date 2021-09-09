using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GBA;
using Magic.Components;
using Magic.Properties;

namespace Magic.Editors
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
        String CurrentEntry
        {
            get
            {
                if (EntrySelector is ByteArrayBox)
                    return Status_Module + " 0x" + Util.ByteToHex(((ByteArrayBox)EntrySelector).Value) + " (" + EntrySelector.Text + ") - ";
                if (EntrySelector is PointerArrayBox)
                    return Status_Module + " " + ((PointerArrayBox)EntrySelector).Value + " (" + EntrySelector.Text + ") - ";
                if (EntrySelector is NumericUpDown)
                    return Status_Module + " 0x" + Util.ByteToHex((Byte)((NumericUpDown)EntrySelector).Value) + " - ";
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
                    return CurrentModule.Address + (Int32)((NumericUpDown)EntrySelector).Value * CurrentModule.EntryLength;
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

        

        public ModuleEditor(IApp app) : base(app)
        {
            InitializeComponent();

            WriteDisabled = true;

            File_OpenModule.LoadFiles(Core.Path_Modules, ".mmf");

            LayoutPanel.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

            Entry_MagicButton.Enabled = false;
        }


        
        public override void Core_Update()
        {
            Core_LoadValues(CurrentValue);
        }
        public override void Core_SetEntry(UInt32 entry)
        {
            if (EntrySelector is ByteArrayBox)
                ((ByteArrayBox)EntrySelector).Value = (Byte)entry;
            if (EntrySelector is PointerArrayBox)
                ((PointerArrayBox)EntrySelector).Value = new Pointer(entry);
            if (EntrySelector is NumericUpDown)
                ((NumericUpDown)EntrySelector).Value = entry;
        }

        /// <summary>
        /// Opens a module file and sets CurrentModule, as well as the status labels
        /// </summary>
        /// <param name="path"></param>
        public void Core_OpenFile(String path)
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
                    Entry_MagicButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been an error while trying to load this Module.", ex);
                }
            }
            else
            {
                ModuleEditor module = new ModuleEditor(App);
                module.Core_OpenFile(path);
                App.Core_OpenEditor(module);
            }
        }
        /// <summary>
        /// Loads the MMF module at the given filepath into this window
        /// </summary>
        void Core_LoadModule(String path)
        {
            if (path == null || path.Length == 0)
                throw new Exception("The module file path given is invalid.");
            if (!File.Exists(path))
                throw new Exception("The module file was not found: " + path);

            String[] filedata = File.ReadAllLines(path);
            List<String> file = new List<String>();
            foreach (String line in filedata)
            {
                if (line == null) continue;
                else if (line == "" || line[0] == '#')
                {
                    continue;
                }
                else for (Int32 i = 1; i < line.Length; i++)
                {
                    if (line[i] == '#') file.Add(line.Substring(0, i)); continue;
                }   // take out commented lines or parts of lines
                file.Add(line);
            }

            CurrentModule = new Module(App.Game.Identifier, file.ToArray());

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

            Boolean unused = false;
            List<String> sectionNames = new List<String>();
            foreach (Property item in CurrentModule.Properties)
            {
                if (item.Section == "Unused") { unused = true; continue; }

                if (!sectionNames.Contains(item.Section))
                    sectionNames.Add(item.Section);
            }
            if (unused) sectionNames.Add("Unused");
            
            GroupBox[] sections = new GroupBox[sectionNames.Count];
            TableLayoutPanel[] layouts = new TableLayoutPanel[sections.Length];
            for (Int32 i = 0; i < sections.Length; i++)
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

            Int32[] rows = new Int32[sections.Length];
            Int32 total = 0;
            Int32 index = 0;
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
            Int32 i = 0;
            foreach (Property property in CurrentModule.Properties)
            {
                switch (property.ControlType)
                {
                    case PropertyType.BOOL: {
                            ((CheckBox)ModuleControls[i]).CheckedChanged += delegate (Object sender, EventArgs e)
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
                            ((TextBox)ModuleControls[i]).TextChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    ByteArray.Make_ASCII(((TextBox)sender).Text),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.HEXT: {
                            ((HexBox)ModuleControls[i]).TextChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    ((HexBox)sender).Value,
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.HEXU:
                    case PropertyType.NUMU: {
                            ((NumericUpDown)ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((Int64)((NumericUpDown)sender).Value, property.Length >> 3, false),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.NUMS: {
                            ((NumericUpDown)ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((Int64)((NumericUpDown)sender).Value, property.Length >> 3, true),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.LIST: { if (property.FileName == null)
                            ((ByteBox)ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteByte(this,
                                    CurrentAddress + (property.Offset),
                                    ((ByteBox)sender).Value,
                                    CurrentEntry + property.Name + " changed");
                            };
                            else ((ByteArrayBox)ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((Int64)((NumericUpDown)sender).Value, property.Length >> 3, true),
                                    CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.POIN: { if (property.FileName == null)
                            ((PointerBox)ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!WriteDisabled) Core.WriteData(this,
                                    CurrentAddress + (property.Offset),
                                    ((PointerBox)sender).Value.ToBytes(),
                                    CurrentEntry + property.Name + " changed");
                            };
                            else ((PointerArrayBox)ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
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
        void Core_LoadValues(UInt32 entry)
        {
            WriteDisabled = true;
            Object[] values;
            try
            {
                if (EntrySelector is PointerArrayBox)
                    values = (CurrentModule.Entries == null) ?
                        CurrentModule[new Pointer(entry)] :
                        CurrentModule[new Pointer(entry)];
                else
                    values = CurrentModule[(Int32)entry];
            }
            catch (Exception ex)
            {
                UI.ShowError("The current entry could not be properly loaded.", ex);
                return;
            }
            Int32 i = 0;
            foreach (Property property in CurrentModule.Properties)
            {
                try
                {
                    switch (property.ControlType)
                    {
                        case PropertyType.BOOL: ((CheckBox)ModuleControls[i]).Checked = (Boolean)values[i]; break;

                        case PropertyType.TEXT: ((TextBox)ModuleControls[i]).Text = (String)values[i]; break;
                        case PropertyType.HEXT: ((HexBox)ModuleControls[i]).Value = (Byte[])values[i]; break;

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
                    UI.ShowError("There has been an error while trying to load the value for property:\n" + property.Name, ex);
                }
                i++;
            }
            WriteDisabled = false;
        }



        MagicButton GetEditorShortcut(Int32 index, Property property)
        {
            MagicButton result = new MagicButton(App)
            {
                //Location = new Point(ModuleControls[index].Width + 8, 0),
                Anchor = AnchorStyles.Left,
                EditorToOpen = property.EditorShortcut
            };
            switch (property.ControlType)
            {
                case PropertyType.BOOL:
                    ((CheckBox)ModuleControls[index]).CheckedChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = 0;
                    };
                    break;

                case PropertyType.TEXT:
                    ((TextBox)ModuleControls[index]).TextChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = 0;
                    };
                    break;

                case PropertyType.HEXT:
                    ((HexBox)ModuleControls[index]).TextChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = Util.BytesToUInt(((HexBox)ModuleControls[index]).Value, true);
                    };
                    break;

                case PropertyType.HEXU:
                case PropertyType.NUMU:
                    ((NumericUpDown)ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = (UInt32)((NumericUpDown)ModuleControls[index]).Value;
                    };
                    break;

                case PropertyType.NUMS:
                    ((NumericUpDown)ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = (UInt32)((NumericUpDown)ModuleControls[index]).Value;
                    };
                    break;

                case PropertyType.LIST:
                    if (property.FileName == null)
                    {
                        ((ByteBox)ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((ByteBox)ModuleControls[index]).Value;
                        };
                    }
                    else
                    {
                        ((ByteArrayBox)ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((ByteArrayBox)ModuleControls[index]).Value;
                        };
                    }
                    break;

                case PropertyType.POIN:
                    if (property.FileName == null)
                    {
                        ((PointerBox)ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((PointerBox)ModuleControls[index]).Value;
                        };
                    }
                    else
                    {
                        ((PointerArrayBox)ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((PointerArrayBox)ModuleControls[index]).Value;
                        };
                    }
                    break;

                default: break;
            }
            return result;
        }



        void EntryIndexChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        void File_OpenModule_Click(Object sender, ToolStripItemClickedEventArgs e)
        {
            FolderViewMenuItem menu_item = (FolderViewMenuItem)e.ClickedItem;

            Core_OpenFile(menu_item.FilePath);
        }
        void File_OpenMMFFile_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "All files (*.*)|*.*|Emblem Magic Modules (*.mmf)|*.mmf";
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
                ((PointerArrayBox)EntrySelector).Value = new GBA.Pointer(((PointerArrayBox)EntrySelector).Value.Address + (UInt32)CurrentModule.EntryLength);
            }
        }
        private void Offset_Minus_Button_Click(Object sender, EventArgs e)
        {
            if (EntrySelector is PointerArrayBox)
            {
                ((PointerArrayBox)EntrySelector).Value = new GBA.Pointer(((PointerArrayBox)EntrySelector).Value.Address - (UInt32)CurrentModule.EntryLength);
            }
        }

        private void Entry_MagicButton_Click(Object sender, EventArgs e)
        {
            BasicEditor editor = new BasicEditor(App);
            App.Core_OpenEditor(editor);

            Pointer address = CurrentAddress;
            Int32 length = CurrentModule.EntryLength;
            editor.Core_SetEntry(address, length, (length > 0 ? Core.ReadData(address, length) : null));
        }
    }
}
