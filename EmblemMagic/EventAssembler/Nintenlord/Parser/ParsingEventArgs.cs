using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Parser
{
    public class ParsingEventArgs<TIn,TOut> : EventArgs
    {
        public Match<TIn> Match
        {
            get;
            private set;
        }
        public TOut Result
        {
            get
            {
                if (Match.Success)
                {
                    return result;
                }
                else
                {
                    throw new InvalidOperationException("No result for non-successful match.");
                }
            }
        }

        private TOut result;

        public ParsingEventArgs(Match<TIn> match, TOut result)
        {
            Match = match;
            this.result = result;
        }
    }
}
