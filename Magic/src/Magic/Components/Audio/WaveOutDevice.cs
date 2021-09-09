//-----------------------------------------------------------------------
// <copyright file="WaveOutDeviceCaps.cs" company="(none)">
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
    /// Enumerates the capabilities of a WaveOut device.
    /// </summary>
    public class WaveOutDevice
    {
        /// <summary>
        /// Holds the capabilities of the device.
        /// </summary>
        private WAVECAPS capabilities;

        /// <summary>
        /// Gets the system specific identifier of the device.
        /// </summary>
        public System.Int32 DeviceId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the device's manufacturer name.
        /// </summary>
        public System.String Manufacturer
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the device's product id.
        /// </summary>
        public System.Int32 ProductId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the device's driver version.
        /// </summary>
        public System.Int32 DriverVersion
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public System.String Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of channels the device is capable of playing.
        /// </summary>
        public System.Int32 Channels
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether a device supports pitch modulation.
        /// </summary>
        public System.Boolean SupportsPitch
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_PITCH) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a device supports playback rate modification.
        /// </summary>
        public System.Boolean SupportsPlaybackRate
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_PLAYBACKRATE) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a device supports volume changing.
        /// </summary>
        public System.Boolean SupportsVolume
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_VOLUME) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a device supports stereo volume changing.
        /// </summary>
        public System.Boolean SupportsStereoVolume
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_LRVOLUME) != 0;
            }
        }

        /// <summary>
        /// Gets or sets the capabilities of the device.
        /// </summary>
        internal WAVECAPS Capabilities
        {
            get
            {
                return this.capabilities;
            }

            set
            {
                this.capabilities = value;
            }
        }
    }
}
