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
                return "Chapter 0x" + Util.ByteToHex(EntryArrayBox.Value) + " [" + EntryArrayBox.Text + "] - ";
            }
        }



        public MapEditor(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();

                EntryArrayBox.Load("Chapter List.txt");
                Current = new StructFile("Chapter Struct.txt");
                Current.Address = Core.GetPointer("Chapter Array");

                ArrayFile map_file = new ArrayFile("Map List.txt");
                MapData_ArrayBox.Load(map_file);
                Changes_ArrayBox.Load(map_file);
                TileAnim1_ArrayBox.Load(map_file);
                TileAnim2_ArrayBox.Load(map_file);
                Palette_ArrayBox.Load(map_file);
                TilesetTSA_ArrayBox.Load(map_file);
                Tileset1_ArrayBox.Load(map_file);
                Tileset2_ArrayBox.Load(map_file);

                Chapter_MagicButton.EditorToOpen = "Module:Chapter Editor";
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        override public void Core_OnOpen()
        {
            Core_Update();
        }
        override public void Core_Update()
        {
            Current.EntryIndex = EntryArrayBox.Value;
            
            Core_LoadValues();
            Core_LoadTileset();
            Core_LoadMap();
            Core_LoadMapValues();
        }

        void Core_LoadTileset()
        {
            try
            {
                Byte[] palette_data = Core.ReadData(Palette_PointerBox.Value, Map.PALETTES * GBA.Palette.LENGTH);

                CurrentTileset = new MapTileset(palette_data,
                    Core.ReadData(Tileset1_PointerBox.Value, 0),
                    Core.ReadData(Tileset2_PointerBox.Value, 0),
                    Core.ReadData(TilesetTSA_PointerBox.Value, 0));

                Tileset_GridBox.Load(CurrentTileset);
                Palette_PaletteBox.Load(new GBA.Palette(palette_data, Map.PALETTES * GBA.Palette.MAX));
            }
            catch (Exception ex)
            {
                CurrentTileset = null;
                UI.ShowError("Could not load the the tileset for this chapter.", ex);
            }
        }
        void Core_LoadMap()
        {
            try
            {
                CurrentMap = new Map(
                    CurrentTileset,
                    Core.ReadData(MapData_PointerBox.Value, 0),
                    Changes_PointerBox.Value);

                Map_GridBox.Load(CurrentMap);
            }
            catch (Exception ex)
            {
                CurrentMap = null;
                Map_GridBox.Load(CurrentMap);
                UI.ShowError("Could not read the map for this chapter.", ex);
            }
        }
        void Core_LoadValues()
        {
            MapData_ArrayBox.ValueChanged -= MapData_ArrayBox_ValueChanged;
            Changes_ArrayBox.ValueChanged -= Changes_ArrayBox_ValueChanged;
            TileAnim1_ArrayBox.ValueChanged -= TileAnim1_ArrayBox_ValueChanged;
            TileAnim2_ArrayBox.ValueChanged -= TileAnim2_ArrayBox_ValueChanged;
            Palette_ArrayBox.ValueChanged -= Palette_ArrayBox_ValueChanged;
            TilesetTSA_ArrayBox.ValueChanged -= TilesetTSA_ArrayBox_ValueChanged;
            Tileset1_ArrayBox.ValueChanged -= Tileset1_ArrayBox_ValueChanged;
            Tileset2_ArrayBox.ValueChanged -= Tileset2_ArrayBox_ValueChanged;

            MapData_PointerBox.ValueChanged -= MapData_PointerBox_ValueChanged;
            Changes_PointerBox.ValueChanged -= Changes_PointerBox_ValueChanged;
            TileAnim1_PointerBox.ValueChanged -= TileAnim1_PointerBox_ValueChanged;
            TileAnim2_PointerBox.ValueChanged -= TileAnim2_PointerBox_ValueChanged;
            Palette_PointerBox.ValueChanged -= Palette_PointerBox_ValueChanged;
            TilesetTSA_PointerBox.ValueChanged -= TilesetTSA_PointerBox_ValueChanged;
            Tileset1_PointerBox.ValueChanged -= Tileset1_PointerBox_ValueChanged;
            Tileset2_PointerBox.ValueChanged -= Tileset2_PointerBox_ValueChanged;
            
            try
            {
                MapData_ArrayBox.Value    = (Byte)Current["Map"];
                Changes_ArrayBox.Value    = (Byte)Current["MapChanges"];
                TileAnim1_ArrayBox.Value  = (Byte)Current["TileAnim1"];
                TileAnim2_ArrayBox.Value  = (Byte)Current["TileAnim2"];
                Palette_ArrayBox.Value    = (Byte)Current["Palette"];
                TilesetTSA_ArrayBox.Value = (Byte)Current["TSA"];
                Tileset1_ArrayBox.Value   = (Byte)Current["Tileset1"];
                Tileset2_ArrayBox.Value   = (Byte)Current["Tileset2"];

                GBA.Pointer address = Core.GetPointer("Map Data Array");
                MapData_PointerBox.Value   = Core.ReadPointer(address + 4 * MapData_ArrayBox.Value);
                Changes_PointerBox.Value   = Core.ReadPointer(address + 4 * Changes_ArrayBox.Value);
                TileAnim1_PointerBox.Value = Core.ReadPointer(address + 4 * TileAnim1_ArrayBox.Value);
                TileAnim2_PointerBox.Value = Core.ReadPointer(address + 4 * TileAnim2_ArrayBox.Value);
                Palette_PointerBox.Value   = Core.ReadPointer(address + 4 * Palette_ArrayBox.Value);
                TilesetTSA_PointerBox.Value   = Core.ReadPointer(address + 4 * TilesetTSA_ArrayBox.Value);
                Tileset1_PointerBox.Value  = Core.ReadPointer(address + 4 * Tileset1_ArrayBox.Value);
                Tileset2_PointerBox.Value  = Core.ReadPointer(address + 4 * Tileset2_ArrayBox.Value);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);
            }

            MapData_ArrayBox.ValueChanged += MapData_ArrayBox_ValueChanged;
            Changes_ArrayBox.ValueChanged += Changes_ArrayBox_ValueChanged;
            TileAnim1_ArrayBox.ValueChanged += TileAnim1_ArrayBox_ValueChanged;
            TileAnim2_ArrayBox.ValueChanged += TileAnim2_ArrayBox_ValueChanged;
            Palette_ArrayBox.ValueChanged += Palette_ArrayBox_ValueChanged;
            TilesetTSA_ArrayBox.ValueChanged += TilesetTSA_ArrayBox_ValueChanged;
            Tileset1_ArrayBox.ValueChanged += Tileset1_ArrayBox_ValueChanged;
            Tileset2_ArrayBox.ValueChanged += Tileset2_ArrayBox_ValueChanged;

            MapData_PointerBox.ValueChanged += MapData_PointerBox_ValueChanged;
            Changes_PointerBox.ValueChanged += Changes_PointerBox_ValueChanged;
            TileAnim1_PointerBox.ValueChanged += TileAnim1_PointerBox_ValueChanged;
            TileAnim2_PointerBox.ValueChanged += TileAnim2_PointerBox_ValueChanged;
            Palette_PointerBox.ValueChanged += Palette_PointerBox_ValueChanged;
            TilesetTSA_PointerBox.ValueChanged += TilesetTSA_PointerBox_ValueChanged;
            Tileset1_PointerBox.ValueChanged += Tileset1_PointerBox_ValueChanged;
            Tileset2_PointerBox.ValueChanged += Tileset2_PointerBox_ValueChanged;

            Chapter_MagicButton.EntryToSelect = EntryArrayBox.Value;
        }
        void Core_LoadMapValues()
        {
            if (Changes_PointerBox.Value == new GBA.Pointer())
            {
                Changes_CheckBox.Enabled = false;
            }
            else
            {
                Changes_CheckBox.Enabled = true;
            }
            Changes_CheckBox.CheckedChanged -= Changes_CheckBox_CheckedChanged;
            Changes_CheckBox.Checked = false;
            Changes_CheckBox.CheckedChanged += Changes_CheckBox_CheckedChanged;

            Changes_Total_NumBox.Value = (CurrentMap == null || CurrentMap.Changes == null) ?
                (Byte)(0x00) : (Byte)(CurrentMap.Changes.Count - 1);
            Changes_Total_NumBox.Enabled = false;
            Changes_Total_Label.Enabled = false;
            Changes_NumBox.Enabled = false;
            Changes_NumBox.ValueChanged -= Changes_NumBox_ValueChanged;
            Changes_NumBox.Value = 0;
            Changes_NumBox.Maximum = Changes_Total_NumBox.Value;
            Changes_NumBox.ValueChanged += Changes_NumBox_ValueChanged;

            View_AltPalette.Click -= View_AltPalette_Click;
            View_AltPalette.Checked = false;
            View_AltPalette.Click += View_AltPalette_Click;

            Map_Width_NumBox.ValueChanged -= Map_WidthNumBox_ValueChanged;
            Map_Height_NumBox.ValueChanged -= Map_HeightNumBox_ValueChanged;
            Map_Width_NumBox.Value = (CurrentMap == null) ? 15 : CurrentMap.WidthTiles;
            Map_Height_NumBox.Value = (CurrentMap == null) ? 10 : CurrentMap.HeightTiles;
            Map_Width_NumBox.ValueChanged += Map_WidthNumBox_ValueChanged;
            Map_Height_NumBox.ValueChanged += Map_HeightNumBox_ValueChanged;
        }

        /// <summary>
        /// Writes the CurrentMap onto the ROM
        /// </summary>
        void Core_WriteMap()
        {
            Boolean[,] selected = Tileset_GridBox.Selection;
            Boolean viewpalette = View_AltPalette.Checked;
            Boolean changes = Changes_CheckBox.Checked;
            Byte number = Changes_NumBox.Value;

            Core.WriteData(this,
                MapData_PointerBox.Value,
                CurrentMap.ToBytes(),
                CurrentEntry + "Map changed");

            Tileset_GridBox.Selection = selected;
            View_AltPalette.Checked = viewpalette;
            Changes_CheckBox.Checked = changes;
            Changes_NumBox.Value = number;
        }
        /// <summary>
        /// Writes the triggerable map changes associated with the CurrentMap onto the ROM
        /// </summary>
        void Core_WriteMapChanges()
        {
            Boolean[,] selected = Tileset_GridBox.Selection;
            Boolean viewpalette = View_AltPalette.Checked;
            Boolean changes = Changes_CheckBox.Checked;
            Byte number = Changes_NumBox.Value;

            Core.WriteData(this,
                Changes_PointerBox.Value,
                CurrentMap.Changes.ToBytes(Changes_PointerBox.Value),
                CurrentEntry + "Map Changes changed");

            Tileset_GridBox.Selection = selected;
            View_AltPalette.Checked = viewpalette;
            Changes_CheckBox.Checked = changes;
            Changes_NumBox.Value = number;
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

            CurrentMap.Layout = layout;

            Core_WriteMap();
        }
        void Core_InsertTMX(String filepath, Int32 width, Int32 height)
        {
            XmlDocument file = new XmlDocument();
            file.Load(filepath);

            width = Int32.Parse(file["map"].Attributes["width"].Value);
            height = Int32.Parse(file["map"].Attributes["height"].Value);
            CurrentMap.Layout = new Int32[width, height];

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
                        CurrentMap.Layout[x, y] = Int32.Parse(tile.Attributes["gid"].Value) - 1;
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
                Core_WriteMap();
                if (changes.Count > 0)
                {
                    CurrentMap.Changes = new MapChanges(changes);
                    Core_WriteMapChanges();
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
                Int32 tile = CurrentMap.Layout[x, y] << 3;
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
            Rectangle selection = Tileset_GridBox.GetSelectionRectangle();

            if (Changes_CheckBox.Checked)
            {
                for (Int32 y = 0; y < selection.Height; y++)
                for (Int32 x = 0; x < selection.Width; x++)
                {
                    if (Map_GridBox.Hovered.X + x < 0 || Map_GridBox.Hovered.X + x >= CurrentMap.WidthTiles ||
                        Map_GridBox.Hovered.Y + y < 0 || Map_GridBox.Hovered.Y + y >= CurrentMap.HeightTiles)
                            continue;

                    if (Map_GridBox.Hover[x, y])
                    {
                        CurrentMap.Changes.SetTile(
                            Changes_NumBox.Value,
                            Map_GridBox.Hovered.X + x,
                            Map_GridBox.Hovered.Y + y,
                            (selection.X + x) + (selection.Y + y) * 32);
                    }
                }

                UI.SuspendUpdate();
                Core_WriteMapChanges();
                UI.ResumeUpdate();
            }
            else if (Map_GridBox.Hover != null)
            {
                for (Int32 y = 0; y < selection.Height; y++)
                for (Int32 x = 0; x < selection.Width; x++)
                {
                    if (Map_GridBox.Hovered.X + x < 0 || Map_GridBox.Hovered.X + x >= CurrentMap.WidthTiles ||
                        Map_GridBox.Hovered.Y + y < 0 || Map_GridBox.Hovered.Y + y >= CurrentMap.HeightTiles)
                            continue;

                    if (Map_GridBox.Hover[x, y])
                    {
                        CurrentMap.Layout[Map_GridBox.Hovered.X + x,
                                          Map_GridBox.Hovered.Y + y] =
                            (selection.X + x) + (selection.Y + y) * 32;
                    }
                }
            }
        }
        void Core_EraseTool()
        {
            if (Changes_CheckBox.Checked)
            {
                CurrentMap.Changes.SetTile(
                    (Int32)Changes_NumBox.Value,
                    Map_GridBox.Hovered.X,
                    Map_GridBox.Hovered.Y,
                    0);

                UI.SuspendUpdate();
                Core_WriteMapChanges();
                UI.ResumeUpdate();
            }
            else
            {
                CurrentMap.Layout[Map_GridBox.Hovered.X, Map_GridBox.Hovered.Y] = 0;
            }
        }
        void Core_PickTool()
        {
            Int32 width = Tileset_GridBox.Selection.GetLength(0);
            Int32 height = Tileset_GridBox.Selection.GetLength(1);
            Int32 tile;
            if (Changes_CheckBox.Checked)
            {
                tile = CurrentMap.Changes.GetTile((Int32)Changes_NumBox.Value, Map_GridBox.Hovered.X, Map_GridBox.Hovered.Y);
                if (tile == 0) tile = CurrentMap.Layout[Map_GridBox.Hovered.X, Map_GridBox.Hovered.Y];
            }
            else
            {
                tile = CurrentMap.Layout[Map_GridBox.Hovered.X, Map_GridBox.Hovered.Y];
            }
            Tileset_GridBox.Selection = new Boolean[width, height];
            Tileset_GridBox.Selection[tile % width, tile / width] = true;
        }
        void Core_FillTool()
        {
            Rectangle selection = Tileset_GridBox.GetSelectionRectangle();
            Int32[] tiles = new Int32[Tileset_GridBox.GetSelectionAmount()];
            Int32 index = 0;
            for (Int32 y = 0; y < selection.Height; y++)
            for (Int32 x = 0; x < selection.Width; x++)
            {
                if (Tileset_GridBox.Selection[selection.X + x, selection.Y + y])
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
                Point start = Map_GridBox.Hovered;
                Boolean[,] map = new Boolean[CurrentMap.WidthTiles, CurrentMap.HeightTiles];
                Int32 tile = CurrentMap.Layout[start.X, start.Y];
                Core_FillHelper(ref map, ref tile, start.X, start.Y);

                Random random = new Random();
                for (Int32 y = 0; y < CurrentMap.HeightTiles; y++)
                for (Int32 x = 0; x < CurrentMap.WidthTiles; x++)
                {
                    if (map[x, y])
                    {
                        CurrentMap.Layout[x, y] = tiles[random.Next(tiles.Length)];
                    }
                }
            }
        }
        void Core_FillHelper(ref Boolean[,] map, ref Int32 tile, Int32 x, Int32 y)
        {
            if (x < 0 || x >= CurrentMap.WidthTiles
             || y < 0 || y >= CurrentMap.HeightTiles
             || map[x, y]
             || CurrentMap.Layout[x, y] != tile)
                return;
            else
            {
                map[x, y] = true;

                Core_FillHelper(ref map, ref tile, x - 1, y);
                Core_FillHelper(ref map, ref tile, x + 1, y);
                Core_FillHelper(ref map, ref tile, x, y - 1);
                Core_FillHelper(ref map, ref tile, x, y + 1);
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
                    Core_InsertTMX(openWindow.FileName, (Int32)Map_Width_NumBox.Value, (Int32)Map_Height_NumBox.Value);
                    return;
                }
                if (openWindow.FileName.EndsWith(".mar"))
                {
                    Core_InsertMAR(openWindow.FileName, (Int32)Map_Width_NumBox.Value, (Int32)Map_Height_NumBox.Value);
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
                    Core_SaveTMX(saveWindow.FileName, (Int32)Map_Width_NumBox.Value, (Int32)Map_Height_NumBox.Value);
                    return;
                }
                if (saveWindow.FileName.EndsWith(".mar", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveMAR(saveWindow.FileName, (Int32)Map_Width_NumBox.Value, (Int32)Map_Height_NumBox.Value);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }
        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                "Map Palette 0x" + Util.ByteToHex(Palette_ArrayBox.Value) + " [" + Palette_ArrayBox.Text + "] - ",
                Palette_PointerBox.Value, 10);
        }
        private void View_AltPalette_Click(Object sender, EventArgs e)
        {
            CurrentMap.ShowFog = View_AltPalette.Checked;

            Map_GridBox.Load(CurrentMap);
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Map_GridBox_MouseMove(Object sender, MouseEventArgs e)
        {
            if (Tileset_GridBox.SelectionIsEmpty())
            {
                Map_GridBox.Hover = new Boolean[1, 1] { { false } };
            }
            else if (Tool_Tile_Button.Checked)
            {
                Rectangle selection = Tileset_GridBox.GetSelectionRectangle();
                Map_GridBox.Hover = new Boolean[selection.Width, selection.Height];
                for (Int32 y = 0; y < selection.Height; y++)
                    for (Int32 x = 0; x < selection.Width; x++)
                    {
                        Map_GridBox.Hover[x, y] = Tileset_GridBox.Selection[selection.X + x, selection.Y + y];
                    }
            }
            else
            {
                Map_GridBox.Hover = new Boolean[1, 1] { { true } };
            }
        }
        private void Map_GridBox_MouseDown(Object sender, EventArgs e)
        {
            if (Tool_Tile_Button.Checked)
            {
                Core_TileTool();
                Map_MouseTimer.Enabled = true;
                Map_GridBox.Load(CurrentMap);
            }
            else if (Tool_Fill_Button.Checked)
            {
                Core_FillTool();
                Map_MouseTimer.Enabled = true;
                Map_GridBox.Load(CurrentMap);
            }
            else if (Tool_Erase_Button.Checked)
            {
                Core_EraseTool();
                Map_MouseTimer.Enabled = true;
                Map_GridBox.Load(CurrentMap);
            }
            else if (Tool_Pick_Button.Checked)
            {
                Core_PickTool();
                Map_MouseTimer.Enabled = false;
            }
            else
            {
                UI.ShowMessage("No tool is selected.");
            }
        }
        private void Map_GridBox_MouseUp(Object sender, EventArgs e)
        {
            if (Tool_Pick_Button.Checked) return;
            Map_MouseTimer.Enabled = false;
            Core_WriteMap();
        }
        private void Map_MouseTimer_Tick(Object sender, EventArgs e)
        {
            Map_GridBox_MouseDown(this, null);
        }

        private void Map_WidthNumBox_ValueChanged(Object sender, EventArgs e)
        {
            CurrentMap.WidthTiles = (Byte)Map_Width_NumBox.Value;

            Core_WriteMap();
        }
        private void Map_HeightNumBox_ValueChanged(Object sender, EventArgs e)
        {
            CurrentMap.HeightTiles = (Byte)Map_Height_NumBox.Value;

            Core_WriteMap();
        }

        private void Changes_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            CurrentMap.ShowChanges = new Boolean[Changes_Total_NumBox.Value];
            CurrentMap.ShowChanges[0] = Changes_CheckBox.Checked;

            Tool_Fill_Button.Enabled = !Changes_CheckBox.Checked;

            Changes_Total_NumBox.Enabled = Changes_CheckBox.Checked;
            Changes_Total_Label.Enabled = Changes_CheckBox.Checked;
            Changes_NumBox.Enabled = Changes_CheckBox.Checked;
            Changes_NumBox.Value = 0;

            Map_GridBox.Load(CurrentMap);
        }
        private void Changes_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            CurrentMap.ShowChanges = new Boolean[Changes_Total_NumBox.Value + 1];
            CurrentMap.ShowChanges[Changes_NumBox.Value] = true;
            Map_GridBox.Load(CurrentMap);
        }

        private void Palette_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "Palette"),
                Palette_ArrayBox.Value,
                CurrentEntry + "Palette changed");
        }
        private void Tileset1_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "Tileset1"),
                Tileset1_ArrayBox.Value,
                CurrentEntry + "Tileset 1 changed");
        }
        private void Tileset2_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "Tileset2"),
                Tileset2_ArrayBox.Value,
                CurrentEntry + "Tileset 2 changed");
        }
        private void TilesetTSA_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "TSA"),
                TilesetTSA_ArrayBox.Value,
                CurrentEntry + "Tileset TSA changed");
        }
        private void MapData_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "Map"),
                MapData_ArrayBox.Value,
                CurrentEntry + "Map changed");
        }
        private void Changes_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "Changes"),
                Changes_ArrayBox.Value,
                CurrentEntry + "Map Changes changed");
        }
        private void TileAnim1_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "TileAnim1"),
                TileAnim1_ArrayBox.Value,
                CurrentEntry + "Map Anim 1 changed");
        }
        private void TileAnim2_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "TileAnim2"),
                TileAnim2_ArrayBox.Value,
                CurrentEntry + "Map Anim 2 changed");
        }

        private void Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Palette_ArrayBox.Value,
                Palette_PointerBox.Value,
                CurrentEntry + "Palette repoint");
        }
        private void Tileset1_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Tileset1_ArrayBox.Value,
                Tileset1_PointerBox.Value,
                CurrentEntry + "Tileset 1 repoint");
        }
        private void Tileset2_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Tileset2_ArrayBox.Value,
                Tileset2_PointerBox.Value,
                CurrentEntry + "Tileset 2 repoint");
        }
        private void TilesetTSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * TilesetTSA_ArrayBox.Value,
                TilesetTSA_PointerBox.Value,
                CurrentEntry + "Tileset TSA repoint");
        }
        private void MapData_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * MapData_ArrayBox.Value,
                MapData_PointerBox.Value,
                CurrentEntry + "Map repoint");
        }
        private void Changes_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Changes_ArrayBox.Value,
                Changes_PointerBox.Value,
                CurrentEntry + "Map Changes repoint");
        }
        private void TileAnim1_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * TileAnim1_ArrayBox.Value,
                TileAnim1_PointerBox.Value,
                CurrentEntry + "Tile Anim 1 repoint");
        }
        private void TileAnim2_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * TileAnim2_ArrayBox.Value,
                TileAnim2_PointerBox.Value,
                CurrentEntry + "Tile Anim 2 repoint");
        }

        private void Clear_Button_Click(Object sender, EventArgs e)
        {
            CurrentMap.Layout = new Int32[CurrentMap.WidthTiles, CurrentMap.HeightTiles];

            Core_WriteMap();
        }

        private void MapTileset_MagicButton_Click(Object sender, EventArgs e)
        {
            MapTilesetEditor editor = new MapTilesetEditor(App);
            Program.Core.Core_OpenEditor(editor);

            editor.Core_SetEntry(
                Palette_ArrayBox.Value,
                Tileset1_ArrayBox.Value,
                Tileset2_ArrayBox.Value,
                TilesetTSA_ArrayBox.Value,
                TileAnim1_ArrayBox.Value,
                TileAnim2_ArrayBox.Value);
        }
    }
}