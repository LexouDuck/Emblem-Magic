// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros.CurrentFile
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.IO.Input;
using System;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros
{
  internal class CurrentFile : IMacro, IEquatable<IMacro>
  {
    public IInputStream Stream { get; set; }

    public bool IsCorrectAmountOfParameters(int amount)
    {
      return amount == 0;
    }

    public string Replace(string[] parameters)
    {
      return '"' + this.Stream.CurrentFile + '"';
    }

    public bool Equals(IMacro other)
    {
      return this.GetType() == other.GetType();
    }

    public bool ShouldPreprocessParameter(int index)
    {
      return false;
    }
  }
}
