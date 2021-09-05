using System;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors
{
	public class DummyIncludeListener : IIncludeListener
	{
		public bool AllowsMissingFiles() {
			return false;
		}

		public void IncludeTextFile(string fPath) {
			// System.Console.WriteLine (string.Format ("Included Text {0}", fPath));
		}

		public void IncludeBinaryFile(string fPath) {
			// System.Console.WriteLine (string.Format ("Included Binary {0}", fPath));
		}
	}
}
