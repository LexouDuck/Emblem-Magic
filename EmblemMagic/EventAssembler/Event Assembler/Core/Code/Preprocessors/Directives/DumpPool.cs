// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives.DumpPool
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
  internal class DumpPool : IDirective, INamed<string>, IParameterized
  {
    public string Name
    {
      get
      {
        return "pool";
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
        return 0;
      }
    }

    public int MaxAmountOfParameters
    {
      get
      {
        return 0;
      }
    }

    public CanCauseError Apply(string[] parameters, IDirectivePreprocessor host)
    {
      host.Input.AddNewLines((IEnumerable<string>) host.Pool.DumpPool());
      return CanCauseError.NoError;
    }
  }
}
