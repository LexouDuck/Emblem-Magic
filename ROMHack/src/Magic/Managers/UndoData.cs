using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic
{
    /// <summary>
    /// Stores all information relevant to undoing and redoing an action.
    /// </summary>
    public class UndoRedo
    {
        /// <summary>
        /// The type of action to undo/redo.
        /// </summary>
        public UserAction Action { get; set; }
        /// <summary>
        /// Reference to the write associated with this undo/redo
        /// </summary>
        public Write Associated { get; private set; }
        /// <summary>
        /// The old data to retrieve when performing an undo/redo
        /// </summary>
        public Byte[] Data { get; private set; }
        /// <summary>
        /// The list writes affected by this undo/redo
        /// </summary>
        public List<WriteConflict> Conflicts { get; private set; }

        public UndoRedo(UserAction action, Write associatedWrite, Byte[] oldData, List<WriteConflict> conflict)
        {
            Action = action;
            Associated = associatedWrite;
            Data = oldData;
            Conflicts = conflict;
        }
    }
}
