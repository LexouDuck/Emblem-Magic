// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.StringReplacers.OriginalReplacer
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Preprocessors;
using Nintenlord.Event_Assembler.Core.Collections;
using Nintenlord.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Code.StringReplacers
{
  internal sealed class OriginalReplacer : IStringReplacer
  {
    private IDictionary<string, IDictionary<int, IMacro>> values;
    private IDictionary<string, IMacro> builtInValues;
    private int maxIter;

    public IDictionary<string, IDictionary<int, IMacro>> Values
    {
      set
      {
        this.values = value;
      }
    }

    public IDictionary<string, IMacro> BuiltInValues
    {
      set
      {
        this.builtInValues = value;
      }
    }

    public int MaxIter
    {
      set
      {
        this.maxIter = value;
      }
    }

    public bool Replace(string s, out string newString)
    {
      StringBuilder stringBuilder = new StringBuilder(s);
      IDictionary<int, string> containedOriginals = (IDictionary<int, string>) new SortedDictionary<int, string>((IComparer<int>) new ReverseComparer<int>((IComparer<int>) Comparer<int>.Default));
      for (int index1 = 0; this.GetContainedOriginals(stringBuilder.ToString(), containedOriginals) > 0 && index1 < this.maxIter; ++index1)
      {
        foreach (KeyValuePair<int, string> keyValuePair in (IEnumerable<KeyValuePair<int, string>>) containedOriginals)
        {
          string key = keyValuePair.Value;
          int startIndex = keyValuePair.Key + key.Length;
          string[] parameters;
          string str;
          if (startIndex < stringBuilder.Length && (int) stringBuilder[startIndex] == 40)
          {
            int num = 1;
            int index2;
            for (index2 = startIndex + 1; index2 < s.Length; ++index2)
            {
              if ((int) stringBuilder[index2] == 41)
                --num;
              else if ((int) stringBuilder[index2] == 40)
                ++num;
              if (num == 0)
                break;
            }
            parameters = stringBuilder.ToString(startIndex + 1, index2 - startIndex - 1).Split(',');
            str = stringBuilder.ToString(startIndex, index2 - startIndex + 1);
          }
          else
          {
            parameters = new string[0];
            str = "";
          }
          for (int index2 = 0; index2 < parameters.Length; ++index2)
            parameters[index2] = parameters[index2].Trim();
          IMacro macro;
          if (this.builtInValues.TryGetValue(key, out macro) || this.values[key].TryGetValue(parameters.Length, out macro))
          {
            string oldValue = key + str;
            string newValue = macro.Replace(parameters);
            stringBuilder.Replace(oldValue, newValue, keyValuePair.Key, oldValue.Length);
          }
        }
        containedOriginals.Clear();
      }
      newString = stringBuilder.ToString();
      return containedOriginals.Count == 0;
    }

    private int GetContainedOriginals(string s, IDictionary<int, string> containedOriginals)
    {
      foreach (string key in (IEnumerable<string>) this.builtInValues.Keys)
        this.FindString(s, containedOriginals, key);
      foreach (string key in (IEnumerable<string>) this.values.Keys)
        this.FindString(s, containedOriginals, key);
      return containedOriginals.Count;
    }

    private void FindString(string s, IDictionary<int, string> containedOriginals, string toFind)
    {
      for (int key = s.IndexOf(toFind); key >= 0; key = s.IndexOf(toFind, key + 1))
      {
        if ((key <= 0 || !DefineCollectionOptimized.IsValidCharacter(s[key - 1])) && (key + toFind.Length >= s.Length || !DefineCollectionOptimized.IsValidCharacter(s[key + toFind.Length])) && !containedOriginals.ContainsKey(key))
        {
          containedOriginals[key] = toFind;
          break;
        }
      }
    }

    public CanCauseError<string> Replace(string textToEdit)
    {
      throw new NotImplementedException();
    }

    public CanCauseError Replace(StringBuilder textToEdit)
    {
      throw new NotImplementedException();
    }
  }
}
