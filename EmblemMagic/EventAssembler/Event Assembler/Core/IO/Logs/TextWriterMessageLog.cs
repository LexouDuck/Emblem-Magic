// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.IO.Logs.TextWriterMessageLog
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System.IO;

namespace Nintenlord.Event_Assembler.Core.IO.Logs
{
  public class TextWriterMessageLog : MessageLog
  {
    private TextWriter writer;

    public TextWriter Writer
    {
      get
      {
        return this.writer;
      }
      set
      {
        this.writer = value;
      }
    }

    public TextWriterMessageLog(TextWriter writer)
    {
      this.writer = writer;
    }

    public override void PrintAll()
    {
      this.writer.WriteLine(this.GetText());
    }
  }
}
