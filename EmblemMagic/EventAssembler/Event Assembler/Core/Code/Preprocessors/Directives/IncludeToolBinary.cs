using System;
using Nintenlord.Utility;
using System.Collections.Generic;
using Nintenlord.Collections;
using System.Text;
using System.IO;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
    internal class IncludeToolBinary : ToolDirectiveBase
    {
        //Nintenlord.Event_Assembler.Core.IO.IOHelpers.FindFile
        public override string Name
        {
            get
            {
                return "incext";
            }
        }
        
        public override CanCauseError ApplyIncludeTool(byte[] toolOutput, IDirectivePreprocessor host)
        {
            host.Input.AddBytes(toolOutput);
            return CanCauseError.NoError;
        }
    }
}
