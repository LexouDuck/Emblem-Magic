using Nintenlord.IO;
using System;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Lexer
{
	public struct Token : IFilePositionable
	{
		private readonly FilePosition position;
		private readonly TokenType type;
		private readonly string value;

		public bool HasValue {
			get {
				return this.value != null;
			}
		}

		public TokenType Type {
			get {
				return this.type;
			}
		}

		public string Value {
			get {
				if (this.value != null)
					return this.value;
				throw new InvalidOperationException ();
			}
		}

		public FilePosition Position {
			get {
				return this.position;
			}
		}

		private Token (FilePosition position, TokenType type)
		{
			this = new Token (position, type, (string)null);
		}

		public Token (FilePosition position, TokenType type, string value)
		{
			this.position = position;
			this.type = type;
			this.value = value;
		}

		public override string ToString ()
		{
			return this.type.ToString () + (this.value != null ? "(" + this.value + ")" : "");
		}
	}
}
