using Compression;
using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class PortraitEditor : Editor
    {
        private StructFile Current;
        private Portrait CurrentPortrait;
        private GBA.SpriteSheet TestPortrait;

        /// <summary>
        /// Gets a string of the current byte index of portrait in the array
        /// </summary>
        string CurrentEntry
        {
            get
            {
                return "Portrait 0x" + Util.UInt16ToHex(EntryArrayBox.Value) + " [" + EntryArrayBox.Text + "] - ";
            }
        }
        /// <summary>
        /// Whether or not the current portrait is a generic class card portrait
        /// </summary>
        bool IsGenericClassCard
        {
            get
            {
                return (Core.CurrentROM is FE6) ? (Current["Chibi"] == 0) : (Current["Face"] == 0);
            }
        }



        public PortraitEditor()
        {
            try
            {
                InitializeComponent();

                EntryArrayBox.Load("Portrait List.txt");
                Current = new StructFile("Portrait Struct.txt");
                Current.Address = Core.GetPointer("Portrait Array");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }
        
        public override void Core_SetEntry(uint entry)
        {
            EntryArrayBox.Value = (UInt16)entry;
        }
        public override void Core_OnOpen()
        {
            EntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;
            EntryArrayBox.Value = 1;
            EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;

            Core_Update();
        }
        public override void Core_Update()
        {
            Current.EntryIndex = EntryArrayBox.Value;

            Core_UpdatePortrait();
            Core_LoadValues();
            Core_CheckControls();
            Core_UpdateTestView();
        }
        
        void Core_UpdatePortrait()
        {
            try
            {
                if (IsGenericClassCard)
                {
                    CurrentPortrait = new Portrait(
                        Core.ReadPalette((Pointer)Current["Palette"], Palette.LENGTH),
                        Core.ReadData((Pointer)Current[(Core.CurrentROM is FE6) ? "Main" : "Card"], 0));
                }
                else
                {
                    CurrentPortrait = new Portrait(
                        Core.ReadPalette((Pointer)Current["Palette"], Palette.LENGTH),
                        ((Core.CurrentROM is FE7 && Core.CurrentROM.Version == GameVersion.JAP) || Core.CurrentROM is FE8) ?
                            Core.ReadData((Pointer)Current["Face"] + 4, Portrait.Face_Length) :
                            Core.ReadData((Pointer)Current[(Core.CurrentROM is FE6) ? "Main" : "Face"], 0),
                        Core.ReadData((Pointer)Current["Chibi"], (Core.CurrentROM is FE6) ? Portrait.Chibi_Length : 0),
                        (Core.CurrentROM is FE6) ? null :
                        Core.ReadData((Pointer)Current["Mouth"], Portrait.Mouth_Length));
                }

                Image_ImageBox.Load(CurrentPortrait);
                Palette_PaletteBox.Load(CurrentPortrait.Colors);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the portrait image.", ex);
                Image_ImageBox.Reset();
                Palette_PaletteBox.Reset();
            }
        }
        void Core_UpdateTestView()
        {
            try
            {
                GBA.Palette palette = new GBA.Palette(CurrentPortrait.Colors);
                palette[0] = palette[0].SetAlpha(true);
                for (int i = 1; i < palette.Count; i++)
                {
                    palette[i] = palette[i].SetAlpha(false);
                }   // force correct alpha on the palette (1st color transparent, all others opaque)
                GBA.TileMap tilemap = new TileMap(Portrait.Map_Test(IsGenericClassCard));

                TestPortrait = new SpriteSheet(tilemap.Width * 8, tilemap.Height * 8);

                if (!IsGenericClassCard)
                {
                    tilemap = new GBA.TileMap(GetTileMap_Mouth(
                        Test_Mouth_Smile_RadioButton.Checked,
                        Test_Mouth_TrackBar.Value));
                    TestPortrait.AddSprite(
                        new Sprite(palette,
                        (Core.CurrentROM is FE6 || (Test_Mouth_Smile_RadioButton.Checked && Test_Mouth_TrackBar.Value == 0)) ?
                        CurrentPortrait.Sprites[Portrait.MAIN].Sheet :
                        CurrentPortrait.Sprites[Portrait.MOUTH].Sheet,
                        tilemap),
                        (byte)Current["MouthX"] * 8,
                        (byte)Current["MouthY"] * 8);

                    tilemap = new GBA.TileMap(GetTileMap_Eyes(
                        EyesClosed_CheckBox.Checked,
                        Test_Blink_TrackBar.Value));
                    TestPortrait.AddSprite(
                        new Sprite(palette,
                        CurrentPortrait.Sprites[Portrait.MAIN].Sheet,
                        tilemap),
                        (byte)Current["BlinkX"] * 8,
                        (byte)Current["BlinkY"] * 8);
                }
                tilemap = new TileMap(Portrait.Map_Test(IsGenericClassCard));
                TestPortrait.AddSprite(
                    new Sprite(palette,
                    CurrentPortrait.Sprites[Portrait.MAIN].Sheet,
                    tilemap),
                    0, 0);

                Test_ImageBox.Load(TestPortrait);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the portrait test view.", ex);
                Test_ImageBox.Reset();
            }
        }
        void Core_LoadValues()
        {
            Palette_PointerBox.ValueChanged -= Palette_PointerBox_Changed;
            Image_PointerBox.ValueChanged -= Image_PointerBox_Changed;
            Chibi_PointerBox.ValueChanged -= Chibi_PointerBox_Changed;
            Mouth_PointerBox.ValueChanged -= Mouth_PointerBox_Changed;
            MouthX_ByteBox.ValueChanged -= MouthX_NumBox_Changed;
            MouthY_ByteBox.ValueChanged -= MouthY_NumBox_Changed;
            BlinkX_ByteBox.ValueChanged -= BlinkX_NumBox_Changed;
            BlinkY_ByteBox.ValueChanged -= BlinkY_NumBox_Changed;
            EyesClosed_CheckBox.CheckedChanged -= EyesClosed_CheckBox_Changed;
            
            try
            {
                Palette_PointerBox.Value = (Pointer)Current["Palette"];
                Chibi_PointerBox.Value = (Pointer)Current["Chibi"];

                MouthX_ByteBox.Value = (byte)Current["MouthX"];
                MouthY_ByteBox.Value = (byte)Current["MouthY"];
                BlinkX_ByteBox.Value = (byte)Current["BlinkX"];
                BlinkY_ByteBox.Value = (byte)Current["BlinkY"];

                if (Core.CurrentROM is FE6)
                {
                    Image_PointerBox.Value = (Pointer)Current["Main"];
                }
                else
                {
                    Image_PointerBox.Value = (IsGenericClassCard) ?
                        (Pointer)Current["Card"] :
                        (Pointer)Current["Face"];
                    Mouth_PointerBox.Value = (Pointer)Current["Mouth"];

                    EyesClosed_CheckBox.Checked = (Current["EyesClosed"] == 0x06);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the values.", ex);

                Palette_PointerBox.Value = new Pointer();
                Image_PointerBox.Value = new Pointer();
                Chibi_PointerBox.Value = new Pointer();
                Mouth_PointerBox.Value = new Pointer();
                MouthX_ByteBox.Value = 0;
                MouthY_ByteBox.Value = 0;
                BlinkX_ByteBox.Value = 0;
                BlinkY_ByteBox.Value = 0;
            }

            Palette_PointerBox.ValueChanged += Palette_PointerBox_Changed;
            Image_PointerBox.ValueChanged += Image_PointerBox_Changed;
            Chibi_PointerBox.ValueChanged += Chibi_PointerBox_Changed;
            Mouth_PointerBox.ValueChanged += Mouth_PointerBox_Changed;
            MouthX_ByteBox.ValueChanged += MouthX_NumBox_Changed;
            MouthY_ByteBox.ValueChanged += MouthY_NumBox_Changed;
            BlinkX_ByteBox.ValueChanged += BlinkX_NumBox_Changed;
            BlinkY_ByteBox.ValueChanged += BlinkY_NumBox_Changed;
            EyesClosed_CheckBox.CheckedChanged += EyesClosed_CheckBox_Changed;
        }
        void Core_CheckControls()
        {
            switch (CurrentPortrait.Type)
            {
                case PortraitType.Portrait:
                    Test_Blink_Label.Enabled = !(Core.CurrentROM is FE6);
                    Test_Blink_TrackBar.Enabled = !(Core.CurrentROM is FE6);
                    Test_Mouth_TrackBar.Enabled = true;
                    Test_Mouth_TrackBar.Minimum = 0;
                    Test_Mouth_TrackBar.Maximum = (Core.CurrentROM is FE6) ? 2 : 3;
                    Test_Mouth_Smile_RadioButton.Enabled = true;
                    Test_Mouth_Frown_RadioButton.Enabled = true;
                    break;
                case PortraitType.Generic:
                    Test_Blink_Label.Enabled = false;
                    Test_Blink_TrackBar.Enabled = false;
                    Test_Mouth_TrackBar.Enabled = false;
                    Test_Mouth_TrackBar.Minimum = 0;
                    Test_Mouth_TrackBar.Maximum = 1;
                    Test_Mouth_Smile_RadioButton.Enabled = false;
                    Test_Mouth_Frown_RadioButton.Enabled = false;
                    break;
                case PortraitType.Shop:
                    Test_Blink_Label.Enabled = false;
                    Test_Blink_TrackBar.Enabled = false;
                    Test_Mouth_TrackBar.Enabled = true;
                    Test_Mouth_TrackBar.Minimum = 1;
                    Test_Mouth_TrackBar.Maximum = 3;
                    Test_Mouth_Smile_RadioButton.Enabled = true;
                    Test_Mouth_Frown_RadioButton.Enabled = true;
                    break;
                default: break;
            }
        }
        
        void Core_Insert(Portrait insert)
        {
            Core.SuspendUpdate();
            try
            {
                int header = ((Core.CurrentROM is FE8 ||
                    (Core.CurrentROM is FE7 && Core.CurrentROM.Version == GameVersion.JAP)) ? 4 : 0);
                byte[] data_main = insert.Sprites[Portrait.MAIN].Sheet.ToBytes(false);
                byte[] data_chibi = null;
                byte[] data_mouth = null;
                byte[] data_palette = new byte[Palette.LENGTH];
                Array.Copy(insert.Colors.ToBytes(false), data_palette, insert.Colors.Count * 2);

                var repoints = new List<Tuple<string, Pointer, int>>();
                var writepos = new List<Pointer>();

                repoints.Add(Tuple.Create("Palette", (Pointer)Current["Palette"], data_palette.Length));
                writepos.Add(Current.GetAddress(Current.EntryIndex, "Palette"));
                
                if (IsGenericClassCard)
                {
                    data_main = LZ77.Compress(data_main);

                    repoints.Add(Tuple.Create("Card", (Pointer)Current["Card"], header + data_main.Length));
                    writepos.Add(Current.GetAddress(Current.EntryIndex, "Card"));
                }
                else
                {
                    data_chibi = insert.Sprites[Portrait.CHIBI].Sheet.ToBytes(false);
                    data_mouth = insert.Sprites[Portrait.MOUTH].Sheet.ToBytes(false);

                    if (Core.CurrentROM is FE6 ||
                       (Core.CurrentROM is FE7 && Core.CurrentROM.Version != GameVersion.JAP))
                        data_main = LZ77.Compress(data_main);

                    repoints.Add(Tuple.Create("Face", (Pointer)Current["Face"], header + data_main.Length));
                    writepos.Add(Current.GetAddress(Current.EntryIndex, "Face"));

                    if (!(Core.CurrentROM is FE6))
                    {
                        repoints.Add(Tuple.Create("Mouth", (Pointer)Current["Mouth"], data_mouth.Length));
                        writepos.Add(Current.GetAddress(Current.EntryIndex, "Mouth"));

                        data_chibi = LZ77.Compress(data_chibi);
                    }
                    repoints.Add(Tuple.Create("Chibi", (Pointer)Current["Chibi"], data_chibi.Length));
                    writepos.Add(Current.GetAddress(Current.EntryIndex, "Chibi"));
                }

                bool cancel = Prompt.ShowRepointDialog(this, "Repoint Portrait",
                    "The portrait to insert might need some of its parts to be repointed.",
                    CurrentEntry, repoints.ToArray(), writepos.ToArray());
                if (cancel) return;

                Core.WriteData(this,
                    (Pointer)Current["Palette"],
                    data_palette,
                    CurrentEntry + "Palette changed");

                if (IsGenericClassCard)
                {
                    Core.WriteData(this,
                        (Pointer)Current["Card"],
                        data_main,
                        CurrentEntry + "Image changed");
                }
                else
                {
                    Core.WriteData(this,
                        (Pointer)Current["Face"] + header,
                        data_main,
                        CurrentEntry + "Main image changed");

                    Core.WriteData(this,
                        (Pointer)Current["Chibi"],
                        data_chibi,
                        CurrentEntry + "Chibi image changed");

                    if (!(Program.Core.CurrentROM is FE6))
                        Core.WriteData(this,
                            (Pointer)Current["Mouth"],
                            data_mouth,
                            CurrentEntry + "Mouth image changed");
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the image.", ex);
            }
            Core.ResumeUpdate();
            Core.PerformUpdate();
        }

        void Core_InsertImage(string filepath)
        {
            Portrait portrait;
            try
            {
                GBA.Image image = new GBA.Image(filepath);

                int regular_width = Portrait.WIDTH * Tile.SIZE;
                int regular_height = Portrait.HEIGHT * Tile.SIZE;
                int generic_width = Portrait.Card_Width * Tile.SIZE;
                int generic_height = Portrait.Card_Height * Tile.SIZE;
                if (image.Width == regular_width && image.Height == regular_height)
                {
                    portrait = new Portrait(image, false);
                }
                else if (image.Width == generic_width && image.Height == generic_height)
                {
                    portrait = new Portrait(image, true);
                }
                else throw new Exception("Image given has invalid dimensions.\r\n" +
                    "It must be " + regular_width + "x" + regular_height + " or " + 
                    generic_width + "x" + generic_height + " (for a generic card portrait)");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the image file.", ex);
                return;
            }
            Core_Insert(portrait);
        }
        void Core_InsertData(string filepath)
        {

        }
        void Core_SaveImage(string filepath)
        {
            try
            {
                /*
                System.Drawing.Imaging.ImageFormat format;
                if (filepath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    format = System.Drawing.Imaging.ImageFormat.Png;
                if (filepath.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                    format = System.Drawing.Imaging.ImageFormat.Bmp;
                if (filepath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    format = System.Drawing.Imaging.ImageFormat.Gif;
                */
                using (var image = new System.Drawing.Bitmap(
                    CurrentPortrait.Width,
                    CurrentPortrait.Height))
                {
                    for (int y = 0; y < image.Height; y++)
                    for (int x = 0; x < image.Width; x++)
                    {
                        image.SetPixel(x, y, (System.Drawing.Color)CurrentPortrait[x, y]);
                    }
                    for (int i = 0; i < Palette.MAX; i++)
                    {
                        image.SetPixel(i, 0, (System.Drawing.Color)CurrentPortrait.Colors[i]);
                    }
                    image.Save(filepath, System.Drawing.Imaging.ImageFormat.Png); //format);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save image", ex);
            }
        }
        void Core_SaveData(string filepath)
        {

        }



        /// <summary>
        /// Returns the tile mapping array of eyes according to the given parameters
        /// </summary>
        int?[,] GetTileMap_Eyes(bool closed, int blink)
        {
            if (Program.Core.CurrentROM is FE6)
            {
                return new int?[0, 0];
            }
            else
            {
                if (closed || blink == 2)
                {
                    return new int?[4, 2] {
                        { 0x58, 0x78 },
                        { 0x59, 0x79 },
                        { 0x5A, 0x7A },
                        { 0x5B, 0x7B }};
                }
                else if (blink == 1)
                {
                    return new int?[4, 2] {
                        { 0x18, 0x38 },
                        { 0x19, 0x39 },
                        { 0x1A, 0x3A },
                        { 0x1B, 0x3B }};
                }
                else return new int?[0, 0];
            }
        }
        /// <summary>
        /// Returns the tile mapping array of mouth tiles according to the given parameters 
        /// </summary>
        int?[,] GetTileMap_Mouth(bool smiling, int openness)
        {
            if (Program.Core.CurrentROM is FE6)
            {
                if (smiling) switch (openness)
                {
                    case 0: return new int?[0, 0];
                    case 1: return new int?[4, 2] {
                        { 0x5C, 0x7C },
                        { 0x5D, 0x7D },
                        { 0x5E, 0x7E },
                        { 0x5F, 0x7F }};
                    case 2: return new int?[4, 2] {
                        { 0x1C, 0x3C },
                        { 0x1D, 0x3D },
                        { 0x1E, 0x3E },
                        { 0x1F, 0x3F }};
                    default: throw new Exception("Invalid Mouth openness value");
                }
                else switch (openness)
                {
                    case 0: return new int?[4, 2] {
                        { 0x80, 0x84 },
                        { 0x81, 0x85 },
                        { 0x82, 0x86 },
                        { 0x83, 0x87 }};
                    case 1: return new int?[4, 2] {
                        { 0x58, 0x78 },
                        { 0x59, 0x79 },
                        { 0x5A, 0x7A },
                        { 0x5B, 0x7B }};
                    case 2: return new int?[4, 2] {
                        { 0x18, 0x38 },
                        { 0x19, 0x39 },
                        { 0x1A, 0x3A },
                        { 0x1B, 0x3B }};
                    default: throw new Exception("Invalid Mouth openness value");
                }
            }
            else
            {
                if (smiling) switch (openness)
                {
                    case 0: return new int?[4, 2] {
                        { 0x1C, 0x3C },
                        { 0x1D, 0x3D },
                        { 0x1E, 0x3E },
                        { 0x1F, 0x3F }};
                    case 1: return new int?[4, 2] {
                        { 0x10, 0x14 },
                        { 0x11, 0x15 },
                        { 0x12, 0x16 },
                        { 0x13, 0x17 }};
                    case 2: return new int?[4, 2] {
                        { 0x08, 0x0C },
                        { 0x09, 0x0D },
                        { 0x0A, 0x0E },
                        { 0x0B, 0x0F }};
                    case 3: return new int?[4, 2] {
                        { 0x00, 0x04 },
                        { 0x01, 0x05 },
                        { 0x02, 0x06 },
                        { 0x03, 0x07 }};
                    default: throw new Exception("Invalid Mouth openness value");
                }
                else switch (openness)
                {
                    case 0: return new int?[0, 0];
                    case 1: return new int?[4, 2] {
                        { 0x28, 0x2C },
                        { 0x29, 0x2D },
                        { 0x2A, 0x2E },
                        { 0x2B, 0x2F }};
                    case 2: return new int?[4, 2] {
                        { 0x20, 0x24 },
                        { 0x21, 0x25 },
                        { 0x22, 0x26 },
                        { 0x23, 0x27 }};
                    case 3: return new int?[4, 2] {
                        { 0x18, 0x1C },
                        { 0x19, 0x1D },
                        { 0x1A, 0x1E },
                        { 0x1B, 0x1F }};
                    default: throw new Exception("Invalid Mouth openness value");
                }
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
             // "Image data (.dmp + .pal)|*.dmp;*.pal|" +
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
                // "Image data (.dmp + .pal)|*.dmp;*.pal|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    saveWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    saveWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveImage(saveWindow.FileName);
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
        private void Test_ViewBox_Changed(object sender, EventArgs e)
        {
            Core_UpdateTestView();
        }

        private void Palette_PaletteBox_Click(object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this, CurrentEntry, (Pointer)Current["Palette"], 1);
        }
        private void Palette_PointerBox_Changed(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Palette"),
                Palette_PointerBox.Value,
                CurrentEntry + "Palette repointed");
        }
        private void Image_PointerBox_Changed(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                (Core.CurrentROM is FE6) ?
                    Current.GetAddress(Current.EntryIndex, "Main") :
                (IsGenericClassCard) ?
                    Current.GetAddress(Current.EntryIndex, "Card") :
                    Current.GetAddress(Current.EntryIndex, "Face"),
                Image_PointerBox.Value,
                CurrentEntry + "Main image repointed");
        }
        private void Chibi_PointerBox_Changed(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Chibi"),
                Chibi_PointerBox.Value,
                CurrentEntry + "Chibi image repointed");
        }
        private void Mouth_PointerBox_Changed(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Mouth"),
                Mouth_PointerBox.Value,
                CurrentEntry + "Mouth image repointed");
        }
        private void EyesClosed_CheckBox_Changed(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "EyesClosed"),
                (EyesClosed_CheckBox.Checked) ? (byte)0x06 : (byte)0x01,
                CurrentEntry + (EyesClosed_CheckBox.Checked ? "Eyes closed" : "Eyes opened"));
            Test_ViewBox_Changed(this, null);
        }

        private void MouthX_NumBox_Changed(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "MouthX"),
                MouthX_ByteBox.Value,
                CurrentEntry + "Mouth X changed");
            Test_ViewBox_Changed(this, null);
        }
        private void MouthY_NumBox_Changed(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "MouthY"),
                MouthY_ByteBox.Value,
                CurrentEntry + "Mouth Y changed");
            Test_ViewBox_Changed(this, null);
        }
        private void BlinkX_NumBox_Changed(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "BlinkX"),
                BlinkX_ByteBox.Value,
                CurrentEntry + "Blink X changed");
            Test_ViewBox_Changed(this, null);
        }
        private void BlinkY_NumBox_Changed(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "BlinkY"),
                BlinkY_ByteBox.Value,
                CurrentEntry + "Blink Y changed");
            Test_ViewBox_Changed(this, null);
        }
    }
}