//-----------------------------------------------------------------------
// <copyright file="WaveFormat.cs" company="(none)">
//  Copyright © 2010 John Gietzen
//
//  Permission is hereby granted, free of charge, to any person obtaining
//  a copy of this software and associated documentation files (the
//  "Software"), to deal in the Software without restriction, including
//  without limitation the rights to use, copy, modify, merge, publish,
//  distribute, sublicense, and/or sell copies of the Software, and to
//  permit persons to whom the Software is furnished to do so, subject to
//  the following conditions:
//
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
//  BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
//  ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//  CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WinMM
{
    /// <summary>
    /// Describes a specific wave format.
    /// </summary>
    public class WaveFormat
    {
        /// <summary>
        /// Holds the format of the wave samples.
        /// </summary>
        private WaveFormatTag formatTag;

        /// <summary>
        /// Holds the number of channels.
        /// </summary>
        private short channels;

        /// <summary>
        /// Holds the sampling frequency.
        /// </summary>
        private int samplesPerSecond;

        /// <summary>
        /// Holds the number of bits per sample.
        /// </summary>
        private short bitsPerSample;

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 44.100 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm44Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 44100;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 44.100 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm44Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 44100;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 44.100 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm44Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 44100;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 44.100 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm44Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 44100;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 32.000 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm32Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 32000;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 32.000 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm32Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 32000;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 32.000 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm32Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 32000;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 32.000 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm32Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 32000;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 24.000 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm24Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 24000;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 24.000 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm24Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 24000;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 24.000 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm24Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 24000;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 24.000 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm24Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 24000;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 22.050 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm22Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 22050;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 22.050 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm22Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 22050;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 22.050 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm22Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 22050;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 22.050 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm22Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 22050;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 16.000 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm16Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 16000;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 16.000 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm16Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 16000;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 16.000 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm16Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 16000;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 16.000 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm16Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 16000;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 12.000 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm12Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 12000;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 12.000 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm12Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 12000;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 12.000 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm12Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 12000;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 12.000 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm12Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 12000;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 11.025 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm11Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 11025;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 11.025 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm11Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 11025;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 11.025 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm11Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 11025;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 11.025 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm11Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 11025;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 8.000 kHz, 16 bit, stereo
        /// </summary>
        public static WaveFormat Pcm8Khz16BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 8000;
                wf.BitsPerSample = 16;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 8.000 kHz, 16 bit, monaural
        /// </summary>
        public static WaveFormat Pcm8Khz16BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 8000;
                wf.BitsPerSample = 16;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 8.000 kHz, 8 bit, stereo
        /// </summary>
        public static WaveFormat Pcm8Khz8BitStereo
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 8000;
                wf.BitsPerSample = 8;
                wf.Channels = 2;
                return wf;
            }
        }

        /// <summary>
        /// Gets the preset WaveFormat: PCM, 8.000 kHz, 8 bit, monaural
        /// </summary>
        public static WaveFormat Pcm8Khz8BitMono
        {
            get
            {
                WaveFormat wf = new WaveFormat();
                wf.FormatTag = WaveFormatTag.Pcm;
                wf.SamplesPerSecond = 8000;
                wf.BitsPerSample = 8;
                wf.Channels = 1;
                return wf;
            }
        }

        /// <summary>
        /// Gets or sets the format of the wave samples.
        /// </summary>
        public WaveFormatTag FormatTag
        {
            get
            {
                return this.formatTag;
            }

            set
            {
                this.formatTag = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of channels.
        /// </summary>
        public short Channels
        {
            get
            {
                return this.channels;
            }

            set
            {
                this.channels = value;
            }
        }

        /// <summary>
        /// Gets or sets the sampling frequency.
        /// </summary>
        public int SamplesPerSecond
        {
            get
            {
                return this.samplesPerSecond;
            }

            set
            {
                this.samplesPerSecond = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of bits per sample.
        /// </summary>
        public short BitsPerSample
        {
            get
            {
                return this.bitsPerSample;
            }

            set
            {
                this.bitsPerSample = value;
            }
        }

        /// <summary>
        /// Gets the smallest atomic unit of data, in bytes.
        /// </summary>
        public short BlockAlign
        {
            get
            {
                return (short)(this.Channels * this.BitsPerSample / 8);
            }
        }

        /// <summary>
        /// Gets the average number of bytes per second.
        /// </summary>
        public int AverageBytesPerSecond
        {
            get
            {
                return this.SamplesPerSecond * this.BlockAlign;
            }
        }

        /// <summary>
        /// Creates a duplicate copy of this object.
        /// </summary>
        /// <returns>The requested copy</returns>
        internal WaveFormat Clone()
        {
            WaveFormat clone = new WaveFormat();
            clone.bitsPerSample = this.bitsPerSample;
            clone.channels = this.channels;
            clone.formatTag = this.formatTag;
            clone.samplesPerSecond = this.samplesPerSecond;
            return clone;
        }
    }
}
