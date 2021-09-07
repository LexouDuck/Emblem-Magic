using Magic.Editors;
using GBA;
using System;
using System.Collections.Generic;

namespace Magic
{
    /// <summary>
    /// This class handles the storing and manipulation of the Write history, in 2 lists.
    /// All the fields and methods with prefix "User" refer to writes done since the last FEH save.
    /// The prefix "File" for writes saved to the open FEH file, or loaded from it.
    /// </summary>
    public class WriteManager
    {
        /// <summary>
        /// Describes all the writes done to the ROM in the current session - saving to FEH clears this list.
        /// </summary>
        public List<Write> History { get; set; }
        /// <summary>
        /// Stores all the writes that have been overwritten, undone, or deleted in the current session
        /// </summary>
        public List<Write> DeadList { get; set; }



        public IApp App;
        public WriteManager(IApp app)
        {
            App = app;
            History = new List<Write>();
            DeadList = new List<Write>();
        }

        /// <summary>
        /// Adds a write to the History, deleting any other writes or portions of writes within its range.
        /// </summary>
        public void Add(Write write)
        {
            App.FEH.Changed = true;

            Delete(write.Address, write.Address + write.Data.Length);
            History.Add(write);
        }
        /// <summary>
        /// Changes a write (sending the old one to deadlist) and returns it.
        /// </summary>
        public Write Change(Write write, Pointer address, Byte[] data)
        {
            String editor = write.Author;
            String phrase = write.Phrase;
            History.Remove(write);
            DeadList.Add(write);
            Write result = new Write(editor, address, data, phrase);
            History.Add(result);
            return result;
        }

        /// <summary>
        /// Deletes the given write permanently from the list it is in - returns false if not found
        /// </summary>
        public Boolean Delete(Write write)
        {
            App.FEH.Changed = true;

            DeadList.Add(write);
            return History.Remove(write);
        }
        /// <summary>
        /// Deletes any writes or parts of writes within the given range, and returns the affected writes
        /// </summary>
        public List<WriteConflict> Delete(Pointer start, Pointer end)
        {
            List<WriteConflict> writes = Check(start, end);
            Write write_old;
            Tuple<Write, Write> write_new;

            for (Int32 i = 0; i < writes.Count; i++)
            {
                write_old = writes[i].Write;
                write_new = writes[i].GetNewWrite();

                DeadList.Add(write_old);
                History.Remove(write_old);
                if (write_new.Item1 != null) History.Add(write_new.Item1);
                if (write_new.Item2 != null) History.Add(write_new.Item2);
            }
            return writes;
        }

        /// <summary>
        /// Returns whichever Write is at the given address, or null if no Write is found.
        /// </summary>
        public Write Find(Pointer address, Boolean findDead = false)
        {
            if (findDead)
            {
                foreach (Write write in DeadList)
                {
                    if (write.Address == address)
                    {
                        return write;
                    }
                }
            }
            else
            {
                foreach (Write write in History)
                {
                    if (write.Address == address)
                    {
                        return write;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Returns the most recent Write sharing the same address with the given Write
        /// </summary>
        public Write FindMostRecent(Pointer address, Boolean findDead = false)
        {
            Write result = null;
            foreach (Write write in DeadList)
            {
                if (write.Address == address && (result == null || write.Time.CompareTo(result.Time) < 0))
                {
                    result = write;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns all writes that would be affected by a change in the given area
        /// </summary>
        public List<WriteConflict> Check(Pointer start, Pointer end)
        {
            List<WriteConflict> result = new List<WriteConflict>();
            if (end < start) throw new Exception("given range is negative.");
            Range range;
            foreach (Write write in History)
            {
                if (start <= write.Address)
                {
                    if (end <= write.Address) continue;
                    else if (end < write.Address + write.Data.Length)
                    {
                        range = new Range(0, end - write.Address);
                    }
                    else
                    {
                        range = new Range(0, write.Data.Length);
                    }
                }
                else if (start < write.Address + write.Data.Length)
                {
                    if (end < write.Address + write.Data.Length)
                    {
                        range = new Range(start - write.Address, end - write.Address);
                    }
                    else
                    {
                        range = new Range(start - write.Address, write.Data.Length);
                    }
                }
                else continue;

                result.Add(new WriteConflict(write, range));
            }
            return result;
        }



        /// <summary>
        /// Called when FEH is saved - Sets 'Saved' to true for every write
        /// </summary>
        public void Update_FEHSaved()
        {
            for (Int32 i = 0; i < History.Count; i++)
            {
                History[i].IsSaved = true;
            }
        }
        /// <summary>
        /// Called when ROM is saved - Sets the 'Patched' field for all writes as true.
        /// </summary>
        public void Update_ROMSaved()
        {
            for (Int32 i = 0; i < History.Count; i++)
            {
                History[i].Patched = true;
            }
        }
    }
}
