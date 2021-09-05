using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility
{
    public sealed class CommandLineArgsHandler
    {
        private readonly Dictionary<string, Action<string>> flags;
        private readonly Action<string>[] parameters;
        
        public CommandLineArgsHandler(
            IEnumerable<Action<string>> parameters,
            IEnumerable<KeyValuePair<string, Action<string>>> flags)
        {
            this.flags = flags.ToDictionary(x => x.Key, x => x.Value);
            this.parameters = parameters.ToArray();
        }

        public CanCauseError HandleArgs(IEnumerable<string> args)
        {
            int parameterNumber = 0;
            foreach (var arg in args)
            {
                if (arg[0] == '-')
                {
                    var split = arg.Split(new[] { ':' }, 2);
                    var flagName = split[0].TrimStart('-');
                    var param = split.Length == 2 ? split[1] : null;

                    Action<string> flagAction;
                    if (flags.TryGetValue(flagName, out flagAction))
                    {
                        flagAction(param);
                    }
                    else
                    {
                        return CanCauseError.Error("No flag named {0} exists.", flagName);
                    }
                }
                else
                {
                    if (parameterNumber < parameters.Length)
                    {
                        parameters[parameterNumber](arg);
                    }
                    else
                    {
                        return CanCauseError.Error("Too many parameters.");
                    }
                }

            }
            return CanCauseError.NoError;
        }
    }
}
