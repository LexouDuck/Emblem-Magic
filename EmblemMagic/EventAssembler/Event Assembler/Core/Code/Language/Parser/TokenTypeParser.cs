// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Parser.TokenTypeParser
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.IO.Scanners;
using Nintenlord.Parser;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Parser
{
  internal sealed class TokenTypeParser : Nintenlord.Parser.Parser<Token, Token>
  {
    private static Dictionary<TokenType, TokenTypeParser> Parsers = new Dictionary<TokenType, TokenTypeParser>();
    private readonly TokenType type;

    private TokenTypeParser(TokenType type)
    {
      this.type = type;
    }

    protected override Token ParseMain(IScanner<Token> scanner, out Match<Token> match)
    {
      Token current = scanner.Current;
      if (current.Type == this.type)
      {
        match = new Match<Token>(scanner, 1);
        scanner.MoveNext();
      }
      else
        match = new Match<Token>(scanner, "Got {0}, expected {1}, {3}", new object[4]
        {
          current.HasValue ? (object) current.Value : (object) current.Type.ToString(),
          (object) this.type,
          (object) current,
          (object) current.Position
        });
      return current;
    }

    public static TokenTypeParser GetTypeParser(TokenType type)
    {
      TokenTypeParser tokenTypeParser;
      if (!TokenTypeParser.Parsers.TryGetValue(type, out tokenTypeParser))
        tokenTypeParser = TokenTypeParser.Parsers[type] = new TokenTypeParser(type);
      return tokenTypeParser;
    }

    public override string ToString()
    {
      return string.Format("{0}parser", (object) this.type);
    }
  }
}
