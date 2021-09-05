using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;
using Nintenlord.IO;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Parser
{
    class StatementSeparatorParser<T> : Parser<Token, IExpression<T>>
    {
        protected override IExpression<T> ParseMain(IScanner<Token> scanner, out Match<Token> match)
        {
            match = new Match<Token>(scanner);
            long firstOffset = scanner.Offset;

            FilePosition pos = scanner.Current.Position;

            while (IsStatementEnding(scanner.Current.Type))
            {
                ++match;
                scanner.MoveNext();
            }

            if (scanner.Offset == firstOffset)
            {
                match = new Match<Token>(scanner, "reached end of statements");
                return null;
            }

            return Code<T>.EmptyCode(pos);
        }

        private static bool IsStatementEnding(TokenType type)
        {
            return type == TokenType.CodeEnder || type == TokenType.NewLine;
        }
    }
}
