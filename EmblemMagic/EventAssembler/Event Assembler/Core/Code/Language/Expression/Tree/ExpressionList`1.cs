// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree.ExpressionList`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Collections.Trees;
using Nintenlord.IO;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree
{
  public sealed class ExpressionList<T> : IExpression<T>, ITree<IExpression<T>>
  {
    private readonly FilePosition filePosition;
    private readonly List<IExpression<T>> expressions;

    public int ComponentCount
    {
      get
      {
        return this.expressions.Count;
      }
    }

    public IExpression<T> this[int index]
    {
      get
      {
        return this.expressions[index];
      }
    }

    public EAExpressionType Type
    {
      get
      {
        return EAExpressionType.List;
      }
    }

    public FilePosition Position
    {
      get
      {
        return this.filePosition;
      }
    }

    public ExpressionList(IEnumerable<IExpression<T>> expressions, FilePosition startPosition)
    {
      this.expressions = new List<IExpression<T>>(expressions);
      this.filePosition = startPosition;
    }

    public IEnumerable<IExpression<T>> GetChildren()
    {
      return (IEnumerable<IExpression<T>>) this.expressions;
    }

    public override string ToString()
    {
      return this.expressions.ToElementWiseString<IExpression<T>>(", ", "[", "]");
    }
  }
}
