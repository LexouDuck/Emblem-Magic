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
                return _index;
            }
            set
            {
                if (SwapMode)
                {
                    Core_SwapColors(_index, value);
                    SwapMode = false;
                }
                SwapButton.Enabled = true;
                LoadButton.Enabled = true;
                SaveButton.Enabled = true;

                ColorBoxes[_index].Selected = false;
                ColorBoxes[value].Selected = true;

                _index = value;

                Core_LoadValues();
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
                return Address + CurrentIndex * 2;
            }
        }
        /// <summary>
        /// Is true if "swap colors" has been pressed
        /// </summary>
        Boolean SwapMode
        {
            get
            {
                return _swap;
            }
            set
            {
                if (value)
                {
                    SwapButton.Enabled = false;
                    LoadButton.Enabled = false;
                    SaveButton.Enabled = false;
                    AlphaCheckBox.Enabled = false;
                    NumBox_16bit.Enabled = false;
                    NumBox_32bit.Enabled = false;
                    TrackBar_R.Enabled = false;
                    TrackBar_G.Enabled = false;
                    TrackBar_B.Enabled = false;
                    NumBox_R.Enabled = false;
                    NumBox_G.Enabled = false;
                    NumBox_B.Enabled = false;
                }
                else
                {
                    SwapButton.Enabled = true;
                    SwapButton.Text = "Swap Colors";
                    LoadButton.Enabled = true;
                    LoadButton.Text = "Swap and Recolor";
                    SaveButton.Enabled = true;
                    AlphaCheckBox.Enabled = true;
                    NumBox_16bit.Enabled = true;
                    NumBox_32bit.Enabled = true;
                    TrackBar_R.Enabled = true;
                    TrackBar_G.Enabled = true;
                    TrackBar_B.Enabled = true;
                    NumBox_R.Enabled = true;
                    NumBox_G.Enabled = true;
                    NumBox_B.Enabled = true;
                }
                _swap = value;
            }
        }
        Boolean _swap;



        public PaletteEditor(Editor owner,
            String entry,
            Pointer address,
            Byte amount)
        {
            InitializeComponent();

            Status.Text = entry + address;

            Owner = owner;
            Entry = entry;
            Address = address;
            IsCompressed = (amount == 0);
            Current = Core.ReadPalette(address, amount * Palette.LENGTH);
            if (IsCompressed)
            {
                amount = (Byte)(Current.Count / Palette.MAX);
            }
            PaletteAmount = amount;

            this.Size = new System.Drawing.Size(390, 250 + 22 * amount);
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            SwapButton.Location = new System.Drawing.Point( 13, 170 + 22 * amount);
            LoadButton.Location = new System.Drawing.Point(131, 170 + 22 * amount);
            SaveButton.Location = new System.Drawing.Point(248, 170 + 22 * amount);
            
            ColorBoxes = new ColorBox[GBA.Palette.MAX * amount];
            for (Int32 i = 0; i < ColorBoxes.Length; i++)
            {
                ColorBoxes[i] = new ColorBox();
                ColorBoxes[i].Gradient = false;
                ColorBoxes[i].Selected = false;
                ColorBoxes[i].Location = new System.Drawing.Point(
                    13 + 22 * (i % GBA.Palette.MAX),
                    169 + 22 * (i / GBA.Palette.MAX));
                ColorBoxes[i].Size = new System.Drawing.Size(16, 16);
                ColorBoxes[i].Name = "ColorBox" + i;
                ColorBoxes[i].TabIndex = 8;
                ColorBoxes[i].TabStop = false;

                Int32 index = i;
                ColorBoxes[i].Click += delegate (Object sender, EventArgs e)
                {
                    if (CurrentIndex == index)
                    {
                        ColorDialog colorWindow = new ColorDialog();
                        colorWindow.FullOpen = true;
                        colorWindow.Color = (System.Drawing.Color)Current[CurrentIndex];

                        if (colorWindow.ShowDialog(this) == DialogResult.OK)
                        {
                            Core_WriteColor((GBA.Color)colorWindow.Color);
                        }
                    }
                    else CurrentIndex = index;
                };

                this.Controls.Add(ColorBoxes[i]);
            }
            ColorBoxes[0].Selected = true;
        }

        override public void Core_OnOpen()
        {
            Core_Update();
        }
        override public void Core_Update()
        {
            Current = Core.ReadPalette(Address, IsCompressed ? 0 : PaletteAmount * GBA.Palette.LENGTH);

            for (Int32 i = 0; i < ColorBoxes.Length; i++)
            {
                ColorBoxes[i].Color = (System.Drawing.Color)Current[i];
            }

            Core_LoadValues();
        }

        public void Core_LoadValues()
        {
            AlphaCheckBox.CheckedChanged -= AlphaCheckBox_CheckedChanged;
            NumBox_16bit.ValueChanged -= NumBox_16bit_ValueChanged;
            NumBox_32bit.ValueChanged -= NumBox_32bit_ValueChanged;
            NumBox_R.ValueChanged -= NumBox_R_ValueChanged;
            NumBox_G.ValueChanged -= NumBox_G_ValueChanged;
            NumBox_B.ValueChanged -= NumBox_B_ValueChanged;
            TrackBar_R.Scroll -= TrackBar_R_Scroll;
            TrackBar_G.Scroll -= TrackBar_G_Scroll;
            TrackBar_B.Scroll -= TrackBar_B_Scroll;

            if (Current == null || CurrentIndex >= Current.Count)
            {
                AlphaCheckBox.Checked = false;
                NumBox_16bit.Value = 0;
                NumBox_32bit.Value = 0;
                TrackBar_R.Value = 0;
                TrackBar_G.Value = 0;
                TrackBar_B.Value = 0;
                NumBox_R.Value = 0;
                NumBox_G.Value = 0;
                NumBox_B.Value = 0;
            }
            else
            {
                GBA.Color color = Current[CurrentIndex];

                AlphaCheckBox.Checked = color.GetAlpha();
                NumBox_16bit.Value = color.To16bit();
                NumBox_32bit.Value = color.To32bit();
                TrackBar_R.Value = color.GetValueR();
                TrackBar_G.Value = color.GetValueG();
                TrackBar_B.Value = color.GetValueB();
                NumBox_R.Value = TrackBar_R.Value;
                NumBox_G.Value = TrackBar_G.Value;
                NumBox_B.Value = TrackBar_B.Value;
            }

            AlphaCheckBox.CheckedChanged += AlphaCheckBox_CheckedChanged;
            NumBox_16bit.ValueChanged += NumBox_16bit_ValueChanged;
            NumBox_32bit.ValueChanged += NumBox_32bit_ValueChanged;
            NumBox_R.ValueChanged += NumBox_R_ValueChanged;
            NumBox_G.ValueChanged += NumBox_G_ValueChanged;
            NumBox_B.ValueChanged += NumBox_B_ValueChanged;
            TrackBar_R.Scroll += TrackBar_R_Scroll;
            TrackBar_G.Scroll += TrackBar_G_Scroll;
            TrackBar_B.Scroll += TrackBar_B_Scroll;
        }

        public void Core_Insert(String filepath)
        {
            try
            {
                Palette palette = new Palette(filepath);

                Core.WriteData(
                    Owner ?? this,
                    Address,
                    palette.ToBytes(IsCompressed),
                    Entry + "Palette changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load palette from the file:\n" + filepath, ex);
            }
        }

        public void Core_WriteColor(Color color)
        {
            if (IsCompressed)
            {
                Current[CurrentIndex] = color;

                Core.WriteData(Owner ?? this,
                    Address,
                    Current.ToBytes(true),
                    Entry + "Palette changed");
            }
            else
            {
                Core.WriteData(Owner ?? this,
                    CurrentAddress,
                    color.ToBytes(true),
                    Entry + "Palette color " + CurrentIndex + " changed");
            }
        }
        public void Core_SwapColors(Int32 index1, Int32 index2)
        {
            if (index1 < 0 || index1 >= PaletteAmount * 16
             || index2 < 0 || index2 >= PaletteAmount * 16)
                throw new Exception("Invalid index given.");

            GBA.Color color1 = Current[index1];
            GBA.Color color2 = Current[index2];

            UI.SuspendUpdate();

            if (IsCompressed)
            {
                Current.Swap(index1, index2);

                Core.WriteData(Owner ?? this,
                    Address,
                    Current.ToBytes(true),
                    Entry + "Palette changed");
            }
            else
            {
                Core.WriteData(Owner ?? this,
                    Address + index2 * 2,
                    color1.ToBytes(true),
                    Entry + "Palette color " + index1 + " swapped");

                Core.WriteData(Owner ?? this,
                    Address + index1 * 2,
                    color2.ToBytes(true),
                    Entry + "Palette color " + index2 + " swapped");
            }

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }



        private void SwapButton_Click(Object sender, EventArgs e)
        {
            SwapMode = true;
            SwapButton.Text = "Cancel";
            SwapButton.Enabled = true;
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
                Core_Insert(openWindow.FileName);
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
                File.WriteAllBytes(saveWindow.FileName, Current.ToBytes(false));
            }
        }

        private void AlphaCheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_WriteColor(Current[CurrentIndex].SetAlpha(AlphaCheckBox.Checked));
        }

        private void NumBox_16bit_ValueChanged(Object sender, EventArgs e)
        {
            Core_WriteColor(new GBA.Color((UInt16)NumBox_16bit.Value));
        }
        private void NumBox_32bit_ValueChanged(Object sender, EventArgs e)
        {
            Core_WriteColor(new GBA.Color((UInt32)NumBox_32bit.Value));
        }

        private void NumBox_R_ValueChanged(Object sender, EventArgs e)
        {
            Core_WriteColor(Current[CurrentIndex].SetValueR((Byte)NumBox_R.Value));
        }
        private void NumBox_G_ValueChanged(Object sender, EventArgs e)
        {
            Core_WriteColor(Current[CurrentIndex].SetValueG((Byte)NumBox_G.Value));
        }
        private void NumBox_B_ValueChanged(Object sender, EventArgs e)
        {
            Core_WriteColor(Current[CurrentIndex].SetValueB((Byte)NumBox_B.Value));
        }

        private void TrackBar_R_Scroll(Object sender, EventArgs e)
        {
            NumBox_R.Value = TrackBar_R.Value;
        }
        private void TrackBar_G_Scroll(Object sender, EventArgs e)
        {
            NumBox_G.Value = TrackBar_G.Value;
        }
        private void TrackBar_B_Scroll(Object sender, EventArgs e)
        {
            NumBox_B.Value = TrackBar_B.Value;
        }
    }
}
