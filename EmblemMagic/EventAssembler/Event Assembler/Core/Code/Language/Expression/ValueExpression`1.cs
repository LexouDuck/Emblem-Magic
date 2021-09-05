// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.ValueExpression`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections.Trees;
using Nintenlord.IO;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression
{
  internal sealed class ValueExpression<T> : IExpression<T>, ITree<IExpression<T>>
  {
    private readonly FilePosition position;

    public T Value { get; private set; }

    public EAExpressionType Type
    {
      get
      {
        return EAExpressionType.Value;
      }
    }

    public FilePosition Position
    {
      get
      {
        return this.position;
      }
    }

    public ValueExpression(T value, FilePosition position)
    {
      this.Value = value;
      this.position = position;
    }

    public IEnumerable<IExpression<T>> GetChildren()
    {
      yield break;
    }

    public override string ToString()
    {
      return this.Value.ToString();
    }
  }
}
