// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros.InsertText
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros
{
  public class InsertText : IMacro, IEquatable<IMacro>
  {
    public bool IsCorrectAmountOfParameters(int amount)
    {
      return amount == 1;
    }

    public string Replace(string[] parameters)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(parameters[0].ToCharArray());
      StringBuilder stringBuilder = new StringBuilder(bytes.Length * 5 + 6);
      stringBuilder.Append("BYTE");
      foreach (byte num in bytes)
        stringBuilder.Append(" 0x" + num.ToString("X8"));
      return stringBuilder.ToString();
    }

    public bool Equals(IMacro other)
    {
      return other.GetType() == typeof(InsertText);
    }

    public bool ShouldPreprocessParameter(int index)
    {
      return true;
    }
  }
}
