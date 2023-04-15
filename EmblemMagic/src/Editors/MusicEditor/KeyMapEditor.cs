using System;
using GBA;
using Magic;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class KeyMapEditor : Editor
    {
        Pointer Address;
        KeyMap Current;

        public KeyMapEditor(Pointer address)
        {
            this.Address = address;

            this.InitializeComponent();

            this.KeyMap_PianoBox.SelectionChanged += this.KeyMap_PianoBox_SelectionChanged;
        }



        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Current = new KeyMap(Core.ReadData(this.Address, KeyMap.LENGTH));

            this.KeyMap_PianoBox.SelectionChanged -= this.KeyMap_PianoBox_SelectionChanged;
            for (Int32 i = 0; i < this.KeyMap_PianoBox.Selection.Length; i++)
            {
                if (this.Current[i] == this.Sample_ByteBox.Value)
                {
                    this.KeyMap_PianoBox.Selection[i] = true;
                }
            }
            this.KeyMap_PianoBox.SelectionChanged += this.KeyMap_PianoBox_SelectionChanged;
        }



        private void Sample_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void KeyMap_PianoBox_SelectionChanged(Object sender, EventArgs e)
        {
            for (Int32 i = 0; i < this.KeyMap_PianoBox.Selection.Length; i++)
            {
                if (this.KeyMap_PianoBox.Selection[i] && this.Current[i] != this.Sample_ByteBox.Value)
                {
                    Core.WriteByte(this,
                        this.Address + i,
                        this.Sample_ByteBox.Value,
                        "Key Map at " + this.Address + " - Byte " + i + "changed");
                }
            }
        }
    }
}