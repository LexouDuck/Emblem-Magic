using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Compression;
using Magic.Editors;
using Magic.Properties;
using GBA;

namespace Magic
{
    /// <summary>
    /// This holds useful fields and functions that should be easily accessible anywhere
    /// </summary>
    public static class Core
    {
        public static IApp App;


        //=============================== ROM-Specific =============================

        /// <summary>
        /// The Game (FE6, FE7, FE8) corresponding to the currently open ROM
        /// </summary>
        public static IGame CurrentROM
        {
            get
            {
                return App.CurrentROM;
            }
        }

        /// <summary>
        /// Gets the identifier for the current type of ROM open: "FE6J", "FE7U", etc
        /// </summary>
        public static String ROMIdentifier
        {
            get { return App.CurrentROM.GetIdentifier(); }
        }

        /// <summary>
        /// Gets the current file size of the currently open ROM
        /// </summary>
        public static UInt32 CurrentROMSize
        {
            get
            {
                return App.ROM.FileSize;
            }
        }

        /// <summary>
        /// Gets the CRC32 checksum of the currently open ROM
        /// </summary>
        public static UInt32 CurrentROMChecksum
        {
            get
            {
                return CRC32.GetChecksum(App.ROM.FileData);
            }
        }

        //================================ Folder Paths ============================

        /// <summary>
        /// Gets the full path (and filename) of the corresponding clean ROM
        /// </summary>
        public static String Path_CleanROM
        {
            get
            {
                String path = Settings.Default.PathCleanROMs + "\\" + ROMIdentifier + ".gba";
                Try:
                if (File.Exists(path))
                    return path;
                else
                {
                    UI.ShowError(
                    "This operation requires an appropriate clean ROM named '" + ROMIdentifier + ".gba'" +
                    " in your designated clean ROMs folder to be performed.\n\n" +
                    "Please click 'OK' once this has been done to proceed.");
                    goto Try;
                }
            }
        }
        /// <summary>
        /// Gets the full path to the Arrays folder for the ROM, or a custom folder is the option is set
        /// </summary>
        public static String Path_Arrays
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
        public static String Path_Structs
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
        public static String Path_Modules
        {
            get
            {
                return Settings.Default.PathModules + "\\" + CurrentROM.GetIdentifier() + "\\";
            }
        }
        /// <summary>
        /// Gets the full path to the Patches folder for the program
        /// </summary>
        public static String Path_Patches
        {
            get
            {
                return Settings.Default.PathPatches + "\\" + CurrentROM.GetIdentifier() + "\\";
            }
        }

        public static void SetDefaultPaths()
        {
            String path = Directory.GetCurrentDirectory();

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

        public static Palette FindPaletteFile(String filepath)
        {
            String path = filepath.Substring(0, filepath.Length - 4) + ".pal";
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

        //============================ Reading from ROM ============================

        /// <summary>
        /// Reads a single byte from the current open ROM
        /// </summary>
        public static Byte ReadByte(Pointer address)
        {
            if (address == 0) return 0x00;

            //App.FEH.Space.MarkSpace("BYTE", address, address + 1);

            try
            {
                return App.ROM.FileData[address];
            }
            catch (Exception ex)
            {
                UI.ShowError("Byte could not be read from ROM at address " + address, ex);
                return 0;
            }
        }
        /// <summary>
        /// Reads 'length' of bytes from the currently loaded ROM (does LZ77 decompress if length == 0)
        /// </summary>
        public static Byte[] ReadData(Pointer address, Int32 length)
        {
            if (address == 0) return new Byte[0];

            //App.FEH.Space.MarkSpace("DATA", address, address + length);

            try
            {
                if (length == 0)
                {
                    return LZ77.Decompress(address);
                }
                else return App.ROM.Read(address, length);
            }
            catch (Exception ex)
            {
                UI.ShowError("Data could not be read from ROM at address " + address, ex);
                return new Byte[length];
            }
        }
        /// <summary>
        /// Reads bytes from the ROM and returns a Pointer
        /// </summary>
        public static Pointer ReadPointer(Pointer address)
        {
            if (address == 0) return new Pointer();

            //App.FEH.Space.MarkSpace("POIN", address, address + 4);

            Byte[] data = App.ROM.Read(address, 4);

            if (data[3] < 0x08 && !(data[0] == 0 && data[1] == 0 && data[2] == 0 && data[3] == 0))
                throw new Exception("Pointer is invalid: " + Util.BytesToSpacedHex(data));

            return new Pointer(data, true, true);
        }
        /// <summary>
        /// Reads a palette from the currently loaded ROM (does LZ77 decompress if length == 0)
        /// </summary>
        public static Palette ReadPalette(Pointer address, Int32 length)
        {
            if (address == 0) return null;

            //App.FEH.Space.MarkSpace("PAL ", address, address + length);

            Byte[] palette = Core.ReadData(address, length);

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
        public static TSA_Array ReadTSA(Pointer address, Int32 width, Int32 height, Boolean compressed, Boolean flipRows)
        {
            if (address == 0) return null;

            //App.FEH.Space.MarkSpace("TSA ", address, address + length);

            Byte[] data;
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
                    UI.ShowError("There has been an error while trying to decompress the TSA.", ex);
                    return null;
                }
            }
            else if (flipRows)
            {
                width = Core.ReadByte(address) + 1;
                height = Core.ReadByte(address + 1) + 1;
                data = App.ROM.Read(address + 2, width * height * 2);
            }
            else
            {
                data = App.ROM.Read(address, width * height * 2);
            }

            if (flipRows)
            {
                Byte[] result = new Byte[width * height * 2];
                Int32 row = width * 2;
                for (Int32 i = 0; i < height; i++)
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
        public static Pointer FindData(Byte[] data, UInt32 align = 1)
        {
            return App.ROM.Find(data, align);
        }
        /// <summary>
        /// Returns all the addresses at which the given sequence of bytes was found, 'align' determines search interval
        /// </summary>
        public static Pointer[] SearchData(Byte[] data, UInt32 align = 1)
        {
            return App.ROM.Search(data, align);
        }

        /// <summary>
        /// Returns the current pointer to the asset whose name matches the one given
        /// </summary>
        public static Repoint GetRepoint(String asset)
        {
            Repoint result = App.FEH.Point.Get(asset);
            if (result == null)
                throw new Exception("Couldn't find pointer with asset name: " + asset);
            else return result;
        }
        /// <summary>
        /// Returns the current pointer to the asset whose name matches the one given
        /// </summary>
        public static Pointer GetPointer(String asset)
        {
            return GetRepoint(asset).CurrentAddress;
        }

        /// <summary>
        /// Returns the first encountered address to an area of free space of appropriate size
        /// </summary>
        public static Pointer GetFreeSpace(Int32 length)
        {
            Pointer result = App.FEH.Space.GetPointer("FREE", length);

            if (result == new Pointer()) throw new Exception("No suitable length of free space was found.");

            return result;
        }

        //============================ Writing to ROM ============================

        /// <summary>
        /// Writes a single byte to the currently loaded ROM
        /// </summary>
        public static void WriteByte(Editor sender, Pointer address, Byte data, String description = "")
        {
            if (address == 0)
                return;
            App.Core_UserAction(UserAction.Write, new Write(sender, address, new Byte[1] { data }, description));
        }
        /// <summary>
        /// Writes Data to the currently loaded ROM
        /// </summary>
        public static void WriteData(Editor sender, Pointer address, Byte[] data, String description = "")
        {
            if (address == 0 || data == null)
                return;
            App.Core_UserAction(UserAction.Write, new Write(sender, address, data, description));
        }
        /// <summary>
        /// Writes a 4-byte Pointer to the ROM, converting it as necessary (hardware offsetting and little endian)
        /// </summary>
        public static void WritePointer(Editor sender, Pointer address, Pointer pointer, String description = "")
        {
            if (address == 0)
                return;
            App.Core_UserAction(UserAction.Write, new Write(sender, address, pointer.ToBytes(true, true), description));
        }

        /// <summary>
        /// Replaces any occurence of the 'original' pointer found in the ROM
        /// </summary>
        public static void Repoint(Editor sender, Pointer original, Pointer repoint, String description = "")
        {
            if (original == repoint) return;
            Repoint pointer = App.FEH.Point.Get(original);
            if (pointer == null)
                throw new Exception("Couldn't find any FEH Repoint of address: " + original);
            pointer.UpdateReferences();
            for (Int32 i = 0; i < pointer.References.Length; i++)
            {
                Core.WritePointer(sender, pointer.References[i], repoint, description);
            }
            pointer.CurrentAddress = repoint;
            pointer.UpdateReferences();
        }

        /// <summary>
        /// Restores data from the ROM as it was when last saved
        /// </summary>
        public static void RestoreData(Pointer address, Int32 length)
        {
            if (address == 0)
                return;
            if (length == 0)
                throw new Exception("Data to restore cannot be of length 0.");
            App.Core_UserAction(UserAction.Restore, new Write("", address, new Byte[length]));
        }

        //============================ Input/Output ===============================
        
        public static void SaveImage(String filepath,
            Int32 width, Int32 height,
            Palette[] palettes,
            Func<Int32, Int32, Byte> displayfunc)
        {
            using (var image = new System.Drawing.Bitmap(width, height, PixelFormat.Format8bppIndexed))
            {
                ColorPalette result_palette = image.Palette;
                for (Int32 p = 0; p < 16; p++)
                {
                    if (p >= palettes.Length)
                    {
                        for (Int32 i = 0; i < Palette.MAX; i++)
                        {
                            result_palette.Entries[p * Palette.MAX + i] = System.Drawing.Color.Black;
                        }
                    }
                    else for (Int32 i = 0; i < Palette.MAX; i++)
                    {
                        result_palette.Entries[p * Palette.MAX + i] = (System.Drawing.Color)palettes[p][i];
                    }
                }
                image.Palette = result_palette;
                var data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                unsafe
                {
                    Byte* pixeldata = (Byte*)data.Scan0.ToPointer();
                    Byte* pixel;
                    for (Int32 y = 0; y < image.Height; y++)
                    for (Int32 x = 0; x < image.Width; x++)
                    {
                        pixel = pixeldata + y * data.Width + x;
                        pixel[0] = displayfunc(x, y);
                    }
                }
                image.UnlockBits(data);
                if (Settings.Default.PreferIndexedBMP)
                    image.Save(filepath + ".bmp", ImageFormat.Bmp);
                else image.Save(filepath + ".png", ImageFormat.Png);
            }
        }
    }
}
