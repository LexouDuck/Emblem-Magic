// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives.Define
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using Nintenlord.Event_Assembler.Core.IO.Input;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
  internal class Define : IDirective, INamed<string>, IParameterized
  {
    private static Dictionary<char, char> uniters = new Dictionary<char, char>();
    private static List<char> macroSeparators = new List<char>();
    private static List<char> parameterSeparators = new List<char>();

    public string Name
    {
      get
      {
        return "define";
      }
    }

    public bool RequireIncluding
    {
      get
      {
        return true;
      }
    }

    public int MinAmountOfParameters
    {
      get
      {
        return 1;
      }
    }

    public int MaxAmountOfParameters
    {
      get
      {
        return 2;
      }
    }

    static Define()
    {
      Define.uniters['"'] = '"';
      Define.uniters['('] = ')';
      Define.uniters['['] = ']';
      Define.macroSeparators.Add(',');
      Define.parameterSeparators.AddRange((IEnumerable<char>) " \t");
    }

    public CanCauseError Apply(string[] parameters, IDirectivePreprocessor host)
    {
      CanCauseError canCauseError;
      if (parameters.Length > 1)
      {
        int length = parameters[0].IndexOf('(');
        int num = parameters[0].LastIndexOf(')');
        string[] strArray;
        string name;
        if (length != -1 && num != -1 && length < num)
        {
          strArray = parameters[0].Substring(length + 1, num - length - 1).Split(macroSeparators, uniters);
          name = parameters[0].Substring(0, length);
        }
        else
        {
          strArray = new string[0];
          name = parameters[0];
        }
        for (int index = 0; index < strArray.Length; ++index)
          strArray[index] = strArray[index].Trim();
        if (name.Equals(parameters[1]))
          canCauseError = CanCauseError.Error("Defining something as itself. ");
        else if (!host.DefCol.IsValidName(name))
          canCauseError = CanCauseError.Error(name + " is not valid name to define.");
        else if (host.IsValidToDefine(name))
        {
          canCauseError = CanCauseError.Error(name + " cannot be redefined.");
        }
        else
        {
          if(host.DefCol.ContainsName(name, strArray)) {
            host.Log.AddWarning(host.Input.GetPositionString() + ", Warning: Redefining " + name);
            host.DefCol.Remove(name, strArray);
          }
          
          host.DefCol.Add(name, parameters[1].Trim('"'), strArray);
          canCauseError = CanCauseError.NoError;
        }
      }
      else if (parameters.Length == 1)
      {
        host.DefCol.Add(parameters[0], "");
        canCauseError = CanCauseError.NoError;
      }
      else
        canCauseError = CanCauseError.NoError;
      return canCauseError;
    }
  }
}
