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
        public StructFile CurrentMove;
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
                return "Map Sprite (Idle) 0x" + Util.ByteToHex(Idle_EntryArrayBox.Value) + " [" + Idle_EntryArrayBox.Text + "] - ";
            }
        }
        string CurrentMoveEntry
        {
            get
            {
                return "Map Sprite (Move) 0x" + Util.ByteToHex(Move_EntryArrayBox.Value) + " [" + Move_EntryArrayBox.Text + "] - ";
            }
        }



        public MapSpriteEditor()
        {
            InitializeComponent();

            try
            {
                Idle_EntryArrayBox.Load("Map Sprite List.txt");
                Move_EntryArrayBox.Load("Class List.txt");
                CurrentIdle = new StructFile("Map Sprite Idle Struct.txt");
                CurrentMove = new StructFile("Map Sprite Move Struct.txt");
                CurrentIdle.Address = Core.GetPointer("Map Sprite Idle Array");
                CurrentMove.Address = Core.GetPointer("Map Sprite Move Array");

                PaletteArrayBox.Load("Map Sprite Palettes.txt");

                Idle_Size_ComboBox.DataSource = new KeyValuePair<string, byte>[3]
                {
                    new KeyValuePair<string, byte>("16x16", 0x00),
                    new KeyValuePair<string, byte>("16x32", 0x01),
                    new KeyValuePair<string, byte>("32x32", 0x02)
                };
                Idle_Size_ComboBox.ValueMember = "Value";
                Idle_Size_ComboBox.DisplayMember = "Key";

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
            Idle_EntryArrayBox.Value = (byte)entry;
            Move_EntryArrayBox.Value = (byte)Move_EntryArrayBox.File.FindEntry(Idle_EntryArrayBox.Text);
        }
        public override void Core_OnOpen()
        {
            Move_EntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;
            Move_EntryArrayBox.Value = 1;
            Move_EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;

            Core_Update();
        }
        public override void Core_Update()
        {
            CurrentIdle.EntryIndex = Idle_EntryArrayBox.Value;
            CurrentMove.EntryIndex = Move_EntryArrayBox.Value - 1;

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
                    Core.ReadData((Pointer)CurrentMove["Sprite"], 0),
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
                if (Test_MoveSide.Checked)
                {
                    tilemap.Map(MapSprite.Map_Test(frame));
                    tileset = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet;
                }
                frame += 16 * 4;
                if (Test_MoveDown.Checked)
                {
                    tilemap.Map(MapSprite.Map_Test(frame));
                    tileset = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet;
                }
                frame += 16 * 4;
                if (Test_MoveUp.Checked)
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
            Idle_Size_ComboBox.SelectedValueChanged -= Idle_Size_ComboBox_ValueChanged;
            UnknownNumberBox.ValueChanged -= Unknown_NumBox_ValueChanged;
            Idle_PointerBox.ValueChanged -= Idle_PointerBox_ValueChanged;
            Move_PointerBox.ValueChanged -= Move_PointerBox_ValueChanged;
            AnimPointerBox.ValueChanged -= Anim_PointerBox_ValueChanged;

            try
            {
                Idle_Size_ComboBox.SelectedValue = (byte)CurrentIdle["Size"];
                UnknownNumberBox.Value = (byte)CurrentIdle["Unknown"];
                Idle_PointerBox.Value = (Pointer)CurrentIdle["Sprite"];
                Move_PointerBox.Value = (Pointer)CurrentMove["Sprite"];
                AnimPointerBox.Value = (Pointer)CurrentMove["AnimData"];
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the values.", ex);

                Idle_Size_ComboBox.SelectedValue = 0;
                Idle_PointerBox.Value = new Pointer();
                Move_PointerBox.Value = new Pointer();
                AnimPointerBox.Value = new Pointer();
                UnknownNumberBox.Value = 0;
            }

            Idle_Size_ComboBox.SelectedValueChanged += Idle_Size_ComboBox_ValueChanged;
            UnknownNumberBox.ValueChanged += Unknown_NumBox_ValueChanged;
            Idle_PointerBox.ValueChanged += Idle_PointerBox_ValueChanged;
            Move_PointerBox.ValueChanged += Move_PointerBox_ValueChanged;
            AnimPointerBox.ValueChanged += Anim_PointerBox_ValueChanged;
        }
        
        void Core_Insert(MapSprite insert)
        {
            Core.SuspendUpdate();
            try
            {
                byte[] data_idle = insert.Sprites[MapSprite.IDLE].Sheet.ToBytes(true);
                byte[] data_move = insert.Sprites[MapSprite.WALK].Sheet.ToBytes(true);

                bool cancel = Prompt.ShowRepointDialog(this, "Repoint Map Sprite",
                    "The Map Sprite to insert might need to be repointed.",
                        CurrentIdleEntry,
                    new Tuple<string, Pointer, int>[] {
                        Tuple.Create("Idle Sprite", (Pointer)CurrentIdle["Sprite"], data_idle.Length),
                        Tuple.Create("Move Sprite", (Pointer)CurrentMove["Sprite"], data_move.Length)},
                    new Pointer[] {
                        CurrentIdle.GetAddress(CurrentIdle.EntryIndex, "Sprite"),
                        CurrentMove.GetAddress(CurrentMove.EntryIndex, "Sprite")});
                if (cancel) return;

                Core.WriteData(this,
                    (Pointer)CurrentIdle["Sprite"],
                    data_idle,
                    CurrentIdleEntry + "Idle Sprite changed");

                Core.WriteData(this,
                    (Pointer)CurrentMove["Sprite"],
                    data_move,
                    CurrentMoveEntry + "Move Sprite changed");
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
                GBA.Image image = new GBA.Image(filepath, CurrentPalette);

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
            string move_path = null;
            if (filepath.EndsWith("idle.chr", StringComparison.OrdinalIgnoreCase))
            {
                idle_path = filepath;
                move_path = filepath.Substring(0, filepath.Length - 8) + "move.chr";
            }
            if (filepath.EndsWith("move.chr", StringComparison.OrdinalIgnoreCase))
            {
                idle_path = filepath.Substring(0, filepath.Length - 8) + "idle.chr";
                move_path = filepath;
            }
            if (idle_path == null || move_path == null)
            {
                Program.ShowError("Selected file has invalid name.\r\n" +
                "Image data files must end with either 'idle.chr' or 'move.chr').");
                return;
            }

            Core_Insert(new MapSprite(
                CurrentPalette,
                File.ReadAllBytes(idle_path),
                File.ReadAllBytes(move_path),
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
                byte[] data_move = CurrentMapSprite.Sprites[MapSprite.WALK].Sheet.ToBytes(false);

                File.WriteAllBytes(path + file + " idle.chr", data_idle);
                File.WriteAllBytes(path + file + " move.chr", data_move);
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
                "Image data (.idle.chr + .move.chr)|*.chr|" +
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
                if (openWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase))
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
                "Image file (*.png, *.bmp)|*.png;*.bmp|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void File_Tools_CreateImage_Click(Object sender, EventArgs e)
        {
            GBA.Bitmap idle = null;
            GBA.Bitmap move = null;
            byte size;

            OpenFileDialog openWindow_idle = new OpenFileDialog();
            openWindow_idle.RestoreDirectory = true;
            openWindow_idle.Multiselect = false;
            openWindow_idle.FilterIndex = 1;
            openWindow_idle.Filter =
                "'IDLE' Map sprite image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "All files (*.*)|*.*";
            if (openWindow_idle.ShowDialog() == DialogResult.OK)
            {
                if (openWindow_idle.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow_idle.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow_idle.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        idle = new GBA.Bitmap(openWindow_idle.FileName, CurrentPalette);
                    }
                    catch (Exception ex)
                    {
                        Program.ShowError("Could not read bitmap for map sprite 'idle'.", ex);
                        return;
                    }
                    if (idle.Width == 16 && idle.Height == 16 * 3) size = 0x00;
                    else if (idle.Width == 16 && idle.Height == 32 * 3) size = 0x01;
                    else if (idle.Width == 32 && idle.Height == 32 * 3) size = 0x02;
                    else
                    {
                        Program.ShowError("Invalid IDLE Map sprite image file, size should be either 16x48, 16x96, or 32x96 pixels");
                        return;
                    }
                }
                else
                {
                    Program.ShowError("File chosen has invalid extension.\r\n" + openWindow_idle.FileName);
                    return;
                }
            }
            else return;

            OpenFileDialog openWindow_move = new OpenFileDialog();
            openWindow_move.RestoreDirectory = true;
            openWindow_move.Multiselect = false;
            openWindow_move.FilterIndex = 1;
            openWindow_move.Filter =
                "'MOVE' Map sprite image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "'MOVE' Map sprite image data (.move.chr)|*.chr|" +
                "All files (*.*)|*.*";
            if (openWindow_move.ShowDialog() == DialogResult.OK)
            {
                if (openWindow_move.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow_move.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow_move.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        move = new GBA.Bitmap(openWindow_move.FileName, CurrentPalette);
                    }
                    catch (Exception ex)
                    {
                        Program.ShowError("Could not read bitmap for map sprite 'move'.", ex);
                        return;
                    }
                    if (move.Width != 32 && move.Height != 480)
                    {
                        Program.ShowError("Invalid MOVE Map sprite image file, size should be 32x480 pixels");
                        return;
                    }
                }
                else if (openWindow_move.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO
                    return;
                }
                else
                {
                    Program.ShowError("File chosen has invalid extension.\r\n" + openWindow_move.FileName);
                    return;
                }
            }
            else return;

            GBA.Bitmap result = new GBA.Bitmap(
                MapSprite.WIDTH * Tile.SIZE,
                MapSprite.HEIGHT * Tile.SIZE);
            result.Colors = Core.ReadPalette(Core.CurrentROM.Address_MapSpritePalettes(), GBA.Palette.LENGTH * 8);
            result.SetPixels(delegate (int x, int y) { return ((byte)move[x, y + 32 *  0]); }, new Rectangle( 32,  0, 32, 32 * 4)); // MOVE side
            result.SetPixels(delegate (int x, int y) { return ((byte)move[x, y + 32 *  4]); }, new Rectangle( 64,  0, 32, 32 * 4)); // MOVE down
            result.SetPixels(delegate (int x, int y) { return ((byte)move[x, y + 32 *  8]); }, new Rectangle( 96,  0, 32, 32 * 4)); // MOVE up
            result.SetPixels(delegate (int x, int y) { return ((byte)move[x, y + 32 * 12]); }, new Rectangle(128, 32, 32, 32 * 3)); // MOVE cursor-hover
            switch (size)
            {
                case 0x00: // IDLE 16x16
                    result.SetPixels(delegate (int x, int y) { return ((byte)idle[x, y + 16 * 0]); }, new Rectangle(8, 32 * 1 + 16, 16, 16));
                    result.SetPixels(delegate (int x, int y) { return ((byte)idle[x, y + 16 * 1]); }, new Rectangle(8, 32 * 2 + 16, 16, 16));
                    result.SetPixels(delegate (int x, int y) { return ((byte)idle[x, y + 16 * 2]); }, new Rectangle(8, 32 * 3 + 16, 16, 16));
                    break;
                case 0x01: // IDLE 16x32
                    result.SetPixels(delegate (int x, int y) { return ((byte)idle[x, y]); }, new Rectangle(8, 32, 16, 32 * 3)); break;
                case 0x02: // IDLE 32x32
                    result.SetPixels(delegate (int x, int y) { return ((byte)idle[x, y]); }, new Rectangle(0, 32, 32, 32 * 3)); break;
                default: throw new Exception("Invalid map sprite size value recieved.");
            }

            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Image file (*.png, *.bmp)|*.png;*.bmp|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    saveWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Core.SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4),
                            Tile.SIZE * MapSprite.WIDTH,
                            Tile.SIZE * MapSprite.HEIGHT,
                            Palette.Split(CurrentPalette, 8),
                            delegate (int x, int y)
                            {
                                return (byte)result[x, y];
                            });
                    }
                    catch (Exception ex)
                    {
                        Program.ShowError("Could not save image.", ex);
                    }
                    return;
                }
                else
                {
                    Program.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
                    return;
                }
            }
        }



        private void EntryArrayBox_ValueChanged(object sender, EventArgs e)
        {
            Core_Update();
        }
        private void Entry_DecrementBoth_Button_Click(object sender, EventArgs e)
        {
            Idle_EntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;
            Move_EntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;

            Idle_EntryArrayBox.Value -= 1;
            Move_EntryArrayBox.Value -= 1;

            Idle_EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;
            Move_EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;

            Core_Update();
        }
        private void Entry_IncrementBoth_Button_Click(object sender, EventArgs e)
        {
            Idle_EntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;
            Move_EntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;

            Idle_EntryArrayBox.Value += 1;
            Move_EntryArrayBox.Value += 1;

            Idle_EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;
            Move_EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;

            Core_Update();
        }

        private void Idle_Size_ComboBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentIdle.GetAddress(CurrentIdle.EntryIndex, "Size"),
                (byte)Idle_Size_ComboBox.SelectedValue,
                CurrentIdleEntry + "Size changed");
        }
        private void Idle_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentIdle.GetAddress(CurrentIdle.EntryIndex, "Sprite"),
                Idle_PointerBox.Value,
                CurrentIdleEntry + "Sprite repointed");
        }
        private void Move_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentMove.GetAddress(CurrentMove.EntryIndex, "Sprite"),
                Move_PointerBox.Value,
                CurrentMoveEntry + "Sprite repointed");
        }
        private void Anim_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentMove.GetAddress(CurrentMove.EntryIndex, "AnimData"),
                AnimPointerBox.Value,
                CurrentMoveEntry + "Animation repointed");
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
        private void Test_MoveUp_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_MoveSide.Checked)
                Test_TrackBar.Maximum = 3;

            Core_LoadTestView();
        }
        private void Test_MoveSide_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_MoveDown.Checked)
                Test_TrackBar.Maximum = 3;

            Core_LoadTestView();
        }
        private void Test_MoveDown_CheckedChanged(object sender, EventArgs e)
        {
            if (Test_MoveUp.Checked)
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

        private void Idle_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();
            Program.Core.Core_OpenEditor(editor);

            byte size = (byte)CurrentIdle["Size"];

            editor.Core_SetEntry(
                (size < 0x2 ? 2 : 4),
                (size < 0x1 ? 2 : 4) * 3,
                CurrentPaletteAddress, false,
                (Pointer)CurrentIdle["Sprite"], true);
        }
        private void Move_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();
            Program.Core.Core_OpenEditor(editor);

            editor.Core_SetEntry(4, 60,
                CurrentPaletteAddress, false,
                (Pointer)CurrentMove["Sprite"], true);
        }
    }
}