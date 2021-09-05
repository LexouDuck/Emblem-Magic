// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Collections.IDefineCollection
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility;

namespace Nintenlord.Event_Assembler.Core.Collections
{
  public interface IDefineCollection
  {
    void Add(string name, string replacer, params string[] parameters);

    void Add(string name, string replacer);

    bool ContainsName(string name, params string[] parameters);

    bool ContainsName(string name);

    void Remove(string name, params string[] parameters);

    void Remove(string name);

    bool ApplyDefines(string original, out string newOriginal);

    CanCauseError<string> ApplyDefines(string original);

    CanCauseError<string> ApplyPreprocessorDefines(string original);

    bool IsValidName(string name);
  }
}
