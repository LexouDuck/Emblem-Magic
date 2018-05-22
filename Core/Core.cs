using Compression;
using EmblemMagic.Editors;
using EmblemMagic.FireEmblem;
using EmblemMagic.Properties;
using GBA;
using System;
using System.IO;

namespace EmblemMagic
{
    /// <summary>
    /// This holds useful fields and functions that should be available anywhere, basically
    /// </summary>
    public static class Core
    {
        //================================ Folder Paths ============================

        /// <summary>
        /// Gets the full path (and filename) of the corresponding clean ROM
        /// </summary>
        public static string Path_CleanROM
        {
            get
            {
                string path = Settings.Default.PathCleanROMs + "\\" + ROMIdentifier + ".gba";
                Try:
                if (File.Exists(path))
                    return path;
                else
                {
                    Program.ShowError(
                    "This operation requires an appropriate clean ROM named '" +
                    ROMIdentifier + ".gba' in your designated clean ROMs folder to be performed.\n\n" +
                    "Please click 'OK' once this has been done to proceed.");
                    goto Try;
                }
            }
        }
        /// <summary>
        /// Gets the full path to the Arrays folder for the ROM, or a custom folder is the option is set
        /// </summary>
        public static string Path_Arrays
        {
            get
            {
                if (Settings.Default.UseCustomPathArrays) return Settings.Default.PathCustomArrays + "\\";
                else return Settings.Default.PathArrays + "\\" + CurrentROM.GetIdentifier() + "\\";
            }
        }
        /// <summary>
        /// Gets the full path to the Arrays folder for the ROM, or a custom folder is the option is set
        /// </summary>
        public static string Path_Structs
        {
            get
            {
                if (Settings.Default.UseCustomPathStructs) return Settings.Default.PathCustomStructs + "\\";
                else return Settings.Default.PathStructs + "\\" + CurrentROM.GetIdentifier() + "\\";
            }
        }
        /// <summary>
        /// Gets the full path to the Modules folder appropriate for the current ROM
        /// </summary>
        public static string Path_Modules
        {
            get
            {
                return Settings.Default.PathModules + "\\" + CurrentROM.GetIdentifier() + "\\";
            }
        }
        /// <summary>
        /// Gets the full path to the Patches folder for the program
        /// </summary>
        public static string Path_Patches
        {
            get
            {
                return Settings.Default.PathPatches + "\\" + CurrentROM.GetIdentifier() + "\\";
            }
        }

        public static void SetDefaultPaths()
        {
            string path = Directory.GetCurrentDirectory();

            if (Settings.Default.PathCleanROMs == "")
                Settings.Default.PathCleanROMs = path + "\\ROMs";
            if (Settings.Default.PathArrays == "")
                Settings.Default.PathArrays = path + "\\Arrays";
            if (Settings.Default.PathStructs == "")
                Settings.Default.PathStructs = path + "\\Structs";
            if (Settings.Default.PathModules == "")
                Settings.Default.PathModules = path + "\\Modules";
            if (Settings.Default.PathPatches == "")
                Settings.Default.PathPatches = path + "\\Patches";
        }

        public static Palette FindPaletteFile(string filepath)
        {
            string path = filepath.Substring(0, filepath.Length - 4) + ".pal";
            Palette result = null;
            if (File.Exists(path)) result = new Palette(path);
            path = filepath.Substring(0, filepath.Length - 4) + " palette.png";
            if (File.Exists(path)) result = new Palette(path);
            path = filepath.Substring(0, filepath.Length - 4) + " palette.bmp";
            if (File.Exists(path)) result = new Palette(path);
            path = filepath.Substring(0, filepath.Length - 4) + " palette.gif";
            if (File.Exists(path)) result = new Palette(path);

            return result;
        }

        //=============================== ROM-Specific =============================

        /// <summary>
        /// The Game (FE6, FE7, FE8) corresponding to the currently open ROM
        /// </summary>
        public static Game CurrentROM
        {
            get
            {
                return Program.Core.CurrentROM;
            }
        }

        /// <summary>
        /// Gets the identifier for the current type of ROM open: "FE6J", "FE7U", etc
        /// </summary>
        public static string ROMIdentifier
        {
            get { return Program.Core.CurrentROM.GetIdentifier(); }
        }

        /// <summary>
        /// Gets the default file size of the current ROM (according to game and version)
        /// </summary>
        public static UInt32 DefaultROMSize
        {
            get
            {
                if (CurrentROM is FE6) return FE6.DefaultFileSize(CurrentROM.Version);
                if (CurrentROM is FE7) return FE7.DefaultFileSize(CurrentROM.Version);
                if (CurrentROM is FE8) return FE8.DefaultFileSize(CurrentROM.Version);
                return 0;
            }
        }
        /// <summary>
        /// Gets the CRC32 checksum of the default version of the current ROM (according to game and version)
        /// </summary>
        public static UInt32 DefaultROMChecksum
        {
            get
            {
                if (CurrentROM is FE6) return FE6.Checksum(CurrentROM.Version);
                if (CurrentROM is FE7) return FE7.Checksum(CurrentROM.Version);
                if (CurrentROM is FE8) return FE8.Checksum(CurrentROM.Version);
                return 0;
            }
        }

        /// <summary>
        /// Gets the current file size of the currently open ROM
        /// </summary>
        public static UInt32 CurrentROMSize
        {
            get
            {
                return Program.Core.ROM.FileSize;
            }
        }

        /// <summary>
        /// Gets the CRC32 checksum of the currently open ROM
        /// </summary>
        public static UInt32 CurrentROMChecksum
        {
            get
            {
                return CRC32.GetChecksum(Program.Core.ROM.FileData);
            }
        }

        //============================= Editor Updating ============================

        /// <summary>
        /// Stops all editor windows from updating
        /// </summary>
        internal static void SuspendUpdate()
        {
            Program.Core.Suspend = true;
        }
        /// <summary>
        /// Allows editor windows to update upon writing
        /// </summary>
        internal static void ResumeUpdate()
        {
            Program.Core.Suspend = false;
        }
        /// <summary>
        /// Forces all editor windows to update
        /// </summary>
        internal static void PerformUpdate()
        {
            Program.Core.Suspend = false;
            Program.Core.Core_Update();
        }

        //============================= Context Editors ============================

        /// <summary>
        /// Opens a Palette Editor for the palette at 'address' (set paletteAmount as 0 if palette is compressed)
        /// </summary>
        public static void OpenPaletteEditor(
            Editor sender, String prefix,
            Pointer address, int paletteAmount)
        {
            Program.Core.Core_OpenEditor(new PaletteEditor(sender, prefix, address, (byte)paletteAmount));
        }
        /// <summary>
        /// Opens a TSA Editor for the TSA array at 'address'
        /// </summary>
        public static void OpenTSAEditor(
            Editor sender, String prefix,
            Pointer palette_address, int palette_length,
            Pointer tileset_address, int tileset_length,
            Pointer tsa_address,
            int tsa_width, int tsa_height,
            bool compressed, bool flipRows)
        {
            Program.Core.Core_OpenEditor(new TSAEditor(
                sender, prefix,
                palette_address, palette_length,
                tileset_address, tileset_length,
                tsa_address,
                tsa_width, tsa_height,
                compressed, flipRows));
        }
        /// <summary>
        /// Opens an OAM Editor for the OAM data at 'address'
        /// </summary>
        public static void OpenOAMEditor(
            String prefix,
            Pointer address,
            int compressed,
            int offsetX, int offsetY,
            Palette palette,
            Tileset tileset)
        {
            Program.Core.Core_OpenEditor(new OAMEditor(prefix, address, compressed, offsetX, offsetY, palette, tileset));
        }

        //============================ Reading from ROM ============================

        /// <summary>
        /// Reads a single byte from the current open ROM
        /// </summary>
        public static Byte ReadByte(Pointer address)
        {
            if (address == 0) return 0x00;

            //Program.Core.FEH.Space.MarkSpace("BYTE", address, address + 1);

            try
            {
                return Program.Core.ROM.FileData[address];
            }
            catch (Exception ex)
            {
                Program.ShowError("Byte could not be read from ROM at address " + address, ex);
                return 0;
            }
        }
        /// <summary>
        /// Reads 'length' of bytes from the currently loaded ROM (does LZ77 decompress if length == 0)
        /// </summary>
        public static Byte[] ReadData(Pointer address, int length)
        {
            if (address == 0) return new byte[0];

            //Program.Core.FEH.Space.MarkSpace("DATA", address, address + length);

            try
            {
                if (length == 0)
                {
                    return LZ77.Decompress(address);
                }
                else return Program.Core.ROM.Read(address, length);
            }
            catch (Exception ex)
            {
                Program.ShowError("Data could not be read from ROM at address " + address, ex);
                return new Byte[length];
            }
        }
        /// <summary>
        /// Reads bytes from the ROM and returns a Pointer
        /// </summary>
        public static Pointer ReadPointer(Pointer address)
        {
            if (address == 0) return new Pointer();

            //Program.Core.FEH.Space.MarkSpace("POIN", address, address + 4);
            
            byte[] data = Program.Core.ROM.Read(address, 4);

            if (data[3] < 0x08 && !(data[0] == 0 && data[1] == 0 && data[2] == 0 && data[3] == 0))
                throw new Exception("Pointer is invalid: " + Util.BytesToSpacedHex(data));

            return new Pointer(data, true, true);
        }
        /// <summary>
        /// Reads a palette from the currently loaded ROM (does LZ77 decompress if length == 0)
        /// </summary>
        public static Palette ReadPalette(Pointer address, int length)
        {
            if (address == 0) return null;

            //Program.Core.FEH.Space.MarkSpace("PAL ", address, address + length);

            byte[] palette = Core.ReadData(address, length);

            return new Palette(palette, palette.Length / 2);
        }
        /// <summary>
        /// Reads TSA data at the given address, handling the byte array as necessary
        /// </summary>
        /// <param name="address">The address to the read the data at</param>
        /// <param name="width">The width of the TSA, in tiles</param>
        /// <param name="height">The height of the TSA, in tiles</param>
        /// <param name="compressed">Whether or not the TSA data is LZ77 compressed</param>
        /// <param name="flipRows">Whether or not to flip the rows of the TSA (usually for uncompressed TSA)</param>
        /// <returns></returns>
        public static TSA_Array ReadTSA(Pointer address, int width, int height, bool compressed, bool flipRows)
        {
            if (address == 0) return null;

            //Program.Core.FEH.Space.MarkSpace("TSA ", address, address + length);

            byte[] data;
            if (compressed)
            {
                try
                {
                    data = LZ77.Decompress(address);

                    if (flipRows)
                    {
                        width = data[0] + 1;
                        height = data[1] + 1;
                        data = data.GetBytes(2);
                    }
                }
                catch (Exception ex)
                {
                    Program.ShowError("There has been an error while trying to decompress the TSA.", ex);
                    return null;
                }
            }
            else if (flipRows)
            {
                width = Core.ReadByte(address) + 1;
                height = Core.ReadByte(address + 1) + 1;
                data = Program.Core.ROM.Read(address + 2, width * height * 2);
            }
            else
            {
                data = Program.Core.ROM.Read(address, width * height * 2);
            }

            if (flipRows)
            {
                byte[] result = new byte[width * height * 2];
                int row = width * 2;
                for (int i = 0; i < height; i++)
                {
                    Array.Copy(data, (height - 1 - i) * row, result, i * row, row);
                }
                return new TSA_Array(width, height, result);
            }
            else return new TSA_Array(width, height, data);
        }

        /// <summary>
        /// Returns the first occurence of the given sequence of bytes in the ROM, 'align' determines search interval
        /// </summary>
        public static Pointer FindData(Byte[] data, uint align = 1)
        {
            return Program.Core.ROM.Find(data, align);
        }
        /// <summary>
        /// Returns all the addresses at which the given sequence of bytes was found, 'align' determines search interval
        /// </summary>
        public static Pointer[] SearchData(Byte[] data, uint align = 1)
        {
            return Program.Core.ROM.Search(data, align);
        }

        /// <summary>
        /// Returns the current pointer to the asset whose name matches the one given
        /// </summary>
        public static Repoint GetRepoint(string asset)
        {
            Repoint result = Program.Core.FEH.Point.Get(asset);
            if (result == null)
                throw new Exception("Couldn't find pointer with asset name: " + asset);
            else return result;
        }
        /// <summary>
        /// Returns the current pointer to the asset whose name matches the one given
        /// </summary>
        public static Pointer GetPointer(string asset)
        {
            return GetRepoint(asset).CurrentAddress;
        }

        /// <summary>
        /// Returns the first encountered address to an area of free space of appropriate size
        /// </summary>
        public static Pointer GetFreeSpace(int length)
        {
            Pointer result = Program.Core.FEH.Space.GetPointer("FREE", length);

            if (result == new Pointer()) throw new Exception("No suitable length of free space was found.");

            return result;
        }

        //============================ Writing to ROM ============================

        /// <summary>
        /// Writes a single byte to the currently loaded ROM
        /// </summary>
        public static void WriteByte(Editor sender, Pointer address, byte data, string description = "")
        {
            if (address == 0) return;

            Program.Core.Core_UserAction(UserAction.Write, new Write(sender, address, new byte[1] { data }, description));
        }
        /// <summary>
        /// Writes Data to the currently loaded ROM
        /// </summary>
        public static void WriteData(Editor sender, Pointer address, byte[] data, string description = "")
        {
            if (address == 0 || data == null) return;

            Program.Core.Core_UserAction(UserAction.Write, new Write(sender, address, data, description));
        }
        /// <summary>
        /// Writes a 4-byte Pointer to the ROM, converting it as necessary (hardware offsetting and little endian)
        /// </summary>
        public static void WritePointer(Editor sender, Pointer address, Pointer pointer, string description = "")
        {
            if (address == 0) return;

            Program.Core.Core_UserAction(UserAction.Write, new Write(sender, address, pointer.ToBytes(true, true), description));
        }

        /// <summary>
        /// Replaces any occurence of the 'original' pointer found in the ROM
        /// </summary>
        public static void Repoint(Editor sender, Pointer original, Pointer repoint, string description = "")
        {
            if (original == repoint) return;
            Repoint pointer = Program.Core.FEH.Point.Get(original);
            if (pointer == null)
                throw new Exception("Couldn't find any FEH Repoint of address: " + original);
            pointer.UpdateReferences();
            for (int i = 0; i < pointer.References.Length; i++)
            {
                Core.WritePointer(sender, pointer.References[i], repoint, description);
            }
            pointer.CurrentAddress = repoint;
            pointer.UpdateReferences();
        }

        /// <summary>
        /// Restores data from the ROM as it was when last saved
        /// </summary>
        public static void RestoreData(Pointer address, int length)
        {
            if (address == 0) return;
            if (length == 0) throw new Exception("Data to restore cannot be of length 0.");

            Program.Core.Core_UserAction(UserAction.Restore, new Write("", address, new byte[length]));
        }
    }
}
