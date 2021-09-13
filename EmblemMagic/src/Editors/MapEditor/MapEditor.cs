using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using EmblemMagic.FireEmblem;
using Magic;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class MapEditor : Editor
    {
        /// <summary>
        /// The entry in the chapter array from which to read values to load the map
        /// </summary>
        StructFile Current;
        /// <summary>
        /// The tileset loaded for the current chapter/map
        /// </summary>
        MapTileset CurrentTileset;
        /// <summary>
        /// The map currently being edited.
        /// </summary>
        FireEmblem.Map CurrentMap;
        /// <summary>
        /// Gets a string of the current byte index in the chapter array
        /// </summary>
        String CurrentEntry
        {
            get
            {
                return "Chapter 0x" + Util.ByteToHex(this.EntryArrayBox.Value) + " [" + this.EntryArrayBox.Text + "] - ";
            }
        }



        public MapEditor()
        {
            try
            {
                this.InitializeComponent();

                this.EntryArrayBox.Load("Chapter List.txt");
                this.Current = new StructFile("Chapter Struct.txt");
                this.Current.Address = Core.GetPointer("Chapter Array");

                ArrayFile map_file = new ArrayFile("Map List.txt");
                this.MapData_ArrayBox.Load(map_file);
                this.Changes_ArrayBox.Load(map_file);
                this.TileAnim1_ArrayBox.Load(map_file);
                this.TileAnim2_ArrayBox.Load(map_file);
                this.Palette_ArrayBox.Load(map_file);
                this.TilesetTSA_ArrayBox.Load(map_file);
                this.Tileset1_ArrayBox.Load(map_file);
                this.Tileset2_ArrayBox.Load(map_file);

                this.Chapter_MagicButton.EditorToOpen = "Module:Chapter Editor";
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        override public void Core_OnOpen()
        {
            this.Core_Update();
        }
        override public void Core_Update()
        {
            this.Current.EntryIndex = this.EntryArrayBox.Value;

            this.Core_LoadValues();
            this.Core_LoadTileset();
            this.Core_LoadMap();
            this.Core_LoadMapValues();
        }

        void Core_LoadTileset()
        {
            try
            {
                Byte[] palette_data = Core.ReadData(this.Palette_PointerBox.Value, Map.PALETTES * GBA.Palette.LENGTH);

                this.CurrentTileset = new MapTileset(palette_data,
                    Core.ReadData(this.Tileset1_PointerBox.Value, 0),
                    Core.ReadData(this.Tileset2_PointerBox.Value, 0),
                    Core.ReadData(this.TilesetTSA_PointerBox.Value, 0));

                this.Tileset_GridBox.Load(this.CurrentTileset);
                this.Palette_PaletteBox.Load(new GBA.Palette(palette_data, Map.PALETTES * GBA.Palette.MAX));
            }
            catch (Exception ex)
            {
                this.CurrentTileset = null;
                UI.ShowError("Could not load the the tileset for this chapter.", ex);
            }
        }
        void Core_LoadMap()
        {
            try
            {
                this.CurrentMap = new Map(
                    this.CurrentTileset,
                    Core.ReadData(this.MapData_PointerBox.Value, 0),
                    this.Changes_PointerBox.Value);

                this.Map_GridBox.Load(this.CurrentMap);
            }
            catch (Exception ex)
            {
                this.CurrentMap = null;
                this.Map_GridBox.Load(this.CurrentMap);
                UI.ShowError("Could not read the map for this chapter.", ex);
            }
        }
        void Core_LoadValues()
        {
            this.MapData_ArrayBox.ValueChanged -= this.MapData_ArrayBox_ValueChanged;
            this.Changes_ArrayBox.ValueChanged -= this.Changes_ArrayBox_ValueChanged;
            this.TileAnim1_ArrayBox.ValueChanged -= this.TileAnim1_ArrayBox_ValueChanged;
            this.TileAnim2_ArrayBox.ValueChanged -= this.TileAnim2_ArrayBox_ValueChanged;
            this.Palette_ArrayBox.ValueChanged -= this.Palette_ArrayBox_ValueChanged;
            this.TilesetTSA_ArrayBox.ValueChanged -= this.TilesetTSA_ArrayBox_ValueChanged;
            this.Tileset1_ArrayBox.ValueChanged -= this.Tileset1_ArrayBox_ValueChanged;
            this.Tileset2_ArrayBox.ValueChanged -= this.Tileset2_ArrayBox_ValueChanged;

            this.MapData_PointerBox.ValueChanged -= this.MapData_PointerBox_ValueChanged;
            this.Changes_PointerBox.ValueChanged -= this.Changes_PointerBox_ValueChanged;
            this.TileAnim1_PointerBox.ValueChanged -= this.TileAnim1_PointerBox_ValueChanged;
            this.TileAnim2_PointerBox.ValueChanged -= this.TileAnim2_PointerBox_ValueChanged;
            this.Palette_PointerBox.ValueChanged -= this.Palette_PointerBox_ValueChanged;
            this.TilesetTSA_PointerBox.ValueChanged -= this.TilesetTSA_PointerBox_ValueChanged;
            this.Tileset1_PointerBox.ValueChanged -= this.Tileset1_PointerBox_ValueChanged;
            this.Tileset2_PointerBox.ValueChanged -= this.Tileset2_PointerBox_ValueChanged;
            
            try
            {
                this.MapData_ArrayBox.Value    = (Byte)this.Current["Map"];
                this.Changes_ArrayBox.Value    = (Byte)this.Current["MapChanges"];
                this.TileAnim1_ArrayBox.Value  = (Byte)this.Current["TileAnim1"];
                this.TileAnim2_ArrayBox.Value  = (Byte)this.Current["TileAnim2"];
                this.Palette_ArrayBox.Value    = (Byte)this.Current["Palette"];
                this.TilesetTSA_ArrayBox.Value = (Byte)this.Current["TSA"];
                this.Tileset1_ArrayBox.Value   = (Byte)this.Current["Tileset1"];
                this.Tileset2_ArrayBox.Value   = (Byte)this.Current["Tileset2"];

                GBA.Pointer address = Core.GetPointer("Map Data Array");
                this.MapData_PointerBox.Value    = Core.ReadPointer(address + 4 * this.MapData_ArrayBox.Value);
                this.Changes_PointerBox.Value    = Core.ReadPointer(address + 4 * this.Changes_ArrayBox.Value);
                this.TileAnim1_PointerBox.Value  = Core.ReadPointer(address + 4 * this.TileAnim1_ArrayBox.Value);
                this.TileAnim2_PointerBox.Value  = Core.ReadPointer(address + 4 * this.TileAnim2_ArrayBox.Value);
                this.Palette_PointerBox.Value    = Core.ReadPointer(address + 4 * this.Palette_ArrayBox.Value);
                this.TilesetTSA_PointerBox.Value = Core.ReadPointer(address + 4 * this.TilesetTSA_ArrayBox.Value);
                this.Tileset1_PointerBox.Value   = Core.ReadPointer(address + 4 * this.Tileset1_ArrayBox.Value);
                this.Tileset2_PointerBox.Value   = Core.ReadPointer(address + 4 * this.Tileset2_ArrayBox.Value);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);
            }

            this.MapData_ArrayBox.ValueChanged += this.MapData_ArrayBox_ValueChanged;
            this.Changes_ArrayBox.ValueChanged += this.Changes_ArrayBox_ValueChanged;
            this.TileAnim1_ArrayBox.ValueChanged += this.TileAnim1_ArrayBox_ValueChanged;
            this.TileAnim2_ArrayBox.ValueChanged += this.TileAnim2_ArrayBox_ValueChanged;
            this.Palette_ArrayBox.ValueChanged += this.Palette_ArrayBox_ValueChanged;
            this.TilesetTSA_ArrayBox.ValueChanged += this.TilesetTSA_ArrayBox_ValueChanged;
            this.Tileset1_ArrayBox.ValueChanged += this.Tileset1_ArrayBox_ValueChanged;
            this.Tileset2_ArrayBox.ValueChanged += this.Tileset2_ArrayBox_ValueChanged;

            this.MapData_PointerBox.ValueChanged += this.MapData_PointerBox_ValueChanged;
            this.Changes_PointerBox.ValueChanged += this.Changes_PointerBox_ValueChanged;
            this.TileAnim1_PointerBox.ValueChanged += this.TileAnim1_PointerBox_ValueChanged;
            this.TileAnim2_PointerBox.ValueChanged += this.TileAnim2_PointerBox_ValueChanged;
            this.Palette_PointerBox.ValueChanged += this.Palette_PointerBox_ValueChanged;
            this.TilesetTSA_PointerBox.ValueChanged += this.TilesetTSA_PointerBox_ValueChanged;
            this.Tileset1_PointerBox.ValueChanged += this.Tileset1_PointerBox_ValueChanged;
            this.Tileset2_PointerBox.ValueChanged += this.Tileset2_PointerBox_ValueChanged;

            this.Chapter_MagicButton.EntryToSelect = this.EntryArrayBox.Value;
        }
        void Core_LoadMapValues()
        {
            if (this.Changes_PointerBox.Value == new GBA.Pointer())
            {
                this.Changes_CheckBox.Enabled = false;
            }
            else
            {
                this.Changes_CheckBox.Enabled = true;
            }
            this.Changes_CheckBox.CheckedChanged -= this.Changes_CheckBox_CheckedChanged;
            this.Changes_CheckBox.Checked = false;
            this.Changes_CheckBox.CheckedChanged += this.Changes_CheckBox_CheckedChanged;

            this.Changes_Total_NumBox.Value = (this.CurrentMap == null || this.CurrentMap.Changes == null) ?
                (Byte)(0x00) : (Byte)(this.CurrentMap.Changes.Count - 1);
            this.Changes_Total_NumBox.Enabled = false;
            this.Changes_Total_Label.Enabled = false;
            this.Changes_NumBox.Enabled = false;
            this.Changes_NumBox.ValueChanged -= this.Changes_NumBox_ValueChanged;
            this.Changes_NumBox.Value = 0;
            this.Changes_NumBox.Maximum = this.Changes_Total_NumBox.Value;
            this.Changes_NumBox.ValueChanged += this.Changes_NumBox_ValueChanged;

            this.View_AltPalette.Click -= this.View_AltPalette_Click;
            this.View_AltPalette.Checked = false;
            this.View_AltPalette.Click += this.View_AltPalette_Click;

            this.Map_Width_NumBox.ValueChanged -= this.Map_WidthNumBox_ValueChanged;
            this.Map_Height_NumBox.ValueChanged -= this.Map_HeightNumBox_ValueChanged;
            this.Map_Width_NumBox.Value = (this.CurrentMap == null) ? 15 : this.CurrentMap.WidthTiles;
            this.Map_Height_NumBox.Value = (this.CurrentMap == null) ? 10 : this.CurrentMap.HeightTiles;
            this.Map_Width_NumBox.ValueChanged += this.Map_WidthNumBox_ValueChanged;
            this.Map_Height_NumBox.ValueChanged += this.Map_HeightNumBox_ValueChanged;
        }

        /// <summary>
        /// Writes the CurrentMap onto the ROM
        /// </summary>
        void Core_WriteMap()
        {
            Boolean[,] selected = this.Tileset_GridBox.Selection;
            Boolean viewpalette = this.View_AltPalette.Checked;
            Boolean changes = this.Changes_CheckBox.Checked;
            Byte number = this.Changes_NumBox.Value;

            Core.WriteData(this,
                this.MapData_PointerBox.Value,
                this.CurrentMap.ToBytes(),
                this.CurrentEntry + "Map changed");

            this.Tileset_GridBox.Selection = selected;
            this.View_AltPalette.Checked = viewpalette;
            this.Changes_CheckBox.Checked = changes;
            this.Changes_NumBox.Value = number;
        }
        /// <summary>
        /// Writes the triggerable map changes associated with the CurrentMap onto the ROM
        /// </summary>
        void Core_WriteMapChanges()
        {
            Boolean[,] selected = this.Tileset_GridBox.Selection;
            Boolean viewpalette = this.View_AltPalette.Checked;
            Boolean changes = this.Changes_CheckBox.Checked;
            Byte number = this.Changes_NumBox.Value;

            Core.WriteData(this,
                this.Changes_PointerBox.Value,
                this.CurrentMap.Changes.ToBytes(this.Changes_PointerBox.Value),
                this.CurrentEntry + "Map Changes changed");

            this.Tileset_GridBox.Selection = selected;
            this.View_AltPalette.Checked = viewpalette;
            this.Changes_CheckBox.Checked = changes;
            this.Changes_NumBox.Value = number;
        }

        void Core_InsertMAR(String filepath, Int32 width, Int32 height)
        {
            Byte[] mar;
            try
            {
                mar = File.ReadAllBytes(filepath);
            }
            catch (Exception ex)
            {
                UI.ShowError(filepath + " cannot be read.", ex);
                return;
            }

            if (width * height * 2 != mar.Length)
            {
                MessageBox.Show("The size of the map does not match the file.");
                return;
            }

            Int32[,] layout = new Int32[width, height];
            Int32 x = 0;
            Int32 y = 0;
            for (Int32 i = 0; i < mar.Length; i += 2)
            {
                Int32 tile = mar[i] + (mar[i + 1] << 8);
                tile >>= 3;
                layout[x, y] = tile;
                x++;
                if (x == width)
                {
                    x = 0;
                    y++;
                }
            }

            this.CurrentMap.Layout = layout;

            this.Core_WriteMap();
        }
        void Core_InsertTMX(String filepath, Int32 width, Int32 height)
        {
            XmlDocument file = new XmlDocument();
            file.Load(filepath);

            width = Int32.Parse(file["map"].Attributes["width"].Value);
            height = Int32.Parse(file["map"].Attributes["height"].Value);
            this.CurrentMap.Layout = new Int32[width, height];

            List <Tuple<Byte, Rectangle, Int32[,]>> changes = new List<Tuple<Byte, Rectangle, Int32[,]>>();
            XmlNodeList layers = file.SelectNodes("/map/layer");
            XmlNodeList data;
            foreach (XmlNode layer in layers)
            {
                if (layer["properties"]["property"].Attributes["name"].Value == "Main")
                {
                    data = layer.SelectNodes("data/tile");
                    Int32 x = 0;
                    Int32 y = 0;
                    foreach (XmlNode tile in data)
                    {
                        this.CurrentMap.Layout[x, y] = Int32.Parse(tile.Attributes["gid"].Value) - 1;
                        x++;
                        if (x == width)
                        {
                            x = 0;
                            y++;
                        }
                    }
                }
                else
                {
                    String xpath = "//properties/property[@name=" + '"' + "{0}" + '"' + "]";
                    Byte index = Byte.Parse(layer.SelectSingleNode(String.Format(xpath, "ID")).Attributes["value"].Value);
                    Rectangle region = new Rectangle(
                        Int32.Parse(layer.SelectSingleNode(String.Format(xpath, "X")).Attributes["value"].Value),
                        Int32.Parse(layer.SelectSingleNode(String.Format(xpath, "Y")).Attributes["value"].Value),
                        Int32.Parse(layer.SelectSingleNode(String.Format(xpath, "Width")).Attributes["value"].Value),
                        Int32.Parse(layer.SelectSingleNode(String.Format(xpath, "Height")).Attributes["value"].Value));
                    Int32[,] layout = new Int32[region.Width, region.Height];

                    data = layer.SelectNodes("data/tile");
                    Int32 x = 0;
                    Int32 y = 0;
                    foreach (XmlNode tile in data)
                    {
                        if (x >= region.X && x < region.X + region.Width &&
                            y >= region.Y && y < region.Y + region.Height)
                        {
                            layout[x - region.X, y - region.Y] = Int32.Parse(tile.Attributes["gid"].Value) - 1;
                        }
                        x++;
                        if (x == width)
                        {
                            x = 0;
                            y++;
                        }
                    }
                    changes.Add(Tuple.Create(index, region, layout));
                }
            }

            UI.SuspendUpdate();
            try
            {
                this.Core_WriteMap();
                if (changes.Count > 0)
                {
                    this.CurrentMap.Changes = new MapChanges(changes);
                    this.Core_WriteMapChanges();
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the TMX file.", ex);
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_SaveMAR(String filepath, Int32 width, Int32 height)
        {
            Byte[] mar = new Byte[width * height * 2];

            Int32 x = 0;
            Int32 y = 0;
            for (Int32 i = 0; i < mar.Length; i += 2)
            {
                Int32 tile = this.CurrentMap.Layout[x, y] << 3;
                mar[i] = (Byte)(tile & 0xFF);
                mar[i + 1] = (Byte)(tile >> 8);
                x++;
                if (x == width)
                {
                    x = 0;
                    y++;
                }
            }

            File.WriteAllBytes(filepath, mar);
        }
        void Core_SaveTMX(String filepath, Int32 width, Int32 height)
        {

        }

        void Core_TileTool()
        {
            Rectangle selection = this.Tileset_GridBox.GetSelectionRectangle();

            if (this.Changes_CheckBox.Checked)
            {
                for (Int32 y = 0; y < selection.Height; y++)
                for (Int32 x = 0; x < selection.Width; x++)
                {
                    if (this.Map_GridBox.Hovered.X + x < 0 || this.Map_GridBox.Hovered.X + x >= this.CurrentMap.WidthTiles ||
                        this.Map_GridBox.Hovered.Y + y < 0 || this.Map_GridBox.Hovered.Y + y >= this.CurrentMap.HeightTiles)
                            continue;

                    if (this.Map_GridBox.Hover[x, y])
                    {
                            this.CurrentMap.Changes.SetTile(
                            this.Changes_NumBox.Value,
                            this.Map_GridBox.Hovered.X + x,
                            this.Map_GridBox.Hovered.Y + y,
                            (selection.X + x) + (selection.Y + y) * 32);
                    }
                }

                UI.SuspendUpdate();
                this.Core_WriteMapChanges();
                UI.ResumeUpdate();
            }
            else if (this.Map_GridBox.Hover != null)
            {
                for (Int32 y = 0; y < selection.Height; y++)
                for (Int32 x = 0; x < selection.Width; x++)
                {
                    if (this.Map_GridBox.Hovered.X + x < 0 || this.Map_GridBox.Hovered.X + x >= this.CurrentMap.WidthTiles ||
                        this.Map_GridBox.Hovered.Y + y < 0 || this.Map_GridBox.Hovered.Y + y >= this.CurrentMap.HeightTiles)
                            continue;

                    if (this.Map_GridBox.Hover[x, y])
                    {
                            this.CurrentMap.Layout[this.Map_GridBox.Hovered.X + x,
                                          this.Map_GridBox.Hovered.Y + y] =
                            (selection.X + x) + (selection.Y + y) * 32;
                    }
                }
            }
        }
        void Core_EraseTool()
        {
            if (this.Changes_CheckBox.Checked)
            {
                this.CurrentMap.Changes.SetTile(
                    (Int32)this.Changes_NumBox.Value,
                    this.Map_GridBox.Hovered.X,
                    this.Map_GridBox.Hovered.Y,
                    0);

                UI.SuspendUpdate();
                this.Core_WriteMapChanges();
                UI.ResumeUpdate();
            }
            else
            {
                this.CurrentMap.Layout[this.Map_GridBox.Hovered.X, this.Map_GridBox.Hovered.Y] = 0;
            }
        }
        void Core_PickTool()
        {
            Int32 width = this.Tileset_GridBox.Selection.GetLength(0);
            Int32 height = this.Tileset_GridBox.Selection.GetLength(1);
            Int32 tile;
            if (this.Changes_CheckBox.Checked)
            {
                tile = this.CurrentMap.Changes.GetTile((Int32)this.Changes_NumBox.Value, this.Map_GridBox.Hovered.X, this.Map_GridBox.Hovered.Y);
                if (tile == 0) tile = this.CurrentMap.Layout[this.Map_GridBox.Hovered.X, this.Map_GridBox.Hovered.Y];
            }
            else
            {
                tile = this.CurrentMap.Layout[this.Map_GridBox.Hovered.X, this.Map_GridBox.Hovered.Y];
            }
            this.Tileset_GridBox.Selection = new Boolean[width, height];
            this.Tileset_GridBox.Selection[tile % width, tile / width] = true;
        }
        void Core_FillTool()
        {
            Rectangle selection = this.Tileset_GridBox.GetSelectionRectangle();
            Int32[] tiles = new Int32[this.Tileset_GridBox.GetSelectionAmount()];
            Int32 index = 0;
            for (Int32 y = 0; y < selection.Height; y++)
            for (Int32 x = 0; x < selection.Width; x++)
            {
                if (this.Tileset_GridBox.Selection[selection.X + x, selection.Y + y])
                {
                    tiles[index++] = (selection.X + x) + (selection.Y + y) * 32;
                }
            }
            if (tiles.Length == 0)
            {
                UI.ShowMessage("No tile is selected in the tileset.");
            }
            else
            {
                Point start = this.Map_GridBox.Hovered;
                Boolean[,] map = new Boolean[this.CurrentMap.WidthTiles, this.CurrentMap.HeightTiles];
                Int32 tile = this.CurrentMap.Layout[start.X, start.Y];
                this.Core_FillHelper(ref map, ref tile, start.X, start.Y);

                Random random = new Random();
                for (Int32 y = 0; y < this.CurrentMap.HeightTiles; y++)
                for (Int32 x = 0; x < this.CurrentMap.WidthTiles; x++)
                {
                    if (map[x, y])
                    {
                            this.CurrentMap.Layout[x, y] = tiles[random.Next(tiles.Length)];
                    }
                }
            }
        }
        void Core_FillHelper(ref Boolean[,] map, ref Int32 tile, Int32 x, Int32 y)
        {
            if (x < 0 || x >= this.CurrentMap.WidthTiles
             || y < 0 || y >= this.CurrentMap.HeightTiles
             || map[x, y]
             || this.CurrentMap.Layout[x, y] != tile)
                return;
            else
            {
                map[x, y] = true;

                this.Core_FillHelper(ref map, ref tile, x - 1, y);
                this.Core_FillHelper(ref map, ref tile, x + 1, y);
                this.Core_FillHelper(ref map, ref tile, x, y - 1);
                this.Core_FillHelper(ref map, ref tile, x, y + 1);
            }
        }



        private void File_Insert_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "TMX file (*.tmx)|*.tmx|" +
                "MAR file (*.mar)|*.mar|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".tmx"))
                {
                    this.Core_InsertTMX(openWindow.FileName, (Int32)this.Map_Width_NumBox.Value, (Int32)this.Map_Height_NumBox.Value);
                    return;
                }
                if (openWindow.FileName.EndsWith(".mar"))
                {
                    this.Core_InsertMAR(openWindow.FileName, (Int32)this.Map_Width_NumBox.Value, (Int32)this.Map_Height_NumBox.Value);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_Save_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "TMX file (*.tmx)|*.tmx|" +
                "MAR file (*.mar)|*.mar|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".tmx", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_SaveTMX(saveWindow.FileName, (Int32)this.Map_Width_NumBox.Value, (Int32)this.Map_Height_NumBox.Value);
                    return;
                }
                if (saveWindow.FileName.EndsWith(".mar", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_SaveMAR(saveWindow.FileName, (Int32)this.Map_Width_NumBox.Value, (Int32)this.Map_Height_NumBox.Value);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }
        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                "Map Palette 0x" + Util.ByteToHex(this.Palette_ArrayBox.Value) + " [" + this.Palette_ArrayBox.Text + "] - ",
                this.Palette_PointerBox.Value, 10);
        }
        private void View_AltPalette_Click(Object sender, EventArgs e)
        {
            this.CurrentMap.ShowFog = this.View_AltPalette.Checked;

            this.Map_GridBox.Load(this.CurrentMap);
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void Map_GridBox_MouseMove(Object sender, MouseEventArgs e)
        {
            if (this.Tileset_GridBox.SelectionIsEmpty())
            {
                this.Map_GridBox.Hover = new Boolean[1, 1] { { false } };
            }
            else if (this.Tool_Tile_Button.Checked)
            {
                Rectangle selection = this.Tileset_GridBox.GetSelectionRectangle();
                this.Map_GridBox.Hover = new Boolean[selection.Width, selection.Height];
                for (Int32 y = 0; y < selection.Height; y++)
                    for (Int32 x = 0; x < selection.Width; x++)
                    {
                        this.Map_GridBox.Hover[x, y] = this.Tileset_GridBox.Selection[selection.X + x, selection.Y + y];
                    }
            }
            else
            {
                this.Map_GridBox.Hover = new Boolean[1, 1] { { true } };
            }
        }
        private void Map_GridBox_MouseDown(Object sender, EventArgs e)
        {
            if (this.Tool_Tile_Button.Checked)
            {
                this.Core_TileTool();
                this.Map_MouseTimer.Enabled = true;
                this.Map_GridBox.Load(this.CurrentMap);
            }
            else if (this.Tool_Fill_Button.Checked)
            {
                this.Core_FillTool();
                this.Map_MouseTimer.Enabled = true;
                this.Map_GridBox.Load(this.CurrentMap);
            }
            else if (this.Tool_Erase_Button.Checked)
            {
                this.Core_EraseTool();
                this.Map_MouseTimer.Enabled = true;
                this.Map_GridBox.Load(this.CurrentMap);
            }
            else if (this.Tool_Pick_Button.Checked)
            {
                this.Core_PickTool();
                this.Map_MouseTimer.Enabled = false;
            }
            else
            {
                UI.ShowMessage("No tool is selected.");
            }
        }
        private void Map_GridBox_MouseUp(Object sender, EventArgs e)
        {
            if (this.Tool_Pick_Button.Checked) return;
            this.Map_MouseTimer.Enabled = false;
            this.Core_WriteMap();
        }
        private void Map_MouseTimer_Tick(Object sender, EventArgs e)
        {
            this.Map_GridBox_MouseDown(this, null);
        }

        private void Map_WidthNumBox_ValueChanged(Object sender, EventArgs e)
        {
            this.CurrentMap.WidthTiles = (Byte)this.Map_Width_NumBox.Value;

            this.Core_WriteMap();
        }
        private void Map_HeightNumBox_ValueChanged(Object sender, EventArgs e)
        {
            this.CurrentMap.HeightTiles = (Byte)this.Map_Height_NumBox.Value;

            this.Core_WriteMap();
        }

        private void Changes_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.CurrentMap.ShowChanges = new Boolean[this.Changes_Total_NumBox.Value];
            this.CurrentMap.ShowChanges[0] = this.Changes_CheckBox.Checked;

            this.Tool_Fill_Button.Enabled = !this.Changes_CheckBox.Checked;

            this.Changes_Total_NumBox.Enabled = this.Changes_CheckBox.Checked;
            this.Changes_Total_Label.Enabled = this.Changes_CheckBox.Checked;
            this.Changes_NumBox.Enabled = this.Changes_CheckBox.Checked;
            this.Changes_NumBox.Value = 0;

            this.Map_GridBox.Load(this.CurrentMap);
        }
        private void Changes_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            this.CurrentMap.ShowChanges = new Boolean[this.Changes_Total_NumBox.Value + 1];
            this.CurrentMap.ShowChanges[this.Changes_NumBox.Value] = true;
            this.Map_GridBox.Load(this.CurrentMap);
        }

        private void Palette_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Palette"),
                this.Palette_ArrayBox.Value,
                this.CurrentEntry + "Palette changed");
        }
        private void Tileset1_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Tileset1"),
                this.Tileset1_ArrayBox.Value,
                this.CurrentEntry + "Tileset 1 changed");
        }
        private void Tileset2_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Tileset2"),
                this.Tileset2_ArrayBox.Value,
                this.CurrentEntry + "Tileset 2 changed");
        }
        private void TilesetTSA_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "TSA"),
                this.TilesetTSA_ArrayBox.Value,
                this.CurrentEntry + "Tileset TSA changed");
        }
        private void MapData_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Map"),
                this.MapData_ArrayBox.Value,
                this.CurrentEntry + "Map changed");
        }
        private void Changes_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Changes"),
                this.Changes_ArrayBox.Value,
                this.CurrentEntry + "Map Changes changed");
        }
        private void TileAnim1_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "TileAnim1"),
                this.TileAnim1_ArrayBox.Value,
                this.CurrentEntry + "Map Anim 1 changed");
        }
        private void TileAnim2_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "TileAnim2"),
                this.TileAnim2_ArrayBox.Value,
                this.CurrentEntry + "Map Anim 2 changed");
        }

        private void Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Palette_ArrayBox.Value,
                this.Palette_PointerBox.Value,
                this.CurrentEntry + "Palette repoint");
        }
        private void Tileset1_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Tileset1_ArrayBox.Value,
                this.Tileset1_PointerBox.Value,
                this.CurrentEntry + "Tileset 1 repoint");
        }
        private void Tileset2_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Tileset2_ArrayBox.Value,
                this.Tileset2_PointerBox.Value,
                this.CurrentEntry + "Tileset 2 repoint");
        }
        private void TilesetTSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.TilesetTSA_ArrayBox.Value,
                this.TilesetTSA_PointerBox.Value,
                this.CurrentEntry + "Tileset TSA repoint");
        }
        private void MapData_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.MapData_ArrayBox.Value,
                this.MapData_PointerBox.Value,
                this.CurrentEntry + "Map repoint");
        }
        private void Changes_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Changes_ArrayBox.Value,
                this.Changes_PointerBox.Value,
                this.CurrentEntry + "Map Changes repoint");
        }
        private void TileAnim1_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.TileAnim1_ArrayBox.Value,
                this.TileAnim1_PointerBox.Value,
                this.CurrentEntry + "Tile Anim 1 repoint");
        }
        private void TileAnim2_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.TileAnim2_ArrayBox.Value,
                this.TileAnim2_PointerBox.Value,
                this.CurrentEntry + "Tile Anim 2 repoint");
        }

        private void Clear_Button_Click(Object sender, EventArgs e)
        {
            this.CurrentMap.Layout = new Int32[this.CurrentMap.WidthTiles, this.CurrentMap.HeightTiles];

            this.Core_WriteMap();
        }

        private void MapTileset_MagicButton_Click(Object sender, EventArgs e)
        {
            MapTilesetEditor editor = new MapTilesetEditor();
            Program.Core.Core_OpenEditor(editor);

            editor.Core_SetEntry(
                this.Palette_ArrayBox.Value,
                this.Tileset1_ArrayBox.Value,
                this.Tileset2_ArrayBox.Value,
                this.TilesetTSA_ArrayBox.Value,
                this.TileAnim1_ArrayBox.Value,
                this.TileAnim2_ArrayBox.Value);
        }
    }
}