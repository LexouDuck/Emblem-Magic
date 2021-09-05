// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.TemplateParameter
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Event_Assembler.Core.Code.Language.Types;
using Nintenlord.Utility.Strings;
using Nintenlord.Utility.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  internal class TemplateParameter : IEquatable<TemplateParameter>
  {
    public readonly string name;
    public readonly int position;
    public readonly int lenght;
    public readonly int minDimensions;
    public readonly int maxDimensions;
    public readonly bool pointer;
    public readonly Priority pointedPriority;
    public readonly bool isFixed;
    public readonly bool signed;
    public Func<int, string> conversion;

    public int LenghtInBytes
    {
      get
      {
        return this.lenght / 8;
      }
    }

    public int PositionInBytes
    {
      get
      {
        return this.position / 8;
      }
    }

    public int LastPosition
    {
      get
      {
        return this.position + this.lenght;
      }
    }

    public int LastPositionInBytes
    {
      get
      {
        return (this.position + this.lenght) / 8;
      }
    }

    public int BitsPerCoord
    {
      get
      {
        return this.lenght / this.maxDimensions;
      }
    }

    public TemplateParameter(string name, int position, int lenght, int minDimensions, int maxDimensions, bool pointer, Priority pointedPriority, bool signed, bool isFixed)
    {
      this.signed = signed;
      this.pointedPriority = pointedPriority;
      this.name = name;
      this.position = position;
      this.lenght = lenght;
      this.minDimensions = minDimensions;
      this.maxDimensions = maxDimensions;
      this.pointer = pointer;
      this.isFixed = isFixed;
      this.conversion = new Func<int, string>(TemplateParameter.ToHexString);
    }

    public bool Matches(string parameter)
    {
      return (1 + parameter.Amount(',')).IsInRange(this.minDimensions, this.maxDimensions);
    }

    public int[] GetValues(byte[] data, int codeOffset)
    {
      int[] numArray = new int[this.maxDimensions];
      for (int index = 0; index < numArray.Length; ++index)
      {
        int position = codeOffset * 8 + this.position + index * this.BitsPerCoord;
        byte[] bits = data.GetBits(position, this.BitsPerCoord);
        Array.Resize<byte>(ref bits, 4);
        if (this.signed && (int) bits.GetBits(this.BitsPerCoord - 1, 1)[0] == 1)
          bits.WriteTo(this.BitsPerCoord, new byte[4]
          {
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue
          }, 32 - this.BitsPerCoord);
        numArray[index] = BitConverter.ToInt32(bits, 0);
      }
      return numArray;
    }

    public bool InsertValues(int[] values, byte[] code)
    {
      for (int index = 0; index < values.Length; ++index)
      {
        byte[] bytes = BitConverter.GetBytes(values[index]);
        code.WriteTo(this.position + index * this.BitsPerCoord, bytes, this.BitsPerCoord);
      }
      return true;
    }

    public void SetBase(int valueBase)
    {
      switch (valueBase)
      {
        case 2:
          this.conversion = new Func<int, string>(TemplateParameter.ToBinString);
          break;
        case 10:
          this.conversion = new Func<int, string>(TemplateParameter.ToDecString);
          break;
        case 16:
          this.conversion = new Func<int, string>(TemplateParameter.ToHexString);
          break;
        default:
          throw new ArgumentException("Base must be either 2, 10 or 16.");
      }
    }

    public bool CompatibleType(Language.Types.Type type)
    {
      switch (type.type)
      {
        case MetaType.Atom:
          if (this.minDimensions == this.maxDimensions)
            return this.minDimensions == 1;
          return false;
        case MetaType.Vector:
          if (type.ParameterCount.IsInRange(this.minDimensions, this.maxDimensions))
            return ((IEnumerable<Language.Types.Type>) type.vectorParameterTypes).All<Nintenlord.Event_Assembler.Core.Code.Language.Types.Type>((Func<Language.Types.Type, bool>) (x => x.type == MetaType.Atom));
          return false;
        default:
          throw new ArgumentException();
      }
    }

    public bool Equals(TemplateParameter other)
    {
      return this.name.Equals(other.name) && this.minDimensions == other.minDimensions && this.maxDimensions == other.maxDimensions;
    }

    public override string ToString()
    {
      return this.name;
    }

    public override bool Equals(object obj)
    {
      if (obj is TemplateParameter)
        return this.Equals(obj as TemplateParameter);
      return false;
    }

    public override int GetHashCode()
    {
      return this.name.GetHashCode() ^ this.minDimensions ^ this.maxDimensions;
    }

    public static string ToHexString(int value)
    {
      return value.ToHexString("0x");
    }

    public static string ToDecString(int value)
    {
      return value.ToString();
    }

    public static string ToBinString(int value)
    {
      return value.ToBinString("b");
    }

    public static void WriteDocData(TextWriter writer, TemplateParameter parameter)
    {
      if (parameter.maxDimensions != 1)
      {
        if (parameter.maxDimensions == parameter.minDimensions)
          writer.WriteLine("Amount of coordinates is {0}.", (object) parameter.maxDimensions);
        else
          writer.WriteLine("Amount of coordinates can range from {0} to {1}.", (object) parameter.minDimensions, (object) parameter.maxDimensions);
      }
      long num1;
      long num2;
      if (parameter.signed)
      {
        num1 = (long) (1 << parameter.BitsPerCoord);
        num2 = 0L;
      }
      else
      {
        num1 = (long) (1 << parameter.BitsPerCoord - 1);
        num2 = -num1 - 1L;
      }
      writer.WriteLine("Parameter accepts values from {0} to {1}.", (object) num2, (object) num1);
      if (!parameter.pointer)
        return;
      writer.WriteLine("Parameter will transform passed offsets into pointers.");
    }
  }
}
