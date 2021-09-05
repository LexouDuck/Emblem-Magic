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

        public KeyMapEditor(IApp app,
            Pointer address)
            : base(app)
        {
            Address = address;

            InitializeComponent();

            KeyMap_PianoBox.SelectionChanged += KeyMap_PianoBox_SelectionChanged;
        }



        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Current = new KeyMap(Core.ReadData(Address, KeyMap.LENGTH));

            KeyMap_PianoBox.SelectionChanged -= KeyMap_PianoBox_SelectionChanged;
            for (int i = 0; i < KeyMap_PianoBox.Selection.Length; i++)
            {
                if (Current[i] == Sample_ByteBox.Value)
                {
                    KeyMap_PianoBox.Selection[i] = true;
                }
            }
            KeyMap_PianoBox.SelectionChanged += KeyMap_PianoBox_SelectionChanged;
        }



        private void Sample_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void KeyMap_PianoBox_SelectionChanged(Object sender, EventArgs e)
        {
            for (int i = 0; i < KeyMap_PianoBox.Selection.Length; i++)
            {
                if (KeyMap_PianoBox.Selection[i] && Current[i] != Sample_ByteBox.Value)
                {
                    Core.WriteByte(this,
                        Address + i,
                        Sample_ByteBox.Value,
                        "Key Map at " + Address + " - Byte " + i + "changed");
                }
            }
        }
    }
}