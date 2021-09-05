// -----------------------------------------------------------------------
// <copyright file="MathParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Event_Assembler.Core.Code.Language.Parser
{
	using System;
	using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
	using Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree;
	using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
	using Nintenlord.IO.Scanners;
	using Nintenlord.Parser;
	using System.Collections.Generic;
	using Nintenlord.IO;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	sealed class MathParser<T> : Parser<Token, IExpression<T>>
	{
		readonly private Func<string, T> evaluate;

		public MathParser (Func<string, T> evaluate)
		{
			this.evaluate = evaluate;
		}

		protected override IExpression<T> ParseMain (IScanner<Token> scanner, out Match<Token> match)
		{
			return ParsePrecedence (scanner, out match, (int)Precedences.NoPrecedence);
		}

		private IExpression<T> ParsePrecedence (IScanner<Token> scanner, out Match<Token> match, int precedence)
		{
			match = new Match<Token> (scanner);
			MathTokenType tokenType = GetMathTokenType (scanner.Current);

			if (IsPrefixTokenType (tokenType)) {
				Match<Token> prefixMatch;
				IExpression<T> current = ParsePrefix (tokenType, scanner, out prefixMatch);
				match += prefixMatch;

				if (!match.Success)
					return null;

				tokenType = GetMathTokenType (scanner.Current);

				while (IsInfixTokenType (tokenType)) {
					if (precedence >= GetInfixPrecedence (tokenType))
						break;

					Match<Token> infixMatch;

					current = ParseInfix (tokenType, scanner, out infixMatch, current);
					match += infixMatch;

					if (!match.Success)
						return null;

					tokenType = GetMathTokenType (scanner.Current);
				}

				return current;
			}

			match = new Match<Token> (scanner, "couldn't parse math expression");
			return null;
		}

		/*
        private IExpression<T> ParsePrecedence(IScanner<Token> scanner, out Match<Token> match, int precedence)
        {
            match = new Match<Token>(scanner);
			IPrefixParselet prefixParselet;

            if (prefixParselets.TryGetValue(GetMathTokenType(scanner.Current), out prefixParselet))
            {
				Match<Token> prefixMatch;
                IExpression<T> current = prefixParselet.Parse(this, scanner, out prefixMatch);
                match += prefixMatch;

                if (!match.Success)
                    return null;

				IInfixParselet infixParselet;

                while (infixParselets.TryGetValue(GetMathTokenType(scanner.Current), out infixParselet))
                {
                    if (precedence >= infixParselet.Precedence)
                        break;

					Match<Token> infixMatch;

                    current = infixParselet.Parse(this, scanner, out infixMatch, current);
                    match += infixMatch;

                    if (!match.Success)
                        return null;
                }

                return current;
            }

            match = new Match<Token>(scanner, "couldn't parse math expression");
            return null;
        } //*/

		private enum MathTokenType
		{
			Identifer,
			IntLiteral,

			LeftParenthesis,
			RightParenthesis,

			Plus,
			Minus,
			Multiply,
			Divide,
			Modulus,
			LeftShift,
			RightShift,
			ARightShift,
			BitwiseOr,
			BitwiseAnd,
			BitwiseXor,
			Unknown,
		}

		private enum Precedences : int
		{
			NoPrecedence = 0,
			Or,
			Xor,
			And,
			Shifts,
			Sums,
			Products,
			Prefixes,
		}

		private MathTokenType GetMathTokenType (Token token)
		{
			switch (token.Type) {

			case TokenType.StringLiteral:
			case TokenType.Symbol:
				return MathTokenType.Identifer;

			case TokenType.IntegerLiteral:
				return MathTokenType.IntLiteral;

			case TokenType.LeftParenthesis:
				return MathTokenType.LeftParenthesis;

			case TokenType.RightParenthesis:
				return MathTokenType.RightParenthesis;

			case TokenType.MathOperator:
				switch (token.Value) {

				case "|":
					return MathTokenType.BitwiseOr;

				case "&":
					return MathTokenType.BitwiseAnd;

				case "^":
					return MathTokenType.BitwiseXor;

				case "<<":
					return MathTokenType.LeftShift;

				case ">>":
					return MathTokenType.RightShift;

				case ">>>":
					return MathTokenType.ARightShift;

				case "+":
					return MathTokenType.Plus;

				case "-":
					return MathTokenType.Minus;

				case "*":
					return MathTokenType.Multiply;

				case "/":
					return MathTokenType.Divide;

				case "%":
					return MathTokenType.Modulus;

				default:
					return MathTokenType.Unknown;

				}

			default:
				return MathTokenType.Unknown;

			}
		}

		private bool IsPrefixTokenType (MathTokenType type)
		{
			switch (type) {

			case MathTokenType.Identifer:
			case MathTokenType.IntLiteral:
			case MathTokenType.LeftParenthesis:
			case MathTokenType.Plus:
			case MathTokenType.Minus:
				return true;

			}

			return false;
		}

		private bool IsInfixTokenType (MathTokenType type)
		{
			switch (type) {

			case MathTokenType.Plus:
			case MathTokenType.Minus:
			case MathTokenType.Multiply:
			case MathTokenType.Divide:
			case MathTokenType.Modulus:
			case MathTokenType.BitwiseAnd:
			case MathTokenType.BitwiseOr:
			case MathTokenType.BitwiseXor:
			case MathTokenType.LeftShift:
			case MathTokenType.RightShift:
			case MathTokenType.ARightShift:
				return true;

			}

			return false;
		}

		private int GetInfixPrecedence (MathTokenType type)
		{
			switch (type) {

			case MathTokenType.BitwiseOr:
				return (int)Precedences.Or;

			case MathTokenType.BitwiseXor:
				return (int)Precedences.Xor;

			case MathTokenType.BitwiseAnd:
				return (int)Precedences.And;

			case MathTokenType.LeftShift:
			case MathTokenType.RightShift:
			case MathTokenType.ARightShift:
				return (int)Precedences.Shifts;

			case MathTokenType.Plus:
			case MathTokenType.Minus:
				return (int)Precedences.Sums;

			case MathTokenType.Multiply:
			case MathTokenType.Divide:
			case MathTokenType.Modulus:
				return (int)Precedences.Products;

			default:
				return (int)Precedences.NoPrecedence;

			}
		}

		private IExpression<T> ParsePrefix (MathTokenType type, IScanner<Token> scanner, out Match<Token> match)
		{
			match = new Match<Token> (scanner);
			IExpression<T> result = null;

			switch (type) {

			case MathTokenType.Identifer:
				result = new Symbol<T> (scanner.Current.Value, scanner.Current.Position);

				++match;
				scanner.MoveNext ();

				return result;

			case MathTokenType.IntLiteral:
				try {
					result = new ValueExpression<T> (evaluate (scanner.Current.Value), scanner.Current.Position);

					++match;
					scanner.MoveNext ();

					return result;
				} catch (Exception) {
					match = new Match<Token> (scanner, "failed to parse integer literal \"{0}\"", scanner.Current.Value);
					return null;
				}

			case MathTokenType.LeftParenthesis:
				{
					++match;
					scanner.MoveNext ();

					Match<Token> outMatch;
					result = ParsePrecedence (scanner, out outMatch, (int)Precedences.NoPrecedence);

					if (scanner.Current.Type != TokenType.RightParenthesis) {
						match = new Match<Token> (scanner, "couldn't find closing parenthesis");
						return null;
					}

					match += outMatch;

					++match;
					scanner.MoveNext ();

					return result;
				}

			case MathTokenType.Plus:
				{
					++match;
					scanner.MoveNext ();

					Match<Token> outMatch;
					result = ParsePrecedence (scanner, out outMatch, (int)Precedences.Prefixes);
					match += outMatch;

					return result;
				}

			case MathTokenType.Minus:
				{
					FilePosition pos = scanner.Current.Position;

					++match;
					scanner.MoveNext ();

					Match<Token> outMatch;
					result = ParsePrecedence (scanner, out outMatch, (int)Precedences.Prefixes);
					match += outMatch;

					return new Minus<T> (null, result, pos);
				}
			}

			return null;
		}

		private IExpression<T> ParseInfix (MathTokenType type, IScanner<Token> scanner, out Match<Token> match, IExpression<T> previous)
		{
			match = new Match<Token> (scanner);

			++match;
			scanner.MoveNext ();

			Match<Token> rightMatch;
			IExpression<T> right = ParsePrecedence (scanner, out rightMatch, GetInfixPrecedence (type));

			match += rightMatch;

			if (match.Success)
				return MakeBinaryOpExpression (type, previous, right, previous.Position);

			return null;
		}

		private IExpression<T> MakeBinaryOpExpression (MathTokenType type, IExpression<T> left, IExpression<T> right, FilePosition pos)
		{
			switch (type) {

			case MathTokenType.Plus:
				return new Sum<T> (left, right, pos);

			case MathTokenType.Minus:
				return new Minus<T> (left, right, pos);

			case MathTokenType.Multiply:
				return new Multiply<T> (left, right, pos);

			case MathTokenType.Divide:
				return new Division<T> (left, right, pos);

			case MathTokenType.Modulus:
				return new Modulus<T> (left, right, pos);

			case MathTokenType.BitwiseOr:
				return new BitwiseOr<T> (left, right, pos);

			case MathTokenType.BitwiseAnd:
				return new BitwiseAnd<T> (left, right, pos);

			case MathTokenType.BitwiseXor:
				return new BitwiseXor<T> (left, right, pos);

			case MathTokenType.LeftShift:
				return new BitShiftLeft<T> (left, right, pos);

			case MathTokenType.RightShift:
				return new BitShiftRight<T> (left, right, pos);

			case MathTokenType.ARightShift:
				return new ArithmeticShiftRight<T> (left, right, pos);

			default:
				return null;

			}
		}
	}
}
