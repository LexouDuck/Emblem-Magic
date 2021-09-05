using Nintenlord.Utility;
using Nintenlord.Event_Assembler.Core.IO;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
	internal class Include : IDirective, INamed<string>, IParameterized
	{
		public string Name {
			get {
				return "include";
			}
		}

		public bool RequireIncluding {
			get {
				return true;
			}
		}

		public int MinAmountOfParameters {
			get {
				return 1;
			}
		}

		public int MaxAmountOfParameters {
			get {
				return 1;
			}
		}

		public CanCauseError Apply (string[] parameters, IDirectivePreprocessor host)
		{
			string file = IOHelpers.FindFile (host.Input.CurrentFile, parameters [0]);

			if (file.Length <= 0) {
				if (!host.IncludeListener.AllowsMissingFiles ())
					return CanCauseError.Error ("File " + parameters [0] + " not found.");

				file = IOHelpers.GetPrefferedFileName (host.Input.CurrentFile, parameters [0]);
			} else
				host.Input.OpenSourceFile (file);

			host.IncludeListener.IncludeTextFile (file);
			return CanCauseError.NoError;
		}
	}
}
