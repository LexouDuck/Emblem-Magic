using Magic.Editors;
using Magic.Properties;
using GBA;
using System;
using System.Collections.Generic;
using System.IO;

namespace Magic
{
    public class DataManager
    {
        public const Int32 ROM_MAX_SIZE = 32 * (1 << 20);
        
        private Object Locked = new Object();

        private IApp App;

        /// <summary>
        /// The path of the .gba file to open and save to.
        /// </summary>
        public String FilePath { get; private set; }
        /// <summary>
        /// Gets just the name and extension of the file at FilePath
        /// </summary>
        public String FileName
        {
            get { return Path.GetFileName(this.FilePath); }
        }

        /// <summary>
        /// The byte array holding loaded ROM data to edit
        /// </summary>
        public Byte[] FileData { get; private set; }
        /// <summary>
        /// Gets the size of the loaded ROM file, in bytes.
        /// </summary>
        public UInt32 FileSize
        {
            get { return (UInt32)this.FileData.Length; }
        }

        /// <summary>
        /// Tells whether or not this ROM is an unmodified Fire Emblem ROM
        /// </summary>
        public Boolean IsClean { get; set; }
        /// <summary>
        /// Whether or not the ROM file has been changed since it was last saved.
        /// </summary>
        public Boolean WasChanged { get; set; }
        /// <summary>
        /// Tells whether or not the filesize has been changed
        /// </summary>
        public Boolean WasExpanded { get; set; }

        /// <summary>
        /// Stores the original ROM data in case the user wishes to undo a write, a modification to write, or a data restore.
        /// </summary>
        public List<UndoRedo> UndoList { get; private set; }
        /// <summary>
        /// Stores actions that can be redone - this list is populated when an undo is done, and cleared when an action is.
        /// </summary>
        public List<UndoRedo> RedoList { get; private set; }



        public DataManager(IApp app)
        {
            this.App = app;

            this.FilePath = null;
            this.FileData = new Byte[ROM_MAX_SIZE];

            this.IsClean = true;
            this.WasChanged = false;
            this.WasExpanded = false;

            this.UndoList = new List<UndoRedo>(Settings.Default.UndoListMax);
            this.RedoList = new List<UndoRedo>(Settings.Default.UndoListMax);
        }



        /// <summary>
        /// Opens the file at the given path for the DataManager.
        /// </summary>
        /// <returns>true if the file was successfully opened</returns>
        public void OpenFile(String path)
        {
            if (path == null || path.Length == 0)
                throw new Exception("The ROM file path given is invalid.");
            if (!File.Exists(path))
                throw new Exception("The ROM file was not found: " + path);

            lock (this.Locked)
            {
                FileInfo file = new FileInfo(path);

                if (file.Length > ROM_MAX_SIZE)
                    throw new Exception("The ROM file is too big to be opened by this program.");

                this.FilePath = path;
                this.FileData = File.ReadAllBytes(this.FilePath);

                this.WasChanged = false;
            }
        }
        /// <summary>
        /// Saves the current state of the ROM file to the disk.
        /// </summary>
        public void SaveFile(String path)
        {
            lock (this.Locked)
            {
                this.FilePath = path;
                
                File.WriteAllBytes(this.FilePath, this.FileData);

                this.WasChanged = false;
            }
        }



        /// <summary>
        /// Returns 'length' of data from the loaded ROM at the given address.
        /// </summary>
        public Byte[] Read(Pointer address, Int32 length)
        {
            Byte[] result = new Byte[length];
            lock (this.Locked)
            {
                try
                {
                    Array.Copy(this.FileData, address, result, 0, length);
                }
                catch (Exception ex)
                {
                    UI.ShowError("Data could not be read from ROM.", ex);
                }
            }
            return result;
        }
        /// <summary>
        /// Returns the address of the first occurence of the given sequence of bytes in the ROM
        /// </summary>
        public Pointer Find(Byte[] data, UInt32 align)
        {
            lock (this.Locked)
            {
                Byte[] buffer = new Byte[data.Length];
                try
                {
                    for (UInt32 parse = 0; parse < this.FileData.Length; parse += align)
                    {
                        if (data[0] == this.FileData[parse])
                        {
                            Boolean match = true;
                            Array.Copy(this.FileData, parse, buffer, 0, data.Length);

                            for (Int32 i = 0; i < data.Length; i++)
                            {
                                if (data[i] != buffer[i])
                                {
                                    match = false;
                                    break;
                                }
                            }

                            if (match) return new Pointer(parse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UI.ShowError("Data could not be properly read from loaded ROM.", ex);
                }
                return new Pointer();
            }
        }
        /// <summary>
        /// Returns an array of addresses at which the given sequence of bytes was found in the ROM
        /// </summary>
        public Pointer[] Search(Byte[] data, UInt32 align)
        {
            List<Pointer> result = new List<Pointer>();
            lock (this.Locked)
            {
                Byte[] buffer = new Byte[data.Length];
                try
                {
                    for (UInt32 parse = 0; parse < this.FileData.Length; parse += align)
                    {
                        if (data[0] == this.FileData[parse])
                        {
                            Boolean match = true;
                            Array.Copy(this.FileData, parse, buffer, 0, data.Length);

                            for (Int32 i = 0; i < data.Length; i++)
                            {
                                if (data[i] != buffer[i])
                                {
                                    match = false;
                                    break;
                                }
                            }

                            if (match) result.Add(new Pointer(parse));
                        }
                    }
                }
                catch (Exception ex)
                {
                    UI.ShowError("Data could not be properly read from loaded ROM.", ex);
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// Writes the given data at the given offset onto the ROM, checking for conflicts, recording writes and storing data for undos
        /// </summary>
        public void Write(UserAction action, Write write, List<WriteConflict> conflict)
        {
            lock (this.Locked)
            {
                try
                {
                    Byte[] old_data = new Byte[write.Data.Length];
                    Array.Copy(this.FileData, write.Address, old_data, 0, write.Data.Length);

                    for (Int32 i = 0; i < write.Data.Length; i++)
                    {
                        this.FileData[write.Address + i] = write.Data[i];
                    }

                    if (action == UserAction.Cancel) return;

                    this.RedoList.Clear();
                    this.UndoList.Add(new UndoRedo(action, write, old_data, conflict));

                    this.WasChanged = true;
                }
                catch (Exception ex)
                {
                    UI.ShowError("Data could not be written to loaded ROM.", ex);
                }
            }
        }
        /// <summary>
        /// Restores the data from a clean ROM file for the loaded ROM
        /// </summary>
        public void Restore(Pointer address, Int32 length, List<WriteConflict> conflict)
        {
            lock (this.Locked)
            {
                try
                {
                    Byte[] old_data = new Byte[length];
                    Array.Copy(this.FileData, address, old_data, 0, length);
                    Byte[] restore = new Byte[length];
                    using (FileStream file = File.OpenRead(Core.Path_CleanROM))
                    {
                        file.Position = address.Address;
                        file.Read(restore, 0, length);
                    }

                    this.UndoList.Add(new UndoRedo(
                        UserAction.Restore,
                        new Write("", new Pointer(address), restore),
                        old_data, conflict));

                    for (Int32 i = 0; i < length; i++)
                    {
                        this.FileData[address + i] = restore[i];
                    }
                }
                catch (Exception ex)
                {
                    UI.ShowError("ROM data could not be restored from file.", ex);
                }
            }
        }
        /// <summary>
        /// Changes the length of the loaded ROM, marking expanded space as 'FREE'
        /// </summary>
        public void Resize(Int32 newFileSize, SpaceManager space)
        {
            if (newFileSize == 0 ||
                newFileSize == this.FileSize)
            {
                return;
            }
            else if (newFileSize > ROM_MAX_SIZE)
            {
                UI.ShowMessage("The ROM cannot be expanded beyond 32MB.");
                return;
            }
            Byte[] data = this.FileData;
            this.FileData = new Byte[newFileSize];
            if (newFileSize > data.Length)
            {
                Array.Copy(data, this.FileData, data.Length);
                space.MarkSpace("FREE",
                    new Pointer((UInt32)data.Length),
                    new Pointer((UInt32)this.FileData.Length));
            }
            else Array.Copy(data, this.FileData, this.FileData.Length);
        }

        
        
        /// <summary>
        /// Undoes the action at the given index of UndoList, moving that undo to the RedoList afterwards.
        /// </summary>
        public void UndoAction(Int32 index)
        {
            lock (this.Locked)
            {
                if (index >= 0 && index < this.UndoList.Count) try
                {
                    Byte[] old_data = new Byte[this.UndoList[index].Data.Length];
                    Array.Copy(this.FileData, this.UndoList[index].Associated.Address, old_data, 0, old_data.Length);

                        this.RedoList.Add(new UndoRedo(this.UndoList[index].Action, this.UndoList[index].Associated, old_data, this.UndoList[index].Conflicts));

                    for (Int32 i = 0; i < this.UndoList[index].Data.Length; i++)
                    {
                            this.FileData[this.UndoList[index].Associated.Address + i] = this.UndoList[index].Data[i];
                    }
                        this.UndoList.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    UI.ShowError("Action could not be undone.", ex);
                }
            }
        }
        /// <summary>
        /// Redoes the action at the given index of RedoList - moves the redo list item into UndoList
        /// </summary>
        public void RedoAction(Int32 index)
        {
            lock (this.Locked)
            {
                if (index >= 0 && index < this.RedoList.Count) try
                {
                    Byte[] old_data = new Byte[this.RedoList[index].Data.Length];
                    Array.Copy(this.FileData, this.RedoList[index].Associated.Address, old_data, 0, old_data.Length);
                    UserAction action = this.RedoList[index].Action;
                    if (action == UserAction.Cancel) action = UserAction.Overwrite;

                        this.UndoList.Add(new UndoRedo(action, this.RedoList[index].Associated, old_data, this.RedoList[index].Conflicts));

                    for (Int32 i = 0; i < this.RedoList[index].Data.Length; i++)
                    {
                            this.FileData[this.RedoList[index].Associated.Address + i] = this.RedoList[index].Data[i];
                    }
                        this.RedoList.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    UI.ShowError("Action could not be redone.", ex);
                }

                this.WasChanged = true;
            }
        }
    }
}
