using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EmblemMagic.FireEmblem;
using GBA;
using Magic;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class MapTilesetEditor : Editor
    {
        MapTileset CurrentTileset;
        UInt16[] CurrentTerrainNames;
        MapTileAnim CurrentTileAnim;

        String CurrentEntry_Terrain
        {
            get
            {
                return "Map Terrain 0x" + Util.ByteToHex(Terrain_ArrayBox.Value) + "[" + Terrain_ArrayBox.Text + "] - ";
            }
        }
        String CurrentEntry_TileAnim
        {
            get
            {
                return "Map Tile Anim 0x" + Util.ByteToHex(TileAnim_ArrayBox.Value) + " (" + TileAnim_ArrayBox.Text + ") - ";
            }
        }



        public MapTilesetEditor()
        {
            try
            {
                InitializeComponent();

                Terrain_ArrayBox.Load("Terrain List.txt");
                Terrain_Stat_PointerArrayBox.Load("Terrain Stat Pointers.txt");
                Terrain_Class_PointerArrayBox.Load("Terrain Move Cost Pointers.txt");

                String[] file;
                try
                {
                    file = File.ReadAllLines(Core.Path_Arrays + "Map List.txt");
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not open the array file:\n" + Core.Path_Arrays + "Map List.txt", ex);
                }
                List<String> list_palettes = new List<String>();
                List<String> list_tilesets = new List<String>();
                List<String> list_tsa_array = new List<String>();
                List<String> list_tile_anim = new List<String>();
                foreach (String entry in file)
                {
                    if (entry == null || entry == "" || entry.Length < 10) continue;
                    else switch (entry.Substring(5, 5))
                    {
                        case "[PAL]": list_palettes.Add(entry); break;
                        case "[IMG]": list_tilesets.Add(entry); break;
                        case "[TSA]": list_tsa_array.Add(entry); break;
                        case "[ANM]": list_tile_anim.Add(entry); break;
                    }
                }
                ArrayFile palettes = new ArrayFile(list_palettes.ToArray());
                ArrayFile tilesets = new ArrayFile(list_tilesets.ToArray());
                ArrayFile tsa_array = new ArrayFile(list_tsa_array.ToArray());
                ArrayFile tile_anim = new ArrayFile(list_tile_anim.ToArray());

                Palette_ArrayBox.Load(palettes);
                Tileset1_ArrayBox.Load(tilesets);
                Tileset2_ArrayBox.Load(tilesets);
                TilesetTSA_ArrayBox.Load(tsa_array);
                TileAnim_ArrayBox.Load(tile_anim);

                Terrain_Name_MagicButton.EditorToOpen = "Text Editor";
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        public override void Core_OnOpen()
        {
            Palette_ArrayBox.ValueChanged    -= ArrayBox_ValueChanged;
            Tileset1_ArrayBox.ValueChanged   -= ArrayBox_ValueChanged;
            Tileset2_ArrayBox.ValueChanged   -= ArrayBox_ValueChanged;
            TilesetTSA_ArrayBox.ValueChanged -= ArrayBox_ValueChanged;
            Terrain_Stat_PointerArrayBox.ValueChanged -= Terrain_Stat_PointerArrayBox_ValueChanged;
            Terrain_Class_PointerArrayBox.ValueChanged -= Terrain_Class_PointerArrayBox_ValueChanged;
            TileAnim_ArrayBox.ValueChanged -= TileAnim_ArrayBox_ValueChanged;

            Palette_ArrayBox.SelectFirstItem();
            Tileset1_ArrayBox.SelectFirstItem();
            Tileset2_ArrayBox.SelectFirstItem();
            TilesetTSA_ArrayBox.SelectFirstItem();
            Terrain_Stat_PointerArrayBox.SelectFirstItem();
            Terrain_Class_PointerArrayBox.SelectFirstItem();
            TileAnim_ArrayBox.SelectFirstItem();

            Palette_ArrayBox.ValueChanged    += ArrayBox_ValueChanged;
            Tileset1_ArrayBox.ValueChanged   += ArrayBox_ValueChanged;
            Tileset2_ArrayBox.ValueChanged   += ArrayBox_ValueChanged;
            TilesetTSA_ArrayBox.ValueChanged += ArrayBox_ValueChanged;
            Terrain_Stat_PointerArrayBox.ValueChanged += Terrain_Stat_PointerArrayBox_ValueChanged;
            Terrain_Class_PointerArrayBox.ValueChanged += Terrain_Class_PointerArrayBox_ValueChanged;
            TileAnim_ArrayBox.ValueChanged  += TileAnim_ArrayBox_ValueChanged;

            Core_Update();
        }
        public override void Core_Update()
        {
            Core_LoadTilesetValues();
            Core_LoadTileset();
            Core_LoadTerrain();
            Core_LoadTerrainValues();
            Core_LoadTileAnim();
            Core_LoadTileAnimFrame();
            Core_LoadPaletteAndSmallTileset();
        }

        public void Core_SetEntry(
            Byte palette,
            Byte tileset_1,
            Byte tileset_2,
            Byte tsa_terrain,
            Byte map_anim_1,
            Byte map_anim_2 = 0)
        {
            Palette_ArrayBox.Value = palette;
            Tileset1_ArrayBox.Value = tileset_1;
            Tileset2_ArrayBox.Value = tileset_2;
            TilesetTSA_ArrayBox.Value = tsa_terrain;
            TileAnim_ArrayBox.Value = map_anim_1;
            if (map_anim_2 != 0)
                TileAnim_ArrayBox.Value = map_anim_2;
        }

        void Core_LoadTileset()
        {
            try
            {
                CurrentTileset = new MapTileset(
                    Core.ReadData(Palette_PointerBox.Value, Map.PALETTES * Palette.LENGTH),
                    Core.ReadData(Tileset1_PointerBox.Value, 0),
                    Core.ReadData(Tileset2_PointerBox.Value, 0),
                    Core.ReadData(TilesetTSA_PointerBox.Value,0));
                
                Tileset_GridBox.SelectionChanged -= Tileset_GridBox_SelectionChanged;
                Tileset_GridBox.Load(CurrentTileset);
                Tileset_GridBox.SelectionChanged += Tileset_GridBox_SelectionChanged;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the map tileset.", ex);
            }
        }
        void Core_LoadTilesetValues()
        {
            Palette_PointerBox.ValueChanged -= Palette_PointerBox_ValueChanged;
            Tileset1_PointerBox.ValueChanged -= Tileset1_PointerBox_ValueChanged;
            Tileset2_PointerBox.ValueChanged -= Tileset2_PointerBox_ValueChanged;
            TilesetTSA_PointerBox.ValueChanged -= TilesetTSA_PointerBox_ValueChanged;
            TileAnim_PointerBox.ValueChanged -= TileAnim_PointerBox_ValueChanged;

            try
            {
                Pointer address = Core.GetPointer("Map Data Array");

                Palette_PointerBox.Value  = Core.ReadPointer(address + Palette_ArrayBox.Value * 4);
                Tileset1_PointerBox.Value = Core.ReadPointer(address + Tileset1_ArrayBox.Value * 4);
                Tileset2_PointerBox.Value = Core.ReadPointer(address + Tileset2_ArrayBox.Value * 4);
                TilesetTSA_PointerBox.Value = Core.ReadPointer(address + TilesetTSA_ArrayBox.Value * 4);
                TileAnim_PointerBox.Value = Core.ReadPointer(address + TileAnim_ArrayBox.Value * 4);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error trying to load the tileset values.", ex);

                Palette_PointerBox.Value = new Pointer();
                Tileset1_PointerBox.Value = new Pointer();
                Tileset2_PointerBox.Value = new Pointer();
                TilesetTSA_PointerBox.Value = new Pointer();
                TileAnim_PointerBox.Value = new Pointer();
            }

            Palette_PointerBox.ValueChanged += Palette_PointerBox_ValueChanged;
            Tileset1_PointerBox.ValueChanged += Tileset1_PointerBox_ValueChanged;
            Tileset2_PointerBox.ValueChanged += Tileset2_PointerBox_ValueChanged;
            TilesetTSA_PointerBox.ValueChanged += TilesetTSA_PointerBox_ValueChanged;
            TileAnim_PointerBox.ValueChanged += TileAnim_PointerBox_ValueChanged;
        }
        void Core_LoadTerrain()
        {
            try
            {
                Byte[] terrain = Core.ReadData(TilesetTSA_PointerBox.Value, 0);

                List<UInt16> names = new List<UInt16>();
                Pointer address = Core.GetPointer("Map Terrain Names");
                while (address < Core.App.Game.FileSize)
                {
                    names.Add((UInt16)((Core.ReadByte(address + 1) << 8) | Core.ReadByte(address)));
                    if (names[names.Count - 1] == 0x0000) break;
                    address += 2;
                }
                CurrentTerrainNames = names.ToArray();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the terrain for this TSA.", ex);
            }
        }
        void Core_LoadTerrainValues()
        {
            Tileset_GridBox.SelectionChanged -= Tileset_GridBox_SelectionChanged;
            Terrain_Name_ShortBox.ValueChanged -= Terrain_Name_ShortBox_ValueChanged;
            Terrain_Stat_ByteBox.ValueChanged -= Terrain_Stat_ByteBox_ValueChanged;
            Terrain_Class_ByteBox.ValueChanged -= Terrain_Class_ByteBox_ValueChanged;

            try
            {
                Int32 entry = Terrain_ArrayBox.Value;

                Terrain_Name_ShortBox.Value = CurrentTerrainNames[entry];
                Terrain_Stat_ByteBox.Value = Core.ReadByte(Terrain_Stat_PointerArrayBox.Value + entry);
                Terrain_Class_ByteBox.Value = Core.ReadByte(Terrain_Class_PointerArrayBox.Value + entry);

                Int32 x = 0;
                Int32 y = 0;
                for (Int32 i = 0; i < CurrentTileset.Terrain.Length; i++)
                {
                    if (CurrentTileset.Terrain[i] == entry)
                    {
                        Tileset_GridBox.Selection[x, y] = true;
                    }
                    else Tileset_GridBox.Selection[x, y] = false;

                    x++;
                    if (x == 32)
                    {
                        x = 0;
                        y++;
                    }
                }
                Tileset_GridBox.Invalidate();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error trying to load the terrain values.", ex);
            }

            Tileset_GridBox.SelectionChanged += Tileset_GridBox_SelectionChanged;
            Terrain_Name_ShortBox.ValueChanged += Terrain_Name_ShortBox_ValueChanged;
            Terrain_Stat_ByteBox.ValueChanged += Terrain_Stat_ByteBox_ValueChanged;
            Terrain_Class_ByteBox.ValueChanged += Terrain_Class_ByteBox_ValueChanged;

            Terrain_Name_MagicButton.EntryToSelect = Terrain_Name_ShortBox.Value;
        }
        void Core_LoadTileAnim()
        {
            try
            {
                TileAnim_PointerBox.ValueChanged -= TileAnim_PointerBox_ValueChanged;
                TileAnim_PointerBox.Value = Core.ReadPointer(Core.GetPointer("Map Data Array") + TileAnim_ArrayBox.Value * 4);
                TileAnim_PointerBox.ValueChanged -= TileAnim_PointerBox_ValueChanged;

                CurrentTileAnim = new MapTileAnim(TileAnim_PointerBox.Value);

                TileAnim_Frame_ByteBox.Value = 0;
                TileAnim_Frame_ByteBox.Maximum = CurrentTileAnim.Frames.Count - 2;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the tile animation.", ex);
                CurrentTileAnim = null;
            }
        }
        void Core_LoadTileAnimFrame()
        {
            TileAnim_Data_PointerBox.ValueChanged -= TileAnim_Data_PointerBox_ValueChanged;
            TileAnim_Duration_ByteBox.ValueChanged -= TileAnim_Duration_ByteBox_ValueChanged;
            TileAnim_Length_ByteBox.ValueChanged -= TileAnim_Length_ByteBox_ValueChanged;
            TileAnim_Offset_ByteBox.ValueChanged -= TileAnim_Offset_ByteBox_ValueChanged;

            try
            {
                MapTileFrame frame = CurrentTileAnim.Frames[TileAnim_Frame_ByteBox.Value];

                TileAnim_Data_PointerBox.Value = frame.FrameData;
                TileAnim_Duration_ByteBox.Value = frame.Duration;
                TileAnim_Length_ByteBox.Value = frame.Length;

                if (frame.IsPaletteAnimation())
                {
                    TileAnim_Palette_RadioButton.Checked = true;
                    TileAnim_Tileset_RadioButton.Checked = false;
                    TileAnim_Offset_Label.Enabled = true;
                    TileAnim_Offset_ByteBox.Enabled = true;
                    TileAnim_Offset_ByteBox.Value = frame.Offset;

                    Byte[] change = Core.ReadData(frame.FrameData, frame.Length * 2);
                    TileAnim_PaletteBox.Load(new Palette(change));
                    TileAnim_ImageBox.Reset();
                }
                else
                {
                    TileAnim_Palette_RadioButton.Checked = false;
                    TileAnim_Tileset_RadioButton.Checked = true;
                    TileAnim_Offset_Label.Enabled = false;
                    TileAnim_Offset_ByteBox.Enabled = false;
                    TileAnim_Offset_ByteBox.Value = 0;

                    Byte[] tileset = Core.ReadData(frame.FrameData, frame.Length * 8 * Tile.LENGTH);
                    Byte[] palette = Core.ReadData(Palette_PointerBox.Value + ViewPalette_ByteBox.Value * Palette.LENGTH, Palette.LENGTH);
                    TileAnim_ImageBox.Load(new Tileset(tileset).ToImage(32, 4, palette));
                    TileAnim_PaletteBox.Reset();
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error trying to load the tileset values.", ex);
            }

            TileAnim_Data_PointerBox.ValueChanged += TileAnim_Data_PointerBox_ValueChanged;
            TileAnim_Duration_ByteBox.ValueChanged += TileAnim_Duration_ByteBox_ValueChanged;
            TileAnim_Length_ByteBox.ValueChanged += TileAnim_Length_ByteBox_ValueChanged;
            TileAnim_Offset_ByteBox.ValueChanged += TileAnim_Offset_ByteBox_ValueChanged;
        }
        void Core_LoadPaletteAndSmallTileset()
        {
            try
            {
                MapTileFrame frame = CurrentTileAnim.Frames[TileAnim_Frame_ByteBox.Value];
                Byte[] tileset = Core.ReadData(Tileset1_PointerBox.Value, 0);
                Byte[] palette = Core.ReadData(Palette_PointerBox.Value, Map.PALETTES * Palette.LENGTH);
                if (ViewAnimation_CheckBox.Checked)
                {
                    Byte[] change;
                    if (frame.IsPaletteAnimation())
                    {
                        change = TileAnim_PaletteBox.Colors.ToBytes(false);
                        Array.Copy(change, 0, palette, TileAnim_Offset_ByteBox.Value * 2, change.Length);
                    }
                    else
                    {
                        change = Core.ReadData(frame.FrameData, frame.Length * 8 * Tile.LENGTH);
                        Array.Copy(change, 0, tileset, 32 * 8 * Tile.LENGTH, change.Length);
                    }
                }
                Tileset_PaletteBox.Load(new GBA.Palette(palette, Map.PALETTES * Palette.MAX));
                Tileset_ImageBox.Load(new Tileset(tileset).ToImage(32, 32,
                    palette.GetBytes((UInt32)(ViewPalette_ByteBox.Value * Palette.LENGTH), Palette.LENGTH)));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the palette.", ex);
                CurrentTileAnim = null;
            }
        }

        void Core_Insert(MapTileset map_tileset)
        {
            UI.SuspendUpdate();
            try
            {
                Byte[] data_palette = Palette.Merge(map_tileset.Palettes).ToBytes(false);
                Byte[] data_tileset1 = map_tileset.Tileset1.ToBytes(true);
                Byte[] data_tileset2 = map_tileset.Tileset2.ToBytes(true);
                Byte[] data_tsa = map_tileset.GetTSAandTerrain(true);

                var repoints = new List<Tuple<String, Pointer, Int32>>()
                {
                    Tuple.Create("Palette", Palette_PointerBox.Value, data_palette.Length),
                    Tuple.Create("Tileset 1", Tileset1_PointerBox.Value, data_tileset1.Length),
                    Tuple.Create("Tileset 2", Tileset2_PointerBox.Value, data_tileset2.Length),
                    Tuple.Create("TSA and Terrain", TilesetTSA_PointerBox.Value, data_tsa.Length),
                };
                Pointer address = Core.GetPointer("Map Data Array");
                var writepos = new List<Pointer>()
                {
                    address + 4 * Palette_ArrayBox.Value,
                    address + 4 * Tileset1_ArrayBox.Value,
                    address + 4 * Tileset2_ArrayBox.Value,
                    address + 4 * TilesetTSA_ArrayBox.Value,
                };

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Map Tileset",
                    "The map tileset to insert may need some of its parts to be repointed.",
                    CurrentEntry_Terrain, repoints.ToArray(), writepos.ToArray());
                if (cancel) return;

                Core.WriteData(this,
                    Core.ReadPointer(writepos[0]),
                    data_palette,
                    "Map Data Array 0x" + Util.ByteToHex(Palette_ArrayBox.Value) + " - Palette written");

                Core.WriteData(this,
                    Core.ReadPointer(writepos[1]),
                    data_tileset1,
                    "Map Data Array 0x" + Util.ByteToHex(Tileset1_ArrayBox.Value) + " - Tileset 1 written");

                Core.WriteData(this,
                    Core.ReadPointer(writepos[2]),
                    data_tileset2,
                    "Map Data Array 0x" + Util.ByteToHex(Tileset2_ArrayBox.Value) + " - Tileset 2 written");

                Core.WriteData(this,
                    Core.ReadPointer(writepos[3]),
                    data_tsa,
                    "Map Data Array 0x" + Util.ByteToHex(TilesetTSA_ArrayBox.Value) + " - TSA and Terrain written");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert image file.", ex);
                Core_Update();
                return;
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }

        void Core_InsertImage(String filepath)
        {
            MapTileset tileset = null;
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                Palette palette = null;
                Bitmap maptileset = new Bitmap(filepath);
                Bitmap tileset1 = null;
                Bitmap tileset2 = null;
                String[] extensions = new String[] { "png", "PNG", "bmp", "BMP", "gif", "GIF" };
                for (Int32 i = 0; i < extensions.Length; i++)
                {
                    if (File.Exists(path + file + " palette." + extensions[i])) palette = new Palette(path + file + " palette." + extensions[i]);
                    if (File.Exists(path + file + " 1." + extensions[i])) tileset1 = new Bitmap(path + file + " 1." + extensions[i]);
                    if (File.Exists(path + file + " 2." + extensions[i])) tileset2 = new Bitmap(path + file + " 2." + extensions[i]);
                }
                if (File.Exists(path + file + " palette.pal")) palette = new Palette(path + file + " palette.pal");

                tileset = new MapTileset(palette, maptileset, tileset1, tileset2);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the image file.", ex);
                return;
            }
            Core_Insert(tileset);
        }
        void Core_InsertData(String filepath)
        {
            MapTileset tileset;
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                if (!File.Exists(path + file + ".chr"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".chr");
                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");
                if (!File.Exists(path + file + ".tsa"))
                    throw new Exception("Could not find TSA and Terrain file:\n" + path + file + ".tsa");

                Byte[] data_tileset = File.ReadAllBytes(path + file + ".chr");
                Byte[] data_palette = File.ReadAllBytes(path + file + ".pal");
                Byte[] data_tsa = File.ReadAllBytes(path + file + ".tsa");
                UInt32 length = 32 * 32 * Tile.LENGTH;
                if (data_tileset.Length != length && data_tileset.Length != length * 2)
                    throw new Exception("Tileset .chr file has invalid length." +
                        "\nIt should be " + length + " or " + length * 2 + " bytes long.");

                tileset = new MapTileset(
                    data_palette,
                    data_tileset.GetBytes(0, (Int32)length),
                    data_tileset.GetBytes(length),
                    data_tsa);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load image data.", ex);
                return;
            }
            Core_Insert(tileset);
        }
        void Core_SaveImage(String filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    CurrentTileset.Width,
                    CurrentTileset.Height,
                    CurrentTileset.Palettes,
                    delegate (Int32 x, Int32 y)
                    {
                        return (Byte)CurrentTileset[x, y];
                    });
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save image", ex);
            }
        }
        void Core_SaveData(String filepath)
        {
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                Byte[] tileset1 = Core.ReadData(Tileset1_PointerBox.Value, 0);
                Byte[] tileset2 = Core.ReadData(Tileset2_PointerBox.Value, 0);
                Byte[] data_tileset = new Byte[tileset1.Length + tileset2.Length];
                Array.Copy(tileset1, 0, data_tileset, 0, tileset1.Length);
                Array.Copy(tileset2, 0, data_tileset, tileset1.Length, tileset2.Length);
                Byte[] data_palette = Core.ReadData(Palette_PointerBox.Value, Map.PALETTES * Palette.LENGTH);
                Byte[] data_tsa = Core.ReadData(TilesetTSA_PointerBox.Value, 0);

                File.WriteAllBytes(path + file + ".chr", data_tileset);
                File.WriteAllBytes(path + file + ".pal", data_palette);
                File.WriteAllBytes(path + file + ".tsa", data_tsa);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save image data.", ex);
            }
        }



        private void File_InsertImage_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                //"Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.chr + .pal + .tsa)|*.chr;*.pal;*.tsa|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                /*
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    Core_InsertImage(openWindow.FileName);
                    return;
                }
                */
                if (openWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    Core_InsertData(openWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_SaveImage_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.chr + .pal + .tsa)|*.chr;*.pal;*.tsa|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveData(saveWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }
        private void Tools_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                "Map Palette 0x" + Util.ByteToHex(Palette_ArrayBox.Value) + " [" + Palette_ArrayBox.Text + "] - ",
                Palette_PointerBox.Value, 10);
        }



        private void Tileset_GridBox_SelectionChanged(Object sender, EventArgs e)
        {
            try
            {
                const Int32 offset = 2 * MapTileset.WIDTH * MapTileset.HEIGHT;
                Byte[] terrain = Core.ReadData(TilesetTSA_PointerBox.Value, 0);
                Int32 width  = Tileset_GridBox.Selection.GetLength(0);
                Int32 height = Tileset_GridBox.Selection.GetLength(1);
                Int32 index = 0;
                for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                    if (Tileset_GridBox.Selection[x, y])
                    {
                        CurrentTileset.Terrain[index] = Terrain_ArrayBox.Value;
                    }
                    index++;
                }
                Array.Copy(CurrentTileset.Terrain, 0, terrain, offset, CurrentTileset.Terrain.Length);

                Core.WriteData(this,
                    TilesetTSA_PointerBox.Value,
                    terrain,
                    "Map " + TilesetTSA_ArrayBox.Text + " - Terrain mapping changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to write the terrain map", ex);
            }
        }

        private void ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void ViewPalette_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_LoadTileAnimFrame();
            Core_LoadPaletteAndSmallTileset();
        }
        private void ViewAnimation_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_LoadPaletteAndSmallTileset();
        }

        private void Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Palette_ArrayBox.Value,
                Palette_PointerBox.Value,
                Palette_ArrayBox.Text + "Palette repoint");
        }
        private void Tileset1_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Tileset1_ArrayBox.Value,
                Tileset1_PointerBox.Value,
                Tileset1_ArrayBox.Text + "Tileset 1 repoint");
        }
        private void Tileset2_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Tileset2_ArrayBox.Value,
                Tileset2_PointerBox.Value,
                Tileset2_ArrayBox.Text + "Tileset 2 repoint");
        }
        private void TilesetTSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * TilesetTSA_ArrayBox.Value,
                TilesetTSA_PointerBox.Value,
                TilesetTSA_ArrayBox.Text + "Tileset TSA repoint");
        }



        private void Terrain_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_LoadTerrainValues();
        }
        private void Terrain_Name_ShortBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                Core.GetPointer("Map Terrain Names") + Terrain_ArrayBox.Value * 2,
                Util.UInt16ToBytes(Terrain_Name_ShortBox.Value, true),
                CurrentEntry_Terrain + "Name text index changed");

            Terrain_Name_MagicButton.EntryToSelect = Terrain_Name_ShortBox.Value;
        }

        private void Terrain_Stat_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Terrain_Stat_ByteBox.ValueChanged -= Terrain_Stat_ByteBox_ValueChanged;
            Terrain_Stat_ByteBox.Value = Core.ReadByte(Terrain_Stat_PointerArrayBox.Value + Terrain_ArrayBox.Value);
            Terrain_Stat_ByteBox.ValueChanged += Terrain_Stat_ByteBox_ValueChanged;
        }
        private void Terrain_Stat_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Terrain_Stat_PointerArrayBox.Value + Terrain_ArrayBox.Value,
                Terrain_Stat_ByteBox.Value,
                CurrentEntry_Terrain + Terrain_Stat_PointerArrayBox.Text + " changed");
        }

        private void Terrain_Class_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Terrain_Class_ByteBox.ValueChanged -= Terrain_Class_ByteBox_ValueChanged;
            Terrain_Class_ByteBox.Value = Core.ReadByte(Terrain_Class_PointerArrayBox.Value + Terrain_ArrayBox.Value);
            Terrain_Class_ByteBox.ValueChanged += Terrain_Class_ByteBox_ValueChanged;
        }
        private void Terrain_Class_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Terrain_Class_PointerArrayBox.Value + Terrain_ArrayBox.Value,
                Terrain_Class_ByteBox.Value,
                CurrentEntry_Terrain + "Move Cost at " + Terrain_Class_PointerArrayBox.Value + " changed");
        }



        private void TileAnim_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_LoadTileAnim();
            Core_LoadTileAnimFrame();
            Core_LoadPaletteAndSmallTileset();
        }
        private void TileAnim_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * TileAnim_ArrayBox.Value,
                TileAnim_PointerBox.Value,
                TileAnim_ArrayBox.Text + "Tile Anim 1 repoint");
        }

        private void TileAnim_Frame_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_LoadTileAnimFrame();
            Core_LoadPaletteAndSmallTileset();
        }
        private void TileAnim_RadioButton_Click(Object sender, EventArgs e)
        {

        }

        private void TileAnim_Data_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = TileAnim_Frame_ByteBox.Value;
            Pointer address = CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (CurrentTileAnim.Frames[frame].IsPaletteAnimation())
                address += 0;
            else address += 4;

            Core.WritePointer(this,
               address,
               TileAnim_Data_PointerBox.Value,
               CurrentEntry_TileAnim + "Frame " + frame + " Data repointed");
        }
        private void TileAnim_Duration_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = TileAnim_Frame_ByteBox.Value;
            Pointer address = CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (CurrentTileAnim.Frames[frame].IsPaletteAnimation())
                address += 4;
            else address += 0;

            Core.WriteByte(this,
               address,
               TileAnim_Duration_ByteBox.Value,
               CurrentEntry_TileAnim + "Frame " + frame + " Duration changed");
        }
        private void TileAnim_Length_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = TileAnim_Frame_ByteBox.Value;
            Pointer address = CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (CurrentTileAnim.Frames[frame].IsPaletteAnimation())
                address += 5;
            else address += 3;

            Core.WriteByte(this,
               address,
               TileAnim_Length_ByteBox.Value,
               CurrentEntry_TileAnim + "Frame " + frame + " Data Length changed");
        }
        private void TileAnim_Offset_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = TileAnim_Frame_ByteBox.Value;
            Pointer address = CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (CurrentTileAnim.Frames[frame].IsPaletteAnimation())
            {
                Core.WriteByte(this,
                   address + 6,
                   TileAnim_Duration_ByteBox.Value,
                   CurrentEntry_TileAnim + "Frame " + frame + " Color Offset changed");
            }
        }
    }
}
