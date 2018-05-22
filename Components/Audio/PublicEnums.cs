//-----------------------------------------------------------------------
// <copyright file="PublicEnums.cs" company="(none)">
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
    using System;

    /// <summary>
    /// Current state of joystick buttons.
    /// </summary>
    [Flags]
    public enum JoystickButtons
    {
        /// <summary>
        /// Indicates that no buttons are pressed.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Joystick Button 1
        /// </summary>
        JoystickButton1 = 0x00000001,

        /// <summary>
        /// Joystick Button 2
        /// </summary>
        JoystickButton2 = 0x00000002,

        /// <summary>
        /// Joystick Button 3
        /// </summary>
        JoystickButton3 = 0x00000004,

        /// <summary>
        /// Joystick Button 4
        /// </summary>
        JoystickButton4 = 0x00000008,

        /// <summary>
        /// Joystick Button 5
        /// </summary>
        JoystickButton5 = 0x00000010,

        /// <summary>
        /// Joystick Button 6
        /// </summary>
        JoystickButton6 = 0x00000020,

        /// <summary>
        /// Joystick Button 7
        /// </summary>
        JoystickButton7 = 0x00000040,

        /// <summary>
        /// Joystick Button 8
        /// </summary>
        JoystickButton8 = 0x00000080,

        /// <summary>
        /// Joystick Button 9
        /// </summary>
        JoystickButton9 = 0x00000100,

        /// <summary>
        /// Joystick Button 10
        /// </summary>
        JoystickButton10 = 0x00000200,

        /// <summary>
        /// Joystick Button 11
        /// </summary>
        JoystickButton11 = 0x00000400,

        /// <summary>
        /// Joystick Button 12
        /// </summary>
        JoystickButton12 = 0x00000800,

        /// <summary>
        /// Joystick Button 13
        /// </summary>
        JoystickButton13 = 0x00001000,

        /// <summary>
        /// Joystick Button 14
        /// </summary>
        JoystickButton14 = 0x00002000,

        /// <summary>
        /// Joystick Button 15
        /// </summary>
        JoystickButton15 = 0x00004000,

        /// <summary>
        /// Joystick Button 16
        /// </summary>
        JoystickButton16 = 0x00008000,

        /// <summary>
        /// Joystick Button 17
        /// </summary>
        JoystickButton17 = 0x00010000,

        /// <summary>
        /// Joystick Button 18
        /// </summary>
        JoystickButton18 = 0x00020000,

        /// <summary>
        /// Joystick Button 19
        /// </summary>
        JoystickButton19 = 0x00040000,

        /// <summary>
        /// Joystick Button 20
        /// </summary>
        JoystickButton20 = 0x00080000,

        /// <summary>
        /// Joystick Button 21
        /// </summary>
        JoystickButton21 = 0x00100000,

        /// <summary>
        /// Joystick Button 22
        /// </summary>
        JoystickButton22 = 0x00200000,

        /// <summary>
        /// Joystick Button 23
        /// </summary>
        JoystickButton23 = 0x00400000,

        /// <summary>
        /// Joystick Button 24
        /// </summary>
        JoystickButton24 = 0x00800000,

        /// <summary>
        /// Joystick Button 25
        /// </summary>
        JoystickButton25 = 0x01000000,

        /// <summary>
        /// Joystick Button 26
        /// </summary>
        JoystickButton26 = 0x02000000,

        /// <summary>
        /// Joystick Button 27
        /// </summary>
        JoystickButton27 = 0x04000000,

        /// <summary>
        /// Joystick Button 28
        /// </summary>
        JoystickButton28 = 0x08000000,

        /// <summary>
        /// Joystick Button 29
        /// </summary>
        JoystickButton29 = 0x10000000,

        /// <summary>
        /// Joystick Button 30
        /// </summary>
        JoystickButton30 = 0x20000000,

        /// <summary>
        /// Joystick Button 31
        /// </summary>
        JoystickButton31 = 0x40000000,

        /// <summary>
        /// Joystick Button 32
        /// </summary>
        JoystickButton32 = unchecked((int)0x80000000),
    }

    /// <summary>
    /// Indicates a WaveOut message.
    /// </summary>
    public enum WaveOutMessage
    {
        /// <summary>
        /// Not Used.  Indicates that there is no message.
        /// </summary>
        None = 0x000,

        /// <summary>
        /// Indicates that the device has been opened.
        /// </summary>
        DeviceOpened = 0x3BB,

        /// <summary>
        /// Indicates that the device has been closed.
        /// </summary>
        DeviceClosed = 0x3BC,

        /// <summary>
        /// Indicates that playback of a write operation has been completed.
        /// </summary>
        WriteDone = 0x3BD
    }

    /// <summary>
    /// Indicates a WaveIn message.
    /// </summary>
    public enum WaveInMessage
    {
        /// <summary>
        /// Not Used.  Indicates that there is no message.
        /// </summary>
        None = 0x000,

        /// <summary>
        /// Indicates that the device has been opened.
        /// </summary>
        DeviceOpened = 0x3BE,

        /// <summary>
        /// Indicates that the device has been closed.
        /// </summary>
        DeviceClosed = 0x3BF,

        /// <summary>
        /// Indicates that playback of a write operation has been completed.
        /// </summary>
        DataReady = 0x3C0
    }

    /// <summary>
    /// Indicates a wave data sample format.
    /// </summary>
    public enum WaveFormatTag
    {
        /// <summary>
        /// Indicates an invalid sample format.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Indicates raw Pulse Code Modulation data.
        /// </summary>
        Pcm = 0x01,

        /// <summary>
        /// Indicates Adaptive Differential Pulse Code Modulation data.
        /// </summary>
        Adpcm = 0x02,

        /// <summary>
        /// Indicates IEEE-Float data.
        /// </summary>
        Float = 0x03,

        /// <summary>
        /// Indicates a-law companded data.
        /// </summary>
        ALaw = 0x06,

        /// <summary>
        /// Indicates μ-law  companded data.
        /// </summary>
        MuLaw = 0x07,
    }

    /// <summary>
    /// Describes a variety of channels, frequencies, and bit-depths by which a wave signal may be expressed.
    /// </summary>
    [Flags]
    public enum WaveFormats
    {
        /// <summary>
        /// Monaural, 8-bit, 11025 Hz
        /// </summary>
        Mono8Bit11Khz = 1,

        /// <summary>
        /// Stereo, 8-bit, 11025 Hz
        /// </summary>
        Stereo8Bit11Khz = 2,

        /// <summary>
        /// Monaural, 16-bit, 11025 Hz
        /// </summary>
        Mono16Bit11Khz = 4,

        /// <summary>
        /// Stereo, 16-bit, 11025 Hz
        /// </summary>
        Stereo16Bit11Khz = 8,

        /// <summary>
        /// Monaural, 8-bit, 22050 Hz
        /// </summary>
        Mono8Bit22Khz = 16,

        /// <summary>
        /// Stereo, 8-bit, 22050 Hz
        /// </summary>
        Stereo8Bit22Khz = 32,

        /// <summary>
        /// Monaural, 16-bit, 22050 Hz
        /// </summary>
        Mono16Bit22Khz = 64,

        /// <summary>
        /// Stereo, 16-bit, 22050 Hz
        /// </summary>
        Stereo16Bit22Khz = 128,

        /// <summary>
        /// Monaural, 8-bit, 44100 Hz
        /// </summary>
        Mono8Bit44Khz = 256,

        /// <summary>
        /// Stereo, 8-bit, 44100 Hz
        /// </summary>
        Stereo8Bit44Khz = 512,

        /// <summary>
        /// Monaural, 16-bit, 44100 Hz
        /// </summary>
        Mono16Bit44Khz = 1024,

        /// <summary>
        /// Stereo, 16-bit, 44100 Hz
        /// </summary>
        Stereo16Bit44Khz = 2048,

        /// <summary>
        /// Monaural, 8-bit, 48000 Hz
        /// </summary>
        Mono8Bit48Khz = 4096,

        /// <summary>
        /// Stereo, 8-bit, 48000 Hz
        /// </summary>
        Stereo8Bit48Khz = 8192,

        /// <summary>
        /// Monaural, 16-bit, 48000 Hz
        /// </summary>
        Mono16Bit48Khz = 16384,

        /// <summary>
        /// Stereo, 16-bit, 48000 Hz
        /// </summary>
        Stereo16Bit48Khz = 32768,

        /// <summary>
        /// Monaural, 8-bit, 96000 Hz
        /// </summary>
        Mono8Bit96Khz = 65536,

        /// <summary>
        /// Stereo, 8-bit, 96000 Hz
        /// </summary>
        Stereo8Bit96Khz = 131072,

        /// <summary>
        /// Monaural, 16-bit, 96000 Hz
        /// </summary>
        Mono16Bit96Khz = 262144,

        /// <summary>
        /// Stereo, 16-bit, 96000 Hz
        /// </summary>
        Stereo16Bit96Khz = 524288,
    }
}
