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



        public SpellAnimEditor(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();

                EntryArrayBox.Load("Spell Animations.txt");
                EntryArrayBox.Value = 0;
                EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;

                Commands = new SpellCommands(Core.Path_Structs + "Spell Commands.txt");
                Anim_CodeBox.KeyDown += new KeyEventHandler(TextBox_SelectAll);
                Anim_CodeBox.AddSyntax(ASM.GetInstructionsRegex_Thumb(), System.Drawing.Color.Black, FontStyle.Bold | FontStyle.Italic);
                Anim_CodeBox.AddSyntax(Commands.GetRegex(), SystemColors.Highlight);
                Anim_CodeBox.AddSyntax(@"((\b[0-9]+)|((\b0x|\$)[0-9a-fA-F]+))\b", System.Drawing.Color.SlateBlue);
                Anim_CodeBox.AddSyntax("return|label.*", SystemColors.ControlDark);
                Anim_CodeBox.AddSyntax("@.*", SystemColors.ControlDark);
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
                Pointer address = Core.ReadPointer(Core.GetPointer("Spell Animation Array") + 4 * EntryArrayBox.Value);

                CurrentAnim = new SpellAnimation(address);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load this spell animation.", ex);
            }
            Core_LoadAnimCode();
            Core_LoadValues();
            Core_FindAndDisplayGraphics();
        }

        void Core_UpdateDisplay()
        {
            const Int32 palettes = 8;
            Int32 width = (Int32)Width_NumberBox.Value;
            Int32 height = (Int32)Height_NumberBox.Value;

            Byte[] palette = Core.ReadData(
                Palette_PointerBox.Value,
                Palette_CheckBox.Checked ? 0 : palettes * Palette.LENGTH);
            Byte[] tileset = Core.ReadData(
                Tileset_PointerBox.Value,
                Tileset_CheckBox.Checked ? 0 : width * height * Tile.LENGTH);
            
            for (Int32 i = 0; i < palette.Length; i += 2)
            {
                palette[i + 1] &= 0x7F;
            }   // force opaque colors on display
            
            if (palette == null || palette.Length == 0)
            {
                Spell_PaletteBox.Reset();
                Spell_ImageBox.Reset();
                return;
            }
            if (tileset == null || tileset.Length == 0)
            {
                Spell_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                Spell_ImageBox.Reset();
                return;
            }

            IDisplayable image = null;
            try
            {
                if (TSA_Label.Checked && TSA_PointerBox.Value != new Pointer())
                {
                    image = new TSA_Image(palette, tileset, Core.ReadTSA(
                            TSA_PointerBox.Value,
                            width, height,
                            TSA_CheckBox.Checked, false));
                }
                else
                {
                    image = new Tileset(tileset).ToImage(
                            width, height, palette.GetBytes(0, Palette.LENGTH));
                }
                Spell_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                Spell_ImageBox.Load(image);
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
                for (Int32 i = 0; i < Anim_CodeBox.Text.Length; i = index + 3)
                {
                    index = Anim_CodeBox.Text.IndexOf(match, i);
                    if (index < 0)
                        break;
                    if (Anim_CodeBox.Text.Substring(index - 5, 5) == "Array")
                        is_array = true;
                    if (Anim_CodeBox.Text.Substring(index - 6, 6) == "Sprite")
                        is_sprite = true;
                    index = Anim_CodeBox.Text.IndexOf(")\r\n", index);
                    if (index < 0)
                        break;
                    Int32 index_of_pointer_arg;
                    index_of_pointer_arg = Anim_CodeBox.Text.LastIndexOf("$08", index) + 3;
                    if (index_of_pointer_arg < 0)
                        continue; // TODO throw exception here ?
                    address = new Pointer(Util.StringToAddress(Anim_CodeBox.Text.Substring(index_of_pointer_arg, index - index_of_pointer_arg)));
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
                                    CurrentArray_Dimensions.Add(is_sprite ?
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
                            CurrentArray_Dimensions.Add(is_sprite ?
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
            CurrentArray_Palette = Core_FindAllInAnimCode("_Palette(");
            CurrentArray_Tileset = Core_FindAllInAnimCode("_Tileset(");
            CurrentArray_TSA     = Core_FindAllInAnimCode("_TSA(");

            if (CurrentArray_Palette.Count > 0)
            {
                Palette_PointerBox.Value = (CurrentArray_Palette[CurrentIndex_Palette]);
                Palette_CheckBox.Checked = (Core.ReadByte(CurrentArray_Palette[0]) == 0x10);
            }
            else Palette_PointerBox.Value = new Pointer();

            if (CurrentArray_Tileset.Count > 0)
            {
                Tileset_PointerBox.Value = (CurrentArray_Tileset[CurrentIndex_Tileset]);
                Tileset_CheckBox.Checked = (Core.ReadByte(CurrentArray_Tileset[0]) == 0x10);
            }
            else Tileset_PointerBox.Value = new Pointer();

            if (CurrentArray_TSA.Count > 0)
            {
                TSA_Label.Checked = true;
                TSA_PointerBox.Value     = (CurrentArray_TSA[CurrentIndex_TSA]);
                TSA_CheckBox.Checked     = (Core.ReadByte(CurrentArray_TSA[0]) == 0x10);
            }
            else TSA_PointerBox.Value = new Pointer();

            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }
        void Core_LoadAnimCode()
        {
            try
            {
                Constructor_PointerBox.Value = CurrentAnim.Address_Constructor;
                LoopRoutine_PointerBox.Value = CurrentAnim.Address_LoopRoutine;
                AnimLoading_PointerBox.Value = CurrentAnim.Address_AnimLoading;

                try { if (Constructor_Label.Checked) ASM_ListBox.DataSource = CurrentAnim.GetASM_Constructor(); }
                catch (Exception ex) { throw new Exception("Could not dissassemble constructor.", ex); }
                try { if (LoopRoutine_Label.Checked) ASM_ListBox.DataSource = CurrentAnim.GetASM_LoopRoutine(); }
                catch (Exception ex) { throw new Exception("Could not dissassemble loop routine.", ex); }
                try { if (AnimLoading_Label.Checked) ASM_ListBox.DataSource = CurrentAnim.GetASM_AnimLoading(); }
                catch (Exception ex) { throw new Exception("Could not dissassemble anim loading routine.", ex); }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not dissassemble spell anim routines.", ex);
                ASM_ListBox.DataSource = new String[0];
            }

            try
            {
                Anim_CodeBox.Text = String.Join("\r\n", CurrentAnim.GetAnimCode(Commands));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load anim code.", ex);
                Anim_CodeBox.Text = "";
            }
        }
        void Core_LoadValues()
        {
            try
            {
                Looped_CheckBox.Checked = CurrentAnim.Looped;
                Name_TextBox.Text = (CurrentAnim.Name == new Pointer()) ? "" :
                    Core.ReadData(CurrentAnim.Name, 32).GetASCII(0, 32);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);

                Looped_CheckBox.Checked = false;
                Name_TextBox.Text = "";
            }
        }
        void Core_UpdatePrevNextButtons()
        {
            try
            {
                if (CurrentArray_Palette.Count == 0)
                {
                    Palette_PointerBox.Value = new GBA.Pointer();
                    Palette_Prev_Button.Enabled = false;
                    Palette_Next_Button.Enabled = false;
                }
                else
                {
                    Palette_PointerBox.Value    = CurrentArray_Palette[CurrentIndex_Palette];
                    Palette_Prev_Button.Enabled = CurrentIndex_Palette > 0;
                    Palette_Next_Button.Enabled = CurrentIndex_Palette < CurrentArray_Palette.Count - 1;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                Palette_PointerBox.Value = new GBA.Pointer();
                Palette_Prev_Button.Enabled = false;
                Palette_Next_Button.Enabled = false;
            }

            try
            {
                if (CurrentArray_Tileset.Count == 0)
                {
                    Tileset_PointerBox.Value = new GBA.Pointer();
                    Tileset_Prev_Button.Enabled = false;
                    Tileset_Next_Button.Enabled = false;
                }
                else
                {
                    Tileset_PointerBox.Value    = CurrentArray_Tileset[CurrentIndex_Tileset];
                    Tileset_Prev_Button.Enabled = CurrentIndex_Tileset > 0;
                    Tileset_Next_Button.Enabled = CurrentIndex_Tileset < CurrentArray_Tileset.Count - 1;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                Tileset_PointerBox.Value = new GBA.Pointer();
                Tileset_Prev_Button.Enabled = false;
                Tileset_Next_Button.Enabled = false;
            }

            try
            {
                if (CurrentArray_TSA.Count == 0)
                {
                    TSA_PointerBox.Value = new GBA.Pointer();
                    TSA_Prev_Button.Enabled = false;
                    TSA_Next_Button.Enabled = false;
                }
                else
                {
                    TSA_PointerBox.Value    = CurrentArray_TSA[CurrentIndex_TSA];
                    TSA_Prev_Button.Enabled = CurrentIndex_TSA > 0;
                    TSA_Next_Button.Enabled = CurrentIndex_TSA < CurrentArray_TSA.Count - 1;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                TSA_PointerBox.Value = new GBA.Pointer();
                TSA_Prev_Button.Enabled = false;
                TSA_Next_Button.Enabled = false;
            }

            try
            {
                Prev_Button.Enabled = (
                    Palette_Prev_Button.Enabled ||
                    Tileset_Prev_Button.Enabled ||
                    TSA_Prev_Button.Enabled);
                Next_Button.Enabled = (
                    Palette_Next_Button.Enabled ||
                    Tileset_Next_Button.Enabled ||
                    TSA_Next_Button.Enabled);
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while updating Palette controls: ", ex);
                Palette_PointerBox.Value = new GBA.Pointer();
                Palette_Prev_Button.Enabled = false;
                Palette_Next_Button.Enabled = false;
            }
        }



        private void View_PureASM_Click(Object sender, EventArgs e)
        {
            if (View_PureASM.Checked)
            {
                Anim_CodeBox.Visible = false;
                Anim_CodeBox.Enabled = false;
                ASM_ListBox.Visible = true;
                ASM_ListBox.Enabled = true;
            }
            else
            {
                Anim_CodeBox.Visible = true;
                Anim_CodeBox.Enabled = true;
                ASM_ListBox.Visible = false;
                ASM_ListBox.Enabled = false;
            }
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            CurrentIndex_Palette = 0;
            CurrentIndex_Tileset = 0;
            CurrentIndex_TSA = 0;

            Core_Update();
        }

        private void Refresh_Button_Click(Object sender, EventArgs e)
        {
            Core_UpdateDisplay();
        }

        private void ASM_Label_CheckedChanged(Object sender, EventArgs e)
        {
            Core_LoadAnimCode();
        }

        private void CopyASM_Click(Object sender, EventArgs e)
        {
            String text = "";
            foreach (Object item in ASM_ListBox.Items)
            {
                text += item.ToString().Substring(24) + "\r\n";
            }
            Clipboard.SetText(text);
        }


        private void Width_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_UpdateDisplay();
        }
        private void Height_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_UpdateDisplay();
        }
        private void TSA_Label_CheckedChanged(Object sender, EventArgs e)
        {
            if (TSA_Label.Checked)
            {
                TSA_PointerBox.Enabled = true;
                TSA_CheckBox.Enabled = true;
            }
            else
            {
                TSA_PointerBox.Enabled = false;
                TSA_CheckBox.Enabled = false;
            }
            Core_Update();
        }

        private void Palette_Prev_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_Palette == 0)
                return;
            CurrentIndex_Palette -= 1;
            Palette_PointerBox.Value = CurrentArray_Palette[CurrentIndex_Palette];

            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }
        private void Palette_Next_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_Palette == CurrentArray_Palette.Count - 1)
                return;
            CurrentIndex_Palette += 1;
            Palette_PointerBox.Value = CurrentArray_Palette[CurrentIndex_Palette];

            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }
        private void Tileset_Prev_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_Tileset == 0)
                return;
            CurrentIndex_Tileset -= 1;
            Tileset_PointerBox.Value = CurrentArray_Tileset[CurrentIndex_Tileset];

            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }
        private void Tileset_Next_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_Tileset == CurrentArray_Tileset.Count - 1)
                return;
            CurrentIndex_Tileset += 1;
            Tileset_PointerBox.Value = CurrentArray_Tileset[CurrentIndex_Tileset];

            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }
        private void TSA_Prev_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_TSA == 0)
                return;
            CurrentIndex_TSA -= 1;
            TSA_PointerBox.Value = CurrentArray_TSA[CurrentIndex_TSA];

            Width_NumberBox.ValueChanged -= Width_NumberBox_ValueChanged;
            Width_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Width;
            Width_NumberBox.ValueChanged += Width_NumberBox_ValueChanged;

            Height_NumberBox.ValueChanged -= Height_NumberBox_ValueChanged;
            Height_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Height;
            Height_NumberBox.ValueChanged += Height_NumberBox_ValueChanged;

            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }
        private void TSA_Next_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_TSA == CurrentArray_TSA.Count - 1)
                return;
            CurrentIndex_TSA += 1;
            TSA_PointerBox.Value = CurrentArray_TSA[CurrentIndex_TSA];

            Width_NumberBox.ValueChanged -= Width_NumberBox_ValueChanged;
            Width_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Width;
            Width_NumberBox.ValueChanged += Width_NumberBox_ValueChanged;

            Height_NumberBox.ValueChanged -= Height_NumberBox_ValueChanged;
            Height_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Height;
            Height_NumberBox.ValueChanged += Height_NumberBox_ValueChanged;

            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }

        private void Prev_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_Palette > 0)
            {
                CurrentIndex_Palette -= 1;
                Palette_PointerBox.Value = CurrentArray_Palette[CurrentIndex_Palette];
            }
            if (CurrentIndex_Tileset > 0)
            {
                CurrentIndex_Tileset -= 1;
                Tileset_PointerBox.Value = CurrentArray_Tileset[CurrentIndex_Tileset];
            }
            if (CurrentIndex_TSA > 0)
            {
                CurrentIndex_TSA -= 1;
                TSA_PointerBox.Value = CurrentArray_TSA[CurrentIndex_TSA];

                Width_NumberBox.ValueChanged -= Width_NumberBox_ValueChanged;
                Width_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Width;
                Width_NumberBox.ValueChanged += Width_NumberBox_ValueChanged;

                Height_NumberBox.ValueChanged -= Height_NumberBox_ValueChanged;
                Height_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Height;
                Height_NumberBox.ValueChanged += Height_NumberBox_ValueChanged;
            }
            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }
        private void Next_Button_Click(Object sender, EventArgs e)
        {
            if (CurrentIndex_Palette < CurrentArray_Palette.Count - 1)
            {
                CurrentIndex_Palette += 1;
                Palette_PointerBox.Value = CurrentArray_Palette[CurrentIndex_Palette];
            }
            if (CurrentIndex_Tileset < CurrentArray_Tileset.Count - 1)
            {
                CurrentIndex_Tileset += 1;
                Tileset_PointerBox.Value = CurrentArray_Tileset[CurrentIndex_Tileset];
            }
            if (CurrentIndex_TSA < CurrentArray_TSA.Count - 1)
            {
                CurrentIndex_TSA += 1;
                TSA_PointerBox.Value = CurrentArray_TSA[CurrentIndex_TSA];

                Width_NumberBox.ValueChanged -= Width_NumberBox_ValueChanged;
                Width_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Width;
                Width_NumberBox.ValueChanged += Width_NumberBox_ValueChanged;

                Height_NumberBox.ValueChanged -= Height_NumberBox_ValueChanged;
                Height_NumberBox.Value = CurrentArray_Dimensions[CurrentIndex_TSA].Height;
                Height_NumberBox.ValueChanged += Height_NumberBox_ValueChanged;
            }
            Core_UpdateDisplay();
            Core_UpdatePrevNextButtons();
        }

        private void MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor(App);

            editor.Core_SetEntry(GBA.Screen.W_TILES, GBA.Screen.H_TILES,
                Palette_PointerBox.Value, Palette_CheckBox.Checked,
                Tileset_PointerBox.Value, Tileset_CheckBox.Checked,
                TSA_PointerBox.Value, TSA_CheckBox.Checked, true);

            Program.Core.Core_OpenEditor(editor);
        }
    }
}
