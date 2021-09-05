// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Types.Type
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Types
{
  public sealed class Type
  {
    public static readonly Type Atom = new Type(MetaType.Atom);
    private static Dictionary<int, Type> vectorTypes = new Dictionary<int, Type>();
    public readonly MetaType type;
    public readonly Type[] vectorParameterTypes;

    public int ParameterCount
    {
      get
      {
        return this.vectorParameterTypes.Length;
      }
    }

    private Type(MetaType type)
    {
      this.type = type;
      this.vectorParameterTypes = (Type[]) null;
    }

    private Type(Type[] parameters)
    {
      this.type = MetaType.Vector;
      this.vectorParameterTypes = parameters;
    }

    public static Type Vector(int paramCount)
    {
      Type type;
      if (!Type.vectorTypes.TryGetValue(paramCount, out type))
        type = Type.vectorTypes[paramCount] = new Type(Type.Repeat<Type>(paramCount, Type.Atom).ToArray<Type>());
      return type;
    }

    public static IEnumerable<T> Repeat<T>(int count, T toRepeat)
    {
      for (int i = 0; i < count; ++i)
        yield return toRepeat;
    }

    public static Type GetType<T>(IExpression<T> typed)
    {
      if (Type.IsAtom<T>(typed))
        return Type.GetAtomicType<T>(typed);
      Type[] array = typed.GetChildren().Select<IExpression<T>, Type>((Func<IExpression<T>, Type>) (x => Type.GetType<T>(x))).ToArray<Type>();
      if (!((IEnumerable<Type>) array).All<Type>((Func<Type, bool>) (x => x.type == MetaType.Atom)))
        return new Type(array);
      Type type;
      if (!Type.vectorTypes.TryGetValue(array.Length, out type))
        type = Type.vectorTypes[array.Length] = new Type(array);
      return type;
    }

    private static bool IsAtom<T>(IExpression<T> typed)
    {
      switch (typed.Type)
      {
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
          return true;
        case EAExpressionType.List:
          return false;
        default:
          throw new ArgumentException();
      }
    }

    private static Type GetAtomicType<T>(IExpression<T> typed)
    {
      return Type.Atom;
    }

    public override string ToString()
    {
      if (this.type == MetaType.Vector)
        return ((IEnumerable<Type>) this.vectorParameterTypes).ToElementWiseString<Type>(", ", "[", "]");
      return this.type.ToString();
    }
  }
}
