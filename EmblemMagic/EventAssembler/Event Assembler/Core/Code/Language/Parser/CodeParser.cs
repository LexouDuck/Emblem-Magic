using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Parser
{
    class CodeParser<T> : Parser<Token, IExpression<T>>
    {
        private IParser<Token, IExpression<T>> parameterParser;

        public CodeParser(IParser<Token, IExpression<T>> parameterParser)
        {
            this.parameterParser = parameterParser;
        }

        protected override IExpression<T> ParseMain(IScanner<Token> scanner, out Match<Token> match)
        {
            match = new Match<Token>(scanner);

            Token first = scanner.Current;
            long startTokenIndex = scanner.Offset;

            if (first.Type == TokenType.Symbol)
            {
                List<IExpression<T>> codeParameters = new List<IExpression<T>>();

                ++match;
                scanner.MoveNext();

                while (true)
                {
                    long tokenIndex = scanner.Offset;
					Match<Token> parameterMatch;
                    IExpression<T> param = parameterParser.Parse(scanner, out parameterMatch);

                    if (parameterMatch.Success)
                    {
                        codeParameters.Add(param);
                        match += parameterMatch;
                    }
                    else
                    {
                        scanner.Offset = tokenIndex;
                        return new Code<T>(new Symbol<T>(first.Value, first.Position), codeParameters);
                    }
                }
            }

            scanner.Offset = startTokenIndex;
            match = new Match<Token>(scanner, "failed to parse code");

            return null;
        }
    }
}
