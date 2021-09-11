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
                InitializeComponent();

                base.Owner = owner;
                Owner = owner;
                Entry = entry;

                PaletteAddress = palette_address;
                PaletteLength  = palette_length;
                TilesetAddress = tileset_address;
                TilesetLength  = tileset_length;

                Address = address;
                IsCompressed = compressed;
                FlipRows = flipRows;

                Current = new TSA_Array(width, height);

                MenuBar.LayoutStyle = ToolStripLayoutStyle.Flow;
                EntryInfo_Label.Text = entry + address;

                TSA_GridBox.Size = new Size(width * 8, height * 8);
                this.Size = new Size(40 + width * 8 + 4, 210 + height * 8 + 4);
                this.MaximumSize = this.Size;
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
            Current = Core.ReadTSA(Address, Current.Width, Current.Height, IsCompressed, FlipRows);

            Core_UpdateDisplay();

            Core_LoadValues();
        }

        public void Core_UpdateDisplay()
        {
            try
            {
                Byte[] palette = Core.ReadData(PaletteAddress, PaletteLength);
                Byte[] tileset;
                if (TilesetLength == -1)
                {
                    Int32 amount = 10;
                    Int32 length = 32 * 2 * Tile.LENGTH;
                    tileset = new Byte[amount * length];
                    Pointer address;
                    Byte[] buffer;
                    for (Int32 i = 0; i < amount; i++)
                    {
                        address = Core.ReadPointer(TilesetAddress + i * 4);
                        buffer = Core.ReadData(address, 0);
                        Array.Copy(buffer, 0, tileset, i * length, length);
                    }   // assemble BGs that are stored in 32x2 strips
                }
                else tileset = Core.ReadData(TilesetAddress, TilesetLength);

                TSA_GridBox.Load(new TSA_Image(palette, tileset, Current));
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the image.", ex);

                TSA_GridBox.Reset();
            }
        }

        void Core_LoadValues()
        {
            TileIndex_NumBox.ValueChanged -= TileIndex_NumBox_ValueChanged;
            Palette_NumBox.ValueChanged -= Palette_NumBox_ValueChanged;
            FlipH_CheckBox.CheckedChanged -= FlipH_CheckBox_CheckedChanged;
            FlipV_CheckBox.CheckedChanged -= FlipV_CheckBox_CheckedChanged;
            
            if (TSA_GridBox.SelectionIsEmpty())
            {
                TileIndex_NumBox.Value = 0;
                Palette_NumBox.Value = 0;

                TileIndex_NumBox.Text = TileIndex_NumBox.Value.ToString();
                Palette_NumBox.Text = Palette_NumBox.Value.ToString();

                TileIndex_NumBox.Enabled = false;
                Palette_NumBox.Enabled = false;
                FlipH_CheckBox.Enabled = false;
                FlipV_CheckBox.Enabled = false;

                Coordinates_Label.Text = "X : None, Y : None";
            }
            else
            {
                TileIndex_NumBox.Enabled = true;
                Palette_NumBox.Enabled = true;
                FlipH_CheckBox.Enabled = true;
                FlipV_CheckBox.Enabled = true;

                if (TSA_GridBox.SelectionIsSingle())
                {
                    Point selection = TSA_GridBox.GetSelectionCoords();

                    TileIndex_NumBox.Value = Current[selection.X, selection.Y].TileIndex;
                    Palette_NumBox.Value   = Current[selection.X, selection.Y].Palette;
                    FlipH_CheckBox.Checked = Current[selection.X, selection.Y].FlipH;
                    FlipV_CheckBox.Checked = Current[selection.X, selection.Y].FlipV;

                    TileIndex_NumBox.Text = TileIndex_NumBox.Value.ToString();
                    Palette_NumBox.Text = Palette_NumBox.Value.ToString();

                    CurrentValue_Label.Text = "TSA Value : 0x" + Util.UInt16ToHex(Current[selection.X, selection.Y].Value);
                    Coordinates_Label.Text = "X : " + selection.X + ", Y : " + selection.Y;
                }
                else
                {
                    TileIndex_NumBox.Text = "";
                    Palette_NumBox.Text = "";
                    FlipH_CheckBox.Checked = false;
                    FlipV_CheckBox.Checked = false;

                    CurrentValue_Label.Text = "TSA Value : [mixed]";
                    Coordinates_Label.Text = TSA_GridBox.GetSelectionAmount() + " selected tiles";
                }
            }

            TileIndex_NumBox.ValueChanged += TileIndex_NumBox_ValueChanged;
            Palette_NumBox.ValueChanged += Palette_NumBox_ValueChanged;
            FlipH_CheckBox.CheckedChanged += FlipH_CheckBox_CheckedChanged;
            FlipV_CheckBox.CheckedChanged += FlipV_CheckBox_CheckedChanged;
        }
        void Core_WriteTSA()
        {
            Boolean[,] selection = TSA_GridBox.Selection;

            Core.WriteData(
                Owner ?? this,
                Address,
                Current.ToBytes(IsCompressed, FlipRows),
                Entry + "TSA changed");

            TSA_GridBox.Selection = selection;
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
                Current = new TSA_Array(Current.Width, Current.Height, File.ReadAllBytes(openWindow.FileName));

                Core.WriteData(this,
                    Address,
                    Current.ToBytes(IsCompressed, FlipRows),
                    Entry + "TSA changed");
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
                File.WriteAllBytes(saveWindow.FileName, Current.ToBytes(false, false));
            }
        }

        private void TSA_GridBox_SelectionChanged(Object sender, EventArgs e)
        {
            Core_LoadValues();
        }
        private void ViewGrid_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            TSA_GridBox.ShowGrid = ViewGrid_CheckBox.Checked;
        }

        private void TileIndex_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 width = TSA_GridBox.Selection.GetLength(0);
            Int32 height = TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (TSA_GridBox.Selection[x, y])
                {
                    tsa = Current[x, y];
                    Current[x, y] = new TSA(
                        (UInt16)TileIndex_NumBox.Value,
                        tsa.Palette,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            Core_WriteTSA();
        }
        private void Palette_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Int32 width = TSA_GridBox.Selection.GetLength(0);
            Int32 height = TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (TSA_GridBox.Selection[x, y])
                {
                    tsa = Current[x, y];
                    Current[x, y] = new TSA(
                        tsa.TileIndex,
                        (Byte)Palette_NumBox.Value,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
            Core_WriteTSA();
        }
        private void FlipH_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Int32 width = TSA_GridBox.Selection.GetLength(0);
            Int32 height = TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (TSA_GridBox.Selection[x, y])
                {
                    tsa = Current[x, y];
                    Current[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        FlipH_CheckBox.Checked,
                        tsa.FlipV);
                }
            }
            Core_WriteTSA();
        }
        private void FlipV_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Int32 width = TSA_GridBox.Selection.GetLength(0);
            Int32 height = TSA_GridBox.Selection.GetLength(1);
            TSA tsa;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (TSA_GridBox.Selection[x, y])
                {
                    tsa = Current[x, y];
                    Current[x, y] = new TSA(
                        tsa.TileIndex,
                        tsa.Palette,
                        tsa.FlipH,
                        FlipV_CheckBox.Checked);
                }
            }
            Core_WriteTSA();
        }
    }
}
