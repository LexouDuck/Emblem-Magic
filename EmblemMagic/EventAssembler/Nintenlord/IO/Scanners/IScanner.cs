using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nintenlord.IO.Scanners
{
    /// <summary>
    /// Scans tokens.
    /// </summary>
    /// <typeparam name="T">Type of tokens to scan</typeparam>
    public interface IScanner<out T>
    {
        /// <summary>
        /// True if scanner has run out of tokens and calls to MoveNext and Current are invalid, else false.
        /// </summary>
        bool IsAtEnd { get; }

        /// <summary>
        /// Gets or sets the current offset. Setting is only allowed if CanSeek == True.
        /// </summary>
        /// <exception cref="NotSupportedException">If setting is not allowed.</exception>
        long Offset { get; set; }
        
        /// <summary>
        /// True if offset can be set, else false.
        /// </summary>
        bool CanSeek { get; }

        /// <summary>
        /// Current token on the stream.
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Moves scanner to the next token.
        /// </summary>
        /// <returns>True if next token exists and move was succesful, else false.</returns>
        bool MoveNext();
    }
}
