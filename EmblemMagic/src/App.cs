using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Compression;
using EmblemMagic.FireEmblem;
using EmblemMagic.Editors;
using EmblemMagic.Properties;
using GBA;
using Magic;
using Magic.Components;
using Magic.Editors;



namespace EmblemMagic
{
    public partial class App : Form, IApp
    {
        public Magic.Components.RecentFileMenu File_RecentFiles { get; set; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Undo { get; set; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Redo { get; set; }

        public String AppName { get; set; }
        public Version AppVersion { get; set; }

        /// <summary>
        /// The DataManager: is responsible for all reading/writing of data, and the IO for the ROM file.
        /// </summary>
        public DataManager ROM { get; set; }
        /// <summary>
        /// The HackManager: does the IO for the MHF file, and its submanagers record all relevant hack information.
        /// </summary>
        public HackManager MHF { get; set; }

        /// <summary>
        /// Describes which fire emblem game is open, whether or not it's a clean ROM, and such.
        /// </summary>
        public IGame Game { get; set; }

        /// <summary>
        /// When true, all Emblem Magic windows should update normally
        /// </summary>
        public Boolean Suspend { get; set; }

        /// <summary>
        /// This list contains the instances of all the currently open Editor forms.
        /// </summary>
        public List<Editor> Editors { get; }



        private readonly Object Locked = new Object();



        public App(String software_name)
        {
            Core.App = this;
            UI.App = this;

            this.AppName = software_name;

            this.ROM = new DataManager(this);
            this.MHF = new HackManager(this);
            this.Editors = new List<Editor>();

            this.InitializeComponent();
            
            FormClosing += this.File_Exit_Click;
            this.File_Exit.Click += this.File_Exit_Click;

            this.Core_Menu_Lockdown();

            #if (!DEBUG)
                Open_MenuEditor.Enabled = false;
                Open_SpellAnimEditor.Enabled = false;
            #endif
            Core.SetDefaultPaths();
        }
        
        /// <summary>
        /// Updates the main window and all open editors
        /// </summary>
        public void Core_Update()
        {
            if (this.Suspend) return;

            this.Update_FileMenu();
            this.Update_EditMenu();
            this.Update_InfoTab();
            this.Update_Editors();
        }
        
        public void Update_FileMenu()
        {
            this.File_SaveROM.Enabled = (this.ROM.FilePath == null || this.ROM.FilePath.Length == 0) ? false : this.ROM.WasChanged;
            this.File_SaveMHF.Enabled = (this.MHF.FilePath == null || this.MHF.FilePath.Length == 0) ? false : this.MHF.Changed;
        }
        public void Update_EditMenu()
        {
            if (this.ROM.UndoList.Count > 0)
            {
                this.Edit_Undo.Enabled = true;
                this.Edit_Undo.Text = "Undo " + this.ROM.UndoList[this.ROM.UndoList.Count - 1].Action.ToString();
            }
            else
            {
                this.Edit_Undo.Enabled = false;
                this.Edit_Undo.Text = "Undo";
            }

            if (this.ROM.RedoList.Count > 0)
            {
                this.Edit_Redo.Enabled = true;
                this.Edit_Redo.Text = "Redo " + this.ROM.RedoList[this.ROM.RedoList.Count - 1].Action.ToString();
            }
            else
            {
                this.Edit_Redo.Enabled = false;
                this.Edit_Redo.Text = "Redo";
            }
        }
        public void Update_AutoSaveMHF()
        {
            if (this.File_AutoSaveMHF.Selected)
            {
                this.File_SaveMHF_Click(null, new EventArgs());
            }
        }
        public void Update_AutoSaveROM()
        {
            if (this.File_AutoSaveROM.Selected)
            {
                this.File_SaveROM_Click(null, new EventArgs());
            }
        }
        public void Update_Editors()
        {
            this.Update_AutoSaveMHF();
            this.Update_AutoSaveROM();

            foreach (Editor editor in this.Editors)
            {
                try
                {
                    editor.Core_Update();
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been an error while trying to update the " + editor.Text, ex);
                }
            }
        }
        public void Update_InfoTab()
        {
            this.Tabs_Info_ROM_FileSize.Text = Util.GetDisplayBytes(this.ROM.FileSize);
            this.Tabs_Info_ROM_FilePath.Text = this.ROM.FilePath;
            this.Tabs_Info_MHF_Name.Text = this.MHF.HackName;
            this.Tabs_Info_MHF_Author.Text = this.MHF.HackAuthor;
            this.Tabs_Info_MHF_FileInfo.Text = this.MHF.Write.History.Count + " writes in this file.\n";
            this.Tabs_Info_MHF_FilePath.Text = this.MHF.FilePath;
            this.StatusLabel.Text =
                (this.Game == null ? "" : this.Game.ToString()) + " - " +
                (this.ROM.IsClean ? "Clean" : "Hacked") + " ROM";
        }

        /// <summary>
        /// Sets this.CurrentROM, which stores fire emblem game specific data, like vanilla pointers
        /// </summary>
        void Core_LoadGame()
        {
            try
            {
                this.Game = FireEmblem.Game.FromROM();
                this.ROM.WasExpanded = (this.ROM.FileSize == this.Game.FileSize);
                this.ROM.IsClean = (CRC32.GetChecksum(this.ROM.FileData) == this.Game.Checksum);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been a problem while trying to identify the ROM.", ex);
            }
        }
        /// <summary>
        /// Initializes the environment and menu buttons, is called right after a rom is opened
        /// </summary>
        void Core_Menu_Activate()
        {
            this.Suite_Tabs.Visible = true;
            this.Suite_Tabs.SelectedIndex = 0;

            this.File_OpenMHF.Enabled = true;
            this.File_SaveAsMHF.Enabled = true;
            this.File_SaveAsROM.Enabled = true;
            this.File_CloseROM.Enabled = true;
            this.File_Export.Enabled = true;

            this.Edit_OpenProperties.Enabled = true;

            this.Tool_ExpandROM.Enabled = true;
            this.Tool_MarkSpace.Enabled = true;
            this.Tool_GetPointer.Enabled = true;
            this.Tool_OpenWriteEditor.Enabled = true;
            this.Tool_OpenSpaceEditor.Enabled = true;
            this.Tool_OpenPointEditor.Enabled = true;

            this.Core_Update();
        }
        /// <summary>
        /// Disables most menu buttons and stuff, is called when you decide to close a ROM
        /// </summary>
        void Core_Menu_Lockdown()
        {
            this.Suite_Tabs.Visible = false;

            this.StatusLabel.Text = null;

            this.File_OpenMHF.Enabled = false;
            this.File_SaveMHF.Enabled = false;
            this.File_SaveROM.Enabled = false;
            this.File_SaveAsMHF.Enabled = false;
            this.File_SaveAsROM.Enabled = false;
            this.File_CloseROM.Enabled = false;
            this.File_Export.Enabled = false;

            this.Edit_Undo.Enabled = false;
            this.Edit_Redo.Enabled = false;
            this.Edit_OpenProperties.Enabled = false;

            this.Tool_ExpandROM.Enabled = false;
            this.Tool_MarkSpace.Enabled = false;
            this.Tool_GetPointer.Enabled = false;
            this.Tool_OpenWriteEditor.Enabled = false;
            this.Tool_OpenSpaceEditor.Enabled = false;
            this.Tool_OpenPointEditor.Enabled = false;
        }

        void Core_ResetDataManager()
        {
            this.ROM = new DataManager(this);
            foreach (Editor editor in this.Editors)
            {
                editor.Dispose();
            }
            this.Editors.Clear();

            this.Core_Update();
        }
        void Core_ResetHackManager()
        {
            this.MHF = new HackManager(this);

            this.Core_Update();
        }

        /// <summary>
        /// Checks the differences between the open ROM and a clean ROM to generate an MHF file
        /// </summary>
        Boolean Core_CheckHackedROM()
        {
            if (this.ROM.IsClean) return false;
            String same_filename = this.ROM.FilePath.Remove(this.ROM.FilePath.Length - 4) + ".mhf";
            if (File.Exists(same_filename))
            {
                try
                {
                    this.MHF.OpenFile(same_filename);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message != "ABORT")
                        UI.ShowError("Could not open the MHF hack file.", ex);
                    this.Core_ResetHackManager();
                }
            }
            if (Prompt.AskForMHFForHackedROM() == DialogResult.Yes)
            {
                OpenFileDialog openWindow = new OpenFileDialog();
                openWindow.Filter = "Fire Emblem Hack files (*.mhf)|*.mhf|All files (*.*)|*.*";
                openWindow.FilterIndex = 1;
                openWindow.RestoreDirectory = true;
                openWindow.Multiselect = false;

                if (openWindow.ShowDialog() == DialogResult.OK)
                {
                    if (this.Core_ExitMHFFile())
                    {
                        try
                        {
                            this.MHF.OpenFile(openWindow.FileName);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message != "ABORT")
                                UI.ShowError("Could not open the MHF hack file.", ex);
                            this.Core_ResetHackManager();
                        }
                    }
                }
            }
            else
            {
                Byte[] cleanROM = File.ReadAllBytes(Core.Path_CleanROM);
                List<Write> writes = new List<Write>();
                List<Byte> write_data = new List<Byte>();
                Pointer address = new Pointer();
                const Int32 LENGTH = 16; // any two byte differences less than 16 bytes apart will be made into a single write
                Int32 sequence = 0;
                for (UInt32 i = 0; i < cleanROM.Length; i++)
                {
                    if (this.ROM.FileData[i] == cleanROM[i])
                    {
                        sequence++;
                    }
                    else
                    {
                        if (write_data.Count > 0)
                        {
                            if (sequence >= LENGTH)
                            {
                                address = new Pointer((UInt32)(i - sequence - write_data.Count));
                                writes.Add(new Write("Unknown", address, write_data.ToArray()));
                                write_data.Clear();
                            }
                            else while (sequence >= 0)
                            {
                                write_data.Add(this.ROM.FileData[i - sequence]);
                                sequence--;
                            }
                        }
                        write_data.Add(this.ROM.FileData[i]);
                        sequence = 0;
                    }
                }
                if (write_data.Count > 0)
                { 
                    writes.Add(new Write("Unknown", address, write_data.ToArray()));
                    write_data.Clear();
                }
                address = new Pointer((UInt32)cleanROM.Length);
                for (UInt32 i = address; i < this.ROM.FileSize; i++)
                {
                    write_data.Add(this.ROM.FileData[i]);
                }
                if (write_data.Count > 0)
                {
                    writes.Add(new Write("Expanded", address, write_data.ToArray()));
                    write_data.Clear();
                }
                this.MHF.Write.History = writes;
            }
            return false;
        }
        /// <summary>
        /// Checks if any of the major game asset pointers have been repointed
        /// </summary>
        void Core_CheckPointers()
        {
            if (this.ROM.IsClean)
            {
                this.MHF.Space.Load(this.Game.FreeSpace);
                this.MHF.Point.Load(this.Game.GetDefaultPointers());

                this.MHF.Changed = false;
            }
            else
            {
                Repoint[] pointers = this.Game.GetDefaultPointers();
                List<Repoint> unreferenced = new List<Repoint>();
                for (Int32 i = 0; i < pointers.Length; i++)
                {
                    if (pointers[i].References.Length == 0)
                        unreferenced.Add(pointers[i]);
                }
                if (unreferenced.Count > 0)
                {
                    Prompt.ResolveUnreferencedPointers(unreferenced);
                }
                //MHF.Space.Load(CurrentROM.DefaultFreeSpace());
                this.MHF.Point.Load(pointers);
            }
        }
        /// <summary>
        /// Checks the differences between the open ROM file and MHF file and resolves them.
        /// </summary>
        void Core_CheckDataHackDifferences()
        {
            List<Write> writes = new List<Write>();
            Byte[] buffer;
            foreach (Write write in this.MHF.Write.History)
            {
                buffer = this.ROM.Read(write.Address, write.Data.Length);

                if (!buffer.SequenceEqual(write.Data))
                {
                    writes.Add(write);
                }
            }    // Create a list of differing writes between the MHF and open ROM

            if (writes.Count != 0 &&
                Prompt.ApplyWritesToROM(this.MHF.Write.History.Count, writes.Count) == DialogResult.Yes)
            {
                foreach (Write write in writes)
                {
                    this.ROM.Write(UserAction.Cancel, write, null);
                }
            }
            else
            {
                this.Core_ResetHackManager();
            }
        }

        public void Core_OpenEditor(Editor editor)
        {
            if (editor.IsDisposed) return;

            editor.Icon = this.Icon;
            this.Editors.Add(editor);
            editor.Identifier = editor.Text.Substring(0, Math.Min(editor.Text.Length, HackManager.LENGTH_Write_Author));
            editor.Show();
        }
        public void Core_ExitEditor(Editor editor)
        {
            this.Editors.Remove(editor);
            editor.Dispose();
        }
        public void Core_ExitEditor(Int32 index)
        {
            this.Core_ExitEditor(this.Editors[index]);
        }

        void Core_OpenROMFile(String path)
        {
            try
            {
                this.ROM.OpenFile(path);
                this.Core_LoadGame();

                if (this.MHF.IsEmpty)
                {
                    if (this.Core_CheckHackedROM())
                        goto Continue;
                }
                else this.Core_CheckDataHackDifferences();

                if (this.MHF.IsEmpty) this.Core_CheckPointers();

                Continue:
                this.File_RecentFiles.AddFile(path);
                this.Core_Menu_Activate();
                this.Core_Update();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not open the ROM file.", ex);
                this.Core_ResetDataManager();
            }
        }
        void Core_OpenMHFFile(String path)
        {
            try
            {
                this.MHF.OpenFile(path);

                this.Core_CheckDataHackDifferences();
                this.Core_CheckHackedROM();
                this.Core_CheckPointers();

                this.Core_Update();
            }
            catch (Exception ex)
            {
                if (ex.Message != "ABORT")
                    UI.ShowError("Could not open the MHF hack file.", ex);
                this.Core_ResetHackManager();
            }
        }
        public void Core_SaveROMFile(String path)
        {
            try
            {
                this.MHF.Write.Update_ROMSaved();

                this.ROM.SaveFile(path);

                this.Core_Update();
            }
            catch (IOException ex)
            {
                UI.ShowError("The ROM file could not be saved properly.", ex);
            }
        }
        public void Core_SaveMHFFile(String path)
        {
            try
            {
                this.MHF.Write.Update_MHFSaved();

                this.MHF.SaveFile(path);

                this.Core_Update();
            }
            catch (IOException ex)
            {
                UI.ShowError("MHF Hack file could not be saved properly.", ex);
            }
        }
        Boolean Core_ExitROMFile()
        {
            DialogResult answer = this.ROM.WasChanged ?
                Prompt.SaveROMChanges() : DialogResult.No;
            if (answer == DialogResult.Cancel) return false;
            else
            {
                if (answer == DialogResult.Yes)
                {
                    this.Core_SaveROMFile(this.ROM.FilePath);
                }
                for (Int32 i = 0; i < this.Editors.Count; i++)
                {
                    this.Editors[i].Close();
                }
                this.Core_ResetDataManager();
                this.Core_ResetHackManager();
                this.Core_Menu_Lockdown();
                return true;
            }
        }
        Boolean Core_ExitMHFFile()
        {
            DialogResult answer = this.MHF.Changed ?
                Prompt.SaveMHFChanges() : DialogResult.No;
            if (answer == DialogResult.Cancel) return false;
            else
            {
                if (answer == DialogResult.Yes)
                {
                    this.Core_SaveMHFFile(this.MHF.FilePath);
                }
                for (Int32 i = 0; i < this.Editors.Count; i++)
                {
                    this.Editors[i].Close();
                }
                this.Core_ResetHackManager();
                return true;
            }
        }

        void Core_UserWrite(Write write)
        {
            this.MHF.Write.Add(write);
            this.ROM.Write(UserAction.Write, write, null);
            this.MHF.Space.MarkSpace("USED", write.Address, write.Address + write.Data.Length);
        }
        void Core_UserOverwrite(Write write, List<WriteConflict> conflict)
        {
            this.MHF.Write.Add(write);
            this.ROM.Write(UserAction.Overwrite, write, conflict);
            this.MHF.Space.MarkSpace("USED", write.Address, write.Address + write.Data.Length);
        }
        void Core_UserRestore(Pointer address, Int32 length)
        {
            List<WriteConflict> conflict = this.MHF.Write.Delete(address, address + length);
            this.ROM.Restore(address, length, conflict);
            // not too sure about the space unmarking on data restore
            this.MHF.Space.UnmarkSpace(address, address + length);
        }

        public void Core_UserAction(UserAction action, Write write)
        {
            lock (this.Locked)
            {
                switch (action)
                {
                    case UserAction.Write:
                        List<WriteConflict> conflict = this.MHF.Write.Check(write.Address, write.Address + write.Data.Length);

                        if (conflict == null || conflict.Count == 0)
                        {
                            this.Core_UserWrite(write);
                        }
                        else if (conflict.Count == 1)
                        {
                            if (Settings.Default.WarnOverwrite ||
                               (Settings.Default.WarnOverwriteSizeDiffer &&
                                write.Data.Length != conflict[0].Write.Data.Length))
                            {
                                if (Prompt.WriteConflict(conflict) == DialogResult.No) return;
                            }
                            this.Core_UserOverwrite(write, conflict);
                        }
                        else
                        {
                            DialogResult[] answers = new DialogResult[conflict.Count];
                            if (Settings.Default.WarnOverwriteSeveral)
                            {
                                if (Prompt.WriteConflict(conflict) == DialogResult.No) return;
                            }
                            this.Core_UserOverwrite(write, conflict);
                        }
                        break;
                    case UserAction.Overwrite:
                        this.Core_UserOverwrite(write, this.MHF.Write.Check(write.Address, write.Address + write.Data.Length));
                        break;
                    case UserAction.Restore:
                        this.Core_UserRestore(write.Address, write.Data.Length);
                        break;
                    case UserAction.Cancel:
                        UI.ShowError("Action was cancelled before it started..?");
                        break;
                }

                this.Core_Update();
            }
        }

        public void Core_Undo()
        {
            if (this.ROM.UndoList.Count > 0)
            {
                this.Core_UndoAction(this.ROM.UndoList.Count - 1);
            }
        }
        public void Core_UndoAction(Int32 index)
        {
            lock (this.Locked)
            {
                Write write = this.ROM.UndoList[index].Associated;

                if (this.ROM.UndoList.Count == 0)
                {
                    this.ROM.WasChanged = false;
                    this.MHF.Changed = false;
                }

                switch (this.ROM.UndoList[index].Action)
                {
                    case UserAction.Write:
                        this.MHF.Write.Delete(write);
                        this.ROM.UndoAction(index);
                        break;
                    case UserAction.Overwrite:
                        this.MHF.Write.Delete(write);
                        foreach (WriteConflict conflict in this.ROM.UndoList[index].Conflicts)
                        {
                            this.MHF.Write.Add(conflict.Write);
                        }
                        this.ROM.UndoAction(index);
                        break;
                    case UserAction.Restore:
                        foreach (WriteConflict conflict in this.ROM.UndoList[index].Conflicts)
                        {
                            this.MHF.Write.Add(conflict.Write);
                        }
                        this.ROM.UndoAction(index);
                        break;
                    case UserAction.Cancel:
                        break; // cancels are undone instantly - this runs right after core_useraction
                }

                this.Core_Update();
            }
        }
        public void Core_Redo()
        {
            if (this.ROM.RedoList.Count > 0)
            {
                this.Core_RedoAction(this.ROM.RedoList.Count - 1);
            }
        }
        public void Core_RedoAction(Int32 index)
        {
            lock (this.Locked)
            {
                Write write = this.ROM.RedoList[index].Associated;

                this.ROM.WasChanged = true;
                this.MHF.Changed = true;

                switch (this.ROM.RedoList[index].Action)
                {
                    case UserAction.Write:
                        this.MHF.Write.Add(write);
                        this.ROM.RedoAction(index);
                        break;
                    case UserAction.Overwrite:
                        this.MHF.Write.Add(write);
                        this.ROM.RedoAction(index);
                        break;
                    case UserAction.Restore:
                        this.MHF.Write.Delete(write.Address, write.Address + write.Data.Length);
                        this.ROM.RedoAction(index);
                        break;
                    case UserAction.Cancel:
                        UI.ShowError("This was an empty action you just redid..?");
                        break;
                }

                this.Core_Update();
            }
        }

        public void Core_OpenOptions()
        {
            new FormOptions(this).ShowDialog();
        }
        public void Core_OpenProperties()
        {
            new FormProperties(this).ShowDialog();

            this.Update_InfoTab();
        }

        public void Core_MarkSpace()
        {
            MarkSpace dialog = new MarkSpace(this);

            dialog.Show();
        }
        public void Core_GetFreeSpace()
        {
            Int32 length = Prompt.ShowNumberDialog(
                "What length of free space is needed ?",
                "Request pointer to free space",
                true, 0, (Int32)this.ROM.FileSize);
            if (length == 0) return;

            Pointer address;
            try
            {
                address = Core.GetFreeSpace(length);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not find appropiate area of free space.", ex);
                return;
            }
            UI.ShowMessage("Address: " + address + "\n\n" +
                "Pointer: " + Util.BytesToSpacedHex(address.ToBytes()));
        }
        public void Core_GetLastWrite()
        {
            if (this.MHF.Write.History.Count == 0)
            {
                UI.ShowMessage("No writes have been made to this ROM file.");
            }
            else
            {
                Write write = this.MHF.Write.History[this.MHF.Write.History.Count - 1];
                UI.ShowMessage("Address: " + write.Address + "\n\n" +
                    "Pointer: " + Util.BytesToSpacedHex(write.Address.ToBytes()));
            }
        }
        public void Core_CheckROMIdentifier(String identifier)
        {
            if (identifier.StartsWith("FE"))
            {
                GameRegion version;
                switch (identifier[3])
                {
                    case 'J': version = GameRegion.JAP; break;
                    case 'U': version = GameRegion.USA; break;
                    case 'E': version = GameRegion.EUR; break;
                    default: throw new Exception("MHF file describes an invalid game version.");
                }
                switch (identifier[2])
                {
                    case '6': if (Core.App.Game is FE6 && Core.App.Game.Region == version) return; break;
                    case '7': if (Core.App.Game is FE7 && Core.App.Game.Region == version) return; break;
                    case '8': if (Core.App.Game is FE8 && Core.App.Game.Region == version) return; break;
                    default: throw new Exception("MHF file describes an FE game other than 6,7 or 8, oddly enough.");
                }

                if (Prompt.DifferentGameTypes() == DialogResult.No)
                {
                    throw new Exception("ABORT");
                }
            }
            else
            {
                throw new Exception("MHF file has an invalid identifier.");
            }
        }



        void File_OpenROM_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "GBA ROMs (*.gba)|*.gba|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (this.Core_ExitROMFile())
                {
                    this.Core_OpenROMFile(openWindow.FileName);
                }
            }
        }
        void File_OpenMHF_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "Fire Emblem Hack files (*.mhf)|*.mhf|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
                
            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (this.Core_ExitMHFFile())
                {
                    this.Core_OpenMHFFile(openWindow.FileName);
                }
            }
        }
        void File_SaveROM_Click(Object sender, EventArgs e)
        {
            if (this.ROM.FilePath == null)
            {
                UI.ShowError("There is no file path set for the ROM file.");
            }
            else
            {
                this.Core_SaveROMFile(this.ROM.FilePath);
            }
        }
        void File_SaveMHF_Click(Object sender, EventArgs e)
        {
            if (this.MHF.FilePath == null)
            {
                UI.ShowError("There is no file path set for the MHF file.");
            }
            else
            {
                this.Core_SaveMHFFile(this.MHF.FilePath);
            }
        }
        void File_SaveROMas_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.Filter = "GBA ROMs (*.gba)|*.gba|All files (*.*)|*.*";
            saveWindow.FilterIndex = 1;
            saveWindow.RestoreDirectory = true;
            saveWindow.CreatePrompt = true;
            saveWindow.OverwritePrompt = true;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_SaveROMFile(saveWindow.FileName);
            }
        }
        void File_SaveMHFas_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();

            saveWindow.Filter = "Fire Emblem Hack (*.mhf)|*.mhf|All files (*.*)|*.*";
            saveWindow.FilterIndex = 1;
            saveWindow.RestoreDirectory = true;
            saveWindow.CreatePrompt = true;
            saveWindow.OverwritePrompt = true;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_SaveMHFFile(saveWindow.FileName);
            }
        }
        void File_RecentFiles_Click(Object sender, ToolStripItemClickedEventArgs e)
        {
            this.Menu_File.HideDropDown();

            RecentFileMenuItem menu_item = (RecentFileMenuItem)e.ClickedItem;

            if (menu_item.Text.EndsWith(".gba") || menu_item.Text.EndsWith(".GBA"))
            {
                if (this.Core_ExitROMFile())
                {
                    this.Core_OpenROMFile(menu_item.FileName);
                }
            }
            else if (menu_item.Text.EndsWith(".mhf") || menu_item.Text.EndsWith(".MHF"))
            {
                if (this.Core_ExitMHFFile())
                {
                    this.Core_OpenMHFFile(menu_item.FileName);
                }
            }
            else UI.ShowError("Chosen file has invalid extension");
        }
        void File_ExportUPS_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();

            saveWindow.Filter = "UPS Patch (*.ups)|*.ups|All files (*.*)|*.*";
            saveWindow.FilterIndex = 1;
            saveWindow.RestoreDirectory = true;
            saveWindow.CreatePrompt = true;
            saveWindow.OverwritePrompt = true;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                UPS.WriteFile(saveWindow.FileName,
                    File.ReadAllBytes(Core.Path_CleanROM),
                    this.Game.FileSize,
                    this.Game.Checksum,
                    this.ROM.FileData,
                    Core.CurrentROMSize,
                    Core.CurrentROMChecksum);
            }
        }
        void File_CloseROM_Click(Object sender, EventArgs e)
        {
            this.Core_ExitROMFile();
        }
        void File_Exit_Click(Object sender, EventArgs e)
        {
            if (this.Core_ExitMHFFile() && this.Core_ExitROMFile())
            {
                Settings.Default.Save();

                Application.Exit();
            }
            else if (e is FormClosingEventArgs)
                ((FormClosingEventArgs)e).Cancel = true;
        }

        void Edit_Undo_Click(Object sender, EventArgs e)
        {
            this.Core_Undo();
        }
        void Edit_Redo_Click(Object sender, EventArgs e)
        {
            this.Core_Redo();
        }
        void Edit_OpenProperties_Click(Object sender, EventArgs e)
        {
            this.Core_OpenProperties();
        }
        void Edit_Options_Click(Object sender, EventArgs e)
        {
            this.Core_OpenOptions();
        }

        void Tool_ExpandROM_Click(Object sender, EventArgs e)
        {
            Int32 answer = Prompt.ShowNumberDialog(
                "Please enter the desired ROM file size\r\n(amount of bytes, in hexadecimal).",
                "Resize ROM", true, 0, DataManager.ROM_MAX_SIZE);

            this.ROM.Resize(answer, this.MHF.Space);
            this.Core_Update();
        }
        void Tool_MarkSpace_Click(Object sender, EventArgs e)
        {
            this.Core_MarkSpace();
        }
        void Tool_GetFreeSpace_Click(Object sender, EventArgs e)
        {
            this.Core_GetFreeSpace();
        }
        void Tool_GetLastWrite_Click(Object sender, EventArgs e)
        {
            this.Core_GetLastWrite();
        }

        void Tool_OpenSpace_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new SpaceEditor());
        }
        void Tool_OpenWrite_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new WriteEditor());
        }
        void Tool_OpenPoint_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new PointEditor());
        }



        void Open_BasicEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new BasicEditor());
        }
        void Open_HexEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new HexEditor());
        }
        void Open_PatchEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new PatchEditor());
        }
        void Open_ASMEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new ASMEditor());
        }
        void Open_ModuleEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new ModuleEditor());
        }
        void Open_EventEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new EventEditor());
        }
        void Open_WorldMapEditor_Click(Object sender, EventArgs e)
        {
            if (this.Game == null) return;
            else if (this.Game is FireEmblem.FE6) this.Core_OpenEditor(new WorldMapEditor_FE6());
            else if (this.Game is FireEmblem.FE7) this.Core_OpenEditor(new WorldMapEditor_FE7());
            else if (this.Game is FireEmblem.FE8) this.Core_OpenEditor(new WorldMapEditor_FE8());
        }
        void Open_MapTilesetEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new MapTilesetEditor());
        }
        void Open_MapEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new MapEditor());
        }
        void Open_GraphicsEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new GraphicsEditor());
        }
        void Open_PortraitEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new PortraitEditor());
        }
        void Open_MapSpriteEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new MapSpriteEditor());
        }
        void Open_BattleScreenEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new BattleScreenEditor());
        }
        void Open_BattleAnimEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new BattleAnimEditor());
        }
        void Open_SpellAnimEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new SpellAnimEditor());
        }
        void Open_BackgroundEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new BackgroundEditor());
        }
        void Open_TextEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new TextEditor());
        }
        void Open_MusicEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new MusicEditor());
        }
        void Open_ItemEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new ItemEditor());
        }
        void Open_MenuEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new MenuEditor());
        }
        void Open_TitleScreenEditor_Click(Object sender, EventArgs e)
        {
            this.Core_OpenEditor(new TitleScreenEditor());
        }

        void Help_Help_Click(Object sender, EventArgs e)
        {
            String path = "file://" + Path.Combine(
                    Directory.GetCurrentDirectory(),
                    this.AppName + ".chm");
            Help.ShowHelp(this, path);
        }
        void Help_About_Click(Object sender, EventArgs e)
        {
            FormAbout dialog = new FormAbout(this.AppName,
                "(version " + this.AppVersion + ")"
                 + "\n\n" + "The GBA Fire Emblem all-in-one editing tool"
                 + "\n\n" + "by (Lexou Duck)"
                 + '\n' + "Thanks to documentation and code written by:"
                 + '\n' + "(Hextator), (Nintenlord), (Zahlman), (CrazyColors)"
                 ,
                "Fire Emblem " + '\u00A9' + " is property of Nintendo."
                 + '\n' + "Emblem Magic is in no way affiliated with Nintendo or Intelligent Systems." + '\n'
                 + '\n' + "This software is free and open source, following the GNU General Public License."
                 ,
                this.Icon,
                Properties.Resources.icon_large);
            
            dialog.Show();
        }
    }
}
