// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Parser.Parsers
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Parser;
using Nintenlord.Parser.ParserCombinators.BinaryParsers;
using Nintenlord.Parser.ParserCombinators.UnaryParsers;
using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Parser
{
    internal static class Parsers<T>
    {
        public static IParser<Token, IExpression<T>> MakeStatementParser(Func<string, T> literalEvaluate)
        {
			return ((((new Base64Parser<T>() | new LabelParser<T>()) | new AssignmentParser<T>(MakeAtomParser(literalEvaluate))) | new CodeParser<T>(MakeParameterParser(literalEvaluate)) | new StatementSeparatorParser<T>())).Name("Statement");
        }

        private static IParser<Token, IExpression<T>> MakeParameterParser(Func<string, T> literalEvaluate)
        {
            IParser<Token, IExpression<T>> atomParser   = MakeAtomParser(literalEvaluate);
            IParser<Token, IExpression<T>> vectorParser = MakeVectorParser(atomParser);

            return (new OrParser<Token, IExpression<T>>(atomParser, vectorParser)).Name("Parameter");
        }

        private static IParser<Token, IExpression<T>> MakeVectorParser(IParser<Token, IExpression<T>> nameParser)
        {
            IParser<Token, IExpression<T>> result = null;
            result = ((IParser<Token, IExpression<T>>)(((Func<IParser<Token, IExpression<T>>>)(() => result)).Lazy() | nameParser)).SepBy1(TokenTypeParser.GetTypeParser(TokenType.Comma)).Between(TokenTypeParser.GetTypeParser(TokenType.LeftSquareBracket), TokenTypeParser.GetTypeParser(TokenType.RightSquareBracket)).Transform(x => new ExpressionList<T>(x, x[0].Position)).Name("Vector");
            return result.Name("Vector");
        }

        public static IParser<Token, IExpression<T>> MakeAtomParser(Func<string, T> literalEvaluate)
        {
            return new MathParser<T>(literalEvaluate).Name("Atom");
        }
    }
}
