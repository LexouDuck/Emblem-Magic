// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros.VectorToParameter
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Runtime.InteropServices;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct VectorToParameter : IMacro, IEquatable<IMacro>
  {
    public bool IsCorrectAmountOfParameters(int amount)
    {
      return amount == 1;
    }

    public string Replace(string[] parameters)
    {
      return parameters[0].Trim('[', ']');
    }

    public bool Equals(IMacro other)
    {
      return other.GetType() == typeof (VectorToParameter);
    }

    public bool ShouldPreprocessParameter(int index)
    {
      return true;
    }
  }
}
