// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.ICodeTemplateStorer
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Event_Assembler.Core.Code.Language.Types;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.Utility;
using System.Collections;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  public interface ICodeTemplateStorer : IEnumerable<ICodeTemplate>, IEnumerable
  {
    void AddCode(ICodeTemplate code, Priority priority);

    CanCauseError<ICodeTemplate> FindTemplate(IInputByteStream reader, IEnumerable<Priority> allowedPriorities);

    CanCauseError<ICodeTemplate> FindTemplate(byte[] code, int index, IEnumerable<Priority> allowedPriorities);

    CanCauseError<ICodeTemplate> FindTemplate(string codeName, Type[] parameterTypes);

    CanCauseError<ICodeTemplate> FindTemplate(string name, Priority priority);

    IEnumerable<string> GetNames();

    bool IsUsedName(string name);
  }
}
