using EmblemMagic.FireEmblem;
using EmblemMagic.Properties;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class MapSpriteEditor : Editor
    {
        public StructFile CurrentIdle;
        public StructFile CurrentWalk;
        MapSprite CurrentMapSprite;

        GBA.Sprite TestSprite;
        GBA.Palette CurrentPalette;
        Pointer CurrentPaletteAddress
        {
            get
            {
                return Core.CurrentROM.Address_MapSpritePalettes() + PaletteArrayBox.Value * GBA.Palette.LENGTH;
            }
        }

        string CurrentIdleEntry
        {
            get
            {
                return "Map Sprite (Idle) 0x" + Util.ByteToHex(IdleEntryArrayBox.Value) + " [" + IdleEntryArrayBox.Text + "] - ";
            }
        }
        string CurrentWalkEntry
        {
            get
            {
                return "Map Sprite (Walk) 0x" + Util.ByteToHex(WalkEntryArrayBox.Value) + " [" + WalkEntryArrayBox.Text + "] - ";
            }
        }



        public MapSpriteEditor()
        {
            InitializeComponent();

            try
            {
                IdleEntryArrayBox.Load("Map Sprite List.txt");
                WalkEntryArrayBox.Load("Class List.txt");
                CurrentIdle = new StructFile("Map Sprite Idle Struct.txt");
                CurrentWalk = new StructFile("Map Sprite Walk Struct.txt");
                CurrentIdle.Address = Core.GetPointer("Map Sprite Idle Array");
                CurrentWalk.Address = Core.GetPointer("Map Sprite Walk Array");

                PaletteArrayBox.Load("Map Sprite Palettes.txt");

                SizeComboBox.DataSource = new KeyValuePair<string, byte>[3]
                {
                    new KeyValuePair<string, byte>("16x16", 0x00),
                    new KeyValuePair<string, byte>("16x32", 0x01),
                    new KeyValuePair<string, byte>("32x32", 0x02)
                };
                SizeComboBox.ValueMember = "Value";
                SizeComboBox.DisplayMember = "Key";

                CurrentPalette = Core.ReadPalette(Core.CurrentROM.Address_MapSpritePalettes(), GBA.Palette.LENGTH);

                Test_PaletteBox.Load(CurrentPalette);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        public override void Core_SetEntry(uint entry)
        {
            IdleEntryArrayBox.Value = (byte)entry;
            WalkEntryArrayBox.Value = (byte)WalkEntryArrayBox.File.FindEntry(IdleEntryArrayBox.Text);
        }
        public override void Core_OnOpen()
        {
            WalkEntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;
            WalkEntryArrayBox.Value = 1;
            WalkEntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;

            Core_Update();
        }
        public override void Core_Update()
        {
            CurrentIdle.EntryIndex = IdleEntryArrayBox.Value;
            CurrentWalk.EntryIndex = WalkEntryArrayBox.Value - 1;

            Core_LoadPalette();
            Core_LoadImage();
            Core_LoadValues();
            Core_LoadTestView();
        }

        void Core_LoadPalette()
        {
            try
            {
                CurrentPalette = Core.ReadPalette(CurrentPaletteAddress, GBA.Palette.LENGTH);

                Test_PaletteBox.Load(CurrentPalette);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the palette.", ex);
                Test_PaletteBox.Reset();
            }
        }
        void Core_LoadImage()
        {
            try
            {
                CurrentMapSprite = new MapSprite(CurrentPalette,
                    Core.ReadData((Pointer)CurrentIdle["Sprite"], 0),
                    Core.ReadData((Pointer)CurrentWalk["Sprite"], 0),
                    (byte)CurrentIdle["Size"]);

                Edit_ImageBox.Load(CurrentMapSprite);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the map sprite image.", ex);
                Edit_ImageBox.Reset();
                Test_ImageBox.Reset();
            }
        }
        void Core_LoadTestView()
        {
            try
            {
                GBA.Palette palette = new GBA.Palette(CurrentPalette);
                palette[0] = palette[0].SetAlpha(true);
                GBA.Tileset tileset = null;
                byte size = (byte)CurrentIdle["Size"];
                int frame = Test_TrackBar.Value * 16;
                GBA.TileMap tilemap = new TileMap(4, 4);
                if (Test_Idle.Checked)
                {
                    tilemap.Map(MapSprite.Map_Test(frame, size));
                    tileset = CurrentMapSprite.Sprites[MapSprite.IDLE].Sheet;
                }
                frame++;
                if (Test_WalkSide.Checked)
                {
                    tilemap.Map(MapSprite.Map_Test(frame));
                    tileset = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet;
                }
                frame += 16 * 4;
                if (Test_WalkDown.Checked)
                {
                    tilemap.Map(MapSprite.Map_Test(frame));
                    tileset = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet;
                }
                frame += 16 * 4;
                if (Test_WalkUp.Checked)
                {
                    tilemap.Map(MapSprite.Map_Test(frame));
                    tileset = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet;
                }
                frame += 16 * 4;
                if (Test_Selected.Checked)
                {
                    tilemap.Map(MapSprite.Map_Test(frame));
                    tileset = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet;
                }
                TestSprite = new GBA.Sprite(palette, tileset, tilemap);
                Test_ImageBox.Load(TestSprite);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the map sprite test image.", ex);
            }
        }
        void Core_LoadValues()
        {
            SizeComboBox.SelectedValueChanged -= Size_ComboBox_ValueChanged;
            UnknownNumberBox.ValueChanged -= Unknown_NumBox_ValueChanged;
            IdlePointerBox.ValueChanged -= Idle_PointerBox_ValueChanged;
            WalkPointerBox.ValueChanged -= Walk_PointerBox_ValueChanged;
            AnimPointerBox.ValueChanged -= Anim_PointerBox_ValueChanged;

            try
            {
                SizeComboBox.SelectedValue = (byte)CurrentIdle["Size"];
                UnknownNumberBox.Value = (byte)CurrentIdle["Unknown"];
                IdlePointerBox.Value = (Pointer)CurrentIdle["Sprite"];
                WalkPointerBox.Value = (Pointer)CurrentWalk["Sprite"];
                AnimPointerBox.Value = (Pointer)CurrentWalk["AnimData"];
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the values.", ex);

                SizeComboBox.SelectedValue = 0;
                IdlePointerBox.Value = new Pointer();
                WalkPointerBox.Value = new Pointer();
                AnimPointerBox.Value = new Pointer();
                UnknownNumberBox.Value = 0;
            }

            SizeComboBox.SelectedValueChanged += Size_ComboBox_ValueChanged;
            UnknownNumberBox.ValueChanged += Unknown_NumBox_ValueChanged;
            IdlePointerBox.ValueChanged += Idle_PointerBox_ValueChanged;
            WalkPointerBox.ValueChanged += Walk_PointerBox_ValueChanged;
            AnimPointerBox.ValueChanged += Anim_PointerBox_ValueChanged;
        }
        
        void Core_Insert(MapSprite insert)
        {
            Core.SuspendUpdate();
            try
            {
                byte[] data_idle = insert.Sprites[MapSprite.IDLE].Sheet.ToBytes(true);
                byte[] data_walk = insert.Sprites[MapSprite.WALK].Sheet.ToBytes(true);

                bool cancel = Prompt.ShowRepointDialog(this, "Repoint Map Sprite",
                    "The Map Sprite to insert might need to be repointed.",
                        CurrentIdleEntry,
                    new Tuple<string, Pointer, int>[] {
                        Tuple.Create("Idle Sprite", (Pointer)CurrentIdle["Sprite"], data_idle.Length),
                        Tuple.Create("Walk Sprite", (Pointer)CurrentWalk["Sprite"], data_walk.Length)},
                    new Pointer[] {
                        CurrentIdle.GetAddress(CurrentIdle.EntryIndex, "Sprite"),
                        CurrentWalk.GetAddress(CurrentWalk.EntryIndex, "Sprite")});
                if (cancel) return;

                Core.WriteData(this,
                    (Pointer)CurrentIdle["Sprite"],
                    data_idle,
                    CurrentIdleEntry + "Idle Sprite changed");

                Core.WriteData(this,
                    (Pointer)CurrentWalk["Sprite"],
                    data_walk,
                    CurrentWalkEntry + "Walk Sprite changed");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the map sprite.", ex);
            }
            Core.ResumeUpdate();
            Core.PerformUpdate();
        }

        void Core_InsertImage(string filepath)
        {
            MapSprite mapsprite;
            try
            {
                GBA.Image image = new GBA.Image(filepath);

                if (image.Width != MapSprite.WIDTH * 8 || image.Height != MapSprite.HEIGHT * 8)
                    throw new Exception("Image given has invalid dimensions. It must be 160x128");

                mapsprite = new MapSprite((byte)CurrentIdle["Size"], image);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the image file.", ex);
                return;
            }
            Core_Insert(mapsprite);
        }
        void Core_InsertData(string filepath)
        {
            string idle_path = null;
            string walk_path = null;
            if (filepath.EndsWith("idle.dmp", StringComparison.OrdinalIgnoreCase))
            {
                idle_path = filepath;
                walk_path = filepath.Substring(0, filepath.Length - 8) + "walk.dmp";
            }
            if (filepath.EndsWith("walk.dmp", StringComparison.OrdinalIgnoreCase))
            {
                idle_path = filepath.Substring(0, filepath.Length - 8) + "idle.dmp";
                walk_path = filepath;
            }
            if (idle_path == null || walk_path == null)
            {
                Program.ShowError("Selected file has invalid name.\r\n" +
                "Image data files must end with either 'idle.dmp' or 'walk.dmp').");
                return;
            }

            Core_Insert(new MapSprite(
                CurrentPalette,
                File.ReadAllBytes(idle_path),
                File.ReadAllBytes(walk_path),
                (byte)CurrentIdle["Size"]));
        }
        void Core_SaveImage(string filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    CurrentMapSprite.Width,
                    CurrentMapSprite.Height,
                    new Palette[1] { CurrentPalette },
                    delegate (int x, int y)
                    {
                        return (byte)CurrentMapSprite[x, y];
                    });
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save image.", ex);
            }
        }
        void Core_SaveData(string filepath)
        {
            try
            {
                string path = Path.GetDirectoryName(filepath) + "\\";
                string file = Path.GetFileNameWithoutExtension(filepath);

                byte[] data_idle = CurrentMapSprite.Sprites[MapSprite.IDLE].Sheet.ToBytes(false);
                byte[] data_walk = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet.ToBytes(false);

                File.WriteAllBytes(path + file + " idle.dmp", data_idle);
                File.WriteAllBytes(path + file + " walk.dmp", data_walk);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save image data.", ex);
            }
        }



        private void File_Insert_Click(object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (idle.dmp + walk.dmp)|*.dmp|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    Core_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase))
                {
                    Core_InsertData(openWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data (idle.dmp + walk.dmp)|*.dmp|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveData(saveWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void EntryArrayBox_ValueChanged(object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Size_ComboBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentIdle.GetAddress(CurrentIdle.EntryIndex, "Size"),
                (byte)SizeComboBox.SelectedValue,
                CurrentIdleEntry + "Size changed");
        }
        private void Idle_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentIdle.GetAddress(CurrentIdle.EntryIndex, "Sprite"),
                IdlePointerBox.Value,
                CurrentIdleEntry + "Sprite repointed");
        }
        private void Walk_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentWalk.GetAddress(CurrentWalk.EntryIndex, "Sprite"),
                WalkPointerBox.Value,
                CurrentWalkEntry + "Sprite repointed");
        }
        private void Anim_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentWalk.GetAddress(CurrentWalk.EntryIndex, "AnimData"),
                AnimPointerBox.Value,
                CurrentWalkEntry + "Animation repointed");
        }
        private void Unknown_NumBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentIdle.GetAddress(CurrentIdle.EntryIndex),
                UnknownNumberBox.Value,
                CurrentIdleEntry + "Unknown byte changed");
        }

        private void Test_TrackBar_ValueChanged(object sender, EventArgs e)
        {
            Core_LoadTestView();
        }
        private void Test_Idle_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_Idle.Checked)
                Test_TrackBar.Maximum = 2;

            Core_LoadTestView();
        }
        private void Test_WalkUp_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_WalkSide.Checked)
                Test_TrackBar.Maximum = 3;

            Core_LoadTestView();
        }
        private void Test_WalkSide_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_WalkDown.Checked)
                Test_TrackBar.Maximum = 3;

            Core_LoadTestView();
        }
        private void Test_WalkDown_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_WalkUp.Checked)
                Test_TrackBar.Maximum = 3;

            Core_LoadTestView();
        }
        private void Test_Selected_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_Selected.Checked)
                Test_TrackBar.Maximum = 2;

            Core_LoadTestView();
        }

        private void PaletteArrayBox_ValueChanged(object sender, EventArgs e)
        {
            Core_Update();
        }
        private void PaletteBox_Click(object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                "Map Sprite Palette 0x" + PaletteArrayBox.Value + " [" + PaletteArrayBox.Text + "] - ",
                CurrentPaletteAddress, 1);
        }
    }
}