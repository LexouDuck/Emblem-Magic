using System;
using Magic;

namespace GBA
{
    public class InstrumentArray
    {
        public Instrument this[Int32 index]
        {
            get
            {
                return new Instrument(Core.ReadData(this.GetAddress(index), Instrument.LENGTH));
            }
        }

        public Pointer Address;

        public InstrumentArray(Pointer address)
        {
            this.Address = address;
        }



        public Pointer GetAddress(Int32 index)
        {
            return this.Address + Instrument.LENGTH * index;
        }

        /// <summary>
        /// returns the 16-byte data for a programmable waveform
        /// </summary>
        public Byte[] GetProgrammableWaveData(Pointer address)
        {
            return Core.ReadData(address, 16);
        }



        public Audio[] GetAudio(Int32 index)
        {
            Audio[] result = new Audio[Music.NOTES];
            Instrument entry = this[index];
            Sample sample = new Sample(entry.Sample);

            Instrument instrument;
            KeyMap keymap;
            for (Int32 i = 0; i < result.Length; i++)
            {
                switch (entry.Type)
                {
                    case InstrumentType.Direct:
                        UInt32 frequency = (UInt32)(sample.Pitch * Math.Pow(2, ((i - entry.BaseKey) / (Double)12)));
                        result[i] = new Audio_DirectSound(entry, sample, frequency);
                        break;

                    case InstrumentType.Square1: return null;
                    case InstrumentType.Square2: return null;
                    case InstrumentType.Wave:    return null;
                    case InstrumentType.Noise:   return null;

                    case InstrumentType.Fixed:
                        result[i] = new Audio_DirectSound(entry, sample);
                        break;

                    case InstrumentType.Multi:
                        keymap = new KeyMap(Core.ReadData(entry.KeyMap, KeyMap.LENGTH));
                        instrument = this[keymap[i]];
                        result[i] = new Audio_DirectSound(instrument, new Sample(instrument.Sample));
                        break;

                    case InstrumentType.Drums:
                        instrument = new Instrument(Core.ReadData(entry.Sample, Instrument.LENGTH));
                        result[i] = new Audio_DirectSound(instrument, new Sample(instrument.Sample));
                        break;

                    default: throw new Exception("Invalid instrument type.");
                }
            }
            return result;
        }
    }
}
