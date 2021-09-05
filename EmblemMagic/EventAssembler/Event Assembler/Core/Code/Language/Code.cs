// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.Code
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Templates;
using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language
{
  internal class Code
  {
    private readonly ICodeTemplate template;
    private readonly string[] text;
    private readonly int length;
    private readonly int offset;

    public ICodeTemplate Template
    {
      get
      {
        return this.template;
      }
    }

    public string[] Text
    {
      get
      {
        return this.text;
      }
    }

    public int Length
    {
      get
      {
        return this.length;
      }
    }

    public int Offset
    {
      get
      {
        return this.offset;
      }
    }

    public Code(string[] line, ICodeTemplate template, int length, int offset)
    {
      this.template = template;
      this.text = line;
      this.length = length;
      this.offset = offset;
    }

    public static bool operator ==(Code a, Code b)
    {
      return a.template == b.template;
    }

    public static bool operator !=(Code a, Code b)
    {
      return a.template != b.template;
    }

    public override int GetHashCode()
    {
      return this.template.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Code))
        return false;
      return this == (Code) obj;
    }

    public IEnumerable<Tuple<int, Priority>> GetPointedOffsets()
    {
      CodeTemplate templ = this.template as CodeTemplate;
      if (templ != null)
      {
        for (int i = 0; i < templ.AmountOfParams; ++i)
        {
          if (templ[i].pointer)
            yield return Tuple.Create<int, Priority>(this.text[i + 1].GetValue(), templ[i].pointedPriority);
        }
      }
    }

    public string[] ReplaceOffsetsWithLables(IDictionary<int, string> lables)
    {
      string[] strArray = new string[this.text.Length];
      Array.Copy((Array) this.text, (Array) strArray, this.text.Length);
      for (int index = 1; index < strArray.Length; ++index)
      {
        int key;
        string str;
        if (strArray[index].TryGetValue(out key) && lables.TryGetValue(key, out str))
          strArray[index] = str;
      }
      return strArray;
    }
  }
}
