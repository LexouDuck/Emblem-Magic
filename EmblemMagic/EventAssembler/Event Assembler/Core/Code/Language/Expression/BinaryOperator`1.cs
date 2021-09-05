// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.BinaryOperator`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections.Trees;
using Nintenlord.IO;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression
{
  public abstract class BinaryOperator<T> : IExpression<T>, ITree<IExpression<T>>
  {
    private readonly EAExpressionType type;
    private readonly IExpression<T> first;
    private readonly IExpression<T> second;
    private readonly FilePosition position;

    public IExpression<T> Second
    {
      get
      {
        return this.second;
      }
    }

    public IExpression<T> First
    {
      get
      {
        return this.first;
      }
    }

    public EAExpressionType Type
    {
      get
      {
        return this.type;
      }
    }

    public FilePosition Position
    {
      get
      {
        return this.position;
      }
    }

    protected BinaryOperator(IExpression<T> first, IExpression<T> second, EAExpressionType type, FilePosition position)
    {
      this.first = first;
      this.second = second;
      this.type = type;
      this.position = position;
    }

    public IEnumerable<IExpression<T>> GetChildren()
    {
      yield return this.first;
      yield return this.second;
    }

    public override string ToString()
    {
      return string.Format("({0} {2} {1})", (object) this.first, (object) this.second, (object) this.type);
    }
  }
}
