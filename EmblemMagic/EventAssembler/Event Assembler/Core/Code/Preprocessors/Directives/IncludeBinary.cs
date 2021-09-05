using Nintenlord.Utility;
using System.IO;
using Nintenlord.Event_Assembler.Core.IO;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives
{
	internal class IncludeBinary : IDirective, INamed<string>, IParameterized
	{
		public string Name {
			get {
				return "incbin";
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
				host.Input.OpenBinaryFile (file);

			host.IncludeListener.IncludeBinaryFile (file);
			return CanCauseError.NoError;
		}
	}
}
