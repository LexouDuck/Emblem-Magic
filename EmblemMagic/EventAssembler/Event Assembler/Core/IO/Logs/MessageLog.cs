// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.IO.Logs.MessageLog
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.IO.Logs
{
  public abstract class MessageLog : ILog
  {
    private List<string> messages;
    private List<string> errors;
    private List<string> warnings;

    public int MessageCount
    {
      get
      {
        return this.messages.Count;
      }
    }

    public int ErrorCount
    {
      get
      {
        return this.errors.Count;
      }
    }

    public int WarningCount
    {
      get
      {
        return this.warnings.Count;
      }
    }

    public MessageLog()
    {
      this.messages = new List<string>();
      this.warnings = new List<string>();
      this.errors = new List<string>();
    }

    protected string GetText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Finished.");
      if (this.messages.Count > 0)
      {
        stringBuilder.AppendLine("Messages:");
        foreach (string message in this.messages)
          stringBuilder.AppendLine(message);
        stringBuilder.AppendLine();
      }
      if (this.errors.Count > 0)
      {
        stringBuilder.AppendLine(this.errors.Count.ToString() + " errors encountered:");
        foreach (string error in this.errors)
          stringBuilder.AppendLine(error.Trim());
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("No data written to output.");
        stringBuilder.AppendLine();
      }
      if (this.warnings.Count > 0)
      {
        stringBuilder.AppendLine(this.warnings.Count.ToString() + " warnings encountered:");
        foreach (string warning in this.warnings)
          stringBuilder.AppendLine(warning);
        stringBuilder.AppendLine();
      }
      if (this.warnings.Count == 0 && this.errors.Count == 0)
      {
        stringBuilder.AppendLine("No errors or warnings.");
        stringBuilder.AppendLine("Please continue being awesome.");
        stringBuilder.AppendLine();
      }
      return stringBuilder.ToString();
    }

    public void AddError(string message)
    {
      if (message == null)
        throw new ArgumentNullException();
      this.errors.Add(message);
    }

    public void AddWarning(string message)
    {
      if (message == null)
        throw new ArgumentNullException();
      this.warnings.Add(message);
    }

    public void AddMessage(string message)
    {
      if (message == null)
        throw new ArgumentNullException();
      this.messages.Add(message);
    }

    public void Clear()
    {
      this.messages.Clear();
      this.warnings.Clear();
      this.errors.Clear();
    }

    public void AddError(string file, string line, string message)
    {
      this.AddError(Path.GetFileName(file) + ": " + message + " : " + line);
    }

    public void AddWarning(string file, string line, string message)
    {
      this.AddWarning(Path.GetFileName(file) + ": " + message + " : " + line);
    }

    public void AddMessage(string file, string line, string message)
    {
      this.AddMessage(Path.GetFileName(file) + ": " + message + " : " + line);
    }

    public abstract void PrintAll();

    public void AddError(string format, params object[] parameters)
    {
      this.AddError(string.Format(format, parameters));
    }

    public void AddWarning(string format, params object[] parameters)
    {
      this.AddWarning(string.Format(format, parameters));
    }

    public void AddMessage(string format, params object[] parameters)
    {
      this.AddMessage(string.Format(format, parameters));
    }
  }
}
