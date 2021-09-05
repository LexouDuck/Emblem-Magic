using System;
using Nintenlord.Parser;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Event_Assembler.Core
{
	public class Base64Parser<T> : Parser<Token, IExpression<T>>
	{
		protected override IExpression<T> ParseMain(IScanner<Token> scanner, out Match<Token> match) {
			Token token = scanner.Current;

			if (token.Type != TokenType.Base64Literal) {
				match = new Match<Token>(scanner, "failed to parse base64 expression");
				return null;
			}

			match = new Match<Token>(scanner);

			++match;
			scanner.MoveNext();

			return new RawData<T> (System.Convert.FromBase64String (token.Value.Substring (4, token.Value.Length - 5)), token.Position);
		}
	}
}
