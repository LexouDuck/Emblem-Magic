using System;
using System.Runtime.InteropServices;

namespace Magic.Components
{
    /// <summary>
    /// Represents a position in the HexBox control
    /// </summary>
    struct BytePositionInfo
    {
        public BytePositionInfo(Int64 index, Int32 characterPosition)
        {
            this._index = index;
            this._characterPosition = characterPosition;
        }

        public Int32 CharacterPosition
        {
            get { return this._characterPosition; }
        }
        Int32 _characterPosition;

        public Int64 Index
        {
            get { return this._index; }
        }
        Int64 _index;
    }



    /// <summary>
    /// The interface for objects that can translate between characters and bytes.
    /// </summary>
    public interface IByteCharConverter
    {
        /// <summary>
        /// Returns the character to display for the byte passed across.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        Char ToChar(Byte b);

        /// <summary>
        /// Returns the byte to use when the character passed across is entered during editing.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        Byte ToByte(Char c);
    }

    /// <summary>
    /// The default <see cref="IByteCharConverter"/> implementation.
    /// </summary>
    public class DefaultByteCharConverter : IByteCharConverter
    {
        /// <summary>
        /// Returns the character to display for the byte passed across.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public virtual Char ToChar(Byte b)
        {
            return b > 0x1F && !(b > 0x7E && b < 0xA0) ? (Char)b : '.';
        }

        /// <summary>
        /// Returns the byte to use for the character passed across.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual Byte ToByte(Char c)
        {
            return (Byte)c;
        }

        /// <summary>
        /// Returns a description of the byte char provider.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return "ANSI (Default)";
        }
    }



    /// <summary>
    /// Specifies the case of hex characters in the HexBox control
    /// </summary>
    public enum HexCasing
    {
        /// <summary>
        /// Converts all characters to uppercase.
        /// </summary>
        Upper = 0,
        /// <summary>
        /// Converts all characters to lowercase.
        /// </summary>
        Lower = 1
    }



    internal static class NativeMethods
    {

        [DllImport("user32.dll")]
        public static extern Int32 GetScrollPos(IntPtr hWnd, Int32 nBar);

        [DllImport("user32.dll")]
        public static extern Int32 SetScrollPos(IntPtr hWnd, Int32 nBar, Int32 nPos, Boolean bRedraw);

        [DllImport("user32.dll", EntryPoint = "PostMessageA")]
        public static extern Boolean PostMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, IntPtr wParam, IntPtr lParam);
        

        [DllImport("user32.dll", SetLastError=true)]
		public static extern Boolean CreateCaret(IntPtr hWnd, IntPtr hBitmap, Int32 nWidth, Int32 nHeight);

		[DllImport("user32.dll", SetLastError=true)]
		public static extern Boolean ShowCaret(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError=true)]
		public static extern Boolean DestroyCaret();

		[DllImport("user32.dll", SetLastError=true)]
		public static extern Boolean SetCaretPos(Int32 X, Int32 Y);

		// Key definitions
		public const Int32 WM_KEYDOWN = 0x100;
		public const Int32 WM_KEYUP = 0x101;
		public const Int32 WM_CHAR = 0x102;
	}
}
