using System;
using System.Drawing;
using EmblemMagic.FireEmblem;
using System.Windows.Forms;
using GBA;
using Magic;
using Magic.Components;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class ItemEditor : Editor
    {
        private StructFile Current;
        private StructFile StatBonus;

        ArrayFile ItemAttributes;
        GBA.Image ItemIcons;
        NumericUpDown[] StatBonus_Controls;

        public String CurrentEntry
        {
            get
            {
                Byte entry = (Byte)((this.EntryListBox.SelectedIndices.Count > 0) ?
                    this.EntryListBox.SelectedIndices[0] : 0);
                return this.GetEntryString(entry);
            }
        }
        public String GetEntryString(Byte entry)
        {
            return "Item 0x" + Util.ByteToHex(entry) + " [" + this.EntryListBox.Items[entry].Text + "] - ";
        }

        public ItemEditor()
        {
            try
            {
                this.InitializeComponent();

                this.Current = new StructFile("Item Struct.txt");
                this.StatBonus = new StructFile("Item Stat Bonus Struct.txt");
                this.Core_LoadEntryListBox();
                this.Core_LoadStatBonusControls();

                this.Type_ByteArrayBox.Load("Weapon Type.txt");
                this.Rank_ByteArrayBox.Load("Weapon Rank.txt");
                this.ItemEffect_ByteArrayBox.Load("Item Effects.txt");
                this.WeaponEffect_ByteArrayBox.Load("Weapon Effects.txt");
                this.StatBonus_PointerArrayBox.Load("Item Stat Bonus Pointers.txt");
                this.Effective_PointerArrayBox.Load("Weapon Effectiveness Pointers.txt");

                this.ItemName_MagicButton.EditorToOpen    = "Text Editor";
                this.Description_MagicButton.EditorToOpen = "Text Editor";
                this.UseItemText_MagicButton.EditorToOpen = "Text Editor";

                this.ItemAttributes = new ArrayFile("Item Attribute Flags.txt");
                for (UInt32 i = 0; i <= this.ItemAttributes.LastEntry; i++)
                {
                    this.Attribute_ListBox.Items.Add(this.ItemAttributes[i]);
                }

                if (Core.App.Game is FE6)
                {
                    this.Stat_Exp_Label.Enabled = false;
                    this.Stat_Exp_NumBox.Enabled = false;
                }
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
            this.Current.EntryIndex = (this.EntryListBox.SelectedIndices.Count > 0) ?
                this.EntryListBox.SelectedIndices[0] + 1 : 0;

            this.Core_LoadValues();
            this.Core_LoadValues_ItemIcon();
            this.Core_LoadValues_Attributes();
            this.Core_LoadValues_StatBonuses();
            this.Core_LoadValues_WeaponEffectiveness();
        }

        void Core_LoadEntryListBox()
        {
            Int32 width = 2;
            Int32 height = 0x200;
            Byte[] tileset = Core.ReadData(Core.GetPointer("Item Icon Tileset"), width * height * Tile.LENGTH);
            Byte[] palette = Core.ReadData(Core.GetPointer("Item Icon Palette"), Palette.LENGTH);

            this.ItemIcons = new Tileset(tileset).ToImage(width, height, palette);

            ArrayFile items = new ArrayFile("Item List.txt");
            ImageList icons = new ImageList
            {
                ColorDepth = ColorDepth.Depth16Bit,
                TransparentColor = (System.Drawing.Color)(new GBA.Color((UInt16)(palette[0] | (palette[1] << 8))))
            };
            this.EntryListBox.SmallImageList = icons;
            using (var image = new System.Drawing.Bitmap(16, 16))
            {
                Int32 offset = 0;
                String entry;
                UInt32 index;
                for (Int32 i = 1; i <= items.LastEntry; i++)
                {
                    index = Core.ReadByte(this.Current.GetAddress(i, "ItemIcon"));
                    offset = (Int32)(index * 16);
                    for (Int32 y = 0; y < image.Height; y++)
                    for (Int32 x = 0; x < image.Width; x++)
                    {
                        image.SetPixel(x, y, (System.Drawing.Color)this.ItemIcons.GetColor(x, offset + y));
                    }
                    entry = items[(UInt32)i];
                    icons.Images.Add(entry, new System.Drawing.Bitmap(image));
                    this.EntryListBox.Items.Add(entry, entry);
                }
            }
        }
        void Core_LoadStatBonusControls()
        {
            Int32 length = this.StatBonus.Fields.Length;
            this.StatBonus_Controls = new NumericUpDown[length];
            this.StatBonus_LayoutPanel.RowCount = length / 2 + length % 2;
            Label label;
            Int32 column = 0;
            Int32 row = 0;
            for (Int32 i = 0; i < length; i++, row++)
            {
                switch (i)
                {
                    case 0: row = 0; column = 0; break; // HP
                    case 1: row = 1; column = 0; break; // Str
                    case 2: row = 2; column = 0; break; // Skl
                    case 3: row = 3; column = 0; break; // Spd
                    case 4: row = 1; column = 2; break; // Def
                    case 5: row = 2; column = 2; break; // Res
                    case 6: row = 0; column = 2; break; // Lck
                    case 7: row = 3; column = 2; break; // Mov
                    case 8: row = 4; column = 2; break; // Con
                }
                label = new Label()
                {
                    Text = this.StatBonus.Fields[i].Name + " :",
                    Anchor = AnchorStyles.Right,
                    AutoSize = true,
                };
                this.StatBonus_Controls[i] = new NumericUpDown()
                {
                    Name = this.StatBonus.Fields[i].Name,
                    Anchor = AnchorStyles.Left,
                    Maximum = (Int32)Math.Pow(2, this.StatBonus.Fields[i].Length << 3)
                };
                this.StatBonus_LayoutPanel.Controls.Add(label, column, row);
                this.StatBonus_LayoutPanel.Controls.Add(this.StatBonus_Controls[i], column + 1, row);
                this.StatBonus_Controls[i].ValueChanged += this.StatBonus_Control_ValueChanged;
            }
        }

        void Core_LoadValues()
        {
            this.EntryListBox.SelectedIndexChanged -= this.EntryListBox_SelectedIndexChanged;
            this.StatBonus_PointerArrayBox.ValueChanged -= this.StatBonus_PointerArrayBox_ValueChanged;
            this.Effective_PointerArrayBox.ValueChanged -= this.Effective_PointerArrayBox_ValueChanged;

            this.ItemName_ShortBox.ValueChanged -= this.ItemName_ShortBox_ValueChanged;
            this.Descripton_ShortBox.ValueChanged -= this.Descripton_ShortBox_ValueChanged;
            this.UseItemText_ShortBox.ValueChanged -= this.UseItemText_ShortBox_ValueChanged;
            this.ItemNumber_ByteBox.ValueChanged -= this.ItemNumber_ByteBox_ValueChanged;

            this.Type_ByteArrayBox.ValueChanged -= this.Type_ByteArrayBox_ValueChanged;
            this.Rank_ByteArrayBox.ValueChanged -= this.Rank_ByteArrayBox_ValueChanged;
            this.ItemIcon_ByteBox.ValueChanged -= this.ItemIcon_ByteBox_ValueChanged;

            this.Stat_Uses_NumBox.ValueChanged -= this.Stat_Uses_NumBox_ValueChanged;
            this.Stat_CostPerUse_NumBox.ValueChanged -= this.Stat_CostPerUse_NumBox_ValueChanged;
            this.Stat_Cost_Total_NumBox.ValueChanged -= this.Stat_Cost_Total_NumBox_ValueChanged;
            this.Stat_Exp_NumBox.ValueChanged -= this.Stat_Exp_NumBox_ValueChanged;
            this.Stat_Range_Min_NumBox.ValueChanged -= this.Stat_Range_Min_NumBox_ValueChanged;
            this.Stat_Range_Max_NumBox.ValueChanged -= this.Stat_Range_Max_NumBox_ValueChanged;
            this.Stat_Hit_NumBox.ValueChanged -= this.Stat_Hit_NumBox_ValueChanged;
            this.Stat_Crit_NumBox.ValueChanged -= this.Stat_Crit_NumBox_ValueChanged;
            this.Stat_Might_NumBox.ValueChanged -= this.Stat_Might_NumBox_ValueChanged;
            this.Stat_Weight_NumBox.ValueChanged -= this.Stat_Weight_NumBox_ValueChanged;

            this.ItemEffect_ByteArrayBox.ValueChanged -= this.ItemEffect_ByteArrayBox_ValueChanged;
            this.WeaponEffect_ByteArrayBox.ValueChanged -= this.WeaponEffect_ByteArrayBox_ValueChanged;

            try
            {
                if (this.EntryListBox.SelectedItems.Count == 1)
                {
                    this.ItemName_ShortBox.Value = this.Current["ItemName"];
                    this.Descripton_ShortBox.Value = this.Current["Description"];
                    this.UseItemText_ShortBox.Value = this.Current["UseItemText"];
                    this.ItemName_MagicButton.EntryToSelect = this.ItemName_ShortBox.Value;
                    this.Description_MagicButton.EntryToSelect = this.Descripton_ShortBox.Value;
                    this.UseItemText_MagicButton.EntryToSelect = this.UseItemText_ShortBox.Value;
                    this.ItemNumber_ByteBox.Value = this.Current["ItemNumber"];

                    this.StatBonus_PointerArrayBox.Value = this.Current["StatBonuses"];
                    this.Effective_PointerArrayBox.Value = this.Current["Effectiveness"];

                    this.Type_ByteArrayBox.Value = this.Current["WeaponType"];
                    this.Rank_ByteArrayBox.Value = this.Current["WeaponRank"];
                    this.ItemIcon_ByteBox.Value = this.Current["ItemIcon"];

                    this.Stat_Uses_NumBox.Value = this.Current["Uses"];
                    this.Stat_CostPerUse_NumBox.Value = this.Current["Cost"];
                    this.Stat_Cost_Total_NumBox.Value = this.Stat_Uses_NumBox.Value * this.Stat_CostPerUse_NumBox.Value;
                    this.Stat_Range_Min_NumBox.Value = (this.Current["Range"] & 0xF0) >> 4;
                    this.Stat_Range_Max_NumBox.Value = (this.Current["Range"] & 0x0F);
                    this.Stat_Might_NumBox.Value = this.Current["Might"];
                    this.Stat_Weight_NumBox.Value = this.Current["Weight"];
                    this.Stat_Hit_NumBox.Value = this.Current["Hit%"];
                    this.Stat_Crit_NumBox.Value = this.Current["Crit%"];
                    if (this.Stat_Exp_NumBox.Enabled)
                        this.Stat_Exp_NumBox.Value = this.Current["WeaponExp"];

                    this.ItemEffect_ByteArrayBox.Value = this.Current["ItemEffect"];
                    this.WeaponEffect_ByteArrayBox.Value = this.Current["WeaponEffect"];
                }
                else
                {
                    this.ItemName_ShortBox.Value = 0;
                    this.Descripton_ShortBox.Value = 0;
                    this.UseItemText_ShortBox.Value = 0;
                    this.ItemName_MagicButton.EntryToSelect = 0;
                    this.Description_MagicButton.EntryToSelect = 0;
                    this.UseItemText_MagicButton.EntryToSelect = 0;
                    this.ItemNumber_ByteBox.Value = 0;

                    this.StatBonus_PointerArrayBox.Value = new Pointer();
                    this.Effective_PointerArrayBox.Value = new Pointer();

                    this.Type_ByteArrayBox.Value = 0;
                    this.Rank_ByteArrayBox.Value = 0;
                    this.ItemIcon_ByteBox.Value = 0;

                    this.Stat_Uses_NumBox.Value = 0;
                    this.Stat_CostPerUse_NumBox.Value = 0;
                    this.Stat_Cost_Total_NumBox.Value = 0;
                    this.Stat_Range_Min_NumBox.Value = 0;
                    this.Stat_Range_Max_NumBox.Value = 0;
                    this.Stat_Might_NumBox.Value = 0;
                    this.Stat_Weight_NumBox.Value = 0;
                    this.Stat_Hit_NumBox.Value = 0;
                    this.Stat_Crit_NumBox.Value = 0;
                    this.Stat_Exp_NumBox.Value = 0;

                    this.ItemEffect_ByteArrayBox.Value = 0;
                    this.WeaponEffect_ByteArrayBox.Value = 0;

                    if (this.EntryListBox.SelectedItems.Count > 1)
                    {
                        this.ItemName_ShortBox.Text = "";
                        this.Descripton_ShortBox.Text = "";
                        this.UseItemText_ShortBox.Text = "";
                        this.ItemNumber_ByteBox.Text = "";

                        //StatBonus_PointerArrayBox.Value = new Pointer();
                        //Effective_PointerArrayBox.Value = new Pointer();

                        //Type_ByteArrayBox.Value = 0;
                        //Rank_ByteArrayBox.Value = 0;
                        this.ItemIcon_ByteBox.Text = "";

                        this.Stat_Uses_NumBox.Text = "";
                        this.Stat_CostPerUse_NumBox.Text = "";
                        this.Stat_Cost_Total_NumBox.Text = "";
                        this.Stat_Range_Min_NumBox.Text = "";
                        this.Stat_Range_Max_NumBox.Text = "";
                        this.Stat_Might_NumBox.Text = "";
                        this.Stat_Weight_NumBox.Text = "";
                        this.Stat_Hit_NumBox.Text = "";
                        this.Stat_Crit_NumBox.Text = "";
                        this.Stat_Exp_NumBox.Text = "";

                        //ItemEffect_ByteArrayBox.Value = 0;
                        //WeaponEffect_ByteArrayBox.Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the item values.", ex);
            }

            this.EntryListBox.SelectedIndexChanged += this.EntryListBox_SelectedIndexChanged;
            this.StatBonus_PointerArrayBox.ValueChanged += this.StatBonus_PointerArrayBox_ValueChanged;
            this.Effective_PointerArrayBox.ValueChanged += this.Effective_PointerArrayBox_ValueChanged;

            this.ItemName_ShortBox.ValueChanged += this.ItemName_ShortBox_ValueChanged;
            this.Descripton_ShortBox.ValueChanged += this.Descripton_ShortBox_ValueChanged;
            this.UseItemText_ShortBox.ValueChanged += this.UseItemText_ShortBox_ValueChanged;
            this.ItemNumber_ByteBox.ValueChanged += this.ItemNumber_ByteBox_ValueChanged;

            this.Type_ByteArrayBox.ValueChanged += this.Type_ByteArrayBox_ValueChanged;
            this.Rank_ByteArrayBox.ValueChanged += this.Rank_ByteArrayBox_ValueChanged;
            this.ItemIcon_ByteBox.ValueChanged += this.ItemIcon_ByteBox_ValueChanged;

            this.Stat_Uses_NumBox.ValueChanged += this.Stat_Uses_NumBox_ValueChanged;
            this.Stat_CostPerUse_NumBox.ValueChanged += this.Stat_CostPerUse_NumBox_ValueChanged;
            this.Stat_Cost_Total_NumBox.ValueChanged += this.Stat_Cost_Total_NumBox_ValueChanged;
            this.Stat_Exp_NumBox.ValueChanged += this.Stat_Exp_NumBox_ValueChanged;
            this.Stat_Range_Min_NumBox.ValueChanged += this.Stat_Range_Min_NumBox_ValueChanged;
            this.Stat_Range_Max_NumBox.ValueChanged += this.Stat_Range_Max_NumBox_ValueChanged;
            this.Stat_Hit_NumBox.ValueChanged += this.Stat_Hit_NumBox_ValueChanged;
            this.Stat_Crit_NumBox.ValueChanged += this.Stat_Crit_NumBox_ValueChanged;
            this.Stat_Might_NumBox.ValueChanged += this.Stat_Might_NumBox_ValueChanged;
            this.Stat_Weight_NumBox.ValueChanged += this.Stat_Weight_NumBox_ValueChanged;

            this.ItemEffect_ByteArrayBox.ValueChanged += this.ItemEffect_ByteArrayBox_ValueChanged;
            this.WeaponEffect_ByteArrayBox.ValueChanged += this.WeaponEffect_ByteArrayBox_ValueChanged;
        }
        void Core_LoadValues_ItemIcon()
        {
            try
            {
                if (this.EntryListBox.SelectedItems.Count == 1)
                {
                    GBA.Color[,] icon = this.ItemIcons.GetPixels(
                        new Rectangle(0, this.ItemIcon_ByteBox.Value * 16, 16, 16));
                    const Int32 size = 32;
                    Byte[] pixeldata = new Byte[size * size];
                    Int32 pixel;
                    Int32 i = 0;
                    for (Int32 y = 0; y < 16; y++)
                    {
                        for (Int32 x = 0; x < 16; x++)
                        {
                            pixel = this.ItemIcons.Colors.Find(icon[x, y]);
                            if (pixel == -1) throw new Exception("Pixel of unknown color..?");
                            pixeldata[i] = (Byte)(pixel | (pixel << 4));
                            i++;
                        }
                        for (Int32 x = 0; x < 16; x++)
                        {
                            pixeldata[i] = pixeldata[i - 16];
                            i++;
                        }
                    }
                    this.ItemIcon_ImageBox.Load(new GBA.Image(size, size, this.ItemIcons.Colors.ToBytes(false), pixeldata));
                }
                else
                {
                    this.ItemIcon_ImageBox.Reset();
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the item icon.", ex);
            }
        }
        void Core_LoadValues_Attributes()
        {
            this.Attribute_ListBox.ItemCheck -= this.Attribute_ListBox_ItemCheck;

            try
            {
                if (this.EntryListBox.SelectedItems.Count == 1)
                {
                    Pointer address = this.Current.GetAddress(this.Current.EntryIndex, "Attributes");
                    Int32 index = 8;
                    Int32 length = (Int32)(this.ItemAttributes.LastEntry + 1);
                    Byte buffer = 0x00;
                    for (Int32 i = 0; i < length; i++)
                    {
                        if (i % 8 == 0)
                        {
                            buffer = Core.ReadByte(address + (i / 8));
                            index = 8;
                        }
                        this.Attribute_ListBox.SetItemChecked(i, ((buffer >> --index) & 1) == 1);
                    }
                }
                else
                {
                    for (Int32 i = 0; i <= this.ItemAttributes.LastEntry; i++)
                    {
                        this.Attribute_ListBox.SetItemChecked(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the item attribute flags.", ex);
            }

            this.Attribute_ListBox.ItemCheck += this.Attribute_ListBox_ItemCheck;
        }
        void Core_LoadValues_StatBonuses()
        {
            try
            {
                this.StatBonus.Address = this.StatBonus_PointerArrayBox.Value;
                if (this.StatBonus.Address == 0)
                {
                    for (Int32 i = 0; i < this.StatBonus_Controls.Length; i++)
                    {
                        this.StatBonus_Controls[i].ValueChanged -= this.StatBonus_Control_ValueChanged;
                        this.StatBonus_Controls[i].Value = 0;
                        this.StatBonus_Controls[i].ValueChanged += this.StatBonus_Control_ValueChanged;
                    }
                }
                else
                {
                    for (Int32 i = 0; i < this.StatBonus_Controls.Length; i++)
                    {
                        this.StatBonus_Controls[i].ValueChanged -= this.StatBonus_Control_ValueChanged;
                        this.StatBonus_Controls[i].Value = this.StatBonus[this.StatBonus_Controls[i].Name];
                        this.StatBonus_Controls[i].ValueChanged += this.StatBonus_Control_ValueChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the stat bonus values.", ex);
            }
        }
        void Core_LoadValues_WeaponEffectiveness()
        {
            try
            {
                for (Int32 i = 0; i < this.Effective_LayoutPanel.Controls.Count; i++)
                {
                    this.Effective_LayoutPanel.Controls[i].Dispose();
                }
                this.Effective_LayoutPanel.Controls.Clear();

                Pointer address = this.Effective_PointerArrayBox.Value;
                if (address != new Pointer())
                {
                    ByteArrayBox control;
                    ArrayFile classes = new ArrayFile("Class List.txt");
                    this.Effective_LayoutPanel.SuspendLayout();
                    Byte buffer;
                    Byte index = 0;
                    do
                    {
                        buffer = Core.ReadByte(address + index);
                        control = new ByteArrayBox()
                        {
                            Location = new Point(0, index * 30),
                            Size = new Size(180, 20),
                            Name = Util.ByteToHex(index)
                        };
                        control.Load(classes);
                        this.Effective_LayoutPanel.Controls.Add(control);
                        control.Value = buffer;
                        control.ValueChanged += delegate (Object sender, EventArgs e)
                        {
                            Byte entry = Util.HexToByte(((ByteArrayBox)(((ByteBox)sender).Parent)).Name);
                            Core.WriteByte(this,
                                address + entry,
                                ((ByteBox)sender).Value,
                                "Weapon Effectiveness " + this.Effective_PointerArrayBox.Value +
                                " [" + this.Effective_PointerArrayBox.Text + "] - Class " + index + " changed");
                        };
                        index++;
                    }
                    while (buffer != 0x00);
                    this.Effective_LayoutPanel.ResumeLayout();
                    this.Effective_LayoutPanel.PerformLayout();
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the weapon effectiveness class list.", ex);
            }
        }



        private void EntryListBox_MouseUp(Object sender, MouseEventArgs e)
        {
            if (this.AwaitingUpdate)
                this.Core_Update();
        }
        private void EntryListBox_KeyUp(Object sender, KeyEventArgs e)
        {
            if (this.AwaitingUpdate)
                this.Core_Update();
        }
        Boolean AwaitingUpdate = false;
        private void EntryListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            this.AwaitingUpdate = true;
            this.ItemIcon_MagicButton.Enabled = (this.EntryListBox.SelectedIndices.Count > 0);
        }



        private void StatBonus_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WritePointer(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "StatBonuses"),
                    this.StatBonus_PointerArrayBox.Value,
                    this.CurrentEntry + "Stat Bonus repointed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WritePointer(this,
                        this.Current.GetAddress(entry + 1, "StatBonuses"),
                        this.StatBonus_PointerArrayBox.Value,
                        this.GetEntryString(entry) + "Stat Bonus repointed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Effective_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WritePointer(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Effectiveness"),
                    this.Effective_PointerArrayBox.Value,
                    this.CurrentEntry + "Effectiveness repointed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Byte entry = 0; entry < this.EntryListBox.SelectedItems.Count; entry++)
                {
                    Core.WritePointer(this,
                        this.Current.GetAddress(entry + 1, "Effectiveness"),
                        this.Effective_PointerArrayBox.Value,
                        this.GetEntryString(entry) + "Effectiveness repointed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }

        private void ItemName_ShortBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "ItemName"),
                    Util.UInt16ToBytes(this.ItemName_ShortBox.Value, true),
                    this.CurrentEntry + "Item Name changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        this.Current.GetAddress(entry + 1, "ItemName"),
                        Util.UInt16ToBytes(this.ItemName_ShortBox.Value, true),
                        this.GetEntryString(entry) + "Item Name changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Descripton_ShortBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Description"),
                    Util.UInt16ToBytes(this.Descripton_ShortBox.Value, true),
                    this.CurrentEntry + "Description changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        this.Current.GetAddress(this.Current.EntryIndex, "Description"),
                        Util.UInt16ToBytes(this.Descripton_ShortBox.Value, true),
                        this.GetEntryString(entry) + "Description changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void UseItemText_ShortBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "UseItemText"),
                    Util.UInt16ToBytes(this.UseItemText_ShortBox.Value, true),
                    this.CurrentEntry + "'Use Item' Text changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        this.Current.GetAddress(entry + 1, "UseItemText"),
                        Util.UInt16ToBytes(this.UseItemText_ShortBox.Value, true),
                        this.GetEntryString(entry) + "'Use Item' Text changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void ItemNumber_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "ItemNumber"),
                    this.ItemNumber_ByteBox.Value,
                    this.CurrentEntry + "Item Number changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "ItemNumber"),
                        this.ItemNumber_ByteBox.Value,
                        this.GetEntryString(entry) + "Item Number changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }

        private void Type_ByteArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "WeaponType"),
                    this.Type_ByteArrayBox.Value,
                    this.CurrentEntry + "Weapon Type changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "WeaponType"),
                        this.Type_ByteArrayBox.Value,
                        this.GetEntryString(entry) + "Weapon Type changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Rank_ByteArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "WeaponRank"),
                    this.Rank_ByteArrayBox.Value,
                    this.CurrentEntry + "Weapon Rank changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "WeaponRank"),
                        this.Rank_ByteArrayBox.Value,
                        this.GetEntryString(entry) + "Weapon Rank changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void ItemIcon_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "ItemIcon"),
                    this.ItemIcon_ByteBox.Value,
                    this.CurrentEntry + "Icon changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "ItemIcon"),
                        this.ItemIcon_ByteBox.Value,
                        this.GetEntryString(entry) + "Icon changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }

        private void Stat_Uses_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Uses"),
                    (Byte)this.Stat_Uses_NumBox.Value,
                    this.CurrentEntry + "Item Uses changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "Uses"),
                        (Byte)this.Stat_Uses_NumBox.Value,
                        this.GetEntryString(entry) + "Item Uses changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_CostPerUse_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            UInt16 value = (UInt16)this.Stat_CostPerUse_NumBox.Value;

            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Cost"),
                    Util.UInt16ToBytes(value, true),
                    this.CurrentEntry + "Item Cost changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        this.Current.GetAddress(entry + 1, "Cost"),
                        Util.UInt16ToBytes(value, true),
                        this.GetEntryString(entry) + "Item Cost changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Cost_Total_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            UInt16 value = (UInt16)(this.Stat_Cost_Total_NumBox.Value / this.Stat_Uses_NumBox.Value);

            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Cost"),
                    Util.UInt16ToBytes(value, true),
                    this.CurrentEntry + "Item Cost changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        this.Current.GetAddress(entry + 1, "Cost"),
                        Util.UInt16ToBytes(value, true),
                        this.GetEntryString(entry) + "Item Cost changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Range_Min_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Range"),
                    (Byte)(((Int32)this.Stat_Range_Min_NumBox.Value << 4) | (Int32)this.Stat_Range_Max_NumBox.Value),
                    this.CurrentEntry + "Range (min) changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "Range"),
                        (Byte)(((Int32)this.Stat_Range_Min_NumBox.Value << 4) | (Int32)this.Stat_Range_Max_NumBox.Value),
                        this.GetEntryString(entry) + "Range (min) changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Range_Max_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Range"),
                    (Byte)(((Int32)this.Stat_Range_Min_NumBox.Value << 4) | (Int32)this.Stat_Range_Max_NumBox.Value),
                    this.CurrentEntry + "Range (max) changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "Range"),
                        (Byte)(((Int32)this.Stat_Range_Min_NumBox.Value << 4) | (Int32)this.Stat_Range_Max_NumBox.Value),
                        this.GetEntryString(entry) + "Range (max) changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Hit_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Hit%"),
                    (Byte)this.Stat_Hit_NumBox.Value,
                    this.CurrentEntry + "Stat:Hit % changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "Hit%"),
                        (Byte)this.Stat_Hit_NumBox.Value,
                        this.GetEntryString(entry) + "Stat:Hit % changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Crit_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Crit%"),
                    (Byte)this.Stat_Crit_NumBox.Value,
                    this.CurrentEntry + "Stat:Crit % changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "Crit%"),
                        (Byte)this.Stat_Crit_NumBox.Value,
                        this.GetEntryString(entry) + "Stat:Crit % changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Might_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Might"),
                    (Byte)this.Stat_Might_NumBox.Value,
                    this.CurrentEntry + "Stat:Might changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "Might"),
                        (Byte)this.Stat_Might_NumBox.Value,
                        this.GetEntryString(entry) + "Stat:Might changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Weight_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "Weight"),
                    (Byte)this.Stat_Weight_NumBox.Value,
                    this.CurrentEntry + "Stat:Weight changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "Weight"),
                        (Byte)this.Stat_Weight_NumBox.Value,
                        this.GetEntryString(entry) + "Stat:Weight changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void Stat_Exp_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "WeaponExp"),
                    (Byte)this.Stat_Exp_NumBox.Value,
                    this.CurrentEntry + "Weapon Exp changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "WeaponExp"),
                        (Byte)this.Stat_Exp_NumBox.Value,
                        this.GetEntryString(entry) + "Weapon Exp changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }

        private void ItemEffect_ByteArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "ItemEffect"),
                    this.ItemEffect_ByteArrayBox.Value,
                    this.CurrentEntry + "Item Effect changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "ItemEffect"),
                        this.ItemEffect_ByteArrayBox.Value,
                        this.GetEntryString(entry) + "Item Effect changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }
        private void WeaponEffect_ByteArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    this.Current.GetAddress(this.Current.EntryIndex, "WeaponEffect"),
                    this.WeaponEffect_ByteArrayBox.Value,
                    this.CurrentEntry + "WeaponEffect changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        this.Current.GetAddress(entry + 1, "WeaponEffect"),
                        this.WeaponEffect_ByteArrayBox.Value,
                        this.GetEntryString(entry) + "WeaponEffect changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }

        private void ItemIcon_ImageBox_Click(Object sender, EventArgs e)
        {

        }

        private void Attribute_ListBox_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            UInt32 flag = (UInt32)e.Index;
            Int32 index = (e.Index / 8);
            Int32 bit = (7 - (e.Index % 8));

            if (this.EntryListBox.SelectedItems.Count == 0)
            {
                UI.ShowMessage("Please select one or several items.");
            }
            else if (this.EntryListBox.SelectedItems.Count == 1)
            {
                Pointer address = this.Current.GetAddress(this.Current.EntryIndex, "Attributes");
                Core.WriteByte(this,
                    address + index,
                    Core.ReadByte(address + index).SetBit(bit, (e.NewValue == CheckState.Checked)),
                    this.CurrentEntry + "Attribute flag " + flag + " [" + this.ItemAttributes[flag] + "] changed");
            }
            else
            {
                UI.SuspendUpdate();
                for (Int32 i = 0; i < this.EntryListBox.SelectedItems.Count; i++)
                {
                    Byte entry = (Byte)this.EntryListBox.SelectedItems[i].Index;
                    Pointer address = this.Current.GetAddress(entry + 1, "Attributes");
                    Core.WriteByte(this,
                        address + index,
                        Core.ReadByte(address + index).SetBit(bit, (e.NewValue == CheckState.Checked)),
                        this.GetEntryString(entry) + "Attribute flag " + flag + " [" + this.ItemAttributes[flag] + "] changed");
                }
                UI.ResumeUpdate();
                UI.PerformUpdate();
            }
        }

        private void StatBonus_Control_ValueChanged(Object sender, EventArgs e)
        {
            for (Int32 i = 0; i < this.StatBonus_Controls.Length; i++)
            {
                if (sender == this.StatBonus_Controls[i])
                {
                    Pointer address = this.StatBonus_PointerArrayBox.Value;
                    if (address == new Pointer())
                    {
                        UI.ShowMessage("Please select an item Stat Bonus pointer.");
                        this.Core_LoadValues_StatBonuses();
                    }
                    else
                    {
                        String entry = this.StatBonus_PointerArrayBox.Text;
                        Core.WriteByte(this,
                            address + i,
                            (Byte)((NumericUpDown)sender).Value,
                            "Item Stat Bonus at " + address + " [" + entry + "] - " +
                            this.StatBonus_Controls[i].Name + " Bonus changed");
                    }
                    return;
                }
            }
        }

        private void ItemIcon_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            if (this.EntryListBox.SelectedIndices.Count == 1)
            {
                editor.Core_SetEntry(2, 2,
                    Core.GetPointer("Item Icon Palette"), false,
                    Core.GetPointer("Item Icon Tileset") + GBA.Tile.LENGTH * 4 * this.ItemIcon_ByteBox.Value, false);
            }
            else
            {
                Int32 item_icon_count = 0;
                if (Core.App.Game is FE6) item_icon_count = 160;
                if (Core.App.Game is FE7) item_icon_count = 192;
                if (Core.App.Game is FE8) item_icon_count = 224;
                editor.Core_SetEntry(2, item_icon_count * 2,
                    Core.GetPointer("Item Icon Palette"), false,
                    Core.GetPointer("Item Icon Tileset"), false);
            }

            Program.Core.Core_OpenEditor(editor);
        }
    }
}
