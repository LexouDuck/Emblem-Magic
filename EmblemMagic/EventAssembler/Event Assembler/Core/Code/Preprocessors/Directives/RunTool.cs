using System;
using Nintenlord.Utility;
using System.Collections.Generic;
using Nintenlord.Collections;
using System.Text;
using System.IO;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
  internal class RunTool : ToolDirectiveBase
  {
    public override string Name
    {
      get
      {
        return "runext";
      }
    }

    public override CanCauseError ApplyIncludeTool(byte[] toolOutput, IDirectivePreprocessor host)
    {
      foreach (string line in Encoding.UTF8.GetString(toolOutput).Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
        host.Log.AddMessage(line);

      return CanCauseError.NoError;
    }
  }
}
