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
                if (this.EntrySelector is ByteArrayBox)
                    return (UInt32)((ByteArrayBox)this.EntrySelector).Value;
                if (this.EntrySelector is PointerArrayBox)
                    return ((PointerArrayBox)this.EntrySelector).Value.Address;
                if (this.EntrySelector is NumericUpDown)
                    return (UInt32)((NumericUpDown)this.EntrySelector).Value;
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
                if (this.EntrySelector is ByteArrayBox)
                    return this.Status_Module + " 0x" + Util.ByteToHex(((ByteArrayBox)this.EntrySelector).Value) + " (" + this.EntrySelector.Text + ") - ";
                if (this.EntrySelector is PointerArrayBox)
                    return this.Status_Module + " " + ((PointerArrayBox)this.EntrySelector).Value + " (" + this.EntrySelector.Text + ") - ";
                if (this.EntrySelector is NumericUpDown)
                    return this.Status_Module + " 0x" + Util.ByteToHex((Byte)((NumericUpDown)this.EntrySelector).Value) + " - ";
                return this.Status_Module + " 0x??";
            }
        }
        /// <summary>
        /// Gets the address of the current array entry
        /// </summary>
        Pointer CurrentAddress
        {
            get
            {
                if (this.EntrySelector is ByteArrayBox)
                    return this.CurrentModule.Address + ((ByteArrayBox)this.EntrySelector).Value * this.CurrentModule.EntryLength;
                if (this.EntrySelector is PointerArrayBox)
                    return ((PointerArrayBox)this.EntrySelector).Value;
                if (this.EntrySelector is NumericUpDown)
                    return this.CurrentModule.Address + (Int32)((NumericUpDown)this.EntrySelector).Value * this.CurrentModule.EntryLength;
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
            this.InitializeComponent();

            this.WriteDisabled = true;

            this.File_OpenModule.LoadFiles(Core.Path_Modules, ".mmf");

            this.LayoutPanel.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

            this.Entry_MagicButton.Enabled = false;
        }


        
        public override void Core_Update()
        {
            this.Core_LoadValues(this.CurrentValue);
        }
        public override void Core_SetEntry(UInt32 entry)
        {
            if (this.EntrySelector is ByteArrayBox)
                ((ByteArrayBox)this.EntrySelector).Value = (Byte)entry;
            if (this.EntrySelector is PointerArrayBox)
                ((PointerArrayBox)this.EntrySelector).Value = new Pointer(entry);
            if (this.EntrySelector is NumericUpDown)
                ((NumericUpDown)this.EntrySelector).Value = entry;
        }

        /// <summary>
        /// Opens a module file and sets CurrentModule, as well as the status labels
        /// </summary>
        /// <param name="path"></param>
        public void Core_OpenFile(String path)
        {
            if (this.CurrentModule == null)
            {
                try
                {
                    this.Core_LoadModule(path);
                    this.Core_LoadLayout();
                    this.Core_LoadEvents();
                    if (this.EntrySelector is PointerArrayBox)
                    {
                        this.WriteDisabled = true;
                        ((PointerArrayBox)this.EntrySelector).Value = this.CurrentModule.Pointer.CurrentAddress;
                        this.EntrySelector.Location = new Point(this.EntrySelector.Location.X + 64, this.EntrySelector.Location.Y);
                        this.Offset_Minus_Button.Visible = true;
                        this.Offset_Plus_Button.Visible = true;
                        this.Core_LoadValues(this.CurrentModule.Pointer.CurrentAddress);
                    }
                    else this.Core_LoadValues(0);
                    this.Entry_MagicButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been an error while trying to load this Module.", ex);
                }
            }
            else
            {
                ModuleEditor module = new ModuleEditor();
                module.Core_OpenFile(path);
                this.App.Core_OpenEditor(module);
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

            this.CurrentModule = new Module(this.App.Game.Identifier, file.ToArray());

            this.Status_Module.Text = this.CurrentModule.Name;
            this.Status_Author.Text = "Author of this module: " + this.CurrentModule.Author;

            this.EntrySelector = this.CurrentModule.Entry;

            if (this.CurrentModule.Entries == null)
            {
                ((NumericUpDown)this.EntrySelector).ValueChanged += this.EntryIndexChanged;
            }
            else
            {
                ((IArrayBox)this.EntrySelector).Load(this.CurrentModule.Entries);
                ((IArrayBox)this.EntrySelector).ValueChanged += this.EntryIndexChanged;
            }

            this.Controls.Add(this.EntrySelector);
            this.EntrySelector.BringToFront();
        }
        /// <summary>
        /// Sets up the sections and controls for the module upon opening
        /// </summary>
        void Core_LoadLayout()
        {
            this.LayoutPanel.SuspendLayout();

            Boolean unused = false;
            List<String> sectionNames = new List<String>();
            foreach (Property item in this.CurrentModule.Properties)
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
                this.LayoutPanel.Controls.Add(sections[i]);
            }

            this.ModuleControls = new Control[this.CurrentModule.Properties.Length];

            Int32[] rows = new Int32[sections.Length];
            Int32 total = 0;
            Int32 index = 0;
            Label label;
            foreach (Property property in this.CurrentModule.Properties)
            {
                index = sectionNames.IndexOf(property.Section);

                label = new Label();
                label.Text = property.Name;
                label.AutoSize = true;
                label.Anchor = AnchorStyles.Right;

                this.ModuleControls[total] = property.GetControl();

                layouts[index].Controls.Add(label, 0, rows[index]);
                layouts[index].Controls.Add(this.ModuleControls[total], 2, rows[index]);
                if (property.EditorShortcut != null)
                {
                    layouts[index].Controls.Add(this.GetEditorShortcut(total, property), 1, rows[index]);
                }
                rows[index]++;
                total++;
            }

            this.LayoutPanel.ResumeLayout(true);
        }
        /// <summary>
        /// Loads the event functions for the controls so as to set data
        /// </summary>
        void Core_LoadEvents()
        {
            Int32 i = 0;
            foreach (Property property in this.CurrentModule.Properties)
            {
                switch (property.ControlType)
                {
                    case PropertyType.BOOL: {
                            ((CheckBox)this.ModuleControls[i]).CheckedChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteByte(this,
                                    this.CurrentAddress + (property.Offset),
                                    Util.SetBit(
                                        Core.ReadByte(this.CurrentAddress + (property.Offset)),
                                        property.BitIndex,
                                        ((CheckBox)sender).Checked),
                                    this.CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.TEXT: {
                            ((TextBox)this.ModuleControls[i]).TextChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteData(this,
                                    this.CurrentAddress + (property.Offset),
                                    ByteArray.Make_ASCII(((TextBox)sender).Text),
                                    this.CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.HEXT: {
                            ((HexBox)this.ModuleControls[i]).TextChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteData(this,
                                    this.CurrentAddress + (property.Offset),
                                    ((HexBox)sender).Value,
                                    this.CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.HEXU:
                    case PropertyType.NUMU: {
                            ((NumericUpDown)this.ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteData(this,
                                    this.CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((Int64)((NumericUpDown)sender).Value, property.Length >> 3, false),
                                    this.CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.NUMS: {
                            ((NumericUpDown)this.ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteData(this,
                                    this.CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((Int64)((NumericUpDown)sender).Value, property.Length >> 3, true),
                                    this.CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.LIST: { if (property.FileName == null)
                            ((ByteBox)this.ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteByte(this,
                                    this.CurrentAddress + (property.Offset),
                                    ((ByteBox)sender).Value,
                                    this.CurrentEntry + property.Name + " changed");
                            };
                            else ((ByteArrayBox)this.ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteData(this,
                                    this.CurrentAddress + (property.Offset),
                                    Util.NumberToBytes((Int64)((NumericUpDown)sender).Value, property.Length >> 3, true),
                                    this.CurrentEntry + property.Name + " changed");
                            }; } break;

                    case PropertyType.POIN: { if (property.FileName == null)
                            ((PointerBox)this.ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteData(this,
                                    this.CurrentAddress + (property.Offset),
                                    ((PointerBox)sender).Value.ToBytes(),
                                    this.CurrentEntry + property.Name + " changed");
                            };
                            else ((PointerArrayBox)this.ModuleControls[i]).ValueChanged += delegate (Object sender, EventArgs e)
                            {
                                if (!this.WriteDisabled) Core.WriteData(this,
                                    this.CurrentAddress + (property.Offset),
                                    ((PointerBox)sender).Value.ToBytes(),
                                    this.CurrentEntry + property.Name + " changed");
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
            this.WriteDisabled = true;
            Object[] values;
            try
            {
                if (this.EntrySelector is PointerArrayBox)
                    values = (this.CurrentModule.Entries == null) ?
                        this.CurrentModule[new Pointer(entry)] :
                        this.CurrentModule[new Pointer(entry)];
                else
                    values = this.CurrentModule[(Int32)entry];
            }
            catch (Exception ex)
            {
                UI.ShowError("The current entry could not be properly loaded.", ex);
                return;
            }
            Int32 i = 0;
            foreach (Property property in this.CurrentModule.Properties)
            {
                try
                {
                    switch (property.ControlType)
                    {
                        case PropertyType.BOOL: ((CheckBox)this.ModuleControls[i]).Checked = (Boolean)values[i]; break;

                        case PropertyType.TEXT: ((TextBox)this.ModuleControls[i]).Text = (String)values[i]; break;
                        case PropertyType.HEXT: ((HexBox)this.ModuleControls[i]).Value = (Byte[])values[i]; break;

                        case PropertyType.HEXU: ((NumericUpDown)this.ModuleControls[i]).Value = Convert.ToUInt32(values[i]); break;
                        case PropertyType.NUMU: ((NumericUpDown)this.ModuleControls[i]).Value = Convert.ToUInt32(values[i]); break;
                        case PropertyType.NUMS: ((NumericUpDown)this.ModuleControls[i]).Value = Convert.ToInt32(values[i]); break;

                        case PropertyType.LIST:
                            if (property.FileName == null)
                                ((ByteBox)this.ModuleControls[i]).Value = Convert.ToByte(values[i]);
                            else ((ByteArrayBox)this.ModuleControls[i]).Value = Convert.ToByte(values[i]); break;
                        case PropertyType.POIN:
                            if (property.FileName == null)
                                ((PointerBox)this.ModuleControls[i]).Value = (GBA.Pointer)values[i];
                            else ((PointerArrayBox)this.ModuleControls[i]).Value = (GBA.Pointer)values[i]; break;
                        default: break;
                    }
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been an error while trying to load the value for property:\n" + property.Name, ex);
                }
                i++;
            }
            this.WriteDisabled = false;
        }



        MagicButton GetEditorShortcut(Int32 index, Property property)
        {
            MagicButton result = new MagicButton(this.App)
            {
                //Location = new Point(ModuleControls[index].Width + 8, 0),
                Anchor = AnchorStyles.Left,
                EditorToOpen = property.EditorShortcut
            };
            switch (property.ControlType)
            {
                case PropertyType.BOOL:
                    ((CheckBox)this.ModuleControls[index]).CheckedChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = 0;
                    };
                    break;

                case PropertyType.TEXT:
                    ((TextBox)this.ModuleControls[index]).TextChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = 0;
                    };
                    break;

                case PropertyType.HEXT:
                    ((HexBox)this.ModuleControls[index]).TextChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = Util.BytesToUInt(((HexBox)this.ModuleControls[index]).Value, true);
                    };
                    break;

                case PropertyType.HEXU:
                case PropertyType.NUMU:
                    ((NumericUpDown)this.ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = (UInt32)((NumericUpDown)this.ModuleControls[index]).Value;
                    };
                    break;

                case PropertyType.NUMS:
                    ((NumericUpDown)this.ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                    {
                        result.EntryToSelect = (UInt32)((NumericUpDown)this.ModuleControls[index]).Value;
                    };
                    break;

                case PropertyType.LIST:
                    if (property.FileName == null)
                    {
                        ((ByteBox)this.ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((ByteBox)this.ModuleControls[index]).Value;
                        };
                    }
                    else
                    {
                        ((ByteArrayBox)this.ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((ByteArrayBox)this.ModuleControls[index]).Value;
                        };
                    }
                    break;

                case PropertyType.POIN:
                    if (property.FileName == null)
                    {
                        ((PointerBox)this.ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((PointerBox)this.ModuleControls[index]).Value;
                        };
                    }
                    else
                    {
                        ((PointerArrayBox)this.ModuleControls[index]).ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            result.EntryToSelect = ((PointerArrayBox)this.ModuleControls[index]).Value;
                        };
                    }
                    break;

                default: break;
            }
            return result;
        }



        void EntryIndexChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        void File_OpenModule_Click(Object sender, ToolStripItemClickedEventArgs e)
        {
            FolderViewMenuItem menu_item = (FolderViewMenuItem)e.ClickedItem;

            this.Core_OpenFile(menu_item.FilePath);
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
                this.Core_OpenFile(openWindow.FileName);
            }
        }



        private void Offset_Plus_Button_Click(Object sender, EventArgs e)
        {
            if (this.EntrySelector is PointerArrayBox)
            {
                ((PointerArrayBox)this.EntrySelector).Value = new GBA.Pointer(((PointerArrayBox)this.EntrySelector).Value.Address + (UInt32)this.CurrentModule.EntryLength);
            }
        }
        private void Offset_Minus_Button_Click(Object sender, EventArgs e)
        {
            if (this.EntrySelector is PointerArrayBox)
            {
                ((PointerArrayBox)this.EntrySelector).Value = new GBA.Pointer(((PointerArrayBox)this.EntrySelector).Value.Address - (UInt32)this.CurrentModule.EntryLength);
            }
        }

        private void Entry_MagicButton_Click(Object sender, EventArgs e)
        {
            BasicEditor editor = new BasicEditor();
            this.App.Core_OpenEditor(editor);

            Pointer address = this.CurrentAddress;
            Int32 length = this.CurrentModule.EntryLength;
            editor.Core_SetEntry(address, length, (length > 0 ? Core.ReadData(address, length) : null));
        }
    }
}
