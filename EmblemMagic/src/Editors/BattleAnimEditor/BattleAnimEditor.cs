using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Compression;
using EmblemMagic.FireEmblem;
using EmblemMagic.Properties;
using GBA;
using Magic;
using Magic.Components;
using Magic.Compression;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class BattleAnimEditor : Editor
    {
        /// <summary>
        /// The image to show in the Imagebox
        /// </summary>
        BattleAnimation CurrentAnim { get; set; }
        /// <summary>
        /// The current battle animation struct in the array
        /// </summary>
        StructFile Current { get; set; }
        /// <summary>
        /// The array of character palettes that battle animations can be drawn with
        /// </summary>
        StructFile Palettes { get; set; }
        /// <summary>
        /// The default palettes for this battle animation (blue, red, green and gray)
        /// </summary>
        Palette[] DefaultPalette { get; set; }
        /// <summary>
        /// The current character palette from the character palette array elsewhere in the ROM
        /// </summary>
        Palette[] CharacterPalette { get; set; }
        /// <summary>
        /// The list of frames for the current anim mode - redirects to actual frame numbers
        /// </summary>
        List<Byte>[] Frames;
        /// <summary>
        /// The list of frame durations (in 60ths/second) for the current anim mode
        /// </summary>
        List<Int32>[] Durations;
        /// <summary>
        /// The array of currently existing controls to edit battle anim associations
        /// </summary>
        Control[] Item_Controls;



        /// <summary>
        /// Gets a string of the current byte index of battle anim in the array
        /// </summary>
        String CurrentEntry
        {
            get
            {
                return "Battle Anim 0x" + Util.ByteToHex(this.Entry_ArrayBox.Value) + " [" + this.Entry_ArrayBox.Text + "] - ";
            }
        }
        /// <summary>
        /// Gets the number of the current frame being viewed, irrespective of the 'view all frames' option
        /// </summary>
        Byte CurrentFrame
        {
            get
            {
                return (this.View_AllFrames.Checked) ?
                    this.Frame_ByteBox.Value :
                    this.Frames[this.CurrentMode][this.Frame_ByteBox.Value];
            }
        }
        /// <summary>
        /// The index of the currently selected animation mode, irrespective of the "view 2layer modes as one" option
        /// </summary>
        Int32 CurrentMode
        {
            get
            {
                if (this.View_Layered.Checked)
                {
                    switch (this.Anim_Mode_ListBox.SelectedIndex)
                    {
                        case 0: return 0;
                        case 1: return 2;
                        case 2: case 3: case 4: case 5: case 6:
                            return this.Anim_Mode_ListBox.SelectedIndex + 2;
                        case 7: return 10;
                        case 8: return 11;
                        default: return -1;
                    }
                }
                else return this.Anim_Mode_ListBox.SelectedIndex;
            }
        }
        /// <summary>
        /// Gets the current palette, depending on whether default palettes or character palettes is checked
        /// </summary>
        Palette CurrentPalette
        {
            get
            {
                return (this.Palette_Default_Button.Checked) ?
                    this.DefaultPalette[this.Palette_Default_ArrayBox.Value] :
                    this.CharacterPalette[this.Palette_Character_Current_ArrayBox.Value];
            }
        }
        /// <summary>
        /// Gets a string of the currently selected character plaette
        /// </summary>
        String CurrentPaletteEntry
        {
            get
            {
                return "Character Palette 0x" + this.Palette_Character_ArrayBox.Value + " [" + this.Palette_Character_ArrayBox.Text + "] - ";
            }
        }
        /// <summary>
        /// Gets a string of the current class/item battle anim association entry
        /// </summary>
        String CurrentItemPointerEntry(Int32 word)
        {
            return "Battle Anim association at " + this.Item_PointerArrayBox.Value +
                                            " [" + this.Item_PointerArrayBox.Text + "], word " + word + " - ";
        }



        public BattleAnimEditor()
        {
            try
            {
                this.InitializeComponent();

                this.Entry_ArrayBox.Load("Battle Animation List.txt");
                this.Current = new StructFile("Battle Animation Struct.txt");
                this.Current.Address = Core.GetPointer("Battle Animation Array");

                this.Item_PointerArrayBox.Load("Battle Animation Pointers.txt");

                this.Palette_Default_ArrayBox.Load("Battle Animation Palettes.txt");
                this.Palette_Character_ArrayBox.Load("Character Palettes.txt");
                this.Palette_Character_Current_ArrayBox.Load("Battle Animation Character Palettes.txt");
                this.Palettes = new StructFile("Character Palette Struct.txt");
                this.Palettes.Address = Core.GetPointer("Character Palette Array");

                this.AnimCodeBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                this.Anim_Name_TextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                this.Palette_Character_TextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);

                this.Anim_Mode_ListBox.DataSource = BattleAnimation.Modes;
                this.Anim_Mode_ListBox.SelectedIndexChanged += this.AnimListBox_SelectedIndexChanged;

                this.AnimCodeBox.AddSyntax("#.*", SystemColors.ControlDark);
                this.AnimCodeBox.AddSyntax(@"(a|b|c|d|f|end)(?=[0-9a-fA-F][0-9a-fA-F])", SystemColors.Highlight, FontStyle.Bold);
                this.AnimCodeBox.AddSyntax(@"(end)", SystemColors.Highlight, FontStyle.Bold);
                this.AnimCodeBox.AddSyntax(@"(?<=(a|b|c|d|f))([0-9a-fA-F][0-9a-fA-F])", System.Drawing.Color.SlateBlue);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        override public void Core_SetEntry(UInt32 entry)
        {
            this.Item_PointerArrayBox.Value = new Pointer(entry);
            if (entry != 0)
                this.Entry_ArrayBox.Value = ((ByteArrayBox)this.Item_LayoutPanel.Controls[0].Controls[1]).Value;
        }
        override public void Core_OnOpen()
        {
            this.Entry_ArrayBox.ValueChanged -= this.EntryArrayBox_ValueChanged;
            this.Entry_ArrayBox.Value = 1;
            this.Entry_ArrayBox.ValueChanged += this.EntryArrayBox_ValueChanged;

            this.Palette_Character_ArrayBox.ValueChanged -= this.Palette_Character_ArrayBox_ValueChanged;
            this.Palette_Character_ArrayBox.Value = 1;
            this.Palette_Character_ArrayBox.ValueChanged += this.Palette_Character_ArrayBox_ValueChanged;

            this.Core_Update();
        }
        override public void Core_Update()
        {
            this.Current.EntryIndex = this.Entry_ArrayBox.Value - 1;
            this.Palettes.EntryIndex = this.Palette_Character_ArrayBox.Value - 1;

            this.Core_LoadAnimation();
            this.Core_LoadPalettes();
            this.Core_LoadAnimCode();
            this.Core_LoadValues();
            this.Core_LoadPaletteValues();
            this.Core_LoadItemValues();

            this.Core_UpdatePalettes();
            this.Core_UpdateImageBox();
        }

        void Core_LoadAnimation()
        {
            try
            {
                Byte[] animdata = Core.ReadData((Pointer)this.Current["AnimData"], 0);
                Byte[] oam_L = Core.ReadData((Pointer)this.Current["OAM_Left"], 0);
                Byte[] oam_R = Core.ReadData((Pointer)this.Current["OAM_Right"], 0);
                UInt32[] sections = new UInt32[12];
                Byte[] buffer;
                for (Int32 i = 0; i < sections.Length; i++)
                {
                    buffer = Core.ReadData((Pointer)this.Current["Sections"] + i * 4, 4);
                    sections[i] = Util.BytesToUInt32(buffer, true);
                }

                this.CurrentAnim = new BattleAnimation(sections, animdata, oam_L, oam_R);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the animation.", ex);
                this.CurrentAnim = null;
            }
        }
        void Core_LoadPalettes()
        {
            try
            {
                Pointer address;
                Boolean is_uncompressed;
                Palette palettes;
                Int32 paletteAmount;

                paletteAmount = 4;
                address = (Pointer)this.Current["Palettes"];
                is_uncompressed = (address > 0x80000000);
                if (address >= 0x80000000)
                    address -= 0x80000000;
                palettes = Core.ReadPalette(address, (is_uncompressed ? Palette.LENGTH : 0) * paletteAmount);
                this.DefaultPalette = Palette.Split(Palette.Opacify(palettes), paletteAmount);

                paletteAmount = 5;
                address = (Pointer)this.Palettes["Address"];
                is_uncompressed = (address > 0x80000000);
                if (address >= 0x80000000)
                    address -= 0x80000000;
                palettes = Core.ReadPalette(address, (is_uncompressed ? Palette.LENGTH : 0) * paletteAmount);
                this.CharacterPalette = Palette.Split(Palette.Opacify(palettes), paletteAmount);

                this.Palette_PaletteBox.Load(this.CurrentPalette);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the palettes.", ex);
            }
        }
        void Core_LoadAnimCode()
        {
            if (this.CurrentAnim == null)
                return;
            List<Tuple<UInt32, OAM>> not_used = null; Int32[] not_used_either = null;
            String result = BattleAnimation.GetAnimationCode(
                    this.CurrentAnim.AnimCode,
                    this.CurrentMode,
                    this.View_Layered.Checked,
                    this.View_AllAnimCode.Checked,
                    ref not_used, ref not_used_either);

            this.AnimCodeBox.TextChanged -= this.AnimCode_TextBox_TextChanged;
            this.AnimCodeBox.Text = result;
            this.AnimCodeBox.TextChanged -= this.AnimCode_TextBox_TextChanged;

            String[] animcode;
            this.Frames = new List<Byte>[BattleAnimation.MODES];
            this.Durations = new List<Int32>[BattleAnimation.MODES];
            for (Int32 mode = 0; mode < this.Frames.Length; mode++)
            {
                this.Frames[mode] = new List<Byte>();
                this.Durations[mode] = new List<Int32>();
                animcode = this.CurrentAnim.AnimCode[mode];

                for (Int32 i = 0; i < animcode.Length; i++)
                {
                    if (animcode[i][0] >= '0' && animcode[i][0] <= '9')
                    {
                        this.Durations[mode].Add(Int32.Parse(animcode[i].Substring(0, animcode[i].IndexOf(' '))));
                        for (Int32 j = 0; j < animcode[i].Length; j++)
                        {
                            if (animcode[i][j] == 'f' || animcode[i][j] == 'F')
                            {
                                this.Frames[mode].Add(Util.HexToByte(animcode[i].Substring(j + 1, 2)));
                                break;
                            }
                        }
                    }
                }
            }   // Create the frame array for all the animation modes of the current anim

            if (this.View_AllFrames.Checked)
                this.Frame_ByteBox.Maximum = Math.Max(0, this.CurrentAnim.Frames.Length - 1);
            else this.Frame_ByteBox.Maximum = Math.Max(0, this.Frames[this.CurrentMode].Count - 1);
        }
        void Core_LoadValues()
        {
            this.Anim_Name_TextBox.TextChanged -= this.Anim_NameTextBox_TextChanged;
            this.Sections_PointerBox.ValueChanged -= this.SectionsPointerBox_ValueChanged;
            this.AnimData_PointerBox.ValueChanged -= this.AnimDataPointerBox_ValueChanged;
            this.OAM_R_PointerBox.ValueChanged -= this.OAM_R_PointerBox_ValueChanged;
            this.OAM_L_PointerBox.ValueChanged -= this.OAM_L_PointerBox_ValueChanged;
            this.Palette_Default_PointerBox.ValueChanged -= this.Palette_Default_PointerBox_ValueChanged;

            try
            {
                this.Anim_Name_TextBox.Text = (String)this.Current["Name"];
                this.Sections_PointerBox.Value        = (Pointer)this.Current["Sections"];
                this.AnimData_PointerBox.Value        = (Pointer)this.Current["AnimData"];
                this.OAM_R_PointerBox.Value           = (Pointer)this.Current["OAM_Right"];
                this.OAM_L_PointerBox.Value           = (Pointer)this.Current["OAM_Left"];
                this.Palette_Default_PointerBox.Value = (Pointer)this.Current["Palettes"];
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);

                this.Anim_Name_TextBox.Text = "";
                this.Sections_PointerBox.Value = new Pointer();
                this.AnimData_PointerBox.Value = new Pointer();
                this.OAM_L_PointerBox.Value = new Pointer();
                this.OAM_R_PointerBox.Value = new Pointer();
                this.Palette_Default_PointerBox.Value = new Pointer();
            }

            this.Anim_Name_TextBox.TextChanged += this.Anim_NameTextBox_TextChanged;
            this.Sections_PointerBox.ValueChanged += this.SectionsPointerBox_ValueChanged;
            this.AnimData_PointerBox.ValueChanged += this.AnimDataPointerBox_ValueChanged;
            this.OAM_R_PointerBox.ValueChanged += this.OAM_R_PointerBox_ValueChanged;
            this.OAM_L_PointerBox.ValueChanged += this.OAM_L_PointerBox_ValueChanged;
            this.Palette_Default_PointerBox.ValueChanged += this.Palette_Default_PointerBox_ValueChanged;
        }
        void Core_LoadPaletteValues()
        {
            this.Palette_Character_PointerBox.ValueChanged -= this.Palette_Character_PointerBox_ValueChanged;
            this.Palette_Character_TextBox.TextChanged -= this.Palette_Character_TextBox_TextChanged;

            try
            {
                Pointer address = (Pointer)this.Palettes["Address"];
                if (address >= 0x80000000)
                    address -= 0x80000000;
                this.Palette_Character_PointerBox.Value = address;
                this.Palette_Character_TextBox.Text = (String)this.Palettes["Name"];
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the character palette values.", ex);

                this.Palette_Character_PointerBox.Value = new Pointer();
                this.Palette_Character_TextBox.Text = "";
            }

            this.Palette_Character_PointerBox.ValueChanged += this.Palette_Character_PointerBox_ValueChanged;
            this.Palette_Character_TextBox.TextChanged += this.Palette_Character_TextBox_TextChanged;
        }
        void Core_LoadItemValues()
        {
            for (Int32 i = 0; i < this.Item_LayoutPanel.Controls.Count; i++)
            {
                for (Int32 j = 0; j < this.Item_LayoutPanel.Controls[i].Controls.Count; j++)
                {
                    this.Item_LayoutPanel.Controls[i].Controls[j].Dispose();
                }
                this.Item_LayoutPanel.Controls[i].Dispose();
            }
            this.Item_LayoutPanel.Controls.Clear();

            Pointer address = this.Item_PointerArrayBox.Value;
            if (address != 0)
            {
                List<UInt32> anims = new List<UInt32>();
                UInt32 offset = 0;
                UInt32 buffer;
                do
                {
                    buffer = Util.BytesToUInt32(Core.ReadData(address + offset, 4), false);
                    anims.Add(buffer);
                    offset += 4;
                }
                while (buffer != 0);

                Int32 index = 0;
                Int32 length = anims.Count;
                GroupBox[] groupboxes = new GroupBox[length];
                this.Item_Controls = new Control[length * 8];
                try
                {
                    StructFile word = new StructFile("Battle Animation Association.txt");
                    ArrayFile animtype = new ArrayFile("Battle Animation Types.txt");
                    ArrayFile itemlist = new ArrayFile("Item List.txt");
                    word.Address = address;

                    for (Int32 i = 0; i < length; i++)
                    {
                        word.EntryIndex = i;

                        groupboxes[i] = new GroupBox()
                        {
                            Text = (i == length - 1) ? "Terminator" : "Animation " + i,
                            Location = new Point(2, 150 * i),
                            Size = new Size(235, 140),
                        };
                        this.Item_LayoutPanel.Controls.Add(groupboxes[i]);

                        Core_LoadItemControl(groupboxes[i], i, ref index,
                            "Anim", word["Anim"], word.GetAddress(i, "Anim"), this.Entry_ArrayBox.File);
                        Core_LoadItemControl(groupboxes[i], i, ref index,
                            "Type", word["Type"], word.GetAddress(i, "Type"), animtype, true, word["Bool"], 0x01);
                        Core_LoadItemControl(groupboxes[i], i, ref index,
                            "Item", word["Type"], word.GetAddress(i, "Type"), itemlist, true, word["Bool"], 0x00);
                        Core_LoadItemControl(groupboxes[i], i, ref index,
                            "Separator", word["Separator"], word.GetAddress(i, "Separator"));
                    }
                }
                catch (Exception ex)
                {
                    UI.ShowError("There has been error while trying to load the item association values." +
                         "\r\n\r\n" + "Error at word " + (index / 8) + ", control is " + this.Item_Controls[index].Name, ex);
                }
            }
        }
        void Core_LoadItemControl(
            GroupBox box, Int32 num, ref Int32 index,
            String name, Byte value, Pointer address, ArrayFile entrylist = null,
            Boolean radiobutton = false, Byte boolvalue = 0, Byte radiovalue = 0)
        {
            Control label;
            Int32 control_index = ((index / 2) % 4);

            label = (radiobutton) ?
                (Control)new RadioButton()
                {
                    Text = name + " :",
                    Name = name.Substring(0, 4) + "_" + num,
                    Checked = (boolvalue == radiovalue),
                    AutoSize = false,
                    Location = new Point(10, 20 + 30 * control_index),
                    Size = new Size(60, 20),
                } :
                (Control)new Label()
                {
                    Text = name + " :",
                    Name = name.Substring(0, 4) + "_" + num,
                    TextAlign = ContentAlignment.MiddleRight,
                    AutoSize = false,
                    Location = new Point(4, 20 + 30 * control_index),
                    Size = new Size(60, 20),
                };
            this.Item_Controls[index++] = label;
            this.Item_Controls[index] = new ByteArrayBox()
            {
                Name = name.Substring(0, 4) + "_" + num,
                Location = new Point(70, 20 + 30 * control_index),
                Size = new Size(160, 20),
            };
            if (entrylist != null)
                ((ByteArrayBox)this.Item_Controls[index]).Load(entrylist);
            box.Controls.Add(label);
            box.Controls.Add(this.Item_Controls[index]);
            ((ByteArrayBox)this.Item_Controls[index]).Name = name.Substring(0, 4) + "_" + num;
            if (radiobutton)
            {
                if (boolvalue == radiovalue)
                    ((ByteArrayBox)this.Item_Controls[index]).Value = value;
                else
                {
                    ((ByteArrayBox)this.Item_Controls[index]).Value = 0x00;
                    ((ByteArrayBox)this.Item_Controls[index]).Enabled = false;
                }
                Byte notradiovalue = (radiovalue == 0x00) ? (Byte)0x01 : (Byte)0x00;
                ((RadioButton)label).CheckedChanged += delegate (Object sender, EventArgs e)
                {
                    Int32 word = Int32.Parse(((RadioButton)sender).Name.Substring(5));
                    Boolean radiochecked = ((RadioButton)sender).Checked;
                    if (((RadioButton)sender).Name.Substring(0, 4) == "Type")
                    {
                        this.Item_Controls[word * 8 + 3].Enabled = radiochecked;
                        this.Item_Controls[word * 8 + 5].Enabled = !radiochecked;
                    }
                    else
                    {
                        this.Item_Controls[word * 8 + 3].Enabled = !radiochecked;
                        this.Item_Controls[word * 8 + 5].Enabled = radiochecked;
                    }
                    Core.WriteByte(this,
                        address + 1,
                        (Byte)(((RadioButton)sender).Checked ? radiovalue : notradiovalue),
                        this.CurrentItemPointerEntry(num) + "Bool " + num + " changed");
                };
            }
            else ((ByteArrayBox)this.Item_Controls[index]).Value = value;

            ((ByteArrayBox)this.Item_Controls[index]).ValueChanged += delegate (Object sender, EventArgs e)
            {
                Core.WriteByte(this,
                    address,
                    ((ByteBox)sender).Value,
                    this.CurrentItemPointerEntry(num) + name + " " + num + " changed");
            };
            index++;
        }

        void Core_UpdateImageBox()
        {
            try
            {
                this.CurrentAnim.ShowFrame(this.CurrentPalette, this.CurrentFrame, this.OAM_L_Button.Checked);

                if (!this.View_AllFrames.Checked && this.View_Layered.Checked &&
                    (this.CurrentMode == 0 || this.CurrentMode == 2 || this.CurrentMode == 8))
                {
                    BattleAnimFrame frame = this.CurrentAnim.Frames[this.Frames[this.CurrentMode + 1][this.Frame_ByteBox.Value]];

                    this.CurrentAnim.AddSprite(this.CurrentPalette,
                        this.CurrentAnim.Tilesets[frame.TilesetIndex],
                        this.OAM_L_Button.Checked ? frame.OAM_Data_L : frame.OAM_Data_R,
                        this.OAM_L_Button.Checked ? BattleAnimation.SCREEN_OFFSET_X_L : BattleAnimation.SCREEN_OFFSET_X_R,
                        BattleAnimation.SCREEN_OFFSET_Y);
                }
                this.Anim_ImageBox.Load(this.CurrentAnim);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to show the animation frame.", ex);

                this.Anim_ImageBox.Reset();
            }
        }
        void Core_UpdatePalettes()
        {
            try
            {
                this.Palette_PaletteBox.Load(this.CurrentPalette);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while updating the displayed palette.", ex);

                this.Palette_PaletteBox.Reset();
            }
        }

        /// <summary>
        /// Processes a user-made animation into several files: png tilesheets with .fea, .oam and .pal
        /// </summary>
        void Core_CreateAnim(String filepath)
        {
            try
            {
                String path = Path.GetDirectoryName(filepath) + "\\";
                String[] code = File.ReadAllLines(filepath);

                BattleAnimMaker anim = new BattleAnimMaker(path, code);

                Tuple<UInt32, UInt32>[] frames = anim.FrameData.ToArray();
                List<Byte> animdata = new List<Byte>();
                foreach (List<String> mode in anim.AnimCode)
                {
                    animdata.AddRange(BattleAnimation.CompileAnimationCode(mode.ToArray(), frames));
                }

                Byte[] fea = animdata.ToArray();
                Byte[] oam = OAM_Array.Merge(anim.Frames);
                Byte[] pal = Palette.Merge(anim.Palettes).ToBytes(false);
                Byte[] palette = anim.Palettes[0].ToBytes(false);
                GBA.Image[] sheets = new GBA.Image[anim.Graphics.Count];
                for (Int32 i = 0; i < sheets.Length; i++)
                {
                    sheets[i] = anim.Graphics[i].ToImage(32, 8, palette);
                }

                SaveFileDialog saveWindow = new SaveFileDialog();
                saveWindow.RestoreDirectory = true;
                saveWindow.OverwritePrompt = true;
                saveWindow.CreatePrompt = false;
                saveWindow.FilterIndex = 1;
                saveWindow.Filter =
                    "Fire Emblem Animation (*.fea)|*.fea|" +
                    "All files (*.*)|*.*";

                if (saveWindow.ShowDialog() == DialogResult.OK)
                {
                    String file = Path.GetFileNameWithoutExtension(saveWindow.FileName);
                    path = Path.GetDirectoryName(saveWindow.FileName) + "\\";
                    // remove extension
                    File.WriteAllBytes(path + file + ".fea", fea);
                    File.WriteAllBytes(path + file + ".oam", oam);
                    File.WriteAllBytes(path + file + ".pal", pal);

                    using (System.Drawing.Bitmap image = new System.Drawing.Bitmap(32 * 8, 8 * 8, PixelFormat.Format8bppIndexed))
                    {
                        for (Int32 i = 0; i < sheets.Length; i++)
                        {
                            Core.SaveImage(path + file + " " + (i + 1),
                                GBA.Tile.SIZE * 32,
                                GBA.Tile.SIZE * 8,
                                new Palette[1] { sheets[i].Colors },
                                delegate (Int32 x, Int32 y)
                                {
                                    return (Byte)sheets[i][x, y];
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not create the animation.", ex); return;
            }
        }
        /// <summary>
        /// Loads a FEA with its associated files to insert an animation onto the ROM
        /// </summary>
        void Core_InsertAnim(String filepath, Int32 index)
        {
            UI.SuspendUpdate();
            try
            {
                String path = Path.GetDirectoryName(filepath) + "\\";
                String file = Path.GetFileNameWithoutExtension(filepath);

                List<Pointer> pointers = BattleAnimation.GetTilesetPointers(Core.ReadData((Pointer)this.Current["AnimData"], 0));

                Byte[] animdata = File.ReadAllBytes(filepath);
                Byte[] sections = BattleAnimation.GetSections(animdata);

                filepath = path + file + ".oam";
                Tuple<Byte[], Byte[]> oam_data = BattleAnimation.GetFlippedOAM(File.ReadAllBytes(filepath));
                oam_data = Tuple.Create(LZ77.Compress(oam_data.Item1), LZ77.Compress(oam_data.Item2));

                filepath = path + file + ".pal";
                Byte[] palettes = File.ReadAllBytes(filepath);
                Byte[] buffer = new Byte[GBA.Palette.LENGTH];
                Array.Copy(palettes, buffer, buffer.Length);
                Palette palette = new Palette(buffer);
                palettes = LZ77.Compress(palettes);

                List<Tileset> tilesets = new List<Tileset>();
                for (Int32 i = 1; i < 256; i++)
                {
                    filepath = path + file + " " + i + ".png";
                    if (File.Exists(filepath))
                    {
                        GBA.Image image = new GBA.Image(filepath, palette);

                        if (image.Width != 256 || image.Height != 64)
                            throw new Exception(filepath + '\n' +
                                "This image has invalid dimensions, it must be 256x64 pixels (32x8 tiles)");

                        tilesets.Add(new Tileset(image));
                    }
                    else break;
                }
                if (tilesets.Count == 0) throw new Exception("No tilesheet image was found, please check the file names.");

                var repoints = new List<Tuple<String, Pointer, Int32>>();
                List<Byte[]> graphics = new List<Byte[]>();
                for (Int32 i = 0; i < tilesets.Count; i++)
                {
                    graphics.Add(tilesets[i].ToBytes(true));
                    try
                    { repoints.Add(Tuple.Create("Tileset " + i, pointers[i], graphics[i].Length)); }
                    catch
                    { repoints.Add(Tuple.Create("Tileset " + i, new Pointer(), graphics[i].Length)); }
                }

                if (Settings.Default.WriteToFreeSpace)
                {
                    Int32 length = 0;
                    for (Int32 i = 0; i < repoints.Count; i++)
                    {
                        length += repoints[i].Item3;
                    }
                    pointers = new List<Pointer>();
                    pointers.Add(Core.GetFreeSpace(length));
                    length = graphics[0].Length;
                    for (Int32 i = 1; i < repoints.Count; i++)
                    {
                        pointers.Add(pointers[0] + length);
                        length += repoints[i].Item3;
                    }
                }
                else if (Settings.Default.PromptRepoints)
                {
                    FormRepoint Dialog = new FormRepoint("Repoint Tilesets",
                        "The tileset graphics for this animation might overlap.\n" +
                        "Please check and repoint them as necessary.", repoints.ToArray());

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        pointers = new List<Pointer>();
                        for (Int32 i = 0; i < repoints.Count; i++)
                        {
                            pointers.Add(Dialog.Boxes[i].Value);
                        }
                    }
                    else return;
                }

                for (Int32 i = 0; i < repoints.Count; i++)
                {
                    Program.Core.MHF.Space.MarkSpace("USED", pointers[i], pointers[i] + repoints[i].Item3);
                }

                animdata = LZ77.Compress(BattleAnimation.PrepareAnimationData(animdata, pointers));

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Animation",
                    "The animation to insert has several parts that must be written.\n" +
                    "Some of these may need to be repointed so as to not overwrite each other.",
                    this.CurrentEntry, new Tuple<String, Pointer, Int32>[5] {
                        Tuple.Create("Anim Data Sections", (Pointer)this.Current["Sections"], sections.Length),
                        Tuple.Create("Anim Data",          (Pointer)this.Current["AnimData"], animdata.Length),
                        Tuple.Create("Right-side OAM",     (Pointer)this.Current["OAM_Right"],oam_data.Item2.Length),
                        Tuple.Create("Left-side OAM",      (Pointer)this.Current["OAM_Left"], oam_data.Item1.Length),
                        Tuple.Create("Palettes",           (Pointer)this.Current["Palettes"], palettes.Length)},
                    new Pointer[5] {
                        this.Current.GetAddress(this.Current.EntryIndex, "Sections"),
                        this.Current.GetAddress(this.Current.EntryIndex, "AnimData"),
                        this.Current.GetAddress(this.Current.EntryIndex, "OAM_Right"),
                        this.Current.GetAddress(this.Current.EntryIndex, "OAM_Left"),
                        this.Current.GetAddress(this.Current.EntryIndex, "Palettes")});
                if (cancel) return;

                for (Int32 i = 0; i < tilesets.Count; i++)
                {
                    Core.WriteData(this,
                        pointers[i], graphics[i],
                        this.CurrentEntry + "Tileset " + i + " changed");
                }

                Core.WriteData(this,
                    (Pointer)this.Current["Sections"], sections,
                    this.CurrentEntry + "Anim Data Sections changed");

                Core.WriteData(this,
                    (Pointer)this.Current["AnimData"], animdata,
                    this.CurrentEntry + "Anim Data changed");

                Core.WriteData(this,
                    (Pointer)this.Current["OAM_Right"], oam_data.Item2,
                    this.CurrentEntry + "Right-side OAM changed");

                Core.WriteData(this,
                    (Pointer)this.Current["OAM_Left"], oam_data.Item1,
                    this.CurrentEntry + "Left-side OAM changed");

                Core.WriteData(this,
                    (Pointer)this.Current["Palettes"], palettes,
                    this.CurrentEntry + "Palettes changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the animation.", ex); return;
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        /// <summary>
        /// Saves all the frames as images as .png and the anim code as .txt to a folder
        /// </summary>
        void Core_SaveAnimFolder(String filepath)
        {
            try
            {
                String path = Path.GetDirectoryName(filepath) + "\\";
                String file = Path.GetFileNameWithoutExtension(filepath);

                List<Tuple<UInt32, OAM>> affines = new List<Tuple<UInt32, OAM>>();
                Int32[] duplicates = new Int32[this.CurrentAnim.Frames.Length];
                for (Int32 i = 0; i < duplicates.Length; i++) duplicates[i] = -1;
                String animcode = BattleAnimation.GetAnimationCode(
                    this.CurrentAnim.AnimCode,
                    this.CurrentMode, true, true,
                    ref affines, ref duplicates,
                    file, this.CurrentAnim.Frames);
                File.WriteAllText(path + file + ".txt", animcode);

                Byte[] palette = this.CurrentPalette.ToBytes(false);
                Palette[] palettes = (this.Palette_Default_Button.Checked) ? this.DefaultPalette : this.CharacterPalette;
                Int32 width = 16;
                Int32 height = 4;
                Core.SaveImage(path + "palette",
                    width,
                    height,
                    palettes,
                    delegate (Int32 x, Int32 y)
                    {
                        return (Byte)(y * Palette.MAX + x);
                    });

                Size size;
                for (Int32 i = 0; i < affines.Count; i++)
                {
                    size = affines[i].Item2.GetDimensions();
                    GBA.Image tileset = this.CurrentAnim.Tilesets[affines[i].Item1].ToImage(32, 8, palette);
                    Int32 offsetX = affines[i].Item2.SheetX * Tile.SIZE;
                    Int32 offsetY = affines[i].Item2.SheetY * Tile.SIZE;
                    Core.SaveImage(path + file + "_affine_" + i,
                        size.Width * Tile.SIZE,
                        size.Height * Tile.SIZE,
                        new Palette[1] { tileset.Colors },
                        delegate (Int32 x, Int32 y)
                        {
                            return (Byte)tileset[offsetX + x, offsetY + y];
                        });
                }
                
                for (Byte i = 0; i < this.CurrentAnim.Frames.Length; i++)
                {
                    if (this.CurrentAnim.Frames[i] == null)
                        continue;
                    if (duplicates[i] != -1)
                        continue;
                    this.CurrentAnim.Clear();
                    this.CurrentAnim.AddSprite(this.CurrentPalette,
                        this.CurrentAnim.Tilesets[this.CurrentAnim.Frames[i].TilesetIndex],
                        this.CurrentAnim.Frames[i].OAM_Data_R,
                        BattleAnimation.SCREEN_OFFSET_X_R,
                        BattleAnimation.SCREEN_OFFSET_Y,
                        false);
                    if (this.CurrentAnim.Count > 0)
                    {
                        Core.SaveImage(path + file + "_" + i,
                            this.CurrentAnim.Width,
                            this.CurrentAnim.Height,
                            new Palette[1] { this.CurrentPalette },
                            delegate (Int32 x, Int32 y)
                            {
                                return (Byte)this.CurrentAnim[x, y];
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save battle animation to folder.", ex);
            }
            this.Core_Update();
        }
        /// <summary>
        /// Exports the current animation into FEA (with .oam, .pal and the sheets)
        /// </summary>
        void Core_SaveAnimFiles(String filepath)
        {
            try
            {
                Byte[] fea = Core.ReadData((Pointer)this.Current["AnimData"], 0);
                Byte[] oam = Core.ReadData((Pointer)this.Current["OAM_Right"], 0);
                Byte[] pal = Core.ReadData((Pointer)this.Current["Palettes"], 0);
            
                GBA.Image[] sheets = new GBA.Image[this.CurrentAnim.Tilesets.Length];
                Byte[] palette = this.DefaultPalette[0].ToBytes(false);
                for (Int32 i = 0; i < sheets.Length; i++)
                {
                    sheets[i] = this.CurrentAnim.Tilesets[i].ToImage(32, 8, palette);
                }

                fea = BattleAnimation.ExportAnimationData(fea, BattleAnimation.GetTilesetPointers(fea));
                filepath = filepath.Substring(0, filepath.Length - 4);
                // remove extension
                File.WriteAllBytes(filepath + ".fea", fea);
                File.WriteAllBytes(filepath + ".oam", oam);
                File.WriteAllBytes(filepath + ".pal", pal);

                using (System.Drawing.Bitmap png = new System.Drawing.Bitmap(32 * 8, 8 * 8, PixelFormat.Format8bppIndexed))
                {
                    for (Int32 i = 0; i < sheets.Length; i++)
                    {
                        for (Int32 y = 0; y < png.Height; y++)
                        for (Int32 x = 0; x < png.Width; x++)
                        {
                            png.SetPixel(x, y, (System.Drawing.Color)sheets[i].GetColor(x, y));
                        }
                        png.Save(filepath + " " + (i + 1) + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save the animation to files.", ex); return;
            }
        }
        /// <summary>
        /// Saves a gif of the current anim to the given filepath
        /// </summary>
        void Core_SaveAnimGIF(String filepath, Int32 mode, Boolean browserFriendly)
        {
            try
            {
                if (!filepath.EndsWith(".gif"))
                    throw new Exception("File must have .GIF extension");
            
                List<Byte> gif = new List<Byte>();
                // Header:
                gif.AddRange(new Byte[3] { 0x47, 0x49, 0x46 }); // "GIF" - File type
                gif.AddRange(new Byte[3] { 0x38, 0x39, 0x61 }); // "89a" - File version
                gif.AddRange(Util.UInt16ToBytes((UInt16)this.CurrentAnim.Width, true)); // Logical screen width
                gif.AddRange(Util.UInt16ToBytes((UInt16)this.CurrentAnim.Height, true)); // Logical screen height
                gif.Add(0xB3); // Packed field - 0xB3 indicates a 16-color global palette
                gif.Add(0x00); // Background Color Index in the color table
                gif.Add(0x00); // Pixel Aspect Ratio - not really used apparently
                // Global Color Table:
                for (Int32 i = 0; i < Palette.MAX; i++)
                {   // set the Global Color Table, 3 bytes per color
                    UInt32 color = this.CurrentPalette[i].To32bit();
                    gif.Add((Byte)((color >> 16) & 0xFF)); // R channel
                    gif.Add((Byte)((color >> 8) & 0xFF));  // G channel
                    gif.Add((Byte)(color & 0xFF));         // B channel
                }
                // Application Extension Block:
                gif.Add(0x21); // Extension Introducer (0x21)
                gif.Add(0xFF); // Application Label (0xFF)
                gif.Add(0x0B); // Block Size (11 bytes)
                gif.AddRange(new Byte[11] { 0x4E, 0x45, 0x54, 0x53, 0x43, 0x41, 0x50, 0x45, 0x32, 0x2E, 0x30 }); // "NETSCAPE2.0"
                gif.Add(0x03); // Sub-block Size (3 bytes)
                gif.Add(0x01); // Always 0x01 ? Looping label ?
                gif.AddRange(new Byte[2] { 0x00, 0x00 }); // Loop value, 0x0000 makes it loop forever
                gif.Add(0x00); // Block Terminator (0x00)
                List<Int32> wait_frames = new List<Int32>();
                Int32 wait_frame = 0;
                for (Int32 i = 0; i < this.CurrentAnim.AnimCode[mode].Length; i++)
                {
                    if (this.CurrentAnim.AnimCode[mode][i].StartsWith("c01") || // Wait for HP deplete
                        this.CurrentAnim.AnimCode[mode][i].StartsWith("c13"))   // Wait for Handaxe return
                    {
                        wait_frames.Add(wait_frame - 1);
                    }
                    else if (this.CurrentAnim.AnimCode[mode][i].IndexOf(" f") > 0 &&
                        !this.CurrentAnim.AnimCode[mode][i].StartsWith("c") &&
                        !this.CurrentAnim.AnimCode[mode][i].StartsWith("#"))
                    {
                        wait_frame += 1;
                    }
                }
                //MessageBox.Show(string.Join(";", wait_frames));
                for (Byte i = 0; i < this.Frames[mode].Count; i++)
                {
                    this.CurrentAnim.ShowFrame(this.CurrentPalette, this.Frames[mode][i], this.OAM_L_Button.Checked);
                    UInt16 duration = (UInt16)Math.Round(this.Durations[mode][i] * (100f / 60f));
                    if (i == 0 || wait_frames.Contains(i))
                        duration = 60;
                    else if (browserFriendly && duration < 4)
                        duration = 4;
                    if (!this.View_AllFrames.Checked && this.View_Layered.Checked && (mode == 0 || mode == 2 || mode == 8))
                    {
                        BattleAnimFrame frame = this.CurrentAnim.Frames[this.Frames[mode + 1][i]];

                        this.CurrentAnim.AddSprite(this.CurrentPalette,
                            this.CurrentAnim.Tilesets[frame.TilesetIndex],
                            this.OAM_L_Button.Checked ? frame.OAM_Data_L : frame.OAM_Data_R,
                            this.OAM_L_Button.Checked ? BattleAnimation.SCREEN_OFFSET_X_L : BattleAnimation.SCREEN_OFFSET_X_R,
                            BattleAnimation.SCREEN_OFFSET_Y);
                    }
                    // Graphic Control Extension Block:
                    gif.Add(0x21); // Extension Introducer (0x21)
                    gif.Add(0xF9); // Graphic Control Label (0xF9)
                    gif.Add(0x04); // Block Size (0x04)
                    gif.Add(0x09); // Packed field - 0x09 indicates a redraw-BG, transparency-enabled GIF
                    gif.AddRange(Util.UInt16ToBytes(duration, true)); // Delay Time (1/100ths of a second)
                    gif.Add(0x00); // Transparent Color Index
                    gif.Add(0x00); // Block Terminator (0x00)
                    // Image block:
                    gif.Add(0x2C); // Image Introducer (0x2C)
                    gif.AddRange(new Byte[4] { 0x00, 0x00, 0x00, 0x00 }); // Image X and Y
                    gif.AddRange(Util.UInt16ToBytes((UInt16)this.CurrentAnim.Width, true)); // Image Width
                    gif.AddRange(Util.UInt16ToBytes((UInt16)this.CurrentAnim.Height, true));// Image Height
                    gif.Add(0x00); // Packed field - 0x00 indicates no interlacing, no local color table
                    Byte[] pixels = new Byte[this.CurrentAnim.Width * this.CurrentAnim.Height];
                    Int32 index = 0;
                    for (Int32 y = 0; y < this.CurrentAnim.Height; y++)
                    for (Int32 x = 0; x < this.CurrentAnim.Width; x++)
                    {
                        pixels[index++] = (Byte)this.CurrentAnim[x, y];
                    }
                    gif.AddRange(new LZW(this.CurrentAnim.Width, this.CurrentAnim.Height, pixels, 0x04).Compress());
                }
                gif.Add(0x3B); // Trailer (0x3B) - every GIF file ends with this byte

                File.WriteAllBytes(filepath, gif.ToArray());
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save the animation to GIF file.", ex); return;
            }
            this.CurrentAnim.ShowFrame(this.CurrentPalette, this.CurrentFrame, this.OAM_L_Button.Checked);
        }



        private void File_Create_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Battle Animation Code (*.txt)|*.txt|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_CreateAnim(openWindow.FileName);
            }
        }
        private void File_Insert_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter = 
                "Fire Emblem Animation (*.fea)|*.fea|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_InsertAnim(openWindow.FileName, this.Entry_ArrayBox.Value);
            }
        }
        private void File_SaveFolder_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Text file (*.txt)|*.txt|" +
                "All files (*.*)|*.*";
            saveWindow.FileName = this.Entry_ArrayBox.Text;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_SaveAnimFolder(saveWindow.FileName);
            }
        }
        private void File_SaveFiles_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Fire Emblem Animation (*.fea)|*.fea|" +
                "All files (*.*)|*.*";
            saveWindow.FileName = this.Entry_ArrayBox.Text;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_SaveAnimFiles(saveWindow.FileName);
            }
        }
        private void File_SaveGIF_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Browser-friendly GIF File (*.gif)|*.gif|" +
                "Faithful GIF File (*.gif)|*.gif|" +
                "All files (*.*)|*.*";
            saveWindow.FileName = this.Entry_ArrayBox.Text + " - " + this.Anim_Mode_ListBox.GetItemText(this.Anim_Mode_ListBox.SelectedItem);

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_SaveAnimGIF(saveWindow.FileName,
                    this.CurrentMode, saveWindow.FilterIndex == 1);
            }
        }
        private void File_SaveAllGIF_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 2;
            saveWindow.Filter =
                "Browser-friendly GIF File (*.gif)|*.gif|" +
                "Faithful GIF File (*.gif)|*.gif|" +
                "All files (*.*)|*.*";
            saveWindow.FileName = this.Entry_ArrayBox.Text;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                String filename = saveWindow.FileName;
                if (filename.EndsWith(".gif"))
                    filename = filename.Substring(0, filename.Length - 4);
                Int32 length = this.View_Layered.Checked ? BattleAnimation.Modes_Layered.Length : BattleAnimation.Modes.Length;
                Int32 mode = 0;
                for (Int32 i = 0; i < length; i++)
                {
                    this.Anim_Mode_ListBox.SelectedIndex = i;
                    this.Core_SaveAnimGIF(filename + " - " + this.Anim_Mode_ListBox.GetItemText(this.Anim_Mode_ListBox.SelectedItem) + ".gif",
                        mode, saveWindow.FilterIndex == 1);
                    if (this.View_Layered.Checked && (mode == 0 || mode == 2 || mode == 8))
                        mode++;
                    mode++;
                }
                this.Anim_Mode_ListBox.SelectedIndex = 0;
            }
        }

        private void Tool_OpenOAMEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenSpriteEditor(this,
                this.CurrentEntry + "Frame 0x" + Util.ByteToHex(this.CurrentFrame) + " - ",
                this.OAM_R_Button.Checked ? this.OAM_R_PointerBox.Value : this.OAM_L_PointerBox.Value,
                (Int32)this.CurrentAnim.Frames[this.CurrentFrame].OAM_Offset,
                this.OAM_R_Button.Checked ?
                    BattleAnimation.SCREEN_OFFSET_X_R :
                    BattleAnimation.SCREEN_OFFSET_X_L,
                BattleAnimation.SCREEN_OFFSET_Y,
                this.CurrentPalette,
                this.CurrentAnim.Tilesets[this.CurrentAnim.Frames[this.CurrentFrame].TilesetIndex]);
        }
        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            Pointer address;
            Boolean is_uncompressed;
            if (this.Palette_Default_Button.Checked)
            {
                address = (Pointer)this.Current["Palettes"];
                is_uncompressed = (address >= 0x80000000);
                UI.OpenPaletteEditor(this,
                    this.CurrentEntry,
                    (is_uncompressed ? address - 0x80000000 : address),
                    (is_uncompressed ? 5 : 0));
            }
            else
            {
                address = (Pointer)this.Palettes["Address"];
                is_uncompressed = (address >= 0x80000000);
                UI.OpenPaletteEditor(this,
                    "Character Palette 0x" + this.Palette_Character_ArrayBox.Value + " [" + this.Palette_Character_ArrayBox.Text + "] - ",
                    (is_uncompressed ? address - 0x80000000 : address),
                    (is_uncompressed ? 5 : 0));
            }
        }

        private void View_AllFrames_Click(Object sender, EventArgs e)
        {
            this.Frame_ByteBox.ValueChanged -= this.FrameByteBox_ValueChanged;
            this.Frame_ByteBox.Value = 0;
            this.Frame_ByteBox.ValueChanged += this.FrameByteBox_ValueChanged;

            this.Core_LoadAnimCode();
            this.Core_UpdateImageBox();
        }
        private void View_Layered_Click(Object sender, EventArgs e)
        {
            if (this.View_Layered.Checked)
                this.Anim_Mode_ListBox.DataSource = BattleAnimation.Modes_Layered;
            else this.Anim_Mode_ListBox.DataSource = BattleAnimation.Modes;

            this.Core_UpdateImageBox();
        }
        private void View_AllAnimCode_Click(Object sender, EventArgs e)
        {
            this.Core_LoadAnimCode();
        }



        private void Item_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_LoadItemValues();
        }
        private void Item_AppendButton_Click(Object sender, EventArgs e)
        {
            Pointer address = this.Item_PointerArrayBox.Value;
            UInt32 offset = 0;
            UInt32 buffer;
            do
            {
                buffer = Util.BytesToUInt32(Core.ReadData(address + offset, 4), false);
                offset += 4;
            }
            while (buffer != 0);
            offset -= 4;

            Core.WriteByte(this,
                address + offset + 1,
                0x01,
                this.CurrentItemPointerEntry((Int32)offset / 4) + "Bool changed");
        }
        private void Item_RemoveButton_Click(Object sender, EventArgs e)
        {
            Pointer address = this.Item_PointerArrayBox.Value;
            UInt32 offset = 0;
            UInt32 buffer;
            do
            {
                buffer = Util.BytesToUInt32(Core.ReadData(address, 4), false);
                offset += 4;
            }
            while (buffer != 0);
            offset -= 8;

            Core.WriteData(this,
                address + offset,
                new Byte[4] { 0x00, 0x00, 0x00, 0x00 },
                this.CurrentItemPointerEntry((Int32)offset / 4) + "Bool changed");
        }

        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Frame_ByteBox.ValueChanged -= this.FrameByteBox_ValueChanged;
            this.Frame_ByteBox.Value = 0;
            this.Frame_ByteBox.ValueChanged += this.FrameByteBox_ValueChanged;

            this.Core_Update();
        }
        private void FrameByteBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_UpdateImageBox();
        }
        private void AnimListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            this.Frame_ByteBox.ValueChanged -= this.FrameByteBox_ValueChanged;
            this.Frame_ByteBox.Value = 0;
            this.Frame_ByteBox.ValueChanged += this.FrameByteBox_ValueChanged;

            this.Core_Update();
        }

        private void Anim_NameTextBox_TextChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                this.Current.Address,
                ByteArray.Make_ASCII(this.Anim_Name_TextBox.Text),
                this.CurrentEntry + "Anim name changed");
        }
        private void OAM_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_UpdateImageBox();
        }
        
        private void SectionsPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Sections"),
                this.Sections_PointerBox.Value,
                this.CurrentEntry + "Anim Data Sections repoint");
        }
        private void AnimDataPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "AnimData"),
                this.AnimData_PointerBox.Value,
                this.CurrentEntry + "Anim Data repoint");
        }
        private void OAM_R_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "OAM_Right"),
                this.OAM_R_PointerBox.Value,
                this.CurrentEntry + "Right-side OAM repoint");
        }
        private void OAM_L_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "OAM_Left"),
                this.OAM_L_PointerBox.Value,
                this.CurrentEntry + "Left-side OAM repoint");
        }

        private void Palette_CheckedChanged(Object sender, EventArgs e)
        {
            this.Palette_Default_ArrayBox.Enabled        = this.Palette_Default_Button.Checked;
            this.Palette_Default_PointerBox.Enabled      = this.Palette_Default_Button.Checked;
            this.Palette_Character_TextBox.Enabled          = this.Palette_Character_Button.Checked;
            this.Palette_Character_ArrayBox.Enabled         = this.Palette_Character_Button.Checked;
            this.Palette_Character_PointerBox.Enabled       = this.Palette_Character_Button.Checked;
            this.Palette_Character_Current_ArrayBox.Enabled = this.Palette_Character_Button.Checked;
            this.Core_UpdatePalettes();
            this.Core_UpdateImageBox();
        }
        private void Palette_Default_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.Palette_Default_Button.Checked)
            {
                this.Core_UpdatePalettes();
                this.Core_UpdateImageBox();
            }
        }
        private void Palette_Default_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Palettes"),
                this.Palette_Default_PointerBox.Value,
                this.CurrentEntry + "Default Palettes repoint");
        }
        private void Palette_Character_TextBox_TextChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                this.Current.Address,
                ByteArray.Make_ASCII(this.Anim_Name_TextBox.Text),
                this.CurrentPaletteEntry + "Name changed");
        }
        private void Palette_Character_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Palettes.EntryIndex = this.Palette_Character_ArrayBox.Value - 1;
            this.Core_LoadPalettes();
            this.Core_LoadPaletteValues();
            if (this.Palette_Character_Button.Checked)
            {
                this.Core_UpdatePalettes();
                this.Core_UpdateImageBox();
            }
        }
        private void Palette_Character_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Palettes.GetAddress(this.Palettes.EntryIndex, "Address"),
                this.Palette_Character_PointerBox.Value,
                this.CurrentPaletteEntry + "repoint");
        }
        private void Palette_Character_Current_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (this.Palette_Character_Button.Checked)
            {
                this.Core_UpdatePalettes();
                this.Core_UpdateImageBox();
            }
        }

        private void AnimCode_Apply_Button_Click(Object sender, EventArgs e)
        {
            Tuple<UInt32, UInt32>[] frames = new Tuple<UInt32, UInt32>[this.CurrentAnim.Frames.Length];
            for (Int32 i = 0; i < frames.Length; i++)
            {
                if (this.CurrentAnim.Frames[i] != null)
                    frames[i] = Tuple.Create(
                        this.CurrentAnim.Frames[i].TilesetIndex,
                        this.CurrentAnim.Frames[i].OAM_Offset);
            }
            Byte[] animdata;
            Byte[] sections = new Byte[BattleAnimation.MODES * 4];
            Byte[][] modes = new Byte[BattleAnimation.MODES][];
            Int32 offset = 0;
            try
            {
                Tuple<String[], String[]> layers = null;
                String[] animcode;
                Int32 index = 0;
                Int32 length;
                for (Int32 i = 0; i < BattleAnimation.MODES; i++)
                {
                    if (this.View_AllAnimCode.Checked)
                    {
                        length = this.AnimCodeBox.Text.IndexOf("end", index) + 3 - index;
                        animcode = this.AnimCodeBox.Text.Substring(index, length).Split(
                            new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        index += length;

                        if (this.View_Layered.Checked && (i == 0 || i == 2 || i == 8)) // two-layer anim modes
                            layers = BattleAnimation.SplitLayeredAnimationCode(animcode);
                    }
                    else if (this.CurrentMode == i)
                    {
                        animcode = this.AnimCodeBox.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        if (this.View_Layered.Checked && (i == 0 || i == 2 || i == 8)) // two-layer anim modes
                            layers = BattleAnimation.SplitLayeredAnimationCode(animcode);
                    }
                    else
                    {
                        animcode = this.CurrentAnim.AnimCode[i];
                    }

                    if (layers != null) // two-layer anim modes
                    {
                        animcode = layers.Item1;
                        modes[i] = BattleAnimation.CompileAnimationCode(animcode, frames);
                        Array.Copy(Util.Int32ToBytes(offset, true), 0, sections, i * 4, 4);
                        offset += modes[i].Length;
                        i++;
                        animcode = layers.Item2;
                    }
                    modes[i] = BattleAnimation.CompileAnimationCode(animcode, frames);
                    Array.Copy(Util.Int32ToBytes(offset, true), 0, sections, i * 4, 4);
                    offset += modes[i].Length;
                    layers = null;
                }
                animdata = new Byte[offset];
                offset = 0;
                for (Int32 i = 0; i < modes.Length; i++)
                {
                    Array.Copy(modes[i], 0, animdata, offset, modes[i].Length);
                    offset += modes[i].Length;
                }
                animdata = LZ77.Compress(BattleAnimation.PrepareAnimationData(animdata,
                    BattleAnimation.GetTilesetPointers(Core.ReadData((Pointer)this.Current["AnimData"], 0))));
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while compiling the animation code.", ex);
                return;
            }

            UI.SuspendUpdate();
            try
            {
                Pointer sections_pointer = (Pointer)this.Current["Sections"];
                Pointer animdata_pointer = (Pointer)this.Current["AnimData"];
                if ((sections_pointer > animdata_pointer) && (animdata.Length > (animdata_pointer - sections_pointer))
                 || (animdata_pointer > sections_pointer) && (sections.Length > (sections_pointer - animdata_pointer)))
                {
                    Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Animation Data",
                        "The animation code and/or animation sectioning data is too large.\n" +
                        "At least one of the two must be repointed before attempting to write.",
                            this.CurrentEntry,
                        new Tuple<String, Pointer, Int32>[2] {
                            Tuple.Create("Sections", (Pointer)this.Current["Sections"], sections.Length),
                            Tuple.Create("Anim Data", (Pointer)this.Current["AnimData"], animdata.Length)},
                        new Pointer[2] {
                            this.Current.GetAddress(this.Current.EntryIndex, "Sections"),
                            this.Current.GetAddress(this.Current.EntryIndex, "AnimData")
                        });
                    if (cancel) return;
                }

                Core.WriteData(this,
                    (Pointer)this.Current["AnimData"],
                    animdata,
                    this.CurrentEntry + "Animation code changed");

                Core.WriteData(this,
                    (Pointer)this.Current["Sections"],
                    sections,
                    this.CurrentEntry + "Animation sections changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert animation data.", ex);
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        private void AnimCode_Reset_Button_Click(Object sender, EventArgs e)
        {
            this.Core_LoadAnimCode();
        }
        private void AnimCode_TextBox_TextChanged(Object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            String[] animcode = this.AnimCodeBox.Text.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (this.View_AllAnimCode.Checked)
            {
                String[][] result = new String[BattleAnimation.MODES][];
                Int32 index = 0;
                for (Int32 i = 0; i < BattleAnimation.MODES; i++)
                {
                    while (animcode[index] != "end") index++;
                    result[i] = new String[++index];
                    Array.Copy(animcode, index, result[i], 0, result[i].Length);
                }
                this.CurrentAnim.AnimCode = result;
            }
            else
            {
                this.CurrentAnim.AnimCode[this.CurrentMode] = animcode;
            }
        }
        
        private void PlayAnimButton_Click(Object sender, EventArgs e)
        {
            this.Anim_Play_Button.Enabled = false;

            this.View_AllFrames.Checked = false;

            this.PlayAnimTimer.Enabled = true;
        }
        private void PlayAnimTimer_Tick(Object sender, EventArgs e)
        {
            if (this.Frame_ByteBox.Value >= this.Frame_ByteBox.Maximum ||
                this.Frame_ByteBox.Value >= this.Frames[this.CurrentMode].Count)
            {
                this.Anim_Play_Button.Enabled = true;
                this.PlayAnimTimer.Enabled = false;

                this.Frame_ByteBox.Value = 0;
            }
            else
            {
                this.Frame_ByteBox.Value++;
                this.PlayAnimTimer.Interval = (Int32)Math.Round(this.Durations[this.CurrentMode][this.Frame_ByteBox.Value] * 1000f / 60f);
            }
        }
    }
}
