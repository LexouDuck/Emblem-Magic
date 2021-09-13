using System;
using GBA;
using Magic;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class InstrumentEditor : Editor
    {
        InstrumentArray Instruments;
        Instrument Current;
        Music Preview;

        Pointer CurrentAddress
        {
            get
            {
                return this.Instruments.GetAddress(this.Entry_ByteBox.Value);
            }
        }
        String CurrentEntry
        {
            get
            {
                return "Instrument 0x" + Util.ByteToHex(this.Entry_ByteBox.Value) + " from " + this.Instruments.Address + " - ";
            }
        }

        String[] InstrumentTypes = new String[]
        {
            "DirectSound",
            "PSG Square 1",
            "PSG Square 2",
            "PSG Waveform",
            "PSG Noise",
            "Fixed Pitch",
            "MultiSample",
            "Percussion"
        };

        public InstrumentEditor(InstrumentArray instruments, Byte index)
        {
            try
            {
                this.InitializeComponent();

                this.Preview = new Music();

                this.Entry_ByteBox.ValueChanged -= this.Entry_ByteBox_ValueChanged;
                this.Entry_ByteBox.Value = index;
                this.Entry_ByteBox.ValueChanged += this.Entry_ByteBox_ValueChanged;

                this.Instruments = instruments;

                this.Instrument_PianoBox.SelectionChanged += this.Instrument_PianoBox_SelectionChanged;

                this.Address_Label.Text = this.Instruments.Address.ToString();

                this.Type_ComboBox.SelectedIndexChanged -= this.Type_ComboBox_SelectedIndexChanged;
                this.Type_ComboBox.DataSource = this.InstrumentTypes;
                this.Type_ComboBox.SelectedIndexChanged += this.Type_ComboBox_SelectedIndexChanged;

                this.BaseKey_ByteArrayBox.Load("Music Notes.txt");
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
            this.Current = this.Instruments[this.Entry_ByteBox.Value];

            this.Preview.Reset();
            this.Preview.Load(this.Instruments.GetAudio(this.Entry_ByteBox.Value));

            this.Core_LoadValues();
        }

        void Core_LoadValues()
        {
            this.Type_ComboBox.SelectedIndexChanged -= this.Type_ComboBox_SelectedIndexChanged;
            this.BaseKey_ByteArrayBox.ValueChanged -= this.BaseKey_ByteArrayBox_ValueChanged;
            this.Panning_ByteBox.ValueChanged -= this.Panning_ByteBox_ValueChanged;
            this.Unused_ByteBox.ValueChanged -= this.Unused_ByteBox_ValueChanged;

            this.Envelope_Attack_ByteBox.ValueChanged  -= this.Envelope_Attack_ByteBox_ValueChanged;
            this.Envelope_Decay_ByteBox.ValueChanged   -= this.Envelope_Decay_ByteBox_ValueChanged;
            this.Envelope_Sustain_ByteBox.ValueChanged -= this.Envelope_Sustain_ByteBox_ValueChanged;
            this.Envelope_Release_ByteBox.ValueChanged -= this.Envelope_Release_ByteBox_ValueChanged;

            this.DutyPeriod_ComboBox.SelectedIndexChanged -= this.DutyPeriod_ComboBox_SelectedIndexChanged;
            this.Sample_PointerBox.ValueChanged -= this.Sample_PointerBox_ValueChanged;
            this.KeyMap_PointerBox.ValueChanged -= this.KeyMap_PointerBox_ValueChanged;



            try
            {
                Int32 index = 0;
                switch (this.Current.Type)
                {
                    case InstrumentType.Direct:  index = 0; break;
                    case InstrumentType.Square1: index = 1; break;
                    case InstrumentType.Square2: index = 2; break;
                    case InstrumentType.Wave:    index = 3; break;
                    case InstrumentType.Noise:   index = 4; break;
                    case InstrumentType.Fixed:   index = 5; break;
                    case InstrumentType.Multi:   index = 6; break;
                    case InstrumentType.Drums:   index = 7; break;
                    default: break;
                }
                this.Type_ComboBox.SelectedIndex = this.Type_ComboBox.FindStringExact(this.InstrumentTypes[index]);
                this.BaseKey_ByteArrayBox.Value = this.Current.BaseKey;
                this.Panning_ByteBox.Value = this.Current.Panning;
                this.Unused_ByteBox.Value = this.Current.Unused;

                this.Envelope_Attack_ByteBox.Value = this.Current.Attack;
                this.Envelope_Decay_ByteBox.Value = this.Current.Decay;
                this.Envelope_Sustain_ByteBox.Value = this.Current.Sustain;
                this.Envelope_Release_ByteBox.Value = this.Current.Release;

                if (this.Current.Type == InstrumentType.Direct ||
                    this.Current.Type == InstrumentType.Fixed ||
                    this.Current.Type == InstrumentType.Wave ||
                    this.Current.Type == InstrumentType.Multi ||
                    this.Current.Type == InstrumentType.Drums)
                {
                    this.Sample_Label.Enabled = true;
                    this.Sample_Button.Enabled = true;
                    this.Sample_PointerBox.Enabled = true;
                    this.Sample_PointerBox.Value = this.Current.Sample;
                }
                else
                {
                    this.Sample_Label.Enabled = false;
                    this.Sample_Button.Enabled = false;
                    this.Sample_PointerBox.Enabled = false;
                    this.Sample_PointerBox.Value = new Pointer();
                }

                if (this.Current.Type == InstrumentType.Multi)
                {
                    this.KeyMap_Label.Enabled = true;
                    this.KeyMap_Label.Text = "Key Map :";
                    this.KeyMap_Button.Enabled = true;
                    this.KeyMap_PointerBox.Enabled = true;
                    this.KeyMap_PointerBox.Value = this.Current.KeyMap;
                }
                else if (this.Current.Type == InstrumentType.Wave)
                {
                    this.KeyMap_Label.Enabled = true;
                    this.KeyMap_Label.Text = "Wave :";
                    this.KeyMap_Button.Enabled = true;
                    this.KeyMap_PointerBox.Enabled = true;
                    this.KeyMap_PointerBox.Value = this.Current.KeyMap;
                }
                else
                {
                    this.KeyMap_Label.Enabled = false;
                    this.KeyMap_Button.Enabled = false;
                    this.KeyMap_PointerBox.Enabled = false;
                    this.KeyMap_PointerBox.Value = new Pointer();
                }

                if (this.Current.Type == InstrumentType.Square1 ||
                    this.Current.Type == InstrumentType.Square2)
                {
                    this.DutyPeriod_Label.Enabled = true;
                    this.DutyPeriod_ComboBox.Enabled = true;
                    this.DutyPeriod_ComboBox.DataSource = new String[]
                    {
                        "0x00 - 12.5%",
                        "0x01 - 25%",
                        "0x02 - 50%",
                        "0x03 - 75%",
                    };
                    this.DutyPeriod_ComboBox.SelectedIndex = this.Current.DutyPeriod;
                }
                else if (this.Current.Type == InstrumentType.Noise)
                {
                    this.DutyPeriod_Label.Enabled = true;
                    this.DutyPeriod_ComboBox.Enabled = true;
                    this.DutyPeriod_ComboBox.DataSource = new String[]
                    {
                        "0x00 - Normal",
                        "0x01 - Metallic",
                    };
                    this.DutyPeriod_ComboBox.SelectedIndex = this.Current.DutyPeriod;
                }
                else
                {
                    this.DutyPeriod_Label.Enabled = false;
                    this.DutyPeriod_ComboBox.Enabled = false;
                    this.DutyPeriod_ComboBox.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the instrument values.", ex);
            }



            this.Type_ComboBox.SelectedIndexChanged += this.Type_ComboBox_SelectedIndexChanged;
            this.BaseKey_ByteArrayBox.ValueChanged += this.BaseKey_ByteArrayBox_ValueChanged;
            this.Panning_ByteBox.ValueChanged += this.Panning_ByteBox_ValueChanged;
            this.Unused_ByteBox.ValueChanged += this.Unused_ByteBox_ValueChanged;

            this.Envelope_Attack_ByteBox.ValueChanged += this.Envelope_Attack_ByteBox_ValueChanged;
            this.Envelope_Decay_ByteBox.ValueChanged += this.Envelope_Decay_ByteBox_ValueChanged;
            this.Envelope_Sustain_ByteBox.ValueChanged += this.Envelope_Sustain_ByteBox_ValueChanged;
            this.Envelope_Release_ByteBox.ValueChanged += this.Envelope_Release_ByteBox_ValueChanged;

            this.DutyPeriod_ComboBox.SelectedIndexChanged += this.DutyPeriod_ComboBox_SelectedIndexChanged;
            this.Sample_PointerBox.ValueChanged += this.Sample_PointerBox_ValueChanged;
            this.KeyMap_PointerBox.ValueChanged += this.KeyMap_PointerBox_ValueChanged;
        }



        private void Entry_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void Instrument_PianoBox_SelectionChanged(Object sender, EventArgs e)
        {
            for (Byte i = 0; i < Music.NOTES; i++)
            {
                if (this.Instrument_PianoBox.Selection[i])
                {
                    this.Preview.PlayAudio(0, i);
                }
                else if (!this.Preview.Samples[0][i].Cancel)
                {
                    this.Preview.Samples[0][i].State = Audio.ADSR.Release;
                }
            }
        }

        private void Type_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Byte data;
            switch (this.Type_ComboBox.Text)
            {
                case "DirectSound":  data = (Byte)InstrumentType.Direct;  break;
                case "PSG Square 1": data = (Byte)InstrumentType.Square1; break;
                case "PSG Square 2": data = (Byte)InstrumentType.Square2; break;
                case "PSG Waveform": data = (Byte)InstrumentType.Wave;    break;
                case "PSG Noise":    data = (Byte)InstrumentType.Noise;   break;
                case "Fixed Pitch":  data = (Byte)InstrumentType.Fixed;   break;
                case "MultiSample":  data = (Byte)InstrumentType.Multi;   break;
                case "Percussion":   data = (Byte)InstrumentType.Drums;   break;
                default: data = 0x01; break;
            }
            Core.WriteByte(this,
                this.CurrentAddress,
                data,
                this.CurrentEntry + "Type changed");
        }
        private void BaseKey_ByteArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 1,
                this.BaseKey_ByteArrayBox.Value,
                this.CurrentEntry + "Base Key changed");
        }
        private void Panning_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 3,
                this.Panning_ByteBox.Value,
                this.CurrentEntry + "Panning changed");
        }
        private void Unused_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 2,
                this.Unused_ByteBox.Value,
                this.CurrentEntry + "Unused byte changed");
        }

        private void Envelope_Attack_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 8,
                this.Envelope_Attack_ByteBox.Value,
                this.CurrentEntry + "Env. Attack changed");
        }
        private void Envelope_Decay_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 8,
                this.Envelope_Decay_ByteBox.Value,
                this.CurrentEntry + "Env. Decay changed");
        }
        private void Envelope_Sustain_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 8,
                this.Envelope_Sustain_ByteBox.Value,
                this.CurrentEntry + "Env. Sustain changed");
        }
        private void Envelope_Release_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 8,
                this.Envelope_Release_ByteBox.Value,
                this.CurrentEntry + "Env. Release changed");
        }

        private void DutyPeriod_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.CurrentAddress + 8,
                (Byte)this.DutyPeriod_ComboBox.SelectedIndex,
                this.CurrentEntry + "Duty Period changed");
        }
        private void Sample_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.CurrentAddress + 4,
                this.Sample_PointerBox.Value,
                this.CurrentEntry + "Sample repointed");
        }
        private void KeyMap_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.CurrentAddress + 8,
                this.KeyMap_PointerBox.Value,
                this.CurrentEntry + "Key Map repointed");
        }

        private void Sample_Button_Click(Object sender, EventArgs e)
        {
            if (this.Current.Type == InstrumentType.Drums)
            {
                Program.Core.Core_OpenEditor(new InstrumentEditor( new InstrumentArray(this.Current.Sample), this.Current.BaseKey));
            }
            else
            {
                Program.Core.Core_OpenEditor(new SampleEditor(this.Current.Sample));
            }
        }
        private void KeyMap_Button_Click(Object sender, EventArgs e)
        {
            Program.Core.Core_OpenEditor(new KeyMapEditor(this.Current.KeyMap));
        }
    }
}
