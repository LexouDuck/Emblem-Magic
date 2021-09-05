// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.IO.Output.IBinaryOutput
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;

namespace Nintenlord.Event_Assembler.Core.IO.Output
{
  internal interface IBinaryOutput : IDisposable
  {
    int Offset { get; set; }

    void Write(byte[] data);

    void Write(byte[] data, int index);

    void Write(byte[] data, int index, int length);
  }
}
