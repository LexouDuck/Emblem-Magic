using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code;
using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Event_Assembler.Core.Code.Preprocessors;
using Nintenlord.Event_Assembler.Core.Code.Templates;
using Nintenlord.Event_Assembler.Core.GBA;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using Nintenlord.IO;
using Nintenlord.Parser;
using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using Nintenlord.Utility.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core
{
	public static class Program
	{
		public static readonly StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

		public static IDictionary<string, EACodeLanguage> Languages;

		private static ProgramRunConfig runConfig = new ProgramRunConfig ();

		public class ProgramRunConfig
		{
			public enum RunExecType
			{
				GenDoc,
				GenNppHighlight,
				GenPNHighlight,
				Assemble,
				Disassemble,
			}

			public RunExecType execType;

			public bool quiet = false;

			public string language = null;
			public string rawsFolder = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "Language Raws");
			public string rawsExtension = ".txt";
			public bool isDirectory = true;
			public bool addEndGuards = false;
			public string inputFile = null;
			public string outputFile = null;
			public string errorFile = null;
			public string docHeader = null;
			public string docFooter = null;
			public string symbolOutputFile = null;

			public DisassemblyMode disassemblyMode = DisassemblyMode.Block;
			public int disassemblyOffset = -1;
			public Priority disassemblyPriority;
			public int disassemblySize = 0;

			public bool ppSimulation = false;

			public bool ppDepIgnoreMissingFiles = false;
			public bool ppDepIgnoreSystemFiles = false;
			public bool ppDepEnable = false;
			public string ppDepOutput = null;
			public bool ppDepAddEmptyTargets = false;
			public List<string> ppDepTargets = new List<string> ();

			public bool TrySetRawsPath (string path)
			{
				if (File.Exists (path)) {
					this.rawsFolder = path;
					this.isDirectory = false;

					return true;
				}

				if (Directory.Exists (path)) {
					this.rawsFolder = path;
					this.isDirectory = true;

					return true;
				}

				return false;
			}
		}

		public static ProgramRunConfig RunConfig {
			get {
				return Program.runConfig;
			}

			private set {
				Program.runConfig = value;
			}
		}

		public static bool CodesLoaded {
			get {
				return Program.Languages != null;
			}
		}



		private static int Main (string[] args)
		{
			TextWriterMessageLog writerMessageLog = new TextWriterMessageLog (Console.Error);
			StreamWriter logWriter = null;

			if ((Program.RunConfig = ReadProgramArguments (args, (ILog)writerMessageLog)) != null) {
				if (Program.RunConfig.errorFile != null) {
					logWriter = new StreamWriter (Program.RunConfig.errorFile);
					writerMessageLog.Writer = logWriter;
				}

				// doc generation does raw loading on its own, load in a standard manner for everything else
				if (Program.RunConfig.execType != ProgramRunConfig.RunExecType.GenDoc)
					Program.LoadCodes (false);

				switch (Program.RunConfig.execType) {

				case ProgramRunConfig.RunExecType.GenDoc:
					Program.MakeDoc (
						Program.RunConfig.outputFile,
						Program.RunConfig.rawsFolder,
						Program.RunConfig.rawsExtension,
						Program.RunConfig.isDirectory,
						Program.RunConfig.docHeader,
						Program.RunConfig.docFooter
					);

					break;

				case ProgramRunConfig.RunExecType.GenPNHighlight:
					try {
						HighlightingHelper.GetProgrammersNotepadlanguageDoc (
							(IEnumerable<EACodeLanguage>)Program.Languages.Values,
							Program.RunConfig.outputFile
						);
					} catch (Exception e) {
						writerMessageLog.AddError (e.Message);
					}

					break;

				case ProgramRunConfig.RunExecType.GenNppHighlight:
					throw new NotImplementedException ();

				case ProgramRunConfig.RunExecType.Disassemble:
					Program.Disassemble ((ILog)writerMessageLog);
					break;

				case ProgramRunConfig.RunExecType.Assemble:
					Program.Assemble ((ILog)writerMessageLog);
					break;

				}
			}

			int exitStatus = 0;

			if (writerMessageLog.ErrorCount != 0)
				exitStatus = 1;
			
			if (Program.RunConfig == null || !Program.RunConfig.quiet)
				writerMessageLog.PrintAll ();

			writerMessageLog.Clear ();

			if (logWriter != null)
				logWriter.Dispose ();

			return exitStatus;
		}

		private static void PrintUsage ()
		{
			// TODO?
		}

		private static ProgramRunConfig ReadProgramArguments (string[] args, ILog log)
		{
			IEnumerator<string> it = args.AsEnumerable ().GetEnumerator ();

			if (!it.MoveNext ()) {
				Program.PrintUsage ();
				return null;
			}

			ProgramRunConfig result = new ProgramRunConfig ();

			// First argument is always what kind of exec we want

			switch (it.Current) {

			case "doc":
				result.execType = ProgramRunConfig.RunExecType.GenDoc;
				break;

			case "plusplus":
				result.execType = ProgramRunConfig.RunExecType.GenNppHighlight;
				break;

			case "prognotepad":
				result.execType = ProgramRunConfig.RunExecType.GenPNHighlight;
				break;

			case "A":
			case "assemble":
				result.execType = ProgramRunConfig.RunExecType.Assemble;
				break;

			case "D":
			case "disassemble":
				result.execType = ProgramRunConfig.RunExecType.Disassemble;
				break;

			default:
				log.AddError ("Unknown run mode `{0}`", it.Current);
				return null;

			}

			// For Assembling & Disassembling, second argument is what game we're doing that for

			switch (result.execType) {

			case ProgramRunConfig.RunExecType.Assemble:
			case ProgramRunConfig.RunExecType.Disassemble:
				if (!it.MoveNext ()) {
					log.AddError ("You need to specify a game for which to (dis)assemble!");
					return null;
				}

				result.language = it.Current;
				break;

			}

			// From now on, the argument order doesn't matter

			while (it.MoveNext ()) {
				// -quiet
				if (it.Current.Equals ("-quiet")) {
					result.quiet = true;
					continue;
				}

				// -raws <file>
				if (it.Current.Equals ("-raws")) {
					if (!it.MoveNext ()) {
						log.AddError ("`-raws` passed without specifying a path.");
						return null;
					}

					if (!result.TrySetRawsPath (it.Current)) {
						log.AddError ("File or folder `{0}` doesn't exist.", it.Current);
						return null;
					}

					continue;
				}

				// -raws:<file>
				if (it.Current.StartsWith ("-raws:")) {
					string path = it.Current.Substring ("-raws:".Length);

					if (!result.TrySetRawsPath (path)) {
						log.AddError ("File or folder `{0}` doesn't exist.", path);
						return null;
					}

					continue;
				}

				// -rawsExt <ext>
				if (it.Current.Equals ("-rawsExt")) {
					if (!it.MoveNext ()) {
						log.AddError ("`-rawsExt` passed without specifying an extension.");
						return null;
					}

					if (it.Current.ContainsAnyOf (Path.GetInvalidFileNameChars ())) {
						log.AddError ("`{0}` isn't valid as a file extension.", it.Current);
						return null;
					}

					result.rawsExtension = it.Current;
					continue;
				}

				// -rawsExt:<ext>
				if (it.Current.StartsWith ("-rawsExt:")) {
					string ext = it.Current.Substring ("-rawsExt:".Length);

					if (ext.ContainsAnyOf (Path.GetInvalidFileNameChars ())) {
						log.AddError ("`{0}` isn't valid as a file extension.", ext);
						return null;
					}

					result.rawsExtension = ext;
					continue;
				}

				// -input <file>
				if (it.Current.Equals ("-input")) {
					if (!it.MoveNext ()) {
						log.AddError ("`-input` passed without specifying a file.");
						return null;
					}

					if (!File.Exists (it.Current)) {
						log.AddError ("File `{0}` doesn't exist.", it.Current);
						return null;
					}

					result.inputFile = it.Current;
					continue;
				}

				// -input:<file>
				if (it.Current.StartsWith ("-input:")) {
					string file = it.Current.Substring ("-input:".Length);

					if (!File.Exists (file)) {
						log.AddError ("File `{0}` doesn't exist.", file);
						return null;
					}

					result.inputFile = file;
					continue;
				}

				// -output <file>
				if (it.Current.Equals ("-output")) {
					if (!it.MoveNext ()) {
						log.AddError ("`-output` passed without specifying a file.");
						return null;
					}

					if (!IsValidFileName (it.Current)) {
						log.AddError ("`{0}` isn't a valid file name.", it.Current);
						return null;
					}

					result.outputFile = it.Current;
					continue;
				}

				// -output:<file>
				if (it.Current.StartsWith ("-output:")) {
					string file = it.Current.Substring ("-output:".Length);

					if (!IsValidFileName (file)) {
						log.AddError ("`{0}` isn't a valid file name", file);
						return null;
					}

					result.outputFile = file;
					continue;
				}

				// -error <file>
				if (it.Current.Equals ("-error")) {
					if (!it.MoveNext ()) {
						log.AddError ("`-error` passed without specifying a file.");
						return null;
					}

					if (!IsValidFileName (it.Current)) {
						log.AddError ("`{0}` isn't a valid file name.", it.Current);
						return null;
					}

					result.errorFile = it.Current;
					continue;
				}

				// -error:<file>
				if (it.Current.StartsWith ("-error:")) {
					string file = it.Current.Substring ("-error:".Length);

					if (!IsValidFileName (file)) {
						log.AddError ("`{0}` isn't a valid file name.", file);
						return null;
					}

					result.errorFile = file;
					continue;
				}

				// special disassembly-specific parameters
				if (result.execType == ProgramRunConfig.RunExecType.Disassemble) {
					// -addEndGuards
					if (it.Current.Equals ("-addEndGuards")) {
						result.addEndGuards = true;
						continue;
					}

					DisassemblyMode dMode;

					if (it.Current.TryGetEnum<DisassemblyMode> (out dMode)) {
						result.disassemblyMode = dMode;
						continue;
					}

					int dOffset;

					if (result.disassemblyOffset < 0 && it.Current.TryGetValue (out dOffset)) {
						result.disassemblyOffset = dOffset;
						continue;
					}

					Priority dPriority;

					if (it.Current.TryGetEnum<Priority> (out dPriority)) {
						result.disassemblyPriority = dPriority;
						continue;
					}

					int dSize;

					if (result.disassemblyMode == DisassemblyMode.Block && it.Current.TryGetValue (out dSize)) {
						result.disassemblySize = dSize;
						continue;
					}
				}

				// special assembly-specific parameters
				if (result.execType == ProgramRunConfig.RunExecType.Assemble) {
					// -symOutput <file>
					if (it.Current.Equals ("-symOutput")) {
						if (!it.MoveNext ()) {
							log.AddError ("`-symOutput` passed without specifying a file.");
							return null;
						}

						if (!IsValidFileName (it.Current)) {
							log.AddError ("`{0}` isn't a valid file name.", it.Current);
							return null;
						}

						result.symbolOutputFile = it.Current;
						continue;
					}

					// -symOutput:<file>
					if (it.Current.StartsWith ("-symOutput:")) {
						string file = it.Current.Substring ("-symOutput:".Length);

						if (!IsValidFileName (file)) {
							log.AddError ("`{0}` isn't a valid file name", file);
							return null;
						}

						result.symbolOutputFile = file;
						continue;
					}

					// -M
					if (it.Current.Equals ("-M")) {
						result.ppSimulation = true;
						result.ppDepEnable = true;
						continue;
					}

					// -MM
					if (it.Current.Equals ("-MM")) {
						result.ppSimulation = true;
						result.ppDepEnable = true;
						result.ppDepIgnoreSystemFiles = true;
						continue;
					}

					// -MD
					if (it.Current.Equals ("-MD")) {
						result.ppDepEnable = true;
						continue;
					}

					// -MMD
					if (it.Current.Equals ("-MMD")) {
						result.ppDepEnable = true;
						result.ppDepIgnoreSystemFiles = true;
						continue;
					}

					// -MG
					if (it.Current.Equals ("-MG")) {
						result.ppDepIgnoreMissingFiles = true;
						continue;
					}

					// -MP
					if (it.Current.Equals ("-MP")) {
						result.ppDepAddEmptyTargets = true;
						continue;
					}

					// -MF <file>
					if (it.Current.Equals ("-MF")) {
						if (!it.MoveNext ()) {
							log.AddError ("`-MF` passed without specifying a file.");
							return null;
						}

						if (!IsValidFileName (it.Current)) {
							log.AddError ("`{0}` isn't a valid file name.", it.Current);
							return null;
						}

						result.ppDepOutput = it.Current;
						continue;
					}

					// -MF:<file>
					if (it.Current.StartsWith ("-MF:")) {
						string file = it.Current.Substring ("-MF:".Length);

						if (!IsValidFileName (file)) {
							log.AddError ("`{0}` isn't a valid file name.", file);
							return null;
						}

						result.ppDepOutput = file;
						continue;
					}

					// -MT <name>
					if (it.Current.Equals ("-MT")) {
						if (!it.MoveNext ()) {
							log.AddError ("`-MT` passed without specifying a target.");
							return null;
						}


						if (it.Current.Length <= 0)
							result.ppDepTargets.Clear ();
						else
							result.ppDepTargets.Add (it.Current);
						
						continue;
					}

					// -MT:<name>
					if (it.Current.StartsWith ("-MT:")) {
						string name = it.Current.Substring ("-MT:".Length);

						if (name.Length <= 0)
							result.ppDepTargets.Clear ();
						else
							result.ppDepTargets.Add (name);

						continue;
					}
				}

				// special docgen-specific parameters
				if (result.execType == ProgramRunConfig.RunExecType.GenDoc) {

					// -docHeader <file>
					if (it.Current.Equals ("-docHeader")) {
						if (!it.MoveNext ()) {
							log.AddError ("`-docHeader` passed without specifying a file.");
							return null;
						}

						if (!File.Exists (it.Current)) {
							log.AddError ("File `{0}` doesn't exist.", it.Current);
							return null;
						}

						result.docHeader = it.Current;
						continue;
					}

					// -docHeader:<file>
					if (it.Current.StartsWith ("-docHeader:")) {
						string file = it.Current.Substring ("-docHeader:".Length);

						if (!File.Exists (file)) {
							log.AddError ("File `{0}` doesn't exist.", file);
							return null;
						}

						result.docHeader = file;
						continue;
					}

					// -docFooter <file>
					if (it.Current.Equals ("-docFooter")) {
						if (!it.MoveNext ()) {
							log.AddError ("`-docFooter` passed without specifying a file.");
							return null;
						}

						if (!File.Exists (it.Current)) {
							log.AddError ("File `{0}` doesn't exist.", it.Current);
							return null;
						}

						result.docFooter = it.Current;
						continue;
					}

					// -docFooter:<file>
					if (it.Current.StartsWith ("-docFooter:")) {
						string file = it.Current.Substring ("-docFooter:".Length);

						if (!File.Exists (file)) {
							log.AddError ("File `{0}` doesn't exist.", file);
							return null;
						}

						result.docFooter = file;
						continue;
					}
				}

				log.AddWarning ("Unhandled parameter `{0}`. Ignoring.", it.Current);
			}

			return result;
		}

		private static bool IsValidFileName (string name)
		{
			return !name.ContainsAnyOf (Path.GetInvalidPathChars ());
		}



		// EA GUI Entry point
		public static void Assemble(string inputFile, string outputFile, string languageName, ILog log)
		{
			Program.RunConfig.inputFile = inputFile;
			Program.RunConfig.outputFile = outputFile;
			Program.RunConfig.language = languageName;

			Assemble(log);
		}

		// Used by Emblem Magic (and could potentially be used by other external software)
		public static void Assemble(EACodeLanguage language,
			TextReader input, BinaryWriter output, ILog log)
		{
			List<string> stringList = new List<string>();
			stringList.Add("_" + language.Name + "_");
			stringList.Add("_EA_");
			using (IPreprocessor preprocessor = new Preprocessor(log))
			{
				preprocessor.AddReserved(language.GetCodeNames());
				preprocessor.AddDefined(stringList.ToArray());
				using (IInputStream inputStream = new PreprocessingInputStream(input, preprocessor))
				{
					new EAExpressionAssembler(language.CodeStorage,
						new TokenParser<int>(new Func<string, int>(StringExtensions.GetValue))).Assemble(inputStream, output, log);
				}
			}
		}

		private static void Assemble (ILog log)
		{
			TextReader input;
			bool inputIsFile;

			if (Program.RunConfig.inputFile != null) {
				input = File.OpenText (Program.RunConfig.inputFile);
				inputIsFile = false;
			} else {
				input = Console.In;
				inputIsFile = true;
			}

			using (IDirectivePreprocessor preprocessor = new Preprocessor (log)) {
				// preprocessor.AddReserved (eaCodeLanguage.GetCodeNames ());
				preprocessor.AddDefined (new string[] { "_" + Program.RunConfig.language + "_", "_EA_" });

				DependencyMakingIncludeListener depMaker = null;

				if (Program.RunConfig.ppDepEnable) {
					depMaker = new DependencyMakingIncludeListener ();
					preprocessor.IncludeListener = depMaker;
				}

				using (IInputStream inputStream = new PreprocessingInputStream (input, preprocessor)) {
					if (Program.RunConfig.ppSimulation) {
						// preprocess to null output
						while (inputStream.ReadLine () != null)
							;
					} else {
						if (Program.RunConfig.outputFile == null) {
							log.AddError ("No output file specified for assembly.");
							return;
						}

						string outFile = Program.RunConfig.outputFile;

						if (File.Exists (outFile) && File.GetAttributes (outFile).HasFlag ((Enum)FileAttributes.ReadOnly)) {
							log.AddError ("File `{0}` exists and cannot be written to.", outFile);
							return;
						}

						ChangeStream changeStream = new ChangeStream ();

						using (BinaryWriter output = new BinaryWriter ((Stream)changeStream)) {
							if (!Program.CodesLoaded)
								LoadCodes (false);
							
							EACodeLanguage language = Program.Languages [Program.RunConfig.language];

							EAExpressionAssembler assembler = new EAExpressionAssembler (language.CodeStorage, new TokenParser<int> (new Func<string, int> (StringExtensions.GetValue)));
							assembler.Assemble (inputStream, output, log);

							if (Program.RunConfig.symbolOutputFile != null) {
								// Outputting global symbols to another file

								try {
									if (File.Exists (Program.RunConfig.symbolOutputFile))
										File.Delete (Program.RunConfig.symbolOutputFile);

									using (FileStream fileStream = File.OpenWrite (Program.RunConfig.symbolOutputFile))
									using (StreamWriter symOut = new StreamWriter (fileStream))
										foreach (KeyValuePair<string, int> symbol in assembler.GetGlobalSymbols())
											symOut.WriteLine ("{0}={1}", symbol.Key, symbol.Value.ToHexString ("$"));
								} catch (Exception e) {
									log.AddError (e.ToString ());
								}
							}
						}

						if (log.ErrorCount == 0)
							using (Stream stream = (Stream)File.OpenWrite (outFile))
								changeStream.WriteToFile (stream);
					}
				}

				if (depMaker != null) {
					try {
						depMaker.GenerateMakeDependencies (log);
					} catch (Exception e) {
						log.AddError (e.ToString ());
					}
				}
			}

			if (inputIsFile)
				input.Close ();
		}

		// EA GUI Entry point
		public static void Disassemble(string inputFile, string outputFile, string languageName, bool addEndGuards, DisassemblyMode mode, int offset, Priority priority, int size, ILog messageLog)
		{
			Program.RunConfig.inputFile = inputFile;
			Program.RunConfig.outputFile = outputFile;
			Program.RunConfig.language = languageName;
			Program.RunConfig.addEndGuards = addEndGuards;
			Program.RunConfig.disassemblyMode = mode;
			Program.RunConfig.disassemblyOffset = offset;
			Program.RunConfig.disassemblyPriority = priority;
			Program.RunConfig.disassemblySize = size;

			Disassemble(messageLog);
		}

		// Used by Emblem Magic (and could potentially be used by other external software)
		public static void Disassemble(
			EACodeLanguage language,
			byte[] rom, string filename, TextWriter output,
			bool addEndGuards, DisassemblyMode mode,
			int offset, Priority priority, int size, ILog log)
		{
			if (offset > rom.Length)
				log.AddError("Offset is larger than size of ROM.");
			if (size <= 0 || size + offset > rom.Length)
				size = rom.Length - offset;
			IEnumerable<string[]> strArrays;
			string[] lines;
			switch (mode)
			{
				case DisassemblyMode.Block:
					strArrays = language.Disassemble(rom, offset, size, priority, addEndGuards, log);
					lines = CoreInfo.DefaultLines(language.Name, Path.GetFileName(filename), offset, new int?(size));
					break;
				case DisassemblyMode.ToEnd:
					strArrays = language.DisassembleToEnd(rom, offset, priority, addEndGuards, log);
					lines = CoreInfo.DefaultLines(language.Name, Path.GetFileName(filename), offset, new int?());
					break;
				case DisassemblyMode.Structure:
					strArrays = language.DisassembleChapter(rom, offset, addEndGuards, log);
					lines = CoreInfo.DefaultLines(language.Name, Path.GetFileName(filename), offset, new int?());
					break;
				default:
					throw new ArgumentException();
			}
			if (log.ErrorCount == 0)
			{
				if (filename.Length > 0)
				{
					output.WriteLine(Program.Frame(lines, "//", 1));
					output.WriteLine();
				}
				foreach (string[] strArray in strArrays)
					output.WriteLine(((IEnumerable<string>)strArray).ToElementWiseString(" ", "", ""));
			}
		}

		private static void Disassemble (ILog log) {
			if (!File.Exists (Program.RunConfig.inputFile)) {
				log.AddError ("File `{0}` doesn't exist.", Program.RunConfig.inputFile);
				return;
			}

			if (File.Exists (Program.RunConfig.outputFile) && File.GetAttributes (Program.RunConfig.outputFile).HasFlag (FileAttributes.ReadOnly)) {
				log.AddError ("Output cannot be written to. It is read-only.");
				return;
			}

			if (!Program.CodesLoaded)
				LoadCodes (false);

			EACodeLanguage eaCodeLanguage = Program.Languages [Program.RunConfig.language];
			byte[] code = File.ReadAllBytes (Program.RunConfig.inputFile);

			if (Program.RunConfig.disassemblyOffset > code.Length) {
				log.AddError ("Offset is larger than size of file.");
				return;
			}

			int size   = Program.RunConfig.disassemblySize;
			int offset = Program.RunConfig.disassemblyOffset;

			if (size <= 0 || size + offset > code.Length)
				size = code.Length - offset;

			IEnumerable<string[]> strArrays;
			string[] lines;

			switch (Program.RunConfig.disassemblyMode) {

			case DisassemblyMode.Block:
				strArrays = eaCodeLanguage.Disassemble (
					code,
					offset,
					size,
					Program.RunConfig.disassemblyPriority,
					Program.RunConfig.addEndGuards,
					log
				);

				lines = CoreInfo.DefaultLines (
					eaCodeLanguage.Name,
					Path.GetFileName (Program.RunConfig.inputFile),
					offset,
					new int? (size)
				);

				break;

			case DisassemblyMode.ToEnd:
				strArrays = eaCodeLanguage.DisassembleToEnd (
					code,
					offset,
					Program.RunConfig.disassemblyPriority,
					Program.RunConfig.addEndGuards,
					log
				);

				lines = CoreInfo.DefaultLines (
					eaCodeLanguage.Name,
					Path.GetFileName (Program.RunConfig.inputFile),
					offset,
					new int? ()
				);

				break;

			case DisassemblyMode.Structure:
				strArrays = eaCodeLanguage.DisassembleChapter (
					code,
					offset,
					Program.RunConfig.addEndGuards,
					log
				);

				lines = CoreInfo.DefaultLines (
					eaCodeLanguage.Name,
					Path.GetFileName (Program.RunConfig.inputFile),
					offset,
					new int? ()
				);

				break;

			default:
				throw new ArgumentException ();

			}

			if (log.ErrorCount == 0) {
				using (StreamWriter streamWriter = new StreamWriter (Program.RunConfig.outputFile)) {
					streamWriter.WriteLine ();
					streamWriter.WriteLine (Program.Frame (lines, "//", 1));
					streamWriter.WriteLine ();

					foreach (string[] strArray in strArrays)
						streamWriter.WriteLine (((IEnumerable<string>)strArray).ToElementWiseString<string> (" ", "", ""));
				}
			}
		}

		// EA GUI Entry point
		public static LanguageProcessor LoadCodes(string rawsFolder, string extension, bool isDirectory, bool collectDocCodes)
		{
			Program.RunConfig.rawsFolder = rawsFolder;
			Program.RunConfig.rawsExtension = extension;
			Program.RunConfig.isDirectory = isDirectory;

			return (LoadCodes(collectDocCodes));
		}

		private static LanguageProcessor LoadCodes(bool collectDoc) {
			Program.Languages = (IDictionary<string, EACodeLanguage>)new Dictionary<string, EACodeLanguage> ();

			LanguageProcessor languageProcessor = new LanguageProcessor (collectDoc, new TemplateComparer (), Program.stringComparer);
			IPointerMaker pointerMaker = (IPointerMaker)new GBAPointerMaker ();

			if (Program.RunConfig.isDirectory)
				languageProcessor.ProcessCode (Program.RunConfig.rawsFolder, Program.RunConfig.rawsExtension);
			else
				languageProcessor.ProcessCode (Program.RunConfig.rawsFolder);

			foreach (KeyValuePair<string, ICodeTemplateStorer> language in languageProcessor.Languages) {
				Tuple<string, List<Priority>>[][] pointerList;

				switch (language.Key) {

				case "FE6":
					pointerList = FE6CodeLanguage.PointerList;
					break;

				case "FE7":
					pointerList = FE7CodeLanguage.PointerList;
					break;

				case "FE8":
					// pointerList = DummyCodeLanguage.PointerList;
					pointerList = FE8CodeLanguage.PointerList;
					break;

				default:
					pointerList = DummyCodeLanguage.PointerList;
					break;
				
					// throw new NotSupportedException ("Language " + language.Key + " not supported.");

				}

				Program.Languages [language.Key] = new EACodeLanguage (
					language.Key,
					pointerMaker,
					pointerList,
					language.Value,
					Program.stringComparer
				);
			}
			return (languageProcessor);
		}

		public static void MakeDoc (string output, string rawsFolder, string extension, bool isDirectory, string header, string footer)
		{
			LanguageProcessor languageProcessor = new LanguageProcessor (true, (IComparer<ICodeTemplate>)new TemplateComparer (), Program.stringComparer);

			if (isDirectory)
				languageProcessor.ProcessCode (rawsFolder, extension);
			else
				languageProcessor.ProcessCode (rawsFolder);

			using (StreamWriter text = File.CreateText (output)) {
				if (header != null) {
					text.WriteLine (File.ReadAllText (header));
					text.WriteLine ();
				}

				languageProcessor.WriteDocs ((TextWriter)text);

				if (footer == null)
					return;

				text.WriteLine (File.ReadAllText (footer));
				text.WriteLine ();
			}
		}

		public static void Preprocess (string originalFile, string outputFile, string game, ILog messageLog)
		{
			EACodeLanguage eaCodeLanguage = Program.Languages [game];

			using (IPreprocessor preprocessor = (IPreprocessor)new Preprocessor (messageLog)) {
				preprocessor.AddReserved (eaCodeLanguage.GetCodeNames ());
				preprocessor.AddDefined (new string[] { "_" + game + "_", "_EA_" });

				using (StreamReader streamReader = File.OpenText (originalFile)) {
					using (IInputStream inputStream = (IInputStream)new PreprocessingInputStream ((TextReader)streamReader, preprocessor)) {
						StringWriter stringWriter = new StringWriter ();

						while (true) {
							string str = inputStream.ReadLine ();

							if (str != null)
								stringWriter.WriteLine (str);
							else
								break;
						}

						messageLog.AddMessage ("Processed code:\n" + stringWriter.ToString () + "\nEnd processed code");
					}
				}
			}
		}

		private static string Frame (string[] lines, string toFrameWith, int padding)
		{
			int num = 0;

			for (int index = 0; index < lines.Length; ++index)
				num = Math.Max (num, lines [index].Length);

			string str1 = toFrameWith.Repeat (padding * 2 + toFrameWith.Length * 2 + num);
			string str2 = toFrameWith + " ".Repeat (padding * 2 + num) + toFrameWith;
			string str3 = " ".Repeat (padding);

			StringBuilder stringBuilder = new StringBuilder ();

			stringBuilder.AppendLine (str1);
			stringBuilder.AppendLine (str2);

			foreach (string line in lines)
				stringBuilder.AppendLine (toFrameWith + str3 + line.PadRight (num, ' ') + str3 + toFrameWith);

			stringBuilder.AppendLine (str2);
			stringBuilder.AppendLine (str1);

			return stringBuilder.ToString ();
		}
	}
}
