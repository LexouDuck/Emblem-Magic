// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.Folding
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Nintenlord.Utility;
	using Nintenlord.IO;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	static public class Folding
	{
		static private readonly IExpression<int> zeroExpression = new ValueExpression<int> (0, new FilePosition ());
		/*
        static public int Fold(IExpression<int> expression)
        {
            CanCauseError<int> result = TryFold(expression);
            if (result.CausedError)
            {
                throw new ArgumentException();
            } else
            {
                return result.Result;
            }
        }*/
		static public CanCauseError<int> TryFold (IExpression<int> expression, Func<string, int?> symbolVals = null)
		{
			BinaryOperator<int> op = expression as BinaryOperator<int>;
			Func<int, int, int> func;
			switch (expression.Type) {

			case EAExpressionType.Value:
				return ((ValueExpression<int>)expression).Value;
			
			case EAExpressionType.Division:
				func = (x, y) => x / y;
				break;
			
			case EAExpressionType.Multiply:
				func = (x, y) => x * y;
				break;
			
			case EAExpressionType.Modulus:
				func = (x, y) => x % y;
				break;
			
			case EAExpressionType.Minus:
				func = (x, y) => x - y;
				break;
			
			case EAExpressionType.Sum:
				func = (x, y) => x + y;
				break;
			
			case EAExpressionType.XOR:
				func = (x, y) => x ^ y;
				break;
			
			case EAExpressionType.AND:
				func = (x, y) => x & y;
				break;
			
			case EAExpressionType.OR:
				func = (x, y) => x | y;
				break;
			
			case EAExpressionType.LeftShift:
				func = (x, y) => x << y;
				break;
			
			case EAExpressionType.RightShift:
				func = (x, y) => (int)(((uint)x) >> y);
				break;
			
			case EAExpressionType.ArithmeticRightShift:
				func = (x, y) => x >> y;
				break;
                
			case EAExpressionType.Symbol:
				string name = ((Symbol<int>)expression).Name;
				int? val = symbolVals != null ? symbolVals (name) : null;
				return val != null ? val
                        : CanCauseError<int>.Error ("Symbol {0} isn't in scope", name);
				
			default:
				return CanCauseError<int>.Error ("Unsupported type: {0}", expression.Type);
			}
            
			return func.Map (
				TryFold ((op.First ?? zeroExpression), symbolVals),
				TryFold ((op.Second ?? zeroExpression), symbolVals)
			);
		}

		static public CanCauseError<int> Fold (IExpression<int> expression, Func<string, int?> symbolVals)
		{
			return TryFold (expression, symbolVals);
		}
	}
}
