// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Collections.DefineCollectionOptimized
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Preprocessors;
using Nintenlord.Event_Assembler.Core.Code.StringReplacers;
using Nintenlord.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Collections
{
  internal sealed class DefineCollectionOptimized : IDefineCollection
  {
    private IStringReplacer replacer;
    private IDictionary<string, IDictionary<int, IMacro>> values;
    private IDictionary<string, IMacro> builtInValues;

    public IMacro this[string name]
    {
      set
      {
        this.builtInValues[name] = value;
      }
    }

    public DefineCollectionOptimized()
      : this((IStringReplacer)new NewReplacer(), new Dictionary<string, IMacro>())
    {
    }

    public DefineCollectionOptimized(IStringReplacer replacer)
      : this(replacer, new Dictionary<string, IMacro>())
    {
    }

    public DefineCollectionOptimized(Dictionary<string, IMacro> builtInMacros)
      : this((IStringReplacer)new NewReplacer(), builtInMacros)
    {
    }

    public DefineCollectionOptimized(IStringReplacer replacer, Dictionary<string, IMacro> builtInMacros)
    {
      this.values = (IDictionary<string, IDictionary<int, IMacro>>)new Dictionary<string, IDictionary<int, IMacro>>();
      this.builtInValues = (IDictionary<string, IMacro>)new Dictionary<string, IMacro>();
      this.replacer = replacer;
      replacer.BuiltInValues = this.builtInValues;
      replacer.Values = this.values;
      replacer.MaxIter = 100;
    }

    public void Add(string original, string replacer, params string[] parameters)
    {
      DefineCollectionOptimized.UserDefinedReplacer userDefinedReplacer = new DefineCollectionOptimized.UserDefinedReplacer(replacer, parameters);
      IDictionary<int, IMacro> dictionary;
      if (!this.values.TryGetValue(original, out dictionary))
      {
        dictionary = (IDictionary<int, IMacro>)new SortedDictionary<int, IMacro>();
        this.values[original] = dictionary;
      }
      dictionary[parameters.Length] = (IMacro)userDefinedReplacer;
    }

    public void Add(string original, string replacer)
    {
      this.Add(original, replacer, new string[0]);
    }

    public bool ContainsName(string item, params string[] parameters)
    {
      IDictionary<int, IMacro> dictionary;
      if (this.values.TryGetValue(item, out dictionary))
        return dictionary.ContainsKey(parameters.Length);
      return this.builtInValues.ContainsKey(item);
    }

    public bool ContainsName(string item)
    {
      return this.ContainsName(item, new string[0]);
    }

    public void Remove(string original)
    {
      this.Remove(original, new string[0]);
    }

    public void Remove(string original, params string[] parameters)
    {
      IDictionary<int, IMacro> dictionary;
      if (!this.values.TryGetValue(original, out dictionary))
        return;
      dictionary.Remove(parameters.Length);
    }

    public bool ApplyDefines(string s, out string newString)
    {
      return this.replacer.Replace(s, out newString);
    }

    public CanCauseError<string> ApplyDefines(string original)
    {
      return this.replacer.Replace(original);
    }

    public bool IsValidName(string name)
    {
      if (name.Length > 0)
        return name.All<char>(new Func<char, bool>(DefineCollectionOptimized.IsValidCharacter));
      return false;
    }

    public static bool IsValidCharacter(char c)
    {
      if (!char.IsLetterOrDigit(c))
        return (int)c == 95;
      return true;
    }

    private void GetSubstring(string s, int i, int length, StringBuilder output)
    {
      output.Remove(0, output.Length);
      for (int index = i; index < length; ++index)
        output.Append(s[index]);
    }

    public CanCauseError<string> ApplyPreprocessorDefines(string original)
    {
      IStringReplacer tempreplacer = new NewReplacer
      {
        MaxIter = 100,
        BuiltInValues = this.builtInValues,
        Values = new Dictionary<string, IDictionary<int, IMacro>>()
      };

      return tempreplacer.Replace(original);
    }

    private struct UserDefinedReplacer : IEquatable<DefineCollectionOptimized.UserDefinedReplacer>, IMacro, IEquatable<IMacro>
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

      public UserDefinedReplacer(string toReplaceWith, string[] parameters)
      {
        this.toReplaceWith = toReplaceWith;
        this.parameters = parameters;
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

      public bool IsCorrectAmountOfParameters(int amount)
      {
        return this.AmountOfParameters == amount;
      }

      public bool Equals(DefineCollectionOptimized.UserDefinedReplacer other)
      {
        if (this.toReplaceWith.Equals(other.toReplaceWith))
          return this.parameters.Length == other.parameters.Length;
        return false;
      }

      public bool Equals(IMacro other)
      {
        if (other is DefineCollectionOptimized.UserDefinedReplacer)
          return this.Equals(other);
        return false;
      }

      public bool ShouldPreprocessParameter(int index)
      {
        return true;
      }
    }
  }
}
