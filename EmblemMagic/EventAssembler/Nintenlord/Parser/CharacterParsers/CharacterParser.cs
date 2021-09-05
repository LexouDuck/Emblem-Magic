// -----------------------------------------------------------------------
// <copyright file="CharacterParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Parser.CharacterParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class CharacterParser : Parser<Char, Char>
    {
        readonly char character;

        public CharacterParser(char c)
        {
            this.character = c;
        }

        protected override char ParseMain(IO.Scanners.IScanner<char> scanner, out Match<char> match)
        {
            char c = scanner.Current;
            if (c == character)
            {
                match = new Match<char>(scanner, 1);
                scanner.MoveNext();
                return c;
            }
            else
            {
                match = new Match<char>(scanner, "Expected {0}, got {1}", character, c);
                return '\0';
            }
        }
    }
}
