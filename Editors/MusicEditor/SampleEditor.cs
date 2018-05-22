using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class SampleEditor : Editor
    {
        /// <summary>
        /// The address at which the sample to edit is located
        /// </summary>
        Pointer Address;
        /// <summary>
        /// The currently loaded sample
        /// </summary>
        Sample Current;
        /// <summary>
        /// Allow for playing the sound of the sample
        /// </summary>
        Music Preview;

        string CurrentEntry
        {
            get
            {
                return "Sample at " + Address + " - ";
            }
        }

        public SampleEditor(Pointer address)
        {
            Address = address;
            InitializeComponent();
        }
        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Preview = new Music();
            Current = new Sample(Address);

            Wave_SampleBox.Load(Current);

            Core_LoadValues();
        }
        void Core_LoadValues()
        {
            Loop_CheckBox.CheckedChanged -= Loop_CheckBox_CheckedChanged;
            Loop_NumberBox.ValueChanged -= Loop_NumberBox_ValueChanged;
            Rate_NumberBox.ValueChanged -= Rate_NumberBox_ValueChanged;

            Loop_CheckBox.Checked = Current.Looped;
            Loop_NumberBox.Value = Current.LoopStart;
            Rate_NumberBox.Value = Current.Pitch / 1024;

            Loop_CheckBox.CheckedChanged += Loop_CheckBox_CheckedChanged;
            Loop_NumberBox.ValueChanged += Loop_NumberBox_ValueChanged;
            Rate_NumberBox.ValueChanged += Rate_NumberBox_ValueChanged;
        }



        private void File_Insert_Click(Object sender, EventArgs e)
        {

        }
        private void File_Save_Click(Object sender, EventArgs e)
        {

        }

        private void Loop_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Address + 3,
                Loop_CheckBox.Checked ? (byte)0x40 : (byte)0x00,
                CurrentEntry + "Loop changed");
        }
        private void Loop_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                Address + 8,
                Util.UInt32ToBytes((uint)Loop_NumberBox.Value, true),
                CurrentEntry + "Loop Start changed");
        }
        private void Rate_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                Address + 4,
                Util.UInt32ToBytes((uint)(Rate_NumberBox.Value * 1024), true),
                CurrentEntry + "Sampling Rate changed");
        }

        private void Play_Button_Click(Object sender, EventArgs e)
        {
            Preview.PlaySample(Current);
        }
    }
}
