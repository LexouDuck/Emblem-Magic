using Compression;
using Magic.Components;
using Magic.Properties;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Magic.Editors
{
    public partial class GraphicsEditor : Editor
    {
        Palette GrayScale { get; }



        public GraphicsEditor()
        {
            InitializeComponent();

            Tileset_2bpp_RadioButton.Enabled = false;

            GrayScale = new Palette();
            Byte value;
            for (Int32 i = 0; i < Palette.MAX; i++)
            {
                value = (Byte)(i * 16);
                GrayScale.Add(new GBA.Color(0, value, value, value));
            }
        }

        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            try
            {
                Byte[] palette = Core.ReadData(
                    Palette_PointerBox.Value + (Int32)Palette_Index_NumBox.Value * Palette.LENGTH,
                    Palette_CheckBox.Checked ? 0 : 16 * Palette.LENGTH);
                Byte[] tileset = Core.ReadData(
                    Tileset_PointerBox.Value,
                    Tileset_CheckBox.Checked ? 0 : (Tileset_8bpp_RadioButton.Checked ?
                        ((Int32)Width_NumBox.Value * (Int32)Height_NumBox.Value * Tile.SIZE * Tile.SIZE) :
                        ((Int32)Width_NumBox.Value * (Int32)Height_NumBox.Value * Tile.LENGTH)));

                if (Palette_Opaque_CheckBox.Checked)
                {
                    for (Int32 i = 0; i < palette.Length; i += 2)
                    {
                        palette[i + 1] &= 0x7F;
                    }
                }

                if (palette == null || palette.Length == 0)
                {
                    Palette_PaletteBox.Reset();
                    Image_ImageBox.Reset();
                    return;
                }
                if (tileset == null || tileset.Length == 0)
                {
                    Palette_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                    Image_ImageBox.Reset();
                    return;
                }

                IDisplayable image = null;
                if (TSA_Label.Checked && TSA_PointerBox.Value != new Pointer())
                {
                    image = new TSA_Image(palette, tileset,
                        Core.ReadTSA(TSA_PointerBox.Value,
                            (Int32)Width_NumBox.Value,
                            (Int32)Height_NumBox.Value,
                            TSA_CheckBox.Checked,
                            TSA_FlipRows_CheckBox.Checked));

                    Tool_OpenTSAEditor.Enabled = true;
                }
                else if (Tileset_8bpp_RadioButton.Checked)
                {
                    image = new Bitmap(
                        (Int32)Width_NumBox.Value * Tile.SIZE,
                        (Int32)Height_NumBox.Value * Tile.SIZE,
                        View_GrayscalePalette.Checked ?
                            GrayScale.ToBytes(false) :
                            palette,
                        tileset);

                    Tool_OpenTSAEditor.Enabled = false;
                }
                else
                {
                    image = new Tileset(tileset).ToImage(
                        (Int32)Width_NumBox.Value,
                        (Int32)Height_NumBox.Value,
                        View_GrayscalePalette.Checked ?
                            GrayScale.ToBytes(false) :
                            palette.GetBytes(0, Palette.LENGTH));

                    Tool_OpenTSAEditor.Enabled = false;
                }

                Palette_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                Image_ImageBox.Size = new System.Drawing.Size(image.Width, image.Height);
                Image_ImageBox.Load(image);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the image.", ex);

                Image_ImageBox.Reset();
            }
        }

        public void Core_SetEntry(
            Pointer palette, Boolean palette_compressed,
            Pointer tileset, Boolean tileset_compressed,
            Pointer tsa = new Pointer(), Boolean tsa_compressed = false, Boolean tsa_flipped = false)
        {
            Palette_PointerBox.ValueChanged      -= Palette_PointerBox_ValueChanged;
            Palette_CheckBox.CheckedChanged      -= Palette_CheckBox_CheckedChanged;
            Tileset_PointerBox.ValueChanged      -= Tileset_PointerBox_ValueChanged;
            Tileset_CheckBox.CheckedChanged      -= Tileset_CheckBox_CheckedChanged;
            TSA_PointerBox.ValueChanged          -= TSA_PointerBox_ValueChanged;     
            TSA_CheckBox.CheckedChanged          -= TSA_CheckBox_CheckedChanged;
            TSA_FlipRows_CheckBox.CheckedChanged -= TSA_FlipRows_CheckBox_CheckedChanged;

            Palette_PointerBox.Value = palette;
            Palette_CheckBox.Checked = palette_compressed;
            Tileset_PointerBox.Value = tileset;
            Tileset_CheckBox.Checked = tileset_compressed;
            TSA_PointerBox.Value = tsa;
            TSA_CheckBox.Checked = tsa_compressed;
            TSA_FlipRows_CheckBox.Checked = tsa_flipped;
            Palette_Offset_Label.Enabled = !Palette_CheckBox.Checked;
            Palette_Index_NumBox.Enabled = !Palette_CheckBox.Checked;

            Palette_PointerBox.ValueChanged      += Palette_PointerBox_ValueChanged;
            Palette_CheckBox.CheckedChanged      += Palette_CheckBox_CheckedChanged;
            Tileset_PointerBox.ValueChanged      += Tileset_PointerBox_ValueChanged;
            Tileset_CheckBox.CheckedChanged      += Tileset_CheckBox_CheckedChanged;
            TSA_PointerBox.ValueChanged          += TSA_PointerBox_ValueChanged;     
            TSA_CheckBox.CheckedChanged          += TSA_CheckBox_CheckedChanged;
            TSA_FlipRows_CheckBox.CheckedChanged += TSA_FlipRows_CheckBox_CheckedChanged;

            TSA_Label_CheckedChanged(this, null);
        }
        public void Core_SetEntry(Int32 width, Int32 height,
            Pointer palette, Boolean palette_compressed,
            Pointer tileset, Boolean tileset_compressed,
            Pointer tsa = new Pointer(), Boolean tsa_compressed = false, Boolean tsa_flipped = false)
        {
            Width_NumBox.ValueChanged  -= Width_NumBox_ValueChanged;
            Height_NumBox.ValueChanged -= Height_NumBox_ValueChanged;

            Width_NumBox.Value = width;
            Height_NumBox.Value = height;

            Width_NumBox.ValueChanged  += Width_NumBox_ValueChanged;
            Height_NumBox.ValueChanged += Height_NumBox_ValueChanged;

            Core_SetEntry(
                palette, palette_compressed,
                tileset, tileset_compressed,
                tsa, tsa_compressed, tsa_flipped);
        }

        void Core_Insert(
            Palette palette,
            Byte[] graphics,
            TSA_Array tsa = null)
        {
            UI.SuspendUpdate();
            try
            {
                Byte[] data_palette = palette.ToBytes(Palette_CheckBox.Checked);
                Byte[] data_tileset = Tileset_CheckBox.Checked ? LZ77.Compress(graphics) : graphics;
                Byte[] data_tsa = null;

                List<Tuple<String, Pointer, Int32>> repoints = new List<Tuple<String, Pointer, Int32>>();
                repoints.Add(Tuple.Create("Palette", Palette_PointerBox.Value, data_palette.Length));
                repoints.Add(Tuple.Create("Tileset", Tileset_PointerBox.Value, data_tileset.Length));
                if (tsa != null)
                {
                    data_tsa = tsa.ToBytes(TSA_CheckBox.Checked, TSA_FlipRows_CheckBox.Checked);
                    repoints.Add(Tuple.Create("TSA", TSA_PointerBox.Value, data_tsa.Length));
                }

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Graphics",
                    "The image and palette to insert might need to be repointed.",
                    "Image at " + Tileset_PointerBox.Value + " - ", repoints.ToArray());
                if (cancel) return;

                Core.WriteData(this,
                    Palette_PointerBox.Value,
                    data_palette,
                    "Palette at " + Palette_PointerBox.Value + " changed");

                Core.WriteData(this,
                    Tileset_PointerBox.Value,
                    data_tileset,
                    "Tileset at " + Tileset_PointerBox.Value + " changed");

                if (tsa != null)
                {
                    Core.WriteData(this,
                        TSA_PointerBox.Value,
                        data_tsa,
                        "TSA Array at " + TSA_PointerBox.Value + " changed");
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert image.", ex);
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_InsertImage(String filepath)
        {
            IDisplayable image;
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                if (TSA_Label.Checked && TSA_PointerBox.Value != new Pointer())
                {
                    Int32 width = TSA_FlipRows_CheckBox.Checked ?
                        ((TSA_Image)Image_ImageBox.Display).Tiling.Width :
                        (Int32)Width_NumBox.Value;
                    Int32 height = TSA_FlipRows_CheckBox.Checked ?
                        ((TSA_Image)Image_ImageBox.Display).Tiling.Height :
                        (Int32)Height_NumBox.Value;

                    if (palette == null)
                    {
                        image = new TSA_Image(
                            width, height,
                            new GBA.Bitmap(filepath),
                            Palette.MAX,
                            true);
                    }
                    else
                    {
                        image = new TSA_Image(
                            width, height,
                            new GBA.Bitmap(filepath),
                            palette,
                            Palette.MAX,
                            true);
                    }
                }
                else if (Tileset_8bpp_RadioButton.Checked)
                {
                    image = new GBA.Bitmap(filepath, palette);
                }
                else
                {
                    image = new GBA.Image(filepath, palette);
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load image file.", ex);
                Core_Update();
                return;
            }

            if (image is GBA.TSA_Image)
            {
                Core_Insert(
                    Palette.Merge(((TSA_Image)image).Palettes),
                    ((TSA_Image)image).Graphics.ToBytes(false),
                    ((TSA_Image)image).Tiling);
                return;
            }
            if (image is GBA.Bitmap)
            {
                Core_Insert(
                    ((Bitmap)image).Colors,
                    ((Bitmap)image).ToBytes());
                return;
            }
            if (image is GBA.Image)
            {
                Core_Insert(
                    ((Image)image).Colors,
                    new Tileset((Image)image).ToBytes(false));
                return;
            }
            UI.ShowError("Image couldn't be inserted because of an internal error.");
        }
        void Core_InsertData(String filepath)
        {
            String path = Path.GetDirectoryName(filepath) + '\\';
            String file = Path.GetFileNameWithoutExtension(filepath);

            Palette palette;
            Byte[] graphics;
            TSA_Array tsa = null;
            try
            {
                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");
                if (!File.Exists(path + file + ".chr"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".chr");

                palette = new Palette(path + file + ".pal", 256);

                graphics = File.ReadAllBytes(path + file + ".chr");

                if (!File.Exists(path + file + ".tsa"))
                {
                    tsa = new TSA_Array(
                        (Int32)Width_NumBox.Value,
                        (Int32)Height_NumBox.Value,
                        File.ReadAllBytes(path + file + ".tsa"));
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load image data files.", ex);
                return;
            }
            Core_Insert(palette, graphics, tsa);
        }
        void Core_SaveImage(String filepath)
        {
            Byte[] data = Core.ReadData(
                Palette_PointerBox.Value + (Int32)Palette_Index_NumBox.Value * Palette.LENGTH,
                Palette_CheckBox.Checked ? 0 : 16 * Palette.LENGTH);
            Palette[] palettes = new Palette[16];
            for (UInt32 i = 0; i < 16; i++)
            {
                palettes[i] = new Palette(data.GetBytes(i * Palette.LENGTH, Palette.LENGTH));
            }
            Core.SaveImage(filepath,
                Image_ImageBox.Display.Width,
                Image_ImageBox.Display.Height,
                palettes,
                delegate (Int32 x, Int32 y)
                {
                    Int32 palette = 0;
                    if (TSA_Label.Checked)
                        palette = ((TSA_Image)Image_ImageBox.Display).GetPaletteIndex(x, y);
                    return (Byte)palettes[palette].Find(Image_ImageBox.Display.GetColor(x, y));
                });
        }
        void Core_SaveData(String filepath)
        {
            String path = Path.GetDirectoryName(filepath) + '\\';
            String file = Path.GetFileNameWithoutExtension(filepath);
            try
            {
                IDisplayable image = Image_ImageBox.Display;

                Byte[] data_palette = null;
                Byte[] data_tileset = null;
                Byte[] data_tsa     = null;

                if (image is GBA.TSA_Image)
                {
                    data_palette = Palette.Merge(((TSA_Image)image).Palettes).ToBytes(false);
                    data_tileset = ((TSA_Image)image).Graphics.ToBytes(false);
                    data_tsa     = ((TSA_Image)image).Tiling.ToBytes(false, false);
                }
                if (image is GBA.Bitmap)
                {
                    data_palette = ((Bitmap)image).Colors.ToBytes(false);
                    data_tileset = ((Bitmap)image).ToBytes();
                }
                if (image is GBA.Image)
                {
                    data_palette = ((Image)image).Colors.ToBytes(false);
                    data_tileset = new Tileset((Image)image).ToBytes(false);
                }

                if (data_palette != null) File.WriteAllBytes(path + file + ".pal", data_palette);
                if (data_tileset != null) File.WriteAllBytes(path + file + ".chr", data_tileset);
                if (data_tsa     != null) File.WriteAllBytes(path + file + ".tsa", data_tsa);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save image data.", ex);
            }
        }

        void Core_FindPrevLZ77Address(PointerBox pointerBox, Boolean strict)
        {
            for (Pointer address = (pointerBox.Value - 4) - (pointerBox.Value % 4);
                 address >= 0;
                 address -= 4)
            {
                if (Core.ReadByte(address) == 0x10)
                {
                    if (strict)
                    {
                        UInt32 header = Util.BytesToUInt32(Core.ReadData(address, 4), true);
                        header >>= 8;
                        if ((header % 0x20 == 0) &&
                            (header <= 0x8000) &&
                            (header > 0))
                        {
                            pointerBox.Value = address;
                            return;
                        }
                    }
                    else
                    {
                        pointerBox.Value = address;
                        return;
                    }
                }
            }
        }
        void Core_FindNextLZ77Address(PointerBox pointerBox, Boolean strict)
        {
            for (Pointer address = (pointerBox.Value + 4) + (pointerBox.Value % 4);
                address < Core.CurrentROMSize;
                address += 4)
            {
                if (Core.ReadByte(address) == 0x10)
                {
                    if (strict)
                    {
                        UInt32 header = Util.BytesToUInt32(Core.ReadData(address, 4), true);
                        header >>= 8;
                        if ((header % 0x20 == 0) &&
                            (header <= 0x8000) &&
                            (header > 0))
                        {
                            pointerBox.Value = address;
                            return;
                        }
                    }
                    else
                    {
                        pointerBox.Value = address;
                        return;
                    }
                }
            }
        }



        private void File_Insert_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "Image data " + (TSA_Label.Checked ?
                    "(.tsa + .chr + .pal)|*.tsa;*.chr;*.pal|" :
                    "(.chr + .pal)|*.chr;*.pal|") +
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
                if (openWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    Core_InsertData(openWindow.FileName);
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
                "Image file (*.png)|*.png|" +
                "Image data " + (TSA_Label.Checked ?
                    "(.tsa + .chr + .pal)|*.tsa;*.chr;*.pal|" :
                    "(.chr + .pal)|*.chr;*.pal|") +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    saveWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase) ||
                    saveWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveData(saveWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                "Unknown Palette - ",
                Palette_PointerBox.Value + (Int32)Palette_Index_NumBox.Value * Palette.LENGTH,
                Palette_CheckBox.Checked ? 0 : 1);
        }
        private void Tool_OpenTSAEditor_Click(Object sender, EventArgs e)
        {
            TSA_Array tsa = ((TSA_Image)Image_ImageBox.Display).Tiling;

            UI.OpenTSAEditor(this,
                "Unknown TSA Array - ",
                Palette_PointerBox.Value, Palette_CheckBox.Checked ? 0 : 16 * Palette.LENGTH,
                Tileset_PointerBox.Value, Tileset_CheckBox.Checked ? 0 :
                    (Int32)(Width_NumBox.Value * Height_NumBox.Value) * Tile.LENGTH,
                TSA_PointerBox.Value,
                tsa.Width, tsa.Height,
                TSA_CheckBox.Checked, TSA_FlipRows_CheckBox.Checked);
        }

        private void View_GrayscalePalette_Click(Object sender, EventArgs e)
        {
            Core_Update();
        }



        private void Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void Palette_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Palette_Offset_Label.Enabled = !Palette_CheckBox.Checked;
            Palette_Index_NumBox.Enabled = !Palette_CheckBox.Checked;

            Core_Update();
        }
        private void Tileset_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void Tileset_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void TSA_Label_CheckedChanged(Object sender, EventArgs e)
        {
            if (TSA_Label.Checked)
            {
                TSA_PointerBox.Enabled = true;
                TSA_CheckBox.Enabled = true;

                TSA_GroupBox.Enabled = true;

                Tileset_2bpp_RadioButton.Enabled = false;
                Tileset_4bpp_RadioButton.CheckedChanged -= Tileset_4bpp_RadioButton_CheckedChanged;
                Tileset_4bpp_RadioButton.Checked = true;
                Tileset_4bpp_RadioButton.CheckedChanged += Tileset_4bpp_RadioButton_CheckedChanged;
                Tileset_8bpp_RadioButton.Enabled = false;
            }
            else
            {
                TSA_PointerBox.Enabled = false;
                TSA_CheckBox.Enabled = false;

                Size_GroupBox.Enabled = true;
                TSA_FlipRows_CheckBox.CheckedChanged -= TSA_FlipRows_CheckBox_CheckedChanged;
                TSA_FlipRows_CheckBox.Checked = false;
                TSA_FlipRows_CheckBox.CheckedChanged += TSA_FlipRows_CheckBox_CheckedChanged;
                TSA_GroupBox.Enabled = false;

                Tileset_2bpp_RadioButton.Enabled = false;
                Tileset_8bpp_RadioButton.Enabled = true;
            }
            Core_Update();
        }
        private void TSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void TSA_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Width_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void Height_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Tileset_2bpp_RadioButton_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void Tileset_4bpp_RadioButton_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void Tileset_8bpp_RadioButton_CheckedChanged(Object sender, EventArgs e)
        {
            if (Tileset_8bpp_RadioButton.Checked)
            {
                TSA_Label.CheckedChanged -= TSA_Label_CheckedChanged;
                TSA_Label.Checked = false;
                TSA_Label.CheckedChanged += TSA_Label_CheckedChanged;
                TSA_Label.Enabled = false;
            }
            else
            {
                TSA_Label.Enabled = true;
            }
            Core_Update();
        }

        private void Palette_Index_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void Palette_Opaque_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void TSA_Dimensions_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
        private void TSA_FlipRows_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            if (TSA_FlipRows_CheckBox.Checked)
            {
                Size_GroupBox.Enabled = false;
            }
            else
            {
                Size_GroupBox.Enabled = true;
            }
            Core_Update();
        }

        private void Prev_Palette_Button_Click(Object sender, EventArgs e)
        {
            Core_FindPrevLZ77Address(Palette_PointerBox, false);
        }
        private void Next_Palette_Button_Click(Object sender, EventArgs e)
        {
            Core_FindNextLZ77Address(Palette_PointerBox, false);
        }
        private void Prev_Tileset_Button_Click(Object sender, EventArgs e)
        {
            Core_FindPrevLZ77Address(Tileset_PointerBox, true);
        }
        private void Next_Tileset_Button_Click(Object sender, EventArgs e)
        {
            Core_FindNextLZ77Address(Tileset_PointerBox, true);
        }
        private void Prev_TSA_Button_Click(Object sender, EventArgs e)
        {
            Core_FindPrevLZ77Address(TSA_PointerBox, false);
        }
        private void Next_TSA_Button_Click(Object sender, EventArgs e)
        {
            Core_FindNextLZ77Address(TSA_PointerBox, false);
        }
    }
}
