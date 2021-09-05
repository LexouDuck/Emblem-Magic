using Nintenlord.Utility;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
    internal class IncludeToolEventAlias : IncludeToolEvent, INamed<string>
    {

        public new string Name
        {
            get
            {
                return "inctext";
            }
        }
    }
}
