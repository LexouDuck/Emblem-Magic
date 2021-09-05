// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.TokenParser`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Event_Assembler.Core.Code.Language.Parser;
using Nintenlord.IO.Scanners;
using Nintenlord.Parser;
using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression
{
  internal sealed class TokenParser<T> : Nintenlord.Parser.Parser<Token, IExpression<T>>
  {
    private readonly IParser<Token, IExpression<T>> mainParser;

    public TokenParser(Func<string, T> eval)
    {
         mainParser = new ScopeParser<T>(Parsers<T>.MakeStatementParser(eval).Many(), TokenTypeParser.GetTypeParser(TokenType.LeftCurlyBracket), TokenTypeParser.GetTypeParser(TokenType.RightCurlyBracket)).Transform(x => (IExpression<T>)x);
    }

    private void parseEvent<T1, T2>(object sender, ParsingEventArgs<T1, T2> e)
    {
      Console.WriteLine("Parser {0}, matched {1}", sender, e.Match);
    }

    protected override IExpression<T> ParseMain(IScanner<Token> scanner, out Match<Token> match)
    {
      return this.mainParser.Parse(scanner, out match);
    }
  }
}
