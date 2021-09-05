using System;
using Nintenlord.Utility;
using System.Collections.Generic;
using Nintenlord.Collections;
using System.Text;
using System.IO;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
    internal class IncludeToolEvent : ToolDirectiveBase
    {
        //Nintenlord.Event_Assembler.Core.IO.IOHelpers.FindFile
        public override string Name
        {
            get
            {
                return "inctevent";
            }
        }
        
        public override CanCauseError ApplyIncludeTool(byte[] toolOutput, IDirectivePreprocessor host)
        {
            string eventCode = Encoding.UTF8.GetString(toolOutput);
            string[] lines = eventCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            host.Input.AddNewLines(lines);
            return CanCauseError.NoError;
        }
    }
}
