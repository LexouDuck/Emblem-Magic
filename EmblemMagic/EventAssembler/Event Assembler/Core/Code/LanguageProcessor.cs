using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Event_Assembler.Core.Code.Templates;
using Nintenlord.Utility;
using Nintenlord.Utility.Strings;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code
{
	public class LanguageProcessor
	{
		private readonly bool collectDocComments;
		private readonly IComparer<ICodeTemplate> templateComparer;
		private readonly StringComparer stringComparer;
		private IDictionary<string, ICodeTemplateStorer> languages;
		private IDictionary<string, List<LanguageProcessor.LanguageElement>> elements;
		private IDictionary<string, List<LanguageProcessor.DocCode>> docs;

		public IDictionary<string, ICodeTemplateStorer> Languages {
			get {
				return this.languages;
			}
		}



		public LanguageProcessor ()
			: this (false, (IComparer<ICodeTemplate>)Comparer<ICodeTemplate>.Default, StringComparer.OrdinalIgnoreCase)
		{}
		public LanguageProcessor (IComparer<ICodeTemplate> templateComparer)
			: this (false, templateComparer, StringComparer.OrdinalIgnoreCase)
		{}
		public LanguageProcessor (bool collectDocComments)
			: this (collectDocComments, (IComparer<ICodeTemplate>)Comparer<ICodeTemplate>.Default, StringComparer.OrdinalIgnoreCase)
		{}
		public LanguageProcessor (bool collectDocComments, IComparer<ICodeTemplate> equalityComparer, StringComparer stringComparer)
		{
			this.collectDocComments = collectDocComments;
			this.templateComparer = equalityComparer;
			this.stringComparer = stringComparer;
			this.docs = new SortedDictionary<string, List<DocCode>> (new NaturalComparer ());
			this.languages = new Dictionary<string, ICodeTemplateStorer> ();
		}

		public void ProcessCode (string folder, string extension)
		{
			if (!Directory.Exists (folder))
				throw new DirectoryNotFoundException ("Folder " + folder + " not found.");

			DirectoryInfo directoryInfo = new DirectoryInfo (folder);

			folder = Path.GetFullPath (folder);

			FileInfo[] files = directoryInfo.GetFiles ("*" + extension, SearchOption.AllDirectories);

			this.elements = (IDictionary<string, List<LanguageProcessor.LanguageElement>>)new Dictionary<string, List<LanguageProcessor.LanguageElement>> ();

			foreach (FileInfo fileInfo in files)
				this.ParseLinesInFile (fileInfo.FullName.Substring (folder.Length + 1, fileInfo.FullName.Length - folder.Length - extension.Length - 1), File.ReadLines (fileInfo.FullName));
		}

		public void ProcessCode (string file)
		{
			if (!File.Exists (file))
				throw new DirectoryNotFoundException ("File " + file + " not found.");

			ParseLinesInFile (Path.GetFileName (file), File.ReadLines (file));
		}

		private void ParseLinesInFile (string file, IEnumerable<string> lines)
		{
			List<LanguageProcessor.LanguageElement> languageElementList = new List<LanguageProcessor.LanguageElement> ();
			LanguageProcessor.LanguageElement languageElement = new LanguageProcessor.LanguageElement ();

			foreach (string line in lines) {
				if (line.ContainsNonWhiteSpace ()) {
					if ((int)line [0] == 35) {
						if (this.collectDocComments && line.Length > 1 && (int)line [1] == 35)
							languageElement.AddDoc (line.Substring (2));
					} else {
						languageElement.SetMainLine (line);
						languageElementList.Add (languageElement);
						languageElement = new LanguageProcessor.LanguageElement ();
					}
				}
			}

			if (this.collectDocComments) {
				int index = 0;

				while (index < languageElementList.Count) {
					LanguageProcessor.DocCode doc = this.MakeCode ((IList<LanguageProcessor.LanguageElement>)languageElementList, ref index);
          
					foreach (string language in doc.languages) {
						this.AddCode (doc, language);
					}

					this.docs.GetOldOrSetNew<string, List<LanguageProcessor.DocCode>> (file.Replace ('\\', '.')).Add (doc);
				}

				this.elements [file] = languageElementList;
			} else {
				int index = 0;

				while (index < languageElementList.Count) {
					LanguageProcessor.DocCode doc = this.MakeCode ((IList<LanguageProcessor.LanguageElement>)languageElementList, ref index);

					foreach (string language in doc.languages)
						this.AddCode (doc, language);
					
				}
			}
		}

		private void AddCode (LanguageProcessor.DocCode doc, string language)
		{
			ICodeTemplateStorer codeTemplateStorer;
			if (!this.languages.TryGetValue (language, out codeTemplateStorer))
				this.languages [language] = (ICodeTemplateStorer)new CodeTemplateStorer (this.templateComparer);
			this.languages [language].AddCode (doc.code, doc.priority);
			if (doc.code.ID != 0) {
				ICodeTemplate fourByte = doc.code.CopyWithNewName ("_0x" + doc.code.ID.ToString ("X4"));
				fourByte.CanBeDisassembled = false;
				this.languages [language].AddCode (fourByte, doc.priority);
				if (doc.code.ID <= 0xFF) {
					ICodeTemplate twoByte = doc.code.CopyWithNewName ("_0x" + doc.code.ID.ToString ("X2"));
					twoByte.CanBeDisassembled = false;
					this.languages [language].AddCode (twoByte, doc.priority);
				}
			}
		}

		private LanguageProcessor.DocCode MakeCode (IList<LanguageProcessor.LanguageElement> elements, ref int index)
		{
			List<LanguageProcessor.LanguageElement> languageElementList = new List<LanguageProcessor.LanguageElement> ();
			do {
				languageElementList.Add (elements [index]);
				++index;
			} while (index < elements.Count && elements [index].IsParameter);
			List<string> stringList = new List<string> ();
			Priority priority;
			ICodeTemplate code = this.ParseCode ((IList<LanguageProcessor.LanguageElement>)languageElementList, (ICollection<string>)stringList, out priority);
			LanguageProcessor.DocCode docCode = new LanguageProcessor.DocCode ();
			docCode.code = code;
			docCode.priority = priority;
			docCode.languages = stringList.Count <= 0 ? this.languages.Keys.ToArray<string> () : stringList.ToArray ();
			if (this.collectDocComments) {
				docCode.mainDoc = new List<string> ((IEnumerable<string>)languageElementList [0].GetDocLines ());
				docCode.parameterDocs = new Dictionary<string, List<string>> ();
				for (int index1 = 1; index1 < languageElementList.Count; ++index1)
					docCode.parameterDocs.GetOldOrSetNew<string, List<string>> (languageElementList [index1].ParsedLine.name).AddRange ((IEnumerable<string>)languageElementList [index1].GetDocLines ());
			}
			return docCode;
		}

		private ICodeTemplate ParseCode (IList<LanguageProcessor.LanguageElement> lines, ICollection<string> usedLanguages, out Priority priority)
		{
			LanguageProcessor.ParsedLine parsedLine1 = lines [0].ParsedLine;
			priority = Priority.none;
			bool canBeRepeated = false;
			bool chechForProblems = true;
			bool end = false;
			bool canBeDisassembled = true;
			bool canBeAssembled = true;
			int num1 = 1;
			bool flag1 = false;
			byte num2 = 0;
			int num3 = 4;
			foreach (string flag2 in parsedLine1.flags) {
				if (flag2.StartsWith ("language") || flag2.StartsWith ("game")) {
					string[] strArray = flag2.Split (new char[1]{ ':' }, StringSplitOptions.RemoveEmptyEntries);
					for (int index = 1; index < strArray.Length; ++index)
						usedLanguages.Add (strArray [index].Trim ());
				} else if (flag2.StartsWith ("priority")) {
					int num4 = flag2.IndexOf (':');
					string str = flag2.Substring (num4 + 1);
					if (!((IEnumerable<string>)Enum.GetNames (typeof(Priority))).Contains<string> (str))
						throw new FormatException ("Error in enum priority: " + flag2);
					priority = (Priority)Enum.Parse (typeof(Priority), str);
				} else if (flag2.StartsWith ("repeatable"))
					canBeRepeated = true;
				else if (flag2.StartsWith ("unsafe"))
					chechForProblems = false;
				else if (flag2.StartsWith ("end"))
					end = true;
				else if (flag2.StartsWith ("noDisassembly"))
					canBeDisassembled = false;
				else if (flag2.StartsWith ("noAssembly"))
					canBeAssembled = false;
				else if (flag2.StartsWith ("indexMode")) {
					int num4 = flag2.IndexOf (':');
					num1 = int.Parse (flag2.Substring (num4 + 1));
				} else if (flag2.StartsWith ("terminatingList")) {
					int num4 = flag2.IndexOf (':');
					num2 = (byte)int.Parse (flag2.Substring (num4 + 1));
					flag1 = true;
				} else {
					if (!flag2.StartsWith ("offsetMod"))
						throw new FormatException ("Unknown option " + flag2 + " in parameter " + parsedLine1.name);
					int num4 = flag2.IndexOf (':');
					num3 = (int)(byte)int.Parse (flag2.Substring (num4 + 1));
				}
			}
			parsedLine1.number2 = parsedLine1.number2 * num1;
			List<TemplateParameter> templateParameterList = new List<TemplateParameter> ();
			for (int index = 1; index < lines.Count; ++index) {
				LanguageProcessor.ParsedLine parsedLine2 = lines [index].ParsedLine;
				parsedLine2.number1 *= num1;
				parsedLine2.number2 *= num1;
				TemplateParameter parameter = LanguageProcessor.ParsedLine.ParseParameter (parsedLine2);
				templateParameterList.Add (parameter);
			}
			return !flag1 ? (ICodeTemplate)new CodeTemplate (parsedLine1.name, parsedLine1.number1, parsedLine1.number2, (IEnumerable<TemplateParameter>)templateParameterList, canBeRepeated, chechForProblems, end, num3, canBeAssembled, canBeDisassembled, this.stringComparer) : (ICodeTemplate)new TerminatingStringTemplate (parsedLine1.name, (IEnumerable<TemplateParameter>)templateParameterList, (int)num2, num3, this.stringComparer);
		}

		public void WriteDocs (TextWriter writer)
		{
			IndentedTextWriter writer1 = new IndentedTextWriter (writer, " ");
			List<string> a = new List<string> ();
			foreach (KeyValuePair<string, List<LanguageProcessor.DocCode>> doc in (IEnumerable<KeyValuePair<string, List<LanguageProcessor.DocCode>>>) this.docs) {
				string[] strArray = doc.Key.Split ('.');
				int equalsInBeginning = a.GetEqualsInBeginning<string> ((IList<string>)strArray);
				writer1.Indent -= a.Count;
				writer1.Indent += equalsInBeginning;
				for (int index = equalsInBeginning; index < strArray.Length; ++index) {
					writer1.WriteLine (strArray [index]);
					++writer1.Indent;
				}
				Dictionary<string, List<LanguageProcessor.DocCode>> dict = new Dictionary<string, List<LanguageProcessor.DocCode>> ();
				foreach (LanguageProcessor.DocCode docCode in doc.Value)
					dict.GetOldOrSetNew<string, List<LanguageProcessor.DocCode>> (docCode.code.Name).Add (docCode);
				foreach (List<LanguageProcessor.DocCode> list in dict.Values) {
					this.WriteCode (list, writer1);
					writer.WriteLine ();
				}
				a.Clear ();
				a.AddRange ((IEnumerable<string>)strArray);
			}
        }

        public string GetDoc (string code, string game)
        {
            string result = "";
            List<string> result_format = new List<string>();
            List<string> result_params = new List<string>();
            string format;
            string parameters = "";
            foreach (KeyValuePair<string, List<DocCode>> file in this.docs)
            {
                if (file.Value == null)
                    continue;
                foreach (DocCode doc in file.Value)
                {
                    if (doc.languages.Contains(game))
                    {
                        format = doc.ToString();
                        if (format == code || format.StartsWith(code + ' '))
                        {
                            if (result.Length == 0)
                            {
                                if (doc.mainDoc != null)
                                {
                                    foreach (string line in doc.mainDoc)
                                        result += line + '\n';
                                }
                            }
                            format = format.Replace(",", " ");
                            if (!result_format.Contains(format))
                                result_format.Add(format);

                            if (doc.parameterDocs != null && doc.parameterDocs.Count > 0)
                            {
                                foreach (KeyValuePair<string, List<string>> param in doc.parameterDocs)
                                {
                                    if (result_params.Contains(param.Key))
                                        continue;
                                    else result_params.Add(param.Key);
                                    parameters += "\n" + param.Key + "\t- ";
                                    if (param.Value == null || param.Value.Count == 0)
                                    {
                                        parameters += "(no info available)";
                                    }
                                    else for (int i = 0; i < param.Value.Count; i++)
                                        {
                                            if (i > 0) parameters += "\n\t";
                                            parameters += param.Value[i];
                                        }
                                }
                            }
                        }
                    }
                }
            }
            if (result_format.Count > 0) result += "\nFormat:\n" + string.Join("\n", result_format);
            if (result_params.Count > 0) result += "\n\nParameters:" + parameters;
            return result;
        }

        private void WriteCode (List<LanguageProcessor.DocCode> list, IndentedTextWriter writer)
		{
			if (list [0].code is CodeTemplate) {
				CodeTemplate[] codeTemplateArray = Array.ConvertAll<LanguageProcessor.DocCode, CodeTemplate> (list.ToArray (), (Converter<LanguageProcessor.DocCode, CodeTemplate>)(x => x.code as CodeTemplate));
				LanguageProcessor.WriteCodeTemplates ((IList<LanguageProcessor.DocCode>)list, writer, (IList<CodeTemplate>)codeTemplateArray);
			} else if (list [0].code is TerminatingStringTemplate) {
				TerminatingStringTemplate[] terminatingStringTemplateArray = Array.ConvertAll<LanguageProcessor.DocCode, TerminatingStringTemplate> (list.ToArray (), (Converter<LanguageProcessor.DocCode, TerminatingStringTemplate>)(x => x.code as TerminatingStringTemplate));
				LanguageProcessor.WriteTerminatingStringTemplate ((IList<LanguageProcessor.DocCode>)list, writer, (IList<TerminatingStringTemplate>)terminatingStringTemplateArray);
			} else {
				if (!(list [0].code is IFixedDocString))
					return;
				foreach (string str in (list[0].code as IFixedDocString).DocString.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
					writer.WriteLine (str);
			}
		}

		private static void WriteTerminatingStringTemplate (IList<LanguageProcessor.DocCode> list, IndentedTextWriter writer, IList<TerminatingStringTemplate> templates)
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>> ((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase);
			Dictionary<string, List<TerminatingStringTemplate>> dict = new Dictionary<string, List<TerminatingStringTemplate>> ();
			for (int index = 0; index < templates.Count; ++index) {
				string humanString = ((IEnumerable<string>)list [index].languages).ToHumanString<string> ();
				dict.GetOldOrSetNew<string, List<TerminatingStringTemplate>> (humanString).Add (templates [index]);
				dictionary.GetOldOrSetNew<string, List<string>> (templates [index].Parameter.name).AddRange ((IEnumerable<string>)list [index].parameterDocs [templates [index].Parameter.name]);
			}
			foreach (KeyValuePair<string, List<TerminatingStringTemplate>> keyValuePair in dict) {
				writer.WriteLine (keyValuePair.Key + ":");
				++writer.Indent;
				foreach (TerminatingStringTemplate template in keyValuePair.Value)
					TerminatingStringTemplate.WriteDoc ((TextWriter)writer, template);
				--writer.Indent;
			}
			writer.WriteLineNoTabs ("");
			++writer.Indent;
			string[] strArray = (string[])null;
			foreach (LanguageProcessor.DocCode docCode in (IEnumerable<LanguageProcessor.DocCode>) list) {
				if (docCode.mainDoc.Count > 0) {
					strArray = docCode.mainDoc.ToArray ();
					break;
				}
			}
			if (strArray != null) {
				foreach (string str in list[0].mainDoc)
					writer.WriteLine (str);
			}
			if (dictionary.Count > 0)
				LanguageProcessor.WriteParameters (writer, dictionary);
			--writer.Indent;
		}

		private static void WriteCodeTemplates (IList<LanguageProcessor.DocCode> list, IndentedTextWriter indentedWriter, IList<CodeTemplate> templates)
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>> ((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase);
			Dictionary<string, List<CodeTemplate>> dict = new Dictionary<string, List<CodeTemplate>> ();
			for (int index = 0; index < templates.Count; ++index) {
				string humanString = ((IEnumerable<string>)list [index].languages).ToHumanString<string> ();
				dict.GetOldOrSetNew<string, List<CodeTemplate>> (humanString).Add (templates [index]);
				foreach (TemplateParameter templateParameter in templates[index])
					dictionary.GetOldOrSetNew<string, List<string>> (templateParameter.name).AddRange ((IEnumerable<string>)list [index].parameterDocs [templateParameter.name]);
			}
			foreach (KeyValuePair<string, List<CodeTemplate>> keyValuePair in dict) {
				indentedWriter.WriteLine (keyValuePair.Key + ":");
				++indentedWriter.Indent;
				foreach (CodeTemplate code in keyValuePair.Value)
					CodeTemplate.WriteDoc ((TextWriter)indentedWriter, code);
				--indentedWriter.Indent;
			}
			++indentedWriter.Indent;
			List<string> stringList = new List<string> ();
			foreach (LanguageProcessor.DocCode docCode in (IEnumerable<LanguageProcessor.DocCode>) list) {
				if (docCode.mainDoc.Count > 0) {
					stringList.AddRange ((IEnumerable<string>)docCode.mainDoc);
					break;
				}
			}
			if (stringList.Count > 0) {
				indentedWriter.WriteLineNoTabs ("");
				foreach (string str in stringList)
					indentedWriter.WriteLine (str);
			}
			if (dictionary.Count > 0)
				LanguageProcessor.WriteParameters (indentedWriter, dictionary);
			--indentedWriter.Indent;
		}

		private static void WriteParameters (IndentedTextWriter indentedWriter, Dictionary<string, List<string>> parameterDocs)
		{
			indentedWriter.WriteLineNoTabs ("");
			indentedWriter.WriteLine ("Parameters:");
			++indentedWriter.Indent;
			foreach (KeyValuePair<string, List<string>> parameterDoc in parameterDocs) {
				if (parameterDoc.Value.Count > 0) {
					indentedWriter.WriteLine ("{0} = {1}", (object)parameterDoc.Key, (object)parameterDoc.Value [0]);
					indentedWriter.Indent += parameterDoc.Key.Length + 3;
					for (int index = 1; index < parameterDoc.Value.Count; ++index)
						indentedWriter.WriteLine (parameterDoc.Value [index]);
					indentedWriter.Indent -= parameterDoc.Key.Length + 3;
				} else
					indentedWriter.WriteLine ("{0}", (object)parameterDoc.Key);
			}
			--indentedWriter.Indent;
		}

		private class LanguageElement
		{
			private List<string> docComments;
			private string mainLine;
			private LanguageProcessor.ParsedLine parsedLine;

			public bool IsParameter { get; private set; }

			public LanguageProcessor.ParsedLine ParsedLine {
				get {
					return this.parsedLine;
				}
			}

			public string MainLine {
				get {
					return this.mainLine;
				}
			}

			public LanguageElement ()
			{
				this.docComments = new List<string> ();
			}

			public void AddDoc (string line)
			{
				this.docComments.Add (line);
			}

			public void SetMainLine (string line)
			{
				this.IsParameter = char.IsWhiteSpace (line [0]);
				this.mainLine = line;
				this.parsedLine = LanguageProcessor.ParsedLine.ParseLine (line);
			}

			public string[] GetDocLines ()
			{
				return this.docComments.ToArray ();
			}

			public override string ToString ()
			{
				return this.mainLine;
			}
		}

		private struct ParsedLine
		{
			public string name;
			public int number1;
			public int number2;
			public string[] flags;

			public static TemplateParameter ParseParameter (LanguageProcessor.ParsedLine line)
			{
				int minDimensions = 1;
				int maxDimensions = 1;
				bool pointer = false;
				bool isFixed = false;
				bool signed = false;
				int valueBase = 16;
				Priority pointedPriority = Priority.none;
				foreach (string flag in line.flags) {
					if (flag.Length != 0) {
						int num1 = flag.IndexOf (':');
						if (flag.StartsWith ("pointer")) {
							pointer = true;
							if (num1 > 0) {
								string str = flag.Substring (num1 + 1);
								if (((IEnumerable<string>)Enum.GetNames (typeof(Priority))).Contains<string> (str))
									pointedPriority = (Priority)Enum.Parse (typeof(Priority), str);
							}
						} else if (flag.StartsWith ("coordinates") || flag.StartsWith ("coordinate")) {
							if (num1 < 0)
								throw new FormatException ("No : in option " + flag);
							string s = flag.Substring (num1 + 1);
							if (s.Contains ("-")) {
								string[] strArray = s.Split ('-');
								minDimensions = int.Parse (strArray [0]);
								maxDimensions = int.Parse (strArray [1]);
							} else {
								int num2 = int.Parse (s);
								minDimensions = num2;
								maxDimensions = num2;
							}
						} else if (flag.StartsWith ("preferredBase")) {
							if (num1 < 0)
								throw new FormatException ("No : in option " + flag);
							valueBase = flag.Substring (num1 + 1).GetValue ();
						} else if (flag.StartsWith ("fixed")) {
							isFixed = true;
						} else {
							if (!flag.StartsWith ("signed"))
								throw new FormatException ("Unknown option " + flag + " in parameter " + line.name);
							signed = true;
						}
					}
				}
				TemplateParameter templateParameter = new TemplateParameter (line.name, line.number1, line.number2, minDimensions, maxDimensions, pointer, pointedPriority, signed, isFixed);
				templateParameter.SetBase (valueBase);
				return templateParameter;
			}

			public static LanguageProcessor.ParsedLine ParseLine (string line)
			{
				LanguageProcessor.ParsedLine parsedLine = new LanguageProcessor.ParsedLine ();
				string[] strArray = line.Split (new char[1]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
				parsedLine.name = strArray [0].Trim ();
				parsedLine.number1 = strArray [1].Trim ().GetValue ();
				parsedLine.number2 = strArray [2].Trim ().GetValue ();
				if (strArray.Length > 3) {
					List<string> stringList = new List<string> ((IEnumerable<string>)strArray [3].Split (" -".GetArray<string> (), StringSplitOptions.RemoveEmptyEntries));
					for (int index = 0; index < stringList.Count; ++index)
						stringList [index] = stringList [index].Trim ();
					parsedLine.flags = stringList.ToArray ();
				} else
					parsedLine.flags = new string[0];
				return parsedLine;
			}
		}

		private struct DocCode
		{
			public List<string> mainDoc;
			public string[] languages;
			public ICodeTemplate code;
			public Priority priority;
			public Dictionary<string, List<string>> parameterDocs;

			public override string ToString ()
			{
				return this.code.ToString ();
			}
		}
	}
}
