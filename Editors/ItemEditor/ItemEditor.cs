using EmblemMagic.Components;
using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class ItemEditor : Editor
    {
        private StructFile Current;
        private StructFile StatBonus;

        ArrayFile ItemAttributes;
        GBA.Image ItemIcons;
        NumericUpDown[] StatBonus_Controls;

        public string CurrentEntry
        {
            get
            {
                byte entry = (byte)((EntryListBox.SelectedIndices.Count > 0) ?
                    EntryListBox.SelectedIndices[0] : 0);
                return GetEntryString(entry);
            }
        }
        public string GetEntryString(byte entry)
        {
            return "Item 0x" + Util.ByteToHex(entry) + " [" + EntryListBox.Items[entry].Text + "] - ";
        }

        public ItemEditor()
        {
            try
            {
                InitializeComponent();

                Current = new StructFile("Item Struct.txt");
                StatBonus = new StructFile("Item Stat Bonus Struct.txt");
                Core_LoadEntryListBox();
                Core_LoadStatBonusControls();

                Type_ByteArrayBox.Load("Weapon Type.txt");
                Rank_ByteArrayBox.Load("Weapon Rank.txt");
                ItemEffect_ByteArrayBox.Load("Item Effects.txt");
                WeaponEffect_ByteArrayBox.Load("Weapon Effects.txt");
                StatBonus_PointerArrayBox.Load("Item Stat Bonus Pointers.txt");
                Effective_PointerArrayBox.Load("Weapon Effectiveness Pointers.txt");

                ItemName_MagicButton.EditorToOpen    = "Text Editor";
                Description_MagicButton.EditorToOpen = "Text Editor";
                UseItemText_MagicButton.EditorToOpen = "Text Editor";

                ItemAttributes = new ArrayFile("Item Attribute Flags.txt");
                for (uint i = 0; i <= ItemAttributes.LastEntry; i++)
                {
                    Attribute_ListBox.Items.Add(ItemAttributes[i]);
                }

                if (Core.CurrentROM is FE6)
                {
                    Stat_Exp_Label.Enabled = false;
                    Stat_Exp_NumBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }



        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Current.EntryIndex = (EntryListBox.SelectedIndices.Count > 0) ?
                EntryListBox.SelectedIndices[0] + 1 : 0;
            
            Core_LoadValues();
            Core_LoadValues_ItemIcon();
            Core_LoadValues_Attributes();
            Core_LoadValues_StatBonuses();
            Core_LoadValues_WeaponEffectiveness();
        }

        void Core_LoadEntryListBox()
        {
            int width = 2;
            int height = 0x200;
            byte[] tileset = Core.ReadData(Core.GetPointer("Item Icon Tileset"), width * height * Tile.LENGTH);
            byte[] palette = Core.ReadData(Core.GetPointer("Item Icon Palette"), Palette.LENGTH);

            ItemIcons = new Tileset(tileset).ToImage(width, height, palette);

            ArrayFile items = new ArrayFile("Item List.txt");
            ImageList icons = new ImageList
            {
                ColorDepth = ColorDepth.Depth16Bit,
                TransparentColor = (System.Drawing.Color)(new GBA.Color((UInt16)(palette[0] | (palette[1] << 8))))
            };
            EntryListBox.SmallImageList = icons;
            using (var image = new System.Drawing.Bitmap(16, 16))
            {
                int offset = 0;
                string entry;
                uint index;
                for (int i = 1; i <= items.LastEntry; i++)
                {
                    index = Core.ReadByte(Current.GetAddress(i, "ItemIcon"));
                    offset = (int)(index * 16);
                    for (int y = 0; y < image.Height; y++)
                    for (int x = 0; x < image.Width; x++)
                    {
                        image.SetPixel(x, y, (System.Drawing.Color)ItemIcons.GetColor(x, offset + y));
                    }
                    entry = items[(uint)i];
                    icons.Images.Add(entry, new System.Drawing.Bitmap(image));
                    EntryListBox.Items.Add(entry, entry);
                }
            }
        }
        void Core_LoadStatBonusControls()
        {
            int length = StatBonus.Fields.Length;
            StatBonus_Controls = new NumericUpDown[length];
            StatBonus_LayoutPanel.RowCount = length / 2 + length % 2;
            Label label;
            int column = 0;
            int row = 0;
            for (int i = 0; i < length; i++, row++)
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
                    Text = StatBonus.Fields[i].Name + " :",
                    Anchor = AnchorStyles.Right,
                    AutoSize = true,
                };
                StatBonus_Controls[i] = new NumericUpDown()
                {
                    Name = StatBonus.Fields[i].Name,
                    Anchor = AnchorStyles.Left,
                    Maximum = (int)Math.Pow(2, StatBonus.Fields[i].Length << 3)
                };
                StatBonus_LayoutPanel.Controls.Add(label, column, row);
                StatBonus_LayoutPanel.Controls.Add(StatBonus_Controls[i], column + 1, row);
                StatBonus_Controls[i].ValueChanged += StatBonus_Control_ValueChanged;
            }
        }

        void Core_LoadValues()
        {
            EntryListBox.SelectedIndexChanged -= EntryListBox_SelectedIndexChanged;
            StatBonus_PointerArrayBox.ValueChanged -= StatBonus_PointerArrayBox_ValueChanged;
            Effective_PointerArrayBox.ValueChanged -= Effective_PointerArrayBox_ValueChanged;

            ItemName_ShortBox.ValueChanged -= ItemName_ShortBox_ValueChanged;
            Descripton_ShortBox.ValueChanged -= Descripton_ShortBox_ValueChanged;
            UseItemText_ShortBox.ValueChanged -= UseItemText_ShortBox_ValueChanged;
            ItemNumber_ByteBox.ValueChanged -= ItemNumber_ByteBox_ValueChanged;

            Type_ByteArrayBox.ValueChanged -= Type_ByteArrayBox_ValueChanged;
            Rank_ByteArrayBox.ValueChanged -= Rank_ByteArrayBox_ValueChanged;
            ItemIcon_ByteBox.ValueChanged -= ItemIcon_ByteBox_ValueChanged;

            Stat_Uses_NumBox.ValueChanged -= Stat_Uses_NumBox_ValueChanged;
            Stat_CostPerUse_NumBox.ValueChanged -= Stat_CostPerUse_NumBox_ValueChanged;
            Stat_Cost_Total_NumBox.ValueChanged -= Stat_Cost_Total_NumBox_ValueChanged;
            Stat_Exp_NumBox.ValueChanged -= Stat_Exp_NumBox_ValueChanged;
            Stat_Range_Min_NumBox.ValueChanged -= Stat_Range_Min_NumBox_ValueChanged;
            Stat_Range_Max_NumBox.ValueChanged -= Stat_Range_Max_NumBox_ValueChanged;
            Stat_Hit_NumBox.ValueChanged -= Stat_Hit_NumBox_ValueChanged;
            Stat_Crit_NumBox.ValueChanged -= Stat_Crit_NumBox_ValueChanged;
            Stat_Might_NumBox.ValueChanged -= Stat_Might_NumBox_ValueChanged;
            Stat_Weight_NumBox.ValueChanged -= Stat_Weight_NumBox_ValueChanged;

            ItemEffect_ByteArrayBox.ValueChanged -= ItemEffect_ByteArrayBox_ValueChanged;
            WeaponEffect_ByteArrayBox.ValueChanged -= WeaponEffect_ByteArrayBox_ValueChanged;

            try
            {
                if (EntryListBox.SelectedItems.Count == 1)
                {
                    ItemName_ShortBox.Value = Current["ItemName"];
                    Descripton_ShortBox.Value = Current["Description"];
                    UseItemText_ShortBox.Value = Current["UseItemText"];
                    ItemName_MagicButton.EntryToSelect = ItemName_ShortBox.Value;
                    Description_MagicButton.EntryToSelect = Descripton_ShortBox.Value;
                    UseItemText_MagicButton.EntryToSelect = UseItemText_ShortBox.Value;
                    ItemNumber_ByteBox.Value = Current["ItemNumber"];

                    StatBonus_PointerArrayBox.Value = Current["StatBonuses"];
                    Effective_PointerArrayBox.Value = Current["Effectiveness"];

                    Type_ByteArrayBox.Value = Current["WeaponType"];
                    Rank_ByteArrayBox.Value = Current["WeaponRank"];
                    ItemIcon_ByteBox.Value = Current["ItemIcon"];

                    Stat_Uses_NumBox.Value = Current["Uses"];
                    Stat_CostPerUse_NumBox.Value = Current["Cost"];
                    Stat_Cost_Total_NumBox.Value = Stat_Uses_NumBox.Value * Stat_CostPerUse_NumBox.Value;
                    Stat_Range_Min_NumBox.Value = (Current["Range"] & 0xF0) >> 4;
                    Stat_Range_Max_NumBox.Value = (Current["Range"] & 0x0F);
                    Stat_Might_NumBox.Value = Current["Might"];
                    Stat_Weight_NumBox.Value = Current["Weight"];
                    Stat_Hit_NumBox.Value = Current["Hit%"];
                    Stat_Crit_NumBox.Value = Current["Crit%"];
                    if (Stat_Exp_NumBox.Enabled)
                        Stat_Exp_NumBox.Value = Current["WeaponExp"];

                    ItemEffect_ByteArrayBox.Value = Current["ItemEffect"];
                    WeaponEffect_ByteArrayBox.Value = Current["WeaponEffect"];
                }
                else
                {
                    ItemName_ShortBox.Value = 0;
                    Descripton_ShortBox.Value = 0;
                    UseItemText_ShortBox.Value = 0;
                    ItemName_MagicButton.EntryToSelect = 0;
                    Description_MagicButton.EntryToSelect = 0;
                    UseItemText_MagicButton.EntryToSelect = 0;
                    ItemNumber_ByteBox.Value = 0;

                    StatBonus_PointerArrayBox.Value = new Pointer();
                    Effective_PointerArrayBox.Value = new Pointer();

                    Type_ByteArrayBox.Value = 0;
                    Rank_ByteArrayBox.Value = 0;
                    ItemIcon_ByteBox.Value = 0;

                    Stat_Uses_NumBox.Value = 0;
                    Stat_CostPerUse_NumBox.Value = 0;
                    Stat_Cost_Total_NumBox.Value = 0;
                    Stat_Range_Min_NumBox.Value = 0;
                    Stat_Range_Max_NumBox.Value = 0;
                    Stat_Might_NumBox.Value = 0;
                    Stat_Weight_NumBox.Value = 0;
                    Stat_Hit_NumBox.Value = 0;
                    Stat_Crit_NumBox.Value = 0;
                    Stat_Exp_NumBox.Value = 0;

                    ItemEffect_ByteArrayBox.Value = 0;
                    WeaponEffect_ByteArrayBox.Value = 0;

                    if (EntryListBox.SelectedItems.Count > 1)
                    {
                        ItemName_ShortBox.Text = "";
                        Descripton_ShortBox.Text = "";
                        UseItemText_ShortBox.Text = "";
                        ItemNumber_ByteBox.Text = "";

                        //StatBonus_PointerArrayBox.Value = new Pointer();
                        //Effective_PointerArrayBox.Value = new Pointer();

                        //Type_ByteArrayBox.Value = 0;
                        //Rank_ByteArrayBox.Value = 0;
                        ItemIcon_ByteBox.Text = "";

                        Stat_Uses_NumBox.Text = "";
                        Stat_CostPerUse_NumBox.Text = "";
                        Stat_Cost_Total_NumBox.Text = "";
                        Stat_Range_Min_NumBox.Text = "";
                        Stat_Range_Max_NumBox.Text = "";
                        Stat_Might_NumBox.Text = "";
                        Stat_Weight_NumBox.Text = "";
                        Stat_Hit_NumBox.Text = "";
                        Stat_Crit_NumBox.Text = "";
                        Stat_Exp_NumBox.Text = "";

                        //ItemEffect_ByteArrayBox.Value = 0;
                        //WeaponEffect_ByteArrayBox.Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the item values.", ex);
            }

            EntryListBox.SelectedIndexChanged += EntryListBox_SelectedIndexChanged;
            StatBonus_PointerArrayBox.ValueChanged += StatBonus_PointerArrayBox_ValueChanged;
            Effective_PointerArrayBox.ValueChanged += Effective_PointerArrayBox_ValueChanged;

            ItemName_ShortBox.ValueChanged += ItemName_ShortBox_ValueChanged;
            Descripton_ShortBox.ValueChanged += Descripton_ShortBox_ValueChanged;
            UseItemText_ShortBox.ValueChanged += UseItemText_ShortBox_ValueChanged;
            ItemNumber_ByteBox.ValueChanged += ItemNumber_ByteBox_ValueChanged;

            Type_ByteArrayBox.ValueChanged += Type_ByteArrayBox_ValueChanged;
            Rank_ByteArrayBox.ValueChanged += Rank_ByteArrayBox_ValueChanged;
            ItemIcon_ByteBox.ValueChanged += ItemIcon_ByteBox_ValueChanged;

            Stat_Uses_NumBox.ValueChanged += Stat_Uses_NumBox_ValueChanged;
            Stat_CostPerUse_NumBox.ValueChanged += Stat_CostPerUse_NumBox_ValueChanged;
            Stat_Cost_Total_NumBox.ValueChanged += Stat_Cost_Total_NumBox_ValueChanged;
            Stat_Exp_NumBox.ValueChanged += Stat_Exp_NumBox_ValueChanged;
            Stat_Range_Min_NumBox.ValueChanged += Stat_Range_Min_NumBox_ValueChanged;
            Stat_Range_Max_NumBox.ValueChanged += Stat_Range_Max_NumBox_ValueChanged;
            Stat_Hit_NumBox.ValueChanged += Stat_Hit_NumBox_ValueChanged;
            Stat_Crit_NumBox.ValueChanged += Stat_Crit_NumBox_ValueChanged;
            Stat_Might_NumBox.ValueChanged += Stat_Might_NumBox_ValueChanged;
            Stat_Weight_NumBox.ValueChanged += Stat_Weight_NumBox_ValueChanged;

            ItemEffect_ByteArrayBox.ValueChanged += ItemEffect_ByteArrayBox_ValueChanged;
            WeaponEffect_ByteArrayBox.ValueChanged += WeaponEffect_ByteArrayBox_ValueChanged;
        }
        void Core_LoadValues_ItemIcon()
        {
            try
            {
                if (EntryListBox.SelectedItems.Count == 1)
                {
                    GBA.Color[,] icon = ItemIcons.GetPixels(
                        new Rectangle(0, ItemIcon_ByteBox.Value * 16, 16, 16));
                    const int size = 32;
                    byte[] pixeldata = new byte[size * size];
                    int pixel;
                    int i = 0;
                    for (int y = 0; y < 16; y++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            pixel = ItemIcons.Colors.Find(icon[x, y]);
                            if (pixel == -1) throw new Exception("Pixel of unknown color..?");
                            pixeldata[i] = (byte)(pixel | (pixel << 4));
                            i++;
                        }
                        for (int x = 0; x < 16; x++)
                        {
                            pixeldata[i] = pixeldata[i - 16];
                            i++;
                        }
                    }
                    ItemIcon_ImageBox.Load(new GBA.Image(size, size, ItemIcons.Colors.ToBytes(false), pixeldata));
                }
                else
                {
                    ItemIcon_ImageBox.Reset();
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the item icon.", ex);
            }
        }
        void Core_LoadValues_Attributes()
        {
            Attribute_ListBox.ItemCheck -= Attribute_ListBox_ItemCheck;

            try
            {
                if (EntryListBox.SelectedItems.Count == 1)
                {
                    Pointer address = Current.GetAddress(Current.EntryIndex, "Attributes");
                    int index = 8;
                    int length = (int)(ItemAttributes.LastEntry + 1);
                    byte buffer = 0x00;
                    for (int i = 0; i < length; i++)
                    {
                        if (i % 8 == 0)
                        {
                            buffer = Core.ReadByte(address + (i / 8));
                            index = 8;
                        }
                        Attribute_ListBox.SetItemChecked(i, ((buffer >> --index) & 1) == 1);
                    }
                }
                else
                {
                    for (int i = 0; i <= ItemAttributes.LastEntry; i++)
                    {
                        Attribute_ListBox.SetItemChecked(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the item attribute flags.", ex);
            }

            Attribute_ListBox.ItemCheck += Attribute_ListBox_ItemCheck;
        }
        void Core_LoadValues_StatBonuses()
        {
            try
            {
                StatBonus.Address = StatBonus_PointerArrayBox.Value;
                if (StatBonus.Address == 0)
                {
                    for (int i = 0; i < StatBonus_Controls.Length; i++)
                    {
                        StatBonus_Controls[i].ValueChanged -= StatBonus_Control_ValueChanged;
                        StatBonus_Controls[i].Value = 0;
                        StatBonus_Controls[i].ValueChanged += StatBonus_Control_ValueChanged;
                    }
                }
                else
                {
                    for (int i = 0; i < StatBonus_Controls.Length; i++)
                    {
                        StatBonus_Controls[i].ValueChanged -= StatBonus_Control_ValueChanged;
                        StatBonus_Controls[i].Value = StatBonus[StatBonus_Controls[i].Name];
                        StatBonus_Controls[i].ValueChanged += StatBonus_Control_ValueChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the stat bonus values.", ex);
            }
        }
        void Core_LoadValues_WeaponEffectiveness()
        {
            try
            {
                for (int i = 0; i < Effective_LayoutPanel.Controls.Count; i++)
                {
                    Effective_LayoutPanel.Controls[i].Dispose();
                }
                Effective_LayoutPanel.Controls.Clear();

                Pointer address = Effective_PointerArrayBox.Value;
                if (address != new Pointer())
                {
                    ByteArrayBox control;
                    ArrayFile classes = new ArrayFile("Class List.txt");
                    Effective_LayoutPanel.SuspendLayout();
                    byte buffer;
                    byte index = 0;
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
                        Effective_LayoutPanel.Controls.Add(control);
                        control.Value = buffer;
                        control.ValueChanged += delegate (object sender, EventArgs e)
                        {
                            byte entry = Util.HexToByte(((ByteArrayBox)(((ByteBox)sender).Parent)).Name);
                            Core.WriteByte(this,
                                address + entry,
                                ((ByteBox)sender).Value,
                                "Weapon Effectiveness " + Effective_PointerArrayBox.Value +
                                " [" + Effective_PointerArrayBox.Text + "] - Class " + index + " changed");
                        };
                        index++;
                    }
                    while (buffer != 0x00);
                    Effective_LayoutPanel.ResumeLayout();
                    Effective_LayoutPanel.PerformLayout();
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the weapon effectiveness class list.", ex);
            }
        }



        private void EntryListBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (AwaitingUpdate)
                Core_Update();
        }
        private void EntryListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (AwaitingUpdate)
                Core_Update();
        }
        bool AwaitingUpdate = false;
        private void EntryListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            AwaitingUpdate = true;
        }



        private void StatBonus_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WritePointer(this,
                    Current.GetAddress(Current.EntryIndex, "StatBonuses"),
                    StatBonus_PointerArrayBox.Value,
                    CurrentEntry + "Stat Bonus repointed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WritePointer(this,
                        Current.GetAddress(entry + 1, "StatBonuses"),
                        StatBonus_PointerArrayBox.Value,
                        GetEntryString(entry) + "Stat Bonus repointed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Effective_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WritePointer(this,
                    Current.GetAddress(Current.EntryIndex, "Effectiveness"),
                    Effective_PointerArrayBox.Value,
                    CurrentEntry + "Effectiveness repointed");
            }
            else
            {
                Core.SuspendUpdate();
                for (byte entry = 0; entry < EntryListBox.SelectedItems.Count; entry++)
                {
                    Core.WritePointer(this,
                        Current.GetAddress(entry + 1, "Effectiveness"),
                        Effective_PointerArrayBox.Value,
                        GetEntryString(entry) + "Effectiveness repointed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }

        private void ItemName_ShortBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    Current.GetAddress(Current.EntryIndex, "ItemName"),
                    Util.UInt16ToBytes(ItemName_ShortBox.Value, true),
                    CurrentEntry + "Item Name changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        Current.GetAddress(entry + 1, "ItemName"),
                        Util.UInt16ToBytes(ItemName_ShortBox.Value, true),
                        GetEntryString(entry) + "Item Name changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Descripton_ShortBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    Current.GetAddress(Current.EntryIndex, "Description"),
                    Util.UInt16ToBytes(Descripton_ShortBox.Value, true),
                    CurrentEntry + "Description changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        Current.GetAddress(Current.EntryIndex, "Description"),
                        Util.UInt16ToBytes(Descripton_ShortBox.Value, true),
                        GetEntryString(entry) + "Description changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void UseItemText_ShortBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    Current.GetAddress(Current.EntryIndex, "UseItemText"),
                    Util.UInt16ToBytes(UseItemText_ShortBox.Value, true),
                    CurrentEntry + "'Use Item' Text changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        Current.GetAddress(entry + 1, "UseItemText"),
                        Util.UInt16ToBytes(UseItemText_ShortBox.Value, true),
                        GetEntryString(entry) + "'Use Item' Text changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void ItemNumber_ByteBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "ItemNumber"),
                    ItemNumber_ByteBox.Value,
                    CurrentEntry + "Item Number changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "ItemNumber"),
                        ItemNumber_ByteBox.Value,
                        GetEntryString(entry) + "Item Number changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }

        private void Type_ByteArrayBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "WeaponType"),
                    Type_ByteArrayBox.Value,
                    CurrentEntry + "Weapon Type changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "WeaponType"),
                        Type_ByteArrayBox.Value,
                        GetEntryString(entry) + "Weapon Type changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Rank_ByteArrayBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "WeaponRank"),
                    Rank_ByteArrayBox.Value,
                    CurrentEntry + "Weapon Rank changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "WeaponRank"),
                        Rank_ByteArrayBox.Value,
                        GetEntryString(entry) + "Weapon Rank changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void ItemIcon_ByteBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "ItemIcon"),
                    ItemIcon_ByteBox.Value,
                    CurrentEntry + "Icon changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "ItemIcon"),
                        ItemIcon_ByteBox.Value,
                        GetEntryString(entry) + "Icon changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }

        private void Stat_Uses_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "Uses"),
                    (byte)Stat_Uses_NumBox.Value,
                    CurrentEntry + "Item Uses changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "Uses"),
                        (byte)Stat_Uses_NumBox.Value,
                        GetEntryString(entry) + "Item Uses changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_CostPerUse_NumBox_ValueChanged(object sender, EventArgs e)
        {
            UInt16 value = (UInt16)Stat_CostPerUse_NumBox.Value;

            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    Current.GetAddress(Current.EntryIndex, "Cost"),
                    Util.UInt16ToBytes(value, true),
                    CurrentEntry + "Item Cost changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        Current.GetAddress(entry + 1, "Cost"),
                        Util.UInt16ToBytes(value, true),
                        GetEntryString(entry) + "Item Cost changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Cost_Total_NumBox_ValueChanged(object sender, EventArgs e)
        {
            UInt16 value = (UInt16)(Stat_Cost_Total_NumBox.Value / Stat_Uses_NumBox.Value);

            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteData(this,
                    Current.GetAddress(Current.EntryIndex, "Cost"),
                    Util.UInt16ToBytes(value, true),
                    CurrentEntry + "Item Cost changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteData(this,
                        Current.GetAddress(entry + 1, "Cost"),
                        Util.UInt16ToBytes(value, true),
                        GetEntryString(entry) + "Item Cost changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Range_Min_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "Range"),
                    (byte)(((int)Stat_Range_Min_NumBox.Value << 4) | (int)Stat_Range_Max_NumBox.Value),
                    CurrentEntry + "Range (min) changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "Range"),
                        (byte)(((int)Stat_Range_Min_NumBox.Value << 4) | (int)Stat_Range_Max_NumBox.Value),
                        GetEntryString(entry) + "Range (min) changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Range_Max_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "Range"),
                    (byte)(((int)Stat_Range_Min_NumBox.Value << 4) | (int)Stat_Range_Max_NumBox.Value),
                    CurrentEntry + "Range (max) changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "Range"),
                        (byte)(((int)Stat_Range_Min_NumBox.Value << 4) | (int)Stat_Range_Max_NumBox.Value),
                        GetEntryString(entry) + "Range (max) changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Hit_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "Hit%"),
                    (byte)Stat_Hit_NumBox.Value,
                    CurrentEntry + "Stat:Hit % changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "Hit%"),
                        (byte)Stat_Hit_NumBox.Value,
                        GetEntryString(entry) + "Stat:Hit % changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Crit_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "Crit%"),
                    (byte)Stat_Crit_NumBox.Value,
                    CurrentEntry + "Stat:Crit % changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "Crit%"),
                        (byte)Stat_Crit_NumBox.Value,
                        GetEntryString(entry) + "Stat:Crit % changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Might_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "Might"),
                    (byte)Stat_Might_NumBox.Value,
                    CurrentEntry + "Stat:Might changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "Might"),
                        (byte)Stat_Might_NumBox.Value,
                        GetEntryString(entry) + "Stat:Might changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Weight_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "Weight"),
                    (byte)Stat_Weight_NumBox.Value,
                    CurrentEntry + "Stat:Weight changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "Weight"),
                        (byte)Stat_Weight_NumBox.Value,
                        GetEntryString(entry) + "Stat:Weight changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void Stat_Exp_NumBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "WeaponExp"),
                    (byte)Stat_Exp_NumBox.Value,
                    CurrentEntry + "Weapon Exp changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "WeaponExp"),
                        (byte)Stat_Exp_NumBox.Value,
                        GetEntryString(entry) + "Weapon Exp changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }

        private void ItemEffect_ByteArrayBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "ItemEffect"),
                    ItemEffect_ByteArrayBox.Value,
                    CurrentEntry + "Item Effect changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "ItemEffect"),
                        ItemEffect_ByteArrayBox.Value,
                        GetEntryString(entry) + "Item Effect changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }
        private void WeaponEffect_ByteArrayBox_ValueChanged(object sender, EventArgs e)
        {
            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Core.WriteByte(this,
                    Current.GetAddress(Current.EntryIndex, "WeaponEffect"),
                    WeaponEffect_ByteArrayBox.Value,
                    CurrentEntry + "WeaponEffect changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Core.WriteByte(this,
                        Current.GetAddress(entry + 1, "WeaponEffect"),
                        WeaponEffect_ByteArrayBox.Value,
                        GetEntryString(entry) + "WeaponEffect changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }

        private void ItemIcon_ImageBox_Click(object sender, EventArgs e)
        {

        }

        private void Attribute_ListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            uint flag = (uint)e.Index;
            int index = (e.Index / 8);
            int bit = (7 - (e.Index % 8));

            if (EntryListBox.SelectedItems.Count == 0)
            {
                Program.ShowMessage("Please select one or several items.");
            }
            else if (EntryListBox.SelectedItems.Count == 1)
            {
                Pointer address = Current.GetAddress(Current.EntryIndex, "Attributes");
                Core.WriteByte(this,
                    address + index,
                    Core.ReadByte(address + index).SetBit(bit, (e.NewValue == CheckState.Checked)),
                    CurrentEntry + "Attribute flag " + flag + " [" + ItemAttributes[flag] + "] changed");
            }
            else
            {
                Core.SuspendUpdate();
                for (int i = 0; i < EntryListBox.SelectedItems.Count; i++)
                {
                    byte entry = (byte)EntryListBox.SelectedItems[i].Index;
                    Pointer address = Current.GetAddress(entry + 1, "Attributes");
                    Core.WriteByte(this,
                        address + index,
                        Core.ReadByte(address + index).SetBit(bit, (e.NewValue == CheckState.Checked)),
                        GetEntryString(entry) + "Attribute flag " + flag + " [" + ItemAttributes[flag] + "] changed");
                }
                Core.ResumeUpdate();
                Core.PerformUpdate();
            }
        }

        private void StatBonus_Control_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < StatBonus_Controls.Length; i++)
            {
                if (sender == StatBonus_Controls[i])
                {
                    Pointer address = StatBonus_PointerArrayBox.Value;
                    if (address == new Pointer())
                    {
                        Program.ShowMessage("Please select an item Stat Bonus pointer.");
                        Core_LoadValues_StatBonuses();
                    }
                    else
                    {
                        string entry = StatBonus_PointerArrayBox.Text;
                        Core.WriteByte(this,
                            address + i,
                            (byte)((NumericUpDown)sender).Value,
                            "Item Stat Bonus at " + address + " [" + entry + "] - " +
                            StatBonus_Controls[i].Name + " Bonus changed");
                    }
                    return;
                }
            }
        }
    }
}
