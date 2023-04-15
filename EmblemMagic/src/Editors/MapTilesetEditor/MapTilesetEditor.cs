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
                return "Map Terrain 0x" + Util.ByteToHex(this.Terrain_ArrayBox.Value) + "[" + this.Terrain_ArrayBox.Text + "] - ";
            }
        }
        String CurrentEntry_TileAnim
        {
            get
            {
                return "Map Tile Anim 0x" + Util.ByteToHex(this.TileAnim_ArrayBox.Value) + " (" + this.TileAnim_ArrayBox.Text + ") - ";
            }
        }



        public MapTilesetEditor()
        {
            try
            {
                this.InitializeComponent();

                this.Terrain_ArrayBox.Load("Terrain List.txt");
                this.Terrain_Stat_PointerArrayBox.Load("Terrain Stat Pointers.txt");
                this.Terrain_Class_PointerArrayBox.Load("Terrain Move Cost Pointers.txt");

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

                this.Palette_ArrayBox.Load(palettes);
                this.Tileset1_ArrayBox.Load(tilesets);
                this.Tileset2_ArrayBox.Load(tilesets);
                this.TilesetTSA_ArrayBox.Load(tsa_array);
                this.TileAnim_ArrayBox.Load(tile_anim);

                this.Terrain_Name_MagicButton.EditorToOpen = "Text Editor";
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        public override void Core_OnOpen()
        {
            this.Palette_ArrayBox.ValueChanged    -= this.ArrayBox_ValueChanged;
            this.Tileset1_ArrayBox.ValueChanged   -= this.ArrayBox_ValueChanged;
            this.Tileset2_ArrayBox.ValueChanged   -= this.ArrayBox_ValueChanged;
            this.TilesetTSA_ArrayBox.ValueChanged -= this.ArrayBox_ValueChanged;
            this.Terrain_Stat_PointerArrayBox.ValueChanged -= this.Terrain_Stat_PointerArrayBox_ValueChanged;
            this.Terrain_Class_PointerArrayBox.ValueChanged -= this.Terrain_Class_PointerArrayBox_ValueChanged;
            this.TileAnim_ArrayBox.ValueChanged -= this.TileAnim_ArrayBox_ValueChanged;

            this.Palette_ArrayBox.SelectFirstItem();
            this.Tileset1_ArrayBox.SelectFirstItem();
            this.Tileset2_ArrayBox.SelectFirstItem();
            this.TilesetTSA_ArrayBox.SelectFirstItem();
            this.Terrain_Stat_PointerArrayBox.SelectFirstItem();
            this.Terrain_Class_PointerArrayBox.SelectFirstItem();
            this.TileAnim_ArrayBox.SelectFirstItem();

            this.Palette_ArrayBox.ValueChanged    += this.ArrayBox_ValueChanged;
            this.Tileset1_ArrayBox.ValueChanged   += this.ArrayBox_ValueChanged;
            this.Tileset2_ArrayBox.ValueChanged   += this.ArrayBox_ValueChanged;
            this.TilesetTSA_ArrayBox.ValueChanged += this.ArrayBox_ValueChanged;
            this.Terrain_Stat_PointerArrayBox.ValueChanged += this.Terrain_Stat_PointerArrayBox_ValueChanged;
            this.Terrain_Class_PointerArrayBox.ValueChanged += this.Terrain_Class_PointerArrayBox_ValueChanged;
            this.TileAnim_ArrayBox.ValueChanged  += this.TileAnim_ArrayBox_ValueChanged;

            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Core_LoadTilesetValues();
            this.Core_LoadTileset();
            this.Core_LoadTerrain();
            this.Core_LoadTerrainValues();
            this.Core_LoadTileAnim();
            this.Core_LoadTileAnimFrame();
            this.Core_LoadPaletteAndSmallTileset();
        }

        public void Core_SetEntry(
            Byte palette,
            Byte tileset_1,
            Byte tileset_2,
            Byte tsa_terrain,
            Byte map_anim_1,
            Byte map_anim_2 = 0)
        {
            this.Palette_ArrayBox.Value = palette;
            this.Tileset1_ArrayBox.Value = tileset_1;
            this.Tileset2_ArrayBox.Value = tileset_2;
            this.TilesetTSA_ArrayBox.Value = tsa_terrain;
            this.TileAnim_ArrayBox.Value = map_anim_1;
            if (map_anim_2 != 0)
                this.TileAnim_ArrayBox.Value = map_anim_2;
        }

        void Core_LoadTileset()
        {
            try
            {
                this.CurrentTileset = new MapTileset(
                    Core.ReadData(this.Palette_PointerBox.Value, Map.PALETTES * Palette.LENGTH),
                    Core.ReadData(this.Tileset1_PointerBox.Value, 0),
                    Core.ReadData(this.Tileset2_PointerBox.Value, 0),
                    Core.ReadData(this.TilesetTSA_PointerBox.Value,0));

                this.Tileset_GridBox.SelectionChanged -= this.Tileset_GridBox_SelectionChanged;
                this.Tileset_GridBox.Load(this.CurrentTileset);
                this.Tileset_GridBox.SelectionChanged += this.Tileset_GridBox_SelectionChanged;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the map tileset.", ex);
            }
        }
        void Core_LoadTilesetValues()
        {
            this.Palette_PointerBox.ValueChanged -= this.Palette_PointerBox_ValueChanged;
            this.Tileset1_PointerBox.ValueChanged -= this.Tileset1_PointerBox_ValueChanged;
            this.Tileset2_PointerBox.ValueChanged -= this.Tileset2_PointerBox_ValueChanged;
            this.TilesetTSA_PointerBox.ValueChanged -= this.TilesetTSA_PointerBox_ValueChanged;
            this.TileAnim_PointerBox.ValueChanged -= this.TileAnim_PointerBox_ValueChanged;

            try
            {
                Pointer address = Core.GetPointer("Map Data Array");

                this.Palette_PointerBox.Value  = Core.ReadPointer(address + this.Palette_ArrayBox.Value * 4);
                this.Tileset1_PointerBox.Value = Core.ReadPointer(address + this.Tileset1_ArrayBox.Value * 4);
                this.Tileset2_PointerBox.Value = Core.ReadPointer(address + this.Tileset2_ArrayBox.Value * 4);
                this.TilesetTSA_PointerBox.Value = Core.ReadPointer(address + this.TilesetTSA_ArrayBox.Value * 4);
                this.TileAnim_PointerBox.Value = Core.ReadPointer(address + this.TileAnim_ArrayBox.Value * 4);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error trying to load the tileset values.", ex);

                this.Palette_PointerBox.Value = new Pointer();
                this.Tileset1_PointerBox.Value = new Pointer();
                this.Tileset2_PointerBox.Value = new Pointer();
                this.TilesetTSA_PointerBox.Value = new Pointer();
                this.TileAnim_PointerBox.Value = new Pointer();
            }

            this.Palette_PointerBox.ValueChanged += this.Palette_PointerBox_ValueChanged;
            this.Tileset1_PointerBox.ValueChanged += this.Tileset1_PointerBox_ValueChanged;
            this.Tileset2_PointerBox.ValueChanged += this.Tileset2_PointerBox_ValueChanged;
            this.TilesetTSA_PointerBox.ValueChanged += this.TilesetTSA_PointerBox_ValueChanged;
            this.TileAnim_PointerBox.ValueChanged += this.TileAnim_PointerBox_ValueChanged;
        }
        void Core_LoadTerrain()
        {
            try
            {
                Byte[] terrain = Core.ReadData(this.TilesetTSA_PointerBox.Value, 0);

                List<UInt16> names = new List<UInt16>();
                Pointer address = Core.GetPointer("Map Terrain Names");
                while (address < Core.App.Game.FileSize)
                {
                    names.Add((UInt16)((Core.ReadByte(address + 1) << 8) | Core.ReadByte(address)));
                    if (names[names.Count - 1] == 0x0000) break;
                    address += 2;
                }
                this.CurrentTerrainNames = names.ToArray();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the terrain for this TSA.", ex);
            }
        }
        void Core_LoadTerrainValues()
        {
            this.Tileset_GridBox.SelectionChanged -= this.Tileset_GridBox_SelectionChanged;
            this.Terrain_Name_ShortBox.ValueChanged -= this.Terrain_Name_ShortBox_ValueChanged;
            this.Terrain_Stat_ByteBox.ValueChanged -= this.Terrain_Stat_ByteBox_ValueChanged;
            this.Terrain_Class_ByteBox.ValueChanged -= this.Terrain_Class_ByteBox_ValueChanged;

            try
            {
                Int32 entry = this.Terrain_ArrayBox.Value;

                this.Terrain_Name_ShortBox.Value = this.CurrentTerrainNames[entry];
                this.Terrain_Stat_ByteBox.Value = Core.ReadByte(this.Terrain_Stat_PointerArrayBox.Value + entry);
                this.Terrain_Class_ByteBox.Value = Core.ReadByte(this.Terrain_Class_PointerArrayBox.Value + entry);

                Int32 x = 0;
                Int32 y = 0;
                for (Int32 i = 0; i < this.CurrentTileset.Terrain.Length; i++)
                {
                    if (this.CurrentTileset.Terrain[i] == entry)
                    {
                        this.Tileset_GridBox.Selection[x, y] = true;
                    }
                    else this.Tileset_GridBox.Selection[x, y] = false;

                    x++;
                    if (x == 32)
                    {
                        x = 0;
                        y++;
                    }
                }
                this.Tileset_GridBox.Invalidate();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error trying to load the terrain values.", ex);
            }

            this.Tileset_GridBox.SelectionChanged += this.Tileset_GridBox_SelectionChanged;
            this.Terrain_Name_ShortBox.ValueChanged += this.Terrain_Name_ShortBox_ValueChanged;
            this.Terrain_Stat_ByteBox.ValueChanged += this.Terrain_Stat_ByteBox_ValueChanged;
            this.Terrain_Class_ByteBox.ValueChanged += this.Terrain_Class_ByteBox_ValueChanged;

            this.Terrain_Name_MagicButton.EntryToSelect = this.Terrain_Name_ShortBox.Value;
        }
        void Core_LoadTileAnim()
        {
            try
            {
                this.TileAnim_PointerBox.ValueChanged -= this.TileAnim_PointerBox_ValueChanged;
                this.TileAnim_PointerBox.Value = Core.ReadPointer(Core.GetPointer("Map Data Array") + this.TileAnim_ArrayBox.Value * 4);
                this.TileAnim_PointerBox.ValueChanged -= this.TileAnim_PointerBox_ValueChanged;

                this.CurrentTileAnim = new MapTileAnim(this.TileAnim_PointerBox.Value);

                this.TileAnim_Frame_ByteBox.Value = 0;
                this.TileAnim_Frame_ByteBox.Maximum = this.CurrentTileAnim.Frames.Count - 2;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the tile animation.", ex);
                this.CurrentTileAnim = null;
            }
        }
        void Core_LoadTileAnimFrame()
        {
            this.TileAnim_Data_PointerBox.ValueChanged -= this.TileAnim_Data_PointerBox_ValueChanged;
            this.TileAnim_Duration_ByteBox.ValueChanged -= this.TileAnim_Duration_ByteBox_ValueChanged;
            this.TileAnim_Length_ByteBox.ValueChanged -= this.TileAnim_Length_ByteBox_ValueChanged;
            this.TileAnim_Offset_ByteBox.ValueChanged -= this.TileAnim_Offset_ByteBox_ValueChanged;

            try
            {
                MapTileFrame frame = this.CurrentTileAnim.Frames[this.TileAnim_Frame_ByteBox.Value];

                this.TileAnim_Data_PointerBox.Value = frame.FrameData;
                this.TileAnim_Duration_ByteBox.Value = frame.Duration;
                this.TileAnim_Length_ByteBox.Value = frame.Length;

                if (frame.IsPaletteAnimation())
                {
                    this.TileAnim_Palette_RadioButton.Checked = true;
                    this.TileAnim_Tileset_RadioButton.Checked = false;
                    this.TileAnim_Offset_Label.Enabled = true;
                    this.TileAnim_Offset_ByteBox.Enabled = true;
                    this.TileAnim_Offset_ByteBox.Value = frame.Offset;

                    Byte[] change = Core.ReadData(frame.FrameData, frame.Length * 2);
                    this.TileAnim_PaletteBox.Load(new Palette(change));
                    this.TileAnim_ImageBox.Reset();
                }
                else
                {
                    this.TileAnim_Palette_RadioButton.Checked = false;
                    this.TileAnim_Tileset_RadioButton.Checked = true;
                    this.TileAnim_Offset_Label.Enabled = false;
                    this.TileAnim_Offset_ByteBox.Enabled = false;
                    this.TileAnim_Offset_ByteBox.Value = 0;

                    Byte[] tileset = Core.ReadData(frame.FrameData, frame.Length * 8 * Tile.LENGTH);
                    Byte[] palette = Core.ReadData(this.Palette_PointerBox.Value + this.ViewPalette_ByteBox.Value * Palette.LENGTH, Palette.LENGTH);
                    this.TileAnim_ImageBox.Load(new Tileset(tileset).ToImage(32, 4, palette));
                    this.TileAnim_PaletteBox.Reset();
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error trying to load the tileset values.", ex);
            }

            this.TileAnim_Data_PointerBox.ValueChanged += this.TileAnim_Data_PointerBox_ValueChanged;
            this.TileAnim_Duration_ByteBox.ValueChanged += this.TileAnim_Duration_ByteBox_ValueChanged;
            this.TileAnim_Length_ByteBox.ValueChanged += this.TileAnim_Length_ByteBox_ValueChanged;
            this.TileAnim_Offset_ByteBox.ValueChanged += this.TileAnim_Offset_ByteBox_ValueChanged;
        }
        void Core_LoadPaletteAndSmallTileset()
        {
            try
            {
                MapTileFrame frame = this.CurrentTileAnim.Frames[this.TileAnim_Frame_ByteBox.Value];
                Byte[] tileset = Core.ReadData(this.Tileset1_PointerBox.Value, 0);
                Byte[] palette = Core.ReadData(this.Palette_PointerBox.Value, Map.PALETTES * Palette.LENGTH);
                if (this.ViewAnimation_CheckBox.Checked)
                {
                    Byte[] change;
                    if (frame.IsPaletteAnimation())
                    {
                        change = this.TileAnim_PaletteBox.Colors.ToBytes(false);
                        Array.Copy(change, 0, palette, this.TileAnim_Offset_ByteBox.Value * 2, change.Length);
                    }
                    else
                    {
                        change = Core.ReadData(frame.FrameData, frame.Length * 8 * Tile.LENGTH);
                        Array.Copy(change, 0, tileset, 32 * 8 * Tile.LENGTH, change.Length);
                    }
                }
                this.Tileset_PaletteBox.Load(new GBA.Palette(palette, Map.PALETTES * Palette.MAX));
                this.Tileset_ImageBox.Load(new Tileset(tileset).ToImage(32, 32,
                    palette.GetBytes((UInt32)(this.ViewPalette_ByteBox.Value * Palette.LENGTH), Palette.LENGTH)));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the palette.", ex);
                this.CurrentTileAnim = null;
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
                    Tuple.Create("Palette", this.Palette_PointerBox.Value, data_palette.Length),
                    Tuple.Create("Tileset 1", this.Tileset1_PointerBox.Value, data_tileset1.Length),
                    Tuple.Create("Tileset 2", this.Tileset2_PointerBox.Value, data_tileset2.Length),
                    Tuple.Create("TSA and Terrain", this.TilesetTSA_PointerBox.Value, data_tsa.Length),
                };
                Pointer address = Core.GetPointer("Map Data Array");
                var writepos = new List<Pointer>()
                {
                    address + 4 * this.Palette_ArrayBox.Value,
                    address + 4 * this.Tileset1_ArrayBox.Value,
                    address + 4 * this.Tileset2_ArrayBox.Value,
                    address + 4 * this.TilesetTSA_ArrayBox.Value,
                };

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Map Tileset",
                    "The map tileset to insert may need some of its parts to be repointed.",
                    this.CurrentEntry_Terrain, repoints.ToArray(), writepos.ToArray());
                if (cancel) return;

                Core.WriteData(this,
                    Core.ReadPointer(writepos[0]),
                    data_palette,
                    "Map Data Array 0x" + Util.ByteToHex(this.Palette_ArrayBox.Value) + " - Palette written");

                Core.WriteData(this,
                    Core.ReadPointer(writepos[1]),
                    data_tileset1,
                    "Map Data Array 0x" + Util.ByteToHex(this.Tileset1_ArrayBox.Value) + " - Tileset 1 written");

                Core.WriteData(this,
                    Core.ReadPointer(writepos[2]),
                    data_tileset2,
                    "Map Data Array 0x" + Util.ByteToHex(this.Tileset2_ArrayBox.Value) + " - Tileset 2 written");

                Core.WriteData(this,
                    Core.ReadPointer(writepos[3]),
                    data_tsa,
                    "Map Data Array 0x" + Util.ByteToHex(this.TilesetTSA_ArrayBox.Value) + " - TSA and Terrain written");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert image file.", ex);
                this.Core_Update();
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
            this.Core_Insert(tileset);
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
            this.Core_Insert(tileset);
        }
        void Core_SaveImage(String filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    this.CurrentTileset.Width,
                    this.CurrentTileset.Height,
                    this.CurrentTileset.Palettes,
                    delegate (Int32 x, Int32 y)
                    {
                        return (Byte)this.CurrentTileset[x, y];
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

                Byte[] tileset1 = Core.ReadData(this.Tileset1_PointerBox.Value, 0);
                Byte[] tileset2 = Core.ReadData(this.Tileset2_PointerBox.Value, 0);
                Byte[] data_tileset = new Byte[tileset1.Length + tileset2.Length];
                Array.Copy(tileset1, 0, data_tileset, 0, tileset1.Length);
                Array.Copy(tileset2, 0, data_tileset, tileset1.Length, tileset2.Length);
                Byte[] data_palette = Core.ReadData(this.Palette_PointerBox.Value, Map.PALETTES * Palette.LENGTH);
                Byte[] data_tsa = Core.ReadData(this.TilesetTSA_PointerBox.Value, 0);

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
                    this.Core_InsertData(openWindow.FileName);
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
                    this.Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_SaveData(saveWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }
        private void Tools_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                "Map Palette 0x" + Util.ByteToHex(this.Palette_ArrayBox.Value) + " [" + this.Palette_ArrayBox.Text + "] - ",
                this.Palette_PointerBox.Value, 10);
        }



        private void Tileset_GridBox_SelectionChanged(Object sender, EventArgs e)
        {
            try
            {
                const Int32 offset = 2 * MapTileset.WIDTH * MapTileset.HEIGHT;
                Byte[] terrain = Core.ReadData(this.TilesetTSA_PointerBox.Value, 0);
                Int32 width  = this.Tileset_GridBox.Selection.GetLength(0);
                Int32 height = this.Tileset_GridBox.Selection.GetLength(1);
                Int32 index = 0;
                for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                    if (this.Tileset_GridBox.Selection[x, y])
                    {
                            this.CurrentTileset.Terrain[index] = this.Terrain_ArrayBox.Value;
                    }
                    index++;
                }
                Array.Copy(this.CurrentTileset.Terrain, 0, terrain, offset, this.CurrentTileset.Terrain.Length);

                Core.WriteData(this,
                    this.TilesetTSA_PointerBox.Value,
                    terrain,
                    "Map " + this.TilesetTSA_ArrayBox.Text + " - Terrain mapping changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to write the terrain map", ex);
            }
        }

        private void ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void ViewPalette_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_LoadTileAnimFrame();
            this.Core_LoadPaletteAndSmallTileset();
        }
        private void ViewAnimation_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_LoadPaletteAndSmallTileset();
        }

        private void Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Palette_ArrayBox.Value,
                this.Palette_PointerBox.Value,
                this.Palette_ArrayBox.Text + "Palette repoint");
        }
        private void Tileset1_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Tileset1_ArrayBox.Value,
                this.Tileset1_PointerBox.Value,
                this.Tileset1_ArrayBox.Text + "Tileset 1 repoint");
        }
        private void Tileset2_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Tileset2_ArrayBox.Value,
                this.Tileset2_PointerBox.Value,
                this.Tileset2_ArrayBox.Text + "Tileset 2 repoint");
        }
        private void TilesetTSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.TilesetTSA_ArrayBox.Value,
                this.TilesetTSA_PointerBox.Value,
                this.TilesetTSA_ArrayBox.Text + "Tileset TSA repoint");
        }



        private void Terrain_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_LoadTerrainValues();
        }
        private void Terrain_Name_ShortBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                Core.GetPointer("Map Terrain Names") + this.Terrain_ArrayBox.Value * 2,
                Util.UInt16ToBytes(this.Terrain_Name_ShortBox.Value, true),
                this.CurrentEntry_Terrain + "Name text index changed");

            this.Terrain_Name_MagicButton.EntryToSelect = this.Terrain_Name_ShortBox.Value;
        }

        private void Terrain_Stat_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Terrain_Stat_ByteBox.ValueChanged -= this.Terrain_Stat_ByteBox_ValueChanged;
            this.Terrain_Stat_ByteBox.Value = Core.ReadByte(this.Terrain_Stat_PointerArrayBox.Value + this.Terrain_ArrayBox.Value);
            this.Terrain_Stat_ByteBox.ValueChanged += this.Terrain_Stat_ByteBox_ValueChanged;
        }
        private void Terrain_Stat_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Terrain_Stat_PointerArrayBox.Value + this.Terrain_ArrayBox.Value,
                this.Terrain_Stat_ByteBox.Value,
                this.CurrentEntry_Terrain + this.Terrain_Stat_PointerArrayBox.Text + " changed");
        }

        private void Terrain_Class_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Terrain_Class_ByteBox.ValueChanged -= this.Terrain_Class_ByteBox_ValueChanged;
            this.Terrain_Class_ByteBox.Value = Core.ReadByte(this.Terrain_Class_PointerArrayBox.Value + this.Terrain_ArrayBox.Value);
            this.Terrain_Class_ByteBox.ValueChanged += this.Terrain_Class_ByteBox_ValueChanged;
        }
        private void Terrain_Class_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Terrain_Class_PointerArrayBox.Value + this.Terrain_ArrayBox.Value,
                this.Terrain_Class_ByteBox.Value,
                this.CurrentEntry_Terrain + "Move Cost at " + this.Terrain_Class_PointerArrayBox.Value + " changed");
        }



        private void TileAnim_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_LoadTileAnim();
            this.Core_LoadTileAnimFrame();
            this.Core_LoadPaletteAndSmallTileset();
        }
        private void TileAnim_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.TileAnim_ArrayBox.Value,
                this.TileAnim_PointerBox.Value,
                this.TileAnim_ArrayBox.Text + "Tile Anim 1 repoint");
        }

        private void TileAnim_Frame_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_LoadTileAnimFrame();
            this.Core_LoadPaletteAndSmallTileset();
        }
        private void TileAnim_RadioButton_Click(Object sender, EventArgs e)
        {

        }

        private void TileAnim_Data_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = this.TileAnim_Frame_ByteBox.Value;
            Pointer address = this.CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (this.CurrentTileAnim.Frames[frame].IsPaletteAnimation())
                address += 0;
            else address += 4;

            Core.WritePointer(this,
               address,
               this.TileAnim_Data_PointerBox.Value,
               this.CurrentEntry_TileAnim + "Frame " + frame + " Data repointed");
        }
        private void TileAnim_Duration_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = this.TileAnim_Frame_ByteBox.Value;
            Pointer address = this.CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (this.CurrentTileAnim.Frames[frame].IsPaletteAnimation())
                address += 4;
            else address += 0;

            Core.WriteByte(this,
               address,
               this.TileAnim_Duration_ByteBox.Value,
               this.CurrentEntry_TileAnim + "Frame " + frame + " Duration changed");
        }
        private void TileAnim_Length_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = this.TileAnim_Frame_ByteBox.Value;
            Pointer address = this.CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (this.CurrentTileAnim.Frames[frame].IsPaletteAnimation())
                address += 5;
            else address += 3;

            Core.WriteByte(this,
               address,
               this.TileAnim_Length_ByteBox.Value,
               this.CurrentEntry_TileAnim + "Frame " + frame + " Data Length changed");
        }
        private void TileAnim_Offset_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 frame = this.TileAnim_Frame_ByteBox.Value;
            Pointer address = this.CurrentTileAnim.Address + frame * MapTileFrame.LENGTH;
            if (this.CurrentTileAnim.Frames[frame].IsPaletteAnimation())
            {
                Core.WriteByte(this,
                   address + 6,
                   this.TileAnim_Duration_ByteBox.Value,
                   this.CurrentEntry_TileAnim + "Frame " + frame + " Color Offset changed");
            }
        }
    }
}
