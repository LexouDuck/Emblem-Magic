using System;
using System.Collections.Generic;
using System.Threading;
using Magic;
using WinMM;

namespace GBA
{
    /// <summary>
    /// This class is used for playing GBA music out on the speakers
    /// </summary>
    public class Music : IDisposable
    {
        /// <summary>
        /// The total amount of notes in the GBA sappy engine
        /// </summary>
        public const Int32 NOTES = 120;

        /// <summary>
        /// The object used to write sound output data to the speakers
        /// </summary>
        public WaveOut Output { get; private set; }
        /// <summary>
        /// The asynchronous thread used to feed data to the WaveOut object
        /// </summary>
        public Thread Player { get; private set; }
        /// <summary>
        /// The samples that this 'Music' object is currently mixing together and playing
        /// </summary>
        public List<Audio> Playing { get; set; }
        /// <summary>
        /// The note samples of the instruments to play
        /// </summary>
        public List<Audio[]> Samples { get; }



        public Music()
        {
            this.Output = new WaveOut(WaveOut.WaveOutMapperDeviceId);
            this.Output.Open(WaveFormat.Pcm32Khz8BitMono);
            this.Player = null;
            this.Playing = new List<Audio>();
            this.Samples = new List<Audio[]>();
        }
        public void Load(Audio[] notes)
        {
            this.Samples.Add(notes);
        }
        public void Reset()
        {
            this.Samples.Clear();
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        protected virtual void Dispose(Boolean full)
        {
            if (this.Output != null)
            {
                this.Output.Close();
                this.Output.Dispose();
                this.Output = null;
            }
            GC.SuppressFinalize(this);
        }

        
        
        void RunAudioThread()
        {
            while (this.Playing.Count > 0)
            {
                Byte[] buffer = this.MixAudioBuffers(4096); // 3145.728

                if (buffer != null) this.Output.Write(buffer);

                Thread.Sleep(10); // 10.416666...
            }
            this.Output.Stop();
            this.Player = null;
        }
        Byte[] MixAudioBuffers(Int32 length)
        {
            Byte[] result = new Byte[length];

            List<Byte[]> buffers = new List<Byte[]>();
            for (Int32 i = 0; i < this.Playing.Count; i++)
            {
                if (this.Playing[i].Cancel) this.Playing.RemoveAt(i);
                else buffers.Add(this.Playing[i].GetBuffer(result.Length));
            }
            if (buffers.Count == 0) return null;
            Single sum_total;
            for (Int32 i = 0; i < 4096; i++)
            {
                sum_total = 0;
                foreach (Byte[] buffer in buffers)
                {
                    sum_total += buffer[i] / (Single)256;
                }
                sum_total /= buffers.Count;
                sum_total *= 0.8f; // reduce the volume a bit
                if (sum_total > 1) sum_total = 1;
                if (sum_total < -1) sum_total = -1;
                result[i] = (Byte)(sum_total * 256);
            }
            return result;
        }



        public void StopAudio(Audio audio)
        {
            if (audio == null) return;
            audio.Cancel = true;
        }
        public void PlaySample(Sample sample)
        {
            this.Output.Write(Audio.ChangeSampleRate(
                sample.PCM_Data,
                sample.Pitch / 1024,
                Audio.SAMPLE_RATE));
        }
        public void PlayAudio(Byte instrument, Byte note)
        {
            if (this.Samples[instrument] == null) return;
            Audio audio = this.Samples[instrument][note];
            if (audio == null) return;
            audio.Cancel = false;
            audio.State = Audio.ADSR.Attack;
            audio.Position = 0;

            if (!this.Playing.Contains(audio))
            {
                this.Playing.Add(audio);
            }
            if (this.Player == null)
            {
                this.Player = new Thread(this.RunAudioThread);
                this.Player.Start();
            }
        }
        public void PlayTrack(Track track, ref Int32 i)
        {
            if (track.Data[i] < 0x80) // repeat command
            {

            }
            else if (track.Data[i] >= 0xD0) // timed note-on command
            {

            }
            else switch (track.Data[i])
            {
                case 0xB1: // End of track
                    if (i != track.Data.Length - 1)
                        throw new Exception("There shouldn't be any data after an end of track command (0xB1)");
                    break;
                case 0xB2: // Jump to address (4-byte arg)
                    break;
                case 0xB3: // Call subsection (4-byte arg)
                    break;
                case 0xB4: // End subsection
                    break;
                case 0xB5: // Call and repeat subsection. (1-byte and 4-byte args)
                    break;

                case 0xB9: // Conditional jump based on memory content (3 bytes..?)
                    break;
                case 0xBA: // Set track priority (1-byte arg)
                    break;
                case 0xBB: // Set tempo (1-byte arg)
                    break;
                case 0xBC: // Transpose (1 signed byte)
                    break;
                case 0xBD: // Set Instrument (1-byte arg)
                    break;
                case 0xBE: // Set Volume (1-byte arg)
                    break;
                case 0xBF: // Set Panning (1-byte arg)
                    break;
                case 0xC0: // Pitch bend value (1-byte arg)
                    break;
                case 0xC1: // Pitch bend semitones (1-byte arg)
                    break;
                case 0xC2: // LFO Speed (1-byte arg)
                    break;
                case 0xC3: // LFO Delay (1-byte arg)
                    break;
                case 0xC4: // LFO Depth (1-byte arg)
                    break;
                case 0xC5: // LFO Type (1-byte arg)
                    break;

                case 0xC8: // Detune (1-byte arg)
                    break;

                case 0xCD: // Echo (two 1-byte args)
                    break;

                case 0xCE: // Note Off (two optional args)
                    break;
                case 0xCF: // Note On (two optional args)
                    //this.PlayAudio(instruments.GetAudio(_Instrument, Data[++i], Data[++i]));
                    break;

                default: throw new Exception("Unsupported command: " + Util.ByteToHex(track.Data[i]));
            }
        }
    }
}
