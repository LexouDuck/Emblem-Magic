using System;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors
{
	public interface IIncludeListener
	{
		bool AllowsMissingFiles();

		void IncludeTextFile(string fPath);
		void IncludeBinaryFile(string fPath);
	}
}
