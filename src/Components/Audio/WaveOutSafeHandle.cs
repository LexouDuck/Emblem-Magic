//-----------------------------------------------------------------------
// <copyright file="WaveOutSafeHandle.cs" company="(none)">
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
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Encapsulates a handle to a waveOut device.
    /// </summary>
    public sealed class WaveOutSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the WaveOutSafeHandle class.
        /// </summary>
        public WaveOutSafeHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WaveOutSafeHandle class.
        /// </summary>
        /// <param name="tempHandle">A temporary handle from which to initialize.  The temporart handle MUST NOT be released after this instance has been created.</param>
        public WaveOutSafeHandle(IntPtr tempHandle)
            : base(true)
        {
            this.handle = tempHandle;
        }

        /// <summary>
        /// Releases the resuorces used by this handle.
        /// </summary>
        /// <returns>true, if disposing of the handle succeeded; false, otherwise.</returns>
        protected override bool ReleaseHandle()
        {
            MMSYSERROR ret = NativeMethods.waveOutClose(this);
            return ret == MMSYSERROR.MMSYSERR_NOERROR;
        }
    }
}
