using EmblemMagic.Components;
using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace EmblemMagic.Editors
{
    public partial class SpellAnimEditor : Editor
    {
        SpellAnimation CurrentAnim;
        SpellCommands Commands;

        public SpellAnimEditor()
        {
            try
            {
                InitializeComponent();

                EntryArrayBox.Load("Spell Animations.txt");
                EntryArrayBox.Value = 0;
                EntryArrayBox.ValueChanged += SpellArrayBox_ValueChanged;

                Commands = new SpellCommands("Spell Commands.txt");
                AnimCodeBox.KeyDown += new KeyEventHandler(TextBox_SelectAll);
                AnimCodeBox.AddSyntax(Commands.GetRegex(), System.Drawing.SystemColors.Highlight);
                AnimCodeBox.AddSyntax("return|label.*", System.Drawing.SystemColors.ControlDark);
                AnimCodeBox.AddSyntax("@.*", System.Drawing.SystemColors.ControlDark);
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
            try
            {
                Pointer address = Core.ReadPointer(Core.GetPointer("Spell Animation Array") + 4 * EntryArrayBox.Value);

                CurrentAnim = new SpellAnimation(address);
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load this spell animation.", ex);
            }

            Core_LoadAnimCode();
            Core_LoadValues();
        }

        void Core_UpdateDisplay()
        {
            const int WIDTH = 32;
            const int HEIGHT = 8;
            const int PALETTES = 4;

            byte[] palette = Core.ReadData(
                Palette_PointerBox.Value,
                Palette_CheckBox.Checked ? 0 : PALETTES * Palette.LENGTH);
            byte[] tileset = Core.ReadData(
                Tileset_PointerBox.Value,
                Tileset_CheckBox.Checked ? 0 : WIDTH * HEIGHT * Tile.LENGTH);

            for (int i = 0; i < palette.Length; i += 2)
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
                            WIDTH, HEIGHT,
                            TSA_CheckBox.Checked, false));
                }
                else
                {
                    image = new Tileset(tileset).ToImage(
                            WIDTH, HEIGHT, palette.GetBytes(0, Palette.LENGTH));
                }

                Spell_PaletteBox.Load(new Palette(palette, Palette.MAX * 16));
                Spell_ImageBox.Load(image);
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the image.", ex);
            }
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
                Program.ShowError("Could not dissassemble spell anim routines.", ex);
                ASM_ListBox.DataSource = new string[0];
            }

                AnimCodeBox.Text = string.Join("\r\n", CurrentAnim.GetAnimCode(Commands));
            try
            {
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load anim code.", ex);
                AnimCodeBox.Text = "";
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
                Program.ShowError("There has been an error while trying to load the values.", ex);

                Looped_CheckBox.Checked = false;
                Name_TextBox.Text = "";
            }
        }



        private void SpellArrayBox_ValueChanged(object sender, EventArgs e)
        {
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
            string text = "";
            foreach (object item in ASM_ListBox.Items)
            {
                text += item.ToString().Substring(24) + "\r\n";
            }
            Clipboard.SetText(text);
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
    }
}
