// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Collections.DefineCollection
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Collections
{
  internal class DefineCollection : IDefineCollection, IEnumerable<KeyValuePair<string, int>>, IEnumerable
  {
    private readonly int maxIter = 100;
    private Dictionary<KeyValuePair<string, int>, DefineCollection.Replacer> values;
    private HashSet<string> originals;

    public DefineCollection()
    {
      this.values = new Dictionary<KeyValuePair<string, int>, DefineCollection.Replacer>();
      this.originals = new HashSet<string>();
    }

    public void Add(string original, string replacer, params string[] parameters)
    {
      DefineCollection.Replacer replacer1 = new DefineCollection.Replacer(replacer, parameters);
      this.values[new KeyValuePair<string, int>(original, parameters.Length)] = replacer1;
      this.originals.Add(original);
    }

    public void Add(string original, string replacer)
    {
      DefineCollection.Replacer replacer1 = new DefineCollection.Replacer(replacer, new string[0]);
      this.values.Add(new KeyValuePair<string, int>(original, 0), replacer1);
      this.originals.Add(original);
    }

    public bool ContainsName(string item, params string[] parameters)
    {
      return this.values.ContainsKey(new KeyValuePair<string, int>(item, parameters.Length));
    }

    public bool ContainsName(string item)
    {
      foreach (KeyValuePair<KeyValuePair<string, int>, DefineCollection.Replacer> keyValuePair in this.values)
      {
        if (keyValuePair.Key.Key.Equals(item) && keyValuePair.Value.AmountOfParameters == 0)
          return true;
      }
      return false;
    }

    public string GetReplacer(string item, string[] parameters)
    {
      foreach (KeyValuePair<KeyValuePair<string, int>, DefineCollection.Replacer> keyValuePair in this.values)
      {
        if (keyValuePair.Key.Key.Equals(item) && keyValuePair.Value.AmountOfParameters == parameters.Length)
          return keyValuePair.Value.GetReplacer();
      }
      throw new KeyNotFoundException();
    }

    public string GetReplacer(string item)
    {
      foreach (KeyValuePair<KeyValuePair<string, int>, DefineCollection.Replacer> keyValuePair in this.values)
      {
        if (keyValuePair.Key.Key.Equals(item) && keyValuePair.Value.AmountOfParameters == 0)
          return keyValuePair.Value.GetReplacer();
      }
      throw new KeyNotFoundException();
    }

    public void Remove(string original)
    {
      this.values.Remove(new KeyValuePair<string, int>(original, 0));
      this.originals.Remove(original);
    }

    public void Remove(string original, params string[] parameters)
    {
      this.values.Remove(new KeyValuePair<string, int>(original, parameters.Length));
      this.originals.Remove(original);
    }

    public bool ApplyDefines(string s, out string newString)
    {
      StringBuilder stringBuilder = new StringBuilder(s);
      SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>((IComparer<int>) new LamdaComparer<int>((Func<int, int, int>) ((x, y) => y - x)));
      for (int index1 = 0; this.GetContainedOriginals(stringBuilder.ToString(), (IDictionary<int, string>) sortedDictionary) > 0 && index1 < this.maxIter; ++index1)
      {
        foreach (KeyValuePair<int, string> keyValuePair in sortedDictionary)
        {
          string key = keyValuePair.Value;
          int startIndex = keyValuePair.Key + key.Length;
          string[] parameters;
          string str;
          if (startIndex < stringBuilder.Length && (int) stringBuilder[startIndex] == 40)
          {
            int num = 1;
            int index2 = startIndex;
            while (num > 0 && ++index2 < s.Length)
            {
              if ((int) stringBuilder[index2] == 41)
                --num;
              else if ((int) stringBuilder[index2] == 40)
                ++num;
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
          DefineCollection.Replacer replacer;
          if (this.values.TryGetValue(new KeyValuePair<string, int>(key, parameters.Length), out replacer))
          {
            string oldValue = key + str;
            string newValue = replacer.Replace(parameters);
            stringBuilder.Replace(oldValue, newValue, keyValuePair.Key, oldValue.Length);
          }
        }
        sortedDictionary.Clear();
      }
      newString = stringBuilder.ToString();
      return sortedDictionary.Count == 0;
    }

    public CanCauseError<string> ApplyDefines(string original)
    {
      throw new NotImplementedException();
    }

    public bool IsValidName(string name)
    {
      if (name.Length > 0)
        return name.All<char>(new Func<char, bool>(this.IsValidCharacter));
      return false;
    }

    private bool IsValidCharacter(char c)
    {
      if (!char.IsLetterOrDigit(c))
        return (int) c == 95;
      return true;
    }

    private int GetContainedOriginals(string s, IDictionary<int, string> containedOriginals)
    {
      foreach (string original in this.originals)
      {
        for (int key = s.IndexOf(original); key >= 0; key = s.IndexOf(original, key + 1))
        {
          if ((key <= 0 || !this.IsValidCharacter(s[key - 1])) && (key + original.Length >= s.Length || !this.IsValidCharacter(s[key + original.Length])) && !containedOriginals.ContainsKey(key))
          {
            containedOriginals[key] = original;
            break;
          }
        }
      }
      return containedOriginals.Count;
    }

    public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, int>>) this.values.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    private struct Replacer : IEquatable<DefineCollection.Replacer>
    {
      private string toReplaceWith;
      private string[] parameters;

      public int AmountOfParameters
      {
        get
        {
          return this.parameters.Length;
        }
      }

      public Replacer(string toReplaceWith, string[] parameters)
      {
        this.toReplaceWith = toReplaceWith;
        this.parameters = parameters;
      }

      public static explicit operator KeyValuePair<string, string[]>(DefineCollection.Replacer item)
      {
        return new KeyValuePair<string, string[]>(item.toReplaceWith, item.parameters);
      }

      public string GetReplacer()
      {
        return this.toReplaceWith;
      }

      public string Replace(string[] parameters)
      {
        if (parameters.Length != this.parameters.Length)
          throw new ArgumentException();
        StringBuilder stringBuilder = new StringBuilder(this.toReplaceWith);
        for (int index = 0; index < parameters.Length; ++index)
          stringBuilder.Replace(this.parameters[index], parameters[index].Trim());
        return stringBuilder.ToString();
      }

      public bool Equals(DefineCollection.Replacer other)
      {
        if (this.toReplaceWith.Equals(other.toReplaceWith))
          return this.parameters.Length == other.parameters.Length;
        return false;
      }
    }
  }
}
