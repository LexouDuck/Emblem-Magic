// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree.LabeledExpression`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections.Trees;
using Nintenlord.IO;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree
{
  internal sealed class LabelExpression<T> : IExpression<T>
  {
    private readonly string labelName;
    private readonly FilePosition position;

    public string LabelName
    {
      get
      {
        return this.labelName;
      }
    }

    public EAExpressionType Type
    {
      get
      {
        return EAExpressionType.Labeled;
      }
    }

    public FilePosition Position
    {
      get
      {
        return this.position;
      }
    }

    public LabelExpression(FilePosition position, string labelName)
    {
      this.position = position;
      this.labelName = labelName;
    }

    public IEnumerable<IExpression<T>> GetChildren()
    {
      yield break;
    }
  }
}
