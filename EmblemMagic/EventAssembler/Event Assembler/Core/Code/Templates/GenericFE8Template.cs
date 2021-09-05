// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.GenericFE8Template
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Utility;
using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  public class GenericFE8Template : ICodeTemplate, INamed<string>, IParameterized
    {
        public int ID { get { return 0; } }
        public ICodeTemplate CopyWithNewName(string s) { return new GenericFE8Template(); }
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
        return false;
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
        return "FE8Code";
      }
    }

    public int MinAmountOfParameters
    {
      get
      {
        return -1;
      }
    }

    public int MaxAmountOfParameters
    {
      get
      {
        return -1;
      }
    }

    public bool Matches(byte[] data, int offset)
    {
      return (int) data[offset + 1] > 1;
    }

    public int GetLengthBytes(byte[] data, int offset)
    {
      return ((int) data[offset] >> 4) * 2;
    }

    public CanCauseError<string[]> GetAssembly(byte[] data, int offset)
    {
      int num = (int) data[offset] >> 4;
      List<string> stringList = new List<string>();
      stringList.Add(this.Name);
      for (int index = 0; index < num; ++index)
        stringList.Add("0x" + data[offset + index * 2 + 1].ToString("X2") + data[offset + index * 2].ToString("X2"));
      return (CanCauseError<string[]>) stringList.ToArray();
    }

    public bool Matches(Language.Types.Type[] code)
    {
        return true;
        //throw new NotImplementedException();
    }

    public int GetLengthBytes(IExpression<int>[] code)
    {
            //all parameters are shorts.
            return 2 * code.Length;
    }

    public CanCauseError<byte[]> GetData(IExpression<int>[] code, Func<string, int?> getSymbolValue)
        {
            List<byte> byteList = new List<byte>(this.GetLengthBytes(code));
            for(int index = 0; index < code.Length; ++index)
            {
                CanCauseError<int> aResult = Folding.TryFold(code[index], getSymbolValue);
                if (aResult.CausedError)
                    return aResult.ConvertError<byte[]>();
                byteList.AddRange((IEnumerable<byte>)BitConverter.GetBytes((short)aResult.Result));
            }
            //int num = code.Length / this.AmountOfParams;
            return (CanCauseError<byte[]>)byteList.ToArray();
            //throw new NotImplementedException();
        }
    }
}
