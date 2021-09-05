// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.CoreInfo
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility.Primitives;
using System.Collections.Generic;
using System.Reflection;

namespace Nintenlord.Event_Assembler.Core
{
  internal static class CoreInfo
  {
    public static string[] DefaultLines(string game, string file, int offset, int? size)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) new string[5]
      {
        "Disassembled with Nintenlord's Event Assembler",
        "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString(4),
        "Game: " + game,
        "File: " + file,
        "Offset: " + offset.ToHexString("$")
      });
      if (size.HasValue)
        stringList.Add("Size: " + size.Value.ToHexString("0x"));
      return stringList.ToArray();
    }
  }
}
