// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.IO.Input.InputHelpers
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Utility;
using System.IO;

namespace Nintenlord.Event_Assembler.Core.IO.Input
{
  public static class InputHelpers
  {
    public static string GetPositionString(this IPositionableInputStream stream)
    {
      return string.Format("File: {0}, Line: {1}", (object) Path.GetFileName(stream.CurrentFile), (object) stream.LineNumber);
    }

    public static string GetErrorString(this IPositionableInputStream stream, string error)
    {
      return string.Format("{0}: {1}"/*: {2}"*/, (object) stream.GetPositionString(), (object) error/*, (object) stream.PeekOriginalLine()*/);
    }

    public static string GetErrorString(this IPositionableInputStream stream, CanCauseError error)
    {
      return string.Format("{0}: {1}", (object) stream.GetPositionString(), (object) error.ErrorMessage);
    }
    /*
    public static string GetErrorString<T>(this IPositionableInputStream stream, CanCauseError<T> error)
    {
      return string.Format("{0}: {1}: {2}", (object) stream.GetPositionString(), (object) error.ErrorMessage, (object) stream.PeekOriginalLine());
    }*/
  }
}
