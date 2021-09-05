// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros.Pool
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros
{
  public class Pool : IMacro, IEquatable<IMacro>
  {
    private int used = 1;
    private IDictionary<string, string> lines;

    public int AmountOfLines
    {
      get
      {
        return this.lines.Count;
      }
    }

    public Pool()
    {
      this.lines = (IDictionary<string, string>) new Dictionary<string, string>();
    }

    public bool IsCorrectAmountOfParameters(int amount)
    {
      if (amount != 1)
        return amount == 2;
      return true;
    }

    public string Replace(string[] parameters)
    {
      string index;
      switch (parameters.Length)
      {
        case 1:
          index = "poolLabel" + this.used.ToString();
          ++this.used;
          break;
        case 2:
          index = parameters[1];
          break;
        default:
          index = "";
          break;
      }
      this.lines[index] = parameters[0];
      return index;
    }

    public bool Equals(IMacro other)
    {
      return this == other;
    }

    public void DumpPool(TextWriter output)
    {
      foreach (KeyValuePair<string, string> line in (IEnumerable<KeyValuePair<string, string>>) this.lines)
        output.WriteLine(string.Format("{0}:\n {1}", (object) line.Key, (object) line.Value));
      this.lines.Clear();
    }

    public string[] DumpPool()
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, string> line in (IEnumerable<KeyValuePair<string, string>>) this.lines)
      {
        string str1 = string.Format("{0}:\n {1}", (object) line.Key, (object) line.Value);
        char[] chArray = new char[1]{ ';' };
        foreach (string str2 in str1.Split(chArray))
          stringList.Add(str2);
      }
      this.lines.Clear();
      return stringList.ToArray();
    }

    public bool ShouldPreprocessParameter(int index)
    {
      return true;
    }
  }
}
