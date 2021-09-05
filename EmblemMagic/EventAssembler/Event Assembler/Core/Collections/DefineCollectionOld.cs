// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Collections.DefineCollectionOld
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility;
using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Collections
{
  internal class DefineCollectionOld : IDefineCollection
  {
    private List<string> originals;
    private List<string> replacers;
    private List<string[]> parameters;

    private int Count
    {
      get
      {
        return this.originals.Count;
      }
    }

    public DefineCollectionOld()
    {
      this.originals = new List<string>();
      this.replacers = new List<string>();
      this.parameters = new List<string[]>();
    }

    public DefineCollectionOld(int capacity)
    {
      this.originals = new List<string>(capacity);
      this.replacers = new List<string>(capacity);
      this.parameters = new List<string[]>(capacity);
    }

    public DefineCollectionOld(string[] predefined)
    {
      this.originals = new List<string>();
      this.replacers = new List<string>();
      this.parameters = new List<string[]>();
      this.originals.AddRange((IEnumerable<string>) predefined);
      for (int index = 0; index < predefined.Length; ++index)
      {
        this.replacers.Add(string.Empty);
        this.parameters.Add(new string[0]);
      }
    }

    public void Add(string original, string replacer)
    {
      this.originals.Add(original);
      this.replacers.Add(replacer);
      this.parameters.Add(new string[0]);
    }

    public void Add(string original, string replacer, params string[] parameters)
    {
      this.originals.Add(original);
      this.replacers.Add(replacer);
      this.parameters.Add(parameters);
    }

    public void AddRange(string[] original)
    {
      this.originals.AddRange((IEnumerable<string>) original);
      this.replacers.AddRange((IEnumerable<string>) new string[original.Length]);
      for (int index = 0; index < original.Length; ++index)
        this.parameters.Add(new string[0]);
    }

    public bool ContainsName(string item)
    {
      return this.GetIndex(item, 0) != -1;
    }

    public bool ContainsName(string item, params string[] parameters)
    {
      return this.GetIndex(item, parameters.Length) != -1;
    }

    private int GetIndex(string item, int numberOfParameters)
    {
      bool flag = false;
      int index = this.originals.IndexOf(item);
      while (!flag && index >= 0)
      {
        if (this.parameters[index].Length == numberOfParameters)
          flag = true;
        else
          index = this.originals.IndexOf(item, index + 1);
      }
      return index;
    }

    public string GetReplacer(string item)
    {
      return this.GetReplacer(item, new string[0]);
    }

    public string GetReplacer(string item, string[] parameters)
    {
      int index;
      if ((index = this.GetIndex(item, parameters.Length)) > -1)
        return string.Copy(this.replacers[index]);
      return (string) null;
    }

    public KeyValuePair<string, string[]> GetReplacerAndParameters(string item, string[] parameters)
    {
      int index;
      if ((index = this.GetIndex(item, parameters.Length)) > -1)
        return new KeyValuePair<string, string[]>(this.replacers[index], this.parameters[index]);
      return new KeyValuePair<string, string[]>((string) null, (string[]) null);
    }

    public void Remove(string original)
    {
      int index = this.originals.IndexOf(original);
      if (index < 0)
        return;
      this.originals.RemoveAt(index);
      this.replacers.RemoveAt(index);
      this.parameters.RemoveAt(index);
    }

    public bool ApplyDefines(string original, out string newString)
    {
      int index = this.originals.IndexOf(original);
      newString = index >= 0 ? this.replacers[index] : original;
      return true;
    }

    public void Remove(string original, params string[] parameters)
    {
      int index = this.GetIndex(original, parameters.Length);
      if (index < 0)
        return;
      this.originals.RemoveAt(index);
      this.replacers.RemoveAt(index);
      this.parameters.RemoveAt(index);
    }

    public bool IsValidName(string name)
    {
      throw new NotImplementedException();
    }

    public CanCauseError<string> ApplyDefines(string original)
    {
      throw new NotImplementedException();
    }
  }
}
