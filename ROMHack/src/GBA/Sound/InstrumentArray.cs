using System;
using Magic;

namespace GBA
{
    public class InstrumentArray
    {
        public Instrument this[int index]
        {
            get
            {
                return new Instrument(Core.ReadData(GetAddress(index), Instrument.LENGTH));
            }
        }

        public Pointer Address;

        public InstrumentArray(Pointer address)
        {
            Address = address;
        }



        public Pointer GetAddress(int index)
        {
            return Address + Instrument.LENGTH * index;
        }

        /// <summary>
        /// returns the 16-byte data for a programmable waveform
        /// </summary>
        public byte[] GetProgrammableWaveData(Pointer address)
        {
            return Core.ReadData(address, 16);
        }



        public Audio[] GetAudio(int index)
        {
            Audio[] result = new Audio[Music.NOTES];
            Instrument entry = this[index];
            Sample sample = new Sample(entry.Sample);

            Instrument instrument;
            KeyMap keymap;
            for (int i = 0; i < result.Length; i++)
            {
                switch (entry.Type)
                {
                    case InstrumentType.Direct:
                        uint frequency = (uint)(sample.Pitch * Math.Pow(2, ((i - entry.BaseKey) / (double)12)));
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
