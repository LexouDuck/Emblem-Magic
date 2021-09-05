// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Lexer.Tokeniser
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Lexer
{
	public static class Tokeniser
	{
		private static readonly Dictionary<TokenType, string> tokenRegexs = new Dictionary<TokenType, string> ();
		private static readonly Regex tokenRegex;

		static Tokeniser ()
		{
			// Original regexes
			Tokeniser.tokenRegexs [TokenType.IntegerLiteral] = "[0-9\\$][0-9a-zA-Z_]*";
			Tokeniser.tokenRegexs [TokenType.Symbol] = "[a-zA-Z_][0-9a-zA-Z_]*";
			Tokeniser.tokenRegexs [TokenType.MathOperator] = "\\+|\\-|\\*|\\/|\\%|\\&|\\||\\^|\\~|(>>>)|(>>)|(<<)";
			Tokeniser.tokenRegexs [TokenType.CodeEnder] = ";";
			Tokeniser.tokenRegexs [TokenType.LeftParenthesis] = "\\(";
			Tokeniser.tokenRegexs [TokenType.RightParenthesis] = "\\)";
			Tokeniser.tokenRegexs [TokenType.LeftSquareBracket] = "\\[";
			Tokeniser.tokenRegexs [TokenType.RightSquareBracket] = "\\]";
			Tokeniser.tokenRegexs [TokenType.LeftCurlyBracket] = "\\{";
			Tokeniser.tokenRegexs [TokenType.RightCurlyBracket] = "\\}";
			Tokeniser.tokenRegexs [TokenType.StringLiteral] = "\\\".*\\\"";
			Tokeniser.tokenRegexs [TokenType.Comma] = ",";
			Tokeniser.tokenRegexs [TokenType.Equal] = "=";
			Tokeniser.tokenRegexs [TokenType.Colon] = ":";

			// Do we want that?
			// Tokeniser.tokenRegexs [TokenType.RawHex] = "\\@hex[0-9a-fA-F]*\\@";

			// Mostly for internal use by #incbin/#incext
			Tokeniser.tokenRegexs [TokenType.Base64Literal] = "\\@b64[0-9a-zA-Z\\+\\/=]*\\@";
      
			StringBuilder stringBuilder = new StringBuilder ("");

			foreach (KeyValuePair<TokenType, string> keyValuePair in Tokeniser.tokenRegexs)
				stringBuilder.AppendFormat ("(?<{0}>{1})|", (object)keyValuePair.Key, (object)keyValuePair.Value);

			Tokeniser.tokenRegex = new Regex (stringBuilder.ToString (0, stringBuilder.Length - 1), RegexOptions.Compiled | RegexOptions.CultureInvariant);
		}

		public static IEnumerable<Token> TokeniseLine (string line, string file, int lineNumber)
		{
			MatchCollection matches = Tokeniser.tokenRegex.Matches (line);
			foreach (Match match in matches) {
				GroupCollection groups = match.Groups;
				foreach (TokenType key in Tokeniser.tokenRegexs.Keys) {
					Group group = groups [key.ToString ()];
					if (group.Success) {
						for (int i = 0; i < group.Captures.Count; ++i) {
							Capture capture = group.Captures [i];
							Token token = new Token (new FilePosition (file, lineNumber, capture.Index + 1), key, capture.Value);
							yield return token;
						}
					}
				}
			}
			yield return new Token (new FilePosition (file, lineNumber, line.Length), TokenType.NewLine, "NL");
		}

		public static IEnumerable<Token> Tokenise (IPositionableInputStream input)
		{
			label_1:
			string line = input.ReadLine ();
			if (line != null) {
				using (IEnumerator<Token> enumerator = Tokeniser.TokeniseLine (line, input.CurrentFile, input.LineNumber).GetEnumerator ()) {
					while (enumerator.MoveNext ()) {
						Token token = enumerator.Current;
						yield return token;
					}
					goto label_1;
				}
			} else
				yield return new Token (new FilePosition (input.CurrentFile, input.LineNumber, 0), TokenType.EndOfStream, "END");
		}
	}
}
