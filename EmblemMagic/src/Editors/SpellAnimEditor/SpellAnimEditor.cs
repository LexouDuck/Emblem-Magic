using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EmblemMagic.FireEmblem;
using GBA;
using Magic;
using Magic.Components;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class SpellAnimEditor : Editor
    {
        SpellAnimation CurrentAnim;
        SpellCommands Commands;

        List<Pointer> CurrentArray_Palette = new List<Pointer>(); Int32 CurrentIndex_Palette = 0;
        List<Pointer> CurrentArray_Tileset = new List<Pointer>(); Int32 CurrentIndex_Tileset = 0;
        List<Pointer> CurrentArray_TSA     = new List<Pointer>(); Int32 CurrentIndex_TSA = 0;
        List<Size> CurrentArray_Dimensions = new List<Size>();



        public SpellAnimEditor()
        {
            try
            {
                this.InitializeComponent();

                this.EntryArrayBox.Load("Spell Animations.txt");
                this.EntryArrayBox.Value = 0;
                this.EntryArrayBox.ValueChanged += this.EntryArrayBox_ValueChanged;

                this.Commands = new SpellCommands(Core.Path_Structs + "Spell Commands.txt");
                this.Anim_CodeBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                this.Anim_CodeBox.AddSyntax(ASM.GetInstructionsRegex_Thumb(), System.Drawing.Color.Black, FontStyle.Bold | FontStyle.Italic);
                this.Anim_CodeBox.AddSyntax(this.Commands.GetRegex(), SystemColors.Highlight);
                this.Anim_CodeBox.AddSyntax(@"((\b[0-9]+)|((\b0x|\$)[0-9a-fA-F]+))\b", System.Drawing.Color.SlateBlue);
                this.Anim_CodeBox.AddSyntax("return|label.*", SystemColors.ControlDark);
                this.Anim_CodeBox.AddSyntax("@.*", SystemColors.ControlDark);
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
                Pointer address = Core.ReadPointer(Core.GetPointer("Spell Animation Array") + 4 * this.EntryArrayBox.Value);

                this.CurrentAnim = new SpellAnimation(address);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load this spell animation.", ex);
            }
            this.Core_LoadAnimCode();
            this.Core_LoadValues();
            this.Core_FindAndDisplayGraphics();
        }

        void Core_UpdateDisplay()
        {
            const Int32 palettes = 8;
            Int32 width = (Int32)this.Width_NumberBox.Value;
            Int32 height = (Int32)this.Height_NumberBox.Value;

            Byte[] palette = Core.ReadData(
                this.Palette_PointerBox.Value,
                this.Palette_CheckBox.Checked ? 0 : palettes * Palette.LENGTH);
            Byte[] tileset = Core.ReadData(
                this.Tileset_PointerBox.Value,
                this.Tileset_CheckBox.Checked ? 0 : width * height * Tile.LENGTH);
            
            for (Int32 i = 0; i < palette.Length; i += 2)
            {
                palette[i + 1] &= 0x7F;
            }   // force opaque colors on display
            
            if (palette == null || palette.Length == 0)
            {
                this.Spell_PaletteBox.Reset();
                this.Spell_ImageBox.Reset();
                return;
            }
            if (tileset == null || tileset.Length == 0)
            {
                this.Spell_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                this.Spell_ImageBox.Reset();
                return;
            }

            IDisplayable image = null;
            try
            {
                if (this.TSA_Label.Checked && this.TSA_PointerBox.Value != new Pointer())
                {
                    image = new TSA_Image(palette, tileset, Core.ReadTSA(
                            this.TSA_PointerBox.Value,
                            width, height,
                            this.TSA_CheckBox.Checked, false));
                }
                else
                {
                    image = new Tileset(tileset).ToImage(
                            width, height, palette.GetBytes(0, Palette.LENGTH));
                }
                this.Spell_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                this.Spell_ImageBox.Load(image);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the image.", ex);
            }
        }
        List<Pointer> Core_FindAllInAnimCode(String match)
        {
            List<Pointer> result = new List<Pointer>();
            try
            {
                List<Int32> results = new List<Int32>();
                Pointer address = new Pointer();
                Boolean is_array = false;
                Boolean is_sprite = false;
                Int32 index = 0;
                for (Int32 i = 0; i < this.Anim_CodeBox.Text.Length; i = index + 3)
                {
                    index = this.Anim_CodeBox.Text.IndexOf(match, i);
                    if (index < 0)
                        break;
                    if (this.Anim_CodeBox.Text.Substring(index - 5, 5) == "Array")
                        is_array = true;
                    if (this.Anim_CodeBox.Text.Substring(index - 6, 6) == "Sprite")
                        is_sprite = true;
                    index = this.Anim_CodeBox.Text.IndexOf(")\r\n", index);
                    if (index < 0)
                        break;
                    Int32 index_of_pointer_arg;
                    index_of_pointer_arg = this.Anim_CodeBox.Text.LastIndexOf("$08", index) + 3;
                    if (index_of_pointer_arg < 0)
                        continue; // TODO throw exception here ?
                    address = new Pointer(Util.StringToAddress(this.Anim_CodeBox.Text.Substring(index_of_pointer_arg, index - index_of_pointer_arg)));
                    if (is_array)
                    {
                        // TODO change this loop's exit condition for something more reliable
                        for (Int32 offset = 0; offset < 0x100; offset += 4)
                        {
                            try
                            {
                                Byte pointer_msbyte = Core.ReadByte(address + offset + 3);
                                if (pointer_msbyte < 0x08 ||
                                    (pointer_msbyte & 0x70) != 0)
                                    break;
                                result.Add(Core.ReadPointer(address + offset));
                                if (match.Equals("_TSA("))
                                    this.CurrentArray_Dimensions.Add(is_sprite ?
                                        new Size(32, 8) :
                                        new Size(GBA.Screen.W_TILES, GBA.Screen.H_TILES));
                            }
                            catch { break; }
                        }
                    }
                    else
                    {
                        result.Add(address);
                        if (match.Equals("_TSA("))
                            this.CurrentArray_Dimensions.Add(is_sprite ?
                                new Size(32, 8) :
                                new Size(GBA.Screen.W_TILES, GBA.Screen.H_TILES));
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not parse anim code for graphics pointers.", ex);
            }
            return result;
        }
        void Core_FindAndDisplayGraphics()
        {
            this.CurrentArray_Palette = this.Core_FindAllInAnimCode("_Palette(");
            this.CurrentArray_Tileset = this.Core_FindAllInAnimCode("_Tileset(");
            this.CurrentArray_TSA     = this.Core_FindAllInAnimCode("_TSA(");

            if (this.CurrentArray_Palette.Count > 0)
            {
                this.Palette_PointerBox.Value = (this.CurrentArray_Palette[this.CurrentIndex_Palette]);
                this.Palette_CheckBox.Checked = (Core.ReadByte(this.CurrentArray_Palette[0]) == 0x10);
            }
            else this.Palette_PointerBox.Value = new Pointer();

            if (this.CurrentArray_Tileset.Count > 0)
            {
                this.Tileset_PointerBox.Value = (this.CurrentArray_Tileset[this.CurrentIndex_Tileset]);
                this.Tileset_CheckBox.Checked = (Core.ReadByte(this.CurrentArray_Tileset[0]) == 0x10);
            }
            else this.Tileset_PointerBox.Value = new Pointer();

            if (this.CurrentArray_TSA.Count > 0)
            {
                this.TSA_Label.Checked = true;
                this.TSA_PointerBox.Value     = (this.CurrentArray_TSA[this.CurrentIndex_TSA]);
                this.TSA_CheckBox.Checked     = (Core.ReadByte(this.CurrentArray_TSA[0]) == 0x10);
            }
            else this.TSA_PointerBox.Value = new Pointer();

            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }
        void Core_LoadAnimCode()
        {
            try
            {
                this.Constructor_PointerBox.Value = this.CurrentAnim.Address_Constructor;
                this.LoopRoutine_PointerBox.Value = this.CurrentAnim.Address_LoopRoutine;
                this.AnimLoading_PointerBox.Value = this.CurrentAnim.Address_AnimLoading;

                try { if (this.Constructor_Label.Checked) this.ASM_ListBox.DataSource = this.CurrentAnim.GetASM_Constructor(); }
                catch (Exception ex) { throw new Exception("Could not dissassemble constructor.", ex); }
                try { if (this.LoopRoutine_Label.Checked) this.ASM_ListBox.DataSource = this.CurrentAnim.GetASM_LoopRoutine(); }
                catch (Exception ex) { throw new Exception("Could not dissassemble loop routine.", ex); }
                try { if (this.AnimLoading_Label.Checked) this.ASM_ListBox.DataSource = this.CurrentAnim.GetASM_AnimLoading(); }
                catch (Exception ex) { throw new Exception("Could not dissassemble anim loading routine.", ex); }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not dissassemble spell anim routines.", ex);
                this.ASM_ListBox.DataSource = new String[0];
            }

            try
            {
                this.Anim_CodeBox.Text = String.Join("\r\n", this.CurrentAnim.GetAnimCode(this.Commands));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load anim code.", ex);
                this.Anim_CodeBox.Text = "";
            }
        }
        void Core_LoadValues()
        {
            try
            {
                this.Looped_CheckBox.Checked = this.CurrentAnim.Looped;
                this.Name_TextBox.Text = (this.CurrentAnim.Name == new Pointer()) ? "" :
                    Core.ReadData(this.CurrentAnim.Name, 32).GetASCII(0, 32);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);

                this.Looped_CheckBox.Checked = false;
                this.Name_TextBox.Text = "";
            }
        }
        void Core_UpdatePrevNextButtons()
        {
            try
            {
                if (this.CurrentArray_Palette.Count == 0)
                {
                    this.Palette_PointerBox.Value = new GBA.Pointer();
                    this.Palette_Prev_Button.Enabled = false;
                    this.Palette_Next_Button.Enabled = false;
                }
                else
                {
                    this.Palette_PointerBox.Value    = this.CurrentArray_Palette[this.CurrentIndex_Palette];
                    this.Palette_Prev_Button.Enabled = this.CurrentIndex_Palette > 0;
                    this.Palette_Next_Button.Enabled = this.CurrentIndex_Palette < this.CurrentArray_Palette.Count - 1;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                this.Palette_PointerBox.Value = new GBA.Pointer();
                this.Palette_Prev_Button.Enabled = false;
                this.Palette_Next_Button.Enabled = false;
            }

            try
            {
                if (this.CurrentArray_Tileset.Count == 0)
                {
                    this.Tileset_PointerBox.Value = new GBA.Pointer();
                    this.Tileset_Prev_Button.Enabled = false;
                    this.Tileset_Next_Button.Enabled = false;
                }
                else
                {
                    this.Tileset_PointerBox.Value    = this.CurrentArray_Tileset[this.CurrentIndex_Tileset];
                    this.Tileset_Prev_Button.Enabled = this.CurrentIndex_Tileset > 0;
                    this.Tileset_Next_Button.Enabled = this.CurrentIndex_Tileset < this.CurrentArray_Tileset.Count - 1;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                this.Tileset_PointerBox.Value = new GBA.Pointer();
                this.Tileset_Prev_Button.Enabled = false;
                this.Tileset_Next_Button.Enabled = false;
            }

            try
            {
                if (this.CurrentArray_TSA.Count == 0)
                {
                    this.TSA_PointerBox.Value = new GBA.Pointer();
                    this.TSA_Prev_Button.Enabled = false;
                    this.TSA_Next_Button.Enabled = false;
                }
                else
                {
                    this.TSA_PointerBox.Value    = this.CurrentArray_TSA[this.CurrentIndex_TSA];
                    this.TSA_Prev_Button.Enabled = this.CurrentIndex_TSA > 0;
                    this.TSA_Next_Button.Enabled = this.CurrentIndex_TSA < this.CurrentArray_TSA.Count - 1;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                this.TSA_PointerBox.Value = new GBA.Pointer();
                this.TSA_Prev_Button.Enabled = false;
                this.TSA_Next_Button.Enabled = false;
            }

            try
            {
                this.Prev_Button.Enabled = (
                    this.Palette_Prev_Button.Enabled ||
                    this.Tileset_Prev_Button.Enabled ||
                    this.TSA_Prev_Button.Enabled);
                this.Next_Button.Enabled = (
                    this.Palette_Next_Button.Enabled ||
                    this.Tileset_Next_Button.Enabled ||
                    this.TSA_Next_Button.Enabled);
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                this.Palette_PointerBox.Value = new GBA.Pointer();
                this.Palette_Prev_Button.Enabled = false;
                this.Palette_Next_Button.Enabled = false;
            }
        }



        private void View_PureASM_Click(Object sender, EventArgs e)
        {
            if (this.View_PureASM.Checked)
            {
                this.Anim_CodeBox.Visible = false;
                this.Anim_CodeBox.Enabled = false;
                this.ASM_ListBox.Visible = true;
                this.ASM_ListBox.Enabled = true;
            }
            else
            {
                this.Anim_CodeBox.Visible = true;
                this.Anim_CodeBox.Enabled = true;
                this.ASM_ListBox.Visible = false;
                this.ASM_ListBox.Enabled = false;
            }
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.CurrentIndex_Palette = 0;
            this.CurrentIndex_Tileset = 0;
            this.CurrentIndex_TSA = 0;

            this.Core_Update();
        }

        private void Refresh_Button_Click(Object sender, EventArgs e)
        {
            this.Core_UpdateDisplay();
        }

        private void ASM_Label_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_LoadAnimCode();
        }

        private void CopyASM_Click(Object sender, EventArgs e)
        {
            String text = "";
            foreach (Object item in this.ASM_ListBox.Items)
            {
                text += item.ToString().Substring(24) + "\r\n";
            }
            Clipboard.SetText(text);
        }


        private void Width_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_UpdateDisplay();
        }
        private void Height_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_UpdateDisplay();
        }
        private void TSA_Label_CheckedChanged(Object sender, EventArgs e)
        {
            if (this.TSA_Label.Checked)
            {
                this.TSA_PointerBox.Enabled = true;
                this.TSA_CheckBox.Enabled = true;
            }
            else
            {
                this.TSA_PointerBox.Enabled = false;
                this.TSA_CheckBox.Enabled = false;
            }
            this.Core_Update();
        }

        private void Palette_Prev_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_Palette == 0)
                return;
            this.CurrentIndex_Palette -= 1;
            this.Palette_PointerBox.Value = this.CurrentArray_Palette[this.CurrentIndex_Palette];

            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }
        private void Palette_Next_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_Palette == this.CurrentArray_Palette.Count - 1)
                return;
            this.CurrentIndex_Palette += 1;
            this.Palette_PointerBox.Value = this.CurrentArray_Palette[this.CurrentIndex_Palette];

            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }
        private void Tileset_Prev_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_Tileset == 0)
                return;
            this.CurrentIndex_Tileset -= 1;
            this.Tileset_PointerBox.Value = this.CurrentArray_Tileset[this.CurrentIndex_Tileset];

            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }
        private void Tileset_Next_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_Tileset == this.CurrentArray_Tileset.Count - 1)
                return;
            this.CurrentIndex_Tileset += 1;
            this.Tileset_PointerBox.Value = this.CurrentArray_Tileset[this.CurrentIndex_Tileset];

            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }
        private void TSA_Prev_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_TSA == 0)
                return;
            this.CurrentIndex_TSA -= 1;
            this.TSA_PointerBox.Value = this.CurrentArray_TSA[this.CurrentIndex_TSA];

            this.Width_NumberBox.ValueChanged -= this.Width_NumberBox_ValueChanged;
            this.Width_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Width;
            this.Width_NumberBox.ValueChanged += this.Width_NumberBox_ValueChanged;

            this.Height_NumberBox.ValueChanged -= this.Height_NumberBox_ValueChanged;
            this.Height_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Height;
            this.Height_NumberBox.ValueChanged += this.Height_NumberBox_ValueChanged;

            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }
        private void TSA_Next_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_TSA == this.CurrentArray_TSA.Count - 1)
                return;
            this.CurrentIndex_TSA += 1;
            this.TSA_PointerBox.Value = this.CurrentArray_TSA[this.CurrentIndex_TSA];

            this.Width_NumberBox.ValueChanged -= this.Width_NumberBox_ValueChanged;
            this.Width_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Width;
            this.Width_NumberBox.ValueChanged += this.Width_NumberBox_ValueChanged;

            this.Height_NumberBox.ValueChanged -= this.Height_NumberBox_ValueChanged;
            this.Height_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Height;
            this.Height_NumberBox.ValueChanged += this.Height_NumberBox_ValueChanged;

            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }

        private void Prev_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_Palette > 0)
            {
                this.CurrentIndex_Palette -= 1;
                this.Palette_PointerBox.Value = this.CurrentArray_Palette[this.CurrentIndex_Palette];
            }
            if (this.CurrentIndex_Tileset > 0)
            {
                this.CurrentIndex_Tileset -= 1;
                this.Tileset_PointerBox.Value = this.CurrentArray_Tileset[this.CurrentIndex_Tileset];
            }
            if (this.CurrentIndex_TSA > 0)
            {
                this.CurrentIndex_TSA -= 1;
                this.TSA_PointerBox.Value = this.CurrentArray_TSA[this.CurrentIndex_TSA];

                this.Width_NumberBox.ValueChanged -= this.Width_NumberBox_ValueChanged;
                this.Width_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Width;
                this.Width_NumberBox.ValueChanged += this.Width_NumberBox_ValueChanged;

                this.Height_NumberBox.ValueChanged -= this.Height_NumberBox_ValueChanged;
                this.Height_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Height;
                this.Height_NumberBox.ValueChanged += this.Height_NumberBox_ValueChanged;
            }
            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }
        private void Next_Button_Click(Object sender, EventArgs e)
        {
            if (this.CurrentIndex_Palette < this.CurrentArray_Palette.Count - 1)
            {
                this.CurrentIndex_Palette += 1;
                this.Palette_PointerBox.Value = this.CurrentArray_Palette[this.CurrentIndex_Palette];
            }
            if (this.CurrentIndex_Tileset < this.CurrentArray_Tileset.Count - 1)
            {
                this.CurrentIndex_Tileset += 1;
                this.Tileset_PointerBox.Value = this.CurrentArray_Tileset[this.CurrentIndex_Tileset];
            }
            if (this.CurrentIndex_TSA < this.CurrentArray_TSA.Count - 1)
            {
                this.CurrentIndex_TSA += 1;
                this.TSA_PointerBox.Value = this.CurrentArray_TSA[this.CurrentIndex_TSA];

                this.Width_NumberBox.ValueChanged -= this.Width_NumberBox_ValueChanged;
                this.Width_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Width;
                this.Width_NumberBox.ValueChanged += this.Width_NumberBox_ValueChanged;

                this.Height_NumberBox.ValueChanged -= this.Height_NumberBox_ValueChanged;
                this.Height_NumberBox.Value = this.CurrentArray_Dimensions[this.CurrentIndex_TSA].Height;
                this.Height_NumberBox.ValueChanged += this.Height_NumberBox_ValueChanged;
            }
            this.Core_UpdateDisplay();
            this.Core_UpdatePrevNextButtons();
        }

        private void MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            editor.Core_SetEntry(GBA.Screen.W_TILES, GBA.Screen.H_TILES,
                this.Palette_PointerBox.Value, this.Palette_CheckBox.Checked,
                this.Tileset_PointerBox.Value, this.Tileset_CheckBox.Checked,
                this.TSA_PointerBox.Value, this.TSA_CheckBox.Checked, true);

            Program.Core.Core_OpenEditor(editor);
        }
    }
}
