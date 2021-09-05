using System;
using Magic;

namespace GBA
{
    /// <summary>
    /// Supplies the basic waveform used to output sound with the Music object
    /// </summary>
    public abstract class Audio
    {
        /// <summary>
        /// Stops the playback thread when set to true
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// The current position in the sample
        /// </summary>
        public uint Position { get; set; }

        protected float Attack;
        protected float Decay;
        protected float Sustain;
        protected float Release;

        public enum ADSR
        {
            None,
            Attack,
            Decay,
            Sustain,
            Release
        }
        public ADSR State;
        protected double Envelope;

        public Audio(Instrument instrument)
        {
            Cancel = false;
            Position = 0;

            Attack = (0xFF - instrument.Attack) / (float)0xFF;
            Decay =   instrument.Decay   / (float)0xFF;
            Sustain = instrument.Sustain / (float)0xFF;
            Release = instrument.Release / (float)0xFF;

            Envelope = 0;
            State = ADSR.Attack;
        }

        protected void UpdateEnvelope()
        {
            switch (State)
            {
                case ADSR.Attack:
                    Envelope += 1 / (Attack * SAMPLE_RATE);
                    if (Envelope >= 1)
                    {
                        Envelope = 1;
                        State = ADSR.Decay;
                    }
                    break;

                case ADSR.Decay:
                    Envelope -= 1 / (Decay * SAMPLE_RATE);
                    if (Envelope <= Sustain)
                    {
                        Envelope = Sustain;
                        State = ADSR.Sustain;
                    }
                    break;

                case ADSR.Sustain:
                    // the change to Release state is done externally
                    break;

                case ADSR.Release:
                    Envelope -= 1 / (Release * SAMPLE_RATE);
                    if (Envelope <= 0)
                    {
                        Envelope = 0;
                        Cancel = true;
                    }
                    break;

                default: throw new Exception("ADSR State is invalid:" + State);
            }
        }

        /// <summary>
        /// Returns the next buffer of sound bytes to output
        /// </summary>
        public abstract byte[] GetBuffer(int length);
        


        /// <summary>
        /// The default sampling rate of the GBA
        /// </summary>
        public const int SAMPLE_RATE = 32768;

        /// <summary>
        /// Uses linear interpolation to return a byte array resampled to the new frequency
        /// </summary>
        public static byte[] ChangeSampleRate(byte[] data, uint old_frequency, uint new_frequency)
        {
            if (old_frequency == new_frequency) return data;

            double factor = (double)old_frequency / (double)new_frequency;
            int length = (int)Math.Floor((data.Length - 1) / factor);
            byte[] result = new byte[length];
            result[0] = data[0];
            int index1;
            int index2;
            for (int i = 1; i < length; i++)
            {
                index1 = (int)Math.Floor(i * factor);
                index2 = (int)Math.Ceiling(i * factor);
                result[i] = (byte)((data[index1] + data[index2]) / 2);
            }
            return result;
        }
    }



    public class Audio_DirectSound : Audio
    {
        uint Frequency;
        bool Looped;
        uint LoopStart;
        byte[] Sample;

        public Audio_DirectSound(Instrument instrument, Sample sample, uint frequency = 0) : base(instrument)
        {
            Frequency = (frequency == 0 ? sample.Pitch : frequency) / 1024;

            Sample = ChangeSampleRate(sample.PCM_Data, Frequency, SAMPLE_RATE);

            if (sample.Looped)
            {
                Looped = true;
                LoopStart = (uint)(sample.LoopStart / ((double)Frequency / (double)SAMPLE_RATE));
                if (LoopStart >= Sample.Length)
                    throw new Exception("Invalid loop starting point, is beyond the sample length.");
            }
            else
            {
                Looped = false;
                LoopStart = 0;
            }
        }

        public override Byte[] GetBuffer(int length)
        {
            byte[] result = new byte[length];
            byte pcm = 0x00;
            for (int i = 0; i < length; i++)
            {
                if (Position >= Sample.Length)
                {
                    if (Looped)
                    {
                        Position = LoopStart;
                    }
                    else
                    {
                        Position = 0;
                        Cancel = true;
                    }
                }
                UpdateEnvelope();
                try
                {
                    pcm = (byte)(Sample[Position++] * Envelope);
                }
                catch (Exception ex)
                {
                    UI.ShowError("sample: " + Sample.Length + ", position:" + Position, ex);
                }
                result[i] = pcm;
            }
            return result;
        }
    }
}
