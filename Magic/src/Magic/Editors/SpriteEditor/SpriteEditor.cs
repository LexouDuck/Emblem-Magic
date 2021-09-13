using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Compression;
using GBA;
using Magic.Components;

namespace Magic.Editors
{
    public partial class SpriteEditor : Editor
    {
        /// <summary>
        /// The OAM array this OAM editor is to edit
        /// </summary>
        OAM_Array Current { get; set; }

        /// <summary>
        /// The image to display the sprites for this OAM Editor
        /// </summary>
        SpriteSheet Display { get; set; }

        /// <summary>
        /// The text for the description when writing
        /// </summary>
        String Entry { get; }

        /// <summary>
        /// The address at which the OAM array is located
        /// </summary>
        Pointer Address;
        /// <summary>
        /// If under 0, the OAM to edit is not LZ77-compressed, and the negative value is length of OAM data to read.<para/>
        /// Otherwise if 0 or above, data is compressed and the value is the offset to start at within the compressed data
        /// </summary>
        Int32 Compressed;
        /// <summary>
        /// The X offset to show the OAM sprites at
        /// </summary>
        Int32 OffsetX;
        /// <summary>
        /// The Y offset to show the OAM sprites at
        /// </summary>
        Int32 OffsetY;
        /// <summary>
        /// The Palette to use for displaying OAM sprites
        /// </summary>
        Palette Colors;
        /// <summary>
        /// The TileSheet with which the OAM is to be displayed
        /// </summary>
        Tileset Tiles;


        /// <summary>
        /// Enables or disables controls basedon whether the current sprite is an affine sprite or not
        /// </summary>
        public Boolean AffineSpriteMode
        {
            set
            {
                if (value)
                {
                    this.FlipH_CheckBox.Enabled = false;
                    this.FlipV_CheckBox.Enabled = false;

                    this.Affine_GroupBox.Enabled = true;
                }
                else
                {
                    this.FlipH_CheckBox.Enabled = true;
                    this.FlipV_CheckBox.Enabled = true;

                    this.Affine_GroupBox.Enabled = false;
                }
            }
        }



        public SpriteEditor(Editor owner,
            String entry,
            Pointer address,
            Int32 compressed,
            Int32 offsetX, Int32 offsetY,
            Palette palette,
            Tileset tileset)
        {
            try
            {
                this.InitializeComponent();

                this.Status.Text = entry + address;

                base.Owner = owner;
                this.Entry = entry;

                this.Address = address;
                this.Compressed = compressed;
                this.OffsetX = offsetX;
                this.OffsetY = offsetY;
                this.Colors = palette;
                this.Tiles = tileset;



                this.Sprite_ImageBox.Paint += this.OAM_ImageBox_Paint;
                this.Tileset_ImageBox.Paint += this.Tileset_ImageBox_Paint;

                this.Shape_ComboBox.SelectedIndexChanged -= this.Shape_ComboBox_SelectedIndexChanged;
                this.Shape_ComboBox.DisplayMember = "Key";
                this.Shape_ComboBox.ValueMember = "Value";
                this.Shape_ComboBox.DataSource = new KeyValuePair<String, OAM_Shape>[4]
                {
                    new KeyValuePair<String, OAM_Shape>("Square", OAM_Shape.Square),
                    new KeyValuePair<String, OAM_Shape>("Rect H", OAM_Shape.Rect_H),
                    new KeyValuePair<String, OAM_Shape>("Rect V", OAM_Shape.Rect_V),
                    new KeyValuePair<String, OAM_Shape>("Invalid",OAM_Shape.Invalid),
                };
                this.Shape_ComboBox.SelectedIndexChanged += this.Shape_ComboBox_SelectedIndexChanged;

                this.Size_ComboBox.SelectedIndexChanged -= this.Size_ComboBox_SelectedIndexChanged;
                this.Size_ComboBox.DisplayMember = "Key";
                this.Size_ComboBox.ValueMember = "Value";
                this.Size_ComboBox.DataSource = new KeyValuePair<String, OAM_Size>[4]
                {
                    new KeyValuePair<String, OAM_Size>("Times 1", OAM_Size.Times1),
                    new KeyValuePair<String, OAM_Size>("Times 2", OAM_Size.Times2),
                    new KeyValuePair<String, OAM_Size>("Times 4", OAM_Size.Times4),
                    new KeyValuePair<String, OAM_Size>("Times 8", OAM_Size.Times8),
                };
                this.Size_ComboBox.SelectedIndexChanged += this.Size_ComboBox_SelectedIndexChanged;

                this.GFX_Mode_ComboBox.SelectedIndexChanged -= this.GFX_Mode_ComboBox_SelectedIndexChanged;
                this.GFX_Mode_ComboBox.DisplayMember = "Key";
                this.GFX_Mode_ComboBox.ValueMember = "Value";
                this.GFX_Mode_ComboBox.DataSource = new KeyValuePair<String, OAM_GFXMode>[4]
                {
                    new KeyValuePair<String, OAM_GFXMode>("Normal",      OAM_GFXMode.Normal),
                    new KeyValuePair<String, OAM_GFXMode>("Alpha Blend", OAM_GFXMode.AlphaBlend),
                    new KeyValuePair<String, OAM_GFXMode>("Alpha Mask",  OAM_GFXMode.OBJ_Window),
                    new KeyValuePair<String, OAM_GFXMode>("Invalid",     OAM_GFXMode.Invalid),
                };
                this.GFX_Mode_ComboBox.SelectedIndexChanged -= this.GFX_Mode_ComboBox_SelectedIndexChanged;

                this.OBJ_Mode_ComboBox.SelectedIndexChanged -= this.OBJ_Mode_ComboBox_SelectedIndexChanged;
                this.OBJ_Mode_ComboBox.DisplayMember = "Key";
                this.OBJ_Mode_ComboBox.ValueMember = "Value";
                this.OBJ_Mode_ComboBox.DataSource = new KeyValuePair<String, OAM_OBJMode>[4]
                {
                    new KeyValuePair<String, OAM_OBJMode>("Normal",     OAM_OBJMode.Normal),
                    new KeyValuePair<String, OAM_OBJMode>("Affine",     OAM_OBJMode.Affine),
                    new KeyValuePair<String, OAM_OBJMode>("Hidden",     OAM_OBJMode.Hidden),
                    new KeyValuePair<String, OAM_OBJMode>("Big Affine", OAM_OBJMode.BigAffine),
                };
                this.OBJ_Mode_ComboBox.SelectedIndexChanged += this.OBJ_Mode_ComboBox_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
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
                Byte[] data;
                if (this.Compressed < 0)
                {
                    data = Core.ReadData(this.Address, this.Compressed * -1);
                    this.Current = new OAM_Array(data, 0);
                }
                else
                {
                    data = Core.ReadData(this.Address, 0);
                    this.Current = new OAM_Array(data, (UInt32)this.Compressed);
                }

                this.Entry_ListBox.DataSource = this.Current.Sprites;
                this.Affine_ListBox.DataSource = this.Current.Affines;
                this.Core_UpdateValues();

                this.Display = new SpriteSheet(240, 160);
                this.Display.AddSprite(this.Colors, this.Tiles, this.Current, this.OffsetX, this.OffsetY);
                this.Core_UpdateDisplay(this.Display);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to update the OAM Editor.", ex);
            }
        }

        public void Core_UpdateDisplay(IDisplayable display)
        {
            this.Sprite_ImageBox.Load(display);

            this.Tileset_ImageBox.Load(this.Tiles.ToImage(32, 8, this.Colors.ToBytes(false)));
        }
        public void Core_UpdateValues()
        {
            if (this.Current.Sprites.Count == 0)
                return;

            this.ScreenX_NumBox.ValueChanged -= this.ScreenX_NumBox_ValueChanged;
            this.ScreenY_NumBox.ValueChanged -= this.ScreenY_NumBox_ValueChanged;
            this.Shape_ComboBox.SelectedIndexChanged -= this.Shape_ComboBox_SelectedIndexChanged;
            this.Size_ComboBox.SelectedIndexChanged -= this.Size_ComboBox_SelectedIndexChanged;
            this.Priority_NumBox.ValueChanged -= this.Priority_NumBox_ValueChanged;

            this.GFX_Mode_ComboBox.SelectedIndexChanged -= this.GFX_Mode_ComboBox_SelectedIndexChanged;
            this.OBJ_Mode_ComboBox.SelectedIndexChanged -= this.OBJ_Mode_ComboBox_SelectedIndexChanged;
            this.FullColors_CheckBox.CheckedChanged -= this.FullColors_CheckBox_CheckedChanged;
            this.DrawMosaic_CheckBox.CheckedChanged -= this.DrawMosaic_CheckBox_CheckedChanged;
            this.Palette_NumBox.ValueChanged -= this.Palette_NumBox_ValueChanged;

            this.SheetX_NumBox.ValueChanged -= this.SheetX_NumBox_ValueChanged;
            this.SheetY_NumBox.ValueChanged -= this.SheetY_NumBox_ValueChanged;

            this.FlipH_CheckBox.CheckedChanged -= this.FlipH_CheckBox_CheckedChanged;
            this.FlipV_CheckBox.CheckedChanged -= this.FlipV_CheckBox_CheckedChanged;

            this.Affine_Index_NumBox.ValueChanged -= this.Affine_Index_NumBox_ValueChanged;
            this.Affine_Ux_NumBox.ValueChanged -= this.Affine_Ux_NumBox_ValueChanged;
            this.Affine_Vx_NumBox.ValueChanged -= this.Affine_Uy_NumBox_ValueChanged;
            this.Affine_Uy_NumBox.ValueChanged -= this.Affine_Vx_NumBox_ValueChanged;
            this.Affine_Vy_NumBox.ValueChanged -= this.Affine_Vy_NumBox_ValueChanged;

            try
            {
                this.Entry_Amount_Label.Text = "OAM Objects: " + this.Current.Sprites.Count;
                this.Affine_Amount_Label.Text = "Affine Transforms: " + this.Current.Affines.Count;

                this.Sprite_PaletteBox.Load(this.Colors);

                if (this.Entry_ListBox.SelectedIndices.Count == 1)
                {
                    int index = this.Entry_ListBox.SelectedIndex;
                    if (index >= this.Current.Sprites.Count)
                        throw new Exception("index is outside the bounds of this OAM array");

                    this.ScreenX_NumBox.Value = this.Current[index].ScreenX;
                    this.ScreenY_NumBox.Value = this.Current[index].ScreenY;

                    this.SheetX_NumBox.Value = this.Current[index].SheetX;
                    this.SheetY_NumBox.Value = this.Current[index].SheetY;
                    Size dimensions = this.Current[index].GetDimensions();
                    this.ShapeSize_Info.Text = (dimensions.Width * 8) + "x" + (dimensions.Height * 8);
                    this.Shape_ComboBox.SelectedValue = this.Current[index].SpriteShape;
                    this.Size_ComboBox.SelectedValue = this.Current[index].SpriteSize;
                    this.Priority_NumBox.Value = this.Current[index].Priority;

                    this.GFX_Mode_ComboBox.SelectedValue = this.Current[index].GFXMode;
                    this.OBJ_Mode_ComboBox.SelectedValue = this.Current[index].OBJMode;
                    this.FullColors_CheckBox.Checked = this.Current[index].FullColors;
                    this.DrawMosaic_CheckBox.Checked = this.Current[index].DrawMosaic;
                    this.Palette_NumBox.Value = this.Current[index].Palette;

                    if (this.Current[index].IsAffineSprite())
                    {
                        OAM_Affine transform = this.Current.Affines[this.Current[index].AffineIndex];
                        //Affine_ListBox.SelectedIndex = Current[index].AffineIndex;
                        this.FlipH_CheckBox.Enabled = false; this.FlipH_CheckBox.Checked = false;
                        this.FlipV_CheckBox.Enabled = false; this.FlipV_CheckBox.Checked = false;

                        this.AffineSpriteMode = true;
                        this.Affine_Index_NumBox.Value = this.Current[index].AffineIndex;
                        this.Affine_Ux_NumBox.Value = (Decimal)transform.Ux;
                        this.Affine_Uy_NumBox.Value = (Decimal)transform.Uy;
                        this.Affine_Vx_NumBox.Value = (Decimal)transform.Vx;
                        this.Affine_Vy_NumBox.Value = (Decimal)transform.Vy;
                    }
                    else
                    {
                        this.FlipH_CheckBox.Enabled = true; this.FlipH_CheckBox.Checked = this.Current[index].FlipH;
                        this.FlipV_CheckBox.Enabled = true; this.FlipV_CheckBox.Checked = this.Current[index].FlipV;

                        this.AffineSpriteMode = false;
                        this.Affine_Index_NumBox.Value = 0;
                        this.Affine_Ux_NumBox.Value = (Decimal)0;
                        this.Affine_Uy_NumBox.Value = (Decimal)0;
                        this.Affine_Vx_NumBox.Value = (Decimal)0;
                        this.Affine_Vy_NumBox.Value = (Decimal)0;
                    }
                }
                else
                {
                    this.ScreenX_NumBox.Value = 0;
                    this.ScreenY_NumBox.Value = 0;

                    this.SheetX_NumBox.Value = 0;
                    this.SheetY_NumBox.Value = 0;
                    this.Shape_ComboBox.SelectedValue = OAM_Shape.Invalid;
                    this.Size_ComboBox.SelectedValue = OAM_Size.Times1;

                    this.GFX_Mode_ComboBox.SelectedValue = OAM_GFXMode.Normal;
                    this.OBJ_Mode_ComboBox.SelectedValue = OAM_OBJMode.Normal;
                    this.FullColors_CheckBox.Checked = false;
                    this.DrawMosaic_CheckBox.Checked = false;
                    this.Priority_NumBox.Value = 0;
                    this.Palette_NumBox.Value = 0;

                    this.FlipH_CheckBox.Checked = false;
                    this.FlipV_CheckBox.Checked = false;

                    if (this.Entry_ListBox.SelectedIndices.Count == 0)
                    {
                        this.ScreenX_NumBox.Text = "";
                        this.ScreenY_NumBox.Text = "";

                        this.SheetX_NumBox.Text = "";
                        this.SheetY_NumBox.Text = "";
                        this.ShapeSize_Info.Text = "";
                        this.Shape_ComboBox.Text = "";
                        this.Size_ComboBox.Text = "";

                        this.GFX_Mode_ComboBox.Text = "";
                        this.OBJ_Mode_ComboBox.Text = "";
                        this.Priority_NumBox.Text = "";
                        this.Palette_NumBox.Text = "";
                    }
                    else
                    {
                        this.SheetX_NumBox.Text = "-";
                        this.SheetY_NumBox.Text = "-";

                        this.ScreenX_NumBox.Text = "-";
                        this.ScreenY_NumBox.Text = "-";
                        this.ShapeSize_Info.Text = "(mixed)";
                        this.Shape_ComboBox.Text = "(mixed)";
                        this.Size_ComboBox.Text = "(mixed)";

                        this.GFX_Mode_ComboBox.Text = "(mixed)";
                        this.OBJ_Mode_ComboBox.Text = "(mixed)";
                        this.Priority_NumBox.Text = "-";
                        this.Palette_NumBox.Text = "-";
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load OAM values.", ex);
            }

            this.ScreenX_NumBox.ValueChanged += this.ScreenX_NumBox_ValueChanged;
            this.ScreenY_NumBox.ValueChanged += this.ScreenY_NumBox_ValueChanged;
            this.Shape_ComboBox.SelectedIndexChanged += this.Shape_ComboBox_SelectedIndexChanged;
            this.Size_ComboBox.SelectedIndexChanged += this.Size_ComboBox_SelectedIndexChanged;
            this.Priority_NumBox.ValueChanged += this.Priority_NumBox_ValueChanged;

            this.GFX_Mode_ComboBox.SelectedIndexChanged += this.GFX_Mode_ComboBox_SelectedIndexChanged;
            this.OBJ_Mode_ComboBox.SelectedIndexChanged += this.OBJ_Mode_ComboBox_SelectedIndexChanged;
            this.FullColors_CheckBox.CheckedChanged += this.FullColors_CheckBox_CheckedChanged;
            this.DrawMosaic_CheckBox.CheckedChanged += this.DrawMosaic_CheckBox_CheckedChanged;
            this.Palette_NumBox.ValueChanged += this.Palette_NumBox_ValueChanged;

            this.SheetX_NumBox.ValueChanged += this.SheetX_NumBox_ValueChanged;
            this.SheetY_NumBox.ValueChanged += this.SheetY_NumBox_ValueChanged;

            this.FlipH_CheckBox.CheckedChanged += this.FlipH_CheckBox_CheckedChanged;
            this.FlipV_CheckBox.CheckedChanged += this.FlipV_CheckBox_CheckedChanged;

            this.Affine_Index_NumBox.ValueChanged += this.Affine_Index_NumBox_ValueChanged;
            this.Affine_Ux_NumBox.ValueChanged += this.Affine_Ux_NumBox_ValueChanged;
            this.Affine_Vx_NumBox.ValueChanged += this.Affine_Uy_NumBox_ValueChanged;
            this.Affine_Uy_NumBox.ValueChanged += this.Affine_Vx_NumBox_ValueChanged;
            this.Affine_Vy_NumBox.ValueChanged += this.Affine_Vy_NumBox_ValueChanged;
        }
        public void Core_Write()
        {
            /* TODO handle affine data more smartly
            if (Current[Entry_ListBox.SelectedIndex].IsAffineSprite())
            {
                while (Affine_Index_NumBox.Value >= Current.Affines.Count)
                {
                    Current.Affines.Add(new OAM_Affine());
                }
            }
            */
            UI.SuspendUpdate();
            try
            {
                Byte[] data;

                if (this.Compressed < 0)
                {
                    data = this.Current.ToBytes();
                }
                else // its in a compressed OAM data block
                {
                    data = Core.ReadData(this.Address, 0);
                    Byte[] oam = this.Current.ToBytes();
                    Array.Copy(oam, 0, data, this.Compressed, oam.Length);
                    data = LZ77.Compress(data);
                }
                Pointer address = Core.FindData(this.Address.ToBytes(), 4);
                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint OAM Data",
                    "The OAM data to insert might need to be repointed.", this.Entry,
                    new Tuple<String, Pointer, Int32>[] { Tuple.Create("OAM Data", this.Address, data.Length) },
                    new Pointer[] { address });
                if (cancel) return;

                this.Address = Core.ReadPointer(address);

                Core.WriteData(this,
                    this.Address, data,
                    this.Entry + "OAM changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the OAM data.", ex);
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }



        public static void Core_DrawRectangle(ImageBox control, Graphics g, System.Drawing.Color color, 
            Point pos,
            Size size)
        {
            using Pen pen = new(color);
            Point offset = new(
                (control.Width  / 2) - (control.Display.Width  / 2),
                (control.Height / 2) - (control.Display.Height / 2));
            g.DrawRectangle(pen, new Rectangle(
                offset.X + pos.X,
                offset.Y + pos.Y,
                size.Width  - 1,
                size.Height - 1));
        }
        public static void Core_DrawVector(ImageBox control, Graphics g, System.Drawing.Color color,
            Point a,
            Point b)
        {
            using Pen pen = new(color);
            Point offset = new(
                (control.Width  / 2) - (control.Display.Width  / 2),
                (control.Height / 2) - (control.Display.Height / 2));
            g.DrawLine(pen,
                new Point(offset.X + a.X, offset.Y + a.Y),
                new Point(offset.X + b.X, offset.Y + b.Y));
        }
        public static void Core_DrawAffineVectors(ImageBox control, Graphics g,
            Point pos,
            Size size,
            OAM_Affine affine = null)
        {
            Point offset;
            if (affine == null)
            {
                affine = OAM_Affine.IDENTITY;
                offset = new(0,0);
            }
            else
            {
                offset = new(
                    (size.Width / 2),
                    (size.Height / 2));
                offset = new(
                    offset.X - ((int)(affine.Ux * offset.X) + (int)(affine.Vx * offset.Y)),
                    offset.Y - ((int)(affine.Uy * offset.X) + (int)(affine.Vy * offset.Y)));
                pos += new Size(offset);
            }

            // U vector (horizontal axis)
            Core_DrawVector(control, g, System.Drawing.Color.Red,
                pos, new Point(
                pos.X + (int)(affine.Ux * size.Width),
                pos.Y + (int)(affine.Uy * size.Width)));

            // V vector (vertical axis)
            Core_DrawVector(control, g, System.Drawing.Color.Green,
                pos, new Point(
                pos.X + (int)(affine.Vx * size.Height),
                pos.Y + (int)(affine.Vy * size.Height)));
        }



        private void OAM_ImageBox_Paint(Object sender, PaintEventArgs e)
        {
            if (this.Sprite_ImageBox == null)
                return;
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                OAM obj = this.Current[index];
                Size size = 8 * obj.GetDimensions();
                Point pos = new(
                    obj.ScreenX + this.OffsetX,
                    obj.ScreenY + this.OffsetY);

                Core_DrawRectangle(this.Sprite_ImageBox, e.Graphics,
                    SystemColors.Highlight,
                    pos, size * (this.Current[index].OBJMode == OAM_OBJMode.BigAffine ? 2 : 1));
                if (obj.IsAffineSprite())
                {
                    Core_DrawAffineVectors(this.Sprite_ImageBox, e.Graphics,
                        pos, size,
                        this.Current.Affines[obj.AffineIndex]);
                }
            }
        }
        private void Tileset_ImageBox_Paint(Object sender, PaintEventArgs e)
        {
            if (this.Tileset_ImageBox == null)
                return;
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                OAM obj = this.Current[index];
                Size size = 8 * obj.GetDimensions();
                Point pos = new(
                    obj.SheetX * 8,
                    obj.SheetY * 8);

                Core_DrawRectangle(this.Tileset_ImageBox, e.Graphics,
                    SystemColors.Highlight,
                    pos, size);
                if (obj.IsAffineSprite())
                {
                    Core_DrawAffineVectors(this.Tileset_ImageBox, e.Graphics,
                        pos, size);
                }
            }
        }



        private void Entry_ListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            this.Core_UpdateDisplay(this.Display);
            this.Core_UpdateValues();
        }
        private void Entry_ListBox_MouseDown(Object sender, MouseEventArgs e)
        {   // deselect list item when user clicks empty area
            int index = this.Entry_ListBox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                this.Entry_ListBox.SelectedItems.Clear();
            }
        }

        private void Entry_DeleteButton_Click(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current.Sprites.RemoveAt(index);
            }
            this.Core_Write();
        }
        private void Entry_CreateButton_Click(Object sender, EventArgs e)
        {
            this.Current.Sprites.Add(OAM.NULL);

            this.Core_Write();
        }
        private void Entry_MoveUp_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                OAM tmp = this.Current.Sprites[index - 1];
                this.Current.Sprites[index - 1] = this.Current.Sprites[index];
                this.Current.Sprites[index] = tmp;
            }
            this.Core_Write();
        }
        private void Entry_MoveDown_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                OAM tmp = this.Current.Sprites[index + 1];
                this.Current.Sprites[index + 1] = this.Current.Sprites[index];
                this.Current.Sprites[index] = tmp;
            }
            this.Core_Write();
        }



        private void Affine_ListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            this.Core_UpdateValues();
        }
        private void Affine_ListBox_MouseDown(Object sender, MouseEventArgs e)
        {   // deselect list item when user clicks empty area
            int index = this.Affine_ListBox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                this.Affine_ListBox.SelectedItems.Clear();
            }
        }

        private void Affine_DeleteButton_Click(Object sender, EventArgs e)
        {
            foreach (int index in this.Affine_ListBox.SelectedIndices)
            {
                this.Current.Affines.RemoveAt(index);
            }
            this.Core_Write();
        }
        private void Affine_CreateButton_Click(Object sender, EventArgs e)
        {
            this.Current.Affines.Add(OAM_Affine.IDENTITY);

            this.Core_Write();
        }
        private void Affine_MoveUp_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in this.Affine_ListBox.SelectedIndices)
            {
                OAM_Affine tmp = this.Current.Affines[index - 1];
                this.Current.Affines[index - 1] = this.Current.Affines[index];
                this.Current.Affines[index] = tmp;
            }
            this.Core_Write();
        }
        private void Affine_MoveDown_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in this.Affine_ListBox.SelectedIndices)
            {
                OAM_Affine tmp = this.Current.Affines[index + 1];
                this.Current.Affines[index + 1] = this.Current.Affines[index];
                this.Current.Affines[index] = tmp;
            }
            this.Core_Write();
        }



        private void ScreenX_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].ScreenX = (Int16)this.ScreenX_NumBox.Value;
            }
            this.Core_Write();
        }
        private void ScreenY_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].ScreenY = (Int16)this.ScreenY_NumBox.Value;
            }
            this.Core_Write();
        }
        private void Shape_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].SpriteShape = (OAM_Shape)this.Shape_ComboBox.SelectedValue;
            }
            this.Core_Write();
        }
        private void Size_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].SpriteSize = (OAM_Size)this.Size_ComboBox.SelectedValue;
            }
            this.Core_Write();
        }

        private void SheetX_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].SheetX = (Byte)this.SheetX_NumBox.Value;
            }
            this.Core_Write();
        }
        private void SheetY_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].SheetY = (Byte)this.SheetY_NumBox.Value;
            }
            this.Core_Write();
        }

        private void GFX_Mode_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].GFXMode = (OAM_GFXMode)this.GFX_Mode_ComboBox.SelectedValue;
            }
            this.Core_Write();
        }
        private void OBJ_Mode_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].OBJMode = (OAM_OBJMode)this.OBJ_Mode_ComboBox.SelectedValue;
            }
            this.Core_Write();
        }

        private void FullColors_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].FullColors = this.FullColors_CheckBox.Checked;
            }
            this.Core_Write();
        }
        private void DrawMosaic_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].DrawMosaic = this.DrawMosaic_CheckBox.Checked;
            }
            this.Core_Write();
        }
        private void Palette_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].Palette = (Byte)this.Palette_NumBox.Value;
            }
            this.Core_Write();
        }
        private void Priority_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].Priority = (Byte)this.Priority_NumBox.Value;
            }
            this.Core_Write();
        }

        private void FlipH_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].FlipH = this.FlipH_CheckBox.Checked;
            }
            this.Core_Write();
        }
        private void FlipV_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].FlipV = this.FlipV_CheckBox.Checked;
            }
            this.Core_Write();
        }



        private void Affine_Index_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Entry_ListBox.SelectedIndices)
            {
                this.Current[index].AffineIndex = this.Affine_Index_NumBox.Value;
            }
            this.Core_Write();
        }
        private void Affine_Ux_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Affine_ListBox.SelectedIndices)
            {
                this.Current.Affines[index].Ux = (Double)this.Affine_Ux_NumBox.Value;
            }
            this.Core_Write();
        }
        private void Affine_Vx_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Affine_ListBox.SelectedIndices)
            {
                this.Current.Affines[index].Uy = (Double)this.Affine_Uy_NumBox.Value;
            }
            this.Core_Write();
        }
        private void Affine_Uy_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Affine_ListBox.SelectedIndices)
            {
                this.Current.Affines[index].Vx = (Double)this.Affine_Vx_NumBox.Value;
            }
            this.Core_Write();
        }
        private void Affine_Vy_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in this.Affine_ListBox.SelectedIndices)
            {
                this.Current.Affines[index].Vy = (Double)this.Affine_Vy_NumBox.Value;
            }
            this.Core_Write();
        }
    }
}
