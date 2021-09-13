using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GBA;
using Magic.Components;

namespace Magic.Editors
{
    public partial class TSAEditor : Editor
    {
        /// <summary>
        /// Link to the parent Editor
        /// </summary>
        new Editor Owner { get; }
        /// <summary>
        /// The text for the description when writing
        /// </summary>
        String Entry { get; }

        Pointer PaletteAddress { get; }
        Int32 PaletteLength { get; }
        Pointer TilesetAddress { get; }
        Int32 TilesetLength { get; }
        /// <summary>
        /// The address at which the TSA array is located
        /// </summary>
        Pointer Address { get; }
        /// <summary>
        /// Whether or not the TSA data to edit is stored LZ77 compressed
        /// </summary>
        Boolean IsCompressed { get; }
        /// <summary>
        /// Whether or not to flip the rows of this TSA before displaying
        /// </summary>
        Boolean FlipRows { get; }

        /// <summary>
        /// The TSA array this TSA editor is to edit
        /// </summary>
        public TSA_Array Current { get; set; }

        /// <summary>
        /// The image to display on behind the grid for this TSAEditor
        /// </summary>
        IDisplayable Display { get; set; }



        public TSAEditor(Editor owner,
            String entry,
            Pointer palette_address, Int32 palette_length,
            Pointer tileset_address, Int32 tileset_length,
            Pointer address,
            Int32 width, Int32 height,
            Boolean compressed,
            Boolean flipRows)
        {
            try
            {
                this.InitializeComponent();

                base.Owner = owner;
                this.Owner = owner;
                this.Entry = entry;

                this.PaletteAddress = palette_address;
                this.PaletteLength  = palette_length;
                this.TilesetAddress = tileset_address;
                this.TilesetLength  = tileset_length;

                this.Address = address;
                this.IsCompressed = compressed;
                this.FlipRows = flipRows;

                this.Current = new TSA_Array(width, height);

                this.MenuBar.LayoutStyle = ToolStripLayoutStyle.Flow;
                this.EntryInfo_Label.Text = entry + address;

                this.TSA_GridBox.Size = new Size(width * 8, height * 8);
                this.Size = new Size(40 + width * 8 + 4, 210 + height * 8 + 4);
                this.MaximumSize = this.Size;
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
            this.Current = Core.ReadTSA(this.Address, this.Current.Width, this.Current.Height, this.IsCompressed, this.FlipRows);

            this.Core_UpdateDisplay();

            this.Core_LoadValues();
        }

        public void Core_UpdateDisplay()
        {
            try
            {
                Byte[] palette = Core.ReadData(this.PaletteAddress, this.PaletteLength);
                Byte[] tileset;
                if (this.TilesetLength == -1)
                {
                    Int32 amount = 10;
                    Int32 length = 32 * 2 * Tile.LENGTH;
                    tileset = new Byte[amount * length];
                    Pointer address;
                    Byte[] buffer;
                    for (Int32 i = 0; i < amount; i++)
                    {
                        address = Core.ReadPointer(this.TilesetAddress + i * 4);
                        buffer = Core.ReadData(address, 0);
                        Array.Copy(buffer, 0, tileset, i * length, length);
                    }   // assemble BGs that are stored in 32x2 strips
                }
                else tileset = Core.ReadData(this.TilesetAddress, this.TilesetLength);

                this.TSA_GridBox.Load(new TSA_Image(palette, tileset, this.Current));
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the image.", ex);

                this.TSA_GridBox.Reset();
            }
        }

        void Core_LoadValues()
        {
            this.TileIndex_NumBox.ValueChanged -= this.TileIndex_NumBox_ValueChanged;
            this.Palette_NumBox.ValueChanged -= this.Palette_NumBox_ValueChanged;
            this.FlipH_CheckBox.CheckedChanged -= this.FlipH_CheckBox_CheckedChanged;
            this.FlipV_CheckBox.CheckedChanged -= this.FlipV_CheckBox_CheckedChanged;
            
            if (this.TSA_GridBox.SelectionIsEmpty())
            {
                this.TileIndex_NumBox.Value = 0;
                this.Palette_NumBox.Value = 0;

                this.TileIndex_NumBox.Text = this.TileIndex_NumBox.Value.ToString();
                this.Palette_NumBox.Text = this.Palette_NumBox.Value.ToString();

                this.TileIndex_NumBox.Enabled = false;
                this.Palette_NumBox.Enabled = false;
                this.FlipH_CheckBox.Enabled = false;
                this.FlipV_CheckBox.Enabled = false;

                this.Coordinates_Label.Text = "X : None, Y : None";
            }
            else
            {
                this.TileIndex_NumBox.Enabled = true;
                this.Palette_NumBox.Enabled = true;
                this.FlipH_CheckBox.Enabled = true;
                this.FlipV_CheckBox.Enabled = true;

                if (this.TSA_GridBox.SelectionIsSingle())
                {
                    Point selection = this.TSA_GridBox.GetSelectionCoords();

                    this.TileIndex_NumBox.Value = this.Current[selection.X, selection.Y].TileIndex;
                    this.Palette_NumBox.Value   = this.Current[selection.X, selection.Y].Palette;
                    this.FlipH_CheckBox.Checked = this.Current[selection.X, selection.Y].FlipH;
                    this.FlipV_CheckBox.Checked = this.Current[selection.X, selection.Y].FlipV;

                    this.TileIndex_NumBox.Text = this.TileIndex_NumBox.Value.ToString();
                    this.Palette_NumBox.Text = this.Palette_NumBox.Value.ToString();

                    this.CurrentValue_Label.Text = "TSA Value : 0x" + Util.UInt16ToHex(this.Current[selection.X, selection.Y].Value);
                    this.Coordinates_Label.Text = "X : " + selection.X + ", Y : " + selection.Y;
                }
                else
                {
                    this.TileIndex_NumBox.Text = "";
                    this.Palette_NumBox.Text = "";
                    this.FlipH_CheckBox.Checked = false;
                    this.FlipV_CheckBox.Checked = false;

                    this.CurrentValue_Label.Text = "TSA Value : [mixed]";
                    this.Coordinates_Label.Text = this.TSA_GridBox.GetSelectionAmount() + " selected tiles";
                }
            }

            this.TileIndex_NumBox.ValueChanged += this.TileIndex_NumBox_ValueChanged;
            this.Palette_NumBox.ValueChanged += this.Palette_NumBox_ValueChanged;
            this.FlipH_CheckBox.CheckedChanged += this.FlipH_CheckBox_CheckedChanged;
            this.FlipV_CheckBox.CheckedChanged += this.FlipV_CheckBox_CheckedChanged;
        }
        void Core_WriteTSA()
        {
            Boolean[,] selection = this.TSA_GridBox.Selection;

            Core.WriteData(
                this.Owner ?? this,
                this.Address,
                this.Current.ToBytes(this.IsCompressed, this.FlipRows),
                this.Entry + "TSA changed");

            this.TSA_GridBox.Selection = selection;
        }



        private void LoadButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "TSA data (*.tsa)|*.tsa|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                this.Current = new TSA_Array(this.Current.Width, this.Current.Height, File.ReadAllBytes(openWindow.FileName));

                Core.WriteData(this,
                    this.Address,
                    this.Current.ToBytes(this.IsCompressed, this.FlipRows),
                    this.Entry + "TSA changed");
            }
        }
        private void SaveButton_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.Filter = "TSA data (*.tsa)|*.tsa|All files (*.*)|*.*";
            saveWindow.FilterIndex = 1;
            saveWindow.RestoreDirectory = true;
            saveWindow.CreatePrompt = true;
            saveWindow.OverwritePrompt = true;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveWindow.FileName, this.Current.ToBytes(false, false));
            }
        }

        private void TSA_GridBox_SelectionChanged(Object sender, EventArgs e)
        {
            this.Core_LoadValues();
        }
        private void ViewGrid_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.TSA_GridBox.ShowGrid = this.ViewGrid_CheckBox.Checked;
        }

        private void TileIndex_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 width = this.TSA_GridBox.Selection.GetLength(0);
            Int32 height = this.TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.TSA_GridBox.Selection[x, y])
                {
                    tsa = this.Current[x, y];
                        this.Current[x, y] = new TSA(
                        (UInt16)this.TileIndex_NumBox.Value,
                        tsa.Palette,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            this.Core_WriteTSA();
        }
        private void Palette_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 width = this.TSA_GridBox.Selection.GetLength(0);
            Int32 height = this.TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.TSA_GridBox.Selection[x, y])
                {
                    tsa = this.Current[x, y];
                        this.Current[x, y] = new TSA(
                        tsa.TileIndex,
                        (Byte)this.Palette_NumBox.Value,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            this.Core_WriteTSA();
        }
        private void FlipH_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Int32 width = this.TSA_GridBox.Selection.GetLength(0);
            Int32 height = this.TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.TSA_GridBox.Selection[x, y])
                {
                    tsa = this.Current[x, y];
                        this.Current[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        this.FlipH_CheckBox.Checked,
                        tsa.FlipV);
                }
            }
            this.Core_WriteTSA();
        }
        private void FlipV_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Int32 width = this.TSA_GridBox.Selection.GetLength(0);
            Int32 height = this.TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.TSA_GridBox.Selection[x, y])
                {
                    tsa = this.Current[x, y];
                        this.Current[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        tsa.FlipH,
                        this.FlipV_CheckBox.Checked);
                }
            }
            this.Core_WriteTSA();
        }
    }
}
