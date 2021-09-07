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
                _current = Tabs_Control.SelectedIndex - 1;
                return _current;
            }
            set
            {
                _current = value;
                Tabs_Control.SelectTab(_current + 1);
                Tabs_Control.SelectedTab.Focus();
                Update_FileMenu();
                Update_EditMenu();
                Update_StatusLabel();
                return;
            }
        }
        Int32 _current;
        /// <summary>
        /// Returns true if the currently focused tab is that of the open ROM
        /// </summary>
        Boolean CurrentTabIsROM()
        {
            return (Tabs_Control.SelectedIndex == 0);
        }
        /// <summary>
        /// Gets or sets the address of the current cursor location
        /// </summary>
        Pointer CurrentAddress
        {
            get
            {
                HexBox hexbox = CurrentTabIsROM() ? MainHexBox : FileHexBoxes[CurrentTab];
                return new Pointer((UInt32)((hexbox.CurrentLine - 1) * hexbox.BytesPerLine + hexbox.CurrentPositionInLine));
            }
            set
            {
                HexBox hexbox = CurrentTabIsROM() ? MainHexBox : FileHexBoxes[CurrentTab];
                hexbox.Select((UInt32)value, 1);
                hexbox.Focus();
            }
        }

        HexFind Tool_Find;
        HexGoTo Tool_GoTo;

        List<Tuple<Pointer, Byte>> Differences;

        public HexEditor(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();

                Tabs_Control.TabIndexChanged += Core_UpdateMenu;

                FileHexBoxes = new List<HexBox>();
                FileTabPages = new List<TabPage>();
                FilePaths = new List<String>();

                Differences = new List<Tuple<Pointer, Byte>>();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        /// <summary>
        /// Initializes the hex editor´s main form
        /// </summary>
        public override void Core_OnOpen()
        {
            Core_LoadFile();

            Core_UpdateMenu(this, null);
        }
        /// <summary>
        /// Called whenever changes are made outside this editor
        /// </summary>
        public override void Core_Update()
        {
            Pointer current = CurrentAddress;

            Core_LoadFile();

            Core_UpdateMenu(this, null);

            CurrentAddress = current - 1;
        }
        /// <summary>
        /// Potentially prompts the user about closing files without saving changes
        /// </summary>
        public override void Core_OnExit(Object sender, FormClosingEventArgs e)
        {
            DialogResult answer;
            for (Int32 i = 0; i < FilePaths.Count; i++)
            {
                answer = Prompt.SaveHexChanges();
                CurrentTab = i;
                if (answer == DialogResult.Cancel) e.Cancel = true;
                else Core_ExitFile(i);
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
                rom = new DataByteProvider(App.ROM.FileData);
                rom.Changed += new EventHandler(HexBox_ByteProvider_Changed);
                rom.LengthChanged += new EventHandler(HexBox_ByteProvider_LengthChanged);
                rom.Changed += new EventHandler(Core_ROMChanged);
                rom.LengthChanged += new EventHandler(Core_ROMSizeChanged);

                MainHexBox.ByteProvider = rom;
                Differences = new List<Tuple<Pointer, Byte>>();
            }
            catch (Exception ex)
            {
                UI.ShowError("The attempt to load the ROM into the hex editor has failed.", ex);
                return;
            }
            Update_TabControl();
            Update_StatusLabel();
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
                    file.Changed += new EventHandler(HexBox_ByteProvider_Changed);
                    file.LengthChanged += new EventHandler(HexBox_ByteProvider_LengthChanged);
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

                Core_CreateTabPage(file, path);

                Menu_File_RecentFiles.AddFile(path);
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
                App.Core_SaveROMFile(App.ROM.FilePath);
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
                FileByteProvider file = FileHexBoxes[index].ByteProvider as FileByteProvider;
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
                    App.Core_SaveROMFile(SaveROM_Window.FileName);
                }
            }
        }
        /// <summary>
        /// Saves the file from the tab at the given index as a new file.
        /// </summary>
        void Core_MakeFile(Int32 index)
        {
            using (FileStream file = File.OpenWrite(FilePaths[index]))
            {
                Byte[] data = FileHexBoxes[index].Value;
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
            if (CurrentTabIsROM())
            {
                UI.ShowError("The Hex Editor tab of the current loaded ROM cannot be closed.");
            }
            else
            {
                ((IDisposable)FileHexBoxes[index].ByteProvider).Dispose();
                FileHexBoxes[index].ByteProvider = null;

                Core_CloseTabPage(index);

                CurrentTab = index;
            }

            Update_FileMenu();
            Update_EditMenu();
            Update_TabControl();
            Update_StatusLabel();
        }
        
        /// <summary>
        /// Adds the byte that has just been typed into Differences
        /// </summary>
        void Core_ROMChanged(Object sender, EventArgs e)
        {
            for (Int32 i = 0; i < Differences.Count; i++)
            {
                if (Differences[i].Item1 == CurrentAddress)
                {
                    Differences.RemoveAt(i);
                }
            }
            Differences.Add(Tuple.Create(CurrentAddress, MainHexBox.ByteProvider.ReadByte((Int32)CurrentAddress)));
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
            Update_FileMenu();
            Update_EditMenu();
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
                Core_OpenFile(fileNames[0]);
            }
        }

        /// <summary>
        /// Creates a new TabPage for the given file
        /// </summary>
        void Core_CreateTabPage(FileByteProvider file, String fileName)
        {
            TabPage tabpage = new TabPage();
            tabpage.Name = "FileTabPage" + FileTabPages.Count;
            tabpage.Text = fileName;
            tabpage.Padding = new Padding(3);
            tabpage.Size = MainTabPage.Size;

            HexBox hexbox = new HexBox();
            hexbox.ByteProvider = file;
            hexbox.Name = "MainHexBox";
            hexbox.LineInfoVisible = true;
            hexbox.ColumnInfoVisible = true;
            hexbox.StringViewVisible = true;
            hexbox.VScrollBarVisible = true;
            hexbox.UseFixedBytesPerLine = true;
            hexbox.Location = new Point(-2, 0);
            hexbox.Size = MainHexBox.Size;
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

            FilePaths.Add(fileName);
            FileTabPages.Add(tabpage);
            FileHexBoxes.Add(hexbox);

            Tabs_Control.Controls.Add(tabpage);
            Tabs_Control.SelectTab(Tabs_Control.TabCount - 1);
            hexbox.Focus();
        }
        /// <summary>
        /// Closes the tab page at the given index
        /// </summary>
        /// <param name="index"></param>
        void Core_CloseTabPage(Int32 index)
        {
            Tabs_Control.TabPages.Remove(FileTabPages[index]);
            Tabs_Control.Controls.Remove(FileTabPages[index]);
            Tabs_Control.Invalidate();
            FileHexBoxes[index].Dispose();
            FileTabPages[index].Dispose();
            FileHexBoxes.RemoveAt(index);
            FileTabPages.RemoveAt(index);
            FilePaths.RemoveAt(index);
        }

        /// <summary>
        /// Opens a FormFind dialog window.
        /// </summary>
        void Core_ShowFind()
        {
            if (Tool_Find == null || Tool_Find.IsDisposed)
            {
                Tool_Find = new HexFind();

                if (CurrentTabIsROM()) Tool_Find.HexEditSource = MainHexBox;
                else Tool_Find.HexEditSource = FileHexBoxes[CurrentTab];

                Tool_Find.Core_OnOpen();
                Tool_Find.Show(this);
            }
            else
            {
                Tool_Find.Focus();
            }
        }
        /// <summary>
        /// Opens a FormGoTo dialog window.
        /// </summary>
        void Core_ShowGoTo()
        {
            HexBox hexbox = CurrentTabIsROM() ?
                MainHexBox :
                FileHexBoxes[CurrentTab];

            Tool_GoTo = new HexGoTo();
            Tool_GoTo.SetMaxByteIndex(hexbox.ByteProvider.Length);
            Tool_GoTo.SetDefaultValue(hexbox.SelectionStart);

            if (Tool_GoTo.ShowDialog() == DialogResult.OK)
            {
                hexbox.Select(Tool_GoTo.GetByteIndex(), 1);
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
            IByteProvider currentFile = CurrentTabIsROM() ?
                MainHexBox.ByteProvider :
                FileHexBoxes[CurrentTab].ByteProvider;
            UInt32 selection = CurrentAddress;

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
            Differences.Sort(delegate (Tuple<Pointer, Byte> first, Tuple<Pointer, Byte> second)
            {
                return (first.Item1 - second.Item1);
            }); // first we sort the differences (they were in chronological order, now they're sorted by offset)

            Int32 chain;
            Pointer address;
            List<Tuple<Pointer, Byte[]>> data = new List<Tuple<Pointer, Byte[]>>();
            Byte[] buffer;
            for (Int32 i = 0; i < Differences.Count; i++)
            {   // then we put adjacent bytes together so as to make byte arrays
                chain = 1;
                while (((i + 1) < Differences.Count) &&
                    (Differences[i].Item1 + 1 == Differences[i + 1].Item1))
                {
                    i++;
                    chain++;
                }
                address = Differences[(i - chain) + 1].Item1;
                buffer = new Byte[chain];
                UI.ShowMessage("address: " + address + ", buffer: " + chain);
                for (Int32 index = 0; index < chain; index++)
                {
                    buffer[index] = Differences[(i - chain) + index + 1].Item2;
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
            IByteProvider hexbox = CurrentTabIsROM() ?
                MainHexBox.ByteProvider :
                FileHexBoxes[CurrentTab].ByteProvider;

            if (hexbox.HasChanges()) Menu_File_Save.Enabled = true;
            else Menu_File_Save.Enabled = false;
            
            Menu_File_Close.Enabled = !CurrentTabIsROM();

            String romfile = CurrentTabIsROM() ? "ROM" : "File";
            Menu_File_Save.Text = "Save " + romfile + "...";
            Menu_File_SaveAs.Text = "Save " + romfile + " As...";
        }
        /// <summary>
        /// Manages enabling or disabling of menustrip items and toolstrip buttons for copy and paste
        /// </summary>
        void Update_EditMenu()
        {
            HexBox hexbox = CurrentTabIsROM() ?
                MainHexBox :
                FileHexBoxes[CurrentTab];
            
            Menu_Edit_Cut.Enabled = hexbox.CanCut();
            Menu_Edit_Copy.Enabled =
            Menu_Edit_CopyHex.Enabled = hexbox.CanCopy();
            Menu_Edit_Paste.Enabled = hexbox.CanPaste();
            Menu_Edit_PasteHex.Enabled = hexbox.CanPasteHex();
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

            for (Int32 i = 0; i < FilePaths.Count; i++)
            {
                fileName = Path.GetFileName(FilePaths[i]);
                readOnly = ((FileByteProvider)FileHexBoxes[i].ByteProvider).ReadOnly ? " [Read-only]" : "";
                changed = ((FileByteProvider)FileHexBoxes[i].ByteProvider).HasChanges() ? " *" : "";

                FileTabPages[i].Text = fileName + changed + readOnly;
            }
            changed = ((DataByteProvider)MainHexBox.ByteProvider).HasChanges() ? " *" : "";

            MainTabPage.Text = "Loaded ROM" + changed;
        }
        /// <summary>
        /// Updates the File size status label
        /// </summary>
        void Update_StatusLabel()
        {
            HexBox hexbox =  CurrentTabIsROM() ?
                MainHexBox :
                FileHexBoxes[CurrentTab];
            Pointer address = new Pointer((UInt32)hexbox.SelectionStart);
            UInt32 length = (UInt32)hexbox.SelectionLength;
            Status_File.Text = App.ROM.FileName + " - " + Util.GetDisplayBytes(hexbox.ByteProvider.Length) +
                " | Path: " + App.ROM.FilePath;
            Status_Position.Text = "Address: " + address + " | " + (length == 0 ? "No selection" : ("Selected: 0x" + Util.IntToHex(length)));
            Status_Bits.Text = Core_ShowBits();
            UInt16 uint16 = (UInt16)(Core.ReadByte(address) |
                (Core.ReadByte(address + 1) << 8));
            UInt32 uint32 = (UInt32)(Core.ReadByte(address) |
                (Core.ReadByte(address + 1) << 8) |
                (Core.ReadByte(address + 2) << 16) |
                (Core.ReadByte(address + 3) << 24));
            Status_Ints.Text = (address > Core.CurrentROMSize - 4) ? "" : (
                "UInt16: " + Util.UInt16ToHex(uint16) + " (" + uint16 + ")" + " | " +
                "UInt32: " + Util.UInt32ToHex(uint32) + " (" + uint32 + ")"
            );
        }



        void HexBox_Copy(Object sender, EventArgs e)
        {
            Update_EditMenu();
        }
        void HexBox_CopyHex(Object sender, EventArgs e)
        {
            Update_EditMenu();
        }

        void HexBox_Position_Changed(Object sender, EventArgs e)
        {
            Update_StatusLabel();
        }
        void HexBox_SelectionStartChanged(Object sender, EventArgs e)
        {
            Update_EditMenu();
            Update_StatusLabel();
        }
        void HexBox_SelectionLengthChanged(Object sender, EventArgs e)
        {
            Update_EditMenu();
            Update_StatusLabel();
        }

        void HexBox_ByteProvider_Changed(Object sender, EventArgs e)
        {
            Update_FileMenu();
            Update_EditMenu();
            Update_TabControl();
            Update_StatusLabel();
        }
        void HexBox_ByteProvider_LengthChanged(Object sender, EventArgs e)
        {
            Update_StatusLabel();
        }


        
        void File_Open_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*|ROMs (*.gba)|*.gba|Hacks (*.feh)|*.feh";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Core_OpenFile(openFileDialog.FileName);
            }
            Update_FileMenu();
            Update_EditMenu();
            Update_TabControl();
            Update_StatusLabel();
        }
        void File_Recent_Click(Object sender, ToolStripItemClickedEventArgs e)
        {
            RecentFileMenuItem menu_item = (RecentFileMenuItem)e.ClickedItem;
            this.Core_OpenFile(menu_item.FileName);
        }
        void File_Save_Click(Object sender, EventArgs e)
        {
            if (CurrentTabIsROM()) Core_SaveFile();
            else Core_SaveFile(CurrentTab);
            
            Update_FileMenu();
            Update_EditMenu();
            Update_TabControl();
            Update_StatusLabel();
        }
        void File_SaveAs_Click(Object sender, EventArgs e)
        {
            if (CurrentTabIsROM()) Core_MakeFile();
            else Core_MakeFile(CurrentTab);
        }
        void File_Apply_Click(Object sender, EventArgs e)
        {
            List<Tuple<Pointer, Byte[]>> data = Core_MergeDifferences();
            for (Int32 i = 0; i < data.Count; i++)
            {
                Core.WriteData(this, data[i].Item1, data[i].Item2, "Apply n°" + i);
            }
            Differences = new List<Tuple<Pointer, Byte>>();
        }
        void File_Close_Click(Object sender, EventArgs e)
        {
            try
            {
                if (FileHexBoxes[CurrentTab].ByteProvider.HasChanges())
                {
                    DialogResult answer = Prompt.SaveHexChanges();

                    if (answer == DialogResult.Yes)
                    {
                        Core_SaveFile(CurrentTab);
                    }
                    if (answer != DialogResult.Cancel)
                    {
                        Core_ExitFile(CurrentTab);
                    }
                }
                else
                {
                    Core_ExitFile(CurrentTab);
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error during the closing operation.", ex);
            }
        }

        void Edit_Cut_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = CurrentTabIsROM() ?  MainHexBox : FileHexBoxes[CurrentTab];

            hexbox.Cut();
        }
        void Edit_Copy_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = CurrentTabIsROM() ? MainHexBox : FileHexBoxes[CurrentTab];

            hexbox.Copy();
        }
        void Edit_Paste_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = CurrentTabIsROM() ? MainHexBox : FileHexBoxes[CurrentTab];

            hexbox.Paste();
        }
        void Edit_CopyHex_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = CurrentTabIsROM() ? MainHexBox : FileHexBoxes[CurrentTab];

            hexbox.CopyHex();
        }
        void Edit_PasteHex_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = CurrentTabIsROM() ? MainHexBox : FileHexBoxes[CurrentTab];

            hexbox.PasteHex();
        }
        void Edit_SelectAll_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = CurrentTabIsROM() ? MainHexBox : FileHexBoxes[CurrentTab];

            hexbox.SelectAll();
        }

        void Tool_Find_Click(Object sender, EventArgs e)
        {
            Core_ShowFind();
        }
        void Tool_FindNext_Click(Object sender, EventArgs e)
        {
            Core_ShowFind();

            Tool_Find.Core_FindNext();
        }
        void Tool_GoTo_Click(Object sender, EventArgs e)
        {
            Core_ShowGoTo();
        }

        private void MagicButton_Click(Object sender, EventArgs e)
        {
            HexBox hexbox = CurrentTabIsROM() ?
                MainHexBox :
                FileHexBoxes[CurrentTab];
            BasicEditor editor = new BasicEditor(App);
            App.Core_OpenEditor(editor);

            Pointer address = new Pointer((UInt32)hexbox.SelectionStart);
            Int32 length = (Int32)hexbox.SelectionLength;
            editor.Core_SetEntry(address, length, (length > 0 ? Core.ReadData(address, length) : null));
        }
    }
}