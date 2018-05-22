using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class BattleScreenEditor : Editor
    {
        StructFile Current;
        BattlePlatform CurrentPlatform;
        /// <summary>
        /// Gets a string representing the current entry in the array
        /// </summary>
        string CurrentEntry
        {
            get
            {
                return "Battle Platform 0x" + Util.ByteToHex(EntryArrayBox.Value) + " [" + EntryArrayBox.Text + "] - ";
            }
        }

        BattleScreen CurrentScreen;
        string BattleScreenEntry = "Battle Screen Frame - ";



        public BattleScreenEditor()
        {
            try
            {
                InitializeComponent();

                EntryArrayBox.Load("Battle Platform List.txt");
                Current = new StructFile("Battle Platform Struct.txt");
                Current.Address = Core.GetPointer("Battle Platform Array");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Current.EntryIndex = EntryArrayBox.Value;

            Core_LoadScreen();
            Core_LoadScreenValues();
            Core_LoadPlatform();
            Core_LoadPlatformValues();
        }

        void Core_LoadScreen()
        {
                byte[] tileset = Core.ReadData(Core.GetPointer("Battle Screen Tileset"), 0);
                byte[] palettes = Core.ReadData(Core.GetPointer("Battle Screen Palettes"), 4 * Palette.LENGTH);
                TSA_Array tsa = Core.ReadTSA(Core.GetPointer("Battle Screen TSA"), 16, 32, false, false);

                CurrentScreen = new BattleScreen(
                    palettes, tileset, BattleScreen.GetReadibleTSA(tsa),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen L Name"), 0)),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen L Weapon"), 0)),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen R Name"), 0)),
                    new Tileset(Core.ReadData(Core.GetPointer("Battle Screen R Weapon"), 0)));

                Screen_GridBox.Load(CurrentScreen);
                Screen_PaletteBox.Load(new Palette(palettes, 4 * Palette.MAX));
            try
            {
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the battle platform image.", ex);

                Screen_GridBox.Reset();
                Screen_PaletteBox.Reset();
            }
        }
        void Core_LoadPlatform()
        {
            try
            {
                byte[] tileset = Core.ReadData((Pointer)Current["Tileset"], 0);
                byte[] palette = Core.ReadData((Pointer)Current["Palette"], Palette.LENGTH);

                CurrentPlatform = new BattlePlatform(tileset, palette);

                Platform_ImageBox.Load(CurrentPlatform);
                Platform_PaletteBox.Load(new Palette(palette));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the battle platform image.", ex);

                Platform_ImageBox.Reset();
                Platform_PaletteBox.Reset();
            }
        }
        void Core_LoadScreenValues()
        {
            Screen_TileIndex_NumBox.ValueChanged -= Screen_Tile_NumBox_ValueChanged;
            Screen_Palette_NumBox.ValueChanged -= Screen_Palette_NumBox_ValueChanged;
            Screen_FlipH_CheckBox.CheckedChanged -= Screen_FlipH_CheckBox_CheckedChanged;
            Screen_FlipV_CheckBox.CheckedChanged -= Screen_FlipV_CheckBox_CheckedChanged;

            try
            {
                if (Screen_GridBox.SelectionIsEmpty())
                {
                    Screen_TileIndex_NumBox.Enabled = false;
                    Screen_Palette_NumBox.Enabled = false;
                    Screen_FlipH_CheckBox.Enabled = false;
                    Screen_FlipV_CheckBox.Enabled = false;

                    Screen_TileIndex_NumBox.Value = 0;
                    Screen_Palette_NumBox.Value = 0;
                    Screen_FlipH_CheckBox.Checked = false;
                    Screen_FlipV_CheckBox.Checked = false;

                    Screen_TileIndex_NumBox.Text = Screen_TileIndex_NumBox.Value.ToString();
                    Screen_Palette_NumBox.Text = Screen_Palette_NumBox.Value.ToString();
                }
                else
                {
                    Screen_TileIndex_NumBox.Enabled = true;
                    Screen_Palette_NumBox.Enabled = true;
                    Screen_FlipH_CheckBox.Enabled = true;
                    Screen_FlipV_CheckBox.Enabled = true;

                    if (Screen_GridBox.SelectionIsSingle())
                    {
                        Point selection = Screen_GridBox.GetSelectionCoords();
                        TSA tsa = CurrentScreen.Tiling[selection.X, selection.Y];

                        Screen_TileIndex_NumBox.Value = tsa.TileIndex;
                        Screen_Palette_NumBox.Value = tsa.Palette;
                        Screen_FlipH_CheckBox.Checked = tsa.FlipH;
                        Screen_FlipV_CheckBox.Checked = tsa.FlipV;

                        Screen_TileIndex_NumBox.Text = Screen_TileIndex_NumBox.Value.ToString();
                        Screen_Palette_NumBox.Text = Screen_Palette_NumBox.Value.ToString();
                    }
                    else
                    {
                        Screen_TileIndex_NumBox.Text = "";
                        Screen_Palette_NumBox.Text = "";
                        Screen_FlipH_CheckBox.Checked = false;
                        Screen_FlipV_CheckBox.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the Battle Screen Frame TSA values.", ex);
            }

            Screen_TileIndex_NumBox.ValueChanged += Screen_Tile_NumBox_ValueChanged;
            Screen_Palette_NumBox.ValueChanged += Screen_Palette_NumBox_ValueChanged;
            Screen_FlipH_CheckBox.CheckedChanged += Screen_FlipH_CheckBox_CheckedChanged;
            Screen_FlipV_CheckBox.CheckedChanged += Screen_FlipV_CheckBox_CheckedChanged;
        }
        void Core_LoadPlatformValues()
        {
            Platform_Name_TextBox.TextChanged -= Platform_Name_TextBox_TextChanged;
            Platform_Tileset_PointerBox.ValueChanged -= Platform_Tileset_PointerBox_ValueChanged;
            Platform_Palette_PointerBox.ValueChanged -= Platform_Palette_PointerBox_ValueChanged;

            try
            {
                Platform_Name_TextBox.Text = (string)Current["Name"];
                Platform_Tileset_PointerBox.Value = (Pointer)Current["Tileset"];
                Platform_Palette_PointerBox.Value = (Pointer)Current["Palette"];
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the Battle Platform values.", ex);

                Platform_Name_TextBox.Text = "";
                Platform_Tileset_PointerBox.Value = new Pointer();
                Platform_Palette_PointerBox.Value = new Pointer();
            }

            Platform_Name_TextBox.TextChanged += Platform_Name_TextBox_TextChanged;
            Platform_Tileset_PointerBox.ValueChanged += Platform_Tileset_PointerBox_ValueChanged;
            Platform_Palette_PointerBox.ValueChanged += Platform_Palette_PointerBox_ValueChanged;
        }

        void Core_WriteBattleScreenTSA(TSA_Array tsa)
        {
            bool[,] selection = Screen_GridBox.Selection;

            Core.WriteData(this,
                Core.GetPointer("Battle Screen TSA"),
                BattleScreen.GetInsertableTSA(CurrentScreen.Tiling).ToBytes(false, false),
                BattleScreenEntry + "TSA changed");

            Screen_GridBox.Selection = selection;
        }
        void Core_Insert(BattleScreen insert)
        {
            Core.SuspendUpdate();
                Tileset tileset = new Tileset(insert.Graphics.Count);
                for (int i = 0; i < insert.Graphics.Count; i++)
                {
                    if (i >= BattleScreen.TILE_LIMIT && i < BattleScreen.TILE_LIMIT_END)
                        tileset.Add(Tile.Empty);
                    else tileset.Add(insert.Graphics[i]);
                }
                byte[] data_tileset = tileset.ToBytes(true);
                byte[] data_palette = Palette.Merge(insert.Palettes).ToBytes(false);
                byte[] data_tsa = BattleScreen.GetInsertableTSA(insert.Tiling).ToBytes(false, false);
                byte[] data_L_name   = insert.L_Name.ToBytes(true);
                byte[] data_L_weapon = insert.L_Weapon.ToBytes(true);
                byte[] data_R_name   = insert.R_Name.ToBytes(true);
                byte[] data_R_weapon = insert.R_Weapon.ToBytes(true);
            try
            {

                bool cancel = Prompt.ShowRepointDialog(this, "Repoint Battle Screen",
                    "The battle screen to insert might need some of its parts to be repointed.",
                    BattleScreenEntry, new Tuple<string, Pointer, int>[] {
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
                    CurrentEntry + "Tileset changed");

                Core.WriteData(this,
                    Core.GetPointer("Battle Screen Palettes"),
                    data_palette,
                    CurrentEntry + "Palettes changed");

                Core.WriteData(this,
                    Core.GetPointer("Battle Screen TSA"),
                    data_tsa,
                    CurrentEntry + "TSA changed");

                Core.WriteData(this,
                    Core.GetPointer("Battle Screen L Name"),
                    data_L_name,   CurrentEntry + "L Name changed");
                Core.WriteData(this,
                    Core.GetPointer("Battle Screen L Weapon"),
                    data_L_weapon, CurrentEntry + "L Weapon changed");
                Core.WriteData(this,
                    Core.GetPointer("Battle Screen R Name"),
                    data_R_name,   CurrentEntry + "R Name changed");
                Core.WriteData(this,
                    Core.GetPointer("Battle Screen R Weapon"),
                    data_R_weapon, CurrentEntry + "R Weapon changed");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the Battle Screen Frame.", ex);
                Core_Update();
            }
            Core.ResumeUpdate();
            Core.PerformUpdate();
        }
        void Core_Insert(BattlePlatform insert)
        {
            Core.SuspendUpdate();
            try
            {
                byte[] data_tileset = insert.Sheet.ToBytes(true);
                byte[] data_palette = insert.Colors.ToBytes(false);

                bool cancel = Prompt.ShowRepointDialog(this, "Repoint Battle Platform",
                    "The battle platform to insert might need some of its parts to be repointed.",
                    CurrentEntry, new Tuple<string, Pointer, int>[] {
                        Tuple.Create("Tileset", (Pointer)Current["Tileset"], data_tileset.Length),
                        Tuple.Create("Palette", (Pointer)Current["Palette"], data_palette.Length),
                    }, new Pointer[] {
                        Current.GetAddress(Current.EntryIndex, "Tileset"),
                        Current.GetAddress(Current.EntryIndex, "Palette"),
                    });
                if (cancel) return;

                Core.WriteData(this,
                    (Pointer)Current["Tileset"],
                    data_tileset,
                    CurrentEntry + "Tileset changed");

                Core.WriteData(this,
                    (Pointer)Current["Palette"],
                    data_palette,
                    CurrentEntry + "Palette changed");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the image.", ex);
                Core_Update();
            }
            Core.ResumeUpdate();
            Core.PerformUpdate();
        }

        void Core_Screen_InsertImage(string filepath)
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
                Program.ShowError("Could not load the image.", ex);
                return;
            }
            Core_Insert(screen);
        }
        void Core_Screen_InsertData(string filepath)
        {
            BattleScreen screen;
            try
            {
                string path = Path.GetDirectoryName(filepath) + '\\';
                string file = Path.GetFileNameWithoutExtension(filepath);

                if (!File.Exists(path + file + ".dmp"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".dmp");
                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");
                if (!File.Exists(path + file + ".tsa"))
                    throw new Exception("Could not find TSA file:\n" + path + file + ".tsa");

                byte[] tileset = File.ReadAllBytes(path + file + ".dmp");
                byte[] palette = File.ReadAllBytes(path + file + ".pal");
                TSA_Array tsa = new TSA_Array(
                    BattleScreen.WIDTH,
                    BattleScreen.HEIGHT,
                    File.ReadAllBytes(path + file + ".tsa"));

                screen = new BattleScreen(
                    palette, tileset, tsa,
                    new Tileset(File.ReadAllBytes(path + file + " L name.dmp")),
                    new Tileset(File.ReadAllBytes(path + file + " L weapon.dmp")),
                    new Tileset(File.ReadAllBytes(path + file + " R name.dmp")),
                    new Tileset(File.ReadAllBytes(path + file + " R weapon.dmp")));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the image data.", ex);
                return;
            }
            Core_Insert(screen);
        }
        void Core_Screen_SaveImage(string filepath)
        {
            try
            {
                using (var image = new System.Drawing.Bitmap(
                    CurrentScreen.Width,
                    CurrentScreen.Height))
                {
                    for (int y = 0; y < image.Height; y++)
                    for (int x = 0; x < image.Width; x++)
                    {
                        image.SetPixel(x, y, (System.Drawing.Color)CurrentScreen[x, y]);
                    }
                    image.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save image", ex);
            }
        }
        void Core_Screen_SaveData(string filepath)
        {
            try
            {
                string path = Path.GetDirectoryName(filepath) + "\\";
                string file = Path.GetFileNameWithoutExtension(filepath);

                byte[] data_tileset = CurrentScreen.Graphics.ToBytes(false);
                byte[] data_palette = Palette.Merge(CurrentScreen.Palettes).ToBytes(false);
                byte[] data_tsa = CurrentScreen.Tiling.ToBytes(false, false);

                File.WriteAllBytes(path + file + ".dmp", data_tileset);
                File.WriteAllBytes(path + file + ".pal", data_palette);
                File.WriteAllBytes(path + file + ".tsa", data_tsa);
                File.WriteAllBytes(path + file + " L name.dmp",   CurrentScreen.L_Name.ToBytes(false));
                File.WriteAllBytes(path + file + " L weapon.dmp", CurrentScreen.L_Weapon.ToBytes(false));
                File.WriteAllBytes(path + file + " R name.dmp",   CurrentScreen.R_Name.ToBytes(false));
                File.WriteAllBytes(path + file + " R weapon.dmp", CurrentScreen.R_Weapon.ToBytes(false));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save image data.", ex);
            }
        }

        void Core_Platform_InsertImage(string filepath)
        {
            BattlePlatform platform;
            try
            {
                platform = new BattlePlatform(new GBA.Image(filepath));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the image file.", ex);
                return;
            }
            Core_Insert(platform);
        }
        void Core_Platform_InsertData(string filepath)
        {
            BattlePlatform platform;
            try
            {
                string path = Path.GetDirectoryName(filepath) + '\\';
                string file = Path.GetFileNameWithoutExtension(filepath);

                if (!File.Exists(path + file + ".dmp"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".dmp");
                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");

                byte[] tileset = File.ReadAllBytes(path + file + ".dmp");
                byte[] palette = File.ReadAllBytes(path + file + ".pal");

                platform = new BattlePlatform(tileset, palette);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the image file.", ex);
                return;
            }
            Core_Insert(platform);
        }
        void Core_Platform_SaveImage(string filepath)
        {
            try
            {
                using (var image = new System.Drawing.Bitmap(
                    CurrentPlatform.Width,
                    CurrentPlatform.Height))
                {
                    for (int y = 0; y < image.Height; y++)
                    for (int x = 0; x < image.Width; x++)
                    {
                        image.SetPixel(x, y, (System.Drawing.Color)CurrentPlatform[x, y]);
                    }
                    image.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save image", ex);
            }
        }
        void Core_Platform_SaveData(string filepath)
        {
            try
            {
                string path = Path.GetDirectoryName(filepath) + "\\";
                string file = Path.GetFileNameWithoutExtension(filepath);

                byte[] data_tileset = CurrentPlatform.Sheet.ToBytes(false);
                byte[] data_palette = CurrentPlatform.Colors.ToBytes(false);

                File.WriteAllBytes(path + file + ".dmp", data_tileset);
                File.WriteAllBytes(path + file + ".pal", data_palette);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save image data.", ex);
            }
        }



        private void File_Insert_Screen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.dmp + .pal + .tsa)|*.dmp;*.pal;*.tsa|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Screen_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Screen_InsertData(openWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_Insert_Platform_Click(object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.dmp + .pal)|*.dmp;*.pal|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Platform_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Platform_InsertData(openWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_Save_Screen_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.dmp + .pal + .tsa)|*.dmp;*.pal;*.tsa|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Screen_SaveImage(saveWindow.FileName);
                    return;
                }
                if (saveWindow.FileName.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Screen_SaveData(saveWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }
        private void File_Save_Platform_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (.dmp + .pal)|*.dmp;*.pal|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Platform_SaveImage(saveWindow.FileName);
                    return;
                }
                if (saveWindow.FileName.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase))
                {
                    Core_Platform_SaveData(saveWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void Tool_OpenPalette_Platform_Click(object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                CurrentEntry,
                (Pointer)Current["Palette"], 1);
        }
        private void Tool_OpenPalette_Screen_Click(object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                "Battle Screen Frame Palettes - ",
                Core.GetPointer("Battle Screen Palettes"), 4);
        }

        private void View_ShowGrid_Click(object sender, EventArgs e)
        {
            Screen_GridBox.ShowGrid = View_ShowGrid.Checked;
        }



        private void Screen_GridBox_SelectionChanged(Object sender, EventArgs e)
        {
            Core_LoadScreenValues();
        }

        private void Screen_Tile_NumBox_ValueChanged(object sender, EventArgs e)
        {
            int width = Screen_GridBox.Selection.GetLength(0);
            int height = Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (Screen_GridBox.Selection[x, y])
                {
                    tsa = CurrentScreen.Tiling[x, y];
                    CurrentScreen.Tiling[x, y] = new TSA(
                        (int)Screen_TileIndex_NumBox.Value,
                        tsa.Palette,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            Core_WriteBattleScreenTSA(CurrentScreen.Tiling);
        }
        private void Screen_Palette_NumBox_ValueChanged(object sender, EventArgs e)
        {
            int width = Screen_GridBox.Selection.GetLength(0);
            int height = Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (Screen_GridBox.Selection[x, y])
                {
                    tsa = CurrentScreen.Tiling[x, y];
                    CurrentScreen.Tiling[x, y] = new TSA(
                        tsa.TileIndex,
                        (byte)Screen_Palette_NumBox.Value,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            Core_WriteBattleScreenTSA(CurrentScreen.Tiling);
        }
        private void Screen_FlipH_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int width = Screen_GridBox.Selection.GetLength(0);
            int height = Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (Screen_GridBox.Selection[x, y])
                {
                    tsa = CurrentScreen.Tiling[x, y];
                    CurrentScreen.Tiling[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        Screen_FlipH_CheckBox.Checked,
                        tsa.FlipV);
                }
            }
            Core_WriteBattleScreenTSA(CurrentScreen.Tiling);
        }
        private void Screen_FlipV_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int width = Screen_GridBox.Selection.GetLength(0);
            int height = Screen_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (Screen_GridBox.Selection[x, y])
                {
                    tsa = CurrentScreen.Tiling[x, y];
                    CurrentScreen.Tiling[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        tsa.FlipH,
                        Screen_FlipV_CheckBox.Checked);
                }
            }
            Core_WriteBattleScreenTSA(CurrentScreen.Tiling);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Core_LoadScreenValues();

            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Platform_Name_TextBox_TextChanged(object sender, EventArgs e)
        {
            Core.WriteData(this,
                Current.GetAddress(Current.EntryIndex, "Name"),
                ByteArray.Make_ASCII(Platform_Name_TextBox.Text),
                CurrentEntry + "Name changed");
        }
        private void Platform_Tileset_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Tileset"),
                Platform_Tileset_PointerBox.Value,
                CurrentEntry + "Tileset repointed");
        }
        private void Platform_Palette_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Palette"),
                Platform_Palette_PointerBox.Value,
                CurrentEntry + "Palette repointed");
        }
    }
}
