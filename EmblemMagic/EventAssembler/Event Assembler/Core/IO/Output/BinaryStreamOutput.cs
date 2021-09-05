// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.IO.Output.BinaryStreamOutput
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.IO;

namespace Nintenlord.Event_Assembler.Core.IO.Output
{
  internal class BinaryStreamOutput : IBinaryOutput, IDisposable
  {
    private Stream stream;

    public int Offset
    {
      get
      {
        return (int) this.stream.Position;
      }
      set
      {
        this.stream.Position = (long) value;
      }
    }

    public BinaryStreamOutput(Stream stream)
    {
      this.stream = stream;
    }

    public void Write(byte[] data)
    {
      this.Write(data, 0, data.Length);
    }

    public void Write(byte[] data, int index)
    {
      this.Write(data, index, data.Length - index);
    }

    public void Write(byte[] data, int index, int length)
    {
      this.stream.Write(data, index, length);
    }

    public void Dispose()
    {
      this.stream.Flush();
      this.stream.Dispose();
    }
  }
}
