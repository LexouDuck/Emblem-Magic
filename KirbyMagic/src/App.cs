using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Compression;
using KirbyMagic.Kirby;
using KirbyMagic.Editors;
using KirbyMagic.Properties;
using GBA;
using Magic;
using Magic.Components;
using Magic.Editors;



namespace KirbyMagic
{
    public partial class App : Form, IApp
    {
        public Magic.Components.RecentFileMenu File_RecentFiles { get; set; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Undo { get; set; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Redo { get; set; }

        public String AppName { get; set; }
        public Version AppVersion { get; set; }

        /// <summary>
        /// Is responsible for all reading/writing of data, and the IO for the ROM file.
        /// </summary>
        public DataManager ROM { get; set; }
        /// <summary>
        /// The HackManager does the IO for the MHF file, and its submanagers record all relevant hack information.
        /// </summary>
        public HackManager MHF { get; set; }

        /// <summary>
        /// Describes which fire emblem game is open, whether or not it's a clean ROM, and such.
        /// </summary>
        Kirby.Game currentROM;
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

            AppName = software_name;
            AppVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

            ROM = new DataManager(this);
            MHF = new HackManager(this);
            Editors = new List<Editor>();

            InitializeComponent();
            
            FormClosing += File_Exit_Click;
            File_Exit.Click += File_Exit_Click;

            Core_Menu_Lockdown();

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
            if (Suspend) return;

            Update_FileMenu();
            Update_EditMenu();
            Update_InfoTab();
            Update_Editors();
        }
        
        public void Update_FileMenu()
        {
            File_SaveROM.Enabled = (ROM.FilePath == null || ROM.FilePath.Length == 0) ? false : ROM.WasChanged;
            File_SaveMHF.Enabled = (MHF.FilePath == null || MHF.FilePath.Length == 0) ? false : MHF.Changed;
        }
        public void Update_EditMenu()
        {
            if (ROM.UndoList.Count > 0)
            {
                Edit_Undo.Enabled = true;
                Edit_Undo.Text = "Undo " + ROM.UndoList[ROM.UndoList.Count - 1].Action.ToString();
            }
            else
            {
                Edit_Undo.Enabled = false;
                Edit_Undo.Text = "Undo";
            }

            if (ROM.RedoList.Count > 0)
            {
                Edit_Redo.Enabled = true;
                Edit_Redo.Text = "Redo " + ROM.RedoList[ROM.RedoList.Count - 1].Action.ToString();
            }
            else
            {
                Edit_Redo.Enabled = false;
                Edit_Redo.Text = "Redo";
            }
        }
        public void Update_AutoSaveMHF()
        {
            if (File_AutoSaveMHF.Selected)
            {
                File_SaveMHF_Click(null, new EventArgs());
            }
        }
        public void Update_AutoSaveROM()
        {
            if (File_AutoSaveROM.Selected)
            {
                File_SaveROM_Click(null, new EventArgs());
            }
        }
        public void Update_Editors()
        {
            Update_AutoSaveMHF();
            Update_AutoSaveROM();

            foreach (Editor editor in Editors)
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
            Tabs_Info_ROM_FileSize.Text = Util.GetDisplayBytes(ROM.FileSize);
            Tabs_Info_ROM_FilePath.Text = ROM.FilePath;
            Tabs_Info_MHF_Name.Text     = MHF.HackName;
            Tabs_Info_MHF_Author.Text   = MHF.HackAuthor;
            Tabs_Info_MHF_FileInfo.Text = MHF.Write.History.Count + " writes in this file.\n";
            Tabs_Info_MHF_FilePath.Text = MHF.FilePath;
            StatusLabel.Text =
                (Game == null ? "" : Game.ToString()) + " - " +
                (ROM.IsClean ? "Clean" : "Hacked") + " ROM";
        }

        /// <summary>
        /// Sets this.CurrentROM, which stores fire emblem game specific data, like vanilla pointers
        /// </summary>
        void Core_LoadGame()
        {
            try
            {
                Game = Kirby.Game.FromROM();
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
            Suite_Tabs.Visible = true;
            Suite_Tabs.SelectedIndex = 0;

            File_OpenMHF.Enabled = true;
            File_SaveAsMHF.Enabled = true;
            File_SaveAsROM.Enabled = true;
            File_CloseROM.Enabled = true;
            File_Export.Enabled = true;

            Edit_OpenProperties.Enabled = true;

            Tool_ExpandROM.Enabled = true;
            Tool_MarkSpace.Enabled = true;
            Tool_GetPointer.Enabled = true;
            Tool_OpenWriteEditor.Enabled = true;
            Tool_OpenSpaceEditor.Enabled = true;
            Tool_OpenPointEditor.Enabled = true;

            Core_Update();
        }
        /// <summary>
        /// Disables most menu buttons and stuff, is called when you decide to close a ROM
        /// </summary>
        void Core_Menu_Lockdown()
        {
            Suite_Tabs.Visible = false;

            StatusLabel.Text = null;

            File_OpenMHF.Enabled = false;
            File_SaveMHF.Enabled = false;
            File_SaveROM.Enabled = false;
            File_SaveAsMHF.Enabled = false;
            File_SaveAsROM.Enabled = false;
            File_CloseROM.Enabled = false;
            File_Export.Enabled = false;

            Edit_Undo.Enabled = false;
            Edit_Redo.Enabled = false;
            Edit_OpenProperties.Enabled = false;

            Tool_ExpandROM.Enabled = false;
            Tool_MarkSpace.Enabled = false;
            Tool_GetPointer.Enabled = false;
            Tool_OpenWriteEditor.Enabled = false;
            Tool_OpenSpaceEditor.Enabled = false;
            Tool_OpenPointEditor.Enabled = false;
        }

        void Core_ResetDataManager()
        {
            ROM = new DataManager(this);
            foreach (Editor editor in Editors)
            {
                editor.Dispose();
            }
            Editors.Clear();

            Core_Update();
        }
        void Core_ResetHackManager()
        {
            MHF = new HackManager(this);

            Core_Update();
        }

        /// <summary>
        /// Checks the differences between the open ROM and a clean ROM to generate an MHF file
        /// </summary>
        Boolean Core_CheckHackedROM()
        {
            if (ROM.IsClean) return false;
            String same_filename = ROM.FilePath.Remove(ROM.FilePath.Length - 4) + ".mhf";
            if (File.Exists(same_filename))
            {
                try
                {
                    MHF.OpenFile(same_filename);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message != "ABORT")
                        UI.ShowError("Could not open the MHF hack file.", ex);
                    Core_ResetHackManager();
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
                    if (Core_ExitMHFFile())
                    {
                        try
                        {
                            MHF.OpenFile(openWindow.FileName);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message != "ABORT")
                                UI.ShowError("Could not open the MHF hack file.", ex);
                            Core_ResetHackManager();
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
                    if (ROM.FileData[i] == cleanROM[i])
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
                                write_data.Add(ROM.FileData[i - sequence]);
                                sequence--;
                            }
                        }
                        write_data.Add(ROM.FileData[i]);
                        sequence = 0;
                    }
                }
                if (write_data.Count > 0)
                { 
                    writes.Add(new Write("Unknown", address, write_data.ToArray()));
                    write_data.Clear();
                }
                address = new Pointer((UInt32)cleanROM.Length);
                for (UInt32 i = address; i < ROM.FileSize; i++)
                {
                    write_data.Add(ROM.FileData[i]);
                }
                if (write_data.Count > 0)
                {
                    writes.Add(new Write("Expanded", address, write_data.ToArray()));
                    write_data.Clear();
                }
                MHF.Write.History = writes;
            }
            return false;
        }
        /// <summary>
        /// Checks if any of the major game asset pointers have been repointed
        /// </summary>
        void Core_CheckPointers()
        {
            if (ROM.IsClean)
            {
                MHF.Space.Load(Game.FreeSpace);
                MHF.Point.Load(Game.GetDefaultPointers());
                
                MHF.Changed = false;
            }
            else
            {
                Repoint[] pointers = Game.GetDefaultPointers();
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
                MHF.Point.Load(pointers);
            }
        }
        /// <summary>
        /// Checks the differences between the open ROM file and MHF file and resolves them.
        /// </summary>
        void Core_CheckDataHackDifferences()
        {
            List<Write> writes = new List<Write>();
            Byte[] buffer;
            foreach (Write write in MHF.Write.History)
            {
                buffer = ROM.Read(write.Address, write.Data.Length);

                if (!buffer.SequenceEqual(write.Data))
                {
                    writes.Add(write);
                }
            }    // Create a list of differing writes between the MHF and open ROM

            if (writes.Count != 0 &&
                Prompt.ApplyWritesToROM(MHF.Write.History.Count, writes.Count) == DialogResult.Yes)
            {
                foreach (Write write in writes)
                {
                    ROM.Write(UserAction.Cancel, write, null);
                }
            }
            else
            {
                Core_ResetHackManager();
            }
        }

        public void Core_OpenEditor(Editor editor)
        {
            if (editor.IsDisposed) return;

            editor.Icon = Icon;
            this.Editors.Add(editor);
            editor.Identifier = editor.Text.Substring(0, Math.Min(editor.Text.Length, HackManager.LENGTH_Write_Author));
            editor.Show();
        }
        public void Core_ExitEditor(Editor editor)
        {
            Editors.Remove(editor);
            editor.Dispose();
        }
        public void Core_ExitEditor(Int32 index)
        {
            Core_ExitEditor(Editors[index]);
        }

        void Core_OpenROMFile(String path)
        {
            try
            {
                ROM.OpenFile(path);
                Core_LoadGame();

                if (MHF.IsEmpty)
                {
                    if (Core_CheckHackedROM())
                        goto Continue;
                }
                else Core_CheckDataHackDifferences();

                if (MHF.IsEmpty) Core_CheckPointers();

                Continue:
                File_RecentFiles.AddFile(path);
                Core_Menu_Activate();
                Core_Update();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not open the ROM file.", ex);
                Core_ResetDataManager();
            }
        }
        void Core_OpenMHFFile(String path)
        {
            try
            {
                MHF.OpenFile(path);

                Core_CheckDataHackDifferences();
                Core_CheckHackedROM();
                Core_CheckPointers();
                
                Core_Update();
            }
            catch (Exception ex)
            {
                if (ex.Message != "ABORT")
                    UI.ShowError("Could not open the MHF hack file.", ex);
                Core_ResetHackManager();
            }
        }
        public void Core_SaveROMFile(String path)
        {
            try
            {
                MHF.Write.Update_ROMSaved();

                ROM.SaveFile(path);

                Core_Update();
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
                MHF.Write.Update_MHFSaved();

                MHF.SaveFile(path);

                Core_Update();
            }
            catch (IOException ex)
            {
                UI.ShowError("MHF Hack file could not be saved properly.", ex);
            }
        }
        Boolean Core_ExitROMFile()
        {
            DialogResult answer = ROM.WasChanged ?
                Prompt.SaveROMChanges() : DialogResult.No;
            if (answer == DialogResult.Cancel) return false;
            else
            {
                if (answer == DialogResult.Yes)
                {
                    Core_SaveROMFile(ROM.FilePath);
                }
                for (Int32 i = 0; i < Editors.Count; i++)
                {
                    Editors[i].Close();
                }
                this.Core_ResetDataManager();
                this.Core_ResetHackManager();
                this.Core_Menu_Lockdown();
                return true;
            }
        }
        Boolean Core_ExitMHFFile()
        {
            DialogResult answer = MHF.Changed ?
                Prompt.SaveMHFChanges() : DialogResult.No;
            if (answer == DialogResult.Cancel) return false;
            else
            {
                if (answer == DialogResult.Yes)
                {
                    Core_SaveMHFFile(MHF.FilePath);
                }
                for (Int32 i = 0; i < Editors.Count; i++)
                {
                    Editors[i].Close();
                }
                this.Core_ResetHackManager();
                return true;
            }
        }

        void Core_UserWrite(Write write)
        {
            MHF.Write.Add(write);
            ROM.Write(UserAction.Write, write, null);
            MHF.Space.MarkSpace("USED", write.Address, write.Address + write.Data.Length);
        }
        void Core_UserOverwrite(Write write, List<WriteConflict> conflict)
        {
            MHF.Write.Add(write);
            ROM.Write(UserAction.Overwrite, write, conflict);
            MHF.Space.MarkSpace("USED", write.Address, write.Address + write.Data.Length);
        }
        void Core_UserRestore(Pointer address, Int32 length)
        {
            List<WriteConflict> conflict = MHF.Write.Delete(address, address + length);
            ROM.Restore(address, length, conflict);
            // not too sure about the space unmarking on data restore
            MHF.Space.UnmarkSpace(address, address + length);
        }

        public void Core_UserAction(UserAction action, Write write)
        {
            lock (Locked)
            {
                switch (action)
                {
                    case UserAction.Write:
                        List<WriteConflict> conflict = MHF.Write.Check(write.Address, write.Address + write.Data.Length);

                        if (conflict == null || conflict.Count == 0)
                        {
                            Core_UserWrite(write);
                        }
                        else if (conflict.Count == 1)
                        {
                            if (Settings.Default.WarnOverwrite ||
                               (Settings.Default.WarnOverwriteSizeDiffer &&
                                write.Data.Length != conflict[0].Write.Data.Length))
                            {
                                if (Prompt.WriteConflict(conflict) == DialogResult.No) return;
                            }
                            Core_UserOverwrite(write, conflict);
                        }
                        else
                        {
                            DialogResult[] answers = new DialogResult[conflict.Count];
                            if (Settings.Default.WarnOverwriteSeveral)
                            {
                                if (Prompt.WriteConflict(conflict) == DialogResult.No) return;
                            }
                            Core_UserOverwrite(write, conflict);
                        }
                        break;
                    case UserAction.Overwrite:
                        Core_UserOverwrite(write, MHF.Write.Check(write.Address, write.Address + write.Data.Length));
                        break;
                    case UserAction.Restore:
                        Core_UserRestore(write.Address, write.Data.Length);
                        break;
                    case UserAction.Cancel:
                        UI.ShowError("Action was cancelled before it started..?");
                        break;
                }

                Core_Update();
            }
        }

        public void Core_Undo()
        {
            if (ROM.UndoList.Count > 0)
            {
                Core_UndoAction(ROM.UndoList.Count - 1);
            }
        }
        public void Core_UndoAction(Int32 index)
        {
            lock (Locked)
            {
                Write write = ROM.UndoList[index].Associated;

                if (ROM.UndoList.Count == 0)
                {
                    ROM.WasChanged = false;
                    MHF.Changed = false;
                }

                switch (ROM.UndoList[index].Action)
                {
                    case UserAction.Write:
                        MHF.Write.Delete(write);
                        ROM.UndoAction(index);
                        break;
                    case UserAction.Overwrite:
                        MHF.Write.Delete(write);
                        foreach (WriteConflict conflict in ROM.UndoList[index].Conflicts)
                        {
                            MHF.Write.Add(conflict.Write);
                        }
                        ROM.UndoAction(index);
                        break;
                    case UserAction.Restore:
                        foreach (WriteConflict conflict in ROM.UndoList[index].Conflicts)
                        {
                            MHF.Write.Add(conflict.Write);
                        }
                        ROM.UndoAction(index);
                        break;
                    case UserAction.Cancel:
                        break; // cancels are undone instantly - this runs right after core_useraction
                }

                Core_Update();
            }
        }
        public void Core_Redo()
        {
            if (ROM.RedoList.Count > 0)
            {
                Core_RedoAction(ROM.RedoList.Count - 1);
            }
        }
        public void Core_RedoAction(Int32 index)
        {
            lock (Locked)
            {
                Write write = ROM.RedoList[index].Associated;

                ROM.WasChanged = true;
                MHF.Changed = true;

                switch (ROM.RedoList[index].Action)
                {
                    case UserAction.Write:
                        MHF.Write.Add(write);
                        ROM.RedoAction(index);
                        break;
                    case UserAction.Overwrite:
                        MHF.Write.Add(write);
                        ROM.RedoAction(index);
                        break;
                    case UserAction.Restore:
                        MHF.Write.Delete(write.Address, write.Address + write.Data.Length);
                        ROM.RedoAction(index);
                        break;
                    case UserAction.Cancel:
                        UI.ShowError("This was an empty action you just redid..?");
                        break;
                }

                Core_Update();
            }
        }

        public void Core_OpenOptions()
        {
            new FormOptions(this).ShowDialog();
        }
        public void Core_OpenProperties()
        {
            new FormProperties(this).ShowDialog();

            Update_InfoTab();
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
                true, 0, (Int32)ROM.FileSize);
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
            if (MHF.Write.History.Count == 0)
            {
                UI.ShowMessage("No writes have been made to this ROM file.");
            }
            else
            {
                Write write = MHF.Write.History[MHF.Write.History.Count - 1];
                UI.ShowMessage("Address: " + write.Address + "\n\n" +
                    "Pointer: " + Util.BytesToSpacedHex(write.Address.ToBytes()));
            }
        }
        public void Core_CheckROMIdentifier(String identifier)
        {
            if (identifier.Length != 4)
                throw new Exception("Given ROM identifier is invalid, should have 4 chars (" + identifier + ")");
            if (identifier.StartsWith("K"))
            {
                GameRegion version;
                switch (identifier[3])
                {
                    case 'J': version = GameRegion.JAP; break;
                    case 'U': version = GameRegion.USA; break;
                    case 'E': version = GameRegion.EUR; break;
                    default: throw new Exception("MHF file describes an invalid game region (" + identifier + ")");
                }
                if (Game.Region != version)
                    throw new Exception("Loaded ROM has invalid version (" + identifier + ")");
                if (false) { }
                else if (identifier.Substring(1, 2).Equals("ND", StringComparison.Ordinal)) return;
                else if (identifier.Substring(1, 2).Equals("AM", StringComparison.Ordinal)) return;
                else throw new Exception("MHF file describes an invalid Kirby game (" + identifier + ")");

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
                if (Core_ExitROMFile())
                {
                    Core_OpenROMFile(openWindow.FileName);
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
                if (Core_ExitMHFFile())
                {
                    Core_OpenMHFFile(openWindow.FileName);
                }
            }
        }
        void File_SaveROM_Click(Object sender, EventArgs e)
        {
            if (ROM.FilePath == null)
            {
                UI.ShowError("There is no file path set for the ROM file.");
            }
            else
            {
                Core_SaveROMFile(ROM.FilePath);
            }
        }
        void File_SaveMHF_Click(Object sender, EventArgs e)
        {
            if (MHF.FilePath == null)
            {
                UI.ShowError("There is no file path set for the MHF file.");
            }
            else
            {
                Core_SaveMHFFile(MHF.FilePath);
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
                Core_SaveROMFile(saveWindow.FileName);
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
                Core_SaveMHFFile(saveWindow.FileName);
            }
        }
        void File_RecentFiles_Click(Object sender, ToolStripItemClickedEventArgs e)
        {
            Menu_File.HideDropDown();

            RecentFileMenuItem menu_item = (RecentFileMenuItem)e.ClickedItem;

            if (menu_item.Text.EndsWith(".gba") || menu_item.Text.EndsWith(".GBA"))
            {
                if (Core_ExitROMFile())
                {
                    Core_OpenROMFile(menu_item.FileName);
                }
            }
            else if (menu_item.Text.EndsWith(".mhf") || menu_item.Text.EndsWith(".MHF"))
            {
                if (Core_ExitMHFFile())
                {
                    Core_OpenMHFFile(menu_item.FileName);
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
                    Game.FileSize,
                    Game.Checksum,
                    this.ROM.FileData,
                    Core.CurrentROMSize,
                    Core.CurrentROMChecksum);
            }
        }
        void File_CloseROM_Click(Object sender, EventArgs e)
        {
            Core_ExitROMFile();
        }
        void File_Exit_Click(Object sender, EventArgs e)
        {
            if (Core_ExitMHFFile() && Core_ExitROMFile())
            {
                Settings.Default.Save();

                Application.Exit();
            }
            else if (e is FormClosingEventArgs)
                ((FormClosingEventArgs)e).Cancel = true;
        }

        void Edit_Undo_Click(Object sender, EventArgs e)
        {
            Core_Undo();
        }
        void Edit_Redo_Click(Object sender, EventArgs e)
        {
            Core_Redo();
        }
        void Edit_OpenProperties_Click(Object sender, EventArgs e)
        {
            Core_OpenProperties();
        }
        void Edit_Options_Click(Object sender, EventArgs e)
        {
            Core_OpenOptions();
        }

        void Tool_ExpandROM_Click(Object sender, EventArgs e)
        {
            Int32 answer = Prompt.ShowNumberDialog(
                "Please enter the desired ROM file size\r\n(amount of bytes, in hexadecimal).",
                "Resize ROM", true, 0, DataManager.ROM_MAX_SIZE);

            ROM.Resize(answer, MHF.Space);
            Core_Update();
        }
        void Tool_MarkSpace_Click(Object sender, EventArgs e)
        {
            Core_MarkSpace();
        }
        void Tool_GetFreeSpace_Click(Object sender, EventArgs e)
        {
            Core_GetFreeSpace();
        }
        void Tool_GetLastWrite_Click(Object sender, EventArgs e)
        {
            Core_GetLastWrite();
        }

        void Tool_OpenSpace_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new SpaceEditor(this));
        }
        void Tool_OpenWrite_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new WriteEditor(this));
        }
        void Tool_OpenPoint_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new PointEditor(this));
        }



        void Open_BasicEditor_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new BasicEditor(this));
        }
        void Open_HexEditor_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new HexEditor(this));
        }
        void Open_PatchEditor_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new PatchEditor(this));
        }
        void Open_ASMEditor_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new ASMEditor(this));
        }
        void Open_ModuleEditor_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new ModuleEditor(this));
        }
        void Open_GraphicsEditor_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new GraphicsEditor(this));
        }

        void Open_EventEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new EventEditor(this));
        }
        void Open_WorldMapEditor_Click(Object sender, EventArgs e)
        {
            //if (CurrentROM == null) return;
            //else if (CurrentROM is FireEmblem.FE6) Core_OpenEditor(new WorldMapEditor_FE6(this));
            //else if (CurrentROM is FireEmblem.FE7) Core_OpenEditor(new WorldMapEditor_FE7(this));
            //else if (CurrentROM is FireEmblem.FE8) Core_OpenEditor(new WorldMapEditor_FE8(this));
        }
        void Open_MapTilesetEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new MapTilesetEditor(this));
        }
        void Open_MapEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new MapEditor(this));
        }
        void Open_PortraitEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new PortraitEditor(this));
        }
        void Open_MapSpriteEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new MapSpriteEditor(this));
        }
        void Open_BattleScreenEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new BattleScreenEditor(this));
        }
        void Open_BattleAnimEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new BattleAnimEditor(this));
        }
        void Open_SpellAnimEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new SpellAnimEditor(this));
        }
        void Open_BackgroundEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new BackgroundEditor(this));
        }
        void Open_TextEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new TextEditor(this));
        }
        void Open_MusicEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new MusicEditor(this));
        }
        void Open_ItemEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new ItemEditor(this));
        }
        void Open_MenuEditor_Click(Object sender, EventArgs e)
        {
            //Core_OpenEditor(new MenuEditor(this));
        }
        void Open_TitleScreenEditor_Click(Object sender, EventArgs e)
        {
            Core_OpenEditor(new TitleScreenEditor(this));
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
            FormAbout dialog = new FormAbout(AppName,
                "(version " + AppVersion + ")"
                 + "\n\n" + "The GBA Kirby all-in-one editing tool"
                 + "\n\n" + "by (Lexou Duck)"
                 + "\n" + "Thanks to documentation and code written by:"
                 + "\n" + "TODO" // TODO
                 ,
                "Kirby " + '\u00A9' + " is property of Nintendo."
                 + "\n" + "Kirby Magic is in no way affiliated with Nintendo or HAL Laboratories."
                 + "\n\n" + "This software is free and open source, following the GNU General Public License."
                 ,
                this.Icon,
                Resources.Icon_Large);

            dialog.Show();
        }
    }
}
