// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.TemplateComparer
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  public class TemplateComparer : IComparer<ICodeTemplate>, IEqualityComparer<ICodeTemplate>
  {
    public int Compare(ICodeTemplate a, ICodeTemplate b)
    {
      if (a == b)
        return 0;
      /* if (a.GetType() == typeof (GenericFE8Template))
        return -1;
      if (b.GetType() == typeof (GenericFE8Template))
        return 1; */
      if (a.AmountOfFixedCode != b.AmountOfFixedCode)
        return a.AmountOfFixedCode - b.AmountOfFixedCode;
      if (a.OffsetMod != b.OffsetMod)
        return a.OffsetMod - b.OffsetMod;
      if (a.EndingCode != b.EndingCode)
        return a.EndingCode ? 1 : -1;
      CodeTemplate source1 = a as CodeTemplate;
      CodeTemplate source2 = b as CodeTemplate;
      if (source1 != null && source2 != null)
      {
        if (source1.Length != source2.Length)
          return source1.Length - source2.Length;
        int num1 = source1.Aggregate<TemplateParameter, int>(1, (Func<int, TemplateParameter, int>) ((x, y) => x * y.maxDimensions));
        int num2 = source2.Aggregate<TemplateParameter, int>(1, (Func<int, TemplateParameter, int>) ((x, y) => x * y.maxDimensions));
        if (num1 != num2)
          return num1 - num2;
        if (source1.AmountOfParams != source2.AmountOfParams)
          return -source1.AmountOfParams + source2.AmountOfParams;
      }
      else
      {
        if (source1 != null)
          return 1;
        if (source2 != null)
          return -1;
      }
      return 0;
    }

    public bool Equals(ICodeTemplate x, ICodeTemplate y)
    {
      return this.Compare(x, y) == 0;
    }

    public int GetHashCode(ICodeTemplate obj)
    {
      if (obj is CodeTemplate)
        return obj.GetHashCode();
      return obj.Name.GetHashCode();
    }
  }
}
