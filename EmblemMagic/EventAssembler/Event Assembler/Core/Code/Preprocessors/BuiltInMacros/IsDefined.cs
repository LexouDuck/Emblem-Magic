// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros.IsDefined
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Collections;
using System;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros
{
  public struct IsDefined : IMacro, IEquatable<IMacro>
  {
    private IDefineCollection defCol;

    public IsDefined(IDefineCollection defCol)
    {
      this.defCol = defCol;
    }

    public bool IsCorrectAmountOfParameters(int amount)
    {
      return amount == 1;
    }

    public string Replace(string[] parameters)
    {
      return this.defCol.ContainsName(parameters[0]) ? "1" : "0";
    }

    public bool Equals(IMacro other)
    {
      return this.GetType() == other.GetType();
    }

    public bool ShouldPreprocessParameter(int index)
    {
      // This is important
      return false;
    }
  }
}
