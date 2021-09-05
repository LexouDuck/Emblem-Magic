// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.ParameterizedHelpers
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility.Primitives;

namespace Nintenlord.Event_Assembler.Core.Code
{
  public static class ParameterizedHelpers
  {
    public static bool Matches(this IParameterized parameterized, string name, int paramCount, out string error)
    {
      int amountOfParameters1 = parameterized.MinAmountOfParameters;
      int amountOfParameters2 = parameterized.MaxAmountOfParameters;
      if (amountOfParameters1 != -1 && amountOfParameters2 != -1 && paramCount.IsInRange(amountOfParameters1, amountOfParameters2) || (amountOfParameters1 == -1 && paramCount <= amountOfParameters2 || amountOfParameters2 == -1 && paramCount >= amountOfParameters1) || amountOfParameters1 == -1 && amountOfParameters2 == -1)
      {
        error = string.Empty;
        return true;
      }
      string str = amountOfParameters1 != -1 ? (amountOfParameters2 != -1 ? (amountOfParameters1 != amountOfParameters2 ? "range {1}-{2}" : "{1}") : "minimun of {2}") : "maximun of {1}";
      error = string.Format("{0} requires " + str + " parameters", (object) name, (object) amountOfParameters1, (object) amountOfParameters2);
      return false;
    }
  }
}
