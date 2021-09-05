using System;
using Nintenlord.Event_Assembler.Core.Code.Language;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core
{
	public static class DummyCodeLanguage
	{
		public static readonly Tuple<string, List<Priority>>[][] PointerList = new Tuple<string, List<Priority>>[0][]{};
	}
}
	