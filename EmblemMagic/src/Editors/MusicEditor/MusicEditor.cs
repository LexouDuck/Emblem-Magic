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
        String CurrentEntry
        {
            get
            {
                return "Music 0x" + Util.ByteToHex(this.EntryArrayBox.Value) + " [" + this.EntryArrayBox.Text + "] - ";
            }
        }



        public MusicEditor()
        {
            try
            {
                this.InitializeComponent();

                this.EntryArrayBox.Load("Music List.txt");
                this.Current = new StructFile("Music Struct.txt");
                this.Current.Address = Core.GetPointer("Music Array");

                this.Preview = new Music();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        public override void Core_SetEntry(UInt32 entry)
        {
            this.EntryArrayBox.Value =(Byte)entry;
        }
        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Current.EntryIndex = this.EntryArrayBox.Value;

            this.CurrentMusic = new MusicHeader((Pointer)this.Current["Address"]);

            this.Core_LoadInstruments();
            this.Core_LoadTracker();
            this.Core_LoadValues();
            //Core_LoadAudio();
        }

        void Core_LoadInstruments()
        {
            try
            {
                if (this.CurrentMusic.TrackAmount == 0)
                {
                    this.Instrument_PointerBox.Value = new Pointer();

                    this.Instrument_ListBox.DataSource = null;
                }
                else
                {
                    this.Instrument_PointerBox.ValueChanged -= this.Instrument_PointerBox_ValueChanged;
                    this.Instrument_PointerBox.Value = this.CurrentMusic.Instruments;
                    this.Instrument_PointerBox.ValueChanged += this.Instrument_PointerBox_ValueChanged;

                    this.Instruments = new InstrumentArray(this.CurrentMusic.Instruments);
                    List<String> instruments = new List<String>();
                    String instrument;
                    for (Byte i = 0; i < 128; i++)
                    {
                        if (this.Instruments[i].IsUnused())
                        {
                            if (this.View_HideInstruments.Checked)
                                continue;
                            else instrument = "None";
                        }
                        else switch (this.Instruments[i].Type)
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
                    this.Instrument_ListBox.DataSource = instruments;
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
                Pointer[] pointers = this.CurrentMusic.Tracks;
                for (Int32 i = 0; i < this.CurrentMusic.TrackAmount; i++)
                {
                    tracks.Add(new Track(pointers[i]));
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while reading tracks.", ex);
            }
            this.Music_Tracker.Load(tracks);
        }
        void Core_LoadValues()
        {
            this.Entry_PointerBox.ValueChanged -= this.Entry_PointerBox_ValueChanged;
            this.Entry_ByteBox1.ValueChanged -= this.Entry_ByteBox1_ValueChanged;
            this.Entry_ByteBox2.ValueChanged -= this.Entry_ByteBox2_ValueChanged;

            this.TrackAmount_ByteBox.ValueChanged -= this.TrackAmount_ByteBox_ValueChanged;
            this.Music_Unknown_ByteBox.ValueChanged -= this.Music_Unknown_ByteBox_ValueChanged;
            this.Music_Priority_ByteBox.ValueChanged -= this.Music_Priority_ByteBox_ValueChanged;
            this.Music_Reverb_ByteBox.ValueChanged -= this.Music_Reverb_ByteBox_ValueChanged;
            
            try
            {
                this.Entry_PointerBox.Value = (Pointer)this.Current["Address"];
                this.Entry_ByteBox1.Value = (Byte)this.Current["TrackGroup1"];
                this.Entry_ByteBox2.Value = (Byte)this.Current["TrackGroup2"];

                this.TrackAmount_ByteBox.Value = this.CurrentMusic.TrackAmount;
                this.Music_Unknown_ByteBox.Value = this.CurrentMusic.Unknown;
                this.Music_Priority_ByteBox.Value = this.CurrentMusic.Priority;
                this.Music_Reverb_ByteBox.Value = this.CurrentMusic.Reverb;
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);

                this.Entry_PointerBox.Value = new Pointer();
                this.Entry_ByteBox1.Value = 0;
                this.Entry_ByteBox2.Value = 0;

                this.TrackAmount_ByteBox.Value = 0;
                this.Music_Unknown_ByteBox.Value = 0;
                this.Music_Priority_ByteBox.Value = 0;
                this.Music_Reverb_ByteBox.Value = 0;
            }

            this.Entry_PointerBox.ValueChanged += this.Entry_PointerBox_ValueChanged;
            this.Entry_ByteBox1.ValueChanged += this.Entry_ByteBox1_ValueChanged;
            this.Entry_ByteBox2.ValueChanged += this.Entry_ByteBox2_ValueChanged;

            this.TrackAmount_ByteBox.ValueChanged += this.TrackAmount_ByteBox_ValueChanged;
            this.Music_Unknown_ByteBox.ValueChanged += this.Music_Unknown_ByteBox_ValueChanged;
            this.Music_Priority_ByteBox.ValueChanged += this.Music_Priority_ByteBox_ValueChanged;
            this.Music_Reverb_ByteBox.ValueChanged += this.Music_Reverb_ByteBox_ValueChanged;
        }
        void Core_LoadAudio()
        {
            this.Preview.Reset();

            for (Byte i = 0; i < 128; i++)
            {
                if (this.Instruments[i].IsUnused())
                    this.Preview.Load(null);
                else this.Preview.Load(this.Instruments.GetAudio(i));
            }
        }



        private void File_Insert_Click(Object sender, EventArgs e)
        {

        }
        private void File_Save_Click(Object sender, EventArgs e)
        {

        }
        private void View_HideInstruments_Click(Object sender, EventArgs e)
        {
            this.Core_LoadInstruments();
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void Entry_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Address"),
                this.Instrument_PointerBox.Value,
                this.CurrentEntry + "Music Header repointed");
        }
        private void Entry_ByteBox1_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "TrackGroup1"),
                this.Entry_ByteBox1.Value,
                this.CurrentEntry + "Track Group 1 changed");
        }
        private void Entry_ByteBox2_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "TrackGroup2"),
                this.Entry_ByteBox2.Value,
                this.CurrentEntry + "Track Group 2 changed");
        }

        private void Instrument_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.Address + 4,
                this.Instrument_PointerBox.Value,
                this.CurrentEntry + "Instruments repointed");
        }
        private void Instrument_ListBox_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            this.App.Core_OpenEditor(new InstrumentEditor(this.Instruments,
                Util.HexToByte(((String)this.Instrument_ListBox.SelectedItem).Substring(0, 2))));
        }

        private void TrackAmount_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.Address,
                this.TrackAmount_ByteBox.Value,
                this.CurrentEntry + "Track Amount changed");
        }
        private void Music_Unknown_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.Address + 1,
                this.Music_Unknown_ByteBox.Value,
                this.CurrentEntry + "Unknown changed");
        }
        private void Music_Priority_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.Address + 2,
                this.Music_Priority_ByteBox.Value,
                this.CurrentEntry + "Priority changed");
        }
        private void Music_Reverb_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.Address + 3,
                this.Music_Reverb_ByteBox.Value,
                this.CurrentEntry + "Reverb changed");
        }

        private void PlayStop_Button_Click(Object sender, EventArgs e)
        {

        }
    }
}