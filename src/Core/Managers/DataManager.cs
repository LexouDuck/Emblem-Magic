﻿using EmblemMagic.Editors;
using EmblemMagic.Properties;
using GBA;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmblemMagic
{
    public class DataManager
    {
        public const Int32 ROM_MAX_SIZE = 32 * (1 << 20);
        
        private Object Locked = new Object();

        /// <summary>
        /// Whether or not the ROM file has been changed since it was last saved.
        /// </summary>
        public Boolean Changed { get; set; }

        /// <summary>
        /// Gets just the name and extension of the file at FilePath
        /// </summary>
        public String FileName
        {
            get { return Path.GetFileName(FilePath); }
        }
        /// <summary>
        /// The path of the .gba file to open and save to.
        /// </summary>
        public String FilePath { get; private set; }
        /// <summary>
        /// The byte array holding loaded ROM data to edit
        /// </summary>
        public Byte[] FileData { get; private set; }
        /// <summary>
        /// Gets the size of the loaded ROM file, in bytes.
        /// </summary>
        public UInt32 FileSize
        {
            get { return (uint)FileData.Length; }
        }

        /// <summary>
        /// Stores the original ROM data in case the user wishes to undo a write, a modification to write, or a data restore.
        /// </summary>
        public List<UndoRedo> UndoList { get; private set; }
        /// <summary>
        /// Stores actions that can be redone - this list is populated when an undo is done, and cleared when an action is.
        /// </summary>
        public List<UndoRedo> RedoList { get; private set; }



        public DataManager()
        {
            FilePath = "";
            FileData = new Byte[ROM_MAX_SIZE];
            UndoList = new List<UndoRedo>(Settings.Default.UndoListMax);
            RedoList = new List<UndoRedo>(Settings.Default.UndoListMax);
        }



        /// <summary>
        /// Opens the file at the given path for the DataManager.
        /// </summary>
        /// <returns>true if the file was successfully opened</returns>
        public void OpenFile(string path)
        {
            if (path == null || path.Length == 0)
                throw new Exception("The ROM file path given is invalid.");
            if (!File.Exists(path))
                throw new Exception("The ROM file was not found: " + path);

            lock (Locked)
            {
                FileInfo file = new FileInfo(path);

                if (file.Length > ROM_MAX_SIZE)
                    throw new Exception("The ROM file is too big to be opened by this program.");
                
                FilePath = path;
                FileData = File.ReadAllBytes(FilePath);

                this.Changed = false;
            }
        }
        /// <summary>
        /// Saves the current state of the ROM file to the disk.
        /// </summary>
        public void SaveFile(String path)
        {
            lock (Locked)
            {
                FilePath = path;
                
                File.WriteAllBytes(FilePath, FileData);

                this.Changed = false;
            }
        }



        /// <summary>
        /// Returns 'length' of data from the loaded ROM at the given address.
        /// </summary>
        public Byte[] Read(Pointer address, int length)
        {
            byte[] result = new byte[length];
            lock (Locked)
            {
                try
                {
                    Array.Copy(FileData, address, result, 0, length);
                }
                catch (Exception ex)
                {
                    Program.ShowError("Data could not be read from ROM.", ex);
                }
            }
            return result;
        }
        /// <summary>
        /// Returns the address of the first occurence of the given sequence of bytes in the ROM
        /// </summary>
        public Pointer Find(byte[] data, uint align)
        {
            lock (Locked)
            {
                byte[] buffer = new byte[data.Length];
                try
                {
                    for (uint parse = 0; parse < FileData.Length; parse += align)
                    {
                        if (data[0] == FileData[parse])
                        {
                            bool match = true;
                            Array.Copy(FileData, parse, buffer, 0, data.Length);

                            for (int i = 0; i < data.Length; i++)
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
                    Program.ShowError("Data could not be properly read from loaded ROM.", ex);
                }
                return new Pointer();
            }
        }
        /// <summary>
        /// Returns an array of addresses at which the given sequence of bytes was found in the ROM
        /// </summary>
        public Pointer[] Search(byte[] data, uint align)
        {
            List<Pointer> result = new List<Pointer>();
            lock (Locked)
            {
                byte[] buffer = new byte[data.Length];
                try
                {
                    for (uint parse = 0; parse < FileData.Length; parse += align)
                    {
                        if (data[0] == FileData[parse])
                        {
                            bool match = true;
                            Array.Copy(FileData, parse, buffer, 0, data.Length);

                            for (int i = 0; i < data.Length; i++)
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
                    Program.ShowError("Data could not be properly read from loaded ROM.", ex);
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// Writes the given data at the given offset onto the ROM, checking for conflicts, recording writes and storing data for undos
        /// </summary>
        public void Write(UserAction action, Write write, List<WriteConflict> conflict)
        {
            lock (Locked)
            {
                try
                {
                    Byte[] old_data = new Byte[write.Data.Length];
                    Array.Copy(this.FileData, write.Address, old_data, 0, write.Data.Length);

                    for (int i = 0; i < write.Data.Length; i++)
                    {
                        this.FileData[write.Address + i] = write.Data[i];
                    }

                    if (action == UserAction.Cancel) return;

                    RedoList.Clear();
                    UndoList.Add(new UndoRedo(action, write, old_data, conflict));

                    this.Changed = true;
                }
                catch (Exception ex)
                {
                    Program.ShowError("Data could not be written to loaded ROM.", ex);
                }
            }
        }
        /// <summary>
        /// Restores the data from a clean ROM file for the loaded ROM
        /// </summary>
        public void Restore(Pointer address, int length, List<WriteConflict> conflict)
        {
            lock (Locked)
            {
                try
                {
                    Byte[] old_data = new Byte[length];
                    Array.Copy(FileData, address, old_data, 0, length);
                    Byte[] restore = new Byte[length];
                    using (FileStream file = File.OpenRead(Core.Path_CleanROM))
                    {
                        file.Position = address.Address;
                        file.Read(restore, 0, length);
                    }

                    UndoList.Add(new UndoRedo(
                        UserAction.Restore,
                        new Write("", new Pointer(address), restore),
                        old_data, conflict));

                    for (int i = 0; i < length; i++)
                    {
                        FileData[address + i] = restore[i];
                    }
                }
                catch (Exception ex)
                {
                    Program.ShowError("ROM data could not be restored from file.", ex);
                }
            }
        }
        /// <summary>
        /// Changes the length of the loaded ROM, marking expanded space as 'FREE'
        /// </summary>
        public void Resize(int newFileSize, SpaceManager space)
        {
            if (newFileSize == 0 ||
                newFileSize == FileSize)
            {
                return;
            }
            else if (newFileSize > ROM_MAX_SIZE)
            {
                Program.ShowMessage("The ROM cannot be expanded beyond 32MB.");
                return;
            }
            byte[] data = FileData;
            FileData = new byte[newFileSize];
            if (newFileSize > data.Length)
            {
                Array.Copy(data, FileData, data.Length);
                space.MarkSpace("FREE",
                    new Pointer((uint)data.Length),
                    new Pointer((uint)FileData.Length));
            }
            else Array.Copy(data, FileData, FileData.Length);
        }

        
        
        /// <summary>
        /// Undoes the action at the given index of UndoList, moving that undo to the RedoList afterwards.
        /// </summary>
        public void UndoAction(int index)
        {
            lock (Locked)
            {
                if (index >= 0 && index < UndoList.Count) try
                {
                    Byte[] old_data = new Byte[UndoList[index].Data.Length];
                    Array.Copy(FileData, UndoList[index].Associated.Address, old_data, 0, old_data.Length);

                    RedoList.Add(new UndoRedo(UndoList[index].Action, UndoList[index].Associated, old_data, UndoList[index].Conflicts));

                    for (int i = 0; i < UndoList[index].Data.Length; i++)
                    {
                        FileData[UndoList[index].Associated.Address + i] = UndoList[index].Data[i];
                    }
                    UndoList.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    Program.ShowError("Action could not be undone.", ex);
                }
            }
        }
        /// <summary>
        /// Redoes the action at the given index of RedoList - moves the redo list item into UndoList
        /// </summary>
        public void RedoAction(int index)
        {
            lock (Locked)
            {
                if (index >= 0 && index < RedoList.Count) try
                {
                    Byte[] old_data = new Byte[RedoList[index].Data.Length];
                    Array.Copy(FileData, RedoList[index].Associated.Address, old_data, 0, old_data.Length);
                    UserAction action = RedoList[index].Action;
                    if (action == UserAction.Cancel) action = UserAction.Overwrite;

                    UndoList.Add(new UndoRedo(action, RedoList[index].Associated, old_data, RedoList[index].Conflicts));

                    for (int i = 0; i < RedoList[index].Data.Length; i++)
                    {
                        FileData[RedoList[index].Associated.Address + i] = RedoList[index].Data[i];
                    }
                    RedoList.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    Program.ShowError("Action could not be redone.", ex);
                }

                this.Changed = true;
            }
        }
    }
}
