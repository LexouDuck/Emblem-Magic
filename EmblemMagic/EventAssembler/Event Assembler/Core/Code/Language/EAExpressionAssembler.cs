// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.EAExpressionAssembler
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Event_Assembler.Core.Code.Templates;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using Nintenlord.IO;
using Nintenlord.Parser;
using Nintenlord.Utility;
using Nintenlord.Utility.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Language
{
	internal sealed class EAExpressionAssembler
	{
		private const string currentOffsetCode = "CURRENTOFFSET";
		private const string messagePrinterCode = "MESSAGE";
		private const string errorPrinterCode = "ERROR";
		private const string warningPrinterCode = "WARNING";
		private const string offsetAligner = "ALIGN";
		private const string offsetChanger = "ORG";
		private const string offsetPusher = "PUSH";
		private const string offsetPopper = "POP";
		private const string assertion = "ASSERT";
		private const string protectCode = "PROTECT";
		private readonly IParser<Token, IExpression<int>> parser;
		private readonly ICodeTemplateStorer storer;
		private ILog log;

		private Dictionary<IExpression<int>, ScopeStructure<int>> scopeStructures;

		private int currentOffset;

		private Stack<int> offsetHistory;

		private List<Tuple<int, int>> protectedRegions;

		public EAExpressionAssembler (ICodeTemplateStorer storer, IParser<Token, IExpression<int>> parser)
		{
			this.parser = parser;
			this.storer = storer;
		}

		public void Assemble(IPositionableInputStream input, BinaryWriter output, ILog log) {
			this.log = log;

			this.offsetHistory = new Stack<int> ();
			this.protectedRegions = new List<Tuple<int, int>> ();

			this.scopeStructures = new Dictionary<IExpression<int>, ScopeStructure<int>> ();

			TokenScanner tokenScanner = new TokenScanner (input);

			if (!tokenScanner.MoveNext ())
				return;

			Match<Token> match;
			IExpression<int> expression = parser.Parse (tokenScanner, out match);

			if (!match.Success) {
				log.AddError (match.Error);
				return;
			}

			if (!tokenScanner.IsAtEnd && tokenScanner.Current.Type != TokenType.EndOfStream) {
				AddNotReachedEnd (tokenScanner.Current);
				return;
			}

			if (log.ErrorCount == 0) {
				this.currentOffset = 0;
				ExecuteLayoutPass (expression, null);
			}

			if (log.ErrorCount == 0) {
				this.currentOffset = 0;
				ExecuteWritePass (output, expression, null);
			}
		}

		private void ExecuteLayoutPass(IExpression<int> expression, ScopeStructure<int> scope) {
			switch (expression.Type) {

			case EAExpressionType.Scope:
				{
					ScopeStructure<int> newScope = new ScopeStructure<int> (scope);
					scopeStructures [(Scope<int>)expression] = newScope;

					foreach (IExpression<int> child in expression.GetChildren())
						ExecuteLayoutPass (child, newScope);

					break;
				}

			case EAExpressionType.Code:
				{
					Code<int> code = expression as Code<int>;

					if (code.IsEmpty || HandleBuiltInCodeLayout (code, scope))
						break;

					Types.Type[] sig = ((IEnumerable<IExpression<int>>)code.Parameters).Select (new Func<IExpression<int>, Types.Type> (Types.Type.GetType<int>)).ToArray ();

					CanCauseError<ICodeTemplate> templateError = this.storer.FindTemplate (code.CodeName.Name, sig);

					if (templateError.CausedError) {
						AddError<int, ICodeTemplate> ((IExpression<int>)code, templateError);
						break;
					}

					ICodeTemplate template = templateError.Result;

					// Checking alignment
					if (currentOffset % template.OffsetMod != 0)
						this.AddError<int> ((IExpression<int>)code, "Code {0}'s offset {1} is not divisible by {2}", (object)code.CodeName.Name, (object)this.currentOffset.ToHexString ("$"), (object)template.OffsetMod);

					// TODO: maybe we want to store the template somewhere to lookup for later?
					// May or may not be faster than computing it again in the write pass

					this.currentOffset += template.GetLengthBytes (((IEnumerable<IExpression<int>>)code.Parameters).ToArray ());
					break;
				}

			case EAExpressionType.Labeled:
				{
					CanCauseError err = scope.AddNewSymbol (((LabelExpression<int>)expression).LabelName, new ValueExpression<int> (this.currentOffset, new FilePosition ()));

					if (err.CausedError)
						AddWarning (expression, err.ErrorMessage);

					break;
				}

			case EAExpressionType.Assignment:
				{
					Assignment<int> assingment = (Assignment<int>)expression;
					CanCauseError<int> value = Folding.Fold (assingment.Value, x => this.GetSymbolValue (scope, x));

					CanCauseError err = null;

					if (value.CausedError)
						err = scope.AddNewSymbol (assingment.Name, assingment.Value);
					else
						err = scope.AddNewSymbol (assingment.Name, new ValueExpression<int> (value.Result, new FilePosition ()));

					if (err.CausedError)
						AddWarning (expression, err.ErrorMessage);

					break;
				}

			case EAExpressionType.RawData:
				{
					RawData<int> rawData = (RawData<int>)expression;
					this.currentOffset += rawData.Data.Length;

					break;
				}

			default:
				throw new ArgumentException ("Badly formed tree.");

			}
		}

		private void ExecuteWritePass(BinaryWriter output, IExpression<int> expression, ScopeStructure<int> scope) {
			// This is to be executed *after* the layout pass

			switch (expression.Type) {

			case EAExpressionType.Scope:
				{
					ScopeStructure<int> newScope = scopeStructures [(Scope<int>)expression];

					foreach (IExpression<int> child in expression.GetChildren())
						ExecuteWritePass (output, child, newScope);

					break;
				}

			case EAExpressionType.Code:
				{
					Code<int> code = expression as Code<int>;

					if (code.IsEmpty || HandleBuiltInCodeWrite (code, scope))
						break;

					// Maybe all of this template lookup up can be made faster by
					// storing the found template from the layout pass?

					Types.Type[] sig = ((IEnumerable<IExpression<int>>)code.Parameters).Select (new Func<IExpression<int>, Types.Type> (Types.Type.GetType<int>)).ToArray ();

					CanCauseError<ICodeTemplate> templateError = this.storer.FindTemplate (code.CodeName.Name, sig);

					if (templateError.CausedError) {
						AddError<int, ICodeTemplate> ((IExpression<int>)code, templateError);
						break;
					}

					// We won't check for alignment as it should already have been done in the layout pass

					ICodeTemplate template = templateError.Result;

					CanCauseError<byte[]> data = template.GetData (code.Parameters, x => this.GetSymbolValue (scope, x));

					if (data.CausedError)
						// Can't compute code data, so we err
						this.AddError<int, byte[]> (expression, data);
					else {
						// Write data
						TryWrite (output, expression, currentOffset, data.Result);
						this.currentOffset += data.Result.Length;
					}

					break;
				}

			case EAExpressionType.RawData:
				{
					RawData<int> rawData = (RawData<int>)expression;

					TryWrite (output, expression, this.currentOffset, rawData.Data);
					this.currentOffset += rawData.Data.Length;

					break;
				}

			case EAExpressionType.Labeled:
			case EAExpressionType.Assignment:
				break;

			default:
				throw new ArgumentException ("Badly formed tree.");

			}
		}

		private bool SyncOutputCursorWithOffset(BinaryWriter output, long offset) {
			if (output.BaseStream.Position != offset) {
				if (!output.BaseStream.CanSeek)
					return false;
				
				output.BaseStream.Seek (offset, SeekOrigin.Begin);
			}

			return true;
		}

		private bool TryWrite(BinaryWriter output, IExpression<int> expression, long offset, byte[] data) {
			if (!SyncOutputCursorWithOffset (output, (long)this.currentOffset)) {
				this.AddError<int> (expression, "Stream cannot be seeked.");
				return false;
			}

			if (IsProtected (this.currentOffset, data.Length)) {
				this.AddError<int> (expression, "Attempting to modify protected memory at " + this.currentOffset.ToHexString ("$") + " with code of length " + data.Length);
				return false;
			}

			output.Write (data);
			return true;
		}

		private bool HandleBuiltInCodeLayout(Code<int> code, ScopeStructure<int> scope) {
			switch (code.CodeName.Name) {

			case messagePrinterCode:
			case errorPrinterCode:
			case warningPrinterCode:
				return true;

			case currentOffsetCode:
			case offsetAligner:
				HandleBuiltInOffsetAlign (code, scope);
				return true;

			case offsetChanger:
				HandleBuiltInOffsetChange (code, scope);
				return true;

			case offsetPusher:
				HandleBuiltInOffsetPush (code, scope);
				return true;

			case offsetPopper:
				HandleBuiltInOffsetPop (code, scope);
				return true;

			case assertion:
				return true;

			case protectCode:
				HandleBuiltInProtect (code, scope);
				return true;

			default:
				return false;

			}
		}

		private bool HandleBuiltInCodeWrite(Code<int> code, ScopeStructure<int> scope) {
			switch (code.CodeName.Name) {

			case messagePrinterCode:
				this.log.AddMessage (this.ExpressionToString ((IExpression<int>)code, scope).Substring (code.CodeName.Name.Length + 1));
				return true;

			case errorPrinterCode:
				this.log.AddError (this.ExpressionToString ((IExpression<int>)code, scope).Substring (code.CodeName.Name.Length + 1));
				return true;

			case warningPrinterCode:
				this.log.AddWarning (this.ExpressionToString ((IExpression<int>)code, scope).Substring (code.CodeName.Name.Length + 1));
				return true;

			case currentOffsetCode:
			case offsetAligner:
				HandleBuiltInOffsetAlign (code, scope);
				return true;

			case offsetChanger:
				HandleBuiltInOffsetChange (code, scope);
				return true;

			case offsetPusher:
				HandleBuiltInOffsetPush (code, scope);
				return true;

			case offsetPopper:
				HandleBuiltInOffsetPop (code, scope);
				return true;

			case assertion:
				HandleBuiltInAssert (code, scope);
				return true;

			case protectCode:
				return true;

			default:
				return false;

			}
		}

		private void HandleBuiltInOffsetChange(Code<int> code, ScopeStructure<int> scope) {
			if (code.ParameterCount.IsInRange (1, 1) && !(code [0] is ExpressionList<int>)) {
				CanCauseError<int> canCauseError = Folding.Fold (code [0], (x => this.GetSymbolValue (scope, x)));
				if (!canCauseError.CausedError)
					this.currentOffset = canCauseError.Result;
			}
		}

		private void HandleBuiltInOffsetAlign(Code<int> code, ScopeStructure<int> scope) {
			if (code.ParameterCount.IsInRange (1, 1)) {
				if (code [0] is ExpressionList<int>) {
					this.AddNotAtomTypeParameter<int> (code [0]);
				} else {
					CanCauseError<int> error = Folding.Fold (code [0], (x => this.GetSymbolValue (scope, x)));

					if (error.CausedError)
						this.AddError<int, int> ((IExpression<int>)code, error);
					else
						this.currentOffset = this.currentOffset.ToMod (error.Result);
				}
			} else
				this.AddNotCorrectParameters<int> (code, 1);
		}

		private void HandleBuiltInOffsetPush (Code<int> code, ScopeStructure<int> scope) {
			if (code.ParameterCount.IsInRange (0, 0))
				this.offsetHistory.Push (this.currentOffset);
			else
				this.AddNotCorrectParameters<int> (code, 0);
		}

		private void HandleBuiltInOffsetPop(Code<int> code, ScopeStructure<int> scope) {
			if (code.ParameterCount.IsInRange (0, 0)) {
				if (this.offsetHistory.Count > 0)
					this.currentOffset = this.offsetHistory.Pop ();
				else
					this.AddError ((IExpression<int>)code, "Tried to pop while offset stack was empty.");
			} else
				this.AddNotCorrectParameters<int> (code, 0);
		}

		private void HandleBuiltInAssert (Code<int> code, ScopeStructure<int> scope) {
			if (code.ParameterCount.IsInRange (1, 1)) {
				if (code [0] is ExpressionList<int>) {
					this.AddNotAtomTypeParameter<int> (code [0]);
				} else {
					CanCauseError<int> error = Folding.Fold (code [0], (x => this.GetSymbolValue (scope, x)));

					if (error.CausedError)
						this.AddError ((IExpression<int>)code, error);
					else if (error.Result < 0)
						this.AddError (code, "Assertion failed.");
				}
			} else
				this.AddNotCorrectParameters<int> (code, 1);
		}

		private void HandleBuiltInProtect(Code<int> code, ScopeStructure<int> scope) {
			if (code.ParameterCount.IsInRange (1, 1)) {
				CanCauseError<int> canCauseError = Folding.Fold (code [0], (x => this.GetSymbolValue (scope, x)));
				if (!canCauseError.CausedError)
					this.protectedRegions.Add (new Tuple<int, int> (canCauseError.Result, 4));
			} else if (code.ParameterCount.IsInRange (2, 2)) {
				CanCauseError<int> firstParam = Folding.Fold (code [0], (x => this.GetSymbolValue (scope, x)));
				CanCauseError<int> secondParam = Folding.Fold (code [1], (x => this.GetSymbolValue (scope, x)));
				if (!firstParam.CausedError && !secondParam.CausedError) {
					if (firstParam.Result < secondParam.Result)
						this.protectedRegions.Add (new Tuple<int, int> (firstParam.Result, secondParam.Result - firstParam.Result));
					else
						this.log.AddWarning ("Protected region not valid (end offset not after start offset). No region protected.");
				}
			} else {
				this.AddNotCorrectParameters<int> (code, 2);
			}
		}

		public IEnumerable<KeyValuePair<string, int>> GetGlobalSymbols ()
		{
			foreach (ScopeStructure<int> scope in scopeStructures.Values) {
				if (scope.IsGlobalScope ()) {
					foreach (KeyValuePair<string, IExpression<int>> pair in scope.GetSymbols()) {
						int? value = GetSymbolValue (scope, pair.Key);

						if (value.HasValue)
							yield return new KeyValuePair<string, int> (pair.Key, value.Value);
					}
				}
			}
		}

		private string ExpressionToString (IExpression<int> exp, ScopeStructure<int> scope)
		{
			switch (exp.Type) {

			case EAExpressionType.Code:
				Code<int> code = exp as Code<int>;
				return code.CodeName.Name + ((IEnumerable<IExpression<int>>)code.Parameters).Select ((x => this.ExpressionToString (x, scope))).ToElementWiseString<string> (" ", " ", "");
			
			case EAExpressionType.XOR:
			case EAExpressionType.AND:
			case EAExpressionType.OR:
			case EAExpressionType.LeftShift:
			case EAExpressionType.RightShift:
			case EAExpressionType.ArithmeticRightShift:
			case EAExpressionType.Division:
			case EAExpressionType.Multiply:
			case EAExpressionType.Modulus:
			case EAExpressionType.Minus:
			case EAExpressionType.Sum:
			case EAExpressionType.Value:
			case EAExpressionType.Symbol:
				CanCauseError<int> canCauseError = Folding.Fold (exp, (y => this.GetSymbolValue (scope, y)));
				if (canCauseError.CausedError)
					return exp.ToString ();
				return canCauseError.Result.ToHexString ("0x");
			
			case EAExpressionType.List:
				return exp.GetChildren ().ToElementWiseString<IExpression<int>> (", ", "[", "]");
			
			default:
				throw new ArgumentException ("malformed tree");
			
			}
		}

		private int? GetSymbolValue (ScopeStructure<int> scope, string symbolName)
		{
			if (symbolName.Equals (currentOffsetCode, StringComparison.OrdinalIgnoreCase) || symbolName.Equals (offsetChanger, StringComparison.OrdinalIgnoreCase))
				return new int? (this.currentOffset);

			CanCauseError<IExpression<int>> symbolValue = scope.GetSymbolValue (symbolName);

			if (symbolValue.CausedError)
				return new int? ();

			CanCauseError<int> canCauseError = Folding.Fold (symbolValue.Result, (x => this.GetSymbolValue (scope, x)));

			if (canCauseError.CausedError)
				return new int? ();

			return new int? (canCauseError.Result);
		}

		private bool IsProtected (int offset, int length)
		{
			foreach (Tuple<int, int> protectedRegion in protectedRegions) {
				//They intersect if the last offset in the given region is after the start of this one
				//and the first offset in the given region is before the last of this one
				if (offset + length > protectedRegion.Item1 && offset < protectedRegion.Item1 + protectedRegion.Item2)
					return true;
			}
			return false;
		}

		private void AddWarning<T> (IExpression<T> code, string error)
		{
			this.log.AddWarning (code.Position.ToString () + ": " + error);
		}

		private void AddWarning<T> (IExpression<T> code, string format, params object[] args)
		{
			this.log.AddWarning (code.Position.ToString () + ": " + string.Format (format, args));
		}

		private void AddError<T, TResult> (IExpression<T> code, CanCauseError<TResult> error)
		{
			this.log.AddError (code.Position.ToString () + ": " + error.ErrorMessage);
		}

		private void AddError<T> (IExpression<T> code, string error)
		{
			this.log.AddError (code.Position.ToString () + ": " + error);
		}

		private void AddError<T> (IExpression<T> code, string format, params object[] args)
		{
			this.log.AddError (code.Position.ToString () + ": " + string.Format (format, args));
		}

		private void AddNotAtomTypeParameter<T> (IExpression<T> parameter)
		{
			this.log.AddError ("{1}: Parameter {0} doesn't have correct type.", new object[2] {
				(object)parameter,
				(object)parameter.Position
			});
		}

		private void AddNotCorrectParameters<T> (Nintenlord.Event_Assembler.Core.Code.Language.Expression.Code<T> code, int paramCount)
		{
			this.log.AddError ("{3}: Code {0} doesn't have {2} parameters, but has {1} parameters", (object)code.CodeName, (object)paramCount, (object)code.Parameters.Length, (object)code.Position);
		}

		private void AddNotCorrectParameters<T> (Nintenlord.Event_Assembler.Core.Code.Language.Expression.Code<T> code, int paramMin, int paramMax)
		{
			this.log.AddError ("{4}: Code {0} doesn't have {3} parameters, but has {1}-{2} parameters", (object)code.CodeName, (object)paramMin, (object)paramMax, (object)code.Parameters.Length, (object)code.Position);
		}

		private void AddNotReachedEnd(Token token) {
			log.AddError (token.Position.ToString () + ": Didn't reach end, currently at " + token.ToString ());

			switch (token.Type) {

			case TokenType.Comma:
				log.AddError ("Maybe you tried to use a non-defined Macro?");
				break;

			case TokenType.IntegerLiteral:
				log.AddError ("Maybe this number isn't formatted properly?");
				break;

			}
		}
	}
}
