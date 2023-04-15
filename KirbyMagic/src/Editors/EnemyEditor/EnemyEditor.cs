using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Compression;
using KirbyMagic.Kirby;
using GBA;
using Magic;
using Magic.Editors;

namespace KirbyMagic.Editors
{
    public partial class EnemyEditor : Editor
    {
        private StructFile Current;

        /// <summary>
        /// Gets a string of the current byte index of enemy in the array
        /// </summary>
        String CurrentEntry
        {
            get
            {
                return "Enemy 0x" + Util.UInt16ToHex(this.EntryArrayBox.Value) + " [" + this.EntryArrayBox.Text + "] - ";
            }
        }



        public EnemyEditor()
        {
            this.InitializeComponent();
        }
        
        override public void Core_OnOpen()
        {
            this.Core_Update();
        }
        override public void Core_Update()
        {
            this.Current.EntryIndex = this.EntryArrayBox.Value;

            this.Core_LoadValues();
        }

        void Core_LoadValues()
        {
            /*
            MapData_ArrayBox.ValueChanged -= MapData_ArrayBox_ValueChanged;
            MapData_PointerBox.ValueChanged -= MapData_PointerBox_ValueChanged;

            try
            {
                MapData_ArrayBox.Value = (Byte)Current["Map"];

                GBA.Pointer address = Core.GetPointer("Map Data Array");
                MapData_PointerBox.Value = Core.ReadPointer(address + 4 * MapData_ArrayBox.Value);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);
            }

            MapData_ArrayBox.ValueChanged += MapData_ArrayBox_ValueChanged;
            MapData_PointerBox.ValueChanged += MapData_PointerBox_ValueChanged;
            */
            //Chapter_MagicButton.EntryToSelect = EntryArrayBox.Value;
        }
    }
}
