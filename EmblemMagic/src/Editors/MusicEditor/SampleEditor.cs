using System;
using GBA;
using Magic;
using Magic.Editors;

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

        String CurrentEntry
        {
            get
            {
                return "Sample at " + this.Address + " - ";
            }
        }

        public SampleEditor(Pointer address)
        {
            this.Address = address;
            this.InitializeComponent();
        }
        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Preview = new Music();
            this.Current = new Sample(this.Address);

            this.Wave_SampleBox.Load(this.Current);

            this.Core_LoadValues();
        }
        void Core_LoadValues()
        {
            this.Loop_CheckBox.CheckedChanged -= this.Loop_CheckBox_CheckedChanged;
            this.Loop_NumberBox.ValueChanged -= this.Loop_NumberBox_ValueChanged;
            this.Rate_NumberBox.ValueChanged -= this.Rate_NumberBox_ValueChanged;

            this.Loop_CheckBox.Checked = this.Current.Looped;
            this.Loop_NumberBox.Value = this.Current.LoopStart;
            this.Rate_NumberBox.Value = this.Current.Pitch / 1024;

            this.Loop_CheckBox.CheckedChanged += this.Loop_CheckBox_CheckedChanged;
            this.Loop_NumberBox.ValueChanged += this.Loop_NumberBox_ValueChanged;
            this.Rate_NumberBox.ValueChanged += this.Rate_NumberBox_ValueChanged;
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
                this.Address + 3,
                this.Loop_CheckBox.Checked ? (Byte)0x40 : (Byte)0x00,
                this.CurrentEntry + "Loop changed");
        }
        private void Loop_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                this.Address + 8,
                Util.UInt32ToBytes((UInt32)this.Loop_NumberBox.Value, true),
                this.CurrentEntry + "Loop Start changed");
        }
        private void Rate_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteData(this,
                this.Address + 4,
                Util.UInt32ToBytes((UInt32)(this.Rate_NumberBox.Value * 1024), true),
                this.CurrentEntry + "Sampling Rate changed");
        }

        private void Play_Button_Click(Object sender, EventArgs e)
        {
            this.Preview.PlaySample(this.Current);
        }
    }
}
