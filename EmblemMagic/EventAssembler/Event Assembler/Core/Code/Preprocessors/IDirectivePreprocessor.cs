using Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros;
using Nintenlord.Event_Assembler.Core.Collections;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors
{
	internal interface IDirectivePreprocessor : IPreprocessor, IDisposable
	{
		Stack<bool> Include { get; }

		IDefineCollection DefCol { get; }

		Pool Pool { get; }

		IInputStream Input { get; }

		ILog Log { get; }

		IIncludeListener IncludeListener { get; set; }

		bool IsValidToDefine (string name);

		bool IsPredefined (string name);
	}
}
