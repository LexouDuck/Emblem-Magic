using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EmblemMagic.FireEmblem;
using Magic;
using Magic.Editors;
using GBA;

namespace EmblemMagic.Editors
{
    public partial class BattleScreenEditor : Editor
    {
        StructFile Current;
        BattlePlatform CurrentPlatform;
        /// <summary>
        /// Gets a string representing the current entry in the array
        /// </summary>
        String CurrentEntry
        {
            get
            {
                return "Battle Platform 0x" + Util.ByteToHex(this.EntryArrayBox.Value) + " [" + this.EntryArrayBox.Text + "] - ";
            }
        }

        BattleScreen CurrentScreen;
        String BattleScreenEntry = "Battle Screen Frame - ";



        public BattleScreenEditor()
        {
            try
            {
                this.InitializeComponent();

                this.EntryArrayBox.Load("Battle Platform List.txt");
                this.Current = new StructFile("Battle Platform Struct.txt");
                this.Current.Address = Core.GetPointer("Battle Platform Array");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Current.EntryIndex = this.EntryArrayBox.Value;

            this.Core_LoadScreen();
            this.Core_LoadScreenValues();
            this.Core_LoadPlatform();
            this.Core_LoadPlatformValues();
        }

        void Core_LoadScreen()
        {
            try
            {
                Byte[] tileset = Core.ReadData(Core.GetPointer("Battle Screen Tileset"), 0);
                Byte[] palettes = Core.ReadData(Core.GetPointer("Battle Screen Palettes"), 4 * Palette.LENGTH);
                TSA_Array tsa = Core.ReadTSA(Core.GetPointer("Battle Screen TSA"), 16, 32, false, false);

                this.CurrentScreen = new BattleScreen(
                    palettes, tileset, BattleScreen.GetReadibleTSA(tsa),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen L Name"), 0)),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen L Weapon"), 0)),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen R Name"), 0)),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen R Weapon"), 0)));

                this.Screen_GridBox.Load(this.CurrentScreen);
                this.Screen_PaletteBox.Load(new Palette(palettes, 4 * Palette.MAX));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the battle platform image.", ex);

                this.Screen_GridBox.Reset();
                this.Screen_PaletteBox.Reset();
            }
        }
        void Core_LoadPlatform()
        {
            try
            {
                Byte[] tileset = Core.ReadData((Pointer)this.Current["Tileset"], 0);
                Byte[] palette = Core.ReadData((Pointer)this.Current["Palette"], Palette.LENGTH);

                this.CurrentPlatform = new BattlePlatform(tileset, palette);

                this.Platform_ImageBox.Load(this.CurrentPlatform);
                this.Platform_PaletteBox.Load(new Palette(palette));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the battle platform image.", ex);

                this.Platform_ImageBox.Reset();
                this.Platform_PaletteBox.Reset();
            }
        }
        void Core_LoadScreenValues()
        {
            this.Screen_TileIndex_NumBox.ValueChanged -= this.Screen_Tile_NumBox_ValueChanged;
            this.Screen_Palette_NumBox.ValueChanged -= this.Screen_Palette_NumBox_ValueChanged;
            this.Screen_FlipH_CheckBox.CheckedChanged -= this.Screen_FlipH_CheckBox_CheckedChanged;
            this.Screen_FlipV_CheckBox.CheckedChanged -= this.Screen_FlipV_CheckBox_CheckedChanged;

            try
            {
                if (this.Screen_GridBox.SelectionIsEmpty())
                {
                    this.Screen_TileIndex_NumBox.Enabled = false;
                    this.Screen_Palette_NumBox.Enabled = false;
                    this.Screen_FlipH_CheckBox.Enabled = false;
                    this.Screen_FlipV_CheckBox.Enabled = false;

                    this.Screen_TileIndex_NumBox.Value = 0;
                    this.Screen_Palette_NumBox.Value = 0;
                    this.Screen_FlipH_CheckBox.Checked = false;
                    this.Screen_FlipV_CheckBox.Checked = false;

                    this.Screen_TileIndex_NumBox.Text = this.Screen_TileIndex_NumBox.Value.ToString();
                    this.Screen_Palette_NumBox.Text = this.Screen_Palette_NumBox.Value.ToString();
                }
                else
                {
                    this.Screen_TileIndex_NumBox.Enabled = true;
                    this.Screen_Palette_NumBox.Enabled = true;
                    this.Screen_FlipH_CheckBox.Enabled = true;
                    this.Screen_FlipV_CheckBox.Enabled = true;

                    if (this.Screen_GridBox.SelectionIsSingle())
                    {
                        Point selection = this.Screen_GridBox.GetSelectionCoords();
                        TSA tsa = this.CurrentScreen.Tiling[selection.X, selection.Y];

                        this.Screen_TileIndex_NumBox.Value = tsa.TileIndex;
                        this.Screen_Palette_NumBox.Value = tsa.Palette;
                        this.Screen_FlipH_CheckBox.Checked = tsa.FlipH;
                        this.Screen_FlipV_CheckBox.Checked = tsa.FlipV;

                        this.Screen_TileIndex_NumBox.Text = this.Screen_TileIndex_NumBox.Value.ToString();
                        this.Screen_Palette_NumBox.Text = this.Screen_Palette_NumBox.Value.ToString();
                    }
                    else
                    {
                        this.Screen_TileIndex_NumBox.Text = "";
                        this.Screen_Palette_NumBox.Text = "";
                        this.Screen_FlipH_CheckBox.Checked = false;
                        this.Screen_FlipV_CheckBox.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the Battle Screen Frame TSA values.", ex);
            }

            this.Screen_TileIndex_NumBox.ValueChanged += this.Screen_Tile_NumBox_ValueChanged;
            this.Screen_Palette_NumBox.ValueChanged += this.Screen_Palette_NumBox_ValueChanged;
            this.Screen_FlipH_CheckBox.CheckedChanged += this.Screen_FlipH_CheckBox_CheckedChanged;
            this.Screen_FlipV_CheckBox.CheckedChanged += this.Screen_FlipV_CheckBox_CheckedChanged;
        }
        void Core_LoadPlatformValues()
        {
            this.Platform_Name_TextBox.TextChanged -= this.Platform_Name_TextBox_TextChanged;
            this.Platform_Tileset_PointerBox.ValueChanged -= this.Platform_Tileset_PointerBox_ValueChanged;
            this.Platform_Palette_PointerBox.ValueChanged -= this.Platform_Palette_PointerBox_ValueChanged;

            try
            {
                this.Platform_Name_TextBox.Text = (String)this.Current["Name"];
                this.Platform_Tileset_PointerBox.Value = (Pointer)this.Current["Tileset"];
                this.Platform_Palette_PointerBox.Value = (Pointer)this.Current["Palette"];
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the Battle Platform values.", ex);

                this.Platform_Name_TextBox.Text = "";
                this.Platform_Tileset_PointerBox.Value = new Pointer();
                this.Platform_Palette_PointerBox.Value = new Pointer();
            }

            this.Platform_Name_TextBox.TextChanged += this.Platform_Name_TextBox_TextChanged;
            this.Platform_Tileset_PointerBox.ValueChanged += this.Platform_Tileset_PointerBox_ValueChanged;
            this.Platform_Palette_PointerBox.ValueChanged += this.Platform_Palette_PointerBox_ValueChanged;
        }

        void Core_WriteBattleScreenTSA(TSA_Array tsa)
        {
            Boolean[,] selection = this.Screen_GridBox.Selection;

            Core.WriteData(this,
                Core.GetPointer("Battle Screen TSA"),
                BattleScreen.GetInsertableTSA(this.CurrentScreen.Tiling).ToBytes(false, false),
                this.BattleScreenEntry + "TSA changed");

            this.Screen_GridBox.Selection = selection;
        }
        void Core_Insert(BattleScreen insert)
        {
            UI.SuspendUpdate();
                Tileset tileset = new Tileset(insert.Graphics.Count);
                for (Int32 i = 0; i < insert.Graphics.Count; i++)
                {
                    if (i >= BattleScreen.TILE_LIMIT && i < BattleScreen.TILE_LIMIT_END)
                        tileset.Add(Tile.Empty);
                    else tileset.Add(insert.Graphics[i]);
                }
            Byte[] data_tileset = tileset.ToBytes(true);
            Byte[] data_palette = Palette.Merge(insert.Palettes).ToBytes(false);
            Byte[] data_tsa = BattleScreen.GetInsertableTSA(insert.Tiling).ToBytes(false, false);
            Byte[] data_L_name   = insert.L_Name.ToBytes(true);
            Byte[] data_L_weapon = insert.L_Weapon.ToBytes(true);
            Byte[] data_R_name   = insert.R_Name.ToBytes(true);
            Byte[] data_R_weapon = insert.R_Weapon.ToBytes(true);
            try
            {

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Battle Screen",
                    "The battle screen to insert might need some of its parts to be repointed.",
                    this.BattleScreenEntry, new Tuple<String, Pointer, Int32>[] {
                        Tuple.Create("Tileset",  Core.GetPointer("Battle Screen Tileset"),  data_tileset.Length),
                        Tuple.Create("Palette",  Core.GetPointer("Battle Screen Palettes"), data_palette.Length),
                        // Tuple.Create("TSA", Core.GetPointer("Battle Screen TSA"), data_tsa.Length),
                        // repointing this TSA fucks everything up, shouldn't be too accessible
                        Tuple.Create("L Name",   Core.GetPointer("Battle Screen L Name"),   data_L_name.Length),
                        Tuple.Create("L Weapon", Core.GetPointer("Battle Screen L Weapon"), data_L_weapon.Length),
                        Tuple.Create("R Name",   Core.GetPointer("Battle Screen R Name"),   data_R_name.Length),
                        Tuple.Create("R Weapon", Core.GetPointer("Battle Screen R Weapon"), data_R_weapon.Length)});
                if (cancel) return;

                Core.WriteData(this,
                    Core.GetPointer("Battle Screen Tileset"),
                    data_tileset,
                    this.CurrentEntry + "Tileset changed");

                Core.WriteData(this,
                    Core.GetPointer("Battle Screen Palettes"),
                    data_palette,
                    this.CurrentEntry + "Palettes changed");

                Core.WriteData(this,
                    Core.GetPointer("Battle Screen TSA"),
                    data_tsa,
                    this.CurrentEntry + "TSA changed");

                Core.WriteData(this,
                    Core.GetPointer("Battle Screen L Name"),
                    data_L_name, this.CurrentEntry + "L Name changed");
                Core.WriteData(this,
                    Core.GetPointer("Battle Screen L Weapon"),
                    data_L_weapon, this.CurrentEntry + "L Weapon changed");
                Core.WriteData(this,
                    Core.GetPointer("Battle Screen R Name"),
                    data_R_name, this.CurrentEntry + "R Name changed");
                Core.WriteData(this,
                    Core.GetPointer("Battle Screen R Weapon"),
                    data_R_weapon, this.CurrentEntry + "R Weapon changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the Battle Screen Frame.", ex);
                this.Core_Update();
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_Insert(BattlePlatform insert)
        {
            UI.SuspendUpdate();
            try
            {
                Byte[] data_tileset = insert.Sheet.ToBytes(true);
                Byte[] data_palette = insert.Colors.ToBytes(false);

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Battle Platform",
                    "The battle platform to insert might need some of its parts to be repointed.",
                    this.CurrentEntry, new Tuple<String, Pointer, Int32>[] {
                        Tuple.Create("Tileset", (Pointer)this.Current["Tileset"], data_tileset.Length),
                        Tuple.Create("Palette", (Pointer)this.Current["Palette"], data_palette.Length),
                    }, new Pointer[] {
                        this.Current.GetAddress(this.Current.EntryIndex, "Tileset"),
                        this.Current.GetAddress(this.Current.EntryIndex, "Palette"),
                    });
                if (cancel) return;

                Core.WriteData(this,
                    (Pointer)this.Current["Tileset"],
                    data_tileset,
                    this.CurrentEntry + "Tileset changed");

                Core.WriteData(this,
                    (Pointer)this.Current["Palette"],
                    data_palette,
                    this.CurrentEntry + "Palette changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the image.", ex);
                this.Core_Update();
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }

        void Core_Screen_InsertImage(String filepath)
        {
            BattleScreen screen;
            try
            {
                GBA.Bitmap image = new GBA.Bitmap(filepath);
                GBA.Palette palettes = new Palette(
                    filepath.Substring(0, filepath.Length - 4) + " palette.png", 4 * Palette.MAX);

                screen = new BattleScreen(image, Palette.Split(palettes, 4));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the image.", ex);
                return;
            }
            this.Core_Insert(screen);
        }
        void Core_Screen_InsertData(String filepath)
        {
            BattleScreen screen;
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                if (!File.Exists(path + file + ".chr"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".chr");
                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");
                if (!File.Exists(path + file + ".tsa"))
                    throw new Exception("Could not find TSA file:\n" + path + file + ".tsa");

                Byte[] tileset = File.ReadAllBytes(path + file + ".chr");
                Byte[] palette = File.ReadAllBytes(path + file + ".pal");
                TSA_Array tsa = new TSA_Array(
                    BattleScreen.WIDTH,
                    BattleScreen.HEIGHT,
                    File.ReadAllBytes(path + file + ".tsa"));

                screen = new BattleScreen(
                    palette, tileset, tsa,
                    new Tileset(File.ReadAllBytes(path + file + " L name.chr")),
                    new Tileset(File.ReadAllBytes(path + file + " L weapon.chr")),
                    new Tileset(File.ReadAllBytes(path + file + " R name.chr")),
                    new Tileset(File.ReadAllBytes(path + file + " R weapon.chr")));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the image data.", ex);
                return;
            }
            this.Core_Insert(screen);
        }
        void Core_Screen_SaveImage(String filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    this.CurrentScreen.Width,
                    this.CurrentScreen.Height,
                    this.CurrentScreen.Palettes,
                    delegate (Int32 x, Int32 y)
                    {
                        return (Byte)this.CurrentScreen[x, y];
                    });
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save image", ex);
            }
        }
        void Core_Screen_SaveData(String filepath)
        {
            try
            {
                String path = Path.GetDirectoryName(filepath) + "\\";
                String file = Path.GetFileNameWithoutExtension(filepath);

                Byte[] data_tileset = this.CurrentScreen.Graphics.ToBytes(false);
                Byte[] data_palette = Palette.Merge(this.CurrentScreen.Palettes).ToBytes(false);
                Byte[] data_tsa = this.CurrentScreen.Tiling.ToBytes(false, false);

                File.WriteAllBytes(path + file + ".chr", data_tileset);
                File.WriteAllBytes(path + file + ".pal", data_palette);
                File.WriteAllBytes(path + file + ".tsa", data_tsa);
                File.WriteAllBytes(path + file + " L name.chr", this.CurrentScreen.L_Name.ToBytes(false));
                File.WriteAllBytes(path + file + " L weapon.chr", this.CurrentScreen.L_Weapon.ToBytes(false));
                File.WriteAllBytes(path + file + " R name.chr", this.CurrentScreen.R_Name.ToBytes(false));
                File.WriteAllBytes(path + file + " R weapon.chr", this.CurrentScreen.R_Weapon.ToBytes(false));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save image data.", ex);
            }
        }

        void Core_Platform_InsertImage(String filepath)
        {
            BattlePlatform platform;
            try
            {
                platform = new BattlePlatform(new GBA.Image(filepath));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the image file.", ex);
                return;
            }
            this.Core_Insert(platform);
        }
        void Core_Platform_InsertData(String filepath)
        {
            BattlePlatform platform;
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                if (!File.Exists(path + file + ".chr"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".chr");
                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");

                Byte[] tileset = File.ReadAllBytes(path + file + ".chr");
                Byte[] palette = File.ReadAllBytes(path + file + ".pal");

                platform = new BattlePlatform(tileset, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the image file.", ex);
                return;
            }
            this.Core_Insert(platform);
        }
        void Core_Platform_SaveImage(String filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    this.CurrentPlatform.Width,
                    this.CurrentPlatform.Height,
                    new Palette[1] { this.CurrentPlatform.Colors },
                    delegate (Int32 x, Int32 y)
                    {
                        return (Byte)this.CurrentPlatform[x, y];
                    });
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save image", ex);
            }
        }
        void Core_Platform_SaveData(String filepath)
        {
            try
            {
                String path = Path.GetDirectoryName(filepath) + "\\";
                String file = Path.GetFileNameWithoutExtension(filepath);

                Byte[] data_tileset = this.CurrentPlatform.Sheet.ToBytes(false);
                Byte[] data_palette = this.CurrentPlatform.Colors.ToBytes(false);

                File.WriteAllBytes(path + file + ".chr", data_tileset);
                File.WriteAllBytes(path + file + ".pal", data_palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save image data.", ex);
            }
        }



        private void File_Insert_Screen_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.chr + .pal + .tsa)|*.chr;*.pal;*.tsa|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_Screen_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_Screen_InsertData(openWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_Insert_Platform_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.chr + .pal)|*.chr;*.pal|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_Platform_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_Platform_InsertData(openWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_Save_Screen_Click(Object sender, EventArgs e)
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
                    this.Core_Screen_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_Screen_SaveData(saveWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }
        private void File_Save_Platform_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.chr + .pal)|*.chr;*.pal|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_Platform_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_Platform_SaveData(saveWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void Tool_OpenPalette_Platform_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry,
                (Pointer)this.Current["Palette"], 1);
        }
        private void Tool_OpenPalette_Screen_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                "Battle Screen Frame Palettes - ",
                Core.GetPointer("Battle Screen Palettes"), 4);
        }

        private void View_ShowGrid_Click(Object sender, EventArgs e)
        {
            this.Screen_GridBox.ShowGrid = this.View_ShowGrid.Checked;
        }



        private void Screen_GridBox_SelectionChanged(Object sender, EventArgs e)
        {
            this.Core_LoadScreenValues();
        }

        private void Screen_Tile_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 width = this.Screen_GridBox.Selection.GetLength(0);
            Int32 height = this.Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.Screen_GridBox.Selection[x, y])
                {
                    tsa = this.CurrentScreen.Tiling[x, y];
                        this.CurrentScreen.Tiling[x, y] = new TSA(
                        (Int32)this.Screen_TileIndex_NumBox.Value,
                        tsa.Palette,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            this.Core_WriteBattleScreenTSA(this.CurrentScreen.Tiling);
        }
        private void Screen_Palette_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 width = this.Screen_GridBox.Selection.GetLength(0);
            Int32 height = this.Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.Screen_GridBox.Selection[x, y])
                {
                    tsa = this.CurrentScreen.Tiling[x, y];
                        this.CurrentScreen.Tiling[x, y] = new TSA(
                        tsa.TileIndex,
                        (Byte)this.Screen_Palette_NumBox.Value,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            this.Core_WriteBattleScreenTSA(this.CurrentScreen.Tiling);
        }
        private void Screen_FlipH_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Int32 width = this.Screen_GridBox.Selection.GetLength(0);
            Int32 height = this.Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.Screen_GridBox.Selection[x, y])
                {
                    tsa = this.CurrentScreen.Tiling[x, y];
                        this.CurrentScreen.Tiling[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        this.Screen_FlipH_CheckBox.Checked,
                        tsa.FlipV);
                }
            }
            this.Core_WriteBattleScreenTSA(this.CurrentScreen.Tiling);
        }
        private void Screen_FlipV_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Int32 width = this.Screen_GridBox.Selection.GetLength(0);
            Int32 height = this.Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.Screen_GridBox.Selection[x, y])
                {
                    tsa = this.CurrentScreen.Tiling[x, y];
                        this.CurrentScreen.Tiling[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        tsa.FlipH,
                        this.Screen_FlipV_CheckBox.Checked);
                }
            }
            this.Core_WriteBattleScreenTSA(this.CurrentScreen.Tiling);
        }

        private void Screen_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            editor.Core_SetEntry(15, 34,
                Core.GetPointer("Battle Screen Palettes"), false,
                Core.GetPointer("Battle Screen Tileset"), true,
                Core.GetPointer("Battle Screen TSA"), false, false);

            Program.Core.Core_OpenEditor(editor);
        }

        protected override Boolean ProcessCmdKey(ref Message msg, Keys keyData)
        {
            this.Core_LoadScreenValues();

            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void Platform_Name_TextBox_TextChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Name"),
                ByteArray.Make_ASCII(this.Platform_Name_TextBox.Text),
                this.CurrentEntry + "Name changed");
        }
        private void Platform_Tileset_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Tileset"),
                this.Platform_Tileset_PointerBox.Value,
                this.CurrentEntry + "Tileset repointed");
        }
        private void Platform_Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Palette"),
                this.Platform_Palette_PointerBox.Value,
                this.CurrentEntry + "Palette repointed");
        }

        private void Platform_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            editor.Core_SetEntry(32, 4,
                (Pointer)this.Current["Palette"], false,
                (Pointer)this.Current["Tileset"], true);

            Program.Core.Core_OpenEditor(editor);
        }
    }
}
