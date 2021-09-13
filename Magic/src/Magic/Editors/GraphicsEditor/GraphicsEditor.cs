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
            this.InitializeComponent();

            this.Tileset_2bpp_RadioButton.Enabled = false;

            this.GrayScale = new Palette();
            Byte value;
            for (Int32 i = 0; i < Palette.MAX; i++)
            {
                value = (Byte)(i * 16);
                this.GrayScale.Add(new GBA.Color(0, value, value, value));
            }
        }

        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            try
            {
                Byte[] palette = Core.ReadData(
                    this.Palette_PointerBox.Value + (Int32)this.Palette_Index_NumBox.Value * Palette.LENGTH,
                    this.Palette_CheckBox.Checked ? 0 : 16 * Palette.LENGTH);
                Byte[] tileset = Core.ReadData(
                    this.Tileset_PointerBox.Value,
                    this.Tileset_CheckBox.Checked ? 0 : (this.Tileset_8bpp_RadioButton.Checked ?
                        ((Int32)this.Width_NumBox.Value * (Int32)this.Height_NumBox.Value * Tile.SIZE * Tile.SIZE) :
                        ((Int32)this.Width_NumBox.Value * (Int32)this.Height_NumBox.Value * Tile.LENGTH)));

                if (this.Palette_Opaque_CheckBox.Checked)
                {
                    for (Int32 i = 0; i < palette.Length; i += 2)
                    {
                        palette[i + 1] &= 0x7F;
                    }
                }

                if (palette == null || palette.Length == 0)
                {
                    this.Palette_PaletteBox.Reset();
                    this.Image_ImageBox.Reset();
                    return;
                }
                if (tileset == null || tileset.Length == 0)
                {
                    this.Palette_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                    this.Image_ImageBox.Reset();
                    return;
                }

                IDisplayable image = null;
                if (this.TSA_Label.Checked && this.TSA_PointerBox.Value != new Pointer())
                {
                    image = new TSA_Image(palette, tileset,
                        Core.ReadTSA(this.TSA_PointerBox.Value,
                            (Int32)this.Width_NumBox.Value,
                            (Int32)this.Height_NumBox.Value,
                            this.TSA_CheckBox.Checked,
                            this.TSA_FlipRows_CheckBox.Checked));

                    this.Tool_OpenTSAEditor.Enabled = true;
                }
                else if (this.Tileset_8bpp_RadioButton.Checked)
                {
                    image = new Bitmap(
                        (Int32)this.Width_NumBox.Value * Tile.SIZE,
                        (Int32)this.Height_NumBox.Value * Tile.SIZE,
                        this.View_GrayscalePalette.Checked ?
                            this.GrayScale.ToBytes(false) :
                            palette,
                        tileset);

                    this.Tool_OpenTSAEditor.Enabled = false;
                }
                else
                {
                    image = new Tileset(tileset).ToImage(
                        (Int32)this.Width_NumBox.Value,
                        (Int32)this.Height_NumBox.Value,
                        this.View_GrayscalePalette.Checked ?
                            this.GrayScale.ToBytes(false) :
                            palette.GetBytes(0, Palette.LENGTH));

                    this.Tool_OpenTSAEditor.Enabled = false;
                }

                this.Palette_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                this.Image_ImageBox.Size = new System.Drawing.Size(image.Width, image.Height);
                this.Image_ImageBox.Load(image);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the image.", ex);

                this.Image_ImageBox.Reset();
            }
        }

        public void Core_SetEntry(
            Pointer palette, Boolean palette_compressed,
            Pointer tileset, Boolean tileset_compressed,
            Pointer tsa = new Pointer(), Boolean tsa_compressed = false, Boolean tsa_flipped = false)
        {
            this.Palette_PointerBox.ValueChanged      -= this.Palette_PointerBox_ValueChanged;
            this.Palette_CheckBox.CheckedChanged      -= this.Palette_CheckBox_CheckedChanged;
            this.Tileset_PointerBox.ValueChanged      -= this.Tileset_PointerBox_ValueChanged;
            this.Tileset_CheckBox.CheckedChanged      -= this.Tileset_CheckBox_CheckedChanged;
            this.TSA_PointerBox.ValueChanged          -= this.TSA_PointerBox_ValueChanged;
            this.TSA_CheckBox.CheckedChanged          -= this.TSA_CheckBox_CheckedChanged;
            this.TSA_FlipRows_CheckBox.CheckedChanged -= this.TSA_FlipRows_CheckBox_CheckedChanged;

            this.Palette_PointerBox.Value = palette;
            this.Palette_CheckBox.Checked = palette_compressed;
            this.Tileset_PointerBox.Value = tileset;
            this.Tileset_CheckBox.Checked = tileset_compressed;
            this.TSA_PointerBox.Value = tsa;
            this.TSA_CheckBox.Checked = tsa_compressed;
            this.TSA_FlipRows_CheckBox.Checked = tsa_flipped;
            this.Palette_Offset_Label.Enabled = !this.Palette_CheckBox.Checked;
            this.Palette_Index_NumBox.Enabled = !this.Palette_CheckBox.Checked;

            this.Palette_PointerBox.ValueChanged      += this.Palette_PointerBox_ValueChanged;
            this.Palette_CheckBox.CheckedChanged      += this.Palette_CheckBox_CheckedChanged;
            this.Tileset_PointerBox.ValueChanged      += this.Tileset_PointerBox_ValueChanged;
            this.Tileset_CheckBox.CheckedChanged      += this.Tileset_CheckBox_CheckedChanged;
            this.TSA_PointerBox.ValueChanged          += this.TSA_PointerBox_ValueChanged;
            this.TSA_CheckBox.CheckedChanged          += this.TSA_CheckBox_CheckedChanged;
            this.TSA_FlipRows_CheckBox.CheckedChanged += this.TSA_FlipRows_CheckBox_CheckedChanged;

            this.TSA_Label_CheckedChanged(this, null);
        }
        public void Core_SetEntry(Int32 width, Int32 height,
            Pointer palette, Boolean palette_compressed,
            Pointer tileset, Boolean tileset_compressed,
            Pointer tsa = new Pointer(), Boolean tsa_compressed = false, Boolean tsa_flipped = false)
        {
            this.Width_NumBox.ValueChanged  -= this.Width_NumBox_ValueChanged;
            this.Height_NumBox.ValueChanged -= this.Height_NumBox_ValueChanged;

            this.Width_NumBox.Value = width;
            this.Height_NumBox.Value = height;

            this.Width_NumBox.ValueChanged  += this.Width_NumBox_ValueChanged;
            this.Height_NumBox.ValueChanged += this.Height_NumBox_ValueChanged;

            this.Core_SetEntry(
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
                Byte[] data_palette = palette.ToBytes(this.Palette_CheckBox.Checked);
                Byte[] data_tileset = this.Tileset_CheckBox.Checked ? LZ77.Compress(graphics) : graphics;
                Byte[] data_tsa = null;

                List<Tuple<String, Pointer, Int32>> repoints = new List<Tuple<String, Pointer, Int32>>();
                repoints.Add(Tuple.Create("Palette", this.Palette_PointerBox.Value, data_palette.Length));
                repoints.Add(Tuple.Create("Tileset", this.Tileset_PointerBox.Value, data_tileset.Length));
                if (tsa != null)
                {
                    data_tsa = tsa.ToBytes(this.TSA_CheckBox.Checked, this.TSA_FlipRows_CheckBox.Checked);
                    repoints.Add(Tuple.Create("TSA", this.TSA_PointerBox.Value, data_tsa.Length));
                }

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Graphics",
                    "The image and palette to insert might need to be repointed.",
                    "Image at " + this.Tileset_PointerBox.Value + " - ", repoints.ToArray());
                if (cancel) return;

                Core.WriteData(this,
                    this.Palette_PointerBox.Value,
                    data_palette,
                    "Palette at " + this.Palette_PointerBox.Value + " changed");

                Core.WriteData(this,
                    this.Tileset_PointerBox.Value,
                    data_tileset,
                    "Tileset at " + this.Tileset_PointerBox.Value + " changed");

                if (tsa != null)
                {
                    Core.WriteData(this,
                        this.TSA_PointerBox.Value,
                        data_tsa,
                        "TSA Array at " + this.TSA_PointerBox.Value + " changed");
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

                if (this.TSA_Label.Checked && this.TSA_PointerBox.Value != new Pointer())
                {
                    Int32 width = this.TSA_FlipRows_CheckBox.Checked ?
                        ((TSA_Image)this.Image_ImageBox.Display).Tiling.Width :
                        (Int32)this.Width_NumBox.Value;
                    Int32 height = this.TSA_FlipRows_CheckBox.Checked ?
                        ((TSA_Image)this.Image_ImageBox.Display).Tiling.Height :
                        (Int32)this.Height_NumBox.Value;

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
                else if (this.Tileset_8bpp_RadioButton.Checked)
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
                this.Core_Update();
                return;
            }

            if (image is GBA.TSA_Image)
            {
                this.Core_Insert(
                    Palette.Merge(((TSA_Image)image).Palettes),
                    ((TSA_Image)image).Graphics.ToBytes(false),
                    ((TSA_Image)image).Tiling);
                return;
            }
            if (image is GBA.Bitmap)
            {
                this.Core_Insert(
                    ((Bitmap)image).Colors,
                    ((Bitmap)image).ToBytes());
                return;
            }
            if (image is GBA.Image)
            {
                this.Core_Insert(
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
                        (Int32)this.Width_NumBox.Value,
                        (Int32)this.Height_NumBox.Value,
                        File.ReadAllBytes(path + file + ".tsa"));
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load image data files.", ex);
                return;
            }
            this.Core_Insert(palette, graphics, tsa);
        }
        void Core_SaveImage(String filepath)
        {
            Byte[] data = Core.ReadData(
                this.Palette_PointerBox.Value + (Int32)this.Palette_Index_NumBox.Value * Palette.LENGTH,
                this.Palette_CheckBox.Checked ? 0 : 16 * Palette.LENGTH);
            Palette[] palettes = new Palette[16];
            for (UInt32 i = 0; i < 16; i++)
            {
                palettes[i] = new Palette(data.GetBytes(i * Palette.LENGTH, Palette.LENGTH));
            }
            Core.SaveImage(filepath,
                this.Image_ImageBox.Display.Width,
                this.Image_ImageBox.Display.Height,
                palettes,
                delegate (Int32 x, Int32 y)
                {
                    Int32 palette = 0;
                    if (this.TSA_Label.Checked)
                        palette = ((TSA_Image)this.Image_ImageBox.Display).GetPaletteIndex(x, y);
                    return (Byte)palettes[palette].Find(this.Image_ImageBox.Display.GetColor(x, y));
                });
        }
        void Core_SaveData(String filepath)
        {
            String path = Path.GetDirectoryName(filepath) + '\\';
            String file = Path.GetFileNameWithoutExtension(filepath);
            try
            {
                IDisplayable image = this.Image_ImageBox.Display;

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
                "Image data " + (this.TSA_Label.Checked ?
                    "(.tsa + .chr + .pal)|*.tsa;*.chr;*.pal|" :
                    "(.chr + .pal)|*.chr;*.pal|") +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_InsertData(openWindow.FileName);
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
                "Image data " + (this.TSA_Label.Checked ?
                    "(.tsa + .chr + .pal)|*.tsa;*.chr;*.pal|" :
                    "(.chr + .pal)|*.chr;*.pal|") +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    saveWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase) ||
                    saveWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_SaveData(saveWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                "Unknown Palette - ",
                this.Palette_PointerBox.Value + (Int32)this.Palette_Index_NumBox.Value * Palette.LENGTH,
                this.Palette_CheckBox.Checked ? 0 : 1);
        }
        private void Tool_OpenTSAEditor_Click(Object sender, EventArgs e)
        {
            TSA_Array tsa = ((TSA_Image)this.Image_ImageBox.Display).Tiling;

            UI.OpenTSAEditor(this,
                "Unknown TSA Array - ",
                this.Palette_PointerBox.Value, this.Palette_CheckBox.Checked ? 0 : 16 * Palette.LENGTH,
                this.Tileset_PointerBox.Value, this.Tileset_CheckBox.Checked ? 0 :
                    (Int32)(this.Width_NumBox.Value * this.Height_NumBox.Value) * Tile.LENGTH,
                this.TSA_PointerBox.Value,
                tsa.Width, tsa.Height,
                this.TSA_CheckBox.Checked, this.TSA_FlipRows_CheckBox.Checked);
        }

        private void View_GrayscalePalette_Click(Object sender, EventArgs e)
        {
            this.Core_Update();
        }



        private void Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void Palette_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Palette_Offset_Label.Enabled = !this.Palette_CheckBox.Checked;
            this.Palette_Index_NumBox.Enabled = !this.Palette_CheckBox.Checked;

            this.Core_Update();
        }
        private void Tileset_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void Tileset_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void TSA_Label_CheckedChanged(Object sender, EventArgs e)
        {
            if (this.TSA_Label.Checked)
            {
                this.TSA_PointerBox.Enabled = true;
                this.TSA_CheckBox.Enabled = true;

                this.TSA_GroupBox.Enabled = true;

                this.Tileset_2bpp_RadioButton.Enabled = false;
                this.Tileset_4bpp_RadioButton.CheckedChanged -= this.Tileset_4bpp_RadioButton_CheckedChanged;
                this.Tileset_4bpp_RadioButton.Checked = true;
                this.Tileset_4bpp_RadioButton.CheckedChanged += this.Tileset_4bpp_RadioButton_CheckedChanged;
                this.Tileset_8bpp_RadioButton.Enabled = false;
            }
            else
            {
                this.TSA_PointerBox.Enabled = false;
                this.TSA_CheckBox.Enabled = false;

                this.Size_GroupBox.Enabled = true;
                this.TSA_FlipRows_CheckBox.CheckedChanged -= this.TSA_FlipRows_CheckBox_CheckedChanged;
                this.TSA_FlipRows_CheckBox.Checked = false;
                this.TSA_FlipRows_CheckBox.CheckedChanged += this.TSA_FlipRows_CheckBox_CheckedChanged;
                this.TSA_GroupBox.Enabled = false;

                this.Tileset_2bpp_RadioButton.Enabled = false;
                this.Tileset_8bpp_RadioButton.Enabled = true;
            }
            this.Core_Update();
        }
        private void TSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void TSA_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void Width_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void Height_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void Tileset_2bpp_RadioButton_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void Tileset_4bpp_RadioButton_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void Tileset_8bpp_RadioButton_CheckedChanged(Object sender, EventArgs e)
        {
            if (this.Tileset_8bpp_RadioButton.Checked)
            {
                this.TSA_Label.CheckedChanged -= this.TSA_Label_CheckedChanged;
                this.TSA_Label.Checked = false;
                this.TSA_Label.CheckedChanged += this.TSA_Label_CheckedChanged;
                this.TSA_Label.Enabled = false;
            }
            else
            {
                this.TSA_Label.Enabled = true;
            }
            this.Core_Update();
        }

        private void Palette_Index_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void Palette_Opaque_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void TSA_Dimensions_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void TSA_FlipRows_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            if (this.TSA_FlipRows_CheckBox.Checked)
            {
                this.Size_GroupBox.Enabled = false;
            }
            else
            {
                this.Size_GroupBox.Enabled = true;
            }
            this.Core_Update();
        }

        private void Prev_Palette_Button_Click(Object sender, EventArgs e)
        {
            this.Core_FindPrevLZ77Address(this.Palette_PointerBox, false);
        }
        private void Next_Palette_Button_Click(Object sender, EventArgs e)
        {
            this.Core_FindNextLZ77Address(this.Palette_PointerBox, false);
        }
        private void Prev_Tileset_Button_Click(Object sender, EventArgs e)
        {
            this.Core_FindPrevLZ77Address(this.Tileset_PointerBox, true);
        }
        private void Next_Tileset_Button_Click(Object sender, EventArgs e)
        {
            this.Core_FindNextLZ77Address(this.Tileset_PointerBox, true);
        }
        private void Prev_TSA_Button_Click(Object sender, EventArgs e)
        {
            this.Core_FindPrevLZ77Address(this.TSA_PointerBox, false);
        }
        private void Next_TSA_Button_Click(Object sender, EventArgs e)
        {
            this.Core_FindNextLZ77Address(this.TSA_PointerBox, false);
        }
    }
}
