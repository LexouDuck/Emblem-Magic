// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.Code`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Collections.Trees;
using Nintenlord.IO;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression
{
  internal sealed class Code<T> : IExpression<T>, ITree<IExpression<T>>
  {
    private readonly Symbol<T> codeName;
    private readonly IExpression<T>[] parameters;
    private readonly FilePosition position;

    public Symbol<T> CodeName
    {
      get
      {
        return this.codeName;
      }
    }

    public IExpression<T>[] Parameters
    {
      get
      {
        return this.parameters;
      }
    }

    public IExpression<T> this[int index]
    {
      get
      {
        return this.parameters[index];
      }
    }

    public FilePosition Position
    {
      get
      {
        return this.position;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this.codeName == null;
      }
    }

    public int ParameterCount
    {
      get
      {
        return this.parameters.Length;
      }
    }

    public EAExpressionType Type
    {
      get
      {
        return EAExpressionType.Code;
      }
    }

    private Code(FilePosition position, Symbol<T> codeName, List<IExpression<T>> parameters)
    {
      this.codeName = codeName;
      this.parameters = parameters.ToArray();
      this.position = position;
    }

    public Code(Symbol<T> codeName, List<IExpression<T>> parameters)
    {
      this.codeName = codeName;
      this.parameters = parameters.ToArray();
      this.position = codeName.Position;
    }

    public IEnumerable<IExpression<T>> GetChildren()
    {
      yield return (IExpression<T>) this.codeName;
      foreach (IExpression<T> parameter in this.parameters)
        yield return parameter;
    }

    public override string ToString()
    {
      return this.codeName.ToString() + ((IEnumerable<IExpression<T>>) this.parameters).ToElementWiseString<IExpression<T>>(" ", " ", "");
    }

    public static Nintenlord.Event_Assembler.Core.Code.Language.Expression.Code<T> EmptyCode(FilePosition position)
    {
      return new Nintenlord.Event_Assembler.Core.Code.Language.Expression.Code<T>(position, (Symbol<T>) null, new List<IExpression<T>>());
    }
  }
}
