using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Preprocessors.BuiltInMacros;
using Nintenlord.Event_Assembler.Core.Code.Preprocessors.Directives;
using Nintenlord.Event_Assembler.Core.Collections;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors {
	public class Preprocessor : IDirectivePreprocessor, IPreprocessor, IDisposable {
		private const bool includedFilesAsNewScope = true;
		private Stack<bool> include;
		private IDefineCollection defCol;
		private Pool pool;
		private List<string> predefined;
		private List<string> reserved;
		private CurrentLine curLine;
		private CurrentFile curFile;
		private Dictionary<string, IDirective> directives;
		private ILog messageLog;
		private IInputStream inputStream;
		private IIncludeListener includeListener;
		private int blockCommentDepth;

		public ILog Log {
			get { return messageLog; }
		}

		public Stack<bool> Include {
			get {
				return this.include;
			}
		}

		public IDefineCollection DefCol {
			get {
				return this.defCol;
			}
		}

		public Pool Pool {
			get {
				return this.pool;
			}
		}

		public IInputStream Input {
			get {
				return this.inputStream;
			}
		}

		public IIncludeListener IncludeListener {
			get {
				return this.includeListener;
			}

			set {
				this.includeListener = value;
			}
		}

		public Preprocessor(ILog messageLog/*, string outputFile*/) {
			this.messageLog = messageLog;
			this.predefined = new List<string>();
			this.reserved = new List<string>();
			this.pool = new Pool();
			this.curLine = new CurrentLine();
			this.curFile = new CurrentFile();
			this.blockCommentDepth = 0;
			DefineCollectionOptimized collectionOptimized = new DefineCollectionOptimized();
			collectionOptimized["IsDefined"] = (IMacro) new IsDefined((IDefineCollection) collectionOptimized);
			collectionOptimized["DeconstVector"] = (IMacro) new DeconstructVector();
			collectionOptimized["ConstVector"] = (IMacro) new BuildVector();
			collectionOptimized["ToParameters"] = (IMacro) new VectorToParameter();
			collectionOptimized["Signum"] = (IMacro) new Signum();
			collectionOptimized["Switch"] = (IMacro) new Switch();
			collectionOptimized["String"] = (IMacro) new InsertText();
			collectionOptimized["AddToPool"] = (IMacro) this.pool;
			collectionOptimized["_line_"] = (IMacro) this.curLine;
			collectionOptimized["_file_"] = (IMacro) this.curFile;
			//collectionOptimized["_rom_"] = (IMacro) outputFile;

			this.defCol = (IDefineCollection) collectionOptimized;

			this.include = new Stack<bool>();
			this.include.Push(true);

			this.includeListener = new DummyIncludeListener();

			this.directives = ((IEnumerable<IDirective>) new IDirective[14] {
				(IDirective) new IfDefined(),
				(IDirective) new IfNotDefined(),
				(IDirective) new Define(),
				(IDirective) new DumpPool(),
				(IDirective) new Else(),
				(IDirective) new EndIf(),
				(IDirective) new Include(),
				(IDirective) new IncludeBinary(),
				(IDirective) new Undefine(),
				(IDirective) new IncludeToolEvent(),
				(IDirective) new IncludeToolEventAlias(),
				(IDirective) new IncludeToolBinary(),
				(IDirective) new RunTool(),
				(IDirective) new EasterEgg()
			}).GetDictionary<string, IDirective>();
		}

		public void AddDefined(IEnumerable<string> original) {
			this.predefined.AddRange(original);
		}

		public void AddReserved(IEnumerable<string> reserved) {
			this.reserved.AddRange(reserved);
		}

		public string Process(string line, IInputStream inputStream) {
			this.curLine.Stream = inputStream;
			this.curFile.Stream = inputStream;
			this.inputStream = inputStream;
			StringBuilder line1 = new StringBuilder(line);
			if (!Nintenlord.Utility.Strings.Parser.ReplaceCommentsWith(line1, ' ', ref this.blockCommentDepth))
				this.messageLog.AddError(inputStream.GetErrorString("Error removing comments"));
			line = line1.ToString().Trim();
//      foreach (string oldValue in this.predefined)
//          newLine = newLine.Replace(oldValue, " ");
			if (line.FirstNonWhiteSpaceIs('#')) {
				CanCauseError<string> canCauseError = this.defCol.ApplyPreprocessorDefines(line);

				if (!canCauseError.CausedError) {
					this.HandleDirective(canCauseError.Result);
					return "";
				} else if (!this.include.And()) {
					return "";
				} else {
					this.messageLog.AddError(inputStream.GetErrorString(canCauseError.ErrorMessage));
					return line;
				}
			} else {
				if (Program.RunConfig.ppSimulation)
					return "";
				
				CanCauseError<string> customDefines = this.defCol.ApplyDefines(line);

				if (this.include.And() && !customDefines.CausedError) {
					string newLine = customDefines.Result;

					if (this.defCol.ContainsName("USING_CODE"))
						newLine = Preprocessor.HandleCODE(newLine);
					
					return newLine;
				} else if (!this.include.And()) {
					return "";
				}

				this.messageLog.AddError(inputStream.GetErrorString(customDefines.ErrorMessage));
				return line;
			}
		}

		public static string HandleCODE(string newLine) {
			int startIndex = newLine.IndexOf("CODE");
			if (startIndex < 0)
				return newLine;
			string[] strArray = newLine.Remove(startIndex, 4).Split(' ');
			StringBuilder stringBuilder = new StringBuilder();
			bool flag1 = false;
			bool flag2 = false;
			foreach (string str in strArray) {
				bool flag3 = str.StartsWith("0x");
				if (flag2) {
					if (!flag3)
						stringBuilder.Append("; WORD ");
				} else if (flag1) {
					if (flag3)
						stringBuilder.Append("; BYTE ");
				} else if (flag3)
					stringBuilder.Append("BYTE ");
				else
					stringBuilder.Append("WORD ");
				stringBuilder.Append(str);
				stringBuilder.Append(' ');
				flag2 = flag3;
				flag1 = !flag3;
			}
			return stringBuilder.ToString();
		}

		public void Dispose() {
			if (this.pool.AmountOfLines > 0)
				this.messageLog.AddWarning("Pool contains undumped lines at the end");
			if (this.include.Count <= 1)
				return;
			this.messageLog.AddWarning("#ifdef's are missing at the end");
		}

		public bool IsValidToDefine(string name) {
			if (!this.reserved.Contains(name))
				return this.predefined.Contains(name);
			return true;
		}

		public void IncludeFile(string file) {
			this.inputStream.OpenSourceFile(file);
		}

		public void IncludeBinary(string file) {
			this.inputStream.OpenBinaryFile(file);
		}

		public bool IsPredefined(string name) {
			return this.predefined.Contains(name);
		}

		private void HandleDirective(string line) {
			string[] parameters1 = Crazycolorz5.Parser.SplitToParameters(line);
			string key;
			int length;
			if (parameters1[0].Equals("#")) {
				key = parameters1[1];
				length = parameters1.Length - 2;
			} else {
				key = parameters1[0].TrimStart('#');
				length = parameters1.Length - 1;
			}
			string[] parameters2 = new string[length];
			Array.Copy(parameters1, parameters1.Length - length, (Array) parameters2, 0, length);
			IDirective parameterized;
			if (this.directives.TryGetValue(key, out parameterized)) {
				string error1;
				if (parameterized.Matches("Directive " + key, length, out error1)) {
					if (parameterized.RequireIncluding) {
						if (!this.include.And())
							return;
						CanCauseError error2 = parameterized.Apply(parameters2, (IDirectivePreprocessor) this);
						if (!(bool) error2)
							return;
						this.messageLog.AddError(this.inputStream.GetErrorString(error2));
					} else {
						CanCauseError error2 = parameterized.Apply(parameters2, (IDirectivePreprocessor) this);
						if (!(bool) error2)
							return;
						this.messageLog.AddError(this.inputStream.GetErrorString(error2));
					}
				} else
					this.messageLog.AddError(this.inputStream.GetErrorString(error1));
			} else
				this.messageLog.AddError(this.inputStream.GetErrorString(": No directive with the name #" + key + " exists"));
		}
	}
}
