using System;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser
{
    /// <summary>
    /// Parser of tokens that can return a result.
    /// </summary>
    /// <typeparam name="TIn">Type of tokens parsed by this parser.</typeparam>
    /// <typeparam name="TOut">Type of result returned by this parser.</typeparam>
    public interface IParser<TIn, out TOut>
    {
        /// <summary>
        /// Parses tokens from scanner.
        /// </summary>
        /// <param name="scanner">Scanner from  wich tokens are parsed from.</param>
        /// <param name="match">Tells if the parsing was succesful, how many tokens were consumed 
        /// and the starting offset of the parsing.</param>
        /// <returns>Result of the parsing if the parsing was succesful.</returns>
        TOut Parse(IScanner<TIn> scanner, out Match<TIn> match);
    }
}
