﻿using System;
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

/* TODO

    Features:
    - ??? Title Screen Editor ??? FINISH IT
    - ??? Menu Editor ???
    - Adding items to the item array ?
    - Control with pretty map sprites for weapon effectiveness, in item editor
    - ASM assembler
    - Music Player
    - Spell anims yo
    - Get european version default free space ranges for FE7 and FE8
    - CSV import/export for module editor
    - A "reorder palette without changing image" tool for the background editor
    - text-based tilemaps so users can change stuff
    - FE8 Large World Map: Road-editing, and a specific PaletteMap Editor
    - Text editor: have text bubble previewing follow bytecode text commands
    - Portrait Editor: CHR+PAL imagedata portrait saving
    - Event Editor: unit loading/movement on map
    - Event Editor: toggle to show AREA events on map (also villages, treasures, etc)
    - Event Editor: a listbox timeline of all the TURN events for this chapter
    - Battle Anim Editor: make an "export all" button
    - Battle Anim editor: augment the duration of the "wait for HP" frame on GIF export
    - Module Editor: drag-n-drop reordering of UI sections, see https://www.codeproject.com/Articles/48411/Using-the-FlowLayoutPanel-and-Reordering-with-Drag

    Bugfixes:
    - Make it so palettes ALWAYS have 16-byte writes, even if they're shorter than that
    - Clicking 'Cancel' when opening a ROM should actually cancel the file-open
    - For some people, the program suddenly stops working (the main window is permanently minimized) and they need to redownload
    - There are 2 prompts when copying in HexEditor, and deciding not to proceed still copies anyways..?
    - Set correct tab index for every control in each editor
    - When inserting a small world map into FE8, green covers the screen ingame (except when the camera is scrolled)..?
    - Hex Editor, copy-pasting takes actual bytes ?? 
    - ASM Editor, registers getting reset to 0 on bl instruction ? test on routine at 0x2C7C in FE8U
    - Text editor, how can i show japanese shift-JIS fonts properly
    - Map editor, doenst prompt to repoint on TMX or MAR insert
    - Cutscene screen insertion problem with pointers ..?
    - Battle Anim Editor, The in-editor CodeBox doesn't work all too well
    - When the error for not having the correct clean ROM is up, the program cant be closed
    - When opening a second ROM, moushover doc in the event editor doesn't work

    Keep in mind for release to check:
    - File_RecentFiles.Enabled field being set to 'false' in Suite.Designer.cs, go delete that line
    - MarkingComboBox "Datasource modified error" because of generated code in designer files, go delete that too
    - The automatically defined "DEBUG" constant (not defined when compiling as 'Release' build in visual studio), will take care of:
        - Main Suite window (this file): Disabling the "open" buttons for WIP/unfinished Editor windows
        - ./src/Editors/EventEditor.cs, line 95 or so, absolute folderpath changes to appropriate function call
    - Do a "Release" build (any CPU) in Visual studio
    - Run the ./dist.sh shell script (if you're on Windows, use Cygwin), to prepare the "dist" folder for release.
*/

namespace EmblemMagic
{
    public partial class App : Form, IApp
    {
        public Magic.Components.RecentFileMenu File_RecentFiles { get; set; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Undo { get; set; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Redo { get; set; }

        public String SoftwareName { get; set; }

        /// <summary>
        /// Is responsible for all reading/writing of data, and the IO for the ROM file.
        /// </summary>
        public DataManager ROM { get; set; }
        /// <summary>
        /// The HackManager does the IO for the FEH file, and its submanagers record all relevant hack information.
        /// </summary>
        public HackManager FEH { get; set; }

        /// <summary>
        /// Describes which fire emblem game is open, whether or not it's a clean ROM, and such.
        /// </summary>
        FireEmblem.Game currentROM;
        public IGame CurrentROM { get; set; }

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

            SoftwareName = software_name;

            ROM = new DataManager();
            FEH = new HackManager(this);
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
            File_SaveROM.Enabled = (ROM.FilePath == null || ROM.FilePath.Length == 0) ? false : ROM.Changed;
            File_SaveFEH.Enabled = (FEH.FilePath == null || FEH.FilePath.Length == 0) ? false : FEH.Changed;
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
        public void Update_AutoSaveFEH()
        {
            if (File_AutoSaveFEH.Selected)
            {
                File_SaveFEH_Click(null, new EventArgs());
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
            Update_AutoSaveFEH();
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
            Tabs_Info_FEH_Name.Text = FEH.HackName;
            Tabs_Info_FEH_Author.Text = FEH.HackAuthor;
            Tabs_Info_FEH_FileInfo.Text = FEH.Write.History.Count + " writes in this file.\n";
            Tabs_Info_FEH_FilePath.Text = FEH.FilePath;
            StatusLabel.Text = (CurrentROM == null) ? "" : CurrentROM.ToString();
        }

        /// <summary>
        /// Sets this.CurrentROM, which stores fire emblem game specific data, like vanilla pointers
        /// </summary>
        void Core_LoadFireEmblem()
        {
            try
            {
                CurrentROM = FireEmblem.Game.FromROM(ROM);
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

            File_OpenFEH.Enabled = true;
            File_SaveAsFEH.Enabled = true;
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

            File_OpenFEH.Enabled = false;
            File_SaveFEH.Enabled = false;
            File_SaveROM.Enabled = false;
            File_SaveAsFEH.Enabled = false;
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
            ROM = new DataManager();
            foreach (Editor editor in Editors)
            {
                editor.Dispose();
            }
            Editors.Clear();

            Core_Update();
        }
        void Core_ResetHackManager()
        {
            FEH = new HackManager(this);

            Core_Update();
        }

        /// <summary>
        /// Checks the differences between the open ROM and a clean ROM to generate an FEH file
        /// </summary>
        bool Core_CheckHackedROM()
        {
            if (CurrentROM.IsClean) return false;
            string same_filename = ROM.FilePath.Remove(ROM.FilePath.Length - 4) + ".feh";
            if (File.Exists(same_filename))
            {
                try
                {
                    FEH.OpenFile(same_filename);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message != "ABORT")
                        UI.ShowError("Could not open the FEH hack file.", ex);
                    Core_ResetHackManager();
                }
            }
            if (Prompt.AskForFEHForHackedROM() == DialogResult.Yes)
            {
                OpenFileDialog openWindow = new OpenFileDialog();
                openWindow.Filter = "Fire Emblem Hack files (*.feh)|*.feh|All files (*.*)|*.*";
                openWindow.FilterIndex = 1;
                openWindow.RestoreDirectory = true;
                openWindow.Multiselect = false;

                if (openWindow.ShowDialog() == DialogResult.OK)
                {
                    if (Core_ExitFEHFile())
                    {
                        try
                        {
                            FEH.OpenFile(openWindow.FileName);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message != "ABORT")
                                UI.ShowError("Could not open the FEH hack file.", ex);
                            Core_ResetHackManager();
                        }
                    }
                }
            }
            else
            {
                byte[] cleanROM = File.ReadAllBytes(Core.Path_CleanROM);
                List<Write> writes = new List<Write>();
                List<byte> write_data = new List<byte>();
                Pointer address = new Pointer();
                const int LENGTH = 16; // any two byte differences less than 16 bytes apart will be made into a single write
                int sequence = 0;
                for (uint i = 0; i < cleanROM.Length; i++)
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
                                address = new Pointer((uint)(i - sequence - write_data.Count));
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
                address = new Pointer((uint)cleanROM.Length);
                for (uint i = address; i < ROM.FileSize; i++)
                {
                    write_data.Add(ROM.FileData[i]);
                }
                if (write_data.Count > 0)
                {
                    writes.Add(new Write("Expanded", address, write_data.ToArray()));
                    write_data.Clear();
                }
                FEH.Write.History = writes;
            }
            return false;
        }
        /// <summary>
        /// Checks if any of the major game asset pointers have been repointed
        /// </summary>
        void Core_CheckPointers()
        {
            if (CurrentROM.IsClean)
            {
                FEH.Space.Load(CurrentROM.GetDefaultFreeSpace());
                FEH.Point.Load(CurrentROM.GetDefaultPointers());
                
                FEH.Changed = false;
            }
            else
            {
                Repoint[] pointers = CurrentROM.GetDefaultPointers();
                List<Repoint> unreferenced = new List<Repoint>();
                for (int i = 0; i < pointers.Length; i++)
                {
                    if (pointers[i].References.Length == 0)
                        unreferenced.Add(pointers[i]);
                }
                if (unreferenced.Count > 0)
                {
                    Prompt.ResolveUnreferencedPointers(unreferenced);
                }
                //FEH.Space.Load(CurrentROM.DefaultFreeSpace());
                FEH.Point.Load(pointers);
            }
        }
        /// <summary>
        /// Checks the differences between the open ROM file and FEH file and resolves them.
        /// </summary>
        void Core_CheckDataHackDifferences()
        {
            List<Write> writes = new List<Write>();
            byte[] buffer;
            foreach (Write write in FEH.Write.History)
            {
                buffer = ROM.Read(write.Address, write.Data.Length);

                if (!buffer.SequenceEqual(write.Data))
                {
                    writes.Add(write);
                }
            }    // Create a list of differing writes between the FEH and open ROM

            if (writes.Count != 0 &&
                Prompt.ApplyWritesToROM(FEH.Write.History.Count, writes.Count) == DialogResult.Yes)
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
        public void Core_ExitEditor(int index)
        {
            Core_ExitEditor(Editors[index]);
        }

        void Core_OpenROMFile(string path)
        {
            try
            {
                ROM.OpenFile(path);
                Core_LoadFireEmblem();

                if (FEH.IsEmpty)
                {
                    if (Core_CheckHackedROM())
                        goto Continue;
                }
                else Core_CheckDataHackDifferences();

                if (FEH.IsEmpty) Core_CheckPointers();

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
        void Core_OpenFEHFile(string path)
        {
            try
            {
                FEH.OpenFile(path);

                Core_CheckDataHackDifferences();
                Core_CheckHackedROM();
                Core_CheckPointers();
                
                Core_Update();
            }
            catch (Exception ex)
            {
                if (ex.Message != "ABORT")
                    UI.ShowError("Could not open the FEH hack file.", ex);
                Core_ResetHackManager();
            }
        }
        public void Core_SaveROMFile(string path)
        {
            try
            {
                FEH.Write.Update_ROMSaved();

                ROM.SaveFile(path);

                Core_Update();
            }
            catch (IOException ex)
            {
                UI.ShowError("The ROM file could not be saved properly.", ex);
            }
        }
        public void Core_SaveFEHFile(string path)
        {
            try
            {
                FEH.Write.Update_FEHSaved();

                FEH.SaveFile(path);

                Core_Update();
            }
            catch (IOException ex)
            {
                UI.ShowError("FEH Hack file could not be saved properly.", ex);
            }
        }
        bool Core_ExitROMFile()
        {
            DialogResult answer = ROM.Changed ?
                Prompt.SaveROMChanges() : DialogResult.No;
            if (answer == DialogResult.Cancel) return false;
            else
            {
                if (answer == DialogResult.Yes)
                {
                    Core_SaveROMFile(ROM.FilePath);
                }
                for (int i = 0; i < Editors.Count; i++)
                {
                    Editors[i].Close();
                }
                this.Core_ResetDataManager();
                this.Core_ResetHackManager();
                this.Core_Menu_Lockdown();
                return true;
            }
        }
        bool Core_ExitFEHFile()
        {
            DialogResult answer = FEH.Changed ?
                Prompt.SaveFEHChanges() : DialogResult.No;
            if (answer == DialogResult.Cancel) return false;
            else
            {
                if (answer == DialogResult.Yes)
                {
                    Core_SaveFEHFile(FEH.FilePath);
                }
                for (int i = 0; i < Editors.Count; i++)
                {
                    Editors[i].Close();
                }
                this.Core_ResetHackManager();
                return true;
            }
        }

        void Core_UserWrite(Write write)
        {
            FEH.Write.Add(write);
            ROM.Write(UserAction.Write, write, null);
            FEH.Space.MarkSpace("USED", write.Address, write.Address + write.Data.Length);
        }
        void Core_UserOverwrite(Write write, List<WriteConflict> conflict)
        {
            FEH.Write.Add(write);
            ROM.Write(UserAction.Overwrite, write, conflict);
            FEH.Space.MarkSpace("USED", write.Address, write.Address + write.Data.Length);
        }
        void Core_UserRestore(Pointer address, int length)
        {
            List<WriteConflict> conflict = FEH.Write.Delete(address, address + length);
            ROM.Restore(address, length, conflict);
            // not too sure about the space unmarking on data restore
            FEH.Space.UnmarkSpace(address, address + length);
        }

        public void Core_UserAction(UserAction action, Write write)
        {
            lock (Locked)
            {
                switch (action)
                {
                    case UserAction.Write:
                        List<WriteConflict> conflict = FEH.Write.Check(write.Address, write.Address + write.Data.Length);

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
                        Core_UserOverwrite(write, FEH.Write.Check(write.Address, write.Address + write.Data.Length));
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
        public void Core_UndoAction(int index)
        {
            lock (Locked)
            {
                Write write = ROM.UndoList[index].Associated;

                if (ROM.UndoList.Count == 0)
                {
                    ROM.Changed = false;
                    FEH.Changed = false;
                }

                switch (ROM.UndoList[index].Action)
                {
                    case UserAction.Write:
                        FEH.Write.Delete(write);
                        ROM.UndoAction(index);
                        break;
                    case UserAction.Overwrite:
                        FEH.Write.Delete(write);
                        foreach (WriteConflict conflict in ROM.UndoList[index].Conflicts)
                        {
                            FEH.Write.Add(conflict.Write);
                        }
                        ROM.UndoAction(index);
                        break;
                    case UserAction.Restore:
                        foreach (WriteConflict conflict in ROM.UndoList[index].Conflicts)
                        {
                            FEH.Write.Add(conflict.Write);
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
        public void Core_RedoAction(int index)
        {
            lock (Locked)
            {
                Write write = ROM.RedoList[index].Associated;

                ROM.Changed = true;
                FEH.Changed = true;

                switch (ROM.RedoList[index].Action)
                {
                    case UserAction.Write:
                        FEH.Write.Add(write);
                        ROM.RedoAction(index);
                        break;
                    case UserAction.Overwrite:
                        FEH.Write.Add(write);
                        ROM.RedoAction(index);
                        break;
                    case UserAction.Restore:
                        FEH.Write.Delete(write.Address, write.Address + write.Data.Length);
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
            int length = Prompt.ShowNumberDialog(
                "What length of free space is needed ?",
                "Request pointer to free space",
                true, 0, (int)ROM.FileSize);
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
            if (FEH.Write.History.Count == 0)
            {
                UI.ShowMessage("No writes have been made to this ROM file.");
            }
            else
            {
                Write write = FEH.Write.History[FEH.Write.History.Count - 1];
                UI.ShowMessage("Address: " + write.Address + "\n\n" +
                    "Pointer: " + Util.BytesToSpacedHex(write.Address.ToBytes()));
            }
        }
        public void Core_CheckROMIdentifier(string identifier)
        {
            if (identifier.StartsWith("FE"))
            {
                GameVersion version;
                switch (identifier[3])
                {
                    case 'J': version = GameVersion.JAP; break;
                    case 'U': version = GameVersion.USA; break;
                    case 'E': version = GameVersion.EUR; break;
                    default: throw new Exception("FEH file describes an invalid game version.");
                }
                switch (identifier[2])
                {
                    case '6': if (Core.CurrentROM is FE6 && Core.CurrentROM.Version == version) return; break;
                    case '7': if (Core.CurrentROM is FE7 && Core.CurrentROM.Version == version) return; break;
                    case '8': if (Core.CurrentROM is FE8 && Core.CurrentROM.Version == version) return; break;
                    default: throw new Exception("FEH file describes an FE game other than 6,7 or 8, oddly enough.");
                }

                if (Prompt.DifferentGameTypes() == DialogResult.No)
                {
                    throw new Exception("ABORT");
                }
            }
            else
            {
                throw new Exception("FEH file has an invalid identifier.");
            }
        }



        void File_OpenROM_Click(object sender, EventArgs e)
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
        void File_OpenFEH_Click(object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "Fire Emblem Hack files (*.feh)|*.feh|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
                
            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (Core_ExitFEHFile())
                {
                    Core_OpenFEHFile(openWindow.FileName);
                }
            }
        }
        void File_SaveROM_Click(object sender, EventArgs e)
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
        void File_SaveFEH_Click(object sender, EventArgs e)
        {
            if (FEH.FilePath == null)
            {
                UI.ShowError("There is no file path set for the FEH file.");
            }
            else
            {
                Core_SaveFEHFile(FEH.FilePath);
            }
        }
        void File_SaveROMas_Click(object sender, EventArgs e)
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
        void File_SaveFEHas_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();

            saveWindow.Filter = "Fire Emblem Hack (*.feh)|*.feh|All files (*.*)|*.*";
            saveWindow.FilterIndex = 1;
            saveWindow.RestoreDirectory = true;
            saveWindow.CreatePrompt = true;
            saveWindow.OverwritePrompt = true;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                Core_SaveFEHFile(saveWindow.FileName);
            }
        }
        void File_RecentFiles_Click(object sender, ToolStripItemClickedEventArgs e)
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
            else if (menu_item.Text.EndsWith(".feh") || menu_item.Text.EndsWith(".FEH"))
            {
                if (Core_ExitFEHFile())
                {
                    Core_OpenFEHFile(menu_item.FileName);
                }
            }
            else UI.ShowError("Chosen file has invalid extension");
        }
        void File_ExportUPS_Click(object sender, EventArgs e)
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
                    Core.CurrentROM.GetDefaultROMSize(),
                    Core.CurrentROM.GetDefaultROMChecksum(),
                    this.ROM.FileData,
                    Core.CurrentROMSize,
                    Core.CurrentROMChecksum);
            }
        }
        void File_CloseROM_Click(object sender, EventArgs e)
        {
            Core_ExitROMFile();
        }
        void File_Exit_Click(object sender, EventArgs e)
        {
            if (Core_ExitFEHFile() && Core_ExitROMFile())
            {
                Settings.Default.Save();

                Application.Exit();
            }
            else if (e is FormClosingEventArgs)
                ((FormClosingEventArgs)e).Cancel = true;
        }

        void Edit_Undo_Click(object sender, EventArgs e)
        {
            Core_Undo();
        }
        void Edit_Redo_Click(object sender, EventArgs e)
        {
            Core_Redo();
        }
        void Edit_OpenProperties_Click(object sender, EventArgs e)
        {
            Core_OpenProperties();
        }
        void Edit_Options_Click(object sender, EventArgs e)
        {
            Core_OpenOptions();
        }

        void Tool_ExpandROM_Click(object sender, EventArgs e)
        {
            int answer = Prompt.ShowNumberDialog(
                "Please enter the desired ROM file size\r\n(amount of bytes, in hexadecimal).",
                "Resize ROM", true, 0, DataManager.ROM_MAX_SIZE);

            ROM.Resize(answer, FEH.Space);
            Core_Update();
        }
        void Tool_MarkSpace_Click(object sender, EventArgs e)
        {
            Core_MarkSpace();
        }
        void Tool_GetFreeSpace_Click(object sender, EventArgs e)
        {
            Core_GetFreeSpace();
        }
        void Tool_GetLastWrite_Click(object sender, EventArgs e)
        {
            Core_GetLastWrite();
        }

        void Tool_OpenSpace_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new SpaceEditor(this));
        }
        void Tool_OpenWrite_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new WriteEditor(this));
        }
        void Tool_OpenPoint_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new PointEditor(this));
        }



        void Open_BasicEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new BasicEditor(this));
        }
        void Open_HexEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new HexEditor(this));
        }
        void Open_PatchEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new PatchEditor(this));
        }
        void Open_ASMEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new ASMEditor(this));
        }
        void Open_ModuleEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new ModuleEditor(this));
        }
        void Open_EventEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new EventEditor(this));
        }
        void Open_WorldMapEditor_Click(object sender, EventArgs e)
        {
            if (CurrentROM == null) return;
            else if (CurrentROM is FireEmblem.FE6) Core_OpenEditor(new WorldMapEditor_FE6(this));
            else if (CurrentROM is FireEmblem.FE7) Core_OpenEditor(new WorldMapEditor_FE7(this));
            else if (CurrentROM is FireEmblem.FE8) Core_OpenEditor(new WorldMapEditor_FE8(this));
        }
        void Open_MapTilesetEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new MapTilesetEditor(this));
        }
        void Open_MapEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new MapEditor(this));
        }
        void Open_GraphicsEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new GraphicsEditor(this));
        }
        void Open_PortraitEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new PortraitEditor(this));
        }
        void Open_MapSpriteEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new MapSpriteEditor(this));
        }
        void Open_BattleScreenEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new BattleScreenEditor(this));
        }
        void Open_BattleAnimEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new BattleAnimEditor(this));
        }
        void Open_SpellAnimEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new SpellAnimEditor(this));
        }
        void Open_BackgroundEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new BackgroundEditor(this));
        }
        void Open_TextEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new TextEditor(this));
        }
        void Open_MusicEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new MusicEditor(this));
        }
        void Open_ItemEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new ItemEditor(this));
        }
        void Open_MenuEditor_Click(object sender, EventArgs e)
        {
            //Core_OpenEditor(new MenuEditor(this));
        }
        void Open_TitleScreenEditor_Click(object sender, EventArgs e)
        {
            Core_OpenEditor(new TitleScreenEditor(this));
        }

        void Help_Help_Click(object sender, EventArgs e)
        {
            string path = "file://" + Path.Combine(
                    Directory.GetCurrentDirectory(),
                    this.SoftwareName + ".chm");
            Help.ShowHelp(this, path);
        }
        void Help_About_Click(object sender, EventArgs e)
        {
            FormAbout dialog = new FormAbout();
            
            dialog.Show();
        }
    }
}