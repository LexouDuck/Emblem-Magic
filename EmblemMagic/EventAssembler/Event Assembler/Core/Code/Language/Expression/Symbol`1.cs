// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Expression.Symbol`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections.Trees;
using Nintenlord.IO;
using Nintenlord.Utility;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language.Expression
{
  public sealed class Symbol<T> : IExpression<T>, ITree<IExpression<T>>, INamed<string>
  {
    private readonly string name;
    private readonly FilePosition position;

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public FilePosition Position
    {
      get
      {
        return this.position;
      }
    }

    public EAExpressionType Type
    {
      get
      {
        return EAExpressionType.Symbol;
      }
    }

    public Symbol(string name, FilePosition position)
    {
      this.name = name;
      this.position = position;
    }

    public IEnumerable<IExpression<T>> GetChildren()
    {
      yield break;
    }

    public override string ToString()
    {
      return this.name;
    }
  }
}
