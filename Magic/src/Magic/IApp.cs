using System;
using Magic.Editors;

namespace Magic
{
    public interface IApp
    {
        public Magic.Components.RecentFileMenu File_RecentFiles { get; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Undo { get; }
        public System.Windows.Forms.ToolStripMenuItem Edit_Redo { get; }



        /// <summary>
        /// The name of this application, shown on title bars
        /// </summary>
        public String AppName { get; }
        public Version AppVersion { get; }

        /// <summary>
        /// Is responsible for all reading/writing of data, and the IO for the ROM file.
        /// </summary>
        public DataManager ROM { get; set; }
        /// <summary>
        /// The HackManager does the IO for the FEH file, and its submanagers record all relevant hack information.
        /// </summary>
        public HackManager FEH { get; set; }

        /// <summary>
        /// Describes which game ROM is open, and stores any constant pre-known info
        /// </summary>
        public IGame Game { get; set; }

        /// <summary>
        /// When `true`, all Magic.App windows should update normally
        /// </summary>
        public Boolean Suspend { get; set; }
        /*
        /// <summary>
        /// This list contains the instances of all the currently open Editor forms.
        /// </summary>
        public List<Editor> Editors { get; set; }
        */



        /// <summary>
        /// Updates the main window and all open editors
        /// </summary>
        public void Core_Update();

        /// <summary>
        /// Opens a new editor window
        /// </summary>
        /// <param name="editor">The editor Form to open</param>
        public void Core_OpenEditor(Editor editor);
        public void Core_ExitEditor(Editor editor);

        public void Core_SaveROMFile(String path);
        public void Core_SaveFEHFile(String path);

        /// <summary>
        /// Performs a user-requested write operation of any sort
        /// </summary>
        /// <param name="action">The type of write operation requested by the user</param>
        /// <param name="write">The data & address of the write</param>
        public void Core_UserAction(UserAction action, Write write);



        public void Core_Undo();
        public void Core_UndoAction(Int32 index);
        public void Core_Redo();
        public void Core_RedoAction(Int32 index);



        public void Core_MarkSpace();
        public void Core_GetFreeSpace();
        public void Core_GetLastWrite();
        public void Core_CheckROMIdentifier(String identifier);
    }
}