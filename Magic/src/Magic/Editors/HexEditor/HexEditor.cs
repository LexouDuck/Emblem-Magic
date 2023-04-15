using Magic.Components;
using Magic;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Magic.Editors
{
    public partial class HexEditor : Editor
    {
        HexBox  MainHexBox;
        TabPage MainTabPage;
        List<HexBox>  FileHexBoxes;
        List<TabPage> FileTabPages;
        List<String>  FilePaths;

        /// <summary>
        /// Get or sets the currently focused tab.
        /// </summary>
        Int32 CurrentTab
        {
            get
            {
                this._current = this.Tabs_Control.SelectedIndex - 1;
                return this._current;
            }
            set
            {
                this._current = value;
                this.Tabs_Control.SelectTab(this._current + 1);
                this.Tabs_Control.SelectedTab.Focus();
                this.Update_FileMenu();
                this.Update_EditMenu();
                this.Update_StatusLabel();
                return;
            }
        }
        Int32 _current;
        /// <summary>
        /// Returns true if the currently focused tab is that of the open ROM
        /// </summary>
        Boolean CurrentTabIsROM()
        {
            return (this.Tabs_Control.SelectedIndex == 0);
        }
        /// <summary>
        /// Gets or sets the address of the current cursor location
        /// </summary>
        Pointer CurrentAddress
        {
            get
            {
                HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];
                return new Pointer((UInt32)((hexbox.CurrentLine - 1) * hexbox.BytesPerLine + hexbox.CurrentPositionInLine));
            }
            set
            {
                HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];
                hexbox.Select((UInt32)value, 1);
                hexbox.Focus();
            }
        }

        HexFind Tool_Find;
        HexGoTo Tool_GoTo;

        List<Tuple<Pointer, Byte>> Differences;

        public HexEditor()
        {
            try
            {
                this.InitializeComponent();

                this.Tabs_Control.TabIndexChanged += this.Core_UpdateMenu;

                this.FileHexBoxes = new List<HexBox>();
                this.FileTabPages = new List<TabPage>();
                this.FilePaths = new List<String>();

                this.Differences = new List<Tuple<Pointer, Byte>>();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        /// <summary>
        /// Initializes the hex editor´s main form
        /// </summary>
        public override void Core_OnOpen()
        {
            this.Core_LoadFile();

            this.Core_UpdateMenu(this, null);
        }
        /// <summary>
        /// Called whenever changes are made outside this editor
        /// </summary>
        public override void Core_Update()
        {
            Pointer current = this.CurrentAddress;

            this.Core_LoadFile();

            this.Core_UpdateMenu(this, null);

            this.CurrentAddress = current - 1;
        }
        /// <summary>
        /// Potentially prompts the user about closing files without saving changes
        /// </summary>
        public override void Core_OnExit(Object sender, FormClosingEventArgs e)
        {
            DialogResult answer;
            for (Int32 i = 0; i < this.FilePaths.Count; i++)
            {
                answer = Prompt.SaveHexChanges();
                this.CurrentTab = i;
                if (answer == DialogResult.Cancel) e.Cancel = true;
                else this.Core_ExitFile(i);
            }
        }


        /// <summary>
        /// Loads the current ROM onto the main hexbox.
        /// </summary>
        void Core_LoadFile()
        {
            try
            {
                DataByteProvider rom;
                rom = new DataByteProvider(this.App.ROM.FileData);
                rom.Changed += new EventHandler(this.HexBox_ByteProvider_Changed);
                rom.LengthChanged += new EventHandler(this.HexBox_ByteProvider_LengthChanged);
                rom.Changed += new EventHandler(this.Core_ROMChanged);
                rom.LengthChanged += new EventHandler(this.Core_ROMSizeChanged);

                this.MainHexBox.ByteProvider = rom;
                this.Differences = new List<Tuple<Pointer, Byte>>();
            }
            catch (Exception ex)
            {
                UI.ShowError("The attempt to load the ROM into the hex editor has failed.", ex);
                return;
            }
            this.Update_TabControl();
            this.Update_StatusLabel();
        }
        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="path">the file name of the file to open</param>
        void Core_OpenFile(String path)
        {
            if (!File.Exists(path))
            {
                UI.ShowMessage("The file does not exist.");
                return;
            }

            try
            {
                FileByteProvider file;
                try
                {
                    // try to open in write mode
                    file = new FileByteProvider(path);
                    file.Changed += new EventHandler(this.HexBox_ByteProvider_Changed);
                    file.LengthChanged += new EventHandler(this.HexBox_ByteProvider_LengthChanged);
                }
                catch (IOException) // write mode failed
                {
                    try
                    {
                        // try to open in read-only mode
                        file = new FileByteProvider(path, true);
                        if (UI.ShowQuestion("This file can only be opened as read-only. Proceed ?") == DialogResult.No)
                        {
                            file.Dispose();
                            return;
                        }
                    }
                    catch (IOException ex) // read-only also failed
                    {
                        // file cannot be opened
                        UI.ShowError("The attempt to open the file has failed.", ex);
                        return;
                    }
                }

                this.Core_CreateTabPage(file, path);

                this.Menu_File_RecentFiles.AddFile(path);
            }
            catch (Exception ex)
            {
                UI.ShowError(ex);
                return;
            }
        }
        /// <summary>
        /// Saves the loaded ROM, as doing Save in the main hub would.
        /// </summary>
        void Core_SaveFile()
        {
            try
            {
                this.App.Core_SaveROMFile(this.App.ROM.FilePath);
            }
            catch (Exception ex)
            {
                UI.ShowError(ex);
            }
        }
        /// <summary>
        /// Saves the file at the given index - optimized for using an already open stream
        /// </summary>
        void Core_SaveFile(Int32 index)
        {
            try
            {
                FileByteProvider file = this.FileHexBoxes[index].ByteProvider as FileByteProvider;
                file.ApplyChanges();
            }
            catch (Exception ex)
            {
                UI.ShowError("The file could not be saved.", ex);
            }
        }
        /// <summary>
        /// Saves the loaded ROM as a new file.
        /// </summary>
        void Core_MakeFile()
        {
            SaveFileDialog SaveROM_Window = new SaveFileDialog();

            SaveROM_Window.Filter = "GBA ROMs (*.gba)|*.gba|All files (*.*)|*.*";
            SaveROM_Window.FilterIndex = 1;
            SaveROM_Window.RestoreDirectory = true;
            SaveROM_Window.CreatePrompt = true;
            SaveROM_Window.OverwritePrompt = true;

            if (SaveROM_Window.ShowDialog() == DialogResult.OK)
            {
                if ((SaveROM_Window.OpenFile()) != null)
                {
                    this.App.Core_SaveROMFile(SaveROM_Window.FileName);
                }
            }
        }
        /// <summary>
        /// Saves the file from the tab at the given index as a new file.
        /// </summary>
        void Core_MakeFile(Int32 index)
        {
            using (FileStream file = File.OpenWrite(this.FilePaths[index]))
            {
                Byte[] data = this.FileHexBoxes[index].Value;
                file.Write(data, 0, (Int32)data.Length);

                Console.Out.WriteLine("Saved " + data.Length + " to file.");
                //file.Close();
            }
        }
        /// <summary>
        /// Closes the tabpage corresponding to the given index
        /// </summary>
        void Core_ExitFile(Int32 index)
        {
            if (this.CurrentTabIsROM())
            {
                UI.ShowError("The Hex Editor tab of the current loaded ROM cannot be closed.");
            }
            else
            {
                ((IDisposable)this.FileHexBoxes[index].ByteProvider).Dispose();
                this.FileHexBoxes[index].ByteProvider = null;

                this.Core_CloseTabPage(index);

                this.CurrentTab = index;
            }

            this.Update_FileMenu();
            this.Update_EditMenu();
            this.Update_TabControl();
            this.Update_StatusLabel();
        }
        
        /// <summary>
        /// Adds the byte that has just been typed into Differences
        /// </summary>
        void Core_ROMChanged(Object sender, EventArgs e)
        {
            for (Int32 i = 0; i < this.Differences.Count; i++)
            {
                if (this.Differences[i].Item1 == this.CurrentAddress)
                {
                    this.Differences.RemoveAt(i);
                }
            }
            this.Differences.Add(Tuple.Create(this.CurrentAddress, this.MainHexBox.ByteProvider.ReadByte((Int32)this.CurrentAddress)));
        }
        /// <summary>
        /// Prompts the user about chenging the filesize of the ROM
        /// </summary>
        void Core_ROMSizeChanged(Object sender, EventArgs e)
        {
            if (Prompt.ChangeROMSize() == DialogResult.No)
            {

            }
        }
        /// <summary>
        /// An event handler that updates the menustrip
        /// </summary>
        void Core_UpdateMenu(Object sender, EventArgs e)
        {
            this.Update_FileMenu();
            this.Update_EditMenu();
        }
        /// <summary>
        /// Enables drag & drop
        /// </summary>
        void Core_DragEnter(Object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        /// <summary>
        /// Processes a file drop
        /// </summary>
        void Core_DragDrop(Object sender, DragEventArgs e)
        {
            Object fileNameData = e.Data.GetData(DataFormats.FileDrop);
            String[] fileNames = (String[])fileNameData;
            if (fileNames.Length == 1)
            {
                this.Core_OpenFile(fileNames[0]);
            }
        }

        /// <summary>
        /// Creates a new TabPage for the given file
        /// </summary>
        void Core_CreateTabPage(FileByteProvider file, String fileName)
        {
            TabPage tabpage = new TabPage();
            tabpage.Name = "FileTabPage" + this.FileTabPages.Count;
            tabpage.Text = fileName;
            tabpage.Padding = new Padding(3);
            tabpage.Size = this.MainTabPage.Size;

            HexBox hexbox = new HexBox();
            hexbox.ByteProvider = file;
            hexbox.Name = "MainHexBox";
            hexbox.LineInfoVisible = true;
            hexbox.ColumnInfoVisible = true;
            hexbox.StringViewVisible = true;
            hexbox.VScrollBarVisible = true;
            hexbox.UseFixedBytesPerLine = true;
            hexbox.Location = new Point(-2, 0);
            hexbox.Size = this.MainHexBox.Size;
            hexbox.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            hexbox.BorderStyle = BorderStyle.None;
            hexbox.TabIndex = 0;
            hexbox.SelectionStartChanged +=  new System.EventHandler(this.HexBox_SelectionStartChanged);
            hexbox.SelectionLengthChanged += new System.EventHandler(this.HexBox_SelectionLengthChanged);
            hexbox.CurrentLineChanged +=     new System.EventHandler(this.HexBox_Position_Changed);
            hexbox.CurrentPositionInLineChanged +=  new EventHandler(this.HexBox_Position_Changed);
            hexbox.Copied +=                 new System.EventHandler(this.HexBox_Copy);
            hexbox.CopiedHex +=              new System.EventHandler(this.HexBox_CopyHex);

            tabpage.Controls.Add(hexbox);

            this.FilePaths.Add(fileName);
            this.FileTabPages.Add(tabpage);
            this.FileHexBoxes.Add(hexbox);

            this.Tabs_Control.Controls.Add(tabpage);
            this.Tabs_Control.SelectTab(this.Tabs_Control.TabCount - 1);
            hexbox.Focus();
        }
        /// <summary>
        /// Closes the tab page at the given index
        /// </summary>
        /// <param name="index"></param>
        void Core_CloseTabPage(Int32 index)
        {
            this.Tabs_Control.TabPages.Remove(this.FileTabPages[index]);
            this.Tabs_Control.Controls.Remove(this.FileTabPages[index]);
            this.Tabs_Control.Invalidate();
            this.FileHexBoxes[index].Dispose();
            this.FileTabPages[index].Dispose();
            this.FileHexBoxes.RemoveAt(index);
            this.FileTabPages.RemoveAt(index);
            this.FilePaths.RemoveAt(index);
        }

        /// <summary>
        /// Opens a FormFind dialog window.
        /// </summary>
        void Core_ShowFind()
        {
            if (this.Tool_Find == null || this.Tool_Find.IsDisposed)
            {
                this.Tool_Find = new HexFind();

                if (this.CurrentTabIsROM()) this.Tool_Find.HexEditSource = this.MainHexBox;
                else this.Tool_Find.HexEditSource = this.FileHexBoxes[this.CurrentTab];

                this.Tool_Find.Core_OnOpen();
                this.Tool_Find.Show(this);
            }
            else
            {
                this.Tool_Find.Focus();
            }
        }
        /// <summary>
        /// Opens a FormGoTo dialog window.
        /// </summary>
        void Core_ShowGoTo()
        {
            HexBox hexbox = this.CurrentTabIsROM() ?
                this.MainHexBox :
                this.FileHexBoxes[this.CurrentTab];

            this.Tool_GoTo = new HexGoTo();
            this.Tool_GoTo.SetMaxByteIndex(hexbox.ByteProvider.Length);
            this.Tool_GoTo.SetDefaultValue(hexbox.SelectionStart);

            if (this.Tool_GoTo.ShowDialog() == DialogResult.OK)
            {
                hexbox.Select(this.Tool_GoTo.GetByteIndex(), 1);
                hexbox.Focus();
            }
        }
        /// <summary>
        /// Shows the bits for the current byte
        /// </summary>
        /// <returns>A string that is like "Bits of byte n° XXX : XXXXXXXX"</returns>
        String Core_ShowBits()
        {
            String result = "";

            Byte? currentByte = null;
            IByteProvider currentFile = this.CurrentTabIsROM() ?
                this.MainHexBox.ByteProvider :
                this.FileHexBoxes[this.CurrentTab].ByteProvider;
            UInt32 selection = this.CurrentAddress;

            if (selection < currentFile.Length)
            {
                currentByte = currentFile.ReadByte(selection);
            }

            BitInfo bitInfo = (currentByte == null) ? null :
                new BitInfo((Byte)currentByte, selection);

            if (bitInfo != null)
            {
                Byte currentByteNotNull = (Byte)currentByte;
                result = "Bits: " + bitInfo.ToString();
            }

            return result;
        }
        /// <summary>
        /// Puts together the bytes in Differences with chaining addresses
        /// </summary>
        List<Tuple<Pointer, Byte[]>> Core_MergeDifferences()
        {
            this.Differences.Sort(delegate (Tuple<Pointer, Byte> first, Tuple<Pointer, Byte> second)
            {
                return (first.Item1 - second.Item1);
            }); // first we sort the differences (they were in chronological order, now they're sorted by offset)

            Int32 chain;
            Pointer address;
            List<Tuple<Pointer, Byte[]>> data = new List<Tuple<Pointer, Byte[]>>();
            Byte[] buffer;
            for (Int32 i = 0; i < this.Differences.Count; i++)
            {   // then we put adjacent bytes together so as to make byte arrays
                chain = 1;
                while (((i + 1) < this.Differences.Count) &&
                    (this.Differences[i].Item1 + 1 == this.Differences[i + 1].Item1))
                {
                    i++;
                    chain++;
                }
                address = this.Differences[(i - chain) + 1].Item1;
                buffer = new Byte[chain];
                UI.ShowMessage("address: " + address + ", buffer: " + chain);
                for (Int32 index = 0; index < chain; index++)
                {
                    buffer[index] = this.Differences[(i - chain) + index + 1].Item2;
                }
                data.Add(Tuple.Create(address, buffer));
            }
            return data;
        }



        /// <summary>
        /// Manages enabling or disabling of menu items and toolstrip buttons.
        /// </summary>
        void Update_FileMenu()
        {
            IByteProvider hexbox = this.CurrentTabIsROM() ?
                this.MainHexBox.ByteProvider :
                this.FileHexBoxes[this.CurrentTab].ByteProvider;

            if (hexbox.HasChanges()) this.Menu_File_Save.Enabled = true;
            else this.Menu_File_Save.Enabled = false;

            this.Menu_File_Close.Enabled = !this.CurrentTabIsROM();

            String romfile = this.CurrentTabIsROM() ? "ROM" : "File";
            this.Menu_File_Save.Text = "Save " + romfile + "...";
            this.Menu_File_SaveAs.Text = "Save " + romfile + " As...";
        }
        /// <summary>
        /// Manages enabling or disabling of menustrip items and toolstrip buttons for copy and paste
        /// </summary>
        void Update_EditMenu()
        {
            HexBox hexbox = this.CurrentTabIsROM() ?
                this.MainHexBox :
                this.FileHexBoxes[this.CurrentTab];

            this.Menu_Edit_Cut.Enabled = hexbox.CanCut();
            this.Menu_Edit_Copy.Enabled =
            this.Menu_Edit_CopyHex.Enabled = hexbox.CanCopy();
            this.Menu_Edit_Paste.Enabled = hexbox.CanPaste();
            this.Menu_Edit_PasteHex.Enabled = hexbox.CanPasteHex();
        }
        /// <summary>
        /// Displays the file name in the TabPage´s text property
        /// </summary>
        /// <param name="fileName">the file name to display</param>
        void Update_TabControl()
        {
            String fileName;
            String readOnly;
            String changed;

            for (Int32 i = 0; i < this.FilePaths.Count; i++)
            {
                fileName = Path.GetFileName(this.FilePaths[i]);
                readOnly = ((FileByteProvider)this.FileHexBoxes[i].ByteProvider).ReadOnly ? " [Read-only]" : "";
                changed = ((FileByteProvider)this.FileHexBoxes[i].ByteProvider).HasChanges() ? " *" : "";

                this.FileTabPages[i].Text = fileName + changed + readOnly;
            }
            changed = ((DataByteProvider)this.MainHexBox.ByteProvider).HasChanges() ? " *" : "";

            this.MainTabPage.Text = "Loaded ROM" + changed;
        }
        /// <summary>
        /// Updates the File size status label
        /// </summary>
        void Update_StatusLabel()
        {
            HexBox hexbox = this.CurrentTabIsROM() ?
                this.MainHexBox :
                this.FileHexBoxes[this.CurrentTab];
            Pointer address = new Pointer((UInt32)hexbox.SelectionStart);
            UInt32 length = (UInt32)hexbox.SelectionLength;
            this.Status_File.Text = this.App.ROM.FileName + " - " + Util.GetDisplayBytes(hexbox.ByteProvider.Length) +
                " | Path: " + this.App.ROM.FilePath;
            this.Status_Position.Text = "Address: " + address + " | " + (length == 0 ? "No selection" : ("Selected: 0x" + Util.IntToHex(length)));
            this.Status_Bits.Text = this.Core_ShowBits();
            UInt16 uint16 = (UInt16)(Core.ReadByte(address) |
                (Core.ReadByte(address + 1) << 8));
            UInt32 uint32 = (UInt32)(Core.ReadByte(address) |
                (Core.ReadByte(address + 1) << 8) |
                (Core.ReadByte(address + 2) << 16) |
                (Core.ReadByte(address + 3) << 24));
            this.Status_Ints.Text = (address > Core.CurrentROMSize - 4) ? "" : (
                "UInt16: " + Util.UInt16ToHex(uint16) + " (" + uint16 + ")" + " | " +
                "UInt32: " + Util.UInt32ToHex(uint32) + " (" + uint32 + ")"
            );
        }



        void HexBox_Copy(Object sender, EventArgs e)
        {
            this.Update_EditMenu();
        }
        void HexBox_CopyHex(Object sender, EventArgs e)
        {
            this.Update_EditMenu();
        }

        void HexBox_Position_Changed(Object sender, EventArgs e)
        {
            this.Update_StatusLabel();
        }
        void HexBox_SelectionStartChanged(Object sender, EventArgs e)
        {
            this.Update_EditMenu();
            this.Update_StatusLabel();
        }
        void HexBox_SelectionLengthChanged(Object sender, EventArgs e)
        {
            this.Update_EditMenu();
            this.Update_StatusLabel();
        }

        void HexBox_ByteProvider_Changed(Object sender, EventArgs e)
        {
            this.Update_FileMenu();
            this.Update_EditMenu();
            this.Update_TabControl();
            this.Update_StatusLabel();
        }
        void HexBox_ByteProvider_LengthChanged(Object sender, EventArgs e)
        {
            this.Update_StatusLabel();
        }


        
        void File_Open_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*|ROMs (*.gba)|*.gba|Hacks (*.mhf)|*.mhf";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Core_OpenFile(openFileDialog.FileName);
            }
            this.Update_FileMenu();
            this.Update_EditMenu();
            this.Update_TabControl();
            this.Update_StatusLabel();
        }
        void File_Recent_Click(Object sender, ToolStripItemClickedEventArgs e)
        {
            RecentFileMenuItem menu_item = (RecentFileMenuItem)e.ClickedItem;
            this.Core_OpenFile(menu_item.FileName);
        }
        void File_Save_Click(Object sender, EventArgs e)
        {
            if (this.CurrentTabIsROM()) this.Core_SaveFile();
            else this.Core_SaveFile(this.CurrentTab);

            this.Update_FileMenu();
            this.Update_EditMenu();
            this.Update_TabControl();
            this.Update_StatusLabel();
        }
        void File_SaveAs_Click(Object sender, EventArgs e)
        {
            if (this.CurrentTabIsROM()) this.Core_MakeFile();
            else this.Core_MakeFile(this.CurrentTab);
        }
        void File_Apply_Click(Object sender, EventArgs e)
        {
            List<Tuple<Pointer, Byte[]>> data = this.Core_MergeDifferences();
            for (Int32 i = 0; i < data.Count; i++)
            {
                Core.WriteData(this, data[i].Item1, data[i].Item2, "Apply n°" + i);
            }
            this.Differences = new List<Tuple<Pointer, Byte>>();
        }
        void File_Close_Click(Object sender, EventArgs e)
        {
            try
            {
                if (this.FileHexBoxes[this.CurrentTab].ByteProvider.HasChanges())
                {
                    DialogResult answer = Prompt.SaveHexChanges();

                    if (answer == DialogResult.Yes)
                    {
                        this.Core_SaveFile(this.CurrentTab);
                    }
                    if (answer != DialogResult.Cancel)
                    {
                        this.Core_ExitFile(this.CurrentTab);
                    }
                }
                else
                {
                    this.Core_ExitFile(this.CurrentTab);
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error during the closing operation.", ex);
            }
        }

        void Edit_Cut_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];

            hexbox.Cut();
        }
        void Edit_Copy_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];

            hexbox.Copy();
        }
        void Edit_Paste_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];

            hexbox.Paste();
        }
        void Edit_CopyHex_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];

            hexbox.CopyHex();
        }
        void Edit_PasteHex_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];

            hexbox.PasteHex();
        }
        void Edit_SelectAll_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = this.CurrentTabIsROM() ? this.MainHexBox : this.FileHexBoxes[this.CurrentTab];

            hexbox.SelectAll();
        }

        void Tool_Find_Click(Object sender, EventArgs e)
        {
            this.Core_ShowFind();
        }
        void Tool_FindNext_Click(Object sender, EventArgs e)
        {
            this.Core_ShowFind();

            this.Tool_Find.Core_FindNext();
        }
        void Tool_GoTo_Click(Object sender, EventArgs e)
        {
            this.Core_ShowGoTo();
        }

        private void MagicButton_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = this.CurrentTabIsROM() ?
                this.MainHexBox :
                this.FileHexBoxes[this.CurrentTab];
            BasicEditor editor = new BasicEditor();
            this.App.Core_OpenEditor(editor);

            Pointer address = new Pointer((UInt32)hexbox.SelectionStart);
            Int32 length = (Int32)hexbox.SelectionLength;
            editor.Core_SetEntry(address, length, (length > 0 ? Core.ReadData(address, length) : null));
        }
    }
}