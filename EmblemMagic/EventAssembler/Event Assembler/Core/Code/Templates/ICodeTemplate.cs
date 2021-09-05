// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.ICodeTemplate
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Utility;
using System;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  public interface ICodeTemplate : INamed<string>, IParameterized
  {
    int MaxRepetition { get; }

    bool EndingCode { get; }

    int OffsetMod { get; }

    int AmountOfFixedCode { get; }

    int ID { get; }

    bool CanBeDisassembled { get; set; }

    bool CanBeAssembled { get; }

    bool Matches(Language.Types.Type[] parameterTypes);

    int GetLengthBytes(IExpression<int>[] parameters);

    CanCauseError<byte[]> GetData(IExpression<int>[] parameters, Func<string, int?> getSymbolValue);

    bool Matches(byte[] data, int offset);

    int GetLengthBytes(byte[] data, int offset);

    CanCauseError<string[]> GetAssembly(byte[] data, int offset);
    ICodeTemplate CopyWithNewName(string name);
    }
}
