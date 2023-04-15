using System;
using Magic.Editors;

namespace Magic
{
    public interface IApp
    {
        Magic.Components.RecentFileMenu File_RecentFiles { get; }
        System.Windows.Forms.ToolStripMenuItem Edit_Undo { get; }
        System.Windows.Forms.ToolStripMenuItem Edit_Redo { get; }



        /// <summary>
        /// The name of this application, shown on title bars
        /// </summary>
        String AppName { get; }
        Version AppVersion { get; }

        /// <summary>
        /// Is responsible for all reading/writing of data, and the IO for the ROM file.
        /// </summary>
        DataManager ROM { get; set; }
        /// <summary>
        /// The HackManager does the IO for the MHF file, and its submanagers record all relevant hack information.
        /// </summary>
        HackManager MHF { get; set; }

        /// <summary>
        /// Describes which game ROM is open, and stores any constant pre-known info
        /// </summary>
        IGame Game { get; set; }

        /// <summary>
        /// When `true`, all Magic.App windows should update normally
        /// </summary>
        Boolean Suspend { get; set; }
        /*
        /// <summary>
        /// This list contains the instances of all the currently open Editor forms.
        /// </summary>
        List<Editor> Editors { get; set; }
        */



        /// <summary>
        /// Updates the main window and all open editors
        /// </summary>
        void Core_Update();

        /// <summary>
        /// Opens a new editor window
        /// </summary>
        /// <param name="editor">The editor Form to open</param>
        void Core_OpenEditor(Editor editor);
        void Core_ExitEditor(Editor editor);

        void Core_SaveROMFile(String path);
        void Core_SaveMHFFile(String path);

        /// <summary>
        /// Performs a user-requested write operation of any sort
        /// </summary>
        /// <param name="action">The type of write operation requested by the user</param>
        /// <param name="write">The data & address of the write</param>
        void Core_UserAction(UserAction action, Write write);



        void Core_Undo();
        void Core_UndoAction(Int32 index);
        void Core_Redo();
        void Core_RedoAction(Int32 index);



        void Core_MarkSpace();
        void Core_GetFreeSpace();
        void Core_GetLastWrite();
        void Core_CheckROMIdentifier(String identifier);
    }
}