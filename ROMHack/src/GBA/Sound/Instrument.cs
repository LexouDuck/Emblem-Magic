using System;
using Magic;

namespace GBA
{
    /// <summary>
    /// The different kinds of instruments - the first byte of an instrument struct
    /// </summary>
    public enum InstrumentType
    {
        Direct  = 0x00, // Sample (GBA DirectSound channel)
        Square1 = 0x01, // PSG Square 1 (Game Boy channel 1)
        Square2 = 0x02, // PSG Square 2 (Game Boy channel 2)
        Wave    = 0x03, // PSG Programmable Waveform (Game Boy channel 3)
        Noise   = 0x04, // PSG Noise (Game Boy channel 4)
        Fixed   = 0x08, // ignore any pitch offsets and play the sample at the original frequency
        Multi   = 0x40, // Key-split instrument - can set several samples for note ranges
        Drums   = 0x80, // Percussion instrument - each note has its own sound sample
    }
    /// <summary>
    /// Also sometimes called a "voice", an instrument is the basis of any sound on the GBA
    /// </summary>
    public struct Instrument
    {
        public const Int32 LENGTH = 12;

        Byte[] Data;



        public Instrument(Byte[] data)
        {
            if (data.Length != LENGTH)
                throw new Exception("Data given has invalid length.");

            Data = data;
        }



        public Byte[] ToBytes()
        {
            return Data;
        }

        /// <summary>
        /// Returns true if the instrument has bytes which indicate it is not used
        /// </summary>
        public Boolean IsUnused()
        {
            return (
                Data[0x0] == 0x01 &&
                Data[0x1] == 0x3C &&
                Data[0x2] == 0x00 &&
                Data[0x3] == 0x00 &&
                Data[0x4] == 0x02 &&
                Data[0x5] == 0x00 &&
                Data[0x6] == 0x00 &&
                Data[0x7] == 0x00 &&
                Data[0x8] == 0x00 &&
                Data[0x9] == 0x00 &&
                Data[0xA] == 0x0F &&
                Data[0xB] == 0x00);
        } 



        /// <summary>
        /// The kind of instrument - changes a lot about how the struct works
        /// </summary>
        public InstrumentType Type
        {
            get
            {
                return (InstrumentType)Data[0];
            }
        }
        /// <summary>
        /// Which note this instrument plays for (only used as percussion sub-instrument)
        /// </summary>
        public Byte BaseKey
        {
            get
            {
                return Data[1];
            }
        }
        /// <summary>
        /// This byte is apparently always unused
        /// </summary>
        public Byte Unused
        {
            get
            {
                return Data[2];
            }
        }
        /// <summary>
        /// The left/right sound panning (only used as percussion sub-instrument and if bit 7 of the value is set)
        /// </summary>
        public Byte Panning
        {
            get
            {
                return Data[3];
            }
        }
        
        /// <summary>
        /// Pointer to the sample data for this instrument
        /// </summary>
        public Pointer Sample
        {
            get
            {
                return new Pointer(Data.GetUInt32(4, true), false, true);
            }
        }
        
        /// <summary>
        /// if Type=Multi, pointer to key-split mapping data.
        /// if Type=Drum, pointer to percussion sub-instrument array.
        /// </summary>
        public Pointer KeyMap
        {
            get
            {
                return new Pointer(Data.GetUInt32(8, true), false, true);
            }
        }
        /// <summary>
        /// if Type=PSGchannel, is the wave's duty cycle (0=12,5%, 1=25%, 2=50%, 3=75%).
        /// if Type=Noise, is noise's period (0 = normal (32767 samples), 1 = metallic (127 samples)).
        /// </summary>
        public Byte DutyPeriod
        {
            get
            {
                return Data[8];
            }
        }
        
        /// <summary>
        /// Sound attack value;
        /// if Type=PSGchannel (0x00 = no attack, 0x07 = longest attack)
        /// else (0x01 = longest attack, 0xFF = no attack)
        /// </summary>
        public Byte Attack
        {
            get
            {
                return Data[8];
            }
        }
        /// <summary>
        /// Sound decay value;
        /// if Type=PSGchannel (0x00 = no decay, 0x01 = fastest decay, 0x07 = slowest decay)
        /// else (0x00 = no decay, 0xFF = longest decay)
        /// </summary>
        public Byte Decay
        {
            get
            {
                return Data[9];
            }
        }
        /// <summary>
        /// Sound sustain level;
        /// if Type=PSGchannel (0x00 = sustain to silence, 0x0F = sustain to full volume)
        /// else (0x00 = sustain to silence, 0xFF = sustain to full volume)
        /// </summary>
        public Byte Sustain
        {
            get
            {
                return Data[10];
            }
        }
        /// <summary>
        /// Sound release value;
        /// if Type=PSGchannel (0x00 = no release, 0x07 = slowest release)
        /// else (0x00 = instantaneous release, 0xFF = longest release)
        /// </summary>
        public Byte Release
        {
            get
            {
                return Data[11];
            }
        }
    }
}
