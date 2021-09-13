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
                    FlipH_CheckBox.Enabled = false;
                    FlipV_CheckBox.Enabled = false;

                    Affine_GroupBox.Enabled = true;
                }
                else
                {
                    FlipH_CheckBox.Enabled = true;
                    FlipV_CheckBox.Enabled = true;

                    Affine_GroupBox.Enabled = false;
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
                InitializeComponent();

                Status.Text = entry + address;

                base.Owner = owner;
                Entry = entry;

                Address = address;
                Compressed = compressed;
                OffsetX = offsetX;
                OffsetY = offsetY;
                Colors = palette;
                Tiles = tileset;



                Sprite_ImageBox.Paint += OAM_ImageBox_Paint;
                Tileset_ImageBox.Paint += Tileset_ImageBox_Paint;

                Shape_ComboBox.SelectedIndexChanged -= Shape_ComboBox_SelectedIndexChanged;
                Shape_ComboBox.DisplayMember = "Key";
                Shape_ComboBox.ValueMember = "Value";
                Shape_ComboBox.DataSource = new KeyValuePair<String, OAM_Shape>[4]
                {
                    new KeyValuePair<String, OAM_Shape>("Square", OAM_Shape.Square),
                    new KeyValuePair<String, OAM_Shape>("Rect H", OAM_Shape.Rect_H),
                    new KeyValuePair<String, OAM_Shape>("Rect V", OAM_Shape.Rect_V),
                    new KeyValuePair<String, OAM_Shape>("Invalid",OAM_Shape.Invalid),
                };
                Shape_ComboBox.SelectedIndexChanged += Shape_ComboBox_SelectedIndexChanged;

                Size_ComboBox.SelectedIndexChanged -= Size_ComboBox_SelectedIndexChanged;
                Size_ComboBox.DisplayMember = "Key";
                Size_ComboBox.ValueMember = "Value";
                Size_ComboBox.DataSource = new KeyValuePair<String, OAM_Size>[4]
                {
                    new KeyValuePair<String, OAM_Size>("Times 1", OAM_Size.Times1),
                    new KeyValuePair<String, OAM_Size>("Times 2", OAM_Size.Times2),
                    new KeyValuePair<String, OAM_Size>("Times 4", OAM_Size.Times4),
                    new KeyValuePair<String, OAM_Size>("Times 8", OAM_Size.Times8),
                };
                Size_ComboBox.SelectedIndexChanged += Size_ComboBox_SelectedIndexChanged;

                GFX_Mode_ComboBox.SelectedIndexChanged -= GFX_Mode_ComboBox_SelectedIndexChanged;
                GFX_Mode_ComboBox.DisplayMember = "Key";
                GFX_Mode_ComboBox.ValueMember = "Value";
                GFX_Mode_ComboBox.DataSource = new KeyValuePair<String, OAM_GFXMode>[4]
                {
                    new KeyValuePair<String, OAM_GFXMode>("Normal",      OAM_GFXMode.Normal),
                    new KeyValuePair<String, OAM_GFXMode>("Alpha Blend", OAM_GFXMode.AlphaBlend),
                    new KeyValuePair<String, OAM_GFXMode>("Alpha Mask",  OAM_GFXMode.OBJ_Window),
                    new KeyValuePair<String, OAM_GFXMode>("Invalid",     OAM_GFXMode.Invalid),
                };
                GFX_Mode_ComboBox.SelectedIndexChanged -= GFX_Mode_ComboBox_SelectedIndexChanged;

                OBJ_Mode_ComboBox.SelectedIndexChanged -= OBJ_Mode_ComboBox_SelectedIndexChanged;
                OBJ_Mode_ComboBox.DisplayMember = "Key";
                OBJ_Mode_ComboBox.ValueMember = "Value";
                OBJ_Mode_ComboBox.DataSource = new KeyValuePair<String, OAM_OBJMode>[4]
                {
                    new KeyValuePair<String, OAM_OBJMode>("Normal",     OAM_OBJMode.Normal),
                    new KeyValuePair<String, OAM_OBJMode>("Affine",     OAM_OBJMode.Affine),
                    new KeyValuePair<String, OAM_OBJMode>("Hidden",     OAM_OBJMode.Hidden),
                    new KeyValuePair<String, OAM_OBJMode>("Big Affine", OAM_OBJMode.BigAffine),
                };
                OBJ_Mode_ComboBox.SelectedIndexChanged += OBJ_Mode_ComboBox_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
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
                Byte[] data;
                if (Compressed < 0)
                {
                    data = Core.ReadData(Address, Compressed * -1);
                    Current = new OAM_Array(data, 0);
                }
                else
                {
                    data = Core.ReadData(Address, 0);
                    Current = new OAM_Array(data, (UInt32)Compressed);
                }

                Entry_ListBox.DataSource = Current.Sprites;
                Affine_ListBox.DataSource = Current.Affines;
                Core_UpdateValues();

                Display = new SpriteSheet(240, 160);
                Display.AddSprite(Colors, Tiles, Current, OffsetX, OffsetY);
                Core_UpdateDisplay(Display);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to update the OAM Editor.", ex);
            }
        }

        public void Core_UpdateDisplay(IDisplayable display)
        {
            Sprite_ImageBox.Load(display);

            Tileset_ImageBox.Load(Tiles.ToImage(32, 8, Colors.ToBytes(false)));
        }
        public void Core_UpdateValues()
        {
            if (Current.Sprites.Count == 0)
                return;

            ScreenX_NumBox.ValueChanged -= ScreenX_NumBox_ValueChanged;
            ScreenY_NumBox.ValueChanged -= ScreenY_NumBox_ValueChanged;
            Shape_ComboBox.SelectedIndexChanged -= Shape_ComboBox_SelectedIndexChanged;
            Size_ComboBox.SelectedIndexChanged -= Size_ComboBox_SelectedIndexChanged;
            Priority_NumBox.ValueChanged -= Priority_NumBox_ValueChanged;

            GFX_Mode_ComboBox.SelectedIndexChanged -= GFX_Mode_ComboBox_SelectedIndexChanged;
            OBJ_Mode_ComboBox.SelectedIndexChanged -= OBJ_Mode_ComboBox_SelectedIndexChanged;
            FullColors_CheckBox.CheckedChanged -= FullColors_CheckBox_CheckedChanged;
            DrawMosaic_CheckBox.CheckedChanged -= DrawMosaic_CheckBox_CheckedChanged;
            Palette_NumBox.ValueChanged -= Palette_NumBox_ValueChanged;

            SheetX_NumBox.ValueChanged -= SheetX_NumBox_ValueChanged;
            SheetY_NumBox.ValueChanged -= SheetY_NumBox_ValueChanged;

            FlipH_CheckBox.CheckedChanged -= FlipH_CheckBox_CheckedChanged;
            FlipV_CheckBox.CheckedChanged -= FlipV_CheckBox_CheckedChanged;

            Affine_Index_NumBox.ValueChanged -= Affine_Index_NumBox_ValueChanged;
            Affine_Ux_NumBox.ValueChanged -= Affine_Ux_NumBox_ValueChanged;
            Affine_Vx_NumBox.ValueChanged -= Affine_Uy_NumBox_ValueChanged;
            Affine_Uy_NumBox.ValueChanged -= Affine_Vx_NumBox_ValueChanged;
            Affine_Vy_NumBox.ValueChanged -= Affine_Vy_NumBox_ValueChanged;

            try
            {
                Entry_Amount_Label.Text = "OAM Objects: " + Current.Sprites.Count;
                Affine_Amount_Label.Text = "Affine Transforms: " + Current.Affines.Count;

                Sprite_PaletteBox.Load(Colors);

                if (Entry_ListBox.SelectedIndices.Count == 1)
                {
                    int index = Entry_ListBox.SelectedIndex;
                    if (index >= Current.Sprites.Count)
                        throw new Exception("index is outside the bounds of this OAM array");

                    ScreenX_NumBox.Value = Current[index].ScreenX;
                    ScreenY_NumBox.Value = Current[index].ScreenY;

                    SheetX_NumBox.Value = Current[index].SheetX;
                    SheetY_NumBox.Value = Current[index].SheetY;
                    Size dimensions = Current[index].GetDimensions();
                    ShapeSize_Info.Text = (dimensions.Width * 8) + "x" + (dimensions.Height * 8);
                    Shape_ComboBox.SelectedValue = Current[index].SpriteShape;
                    Size_ComboBox.SelectedValue = Current[index].SpriteSize;
                    Priority_NumBox.Value = Current[index].Priority;

                    GFX_Mode_ComboBox.SelectedValue = Current[index].GFXMode;
                    OBJ_Mode_ComboBox.SelectedValue = Current[index].OBJMode;
                    FullColors_CheckBox.Checked = Current[index].FullColors;
                    DrawMosaic_CheckBox.Checked = Current[index].DrawMosaic;
                    Palette_NumBox.Value = Current[index].Palette;

                    if (Current[index].IsAffineSprite())
                    {
                        OAM_Affine transform = Current.Affines[Current[index].AffineIndex];
                        //Affine_ListBox.SelectedIndex = Current[index].AffineIndex;
                        FlipH_CheckBox.Enabled = false; FlipH_CheckBox.Checked = false;
                        FlipV_CheckBox.Enabled = false; FlipV_CheckBox.Checked = false;

                        AffineSpriteMode = true;
                        Affine_Index_NumBox.Value = Current[index].AffineIndex;
                        Affine_Ux_NumBox.Value = (Decimal)transform.Ux;
                        Affine_Uy_NumBox.Value = (Decimal)transform.Uy;
                        Affine_Vx_NumBox.Value = (Decimal)transform.Vx;
                        Affine_Vy_NumBox.Value = (Decimal)transform.Vy;
                    }
                    else
                    {
                        FlipH_CheckBox.Enabled = true; FlipH_CheckBox.Checked = Current[index].FlipH;
                        FlipV_CheckBox.Enabled = true; FlipV_CheckBox.Checked = Current[index].FlipV;

                        AffineSpriteMode = false;
                        Affine_Index_NumBox.Value = 0;
                        Affine_Ux_NumBox.Value = (Decimal)0;
                        Affine_Uy_NumBox.Value = (Decimal)0;
                        Affine_Vx_NumBox.Value = (Decimal)0;
                        Affine_Vy_NumBox.Value = (Decimal)0;
                    }
                }
                else
                {
                    ScreenX_NumBox.Value = 0;
                    ScreenY_NumBox.Value = 0;

                    SheetX_NumBox.Value = 0;
                    SheetY_NumBox.Value = 0;
                    Shape_ComboBox.SelectedValue = OAM_Shape.Invalid;
                    Size_ComboBox.SelectedValue = OAM_Size.Times1;

                    GFX_Mode_ComboBox.SelectedValue = OAM_GFXMode.Normal;
                    OBJ_Mode_ComboBox.SelectedValue = OAM_OBJMode.Normal;
                    FullColors_CheckBox.Checked = false;
                    DrawMosaic_CheckBox.Checked = false;
                    Priority_NumBox.Value = 0;
                    Palette_NumBox.Value = 0;

                    FlipH_CheckBox.Checked = false;
                    FlipV_CheckBox.Checked = false;

                    if (Entry_ListBox.SelectedIndices.Count == 0)
                    {
                        ScreenX_NumBox.Text = "";
                        ScreenY_NumBox.Text = "";

                        SheetX_NumBox.Text = "";
                        SheetY_NumBox.Text = "";
                        ShapeSize_Info.Text = "";
                        Shape_ComboBox.Text = "";
                        Size_ComboBox.Text = "";

                        GFX_Mode_ComboBox.Text = "";
                        OBJ_Mode_ComboBox.Text = "";
                        Priority_NumBox.Text = "";
                        Palette_NumBox.Text = "";
                    }
                    else
                    {
                        SheetX_NumBox.Text = "-";
                        SheetY_NumBox.Text = "-";

                        ScreenX_NumBox.Text = "-";
                        ScreenY_NumBox.Text = "-";
                        ShapeSize_Info.Text = "(mixed)";
                        Shape_ComboBox.Text = "(mixed)";
                        Size_ComboBox.Text = "(mixed)";

                        GFX_Mode_ComboBox.Text = "(mixed)";
                        OBJ_Mode_ComboBox.Text = "(mixed)";
                        Priority_NumBox.Text = "-";
                        Palette_NumBox.Text = "-";
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load OAM values.", ex);
            }

            ScreenX_NumBox.ValueChanged += ScreenX_NumBox_ValueChanged;
            ScreenY_NumBox.ValueChanged += ScreenY_NumBox_ValueChanged;
            Shape_ComboBox.SelectedIndexChanged += Shape_ComboBox_SelectedIndexChanged;
            Size_ComboBox.SelectedIndexChanged += Size_ComboBox_SelectedIndexChanged;
            Priority_NumBox.ValueChanged += Priority_NumBox_ValueChanged;

            GFX_Mode_ComboBox.SelectedIndexChanged += GFX_Mode_ComboBox_SelectedIndexChanged;
            OBJ_Mode_ComboBox.SelectedIndexChanged += OBJ_Mode_ComboBox_SelectedIndexChanged;
            FullColors_CheckBox.CheckedChanged += FullColors_CheckBox_CheckedChanged;
            DrawMosaic_CheckBox.CheckedChanged += DrawMosaic_CheckBox_CheckedChanged;
            Palette_NumBox.ValueChanged += Palette_NumBox_ValueChanged;

            SheetX_NumBox.ValueChanged += SheetX_NumBox_ValueChanged;
            SheetY_NumBox.ValueChanged += SheetY_NumBox_ValueChanged;

            FlipH_CheckBox.CheckedChanged += FlipH_CheckBox_CheckedChanged;
            FlipV_CheckBox.CheckedChanged += FlipV_CheckBox_CheckedChanged;

            Affine_Index_NumBox.ValueChanged += Affine_Index_NumBox_ValueChanged;
            Affine_Ux_NumBox.ValueChanged += Affine_Ux_NumBox_ValueChanged;
            Affine_Vx_NumBox.ValueChanged += Affine_Uy_NumBox_ValueChanged;
            Affine_Uy_NumBox.ValueChanged += Affine_Vx_NumBox_ValueChanged;
            Affine_Vy_NumBox.ValueChanged += Affine_Vy_NumBox_ValueChanged;
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

                if (Compressed < 0)
                {
                    data = Current.ToBytes();
                }
                else // its in a compressed OAM data block
                {
                    data = Core.ReadData(Address, 0);
                    Byte[] oam = Current.ToBytes();
                    Array.Copy(oam, 0, data, Compressed, oam.Length);
                    data = LZ77.Compress(data);
                }
                Pointer address = Core.FindData(Address.ToBytes(), 4);
                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint OAM Data",
                    "The OAM data to insert might need to be repointed.", Entry,
                    new Tuple<String, Pointer, Int32>[] { Tuple.Create("OAM Data", Address, data.Length) },
                    new Pointer[] { address });
                if (cancel) return;

                Address = Core.ReadPointer(address);

                Core.WriteData(this,
                    Address, data,
                    Entry + "OAM changed");
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
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                OAM obj = Current[index];
                Size size = 8 * obj.GetDimensions();
                Point pos = new(
                    obj.ScreenX + OffsetX,
                    obj.ScreenY + OffsetY);

                Core_DrawRectangle(this.Sprite_ImageBox, e.Graphics,
                    SystemColors.Highlight,
                    pos, size * (Current[index].OBJMode == OAM_OBJMode.BigAffine ? 2 : 1));
                if (obj.IsAffineSprite())
                {
                    Core_DrawAffineVectors(this.Sprite_ImageBox, e.Graphics,
                        pos, size,
                        Current.Affines[obj.AffineIndex]);
                }
            }
        }
        private void Tileset_ImageBox_Paint(Object sender, PaintEventArgs e)
        {
            if (this.Tileset_ImageBox == null)
                return;
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                OAM obj = Current[index];
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
            Core_UpdateDisplay(Display);
            Core_UpdateValues();
        }
        private void Entry_ListBox_MouseDown(Object sender, MouseEventArgs e)
        {   // deselect list item when user clicks empty area
            int index = Entry_ListBox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                Entry_ListBox.SelectedItems.Clear();
            }
        }

        private void Entry_DeleteButton_Click(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current.Sprites.RemoveAt(index);
            }
            Core_Write();
        }
        private void Entry_CreateButton_Click(Object sender, EventArgs e)
        {
            Current.Sprites.Add(OAM.NULL);

            Core_Write();
        }
        private void Entry_MoveUp_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                OAM tmp = Current.Sprites[index - 1];
                Current.Sprites[index - 1] = Current.Sprites[index];
                Current.Sprites[index] = tmp;
            }
            Core_Write();
        }
        private void Entry_MoveDown_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                OAM tmp = Current.Sprites[index + 1];
                Current.Sprites[index + 1] = Current.Sprites[index];
                Current.Sprites[index] = tmp;
            }
            Core_Write();
        }



        private void Affine_ListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Core_UpdateValues();
        }
        private void Affine_ListBox_MouseDown(Object sender, MouseEventArgs e)
        {   // deselect list item when user clicks empty area
            int index = Affine_ListBox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                Affine_ListBox.SelectedItems.Clear();
            }
        }

        private void Affine_DeleteButton_Click(Object sender, EventArgs e)
        {
            foreach (int index in Affine_ListBox.SelectedIndices)
            {
                Current.Affines.RemoveAt(index);
            }
            Core_Write();
        }
        private void Affine_CreateButton_Click(Object sender, EventArgs e)
        {
            Current.Affines.Add(OAM_Affine.IDENTITY);

            Core_Write();
        }
        private void Affine_MoveUp_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in Affine_ListBox.SelectedIndices)
            {
                OAM_Affine tmp = Current.Affines[index - 1];
                Current.Affines[index - 1] = Current.Affines[index];
                Current.Affines[index] = tmp;
            }
            Core_Write();
        }
        private void Affine_MoveDown_Button_Click(Object sender, EventArgs e)
        {
            foreach (int index in Affine_ListBox.SelectedIndices)
            {
                OAM_Affine tmp = Current.Affines[index + 1];
                Current.Affines[index + 1] = Current.Affines[index];
                Current.Affines[index] = tmp;
            }
            Core_Write();
        }



        private void ScreenX_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].ScreenX = (Int16)ScreenX_NumBox.Value;
            }
            Core_Write();
        }
        private void ScreenY_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].ScreenY = (Int16)ScreenY_NumBox.Value;
            }
            Core_Write();
        }
        private void Shape_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].SpriteShape = (OAM_Shape)Shape_ComboBox.SelectedValue;
            }
            Core_Write();
        }
        private void Size_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].SpriteSize = (OAM_Size)Size_ComboBox.SelectedValue;
            }
            Core_Write();
        }

        private void SheetX_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].SheetX = (Byte)SheetX_NumBox.Value;
            }
            Core_Write();
        }
        private void SheetY_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].SheetY = (Byte)SheetY_NumBox.Value;
            }
            Core_Write();
        }

        private void GFX_Mode_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].GFXMode = (OAM_GFXMode)GFX_Mode_ComboBox.SelectedValue;
            }
            Core_Write();
        }
        private void OBJ_Mode_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].OBJMode = (OAM_OBJMode)OBJ_Mode_ComboBox.SelectedValue;
            }
            Core_Write();
        }

        private void FullColors_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].FullColors = FullColors_CheckBox.Checked;
            }
            Core_Write();
        }
        private void DrawMosaic_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].DrawMosaic = DrawMosaic_CheckBox.Checked;
            }
            Core_Write();
        }
        private void Palette_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].Palette = (Byte)Palette_NumBox.Value;
            }
            Core_Write();
        }
        private void Priority_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].Priority = (Byte)Priority_NumBox.Value;
            }
            Core_Write();
        }

        private void FlipH_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].FlipH = FlipH_CheckBox.Checked;
            }
            Core_Write();
        }
        private void FlipV_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].FlipV = FlipV_CheckBox.Checked;
            }
            Core_Write();
        }



        private void Affine_Index_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Entry_ListBox.SelectedIndices)
            {
                Current[index].AffineIndex = Affine_Index_NumBox.Value;
            }
            Core_Write();
        }
        private void Affine_Ux_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Affine_ListBox.SelectedIndices)
            {
                Current.Affines[index].Ux = (Double)Affine_Ux_NumBox.Value;
            }
            Core_Write();
        }
        private void Affine_Vx_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Affine_ListBox.SelectedIndices)
            {
                Current.Affines[index].Uy = (Double)Affine_Uy_NumBox.Value;
            }
            Core_Write();
        }
        private void Affine_Uy_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Affine_ListBox.SelectedIndices)
            {
                Current.Affines[index].Vx = (Double)Affine_Vx_NumBox.Value;
            }
            Core_Write();
        }
        private void Affine_Vy_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            foreach (int index in Affine_ListBox.SelectedIndices)
            {
                Current.Affines[index].Vy = (Double)Affine_Vy_NumBox.Value;
            }
            Core_Write();
        }
    }
}
