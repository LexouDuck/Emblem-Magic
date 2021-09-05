// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.ScopeStructure`1
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections.Trees;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Utility;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code
{
  public sealed class ScopeStructure<T> : ITree<ScopeStructure<T>>
  {
    private readonly ScopeStructure<T> ParentScope;
    private List<ScopeStructure<T>> childScopes;
    private Dictionary<string, IExpression<T>> definedSymbols;
    
    public ScopeStructure(ScopeStructure<T> parentScope)
    {
      this.ParentScope = parentScope;
      this.childScopes = new List<ScopeStructure<T>>();
      this.definedSymbols = new Dictionary<string, IExpression<T>>();
    }

    public void AddChildScope(ScopeStructure<T> newChildScope)
    {
      this.childScopes.Add(newChildScope);
    }

    public CanCauseError<IExpression<T>> GetSymbolValue(string symbol)
    {
	  IExpression<T> result;
      if (definedSymbols.TryGetValue(symbol, out result))
        return CanCauseError<IExpression<T>>.NoError(result);

      if (ParentScope != null)
        return ParentScope.GetSymbolValue(symbol);

      return CanCauseError<IExpression<T>>.Error("Symbol {0} not defined", symbol);
    }

    public CanCauseError AddNewSymbol(string symbol, IExpression<T> value)
    {
      if (definedSymbols.ContainsKey(symbol))
        return CanCauseError.Error("Symbol \"{0}\" already exists (ignoring second definition).", symbol);

      definedSymbols[symbol] = value;
      return CanCauseError.NoError;
    }

    public bool IsGlobalScope()
    {
      return (ParentScope == null);
    }

    public IEnumerable<KeyValuePair<string, IExpression<T>>> GetSymbols()
    {
      return definedSymbols;
    }

    public IEnumerable<ScopeStructure<T>> GetChildren()
    {
      return childScopes;
    }
  }
}
