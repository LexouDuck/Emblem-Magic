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
                return Instruments.GetAddress(Entry_ByteBox.Value);
            }
        }
        String CurrentEntry
        {
            get
            {
                return "Instrument 0x" + Util.ByteToHex(Entry_ByteBox.Value) + " from " + Instruments.Address + " - ";
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

        public InstrumentEditor(IApp app,
            InstrumentArray instruments,
            Byte index)
            : base(app)
        {
            try
            {
                InitializeComponent();

                Preview = new Music();

                Entry_ByteBox.ValueChanged -= Entry_ByteBox_ValueChanged;
                Entry_ByteBox.Value = index;
                Entry_ByteBox.ValueChanged += Entry_ByteBox_ValueChanged;

                Instruments = instruments;

                Instrument_PianoBox.SelectionChanged += Instrument_PianoBox_SelectionChanged;

                Address_Label.Text = Instruments.Address.ToString();

                Type_ComboBox.SelectedIndexChanged -= Type_ComboBox_SelectedIndexChanged;
                Type_ComboBox.DataSource = InstrumentTypes;
                Type_ComboBox.SelectedIndexChanged += Type_ComboBox_SelectedIndexChanged;

                BaseKey_ByteArrayBox.Load("Music Notes.txt");
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
            Current = Instruments[Entry_ByteBox.Value];

            Preview.Reset();
            Preview.Load(Instruments.GetAudio(Entry_ByteBox.Value));

            Core_LoadValues();
        }

        void Core_LoadValues()
        {
            Type_ComboBox.SelectedIndexChanged -= Type_ComboBox_SelectedIndexChanged;
            BaseKey_ByteArrayBox.ValueChanged -= BaseKey_ByteArrayBox_ValueChanged;
            Panning_ByteBox.ValueChanged -= Panning_ByteBox_ValueChanged;
            Unused_ByteBox.ValueChanged -= Unused_ByteBox_ValueChanged;

            Envelope_Attack_ByteBox.ValueChanged  -= Envelope_Attack_ByteBox_ValueChanged;
            Envelope_Decay_ByteBox.ValueChanged   -= Envelope_Decay_ByteBox_ValueChanged;
            Envelope_Sustain_ByteBox.ValueChanged -= Envelope_Sustain_ByteBox_ValueChanged;
            Envelope_Release_ByteBox.ValueChanged -= Envelope_Release_ByteBox_ValueChanged;

            DutyPeriod_ComboBox.SelectedIndexChanged -= DutyPeriod_ComboBox_SelectedIndexChanged;
            Sample_PointerBox.ValueChanged -= Sample_PointerBox_ValueChanged;
            KeyMap_PointerBox.ValueChanged -= KeyMap_PointerBox_ValueChanged;



            try
            {
                Int32 index = 0;
                switch (Current.Type)
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
                Type_ComboBox.SelectedIndex = Type_ComboBox.FindStringExact(InstrumentTypes[index]);
                BaseKey_ByteArrayBox.Value = Current.BaseKey;
                Panning_ByteBox.Value = Current.Panning;
                Unused_ByteBox.Value = Current.Unused;

                Envelope_Attack_ByteBox.Value = Current.Attack;
                Envelope_Decay_ByteBox.Value = Current.Decay;
                Envelope_Sustain_ByteBox.Value = Current.Sustain;
                Envelope_Release_ByteBox.Value = Current.Release;

                if (Current.Type == InstrumentType.Direct ||
                    Current.Type == InstrumentType.Fixed ||
                    Current.Type == InstrumentType.Wave ||
                    Current.Type == InstrumentType.Multi ||
                    Current.Type == InstrumentType.Drums)
                {
                    Sample_Label.Enabled = true;
                    Sample_Button.Enabled = true;
                    Sample_PointerBox.Enabled = true;
                    Sample_PointerBox.Value = Current.Sample;
                }
                else
                {
                    Sample_Label.Enabled = false;
                    Sample_Button.Enabled = false;
                    Sample_PointerBox.Enabled = false;
                    Sample_PointerBox.Value = new Pointer();
                }

                if (Current.Type == InstrumentType.Multi)
                {
                    KeyMap_Label.Enabled = true;
                    KeyMap_Label.Text = "Key Map :";
                    KeyMap_Button.Enabled = true;
                    KeyMap_PointerBox.Enabled = true;
                    KeyMap_PointerBox.Value = Current.KeyMap;
                }
                else if (Current.Type == InstrumentType.Wave)
                {
                    KeyMap_Label.Enabled = true;
                    KeyMap_Label.Text = "Wave :";
                    KeyMap_Button.Enabled = true;
                    KeyMap_PointerBox.Enabled = true;
                    KeyMap_PointerBox.Value = Current.KeyMap;
                }
                else
                {
                    KeyMap_Label.Enabled = false;
                    KeyMap_Button.Enabled = false;
                    KeyMap_PointerBox.Enabled = false;
                    KeyMap_PointerBox.Value = new Pointer();
                }

                if (Current.Type == InstrumentType.Square1 ||
                    Current.Type == InstrumentType.Square2)
                {
                    DutyPeriod_Label.Enabled = true;
                    DutyPeriod_ComboBox.Enabled = true;
                    DutyPeriod_ComboBox.DataSource = new String[]
                    {
                        "0x00 - 12.5%",
                        "0x01 - 25%",
                        "0x02 - 50%",
                        "0x03 - 75%",
                    };
                    DutyPeriod_ComboBox.SelectedIndex = Current.DutyPeriod;
                }
                else if (Current.Type == InstrumentType.Noise)
                {
                    DutyPeriod_Label.Enabled = true;
                    DutyPeriod_ComboBox.Enabled = true;
                    DutyPeriod_ComboBox.DataSource = new String[]
                    {
                        "0x00 - Normal",
                        "0x01 - Metallic",
                    };
                    DutyPeriod_ComboBox.SelectedIndex = Current.DutyPeriod;
                }
                else
                {
                    DutyPeriod_Label.Enabled = false;
                    DutyPeriod_ComboBox.Enabled = false;
                    DutyPeriod_ComboBox.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the instrument values.", ex);
            }



            Type_ComboBox.SelectedIndexChanged += Type_ComboBox_SelectedIndexChanged;
            BaseKey_ByteArrayBox.ValueChanged += BaseKey_ByteArrayBox_ValueChanged;
            Panning_ByteBox.ValueChanged += Panning_ByteBox_ValueChanged;
            Unused_ByteBox.ValueChanged += Unused_ByteBox_ValueChanged;

            Envelope_Attack_ByteBox.ValueChanged += Envelope_Attack_ByteBox_ValueChanged;
            Envelope_Decay_ByteBox.ValueChanged += Envelope_Decay_ByteBox_ValueChanged;
            Envelope_Sustain_ByteBox.ValueChanged += Envelope_Sustain_ByteBox_ValueChanged;
            Envelope_Release_ByteBox.ValueChanged += Envelope_Release_ByteBox_ValueChanged;

            DutyPeriod_ComboBox.SelectedIndexChanged += DutyPeriod_ComboBox_SelectedIndexChanged;
            Sample_PointerBox.ValueChanged += Sample_PointerBox_ValueChanged;
            KeyMap_PointerBox.ValueChanged += KeyMap_PointerBox_ValueChanged;
        }



        private void Entry_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Instrument_PianoBox_SelectionChanged(Object sender, EventArgs e)
        {
            for (Byte i = 0; i < Music.NOTES; i++)
            {
                if (Instrument_PianoBox.Selection[i])
                {
                    Preview.PlayAudio(0, i);
                }
                else if (!Preview.Samples[0][i].Cancel)
                {
                    Preview.Samples[0][i].State = Audio.ADSR.Release;
                }
            }
        }

        private void Type_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Byte data;
            switch (Type_ComboBox.Text)
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
                CurrentAddress,
                data,
                CurrentEntry + "Type changed");
        }
        private void BaseKey_ByteArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 1,
                BaseKey_ByteArrayBox.Value,
                CurrentEntry + "Base Key changed");
        }
        private void Panning_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 3,
                Panning_ByteBox.Value,
                CurrentEntry + "Panning changed");
        }
        private void Unused_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 2,
                Unused_ByteBox.Value,
                CurrentEntry + "Unused byte changed");
        }

        private void Envelope_Attack_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 8,
                Envelope_Attack_ByteBox.Value,
                CurrentEntry + "Env. Attack changed");
        }
        private void Envelope_Decay_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 8,
                Envelope_Decay_ByteBox.Value,
                CurrentEntry + "Env. Decay changed");
        }
        private void Envelope_Sustain_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 8,
                Envelope_Sustain_ByteBox.Value,
                CurrentEntry + "Env. Sustain changed");
        }
        private void Envelope_Release_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 8,
                Envelope_Release_ByteBox.Value,
                CurrentEntry + "Env. Release changed");
        }

        private void DutyPeriod_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                CurrentAddress + 8,
                (Byte)DutyPeriod_ComboBox.SelectedIndex,
                CurrentEntry + "Duty Period changed");
        }
        private void Sample_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentAddress + 4,
                Sample_PointerBox.Value,
                CurrentEntry + "Sample repointed");
        }
        private void KeyMap_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                CurrentAddress + 8,
                KeyMap_PointerBox.Value,
                CurrentEntry + "Key Map repointed");
        }

        private void Sample_Button_Click(Object sender, EventArgs e)
        {
            if (Current.Type == InstrumentType.Drums)
            {
                Program.Core.Core_OpenEditor(new InstrumentEditor(App, new InstrumentArray(Current.Sample), Current.BaseKey));
            }
            else
            {
                Program.Core.Core_OpenEditor(new SampleEditor(App, Current.Sample));
            }
        }
        private void KeyMap_Button_Click(Object sender, EventArgs e)
        {
            Program.Core.Core_OpenEditor(new KeyMapEditor(App, Current.KeyMap));
        }
    }
}
