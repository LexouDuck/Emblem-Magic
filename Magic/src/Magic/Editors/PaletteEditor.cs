using System;
using System.IO;
using System.Windows.Forms;
using GBA;
using Magic.Components;

namespace Magic.Editors
{
    public partial class PaletteEditor : Editor
    {
        /// <summary>
        /// The editor that instantiated this PaletteEditor
        /// </summary>
        new Editor Owner { get; }
        /// <summary>
        /// The text for the description when writing
        /// </summary>
        String Entry { get; }
        /// <summary>
        /// The address at which the palette to be edited is located
        /// </summary>
        Pointer Address { get; }
        /// <summary>
        /// The amount of sequential palettes to be loaded/edited
        /// </summary>
        Int32 PaletteAmount { get; }
        /// <summary>
        /// Whether or not the palette to edit is compressed.
        /// </summary>
        Boolean IsCompressed { get; }


        /// <summary>
        /// An array containing the 16 ColorBoxes of the PaletteEditor
        /// </summary>
        ColorBox[] ColorBoxes { get; }
        
        /// <summary>
        /// The current GBA.Palette
        /// </summary>
        Palette Current { get; set; }

        /// <summary>
        /// The index of the currently selected color in the palette
        /// </summary>
        Int32 CurrentIndex
        {
            get
            {
                return this._index;
            }
            set
            {
                if (this.SwapMode)
                {
                    this.Core_SwapColors(this._index, value);
                    this.SwapMode = false;
                }
                this.SwapButton.Enabled = true;
                this.LoadButton.Enabled = true;
                this.SaveButton.Enabled = true;

                this.ColorBoxes[this._index].Selected = false;
                this.ColorBoxes[value].Selected = true;

                this._index = value;

                this.Core_LoadValues();
            }
        }
        Int32 _index;
        /// <summary>
        /// The address at which the currently selected color is located
        /// </summary>
        Pointer CurrentAddress
        {
            get
            {
                return this.Address + this.CurrentIndex * 2;
            }
        }
        /// <summary>
        /// Is true if "swap colors" has been pressed
        /// </summary>
        Boolean SwapMode
        {
            get
            {
                return this._swap;
            }
            set
            {
                if (value)
                {
                    this.SwapButton.Enabled = false;
                    this.LoadButton.Enabled = false;
                    this.SaveButton.Enabled = false;
                    this.AlphaCheckBox.Enabled = false;
                    this.NumBox_16bit.Enabled = false;
                    this.NumBox_32bit.Enabled = false;
                    this.TrackBar_R.Enabled = false;
                    this.TrackBar_G.Enabled = false;
                    this.TrackBar_B.Enabled = false;
                    this.NumBox_R.Enabled = false;
                    this.NumBox_G.Enabled = false;
                    this.NumBox_B.Enabled = false;
                }
                else
                {
                    this.SwapButton.Enabled = true;
                    this.SwapButton.Text = "Swap Colors";
                    this.LoadButton.Enabled = true;
                    this.LoadButton.Text = "Swap and Recolor";
                    this.SaveButton.Enabled = true;
                    this.AlphaCheckBox.Enabled = true;
                    this.NumBox_16bit.Enabled = true;
                    this.NumBox_32bit.Enabled = true;
                    this.TrackBar_R.Enabled = true;
                    this.TrackBar_G.Enabled = true;
                    this.TrackBar_B.Enabled = true;
                    this.NumBox_R.Enabled = true;
                    this.NumBox_G.Enabled = true;
                    this.NumBox_B.Enabled = true;
                }
                this._swap = value;
            }
        }
        Boolean _swap;



        public PaletteEditor(Editor owner,
            String entry,
            Pointer address,
            Byte amount)
        {
            this.InitializeComponent();

            this.Status.Text = entry + address;

            this.Owner = owner;
            this.Entry = entry;
            this.Address = address;
            this.IsCompressed = (amount == 0);
            this.Current = Core.ReadPalette(address, amount * Palette.LENGTH);
            if (this.IsCompressed)
            {
                amount = (Byte)(this.Current.Count / Palette.MAX);
            }
            this.PaletteAmount = amount;

            this.Size = new System.Drawing.Size(390, 250 + 22 * amount);
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.SwapButton.Location = new System.Drawing.Point( 13, 170 + 22 * amount);
            this.LoadButton.Location = new System.Drawing.Point(131, 170 + 22 * amount);
            this.SaveButton.Location = new System.Drawing.Point(248, 170 + 22 * amount);

            this.ColorBoxes = new ColorBox[GBA.Palette.MAX * amount];
            for (Int32 i = 0; i < this.ColorBoxes.Length; i++)
            {
                this.ColorBoxes[i] = new ColorBox();
                this.ColorBoxes[i].Gradient = false;
                this.ColorBoxes[i].Selected = false;
                this.ColorBoxes[i].Location = new System.Drawing.Point(
                    13 + 22 * (i % GBA.Palette.MAX),
                    169 + 22 * (i / GBA.Palette.MAX));
                this.ColorBoxes[i].Size = new System.Drawing.Size(16, 16);
                this.ColorBoxes[i].Name = "ColorBox" + i;
                this.ColorBoxes[i].TabIndex = 8;
                this.ColorBoxes[i].TabStop = false;

                Int32 index = i;
                this.ColorBoxes[i].Click += delegate (Object sender, EventArgs e)
                {
                    if (this.CurrentIndex == index)
                    {
                        ColorDialog colorWindow = new ColorDialog();
                        colorWindow.FullOpen = true;
                        colorWindow.Color = (System.Drawing.Color)this.Current[this.CurrentIndex];

                        if (colorWindow.ShowDialog(this) == DialogResult.OK)
                        {
                            this.Core_WriteColor((GBA.Color)colorWindow.Color);
                        }
                    }
                    else this.CurrentIndex = index;
                };

                this.Controls.Add(this.ColorBoxes[i]);
            }
            this.ColorBoxes[0].Selected = true;
        }

        override public void Core_OnOpen()
        {
            this.Core_Update();
        }
        override public void Core_Update()
        {
            this.Current = Core.ReadPalette(this.Address, this.IsCompressed ? 0 : this.PaletteAmount * GBA.Palette.LENGTH);

            for (Int32 i = 0; i < this.ColorBoxes.Length; i++)
            {
                this.ColorBoxes[i].Color = (System.Drawing.Color)this.Current[i];
            }

            this.Core_LoadValues();
        }

        public void Core_LoadValues()
        {
            this.AlphaCheckBox.CheckedChanged -= this.AlphaCheckBox_CheckedChanged;
            this.NumBox_16bit.ValueChanged -= this.NumBox_16bit_ValueChanged;
            this.NumBox_32bit.ValueChanged -= this.NumBox_32bit_ValueChanged;
            this.NumBox_R.ValueChanged -= this.NumBox_R_ValueChanged;
            this.NumBox_G.ValueChanged -= this.NumBox_G_ValueChanged;
            this.NumBox_B.ValueChanged -= this.NumBox_B_ValueChanged;
            this.TrackBar_R.Scroll -= this.TrackBar_R_Scroll;
            this.TrackBar_G.Scroll -= this.TrackBar_G_Scroll;
            this.TrackBar_B.Scroll -= this.TrackBar_B_Scroll;

            if (this.Current == null || this.CurrentIndex >= this.Current.Count)
            {
                this.AlphaCheckBox.Checked = false;
                this.NumBox_16bit.Value = 0;
                this.NumBox_32bit.Value = 0;
                this.TrackBar_R.Value = 0;
                this.TrackBar_G.Value = 0;
                this.TrackBar_B.Value = 0;
                this.NumBox_R.Value = 0;
                this.NumBox_G.Value = 0;
                this.NumBox_B.Value = 0;
            }
            else
            {
                GBA.Color color = this.Current[this.CurrentIndex];

                this.AlphaCheckBox.Checked = color.GetAlpha();
                this.NumBox_16bit.Value = color.To16bit();
                this.NumBox_32bit.Value = color.To32bit();
                this.TrackBar_R.Value = color.GetValueR();
                this.TrackBar_G.Value = color.GetValueG();
                this.TrackBar_B.Value = color.GetValueB();
                this.NumBox_R.Value = this.TrackBar_R.Value;
                this.NumBox_G.Value = this.TrackBar_G.Value;
                this.NumBox_B.Value = this.TrackBar_B.Value;
            }

            this.AlphaCheckBox.CheckedChanged += this.AlphaCheckBox_CheckedChanged;
            this.NumBox_16bit.ValueChanged += this.NumBox_16bit_ValueChanged;
            this.NumBox_32bit.ValueChanged += this.NumBox_32bit_ValueChanged;
            this.NumBox_R.ValueChanged += this.NumBox_R_ValueChanged;
            this.NumBox_G.ValueChanged += this.NumBox_G_ValueChanged;
            this.NumBox_B.ValueChanged += this.NumBox_B_ValueChanged;
            this.TrackBar_R.Scroll += this.TrackBar_R_Scroll;
            this.TrackBar_G.Scroll += this.TrackBar_G_Scroll;
            this.TrackBar_B.Scroll += this.TrackBar_B_Scroll;
        }

        public void Core_Insert(String filepath)
        {
            try
            {
                Palette palette = new Palette(filepath);

                Core.WriteData(
                    this.Owner ?? this,
                    this.Address,
                    palette.ToBytes(this.IsCompressed),
                    this.Entry + "Palette changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load palette from the file:\n" + filepath, ex);
            }
        }

        public void Core_WriteColor(Color color)
        {
            if (this.IsCompressed)
            {
                this.Current[this.CurrentIndex] = color;

                Core.WriteData(this.Owner ?? this,
                    this.Address,
                    this.Current.ToBytes(true),
                    this.Entry + "Palette changed");
            }
            else
            {
                Core.WriteData(this.Owner ?? this,
                    this.CurrentAddress,
                    color.ToBytes(true),
                    this.Entry + "Palette color " + this.CurrentIndex + " changed");
            }
        }
        public void Core_SwapColors(Int32 index1, Int32 index2)
        {
            if (index1 < 0 || index1 >= this.PaletteAmount * 16
             || index2 < 0 || index2 >= this.PaletteAmount * 16)
                throw new Exception("Invalid index given.");

            GBA.Color color1 = this.Current[index1];
            GBA.Color color2 = this.Current[index2];

            UI.SuspendUpdate();

            if (this.IsCompressed)
            {
                this.Current.Swap(index1, index2);

                Core.WriteData(this.Owner ?? this,
                    this.Address,
                    this.Current.ToBytes(true),
                    this.Entry + "Palette changed");
            }
            else
            {
                Core.WriteData(this.Owner ?? this,
                    this.Address + index2 * 2,
                    color1.ToBytes(true),
                    this.Entry + "Palette color " + index1 + " swapped");

                Core.WriteData(this.Owner ?? this,
                    this.Address + index1 * 2,
                    color2.ToBytes(true),
                    this.Entry + "Palette color " + index2 + " swapped");
            }

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }



        private void SwapButton_Click(Object sender, EventArgs e)
        {
            this.SwapMode = true;
            this.SwapButton.Text = "Cancel";
            this.SwapButton.Enabled = true;
        }
        private void LoadButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "16-bit Palette file (*.pal)|*.pal|" +
                "Image file (*.png,*.bmp,*.gif)|*.png;*.bmp;*.gif|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_Insert(openWindow.FileName);
            }
        }
        private void SaveButton_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "16-bit Palette file (*.pal)|*.pal|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveWindow.FileName, this.Current.ToBytes(false));
            }
        }

        private void AlphaCheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_WriteColor(this.Current[this.CurrentIndex].SetAlpha(this.AlphaCheckBox.Checked));
        }

        private void NumBox_16bit_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_WriteColor(new GBA.Color((UInt16)this.NumBox_16bit.Value));
        }
        private void NumBox_32bit_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_WriteColor(new GBA.Color((UInt32)this.NumBox_32bit.Value));
        }

        private void NumBox_R_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_WriteColor(this.Current[this.CurrentIndex].SetValueR((Byte)this.NumBox_R.Value));
        }
        private void NumBox_G_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_WriteColor(this.Current[this.CurrentIndex].SetValueG((Byte)this.NumBox_G.Value));
        }
        private void NumBox_B_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_WriteColor(this.Current[this.CurrentIndex].SetValueB((Byte)this.NumBox_B.Value));
        }

        private void TrackBar_R_Scroll(Object sender, EventArgs e)
        {
            this.NumBox_R.Value = this.TrackBar_R.Value;
        }
        private void TrackBar_G_Scroll(Object sender, EventArgs e)
        {
            this.NumBox_G.Value = this.TrackBar_G.Value;
        }
        private void TrackBar_B_Scroll(Object sender, EventArgs e)
        {
            this.NumBox_B.Value = this.TrackBar_B.Value;
        }
    }
}
