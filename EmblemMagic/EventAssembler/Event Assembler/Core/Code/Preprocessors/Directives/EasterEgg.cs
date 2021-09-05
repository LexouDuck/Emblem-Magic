// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives.IncludeBinary
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility;
using System;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
  internal class EasterEgg : IDirective, INamed<string>, IParameterized
  {
    public string Name
    {
      get
      {
        return "easteregg";
      }
    }

    public bool RequireIncluding
    {
      get
      {
        return false;
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
        return 1;
      }
    }

    public CanCauseError Apply(string[] parameters, IDirectivePreprocessor host)
    {
      switch (parameters[0])
      {
        case "hailthelord":
          host.Input.AddNewLine("MESSAGE Praise Nintenlord!");
          break;
        case "hextator":
          host.Input.AddNewLine("MESSAGE hexpotato");
          break;
        case "feditor":
          host.Input.AddNewLine("MESSAGE FE_Editor");
          break;
        case "baldur":
        case "tordo":
          host.Input.AddNewLine("MESSAGE The proyect is dead? Reply soon. :)");
          break;
        case "incest":
          host.Input.AddNewLine("MESSAGE incext? Are you sure you didn't mean incest?");
          break;
        case "proyect":
          host.Input.AddNewLine("MESSAGE soon");
          break;
        default:
          break;
      }

      return CanCauseError.NoError;
    }
  }
}
