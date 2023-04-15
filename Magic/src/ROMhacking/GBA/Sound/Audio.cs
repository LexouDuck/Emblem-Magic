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
        public Boolean Cancel { get; set; }

        /// <summary>
        /// The current position in the sample
        /// </summary>
        public UInt32 Position { get; set; }

        protected Single Attack;
        protected Single Decay;
        protected Single Sustain;
        protected Single Release;

        public enum ADSR
        {
            None,
            Attack,
            Decay,
            Sustain,
            Release
        }
        public ADSR State;
        protected Double Envelope;

        public Audio(Instrument instrument)
        {
            this.Cancel = false;
            this.Position = 0;

            this.Attack = (0xFF - instrument.Attack) / (Single)0xFF;
            this.Decay =   instrument.Decay   / (Single)0xFF;
            this.Sustain = instrument.Sustain / (Single)0xFF;
            this.Release = instrument.Release / (Single)0xFF;

            this.Envelope = 0;
            this.State = ADSR.Attack;
        }

        protected void UpdateEnvelope()
        {
            switch (this.State)
            {
                case ADSR.Attack:
                    this.Envelope += 1 / (this.Attack * SAMPLE_RATE);
                    if (this.Envelope >= 1)
                    {
                        this.Envelope = 1;
                        this.State = ADSR.Decay;
                    }
                    break;

                case ADSR.Decay:
                    this.Envelope -= 1 / (this.Decay * SAMPLE_RATE);
                    if (this.Envelope <= this.Sustain)
                    {
                        this.Envelope = this.Sustain;
                        this.State = ADSR.Sustain;
                    }
                    break;

                case ADSR.Sustain:
                    // the change to Release state is done externally
                    break;

                case ADSR.Release:
                    this.Envelope -= 1 / (this.Release * SAMPLE_RATE);
                    if (this.Envelope <= 0)
                    {
                        this.Envelope = 0;
                        this.Cancel = true;
                    }
                    break;

                default: throw new Exception("ADSR State is invalid:" + this.State);
            }
        }

        /// <summary>
        /// Returns the next buffer of sound bytes to output
        /// </summary>
        public abstract Byte[] GetBuffer(Int32 length);
        


        /// <summary>
        /// The default sampling rate of the GBA
        /// </summary>
        public const Int32 SAMPLE_RATE = 32768;

        /// <summary>
        /// Uses linear interpolation to return a byte array resampled to the new frequency
        /// </summary>
        public static Byte[] ChangeSampleRate(Byte[] data, UInt32 old_frequency, UInt32 new_frequency)
        {
            if (old_frequency == new_frequency) return data;

            Double factor = (Double)old_frequency / (Double)new_frequency;
            Int32 length = (Int32)Math.Floor((data.Length - 1) / factor);
            Byte[] result = new Byte[length];
            result[0] = data[0];
            Int32 index1;
            Int32 index2;
            for (Int32 i = 1; i < length; i++)
            {
                index1 = (Int32)Math.Floor(i * factor);
                index2 = (Int32)Math.Ceiling(i * factor);
                result[i] = (Byte)((data[index1] + data[index2]) / 2);
            }
            return result;
        }
    }



    public class Audio_DirectSound : Audio
    {
        UInt32 Frequency;
        Boolean Looped;
        UInt32 LoopStart;
        Byte[] Sample;

        public Audio_DirectSound(Instrument instrument, Sample sample, UInt32 frequency = 0) : base(instrument)
        {
            this.Frequency = (frequency == 0 ? sample.Pitch : frequency) / 1024;

            this.Sample = ChangeSampleRate(sample.PCM_Data, this.Frequency, SAMPLE_RATE);

            if (sample.Looped)
            {
                this.Looped = true;
                this.LoopStart = (UInt32)(sample.LoopStart / ((Double)this.Frequency / (Double)SAMPLE_RATE));
                if (this.LoopStart >= this.Sample.Length)
                    throw new Exception("Invalid loop starting point, is beyond the sample length.");
            }
            else
            {
                this.Looped = false;
                this.LoopStart = 0;
            }
        }

        public override Byte[] GetBuffer(Int32 length)
        {
            Byte[] result = new Byte[length];
            Byte pcm = 0x00;
            for (Int32 i = 0; i < length; i++)
            {
                if (this.Position >= this.Sample.Length)
                {
                    if (this.Looped)
                    {
                        this.Position = this.LoopStart;
                    }
                    else
                    {
                        this.Position = 0;
                        this.Cancel = true;
                    }
                }
                this.UpdateEnvelope();
                try
                {
                    pcm = (Byte)(this.Sample[this.Position++] * this.Envelope);
                }
                catch (Exception ex)
                {
                    UI.ShowError("sample: " + this.Sample.Length + ", position:" + this.Position, ex);
                }
                result[i] = pcm;
            }
            return result;
        }
    }
}
