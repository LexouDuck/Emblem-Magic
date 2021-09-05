// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.GenericFE8Ender
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Utility;
using System;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  public class GenericFE8Ender : ICodeTemplate, INamed<string>, IParameterized
  {
        public int ID { get { return 0; } }
        public ICodeTemplate CopyWithNewName(string s) { return new GenericFE8Ender(); }
    public int MaxRepetition
    {
      get
      {
        return 1;
      }
    }

    public bool EndingCode
    {
      get
      {
        return true;
      }
    }

    public int OffsetMod
    {
      get
      {
        return 2;
      }
    }

    public int AmountOfFixedCode
    {
      get
      {
        return 0;
      }
    }

    public string Name
    {
      get
      {
        return "FE8End";
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

    public bool Matches(byte[] data, int offset)
    {
      return (int) data[offset + 1] == 1;
    }

    public int GetLengthBytes(byte[] data, int offset)
    {
      return ((int) data[offset] >> 4) * 2;
    }

    public CanCauseError<string[]> GetAssembly(byte[] data, int offset)
    {
      return (CanCauseError<string[]>) new string[1]{ this.Name };
    }

    public bool Matches(Language.Types.Type[] code)
    {
      throw new NotImplementedException();
    }

    public int GetLengthBytes(IExpression<int>[] code)
    {
      throw new NotImplementedException();
    }

    public CanCauseError<byte[]> GetData(IExpression<int>[] code, Func<string, int?> getSymbolValue)
    {
      throw new NotImplementedException();
    }
  }
}
