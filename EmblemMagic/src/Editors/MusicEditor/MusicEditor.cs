using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Magic;
using Magic.Editors;
using GBA;

namespace EmblemMagic.Editors
{
    public partial class MusicEditor : Editor
    {
        StructFile Current;
        MusicHeader CurrentMusic;
        InstrumentArray Instruments;

        Music Preview;
        /// <summary>
        /// Gets a string representing the current entry in the music array
        /// </summary>
        string CurrentEntry
        {
            get
            {
                return "Music 0x" + Util.ByteToHex(EntryArrayBox.Value) + " [" + EntryArrayBox.Text + "] - ";
            }
        }



        public MusicEditor(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();

                EntryArrayBox.Load("Music List.txt");
                Current = new StructFile("Music Struct.txt");
                Current.Address = Core.GetPointer("Music Array");

                Preview = new Music();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        public override void Core_SetEntry(uint entry)
        {
            EntryArrayBox.Value =(byte)entry;
        }
        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Current.EntryIndex = EntryArrayBox.Value;

            CurrentMusic = new MusicHeader((Pointer)Current["Address"]);

            Core_LoadInstruments();
            Core_LoadTracker();
            Core_LoadValues();
            //Core_LoadAudio();
        }

        void Core_LoadInstruments()
        {
            try
            {
                if (CurrentMusic.TrackAmount == 0)
                {
                    Instrument_PointerBox.Value = new Pointer();

                    Instrument_ListBox.DataSource = null;
                }
                else
                {
                    Instrument_PointerBox.ValueChanged -= Instrument_PointerBox_ValueChanged;
                    Instrument_PointerBox.Value = CurrentMusic.Instruments;
                    Instrument_PointerBox.ValueChanged += Instrument_PointerBox_ValueChanged;

                    Instruments = new InstrumentArray(CurrentMusic.Instruments);
                    List<string> instruments = new List<string>();
                    string instrument;
                    for (byte i = 0; i < 128; i++)
                    {
                        if (Instruments[i].IsUnused())
                        {
                            if (View_HideInstruments.Checked)
                                continue;
                            else instrument = "None";
                        }
                        else switch (Instruments[i].Type)
                        {
                            case InstrumentType.Direct:  instrument = "DirectSound";  break;
                            case InstrumentType.Square1: instrument = "PSG Square 1"; break;
                            case InstrumentType.Square2: instrument = "PSG Square 2"; break;
                            case InstrumentType.Wave:    instrument = "PSG Waveform"; break;
                            case InstrumentType.Noise:   instrument = "PSG Noise";    break;
                            case InstrumentType.Fixed:   instrument = "Fixed Pitch";  break;
                            case InstrumentType.Multi:   instrument = "MultiSample";  break;
                            case InstrumentType.Drums:   instrument = "Percussion";   break;
                            default: instrument = ""; break;
                        }
                        instruments.Add(Util.ByteToHex(i) + " - " + instrument);
                    }
                    Instrument_ListBox.DataSource = instruments;
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the instrument array.", ex);
            }
        }
        void Core_LoadTracker()
        {
            List<Track> tracks = new List<Track>();
            try
            {
                Pointer[] pointers = CurrentMusic.Tracks;
                for (int i = 0; i < CurrentMusic.TrackAmount; i++)
                {
                    tracks.Add(new Track(pointers[i]));
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while reading tracks.", ex);
            }
            Music_Tracker.Load(tracks);
        }
        void Core_LoadValues()
        {
            Entry_PointerBox.ValueChanged -= Entry_PointerBox_ValueChanged;
            Entry_ByteBox1.ValueChanged -= Entry_ByteBox1_ValueChanged;
            Entry_ByteBox2.ValueChanged -= Entry_ByteBox2_ValueChanged;

            TrackAmount_ByteBox.ValueChanged -= TrackAmount_ByteBox_ValueChanged;
            Music_Unknown_ByteBox.ValueChanged -= Music_Unknown_ByteBox_ValueChanged;
            Music_Priority_ByteBox.ValueChanged -= Music_Priority_ByteBox_ValueChanged;
            Music_Reverb_ByteBox.ValueChanged -= Music_Reverb_ByteBox_ValueChanged;
            
            try
            {
                Entry_PointerBox.Value = (Pointer)Current["Address"];
                Entry_ByteBox1.Value = (byte)Current["TrackGroup1"];
                Entry_ByteBox2.Value = (byte)Current["TrackGroup2"];

                TrackAmount_ByteBox.Value = CurrentMusic.TrackAmount;
                Music_Unknown_ByteBox.Value = CurrentMusic.Unknown;
                Music_Priority_ByteBox.Value = CurrentMusic.Priority;
                Music_Reverb_ByteBox.Value = CurrentMusic.Reverb;
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);

                Entry_PointerBox.Value = new Pointer();
                Entry_ByteBox1.Value = 0;
                Entry_ByteBox2.Value = 0;

                TrackAmount_ByteBox.Value = 0;
                Music_Unknown_ByteBox.Value = 0;
                Music_Priority_ByteBox.Value = 0;
                Music_Reverb_ByteBox.Value = 0;
            }
            
            Entry_PointerBox.ValueChanged += Entry_PointerBox_ValueChanged;
            Entry_ByteBox1.ValueChanged += Entry_ByteBox1_ValueChanged;
            Entry_ByteBox2.ValueChanged += Entry_ByteBox2_ValueChanged;

            TrackAmount_ByteBox.ValueChanged += TrackAmount_ByteBox_ValueChanged;
            Music_Unknown_ByteBox.ValueChanged += Music_Unknown_ByteBox_ValueChanged;
            Music_Priority_ByteBox.ValueChanged += Music_Priority_ByteBox_ValueChanged;
            Music_Reverb_ByteBox.ValueChanged += Music_Reverb_ByteBox_ValueChanged;
        }
        void Core_LoadAudio()
        {
            Preview.Reset();

            for (byte i = 0; i < 128; i++)
            {
                if (Instruments[i].IsUnused())
                    Preview.Load(null);
                else Preview.Load(Instruments.GetAudio(i));
            }
        }



        private void File_Insert_Click(object sender, EventArgs e)
        {

        }
        private void File_Save_Click(object sender, EventArgs e)
        {

        }
        private void View_HideInstruments_Click(object sender, EventArgs e)
        {
            Core_LoadInstruments();
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Entry_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Address"),
                Instrument_PointerBox.Value,
                CurrentEntry + "Music Header repointed");
        }
        private void Entry_ByteBox1_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "TrackGroup1"),
                Entry_ByteBox1.Value,
                CurrentEntry + "Track Group 1 changed");
        }
        private void Entry_ByteBox2_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "TrackGroup2"),
                Entry_ByteBox2.Value,
                CurrentEntry + "Track Group 2 changed");
        }

        private void Instrument_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.Address + 4,
                Instrument_PointerBox.Value,
                CurrentEntry + "Instruments repointed");
        }
        private void Instrument_ListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            App.Core_OpenEditor(new InstrumentEditor(App, Instruments,
                Util.HexToByte(((string)Instrument_ListBox.SelectedItem).Substring(0, 2))));
        }

        private void TrackAmount_ByteBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.Address,
                TrackAmount_ByteBox.Value,
                CurrentEntry + "Track Amount changed");
        }
        private void Music_Unknown_ByteBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.Address + 1,
                Music_Unknown_ByteBox.Value,
                CurrentEntry + "Unknown changed");
        }
        private void Music_Priority_ByteBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.Address + 2,
                Music_Priority_ByteBox.Value,
                CurrentEntry + "Priority changed");
        }
        private void Music_Reverb_ByteBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.Address + 3,
                Music_Reverb_ByteBox.Value,
                CurrentEntry + "Reverb changed");
        }

        private void PlayStop_Button_Click(Object sender, EventArgs e)
        {

        }
    }
}