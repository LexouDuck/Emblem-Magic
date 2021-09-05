// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.TerminatingStringTemplate
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  internal sealed class TerminatingStringTemplate : ICodeTemplate, INamed<string>, IParameterized
    {
        public int ID { get { return -1; } }
        public ICodeTemplate CopyWithNewName(string s) {
            List <TemplateParameter> newParams = new List<TemplateParameter>();
            newParams.Add(this.parameter);
            int newEndingValue = 0;
            for (int i = 0; i < this.endingValue.Length; i++)
            {
                newEndingValue <<= 8;
                newEndingValue += this.endingValue[i];
            }
            return new TerminatingStringTemplate(s, newParams, newEndingValue, this.offsetMod, this.comparer);
        }
        private TemplateParameter parameter;
    private byte[] endingValue;
    private string name;
    private int offsetMod;
    private StringComparer comparer;

    public TemplateParameter Parameter
    {
      get
      {
        return this.parameter;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

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
        return this.offsetMod;
      }
    }

    public int AmountOfFixedCode
    {
      get
      {
        return 0;
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

    public bool CanBeDisassembled { get { return true; } set { } } //TODO?
    public bool CanBeAssembled { get { return true; } }

    public TerminatingStringTemplate(string name, IEnumerable<TemplateParameter> parameters, int endingValue, int offsetMod, StringComparer stringComparer)
    {
      this.offsetMod = offsetMod;
      this.parameter = parameters.First<TemplateParameter>();
      this.endingValue = ((IEnumerable<byte>) BitConverter.GetBytes(endingValue)).Take<byte>(this.parameter.LenghtInBytes).ToArray<byte>();
      this.name = name;
      this.comparer = stringComparer;
    }

    public bool Matches(byte[] data, int offset)
    {
      return true;
    }

    public int GetLengthBytes(byte[] code, int offset)
    {
      int currentOffset = offset;
      while (this.IsNotEnding(code, currentOffset))
        currentOffset += this.parameter.LenghtInBytes;
      return currentOffset - offset + this.endingValue.Length;
    }

    public CanCauseError<string[]> GetAssembly(byte[] code, int offset)
    {
      List<string> stringList = new List<string>();
      stringList.Add(this.name);
      while (this.IsNotEnding(code, offset))
      {
        int[] values = this.parameter.GetValues(code, offset);
        stringList.Add(this.parameter.conversion(values[0]));
        offset += this.parameter.LenghtInBytes;
      }
      return (CanCauseError<string[]>) stringList.ToArray();
    }

    public bool Matches(Language.Types.Type[] code)
    {
      foreach (Language.Types.Type type in code)
      {
        if (!this.parameter.CompatibleType(type))
          return false;
      }
      return true;
    }

    public int GetLengthBytes(IExpression<int>[] code)
    {
      return (code.Length + 1) * this.parameter.LenghtInBytes;
    }

    public CanCauseError<byte[]> GetData(IExpression<int>[] code, Func<string, int?> getSymbolValue)
    {
      List<byte> byteList = new List<byte>(32);
      for (int index = 0; index < code.Length; ++index)
      {
        CanCauseError<int[]> values = CodeTemplate.GetValues(code[index], this.parameter, getSymbolValue, (IPointerMaker) null);
        if (values.CausedError)
          return values.ConvertError<byte[]>();
        byte[] code1 = new byte[this.parameter.LenghtInBytes];
        this.parameter.InsertValues(values.Result, code1);
        byteList.AddRange((IEnumerable<byte>) code1);
      }
      byteList.AddRange((IEnumerable<byte>) this.endingValue);
      return (CanCauseError<byte[]>) byteList.ToArray();
    }

    private bool IsNotEnding(byte[] code, int currentOffset)
    {
      return !code.Equals<byte>(currentOffset, this.endingValue, 0, this.parameter.LenghtInBytes);
    }

    public static void WriteDoc(TextWriter writer, TerminatingStringTemplate template)
    {
      writer.WriteLine("{0} {1}1 {1}2 ... {1}N", (object) template.name, (object) template.parameter.name);
    }
  }
}
