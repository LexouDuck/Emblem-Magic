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
                return "Battle Anim 0x" + Util.ByteToHex(Entry_ArrayBox.Value) + " [" + Entry_ArrayBox.Text + "] - ";
            }
        }
        /// <summary>
        /// Gets the number of the current frame being viewed, irrespective of the 'view all frames' option
        /// </summary>
        Byte CurrentFrame
        {
            get
            {
                return (View_AllFrames.Checked) ?
                    Frame_ByteBox.Value :
                    Frames[CurrentMode][Frame_ByteBox.Value];
            }
        }
        /// <summary>
        /// The index of the currently selected animation mode, irrespective of the "view 2layer modes as one" option
        /// </summary>
        Int32 CurrentMode
        {
            get
            {
                if (View_Layered.Checked)
                {
                    switch (Anim_Mode_ListBox.SelectedIndex)
                    {
                        case 0: return 0;
                        case 1: return 2;
                        case 2: case 3: case 4: case 5: case 6:
                            return Anim_Mode_ListBox.SelectedIndex + 2;
                        case 7: return 10;
                        case 8: return 11;
                        default: return -1;
                    }
                }
                else return Anim_Mode_ListBox.SelectedIndex;
            }
        }
        /// <summary>
        /// Gets the current palette, depending on whether default palettes or character palettes is checked
        /// </summary>
        Palette CurrentPalette
        {
            get
            {
                return (Palette_Default_Button.Checked) ?
                    DefaultPalette[Palette_Default_ArrayBox.Value] :
                    CharacterPalette[Palette_Character_Current_ArrayBox.Value];
            }
        }
        /// <summary>
        /// Gets a string of the currently selected character plaette
        /// </summary>
        String CurrentPaletteEntry
        {
            get
            {
                return "Character Palette 0x" + Palette_Character_ArrayBox.Value + " [" + Palette_Character_ArrayBox.Text + "] - ";
            }
        }
        /// <summary>
        /// Gets a string of the current class/item battle anim association entry
        /// </summary>
        String CurrentItemPointerEntry(Int32 word)
        {
            return "Battle Anim association at " + Item_PointerArrayBox.Value +
                                            " [" + Item_PointerArrayBox.Text + "], word " + word + " - ";
        }



        public BattleAnimEditor(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();

                Entry_ArrayBox.Load("Battle Animation List.txt");
                Current = new StructFile("Battle Animation Struct.txt");
                Current.Address = Core.GetPointer("Battle Animation Array");

                Item_PointerArrayBox.Load("Battle Animation Pointers.txt");

                Palette_Default_ArrayBox.Load("Battle Animation Palettes.txt");
                Palette_Character_ArrayBox.Load("Character Palettes.txt");
                Palette_Character_Current_ArrayBox.Load("Battle Animation Character Palettes.txt");
                Palettes = new StructFile("Character Palette Struct.txt");
                Palettes.Address = Core.GetPointer("Character Palette Array");

                AnimCodeBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                Anim_Name_TextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                Palette_Character_TextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);

                Anim_Mode_ListBox.DataSource = BattleAnimation.Modes;
                Anim_Mode_ListBox.SelectedIndexChanged += AnimListBox_SelectedIndexChanged;

                AnimCodeBox.AddSyntax("#.*", SystemColors.ControlDark);
                AnimCodeBox.AddSyntax(@"(a|b|c|d|f|end)(?=[0-9a-fA-F][0-9a-fA-F])", SystemColors.Highlight, FontStyle.Bold);
                AnimCodeBox.AddSyntax(@"(end)", SystemColors.Highlight, FontStyle.Bold);
                AnimCodeBox.AddSyntax(@"(?<=(a|b|c|d|f))([0-9a-fA-F][0-9a-fA-F])", System.Drawing.Color.SlateBlue);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        override public void Core_SetEntry(UInt32 entry)
        {
            Item_PointerArrayBox.Value = new Pointer(entry);
            if (entry != 0)
                Entry_ArrayBox.Value = ((ByteArrayBox)Item_LayoutPanel.Controls[0].Controls[1]).Value;
        }
        override public void Core_OnOpen()
        {
            Entry_ArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;
            Entry_ArrayBox.Value = 1;
            Entry_ArrayBox.ValueChanged += EntryArrayBox_ValueChanged;

            Palette_Character_ArrayBox.ValueChanged -= Palette_Character_ArrayBox_ValueChanged;
            Palette_Character_ArrayBox.Value = 1;
            Palette_Character_ArrayBox.ValueChanged += Palette_Character_ArrayBox_ValueChanged;

            Core_Update();
        }
        override public void Core_Update()
        {
            Current.EntryIndex = Entry_ArrayBox.Value - 1;
            Palettes.EntryIndex = Palette_Character_ArrayBox.Value - 1;

            Core_LoadAnimation();
            Core_LoadPalettes();
            Core_LoadAnimCode();
            Core_LoadValues();
            Core_LoadPaletteValues();
            Core_LoadItemValues();

            Core_UpdatePalettes();
            Core_UpdateImageBox();
        }

        void Core_LoadAnimation()
        {
            try
            {
                Byte[] animdata = Core.ReadData((Pointer)Current["AnimData"], 0);
                Byte[] oam_L = Core.ReadData((Pointer)Current["OAM_Left"], 0);
                Byte[] oam_R = Core.ReadData((Pointer)Current["OAM_Right"], 0);
                UInt32[] sections = new UInt32[12];
                Byte[] buffer;
                for (Int32 i = 0; i < sections.Length; i++)
                {
                    buffer = Core.ReadData((Pointer)Current["Sections"] + i * 4, 4);
                    sections[i] = Util.BytesToUInt32(buffer, true);
                }

                CurrentAnim = new BattleAnimation(sections, animdata, oam_L, oam_R);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the animation.", ex);
                CurrentAnim = null;
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
                address = (Pointer)Current["Palettes"];
                is_uncompressed = (address > 0x80000000);
                if (address >= 0x80000000)
                    address -= 0x80000000;
                palettes = Core.ReadPalette(address, (is_uncompressed ? Palette.LENGTH : 0) * paletteAmount);
                DefaultPalette = Palette.Split(Palette.Opacify(palettes), paletteAmount);

                paletteAmount = 5;
                address = (Pointer)Palettes["Address"];
                is_uncompressed = (address > 0x80000000);
                if (address >= 0x80000000)
                    address -= 0x80000000;
                palettes = Core.ReadPalette(address, (is_uncompressed ? Palette.LENGTH : 0) * paletteAmount);
                CharacterPalette = Palette.Split(Palette.Opacify(palettes), paletteAmount);

                Palette_PaletteBox.Load(CurrentPalette);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the palettes.", ex);
            }
        }
        void Core_LoadAnimCode()
        {
            if (CurrentAnim == null)
                return;
            List<Tuple<UInt32, OAM>> not_used = null; Int32[] not_used_either = null;
            String result = BattleAnimation.GetAnimationCode(
                    CurrentAnim.AnimCode,
                    CurrentMode,
                    View_Layered.Checked,
                    View_AllAnimCode.Checked,
                    ref not_used, ref not_used_either);

            AnimCodeBox.TextChanged -= AnimCode_TextBox_TextChanged;
            AnimCodeBox.Text = result;
            AnimCodeBox.TextChanged -= AnimCode_TextBox_TextChanged;

            String[] animcode;
            Frames = new List<Byte>[BattleAnimation.MODES];
            Durations = new List<Int32>[BattleAnimation.MODES];
            for (Int32 mode = 0; mode < Frames.Length; mode++)
            {
                Frames[mode] = new List<Byte>();
                Durations[mode] = new List<Int32>();
                animcode = CurrentAnim.AnimCode[mode];

                for (Int32 i = 0; i < animcode.Length; i++)
                {
                    if (animcode[i][0] >= '0' && animcode[i][0] <= '9')
                    {
                        Durations[mode].Add(Int32.Parse(animcode[i].Substring(0, animcode[i].IndexOf(' '))));
                        for (Int32 j = 0; j < animcode[i].Length; j++)
                        {
                            if (animcode[i][j] == 'f' || animcode[i][j] == 'F')
                            {
                                Frames[mode].Add(Util.HexToByte(animcode[i].Substring(j + 1, 2)));
                                break;
                            }
                        }
                    }
                }
            }   // Create the frame array for all the animation modes of the current anim

            if (View_AllFrames.Checked)
                 Frame_ByteBox.Maximum = Math.Max(0, CurrentAnim.Frames.Length - 1);
            else Frame_ByteBox.Maximum = Math.Max(0, Frames[CurrentMode].Count - 1);
        }
        void Core_LoadValues()
        {
            Anim_Name_TextBox.TextChanged -= Anim_NameTextBox_TextChanged;
            Sections_PointerBox.ValueChanged -= SectionsPointerBox_ValueChanged;
            AnimData_PointerBox.ValueChanged -= AnimDataPointerBox_ValueChanged;
            OAM_R_PointerBox.ValueChanged -= OAM_R_PointerBox_ValueChanged;
            OAM_L_PointerBox.ValueChanged -= OAM_L_PointerBox_ValueChanged;
            Palette_Default_PointerBox.ValueChanged -= Palette_Default_PointerBox_ValueChanged;

            try
            {
                Anim_Name_TextBox.Text = (String)Current["Name"];
                Sections_PointerBox.Value        = (Pointer)Current["Sections"];
                AnimData_PointerBox.Value        = (Pointer)Current["AnimData"];
                OAM_R_PointerBox.Value           = (Pointer)Current["OAM_Right"];
                OAM_L_PointerBox.Value           = (Pointer)Current["OAM_Left"];
                Palette_Default_PointerBox.Value = (Pointer)Current["Palettes"];
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);

                Anim_Name_TextBox.Text = "";
                Sections_PointerBox.Value = new Pointer();
                AnimData_PointerBox.Value = new Pointer();
                OAM_L_PointerBox.Value = new Pointer();
                OAM_R_PointerBox.Value = new Pointer();
                Palette_Default_PointerBox.Value = new Pointer();
            }

            Anim_Name_TextBox.TextChanged += Anim_NameTextBox_TextChanged;
            Sections_PointerBox.ValueChanged += SectionsPointerBox_ValueChanged;
            AnimData_PointerBox.ValueChanged += AnimDataPointerBox_ValueChanged;
            OAM_R_PointerBox.ValueChanged += OAM_R_PointerBox_ValueChanged;
            OAM_L_PointerBox.ValueChanged += OAM_L_PointerBox_ValueChanged;
            Palette_Default_PointerBox.ValueChanged += Palette_Default_PointerBox_ValueChanged;
        }
        void Core_LoadPaletteValues()
        {
            Palette_Character_PointerBox.ValueChanged -= Palette_Character_PointerBox_ValueChanged;
            Palette_Character_TextBox.TextChanged -= Palette_Character_TextBox_TextChanged;

            try
            {
                Pointer address = (Pointer)Palettes["Address"];
                if (address >= 0x80000000)
                    address -= 0x80000000;
                Palette_Character_PointerBox.Value = address;
                Palette_Character_TextBox.Text = (String)Palettes["Name"];
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the character palette values.", ex);

                Palette_Character_PointerBox.Value = new Pointer();
                Palette_Character_TextBox.Text = "";
            }

            Palette_Character_PointerBox.ValueChanged += Palette_Character_PointerBox_ValueChanged;
            Palette_Character_TextBox.TextChanged += Palette_Character_TextBox_TextChanged;
        }
        void Core_LoadItemValues()
        {
            for (Int32 i = 0; i < Item_LayoutPanel.Controls.Count; i++)
            {
                for (Int32 j = 0; j < Item_LayoutPanel.Controls[i].Controls.Count; j++)
                {
                    Item_LayoutPanel.Controls[i].Controls[j].Dispose();
                }
                Item_LayoutPanel.Controls[i].Dispose();
            }
            Item_LayoutPanel.Controls.Clear();

            Pointer address = Item_PointerArrayBox.Value;
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
                Item_Controls = new Control[length * 8];
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
                        Item_LayoutPanel.Controls.Add(groupboxes[i]);

                        Core_LoadItemControl(groupboxes[i], i, ref index,
                            "Anim", word["Anim"], word.GetAddress(i, "Anim"), Entry_ArrayBox.File);
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
                         "\r\n\r\n" + "Error at word " + (index / 8) + ", control is " + Item_Controls[index].Name, ex);
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
            Item_Controls[index++] = label;
            Item_Controls[index] = new ByteArrayBox()
            {
                Name = name.Substring(0, 4) + "_" + num,
                Location = new Point(70, 20 + 30 * control_index),
                Size = new Size(160, 20),
            };
            if (entrylist != null)
                ((ByteArrayBox)Item_Controls[index]).Load(entrylist);
            box.Controls.Add(label);
            box.Controls.Add(Item_Controls[index]);
            ((ByteArrayBox)Item_Controls[index]).Name = name.Substring(0, 4) + "_" + num;
            if (radiobutton)
            {
                if (boolvalue == radiovalue)
                    ((ByteArrayBox)Item_Controls[index]).Value = value;
                else
                {
                    ((ByteArrayBox)Item_Controls[index]).Value = 0x00;
                    ((ByteArrayBox)Item_Controls[index]).Enabled = false;
                }
                Byte notradiovalue = (radiovalue == 0x00) ? (Byte)0x01 : (Byte)0x00;
                ((RadioButton)label).CheckedChanged += delegate (Object sender, EventArgs e)
                {
                    Int32 word = Int32.Parse(((RadioButton)sender).Name.Substring(5));
                    Boolean radiochecked = ((RadioButton)sender).Checked;
                    if (((RadioButton)sender).Name.Substring(0, 4) == "Type")
                    {
                        Item_Controls[word * 8 + 3].Enabled = radiochecked;
                        Item_Controls[word * 8 + 5].Enabled = !radiochecked;
                    }
                    else
                    {
                        Item_Controls[word * 8 + 3].Enabled = !radiochecked;
                        Item_Controls[word * 8 + 5].Enabled = radiochecked;
                    }
                    Core.WriteByte(this,
                        address + 1,
                        (Byte)(((RadioButton)sender).Checked ? radiovalue : notradiovalue),
                        CurrentItemPointerEntry(num) + "Bool " + num + " changed");
                };
            }
            else ((ByteArrayBox)Item_Controls[index]).Value = value;

            ((ByteArrayBox)Item_Controls[index]).ValueChanged += delegate (Object sender, EventArgs e)
            {
                Core.WriteByte(this,
                    address,
                    ((ByteBox)sender).Value,
                    CurrentItemPointerEntry(num) + name + " " + num + " changed");
            };
            index++;
        }

        void Core_UpdateImageBox()
        {
            try
            {
                CurrentAnim.ShowFrame(CurrentPalette, CurrentFrame, OAM_L_Button.Checked);

                if (!View_AllFrames.Checked && View_Layered.Checked &&
                    (CurrentMode == 0 || CurrentMode == 2 || CurrentMode == 8))
                {
                    BattleAnimFrame frame = CurrentAnim.Frames[Frames[CurrentMode + 1][Frame_ByteBox.Value]];

                    CurrentAnim.AddSprite(CurrentPalette,
                        CurrentAnim.Tilesets[frame.TilesetIndex],
                        OAM_L_Button.Checked ? frame.OAM_Data_L : frame.OAM_Data_R,
                        OAM_L_Button.Checked ? BattleAnimation.SCREEN_OFFSET_X_L : BattleAnimation.SCREEN_OFFSET_X_R,
                        BattleAnimation.SCREEN_OFFSET_Y);
                }
                Anim_ImageBox.Load(CurrentAnim);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to show the animation frame.", ex);

                Anim_ImageBox.Reset();
            }
        }
        void Core_UpdatePalettes()
        {
            try
            {
                Palette_PaletteBox.Load(CurrentPalette);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while updating the displayed palette.", ex);

                Palette_PaletteBox.Reset();
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

                List<Pointer> pointers = BattleAnimation.GetTilesetPointers(Core.ReadData((Pointer)Current["AnimData"], 0));

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
                    CurrentEntry, new Tuple<String, Pointer, Int32>[5] {
                        Tuple.Create("Anim Data Sections", (Pointer)Current["Sections"], sections.Length),
                        Tuple.Create("Anim Data",          (Pointer)Current["AnimData"], animdata.Length),
                        Tuple.Create("Right-side OAM",     (Pointer)Current["OAM_Right"],oam_data.Item2.Length),
                        Tuple.Create("Left-side OAM",      (Pointer)Current["OAM_Left"], oam_data.Item1.Length),
                        Tuple.Create("Palettes",           (Pointer)Current["Palettes"], palettes.Length)},
                    new Pointer[5] {
                        Current.GetAddress(Current.EntryIndex, "Sections"),
                        Current.GetAddress(Current.EntryIndex, "AnimData"),
                        Current.GetAddress(Current.EntryIndex, "OAM_Right"),
                        Current.GetAddress(Current.EntryIndex, "OAM_Left"),
                        Current.GetAddress(Current.EntryIndex, "Palettes")});
                if (cancel) return;

                for (Int32 i = 0; i < tilesets.Count; i++)
                {
                    Core.WriteData(this,
                        pointers[i], graphics[i],
                        CurrentEntry + "Tileset " + i + " changed");
                }

                Core.WriteData(this,
                    (Pointer)Current["Sections"], sections,
                    CurrentEntry + "Anim Data Sections changed");

                Core.WriteData(this,
                    (Pointer)Current["AnimData"], animdata,
                    CurrentEntry + "Anim Data changed");

                Core.WriteData(this,
                    (Pointer)Current["OAM_Right"], oam_data.Item2,
                    CurrentEntry + "Right-side OAM changed");

                Core.WriteData(this,
                    (Pointer)Current["OAM_Left"], oam_data.Item1,
                    CurrentEntry + "Left-side OAM changed");

                Core.WriteData(this,
                    (Pointer)Current["Palettes"], palettes,
                    CurrentEntry + "Palettes changed");
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
                Int32[] duplicates = new Int32[CurrentAnim.Frames.Length];
                for (Int32 i = 0; i < duplicates.Length; i++) duplicates[i] = -1;
                String animcode = BattleAnimation.GetAnimationCode(
                    CurrentAnim.AnimCode,
                    CurrentMode, true, true,
                    ref affines, ref duplicates,
                    file, CurrentAnim.Frames);
                File.WriteAllText(path + file + ".txt", animcode);

                Byte[] palette = CurrentPalette.ToBytes(false);
                Palette[] palettes = (Palette_Default_Button.Checked) ? DefaultPalette : CharacterPalette;
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
                    GBA.Image tileset = CurrentAnim.Tilesets[affines[i].Item1].ToImage(32, 8, palette);
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
                
                for (Byte i = 0; i < CurrentAnim.Frames.Length; i++)
                {
                    if (CurrentAnim.Frames[i] == null)
                        continue;
                    if (duplicates[i] != -1)
                        continue;
                    CurrentAnim.Clear();
                    CurrentAnim.AddSprite(CurrentPalette,
                        CurrentAnim.Tilesets[CurrentAnim.Frames[i].TilesetIndex],
                        CurrentAnim.Frames[i].OAM_Data_R,
                        BattleAnimation.SCREEN_OFFSET_X_R,
                        BattleAnimation.SCREEN_OFFSET_Y,
                        false);
                    if (CurrentAnim.Count > 0)
                    {
                        Core.SaveImage(path + file + "_" + i,
                            CurrentAnim.Width,
                            CurrentAnim.Height,
                            new Palette[1] { CurrentPalette },
                            delegate (Int32 x, Int32 y)
                            {
                                return (Byte)CurrentAnim[x, y];
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save battle animation to folder.", ex);
            }
            Core_Update();
        }
        /// <summary>
        /// Exports the current animation into FEA (with .oam, .pal and the sheets)
        /// </summary>
        void Core_SaveAnimFiles(String filepath)
        {
            try
            {
                Byte[] fea = Core.ReadData((Pointer)Current["AnimData"], 0);
                Byte[] oam = Core.ReadData((Pointer)Current["OAM_Right"], 0);
                Byte[] pal = Core.ReadData((Pointer)Current["Palettes"], 0);
            
                GBA.Image[] sheets = new GBA.Image[CurrentAnim.Tilesets.Length];
                Byte[] palette = DefaultPalette[0].ToBytes(false);
                for (Int32 i = 0; i < sheets.Length; i++)
                {
                    sheets[i] = CurrentAnim.Tilesets[i].ToImage(32, 8, palette);
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
                gif.AddRange(Util.UInt16ToBytes((UInt16)CurrentAnim.Width, true)); // Logical screen width
                gif.AddRange(Util.UInt16ToBytes((UInt16)CurrentAnim.Height, true)); // Logical screen height
                gif.Add(0xB3); // Packed field - 0xB3 indicates a 16-color global palette
                gif.Add(0x00); // Background Color Index in the color table
                gif.Add(0x00); // Pixel Aspect Ratio - not really used apparently
                // Global Color Table:
                for (Int32 i = 0; i < Palette.MAX; i++)
                {   // set the Global Color Table, 3 bytes per color
                    UInt32 color = CurrentPalette[i].To32bit();
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
                for (Int32 i = 0; i < CurrentAnim.AnimCode[mode].Length; i++)
                {
                    if (CurrentAnim.AnimCode[mode][i].StartsWith("c01") || // Wait for HP deplete
                        CurrentAnim.AnimCode[mode][i].StartsWith("c13"))   // Wait for Handaxe return
                    {
                        wait_frames.Add(wait_frame - 1);
                    }
                    else if (CurrentAnim.AnimCode[mode][i].IndexOf(" f") > 0 &&
                        !CurrentAnim.AnimCode[mode][i].StartsWith("c") &&
                        !CurrentAnim.AnimCode[mode][i].StartsWith("#"))
                    {
                        wait_frame += 1;
                    }
                }
                //MessageBox.Show(string.Join(";", wait_frames));
                for (Byte i = 0; i < Frames[mode].Count; i++)
                {
                    CurrentAnim.ShowFrame(CurrentPalette, Frames[mode][i], OAM_L_Button.Checked);
                    UInt16 duration = (UInt16)Math.Round(Durations[mode][i] * (100f / 60f));
                    if (i == 0 || wait_frames.Contains(i))
                        duration = 60;
                    else if (browserFriendly && duration < 4)
                        duration = 4;
                    if (!View_AllFrames.Checked && View_Layered.Checked && (mode == 0 || mode == 2 || mode == 8))
                    {
                        BattleAnimFrame frame = CurrentAnim.Frames[Frames[mode + 1][i]];

                        CurrentAnim.AddSprite(CurrentPalette,
                            CurrentAnim.Tilesets[frame.TilesetIndex],
                            OAM_L_Button.Checked ? frame.OAM_Data_L : frame.OAM_Data_R,
                            OAM_L_Button.Checked ? BattleAnimation.SCREEN_OFFSET_X_L : BattleAnimation.SCREEN_OFFSET_X_R,
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
                    gif.AddRange(Util.UInt16ToBytes((UInt16)CurrentAnim.Width, true)); // Image Width
                    gif.AddRange(Util.UInt16ToBytes((UInt16)CurrentAnim.Height, true));// Image Height
                    gif.Add(0x00); // Packed field - 0x00 indicates no interlacing, no local color table
                    Byte[] pixels = new Byte[CurrentAnim.Width * CurrentAnim.Height];
                    Int32 index = 0;
                    for (Int32 y = 0; y < CurrentAnim.Height; y++)
                    for (Int32 x = 0; x < CurrentAnim.Width; x++)
                    {
                        pixels[index++] = (Byte)CurrentAnim[x, y];
                    }
                    gif.AddRange(new LZW(CurrentAnim.Width, CurrentAnim.Height, pixels, 0x04).Compress());
                }
                gif.Add(0x3B); // Trailer (0x3B) - every GIF file ends with this byte

                File.WriteAllBytes(filepath, gif.ToArray());
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save the animation to GIF file.", ex); return;
            }
            CurrentAnim.ShowFrame(CurrentPalette, CurrentFrame, OAM_L_Button.Checked);
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
                Core_CreateAnim(openWindow.FileName);
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
                Core_InsertAnim(openWindow.FileName, Entry_ArrayBox.Value);
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
            saveWindow.FileName = Entry_ArrayBox.Text;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                Core_SaveAnimFolder(saveWindow.FileName);
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
            saveWindow.FileName = Entry_ArrayBox.Text;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                Core_SaveAnimFiles(saveWindow.FileName);
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
            saveWindow.FileName = Entry_ArrayBox.Text + " - " + Anim_Mode_ListBox.GetItemText(Anim_Mode_ListBox.SelectedItem);

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                Core_SaveAnimGIF(saveWindow.FileName,
                    CurrentMode, saveWindow.FilterIndex == 1);
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
            saveWindow.FileName = Entry_ArrayBox.Text;

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                String filename = saveWindow.FileName;
                if (filename.EndsWith(".gif"))
                    filename = filename.Substring(0, filename.Length - 4);
                Int32 length = View_Layered.Checked ? BattleAnimation.Modes_Layered.Length : BattleAnimation.Modes.Length;
                Int32 mode = 0;
                for (Int32 i = 0; i < length; i++)
                {
                    Anim_Mode_ListBox.SelectedIndex = i;
                    Core_SaveAnimGIF(filename + " - " + Anim_Mode_ListBox.GetItemText(Anim_Mode_ListBox.SelectedItem) + ".gif",
                        mode, saveWindow.FilterIndex == 1);
                    if (View_Layered.Checked && (mode == 0 || mode == 2 || mode == 8))
                        mode++;
                    mode++;
                }
                Anim_Mode_ListBox.SelectedIndex = 0;
            }
        }

        private void Tool_OpenOAMEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenOAMEditor(this,
                CurrentEntry + "Frame 0x" + Util.ByteToHex(CurrentFrame) + " - ",
                OAM_R_Button.Checked ? OAM_R_PointerBox.Value : OAM_L_PointerBox.Value,
                (Int32)CurrentAnim.Frames[CurrentFrame].OAM_Offset,
                OAM_R_Button.Checked ?
                    BattleAnimation.SCREEN_OFFSET_X_R :
                    BattleAnimation.SCREEN_OFFSET_X_L,
                BattleAnimation.SCREEN_OFFSET_Y,
                CurrentPalette,
                CurrentAnim.Tilesets[CurrentAnim.Frames[CurrentFrame].TilesetIndex]);
        }
        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            Pointer address;
            Boolean is_uncompressed;
            if (Palette_Default_Button.Checked)
            {
                address = (Pointer)Current["Palettes"];
                is_uncompressed = (address >= 0x80000000);
                UI.OpenPaletteEditor(this,
                    CurrentEntry,
                    (is_uncompressed ? address - 0x80000000 : address),
                    (is_uncompressed ? 5 : 0));
            }
            else
            {
                address = (Pointer)Palettes["Address"];
                is_uncompressed = (address >= 0x80000000);
                UI.OpenPaletteEditor(this,
                    "Character Palette 0x" + Palette_Character_ArrayBox.Value + " [" + Palette_Character_ArrayBox.Text + "] - ",
                    (is_uncompressed ? address - 0x80000000 : address),
                    (is_uncompressed ? 5 : 0));
            }
        }

        private void View_AllFrames_Click(Object sender, EventArgs e)
        {
            Frame_ByteBox.ValueChanged -= FrameByteBox_ValueChanged;
            Frame_ByteBox.Value = 0;
            Frame_ByteBox.ValueChanged += FrameByteBox_ValueChanged;

            Core_LoadAnimCode();
            Core_UpdateImageBox();
        }
        private void View_Layered_Click(Object sender, EventArgs e)
        {
            if (View_Layered.Checked)
                 Anim_Mode_ListBox.DataSource = BattleAnimation.Modes_Layered;
            else Anim_Mode_ListBox.DataSource = BattleAnimation.Modes;

            Core_UpdateImageBox();
        }
        private void View_AllAnimCode_Click(Object sender, EventArgs e)
        {
            Core_LoadAnimCode();
        }



        private void Item_PointerArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_LoadItemValues();
        }
        private void Item_AppendButton_Click(Object sender, EventArgs e)
        {
            Pointer address = Item_PointerArrayBox.Value;
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
                CurrentItemPointerEntry((Int32)offset / 4) + "Bool changed");
        }
        private void Item_RemoveButton_Click(Object sender, EventArgs e)
        {
            Pointer address = Item_PointerArrayBox.Value;
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
                CurrentItemPointerEntry((Int32)offset / 4) + "Bool changed");
        }

        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Frame_ByteBox.ValueChanged -= FrameByteBox_ValueChanged;
            Frame_ByteBox.Value = 0;
            Frame_ByteBox.ValueChanged += FrameByteBox_ValueChanged;

            Core_Update();
        }
        private void FrameByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_UpdateImageBox();
        }
        private void AnimListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Frame_ByteBox.ValueChanged -= FrameByteBox_ValueChanged;
            Frame_ByteBox.Value = 0;
            Frame_ByteBox.ValueChanged += FrameByteBox_ValueChanged;

            Core_Update();
        }

        private void Anim_NameTextBox_TextChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                Current.Address,
                ByteArray.Make_ASCII(Anim_Name_TextBox.Text),
                CurrentEntry + "Anim name changed");
        }
        private void OAM_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_UpdateImageBox();
        }
        
        private void SectionsPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Sections"),
                Sections_PointerBox.Value,
                CurrentEntry + "Anim Data Sections repoint");
        }
        private void AnimDataPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "AnimData"),
                AnimData_PointerBox.Value,
                CurrentEntry + "Anim Data repoint");
        }
        private void OAM_R_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "OAM_Right"),
                OAM_R_PointerBox.Value,
                CurrentEntry + "Right-side OAM repoint");
        }
        private void OAM_L_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "OAM_Left"),
                OAM_L_PointerBox.Value,
                CurrentEntry + "Left-side OAM repoint");
        }

        private void Palette_CheckedChanged(Object sender, EventArgs e)
        {
            Palette_Default_ArrayBox.Enabled        = Palette_Default_Button.Checked;
            Palette_Default_PointerBox.Enabled      = Palette_Default_Button.Checked;
            Palette_Character_TextBox.Enabled          = Palette_Character_Button.Checked;
            Palette_Character_ArrayBox.Enabled         = Palette_Character_Button.Checked;
            Palette_Character_PointerBox.Enabled       = Palette_Character_Button.Checked;
            Palette_Character_Current_ArrayBox.Enabled = Palette_Character_Button.Checked;
            Core_UpdatePalettes();
            Core_UpdateImageBox();
        }
        private void Palette_Default_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (Palette_Default_Button.Checked)
            {
                Core_UpdatePalettes();
                Core_UpdateImageBox();
            }
        }
        private void Palette_Default_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Palettes"),
                Palette_Default_PointerBox.Value,
                CurrentEntry + "Default Palettes repoint");
        }
        private void Palette_Character_TextBox_TextChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                Current.Address,
                ByteArray.Make_ASCII(Anim_Name_TextBox.Text),
                CurrentPaletteEntry + "Name changed");
        }
        private void Palette_Character_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Palettes.EntryIndex = Palette_Character_ArrayBox.Value - 1;
            Core_LoadPalettes();
            Core_LoadPaletteValues();
            if (Palette_Character_Button.Checked)
            {
                Core_UpdatePalettes();
                Core_UpdateImageBox();
            }
        }
        private void Palette_Character_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Palettes.GetAddress(Palettes.EntryIndex, "Address"),
                Palette_Character_PointerBox.Value,
                CurrentPaletteEntry + "repoint");
        }
        private void Palette_Character_Current_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            if (Palette_Character_Button.Checked)
            {
                Core_UpdatePalettes();
                Core_UpdateImageBox();
            }
        }

        private void AnimCode_Apply_Button_Click(Object sender, EventArgs e)
        {
            Tuple<UInt32, UInt32>[] frames = new Tuple<UInt32, UInt32>[CurrentAnim.Frames.Length];
            for (Int32 i = 0; i < frames.Length; i++)
            {
                if (CurrentAnim.Frames[i] != null)
                    frames[i] = Tuple.Create(
                        CurrentAnim.Frames[i].TilesetIndex,
                        CurrentAnim.Frames[i].OAM_Offset);
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
                    if (View_AllAnimCode.Checked)
                    {
                        length = AnimCodeBox.Text.IndexOf("end", index) + 3 - index;
                        animcode = AnimCodeBox.Text.Substring(index, length).Split(
                            new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        index += length;

                        if (View_Layered.Checked && (i == 0 || i == 2 || i == 8)) // two-layer anim modes
                            layers = BattleAnimation.SplitLayeredAnimationCode(animcode);
                    }
                    else if (CurrentMode == i)
                    {
                        animcode = AnimCodeBox.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        if (View_Layered.Checked && (i == 0 || i == 2 || i == 8)) // two-layer anim modes
                            layers = BattleAnimation.SplitLayeredAnimationCode(animcode);
                    }
                    else
                    {
                        animcode = CurrentAnim.AnimCode[i];
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
                    BattleAnimation.GetTilesetPointers(Core.ReadData((Pointer)Current["AnimData"], 0))));
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while compiling the animation code.", ex);
                return;
            }

            UI.SuspendUpdate();
            try
            {
                Pointer sections_pointer = (Pointer)Current["Sections"];
                Pointer animdata_pointer = (Pointer)Current["AnimData"];
                if ((sections_pointer > animdata_pointer) && (animdata.Length > (animdata_pointer - sections_pointer))
                 || (animdata_pointer > sections_pointer) && (sections.Length > (sections_pointer - animdata_pointer)))
                {
                    Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Animation Data",
                        "The animation code and/or animation sectioning data is too large.\n" +
                        "At least one of the two must be repointed before attempting to write.",
                            CurrentEntry,
                        new Tuple<String, Pointer, Int32>[2] {
                            Tuple.Create("Sections", (Pointer)Current["Sections"], sections.Length),
                            Tuple.Create("Anim Data", (Pointer)Current["AnimData"], animdata.Length)},
                        new Pointer[2] {
                            Current.GetAddress(Current.EntryIndex, "Sections"),
                            Current.GetAddress(Current.EntryIndex, "AnimData")
                        });
                    if (cancel) return;
                }

                Core.WriteData(this,
                    (Pointer)Current["AnimData"],
                    animdata,
                    CurrentEntry + "Animation code changed");

                Core.WriteData(this,
                    (Pointer)Current["Sections"],
                    sections,
                    CurrentEntry + "Animation sections changed");
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
            Core_LoadAnimCode();
        }
        private void AnimCode_TextBox_TextChanged(Object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            String[] animcode = AnimCodeBox.Text.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (View_AllAnimCode.Checked)
            {
                String[][] result = new String[BattleAnimation.MODES][];
                Int32 index = 0;
                for (Int32 i = 0; i < BattleAnimation.MODES; i++)
                {
                    while (animcode[index] != "end") index++;
                    result[i] = new String[++index];
                    Array.Copy(animcode, index, result[i], 0, result[i].Length);
                }
                CurrentAnim.AnimCode = result;
            }
            else
            {
                CurrentAnim.AnimCode[CurrentMode] = animcode;
            }
        }
        
        private void PlayAnimButton_Click(Object sender, EventArgs e)
        {
            Anim_Play_Button.Enabled = false;

            View_AllFrames.Checked = false;

            PlayAnimTimer.Enabled = true;
        }
        private void PlayAnimTimer_Tick(Object sender, EventArgs e)
        {
            if (Frame_ByteBox.Value >= Frame_ByteBox.Maximum ||
                Frame_ByteBox.Value >= Frames[CurrentMode].Count)
            {
                Anim_Play_Button.Enabled = true;
                PlayAnimTimer.Enabled = false;

                Frame_ByteBox.Value = 0;
            }
            else
            {
                Frame_ByteBox.Value++;
                PlayAnimTimer.Interval = (Int32)Math.Round(Durations[CurrentMode][Frame_ByteBox.Value] * 1000f / 60f);
            }
        }
    }
}
