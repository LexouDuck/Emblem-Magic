using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Parser
{
    class LabelParser<T> : Parser<Token, IExpression<T>>
    {
        protected override IExpression<T> ParseMain(IScanner<Token> scanner, out Match<Token> match)
        {
            match = new Match<Token>(scanner);

            Token current = scanner.Current;
            long currentIndex = scanner.Offset;

            if (current.Type == TokenType.Symbol)
            {
                string labelName = current.Value;

                ++match;
                scanner.MoveNext();

                if (scanner.Current.Type == TokenType.Colon)
                {
                    ++match;
                    scanner.MoveNext();

                    return new LabelExpression<T>(current.Position, labelName);
                }
            }

            scanner.Offset = currentIndex;
            match = new Match<Token>(scanner, "failed to parse label");

            return null;
        }
    }
}
