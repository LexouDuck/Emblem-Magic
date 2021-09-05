// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.IO.Logs.ILog
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

namespace Nintenlord.Event_Assembler.Core.IO.Logs
{
  public interface ILog
  {
    int MessageCount { get; }

    int ErrorCount { get; }

    int WarningCount { get; }

    void AddError(string message);

    void AddError(string format, params object[] parameters);

    void AddError(string file, string line, string message);

    void AddWarning(string message);

    void AddWarning(string format, params object[] parameters);

    void AddWarning(string file, string line, string message);

    void AddMessage(string message);

    void AddMessage(string format, params object[] parameters);

    void AddMessage(string file, string line, string message);
  }
}
