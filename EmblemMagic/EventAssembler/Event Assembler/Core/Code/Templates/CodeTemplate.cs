// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.CodeTemplate
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression.Tree;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using Nintenlord.Utility.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  internal sealed class CodeTemplate : ICodeTemplate, INamed<string>, IParameterized, IEnumerable<TemplateParameter>, IEnumerable, INamed<int>
  {
    private readonly string name;
    private readonly int lenght;
    private readonly int id;
    private readonly byte[] baseData;
    private readonly bool canBeRepeated;
    private readonly bool checkForProblems;
    private readonly bool isEndingCode;
    private readonly int offsetMod;
    private readonly int amountOfFixedCode;
    private readonly bool canBeAssembled;
    private bool canBeDisassembled;
    private readonly List<TemplateParameter> parameters;
    private readonly List<TemplateParameter> fixedParameters;
    private IPointerMaker pointerMaker;
    private readonly StringComparer comparer;

    public IPointerMaker PointerMaker
    {
      get
      {
        return this.pointerMaker;
      }
      set
      {
        this.pointerMaker = value;
      }
    }

    public int Length
    {
      get
      {
        return this.lenght;
      }
    }

    public int LengthInBytes
    {
      get
      {
        return this.lenght / 8;
      }
    }

    public int AmountOfParams
    {
      get
      {
        return this.parameters.Count;
      }
    }

    public TemplateParameter this[int i]
    {
      get
      {
        if (i < 0)
          throw new IndexOutOfRangeException();
        if (this.canBeRepeated)
          return this.parameters[i % this.AmountOfParams];
        return this.parameters[i];
      }
    }

    public bool CanBeDisassembled
    {
      get
      {
        return this.canBeDisassembled;
      }
      set
      {
        canBeDisassembled = value;
      }
    }

    public bool CanBeAssembled
    {
      get
      {
        return this.canBeAssembled;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public int ID
    {
        get { return this.id; }
    }

    public bool EndingCode
    {
      get
      {
        return this.isEndingCode;
      }
    }

    public int MaxRepetition
    {
      get
      {
        return this.canBeRepeated ? 4 : 1;
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
        return this.amountOfFixedCode;
      }
    }

    public int MinAmountOfParameters
    {
      get
      {
        return this.AmountOfParams;
      }
    }

    public int MaxAmountOfParameters
    {
      get
      {
        return this.AmountOfParams;
      }
    }

    int INamed<int>.Name
    {
      get
      {
        return this.id;
      }
    }

    public CodeTemplate(string name, int id, int lenght, IEnumerable<TemplateParameter> parameters, bool canBeRepeated, bool checkForProblems, bool end, int offsetMode, bool canBeAssembled, bool canBeDisassembled, StringComparer stringComparer)
    {
      this.offsetMod = offsetMode;
      this.isEndingCode = end;
      this.checkForProblems = checkForProblems;
      this.canBeRepeated = canBeRepeated;
      this.lenght = lenght;
      this.name = name;
      this.id = id;
      this.canBeAssembled = canBeAssembled;
      this.canBeDisassembled = canBeDisassembled;
      this.comparer = stringComparer;
      this.parameters = new List<TemplateParameter>(parameters.Count<TemplateParameter>());
      this.fixedParameters = new List<TemplateParameter>(parameters.Count<TemplateParameter>());
      this.baseData = new byte[this.LengthInBytes];
      if (id != 0)
      {
        this.baseData[0] = (byte) (id & (int) byte.MaxValue);
        this.baseData[1] = (byte) (id >> 8 & (int) byte.MaxValue);
      }
      foreach (TemplateParameter parameter in parameters)
      {
        if (parameter.isFixed)
        {
          this.fixedParameters.Add(parameter);
          if (!parameter.name.IsValidNumber())
            throw new ArgumentException("The name of fixed parameter is not a number: " + parameter.name);
          int num = parameter.name.GetValue();
          parameter.InsertValues(num.GetArray<int>(), this.baseData);
        }
        else
          this.parameters.Add(parameter);
      }
      this.amountOfFixedCode = 0;
      if (id != 0)
        this.amountOfFixedCode += 2;
      foreach (TemplateParameter fixedParameter in this.fixedParameters)
        this.amountOfFixedCode += fixedParameter.LenghtInBytes;
      if (this.checkForProblems && !this.CheckIfWorks())
        throw new ArgumentException("Arguments are not valid in code: " + name + " " + parameters.ToElementWiseString<TemplateParameter>(", ", "{", "}"));
    }

    private bool CheckIfWorks()
    {
      if (this.id != 0 && this.LengthInBytes < 2 || this.lenght < 0 || this.canBeRepeated && this.parameters.Count != 1)
        return false;
      bool[] flagArray = new bool[this.lenght];
      for (int index = 0; index < flagArray.Length; ++index)
        flagArray[index] = false;
      if (this.id != 0)
      {
        for (int index = 0; index < 16; ++index)
          flagArray[index] = true;
      }
      foreach (TemplateParameter parameter in this.parameters)
      {
        if (parameter.LastPosition > this.lenght || parameter.position < 0)
          return false;
        for (int index = parameter.position; index < parameter.LastPosition; ++index)
        {
          if (flagArray[index])
            return false;
          flagArray[index] = true;
        }
      }
      return true;
    }

    private byte[] GetDataUnit(string[] text, ILog messageLog)
    {
      byte[] code = this.baseData.Clone() as byte[];
      for (int index1 = 1; index1 < text.Length; ++index1)
      {
        TemplateParameter templateParameter = this[index1 - 1];
        if (templateParameter.lenght > 0)
        {
          string[] strArray = text[index1].Trim('[', ']').Split(new char[1]{ ',' }, templateParameter.maxDimensions, StringSplitOptions.RemoveEmptyEntries);
          int[] values = new int[strArray.Length];
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            if (!strArray[index2].GetMathStringValue(out values[index2]))
              messageLog.AddError(strArray[index2] + " is not a valid number.");
            if (templateParameter.pointer)
              values[index2] = this.pointerMaker.MakePointer(values[index2]);
          }
          templateParameter.InsertValues(values, code);
        }
      }
      return code;
    }

    private CanCauseError<byte[]> GetDataUnit(IExpression<int>[] parameters, Func<string, int?> getSymbolValue)
    {
      byte[] code = this.baseData.Clone() as byte[];
      for (int index = 0; index < parameters.Length; ++index)
      {
        TemplateParameter paramTemp = this[index];
        if (paramTemp.lenght > 0)
        {
          CanCauseError<int[]> values = CodeTemplate.GetValues(parameters[index], paramTemp, getSymbolValue, this.pointerMaker);
          if (values.CausedError)
            return values.ConvertError<byte[]>();
          paramTemp.InsertValues(values.Result, code);
        }
      }
      return (CanCauseError<byte[]>) code;
    }

    internal static CanCauseError<int[]> GetValues(IExpression<int> parameter, TemplateParameter paramTemp, Func<string, int?> getSymbolValue, IPointerMaker pointerMaker)
    {
      int[] numArray;
      if (parameter is ExpressionList<int>)
      {
        ExpressionList<int> expressionList = parameter as ExpressionList<int>;
        numArray = new int[expressionList.ComponentCount];
        for (int index = 0; index < expressionList.ComponentCount; ++index)
        {
          CanCauseError<int> canCauseError = Folding.Fold(expressionList[index], getSymbolValue);
          if (canCauseError.CausedError)
            return canCauseError.ConvertError<int[]>();
          numArray[index] = canCauseError.Result;
        }
      }
      else
      {
        CanCauseError<int> canCauseError = Folding.Fold(parameter, getSymbolValue);
        if (canCauseError.CausedError)
          return canCauseError.ConvertError<int[]>();
        if (paramTemp.pointer && pointerMaker != null)
          numArray = new int[1]
          {
            pointerMaker.MakePointer(canCauseError.Result)
          };
        else
          numArray = new int[1]
          {
            canCauseError.Result
          };
      }
      return (CanCauseError<int[]>) numArray;
    }

    public bool Matches(byte[] data, int offset)
    {
      if (!this.canBeDisassembled || offset * 8 + this.lenght > data.Length * 8 || offset % this.offsetMod != 0)
        return false;
      if (this.id == 0 && this.fixedParameters.Count == 0 && this.parameters.Count == 0)
        return true;
      if (this.checkForProblems && this.id != 0 && this.id != (int) data[offset] + ((int) data[offset + 1] << 8))
        return false;
      foreach (TemplateParameter fixedParameter in this.fixedParameters)
      {
        if (!
          (baseData.GetBits(fixedParameter.position, fixedParameter.lenght)).SequenceEqual(
               data.GetBits(offset * 8 + fixedParameter.position, fixedParameter.lenght)))
          return false;
      }
      foreach (TemplateParameter parameter in this.parameters)
      {
        if (parameter.pointer && !this.pointerMaker.IsAValidPointer(BitConverter.ToInt32(data.GetBits(offset * 8 + parameter.position, parameter.lenght), 0)))
          return false;
      }
      return true;
    }

    public int GetLengthBytes(byte[] code, int offset)
    {
      return this.LengthInBytes;
    }

    public CanCauseError<string[]> GetAssembly(byte[] code, int offset)
    {
      string[] strArray = new string[this.AmountOfParams + 1];
      strArray[0] = this.name;
      for (int index1 = 0; index1 < this.parameters.Count; ++index1)
      {
        TemplateParameter templateParameter = this[index1];
        StringBuilder stringBuilder = new StringBuilder(templateParameter.lenght / 2);
        int[] values = templateParameter.GetValues(code, offset);
        if (values.Length > 1)
        {
          stringBuilder.Append("[");
          for (int index2 = 0; index2 < values.Length; ++index2)
          {
            stringBuilder.Append(templateParameter.conversion(values[index2]));
            if (index2 != templateParameter.maxDimensions - 1)
              stringBuilder.Append(",");
          }
          stringBuilder.Append("]");
        }
        else
        {
          int pointer = values[0];
          if (templateParameter.pointer)
            pointer = !this.pointerMaker.IsAValidPointer(pointer) ? 0 : this.pointerMaker.MakeOffset(pointer);
          stringBuilder.Append(templateParameter.conversion(pointer));
        }
        strArray[index1 + 1] = stringBuilder.ToString();
      }
      return (CanCauseError<string[]>) strArray;
    }

    public bool Matches(Language.Types.Type[] paramTypes)
    {
      if (!this.canBeAssembled)
        return false;
      if (this.canBeRepeated)
      {
        if (this.AmountOfParams != 1)
          return false;
        for (int index = 0; index < paramTypes.Length; ++index)
        {
          if (!this[index].CompatibleType(paramTypes[index]))
            return false;
        }
      }
      else
      {
        if (this.AmountOfParams != paramTypes.Length)
          return false;
        for (int index = 0; index < this.AmountOfParams; ++index)
        {
          if (!this[index].CompatibleType(paramTypes[index]))
            return false;
        }
      }
      return true;
    }

    public int GetLengthBytes(IExpression<int>[] code)
    {
      if (this.canBeRepeated)
        return this.LengthInBytes * code.Length;
      return this.LengthInBytes;
    }

    public CanCauseError<byte[]> GetData(IExpression<int>[] code, Func<string, int?> getSymbolValue)
    {
      if (!canBeRepeated)
        return GetDataUnit(code, getSymbolValue);

      if (code.Length == 0)
        return CanCauseError<byte[]>.Error("Encountered {0} code with no parameters", Name);

      List<byte> byteList = new List<byte>(code.Length * this.LengthInBytes);
      int num = code.Length / AmountOfParams;

      for (int index = 0; index < num; ++index)
      {
        CanCauseError<byte[]> dataUnit = GetDataUnit(new IExpression<int>[1]{ code[index] }, getSymbolValue);

        if (dataUnit.CausedError)
          return dataUnit.ConvertError<byte[]>();

        byteList.AddRange(dataUnit.Result);
      }

      return (CanCauseError<byte[]>) byteList.ToArray();
    }

    public IEnumerator<TemplateParameter> GetEnumerator()
    {
      return (IEnumerator<TemplateParameter>) this.parameters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.parameters.GetEnumerator();
    }

    public override string ToString()
    {
      if (this.parameters.Count > 0)
        return string.Format("{0} {1}", (object) this.name, (object) this.parameters.ToElementWiseString<TemplateParameter>(", ", "", ""));
      return this.name;
    }

    public override int GetHashCode()
    {
      return this.name.GetHashCode() ^ this.id;
    }

    public static void WriteDoc(TextWriter writer, CodeTemplate code)
    {
      writer.Write(code.name);
      writer.Write(' ');
      foreach (TemplateParameter parameter in code.parameters)
      {
        if (parameter.maxDimensions > 1)
        {
          writer.Write('[');
          for (int i = 0; i < parameter.maxDimensions; ++i)
          {
            CodeTemplate.WriteName(writer, parameter.name, i, parameter.maxDimensions);
            if (i != parameter.maxDimensions - 1)
              writer.Write(", ");
          }
          writer.Write("] ");
        }
        else
        {
          CodeTemplate.WriteName(writer, parameter.name, 0, parameter.maxDimensions);
          writer.Write(' ');
        }
      }
      writer.WriteLine();
    }

    private static void WriteName(TextWriter writer, string name, int i, int max)
    {
      bool flag;
      string str;
      if (max == 1)
      {
        flag = name.Contains<char>(' ');
        str = "";
      }
      else if (max == 2 && (name.Contains("Position") || name.Contains("position") || (name.Contains("Location") || name.Contains("location")) || (name.Contains("Coordinate") || name.Contains("coordinate"))))
      {
        flag = false;
        str = i != 0 ? " Y" : " X";
      }
      else
      {
        flag = false;
        str = " " + (i + 1).ToString();
      }
      if (flag)
      {
        writer.Write('*');
        writer.Write(name + str);
        writer.Write('*');
      }
      else
        writer.Write(name + str);
    }

    public ICodeTemplate CopyWithNewName(string name)
    {
        return new CodeTemplate(name, this.id, this.lenght, this.parameters, this.canBeRepeated, this.checkForProblems, this.EndingCode, this.offsetMod, this.canBeAssembled, this.canBeDisassembled, this.comparer);
    }
  }
}
