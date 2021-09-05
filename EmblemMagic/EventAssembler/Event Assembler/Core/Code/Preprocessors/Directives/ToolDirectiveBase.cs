using Nintenlord.Collections;
using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
    internal abstract class ToolDirectiveBase : INamed<string>, IParameterized, IDirective
    {
        public abstract string Name { get; }

        public int MinAmountOfParameters
        {
            get
            {
                return 1;
            }
        }

        public int MaxAmountOfParameters
        {
            get
            {
                return -1;
            }
        }

        public bool RequireIncluding
        {
            get
            {
                return true;
            }
        }

        public CanCauseError Apply(string[] parameters, IDirectivePreprocessor host)
        {
            string fileName = IO.IOHelpers.FindFile(host.Input.CurrentFile, GetToolFileName(parameters[0]));

            if (fileName.Length <= 0)
                return CanCauseError.Error("Tool " + parameters[0] + " not found.");

            // Based on http://stackoverflow.com/a/206347/1644720

            // Start the child process.
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(host.Input.CurrentFile);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = fileName;

            // Redirect the output stream of the child process.
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            string[] passedParams = parameters.GetRange(1);

            for (int i = 0; i < passedParams.Length; i++)
            {
                if (passedParams[i].ContainsWhiteSpace())
                    passedParams[i] = "\"" + passedParams[i] + "\"";
            }

            process.StartInfo.Arguments = passedParams.ToElementWiseString(" ", "", " --to-stdout");
            process.Start();

            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.

            MemoryStream outputBytes = new MemoryStream();
            process.StandardOutput.BaseStream.CopyTo(outputBytes);

            MemoryStream errorBytes = new MemoryStream();
            process.StandardError.BaseStream.CopyTo(errorBytes);

            process.WaitForExit();

            // For tools that err using stderr

            if (errorBytes.Length > 0)
                return CanCauseError.Error(Encoding.ASCII.GetString(errorBytes.GetBuffer()));

            // For tools that err using stdout and "ERROR: ..."

            byte[] output = outputBytes.GetBuffer().GetRange(0, (int)outputBytes.Length);

            if (output.Length >= 7 && Encoding.ASCII.GetString(output.GetRange(0, 7)) == "ERROR: ")
                return CanCauseError.Error(Encoding.ASCII.GetString(output.GetRange(7)));

            return ApplyIncludeTool(output, host);
        }

        public abstract CanCauseError ApplyIncludeTool(byte[] toolOutput, IDirectivePreprocessor host);

        private string GetToolFileName(string toolName)
        {
            // TODO: maybe use some standard helper so we can be platform independant in code?

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    return "\"./Tools/" + toolName + "\"";
                default:
                    return "\".\\Tools\\" + toolName + ".exe\"";
            }
        }
    }
}
