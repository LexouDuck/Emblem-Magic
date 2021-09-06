//-----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="(none)">
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
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// The waveOutProc function is the callback function used with the waveform-audio output device. The waveOutProc
    /// function is a placeholder for the application-defined function name. The address of this function can be specified
    /// in the callback-address parameter of the waveOutOpen function.
    /// </summary>
    /// <param name="hwo">Handle to the waveform-audio device associated with the callback.</param>
    /// <param name="uMsg">
    /// Waveform-audio output message. It can be one of the following values.
    /// <list type="Messages">
    /// <item>WOM_CLOSE (Sent when the device is closed using the waveOutClose function.)</item>
    /// <item>WOM_DONE (Sent when the device driver is finished with a data block sent using the waveOutWrite function.)</item>
    /// <item>WOM_OPEN (Sent when the device is opened using the waveOutOpen function.)</item>
    /// </list>
    /// </param>
    /// <param name="dwInstance">User-instance data specified with waveOutOpen.</param>
    /// <param name="dwParam1">Message parameter one.</param>
    /// <param name="dwParam2">Message parameter two.</param>
    /// <remarks>
    /// Applications should not call any system-defined functions from inside a callback function, except for
    /// <list type="Acceptable Calls">
    /// <item>EnterCriticalSection</item>
    /// <item>LeaveCriticalSection</item>
    /// <item>midiOutLongMsg</item>
    /// <item>midiOutShortMsg</item>
    /// <item>OutputDebugString</item>
    /// <item>PostMessage</item>
    /// <item>PostThreadMessage</item>
    /// <item>SetEvent</item>
    /// <item>timeGetSystemTime</item>
    /// <item>timeGetTime</item>
    /// <item>timeKillEvent</item>
    /// <item>timeSetEvent</item>
    /// </list>
    /// Calling other wave functions will cause deadlock.
    /// </remarks>
    public delegate void WaveOutProc(IntPtr hwo, WaveOutMessage uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

    /// <summary>
    /// The waveInProc function is the callback function used with the waveform-audio input device. This function is a placeholder for the application-defined function name. The address of this function can be specified in the callback-address parameter of the waveInOpen function.
    /// </summary>
    /// <param name="hwi">Handle to the waveform-audio device associated with the callback function.</param>
    /// <param name="uMsg">Waveform-audio input message.</param>
    /// <param name="dwInstance">User instance data specified with waveInOpen.</param>
    /// <param name="dwParam1">Message parameter one.</param>
    /// <param name="dwParam2">Message parameter two.</param>
    public delegate void WaveInProc(IntPtr hwi, WaveInMessage uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);



    /// <summary>
    /// The WAVEOUTCAPS structure describes the capabilities of a waveform-audio output device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WAVEOUTCAPS
    {
        /// <summary>
        /// Specifies the manufacturer id of the device.
        /// </summary>
        public Int16 wMid;
        /// <summary>
        /// Specifies the product id of the device.
        /// </summary>
        public Int16 wPid;
        /// <summary>
        /// Specifies the version of the device's driver.
        /// </summary>
        public Int32 vDriverVersion;
        /// <summary>
        /// Specifies the name of the device.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String szPname;
        /// <summary>
        /// Specifies the WAVE formats the device supports.
        /// </summary>
        public WaveFormats dwFormats;
        /// <summary>
        /// Specifies the number of channels the device supports.
        /// </summary>
        public Int16 wChannels;
        /// <summary>
        /// Unused.  Padding.
        /// </summary>
        public Int16 wReserved1;
        /// <summary>
        /// Specifies the features that the device supports.
        /// </summary>
        public WAVECAPS dwSupport;
    }

    /// <summary>
    /// The WAVEINCAPS structure describes the capabilities of a waveform-audio input device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WAVEINCAPS
    {
        /// <summary>
        /// The ManufacturerID.
        /// </summary>
        public Int16 wMid;
        /// <summary>
        /// The ProductID.
        /// </summary>
        public Int16 wPid;
        /// <summary>
        /// The device's driver version.
        /// </summary>
        public Int32 vDriverVersion;
        /// <summary>
        /// The name of the device.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String szPname;
        /// <summary>
        /// The formats the device supports.
        /// </summary>
        public Int32 dwFormats;
        /// <summary>
        /// The number of channels the device supports.
        /// </summary>
        public Int16 wChannels;
        /// <summary>
        /// Reserved for internal use.
        /// </summary>
        public Int16 wReserved1;
    }

    /// <summary>
    /// The WAVEHDR structure defines the header used to identify a waveform-audio buffer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WAVEHDR
    {
        /// <summary>
        /// Pointer to the waveform buffer.
        /// </summary>
        internal IntPtr lpData;
        /// <summary>
        /// Length, in bytes, of the buffer.
        /// </summary>
        internal Int32 dwBufferLength;
        /// <summary>
        /// When the header is used in input, this member specifies how much data is in the buffer.
        /// </summary>
        internal Int32 dwBytesRecorded;
        /// <summary>
        /// User data.
        /// </summary>
        internal IntPtr dwUser;
        /// <summary>
        /// Flags supplying information about the buffer. The following values are defined:
        /// </summary>
        internal WaveHeaderFlags dwFlags;
        /// <summary>
        /// Number of times to play the loop. This member is used only with output buffers.
        /// </summary>
        internal Int32 dwLoops;
        /// <summary>
        /// Reserved for internal use.
        /// </summary>
        internal IntPtr lpNext;
        /// <summary>
        /// Reserved for internal use.
        /// </summary>
        internal Int32 reserved;
    }

    /// <summary>
    /// Describes the full format of a wave formatted stream.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WAVEFORMATEX
    {
        /// <summary>
        /// The wave format of the stream.
        /// </summary>
        public Int16 wFormatTag;
        /// <summary>
        /// The number of channels.
        /// </summary>
        public Int16 nChannels;
        /// <summary>
        /// The number of samples per second.
        /// </summary>
        public Int32 nSamplesPerSec;
        /// <summary>
        /// The average bytes per second.
        /// </summary>
        public Int32 nAvgBytesPerSec;
        /// <summary>
        /// The smallest atomic data size.
        /// </summary>
        public Int16 nBlockAlign;
        /// <summary>
        /// The number of bits per sample.
        /// </summary>
        public Int16 wBitsPerSample;
        /// <summary>
        /// The remaining header size. (Must be zero in this struct format.)
        /// </summary>
        public Int16 cbSize;
    }

    /// <summary>
    /// Describes the full format of a wave formatted stream.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WAVEFORMATEXTENSIBLE
    {
        /// <summary>
        /// The wave format of the stream.
        /// </summary>
        public Int16 wFormatTag;
        /// <summary>
        /// The number of channels.
        /// </summary>
        public Int16 nChannels;
        /// <summary>
        /// The number of samples per second.
        /// </summary>
        public Int32 nSamplesPerSec;
        /// <summary>
        /// The average bytes per second.
        /// </summary>
        public Int32 nAvgBytesPerSec;
        /// <summary>
        /// The smallest atomic data size.
        /// </summary>
        public Int16 nBlockAlign;
        /// <summary>
        /// The number of bits per sample.
        /// </summary>
        public Int16 wBitsPerSample;
        /// <summary>
        /// The remaining header size.
        /// </summary>
        public Int16 cbSize;
        /// <summary>
        /// The number of valid bits per sample.
        /// </summary>
        public Int16 wValidBitsPerSample;
        /// <summary>
        /// The channel mask.
        /// </summary>
        public Int32 dwChannelMask;
        /// <summary>
        /// The sub format identifier.
        /// </summary>
        public Guid SubFormat;
    }

    /// <summary>
    /// The MMTIME structure contains timing information for different types of multimedia data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MMTIME
    {
        /// <summary>
        /// Time format.
        /// </summary>
        public Int32 wType;
        /// <summary>
        /// The first part of the data.
        /// </summary>
        public Int32 wData1;
        /// <summary>
        /// The second part of the data.
        /// </summary>
        public Int32 wData2;
    }

    /// <summary>
    /// Used with the <see cref="NativeMethods.waveOutOpen"/> command.
    /// </summary>
    [Flags]
    public enum WaveOpenFlags
    {
        /// <summary>
        /// No callback mechanism. This is the default setting.
        /// </summary>
        CALLBACK_NULL = 0x00000,
        /// <summary>
        /// Indicates the dwCallback parameter is a window handle.
        /// </summary>
        CALLBACK_WINDOW = 0x10000,
        /// <summary>
        /// The dwCallback parameter is a thread identifier.
        /// </summary>
        CALLBACK_THREAD = 0x20000,
        /// <summary>
        /// The dwCallback parameter is a thread identifier.
        /// </summary>
        [Obsolete]
        CALLBACK_TASK = 0x20000,
        /// <summary>
        /// The dwCallback parameter is a callback procedure address.
        /// </summary>
        CALLBACK_FUNCTION = 0x30000,
        /// <summary>
        /// If this flag is specified, <see cref="NativeMethods.waveOutOpen"/> queries the device to determine if it supports the given format, but the device is not actually opened.
        /// </summary>
        WAVE_FORMAT_QUERY = 0x00001,
        /// <summary>
        /// If this flag is specified, a synchronous waveform-audio device can be opened. If this flag is not specified while opening a synchronous driver, the device will fail to open.
        /// </summary>
        WAVE_ALLOWSYNC = 0x00002,
        /// <summary>
        /// If this flag is specified, the uDeviceID parameter specifies a waveform-audio device to be mapped to by the wave mapper.
        /// </summary>
        WAVE_MAPPED = 0x00004,
        /// <summary>
        /// If this flag is specified, the ACM driver does not perform conversions on the audio data.
        /// </summary>
        WAVE_FORMAT_DIRECT = 0x00008
    }

    /// <summary>
    /// Flags supplying information about the buffer. The following values are defined:
    /// </summary>
    [Flags]
    public enum WaveHeaderFlags
    {
        /// <summary>
        /// This buffer is the first buffer in a loop.  This flag is used only with output buffers.
        /// </summary>
        BeginLoop = 0x00000004,
        /// <summary>
        /// Set by the device driver to indicate that it is finished with the buffer and is returning it to the application.
        /// </summary>
        Done = 0x00000001,
        /// <summary>
        /// This buffer is the last buffer in a loop.  This flag is used only with output buffers.
        /// </summary>
        EndLoop = 0x00000008,
        /// <summary>
        /// Set by Windows to indicate that the buffer is queued for playback.
        /// </summary>
        InQueue = 0x00000010,
        /// <summary>
        /// Set by Windows to indicate that the buffer has been prepared with the waveInPrepareHeader or waveOutPrepareHeader function.
        /// </summary>
        Prepared = 0x00000002
    }

    /// <summary>
    /// Used as a return result from many of the WinMM calls.
    /// </summary>
    public enum MMSYSERROR
    {
        /// <summary>
        /// No Error. (Success)
        /// </summary>
        MMSYSERR_NOERROR = 0,
        /// <summary>
        /// Unspecified Error.
        /// </summary>
        MMSYSERR_ERROR = 1,
        /// <summary>
        /// Device ID out of range.
        /// </summary>
        MMSYSERR_BADDEVICEID = 2,
        /// <summary>
        /// Driver failed enable.
        /// </summary>
        MMSYSERR_NOTENABLED = 3,
        /// <summary>
        /// Device is already allocated.
        /// </summary>
        MMSYSERR_ALLOCATED = 4,
        /// <summary>
        /// Device handle is invalid.
        /// </summary>
        MMSYSERR_INVALHANDLE = 5,
        /// <summary>
        /// No device driver is present.
        /// </summary>
        MMSYSERR_NODRIVER = 6,
        /// <summary>
        /// In sufficient memory, or memory allocation error.
        /// </summary>
        MMSYSERR_NOMEM = 7,
        /// <summary>
        /// Unsupported function.
        /// </summary>
        MMSYSERR_NOTSUPPORTED = 8,
        /// <summary>
        /// Error value out of range.
        /// </summary>
        MMSYSERR_BADERRNUM = 9,
        /// <summary>
        /// Invalid flag passed.
        /// </summary>
        MMSYSERR_INVALFLAG = 10,
        /// <summary>
        /// Invalid parameter passed.
        /// </summary>
        MMSYSERR_INVALPARAM = 11,
        /// <summary>
        /// Handle being used simultaneously on another thread.
        /// </summary>
        MMSYSERR_HANDLEBUSY = 12,
        /// <summary>
        /// Specified alias not found.
        /// </summary>
        MMSYSERR_INVALIDALIAS = 13,
        /// <summary>
        /// Bad registry database.
        /// </summary>
        MMSYSERR_BADDB = 14,
        /// <summary>
        /// Registry key not found.
        /// </summary>
        MMSYSERR_KEYNOTFOUND = 15,
        /// <summary>
        /// Registry read error.
        /// </summary>
        MMSYSERR_READERROR = 16,
        /// <summary>
        /// Registry write error.
        /// </summary>
        MMSYSERR_WRITEERROR = 17,
        /// <summary>
        /// Registry delete error.
        /// </summary>
        MMSYSERR_DELETEERROR = 18,
        /// <summary>
        /// Registry value not found.
        /// </summary>
        MMSYSERR_VALNOTFOUND = 19,
        /// <summary>
        /// Driver does not call DriverCallback.
        /// </summary>
        MMSYSERR_NODRIVERCB = 20,
        /// <summary>
        /// More data to be returned.
        /// </summary>
        MMSYSERR_MOREDATA = 21,
        /// <summary>
        /// Unsupported wave format.
        /// </summary>
        WAVERR_BADFORMAT = 32,
        /// <summary>
        /// Still something playing.
        /// </summary>
        WAVERR_STILLPLAYING = 33,
        /// <summary>
        /// Header not prepared.
        /// </summary>
        WAVERR_UNPREPARED = 34,
        /// <summary>
        /// Device is syncronus.
        /// </summary>
        WAVERR_SYNC = 35,
        /// <summary>
        /// Header not prepared.
        /// </summary>
        MIDIERR_UNPREPARED = 64,
        /// <summary>
        /// Still something playing.
        /// </summary>
        MIDIERR_STILLPLAYING = 65,
        /// <summary>
        /// No configured instruments.
        /// </summary>
        MIDIERR_NOMAP = 66,
        /// <summary>
        /// Hardware is still busy.
        /// </summary>
        MIDIERR_NOTREADY = 67,
        /// <summary>
        /// Port no longer connected
        /// </summary>
        MIDIERR_NODEVICE = 68,
        /// <summary>
        /// Invalid MIF
        /// </summary>
        MIDIERR_INVALIDSETUP = 69,
        /// <summary>
        /// Operation unsupported with open mode.
        /// </summary>
        MIDIERR_BADOPENMODE = 70,
        /// <summary>
        /// Thru device 'eating' a message
        /// </summary>
        MIDIERR_DONT_CONTINUE = 71,
        /// <summary>
        /// The resolution specified in uPeriod is out of range.
        /// </summary>
        TIMERR_NOCANDO = 96 + 1,
        /// <summary>
        /// Time struct size
        /// </summary>
        TIMERR_STRUCT = 96 + 33,
        /// <summary>
        /// Bad parameters
        /// </summary>
        JOYERR_PARMS = 160 + 5,
        /// <summary>
        /// Request not completed
        /// </summary>
        JOYERR_NOCANDO = 160 + 6,
        /// <summary>
        /// Joystick is unplugged
        /// </summary>
        JOYERR_UNPLUGGED = 160 + 7,
        /// <summary>
        /// Invalid device ID
        /// </summary>
        MCIERR_INVALID_DEVICE_ID = 256 + 1,
        /// <summary>
        /// Unrecognized keyword
        /// </summary>
        MCIERR_UNRECOGNIZED_KEYWORD = 256 + 3,
        /// <summary>
        /// Unrecognized command
        /// </summary>
        MCIERR_UNRECOGNIZED_COMMAND = 256 + 5,
        /// <summary>
        /// Hardware error
        /// </summary>
        MCIERR_HARDWARE = 256 + 6,
        /// <summary>
        /// Invalid device name
        /// </summary>
        MCIERR_INVALID_DEVICE_NAME = 256 + 7,
        /// <summary>
        /// Out of memory
        /// </summary>
        MCIERR_OUT_OF_MEMORY = 256 + 8,
        /// <summary>
        /// Device loading error
        /// </summary>
        MCIERR_DEVICE_OPEN = 256 + 9,
        /// <summary>
        /// Driver loading error
        /// </summary>
        MCIERR_CANNOT_LOAD_DRIVER = 256 + 10,
        /// <summary>
        /// Missing command string
        /// </summary>
        MCIERR_MISSING_COMMAND_STRING = 256 + 11,
        /// <summary>
        /// Parameter overflow
        /// </summary>
        MCIERR_PARAM_OVERFLOW = 256 + 12,
        /// <summary>
        /// Missing string argument
        /// </summary>
        MCIERR_MISSING_STRING_ARGUMENT = 256 + 13,
        /// <summary>
        /// Bad integer
        /// </summary>
        MCIERR_BAD_INTEGER = 256 + 14,
        /// <summary>
        /// Parser internal error
        /// </summary>
        MCIERR_PARSER_INTERNAL = 256 + 15,
        /// <summary>
        /// Driver internal error
        /// </summary>
        MCIERR_DRIVER_INTERNAL = 256 + 16,
        /// <summary>
        /// Missing parameter
        /// </summary>
        MCIERR_MISSING_PARAMETER = 256 + 17,
        /// <summary>
        /// Unsupported function
        /// </summary>
        MCIERR_UNSUPPORTED_FUNCTION = 256 + 18,
        /// <summary>
        /// File not found
        /// </summary>
        MCIERR_FILE_NOT_FOUND = 256 + 19,
        /// <summary>
        /// Device not ready
        /// </summary>
        MCIERR_DEVICE_NOT_READY = 256 + 20,
        MCIERR_INTERNAL = 256 + 21,
        MCIERR_DRIVER = 256 + 22,
        MCIERR_CANNOT_USE_ALL = 256 + 23,
        MCIERR_MULTIPLE = 256 + 24,
        MCIERR_EXTENSION_NOT_FOUND = 256 + 25,
        MCIERR_OUTOFRANGE = 256 + 26,
        MCIERR_FLAGS_NOT_COMPATIBLE = 256 + 28,
        MCIERR_FILE_NOT_SAVED = 256 + 30,
        MCIERR_DEVICE_TYPE_REQUIRED = 256 + 31,
        MCIERR_DEVICE_LOCKED = 256 + 32,
        MCIERR_DUPLICATE_ALIAS = 256 + 33,
        MCIERR_BAD_CONSTANT = 256 + 34,
        MCIERR_MUST_USE_SHAREABLE = 256 + 35,
        MCIERR_MISSING_DEVICE_NAME = 256 + 36,
        MCIERR_BAD_TIME_FORMAT = 256 + 37,
        MCIERR_NO_CLOSING_QUOTE = 256 + 38,
        MCIERR_DUPLICATE_FLAGS = 256 + 39,
        MCIERR_INVALID_FILE = 256 + 40,
        MCIERR_NULL_PARAMETER_BLOCK = 256 + 41,
        MCIERR_UNNAMED_RESOURCE = 256 + 42,
        MCIERR_NEW_REQUIRES_ALIAS = 256 + 43,
        MCIERR_NOTIFY_ON_AUTO_OPEN = 256 + 44,
        MCIERR_NO_ELEMENT_ALLOWED = 256 + 45,
        MCIERR_NONAPPLICABLE_FUNCTION = 256 + 46,
        MCIERR_ILLEGAL_FOR_AUTO_OPEN = 256 + 47,
        MCIERR_FILENAME_REQUIRED = 256 + 48,
        MCIERR_EXTRA_CHARACTERS = 256 + 49,
        MCIERR_DEVICE_NOT_INSTALLED = 256 + 50,
        MCIERR_GET_CD = 256 + 51,
        MCIERR_SET_CD = 256 + 52,
        MCIERR_SET_DRIVE = 256 + 53,
        MCIERR_DEVICE_LENGTH = 256 + 54,
        MCIERR_DEVICE_ORD_LENGTH = 256 + 55,
        MCIERR_NO_INTEGER = 256 + 56,
        MCIERR_WAVE_OUTPUTSINUSE = 256 + 64,
        MCIERR_WAVE_SETOUTPUTINUSE = 256 + 65,
        MCIERR_WAVE_INPUTSINUSE = 256 + 66,
        MCIERR_WAVE_SETINPUTINUSE = 256 + 67,
        MCIERR_WAVE_OUTPUTUNSPECIFIED = 256 + 68,
        MCIERR_WAVE_INPUTUNSPECIFIED = 256 + 69,
        MCIERR_WAVE_OUTPUTSUNSUITABLE = 256 + 70,
        MCIERR_WAVE_SETOUTPUTUNSUITABLE = 256 + 71,
        MCIERR_WAVE_INPUTSUNSUITABLE = 256 + 72,
        MCIERR_WAVE_SETINPUTUNSUITABLE = 256 + 73,
        MCIERR_SEQ_DIV_INCOMPATIBLE = 256 + 80,
        MCIERR_SEQ_PORT_INUSE = 256 + 81,
        MCIERR_SEQ_PORT_NONEXISTENT = 256 + 82,
        MCIERR_SEQ_PORT_MAPNODEVICE = 256 + 83,
        MCIERR_SEQ_PORT_MISCERROR = 256 + 84,
        MCIERR_SEQ_TIMER = 256 + 85,
        MCIERR_SEQ_PORTUNSPECIFIED = 256 + 86,
        MCIERR_SEQ_NOMIDIPRESENT = 256 + 87,
        MCIERR_NO_WINDOW = 256 + 90,
        MCIERR_CREATEWINDOW = 256 + 91,
        MCIERR_FILE_READ = 256 + 92,
        MCIERR_FILE_WRITE = 256 + 93,
        MCIERR_NO_IDENTITY = 256 + 94,
        MIXERR_INVALLINE = 1024 + 0,
        MIXERR_INVALCONTROL = 1024 + 1,
        MIXERR_INVALVALUE = 1024 + 2,
        MIXERR_LASTERROR = 1024 + 2,
    }

    /// <summary>
    /// Specifies capabilities of a waveOut device.
    /// </summary>
    [Flags]
    public enum WAVECAPS
    {
        /// <summary>
        /// The device can change playback pitch.
        /// </summary>
        WAVECAPS_PITCH = 0x01,
        /// <summary>
        /// The device can change the playback rate.
        /// </summary>
        WAVECAPS_PLAYBACKRATE = 0x02,
        /// <summary>
        /// The device can change the volume.
        /// </summary>
        WAVECAPS_VOLUME = 0x04,
        /// <summary>
        /// The device can change the stereo volume.
        /// </summary>
        WAVECAPS_LRVOLUME = 0x08,
        /// <summary>
        /// The device is synchronus.
        /// </summary>
        WAVECAPS_SYNC = 0x10,
        /// <summary>
        /// The device supports sample accurate.
        /// </summary>
        WAVECAPS_SAMPLEACCURATE = 0x20,
        /// <summary>
        /// The device supports direct sound writing.
        /// </summary>
        WAVECAPS_DIRECTSOUND = 0x40,
    }

    /// <summary>
    /// Flags used with the PlaySound and sndPlaySound functions.
    /// </summary>
    [Flags]
    public enum PLAYSOUNDFLAGS
    {
        /// <summary>
        /// The sound is played synchronously and the function does not return until the sound ends. 
        /// </summary>
        SND_SYNC = 0x0000,
        /// <summary>
        /// The sound is played asynchronously and the function returns immediately after beginning the sound. To terminate an asynchronously played sound, call sndPlaySound with lpszSoundName set to NULL.
        /// </summary>
        SND_ASYNC = 0x0001,
        /// <summary>
        /// If the sound cannot be found, the function returns silently without playing the default sound.
        /// </summary>
        SND_NODEFAULT = 0x0002,
        /// <summary>
        /// The parameter specified by lpszSoundName points to an image of a waveform sound in memory.
        /// </summary>
        SND_MEMORY = 0x0004,
        /// <summary>
        /// The sound plays repeatedly until sndPlaySound is called again with the lpszSoundName parameter set to NULL. You must also specify the SND_ASYNC flag to loop sounds.
        /// </summary>
        SND_LOOP = 0x0008,
        /// <summary>
        /// If a sound is currently playing, the function immediately returns FALSE, without playing the requested sound.
        /// </summary>
        SND_NOSTOP = 0x0010,
        /// <summary>
        /// Sounds are to be stopped for the calling task. If pszSound is not NULL, all instances of the specified sound are stopped. If pszSound is NULL, all sounds that are playing on behalf of the calling task are stopped.  You must also specify the instance handle to stop SND_RESOURCE events.
        /// </summary>
        SND_PURGE = 0x0040,
        /// <summary>
        /// The sound is played using an application-specific association.
        /// </summary>
        SND_APPLICATION = 0x0080,
        /// <summary>
        /// If the driver is busy, return immediately without playing the sound.
        /// </summary>
        SND_NOWAIT = 0x00002000,
        /// <summary>
        /// The pszSound parameter is a system-event alias in the registry or the WIN.INI file. Do not use with either SND_FILENAME or SND_RESOURCE.
        /// </summary>
        SND_ALIAS = 0x00010000,
        /// <summary>
        /// The pszSound parameter is a filename.
        /// </summary>
        SND_FILENAME = 0x00020000,
        /// <summary>
        /// The pszSound parameter is a resource identifier; hmod must identify the instance that contains the resource.
        /// </summary>
        SND_RESOURCE = 0x00040004,
        /// <summary>
        /// The pszSound parameter is a predefined sound identifier.
        /// </summary>
        SND_ALIAS_ID = 0x00110000,
    }

    /// <summary>
    /// Provides a wrapping class for the winmm.dll 'PlaySound' functions.  This class cannot be inherited.
    /// </summary>
    public class NativeMethods
    {
        /// <summary>
        /// Prevents a default instance of the NativeMethods class from being created.
        /// </summary>
        private NativeMethods()
        {
        }

        /// <summary>
        /// Indicates the type of function that threw an error, for use in error lookups.
        /// </summary>
        public enum ErrorSource
        {
            /// <summary>
            /// Indicated that the error comes from Joystick.
            /// </summary>
            Joystick,
            /// <summary>
            /// Indicated that the error comes from WaveIn.
            /// </summary>
            WaveIn,
            /// <summary>
            /// Indicated that the error comes from WaveOut.
            /// </summary>
            WaveOut,
        }
        


        /// <summary>
        /// A string that specifies the sound to play. The maximum length, including the null terminator, is 256 characters. If this parameter is NULL, any currently playing waveform sound is stopped. To stop a non-waveform sound, specify SND_PURGE in the fdwSound parameter.
        /// Three flags in fdwSound (SND_ALIAS, SND_FILENAME, and SND_RESOURCE) determine whether the name is interpreted as an alias for a system event, a filename, or a resource identifier. If none of these flags are specified, PlaySound searches the registry or the WIN.INI file for an association with the specified sound name. If an association is found, the sound event is played. If no association is found in the registry, the name is interpreted as a filename.
        /// </summary>
        /// <param name="lpszSound"><para>A string that specifies the sound to play. The maximum length, including the null terminator, is 256 characters. If this parameter is NULL, any currently playing waveform sound is stopped. To stop a non-waveform sound, specify SND_PURGE in the fdwSound parameter.</para>Three flags in fdwSound (SND_ALIAS, SND_FILENAME, and SND_RESOURCE) determine whether the name is interpreted as an alias for a system event, a filename, or a resource identifier. If none of these flags are specified, PlaySound searches the registry or the WIN.INI file for an association with the specified sound name. If an association is found, the sound event is played. If no association is found in the registry, the name is interpreted as a filename.</param>
        /// <param name="hmod">Handle to the executable file that contains the resource to be loaded. This parameter must be NULL unless SND_RESOURCE is specified in fdwSound.</param>
        /// <param name="fuSound">Flags for playing the sound.</param>
        /// <returns>Returns TRUE if successful or FALSE otherwise.</returns>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern Int32 PlaySound(String lpszSound, IntPtr hmod, PLAYSOUNDFLAGS fuSound);
        /// <summary>
        /// The sndPlaySound function plays a waveform sound specified either by a filename or by an entry in the registry or the WIN.INI file. This function offers a subset of the functionality of the PlaySound function; sndPlaySound is being maintained for backward compatibility.
        /// </summary>
        /// <param name="lpszSound">A string that specifies the sound to play. This parameter can be either an entry in the registry or in WIN.INI that identifies a system sound, or it can be the name of a waveform-audio file. (If the function does not find the entry, the parameter is treated as a filename.) If this parameter is NULL, any currently playing sound is stopped.</param>
        /// <param name="fuSound">Flags for playing the sound.</param>
        /// <returns>Returns TRUE if successful or FALSE otherwise.</returns>
        /// <remarks>
        /// If the specified sound cannot be found, sndPlaySound plays the system default sound. If there is no system default entry in the registry or WIN.INI file, or if the default sound cannot be found, the function makes no sound and returns FALSE.
        /// The specified sound must fit in available physical memory and be playable by an installed waveform-audio device driver. If sndPlaySound does not find the sound in the current directory, the function searches for it using the standard directory-search order.
        /// </remarks>
        [Obsolete, DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern Int32 sndPlaySound(IntPtr lpszSound, PLAYSOUNDFLAGS fuSound);

        /// <summary>
        /// Takes an <see cref="MMSYSERROR"/> code and, if it is indeed an error, throws a corresponding exception.
        /// </summary>
        /// <param name="error">The error code to check.</param>
        /// <param name="source">The source of the error. Used in error text lookup.</param>
        public static void Throw(MMSYSERROR error, ErrorSource source)
        {
            if (error == MMSYSERROR.MMSYSERR_NOERROR)
            {
                return;
            }

            StringBuilder detailsBuilder = new StringBuilder(255);
            String details = String.Empty;

            MMSYSERROR pullInfoError = MMSYSERROR.MMSYSERR_ERROR;

            switch (source)
            {
                case ErrorSource.WaveIn:
                    // pullInfoError = NativeMethods.waveInGetErrorText(error, detailsBuilder, detailsBuilder.Capacity + 1);
                    break;
                case ErrorSource.WaveOut:
                    pullInfoError = NativeMethods.waveOutGetErrorText(error, detailsBuilder, detailsBuilder.Capacity + 1);
                    break;
            }

            if (pullInfoError != MMSYSERROR.MMSYSERR_NOERROR)
            {
                details = error.ToString() + " (" + ((Int32)error).ToString(CultureInfo.CurrentCulture) + ")";
            }
            else
            {
                details = detailsBuilder.ToString() + " (" + error.ToString() + ")";
            }

            throw new Exception(details.ToString());
        }
        


        /// <summary>
        /// The waveOutBreakLoop function breaks a loop on the given waveform-audio output device and allows playback to continue with the next block in the driver list.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// </list>
        /// </returns>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutBreakLoop(WaveOutSafeHandle hwo);
        /// <summary>
        /// The waveOutClose function closes the given waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device. If the function succeeds, the handle is no longer valid after this call.</param>
        /// <returns>Returns MMSYSERR_NOERROR if successful or an error otherwise.</returns>
        /// <remarks>
        /// If the device is still playing a waveform-audio file, the close operation fails. Use the waveOutReset function to terminate playback before calling waveOutClose.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutClose(WaveOutSafeHandle hwo);
        /// <summary>
        /// The waveOutGetDevCaps function retrieves the capabilities of a given waveform-audio output device.
        /// </summary>
        /// <param name="uDeviceID">Identifier of the waveform-audio output device. It can be either a device identifier or a handle of an open waveform-audio output device.</param>
        /// <param name="pwoc">Pointer to a <see cref="WAVEOUTCAPS"/> structure to be filled with information about the capabilities of the device.</param>
        /// <param name="cbwoc">Size, in bytes, of the <see cref="WAVEOUTCAPS"/> structure.</param>
        /// <returns>Returns MMSYSERR_NOERROR if successful or an error otherwise.</returns>
        /// <remarks>
        /// Use the <see cref="waveOutGetNumDevs"/> function to determine the number of waveform-audio output devices present
        /// in the system. If the value specified by the uDeviceID parameter is a device identifier, it can vary from zero to
        /// one less than the number of devices present. The WAVE_MAPPER constant can also be used as a device identifier.
        /// Only cbwoc bytes (or less) of information is copied to the location pointed to by pwoc. If cbwoc is zero, nothing
        /// is copied and the function returns zero.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetDevCaps(IntPtr uDeviceID, ref WAVEOUTCAPS pwoc, Int32 cbwoc);
        /// <summary>
        /// The waveOutGetErrorText function retrieves a textual description of the error identified by the given error number.
        /// </summary>
        /// <param name="mmrError">Error number.</param>
        /// <param name="pszText">Pointer to a buffer to be filled with the textual error description.</param>
        /// <param name="cchText">Size, in characters, of the buffer pointed to by <paramref name="pszText"/>.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_BADERRNUM if specified error number is out of range.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// If the textual error description is longer than the specified buffer, the description is truncated. The returned
        /// error string is always null-terminated. If cchText is zero, nothing is copied and the function returns zero.
        /// All error descriptions are less than MAXERRORLENGTH characters long.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetErrorText(MMSYSERROR mmrError, StringBuilder pszText, Int32 cchText);
        /// <summary>
        /// The waveOutGetID function retrieves the device identifier for the given waveform-audio output device.
        /// This function is supported for backward compatibility. New applications can cast a handle of the device
        /// rather than retrieving the device identifier.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="puDeviceID">Pointer to a variable to be filled with the device identifier.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// </list>
        /// </returns>
        [Obsolete, DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetID(WaveOutSafeHandle hwo, ref Int32 puDeviceID);
        /// <summary>
        /// The waveOutGetNumDevs function retrieves the number of waveform-audio output devices present in the system.
        /// </summary>
        /// <returns>
        /// Returns the number of devices. A return value of zero means that no devices are present or that an error occurred.
        /// </returns>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern Int32 waveOutGetNumDevs();
        /// <summary>
        /// The waveOutGetPitch function retrieves the current pitch setting for the specified waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="pdwPitch">
        /// Pointer to a variable to be filled with the current pitch multiplier setting.
        /// The pitch multiplier indicates the current change in pitch from the original authored setting.
        /// The pitch multiplier must be a positive value.  The pitch multiplier is specified as a fixed-point value.
        /// The high-order word of the variable contains the signed integer part of the number, and the low-order word
        /// contains the fractional part. A value of 0x8000 in the low-order word represents one-half, and 0x4000 represents
        /// one-quarter. For example, the value 0x00010000 specifies a multiplier of 1.0 (no pitch change), and a value
        /// of 0x000F8000 specifies a multiplier of 15.5.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Changing the pitch does not change the playback rate, sample rate, or playback time. Not all devices support
        /// pitch changes. To determine whether the device supports pitch control, use the WAVECAPS_PITCH flag to test
        /// the dwSupport member of the WAVEOUTCAPS structure (filled by the waveOutGetDevCaps function).
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetPitch(WaveOutSafeHandle hwo, ref Int32 pdwPitch);
        /// <summary>
        /// The waveOutGetPlaybackRate function retrieves the current playback rate for the specified waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="pdwRate">
        /// Pointer to a variable to be filled with the current playback rate. The playback rate setting is a multiplier indicating
        /// the current change in playback rate from the original authored setting. The playback rate multiplier must be a positive
        /// value. The rate is specified as a fixed-point value. The high-order word of the variable contains the signed integer
        /// part of the number, and the low-order word contains the fractional part. A value of 0x8000 in the low-order word
        /// represents one-half, and 0x4000 represents one-quarter. For example, the value 0x00010000 specifies a multiplier of 1.0
        /// (no playback rate change), and a value of 0x000F8000 specifies a multiplier of 15.5.
        /// </param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Changing the playback rate does not change the sample rate but does change the playback time. Not all devices support
        /// playback rate changes. To determine whether a device supports playback rate changes, use the WAVECAPS_PLAYBACKRATE flag
        /// to test the dwSupport member of the WAVEOUTCAPS structure (filled by the waveOutGetDevCaps function).
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetPlaybackRate(WaveOutSafeHandle hwo, ref Int32 pdwRate);
        /// <summary>
        /// The waveOutGetPosition function retrieves the current playback position of the given waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="pmmt">Pointer to an <see cref="MMTIME"/> structure.</param>
        /// <param name="cbmmt">Size, in bytes, of the <see cref="MMTIME"/> structure.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Before calling this function, set the wType member of the MMTIME structure to indicate the time format you want.
        /// After calling this function, check wType to determine whether the time format is supported. If the format is not
        /// supported, wType will specify an alternative format.  The position is set to zero when the device is opened or reset.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetPosition(WaveOutSafeHandle hwo, ref MMTIME pmmt, Int32 cbmmt);
        /// <summary>
        /// The waveOutGetVolume function retrieves the current volume level of the specified waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to an open waveform-audio output device. This parameter can also be a device identifier.</param>
        /// <param name="pdwVolume">
        /// Pointer to a variable to be filled with the current volume setting. The low-order word of this location contains the
        /// left-channel volume setting, and the high-order word contains the right-channel setting. A value of 0xFFFF represents
        /// full volume, and a value of 0x0000 is silence.
        /// If a device does not support both left and right volume control, the low-order word of the specified location
        /// contains the mono volume level.
        /// The full 16-bit setting(s) set with the waveOutSetVolume function is returned, regardless of whether the device
        /// supports the full 16 bits of volume-level control.
        /// </param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// If a device identifier is used, then the result of the waveOutGetVolume call and the information returned in pdwVolume
        /// applies to all instances of the device. If a device handle is used, then the result and information returned applies
        /// only to the instance of the device referenced by the device handle. Not all devices support volume changes. To determine
        /// whether the device supports volume control, use the WAVECAPS_VOLUME flag to test the dwSupport member of the WAVEOUTCAPS
        /// structure (filled by the waveOutGetDevCaps function). To determine whether the device supports left- and right-channel
        /// volume control, use the WAVECAPS_LRVOLUME flag to test the dwSupport member of the WAVEOUTCAPS structure (filled by
        /// waveOutGetDevCaps). Volume settings are interpreted logarithmically. This means the perceived increase in volume is the
        /// same when increasing the volume level from 0x5000 to 0x6000 as it is from 0x4000 to 0x5000.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetVolume(WaveOutSafeHandle hwo, ref Int32 pdwVolume);
        /// <summary>
        /// The waveOutGetVolume function retrieves the current volume level of the specified waveform-audio output device.
        /// </summary>
        /// <param name="uDeviceID">Handle to an open waveform-audio output device. This parameter can also be a device identifier.</param>
        /// <param name="pdwVolume">
        /// Pointer to a variable to be filled with the current volume setting. The low-order word of this location contains the
        /// left-channel volume setting, and the high-order word contains the right-channel setting. A value of 0xFFFF represents
        /// full volume, and a value of 0x0000 is silence.
        /// If a device does not support both left and right volume control, the low-order word of the specified location
        /// contains the mono volume level.
        /// The full 16-bit setting(s) set with the waveOutSetVolume function is returned, regardless of whether the device
        /// supports the full 16 bits of volume-level control.
        /// </param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// If a device identifier is used, then the result of the waveOutGetVolume call and the information returned in pdwVolume
        /// applies to all instances of the device. If a device handle is used, then the result and information returned applies
        /// only to the instance of the device referenced by the device handle. Not all devices support volume changes. To determine
        /// whether the device supports volume control, use the WAVECAPS_VOLUME flag to test the dwSupport member of the WAVEOUTCAPS
        /// structure (filled by the waveOutGetDevCaps function). To determine whether the device supports left- and right-channel
        /// volume control, use the WAVECAPS_LRVOLUME flag to test the dwSupport member of the WAVEOUTCAPS structure (filled by
        /// waveOutGetDevCaps). Volume settings are interpreted logarithmically. This means the perceived increase in volume is the
        /// same when increasing the volume level from 0x5000 to 0x6000 as it is from 0x4000 to 0x5000.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutGetVolume(IntPtr uDeviceID, ref Int32 pdwVolume);
        /// <summary>
        /// The waveOutMessage function sends messages to the waveform-audio output device drivers.
        /// </summary>
        /// <param name="deviceID">
        /// Identifier of the waveform device that receives the message. You must cast the device ID to the HWAVEOUT handle type.
        /// If you supply a handle instead of a device ID, the function fails and returns the MMSYSERR_NOSUPPORT error code.
        /// </param>
        /// <param name="uMsg">Message to send.</param>
        /// <param name="dwParam1">Message parameter one.</param>
        /// <param name="dwParam2">Message parameter two.</param>
        /// <returns>Returns the value returned from the driver.</returns>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern Int32 waveOutMessage(IntPtr deviceID, Int32 uMsg, ref Int32 dwParam1, ref Int32 dwParam2);
        /// <summary>
        /// The waveOutOpen function opens the given waveform-audio output device for playback.
        /// </summary>
        /// <param name="phwo">Pointer to a buffer that receives a handle identifying the open waveform-audio output device. Use the handle to identify the device when calling other waveform-audio output functions. This parameter might be NULL if the WAVE_FORMAT_QUERY flag is specified for fdwOpen.</param>
        /// <param name="uDeviceID">
        /// Identifier of the waveform-audio output device to open. It can be either a device identifier or a handle of an open waveform-audio input device. You can use the following flag instead of a device identifier.
        /// <list type="OptionalFlag">
        /// <item>WAVE_MAPPER (The function selects a waveform-audio output device capable of playing the given format.)</item>
        /// </list>
        /// </param>
        /// <param name="pwfx">Pointer to a <see cref="WAVEFORMATEX"/> structure that identifies the format of the waveform-audio data to be sent to the device. You can free this structure immediately after passing it to <see cref="waveOutOpen"/>.</param>
        /// <param name="dwCallback">Pointer to a fixed callback function, an event handle, a handle to a window, or the identifier of a thread to be called during waveform-audio playback to process messages related to the progress of the playback. If no callback function is required, this value can be zero. For more information on the callback function, see <see cref="waveOutProc"/>.</param>
        /// <param name="dwCallbackInstance">User-instance data passed to the callback mechanism. This parameter is not used with the window callback mechanism.</param>
        /// <param name="dwFlags">Flags for opening the device.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_ALLOCATED if the specified resource is already allocated.</item>
        /// <item>MMSYSERR_BADDEVICEID if the specified device identifier is out of range.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>WAVERR_BADFORMAT if you attempted to open with an unsupported waveform-audio format.</item>
        /// <item>WAVERR_SYNC if the device is synchronous but waveOutOpen was called without using the WAVE_ALLOWSYNC flag.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Use the waveOutGetNumDevs function to determine the number of waveform-audio output devices present in the system. If the value specified by the uDeviceID parameter is a device identifier, it can vary from zero to one less than the number of devices present. The WAVE_MAPPER constant can also be used as a device identifier.
        /// The structure pointed to by pwfx can be extended to include type-specific information for certain data formats. For example, for PCM data, an extra UINT is added to specify the number of bits per sample. Use the PCMWAVEFORMAT structure in this case. For all other waveform-audio formats, use the WAVEFORMATEX structure to specify the length of the additional data.
        /// If you choose to have a window or thread receive callback information, the following messages are sent to the window procedure function to indicate the progress of waveform-audio output: MM_WOM_OPEN, MM_WOM_CLOSE, and MM_WOM_DONE.
        /// If you choose to have a function receive callback information, the following messages are sent to the function to indicate the progress of waveform-audio output: WOM_OPEN, WOM_CLOSE, and WOM_DONE. 
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutOpen(ref IntPtr phwo, Int32 uDeviceID, ref WAVEFORMATEX pwfx, WaveOutProc dwCallback, IntPtr dwCallbackInstance, WaveOpenFlags dwFlags);
        /// <summary>
        /// The waveOutPause function pauses playback on the given waveform-audio output device. The current position is saved.
        /// Use the waveOutRestart function to resume playback from the current position.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Calling this function when the output is already paused has no effect, and the function returns zero.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutPause(WaveOutSafeHandle hwo);
        /// <summary>
        /// The waveOutPrepareHeader function prepares a waveform-audio data block for playback.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="pwh">Pointer to a <see cref="WAVEHDR"/> structure that identifies the data block to be prepared.</param>
        /// <param name="cbwh">Size, in bytes, of the <see cref="WAVEHDR"/> structure.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// The lpData, dwBufferLength, and dwFlags members of the WAVEHDR structure must be set before calling this function.
        /// The dwFlags member must be set to WHDR_PREPARED, WHDR_BEGINLOOP, or WHDR_ENDLOOP.
        /// The dwFlags, dwBufferLength, and dwLoops members of the WAVEHDR structure can change between calls to this
        /// function and the waveOutWrite function. (The only flags that can change in this interval for the dwFlags member
        /// are WHDR_BEGINLOOP and WHDR_ENDLOOP.) If you change the size specified by dwBufferLength before the call to
        /// waveOutWrite, the new value must be less than the prepared value.
        /// Preparing a header that has already been prepared has no effect, and the function returns zero.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutPrepareHeader(WaveOutSafeHandle hwo, IntPtr pwh, Int32 cbwh);
        /// <summary>
        /// The waveOutReset function stops playback on the given waveform-audio output device and resets the current
        /// position to zero. All pending playback buffers are marked as done and returned to the application.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutReset(WaveOutSafeHandle hwo);
        /// <summary>
        /// The waveOutRestart function resumes playback on a paused waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Calling this function when the output is not paused has no effect, and the function returns zero.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutRestart(WaveOutSafeHandle hwo);
        /// <summary>
        /// The waveOutSetPitch function sets the pitch for the specified waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="pdwPitch">New pitch multiplier setting. This setting indicates the current change in pitch from the original authored setting. The pitch multiplier must be a positive value. The pitch multiplier is specified as a fixed-point value. The high-order word contains the signed integer part of the number, and the low-order word contains the fractional part. A value of 0x8000 in the low-order word represents one-half, and 0x4000 represents one-quarter. For example, the value 0x00010000 specifies a multiplier of 1.0 (no pitch change), and a value of 0x000F8000 specifies a multiplier of 15.5.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Changing the pitch does not change the playback rate or the sample rate, nor does it change the playback time. Not all devices support pitch changes. To determine whether the device supports pitch control, use the WAVECAPS_PITCH flag to test the dwSupport member of the WAVEOUTCAPS structure (filled by the waveOutGetDevCaps function).
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutSetPitch(WaveOutSafeHandle hwo, Int32 pdwPitch);
        /// <summary>
        /// The waveOutSetPlaybackRate function sets the playback rate for the specified waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="dwRate">New playback rate setting. This setting is a multiplier indicating the current change in playback rate from the original authored setting. The playback rate multiplier must be a positive value.  The rate is specified as a fixed-point value. The high-order word contains the signed integer part of the number, and the low-order word contains the fractional part. A value of 0x8000 in the low-order word represents one-half, and 0x4000 represents one-quarter. For example, the value 0x00010000 specifies a multiplier of 1.0 (no playback rate change), and a value of 0x000F8000 specifies a multiplier of 15.5.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>Changing the playback rate does not change the sample rate but does change the playback time. Not all devices support playback rate changes. To determine whether a device supports playback rate changes, use the WAVECAPS_PLAYBACKRATE flag to test the dwSupport member of the WAVEOUTCAPS structure (filled by the waveOutGetDevCaps function).</remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutSetPlaybackRate(WaveOutSafeHandle hwo, Int32 dwRate);
        /// <summary>
        /// The waveOutSetVolume function sets the volume level of the specified waveform-audio  output device.
        /// </summary>
        /// <param name="hwo">Handle to an open waveform-audio output device. This parameter can also be a device identifier.</param>
        /// <param name="dwVolume">New volume setting. The low-order word contains the left-channel volume setting, and the high-order word contains the right-channel setting. A value of 0xFFFF represents full volume, and a value of 0x0000 is silence.  If a device does not support both left and right volume control, the low-order word of dwVolume specifies the volume level, and the high-order word is ignored.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// If a device identifier is used, then the result of the waveOutSetVolume call applies to all instances of the device. If a device handle is used, then the result applies only to the instance of the device referenced by the device handle.
        /// Not all devices support volume changes. To determine whether the device supports volume control, use the WAVECAPS_VOLUME flag to test the dwSupport member of the WAVEOUTCAPS structure (filled by the waveOutGetDevCaps function). To determine whether the device supports volume control on both the left and right channels, use the WAVECAPS_LRVOLUME flag.
        /// Most devices do not support the full 16 bits of volume-level control and will not use the high-order bits of the requested volume setting. For example, for a device that supports 4 bits of volume control, requested volume level values of 0x4000, 0x4FFF, and 0x43BE all produce the same physical volume setting: 0x4000. The waveOutGetVolume function returns the full 16-bit setting set with waveOutSetVolume.
        /// Volume settings are interpreted logarithmically. This means the perceived increase in volume is the same when increasing the volume level from 0x5000 to 0x6000 as it is from 0x4000 to 0x5000.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutSetVolume(WaveOutSafeHandle hwo, Int32 dwVolume);
        /// <summary>
        /// The waveOutSetVolume function sets the volume level of the specified waveform-audio  output device.
        /// </summary>
        /// <param name="uDeviceID">Handle to an open waveform-audio output device. This parameter can also be a device identifier.</param>
        /// <param name="dwVolume">New volume setting. The low-order word contains the left-channel volume setting, and the high-order word contains the right-channel setting. A value of 0xFFFF represents full volume, and a value of 0x0000 is silence.  If a device does not support both left and right volume control, the low-order word of dwVolume specifies the volume level, and the high-order word is ignored.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>MMSYSERR_NOTSUPPORTED if the function isn't supported.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// If a device identifier is used, then the result of the waveOutSetVolume call applies to all instances of the device. If a device handle is used, then the result applies only to the instance of the device referenced by the device handle.
        /// Not all devices support volume changes. To determine whether the device supports volume control, use the WAVECAPS_VOLUME flag to test the dwSupport member of the WAVEOUTCAPS structure (filled by the waveOutGetDevCaps function). To determine whether the device supports volume control on both the left and right channels, use the WAVECAPS_LRVOLUME flag.
        /// Most devices do not support the full 16 bits of volume-level control and will not use the high-order bits of the requested volume setting. For example, for a device that supports 4 bits of volume control, requested volume level values of 0x4000, 0x4FFF, and 0x43BE all produce the same physical volume setting: 0x4000. The waveOutGetVolume function returns the full 16-bit setting set with waveOutSetVolume.
        /// Volume settings are interpreted logarithmically. This means the perceived increase in volume is the same when increasing the volume level from 0x5000 to 0x6000 as it is from 0x4000 to 0x5000.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutSetVolume(IntPtr uDeviceID, Int32 dwVolume);
        /// <summary>
        /// The waveOutUnprepareHeader function cleans up the preparation performed by the <see cref="waveOutPrepareHeader"/> function. This function must be called after the device driver is finished with a data block. You must call this function before freeing the buffer.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="pwh">Pointer to a <see cref="WAVEHDR"/> structure identifying the data block to be cleaned up.</param>
        /// <param name="cbwh">Size, in bytes, of the <see cref="WAVEHDR"/> structure.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>WAVERR_STILLPLAYING if the data block pointed to by the <paramref name="pwh"/> parameter is still in the queue.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// This function complements waveOutPrepareHeader. You must call this function before freeing the buffer. After passing a buffer to the device driver with the waveOutWrite function, you must wait until the driver is finished with the buffer before calling waveOutUnprepareHeader.
        /// Unpreparing a buffer that has not been prepared has no effect, and the function returns zero.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutUnprepareHeader(WaveOutSafeHandle hwo, IntPtr pwh, Int32 cbwh);
        /// <summary>
        /// The waveOutWrite function sends a data block to the given waveform-audio output device.
        /// </summary>
        /// <param name="hwo">Handle to the waveform-audio output device.</param>
        /// <param name="pwh">Pointer to a WAVEHDR structure containing information about the data block.</param>
        /// <param name="cbwh">Size, in bytes, of the WAVEHDR structure.</param>
        /// <returns>
        /// Returns MMSYSERR_NOERROR if successful or an error otherwise. Possible error values include the following.
        /// <list type="Errors">
        /// <item>MMSYSERR_INVALHANDLE if the specified device handle is invalid.</item>
        /// <item>MMSYSERR_NODRIVER if no device driver is present.</item>
        /// <item>MMSYSERR_NOMEM if unable to allocate or lock memory.</item>
        /// <item>WAVERR_UNPREPARED if the data block pointed to by the <paramref name="pwh"/> parameter hasn't been prepared. </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// When the buffer is finished, the WHDR_DONE bit is set in the dwFlags member of the WAVEHDR structure. 
        /// The buffer must be prepared with the waveOutPrepareHeader function before it is passed to waveOutWrite. Unless the device is paused by calling the waveOutPause function, playback begins when the first data block is sent to the device.
        /// </remarks>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern MMSYSERROR waveOutWrite(WaveOutSafeHandle hwo, IntPtr pwh, Int32 cbwh);
    }
}
