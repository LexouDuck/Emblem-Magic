// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.StringReplacers.NewReplacer
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Preprocessors;
using Nintenlord.Event_Assembler.Core.Collections;
using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Code.StringReplacers
{
  internal sealed class NewReplacer : IStringReplacer
  {
    private IDictionary<string, IDictionary<int, IMacro>> values;
    private IDictionary<string, IMacro> builtInValues;
    private int maxIter;
    private int currentIter;

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
      StringBuilder textToEdit = new StringBuilder(s);
      bool causedError = this.Replace(textToEdit).CausedError;
      newString = textToEdit.ToString();
      --this.currentIter;
      return causedError;
    }

    public CanCauseError<string> Replace(string textToEdit)
    {
      StringBuilder textToEdit1 = new StringBuilder(textToEdit);

      CanCauseError canCauseError = this.Replace(textToEdit1);

      if (canCauseError.CausedError)
        return CanCauseError<string>.Error(canCauseError.ToString());

      return CanCauseError<string>.NoError(textToEdit1.ToString());
    }

    public CanCauseError Replace(StringBuilder textToEdit)
    {
      if (currentIter == maxIter)
        return CanCauseError.Error("Maximum amount of replacement iterations exceeeded while applying macro.");

      ++currentIter;

      foreach (KeyValuePair<int, Tuple<int, IMacro, string[]>> macro in FindMacros(textToEdit))
      {
        string[] strArray = macro.Value.Item3;
        string[] parameters = new string[strArray.Length];

        for (int index = 0; index < strArray.Length; ++index)
        {
          if (macro.Value.Item2.ShouldPreprocessParameter(index))
          {
            CanCauseError<string> canCauseError = this.Replace(strArray[index]);

            if (canCauseError.CausedError)
              return (CanCauseError)canCauseError;

            parameters[index] = canCauseError.Result;
          }
          else
          {
            parameters[index] = strArray[index];
          }
        }

        CanCauseError<string> canCauseError1 = this.Replace(macro.Value.Item2.Replace(parameters));

        if (canCauseError1.CausedError)
          return (CanCauseError)canCauseError1;

        string @string = textToEdit.Substring(macro.Key, macro.Value.Item1).ToString();
        textToEdit.Replace(@string, canCauseError1.Result, macro.Key, @string.Length);
      }

      --currentIter;
      return CanCauseError.NoError;
    }

    private SortedDictionary<int, Tuple<int, IMacro, string[]>> FindMacros(StringBuilder s)
    {
      SortedDictionary<int, Tuple<int, IMacro, string[]>> replace = new SortedDictionary<int, Tuple<int, IMacro, string[]>>(ReverseComparer<int>.Default);
      StringBuilder stringBuilder = new StringBuilder();
      int i = 0;
      bool inQuotes = false;
      while (i < s.Length)
      {
        if (s[i] == '"')
        {
          inQuotes = !inQuotes;
          ++i;
        }
        else if (inQuotes || DefineCollectionOptimized.IsValidCharacter(s[i]))
        {
          stringBuilder.Append(s[i]);
          ++i;
        }
        else if (stringBuilder.Length > 0)
        {
          this.GetMacroData(s, replace, ref i, stringBuilder.ToString());
          stringBuilder.Clear();
        }
        else
          ++i;
      }
      int length = s.Length;
      this.GetMacroData(s, replace, ref length, stringBuilder.ToString());
      return replace;
    }

    private void GetMacroData(StringBuilder s, SortedDictionary<int, Tuple<int, IMacro, string[]>> replace, ref int i, string name)
    {
      IMacro macro;
      bool flag1 = this.builtInValues.TryGetValue(name, out macro);
      IDictionary<int, IMacro> dictionary;
      bool flag2 = this.values.TryGetValue(name, out dictionary);
      if (flag2 || flag1)
      {
        int lengthInString;
        string[] strArray;
        if (i < s.Length && (int)s[i] == 40)
        {
          strArray = NewReplacer.GetParameters(s, i, out lengthInString);
        }
        else
        {
          lengthInString = 0;
          strArray = new string[0];
        }
        if (flag1 && macro.IsCorrectAmountOfParameters(strArray.Length) || flag2 && dictionary.TryGetValue(strArray.Length, out macro))
          replace[i - name.Length] = new Tuple<int, IMacro, string[]>(lengthInString + name.Length, macro, strArray);
        i += lengthInString;
      }
      else
        ++i;
    }

    private static bool ContainsAt(StringBuilder s, int index, string toSearch)
    {
      bool flag = true;
      if (toSearch.Length > s.Length - index)
      {
        flag = false;
      }
      else
      {
        for (int index1 = 0; index1 < toSearch.Length; ++index1)
        {
          if ((int)s[index + index1] != (int)toSearch[index1])
          {
            flag = false;
            break;
          }
        }
      }
      return flag;
    }

    private static int GetParameterLength(StringBuilder s, int index, out int parameters)
    {
      int num = 1;
      parameters = 1;
      int index1;
      for (index1 = index + 1; index1 < s.Length && num != 0; ++index1)
      {
        switch (s[index1])
        {
          case '(':
            ++num;
            break;
          case ')':
            --num;
            break;
          case ',':
            if (num == 1)
            {
              ++parameters;
              break;
            }
            break;
        }
      }
      return index1 - index;
    }

    private static string[] GetParameters(StringBuilder s, int index)
    {
      int lengthInString;
      return NewReplacer.GetParameters(s, index, out lengthInString);
    }

    private static string[] GetParameters(StringBuilder s, int index, out int lengthInString)
    {
      List<string> stringList = new List<string>();
      int num1 = 1;
      int num2 = 0;
      StringBuilder stringBuilder = new StringBuilder();
      int index1;
      for (index1 = index + 1; index1 < s.Length && num1 > 0; ++index1)
      {
        switch (s[index1])
        {
          case '(':
            ++num1;
            stringBuilder.Append(s[index1]);
            break;
          case ')':
            --num1;
            stringBuilder.Append(s[index1]);
            break;
          case ',':
            if (num1 == 1 && num2 == 0)
            {
              stringList.Add(stringBuilder.ToString());
              stringBuilder.Clear();
              break;
            }
            stringBuilder.Append(s[index1]);
            break;
          case '[':
            ++num2;
            stringBuilder.Append(s[index1]);
            break;
          case ']':
            --num2;
            stringBuilder.Append(s[index1]);
            break;
          default:
            stringBuilder.Append(s[index1]);
            break;
        }
      }
      if (stringBuilder.Length > 0)
        stringList.Add(stringBuilder.ToString(0, stringBuilder.Length - 1));
      lengthInString = index1 - index;
      for (int index2 = 0; index2 < stringList.Count; ++index2)
        stringList[index2] = stringList[index2].Trim();
      return stringList.ToArray();
    }
  }
}
