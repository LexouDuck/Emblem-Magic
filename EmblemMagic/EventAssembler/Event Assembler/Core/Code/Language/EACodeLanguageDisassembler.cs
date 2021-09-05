// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.EACodeLanguageDisassembler
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Templates;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using Nintenlord.Utility;
using Nintenlord.Utility.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Language
{
	internal sealed class EACodeLanguageDisassembler
	{
		private readonly int minimumOffset = 0x100000; // huh
		private const string offsetChanger = "ORG";
		private const string currentOffset = "CURRENTOFFSET";
		private const string messagePrinter = "MESSAGE";
		private readonly ICodeTemplateStorer codeStorage;
		private readonly IPointerMaker pointerMaker;
		private readonly Tuple<string, List<Priority>>[][] pointerList;

		public EACodeLanguageDisassembler (ICodeTemplateStorer codeStorage, IPointerMaker pointerMaker, Tuple<string, List<Priority>>[][] pointerList)
		{
			this.codeStorage = codeStorage;
			this.pointerMaker = pointerMaker;
			this.pointerList = pointerList;
		}

		public IEnumerable<string[]> Disassemble (byte[] code, int offset, int length, Priority priority, ILog log, bool addEndingLines)
		{
			SortedDictionary<int, Code> lines = new SortedDictionary<int, Code> ();
			ICollection<Priority> priorities = (ICollection<Priority>)new List<Priority> ();
			priorities.Add (priority);
			priorities.Add (Priority.low);
			foreach (Code template in this.FindTemplates(code, offset, length, lines, (IEnumerable<Priority>) priorities, log))
				lines [template.Offset] = template;
			SortedDictionary<int, string> labels = new SortedDictionary<int, string> ();
			this.FindLables ((IDictionary<int, Code>)lines, (IDictionary<int, string>)labels);
			this.MergeRepeatableCodes (lines, (Predicate<int>)(x => !labels.ContainsKey (x)));
			return this.GetLines ((IEnumerable<KeyValuePair<int, Code>>)lines, (IDictionary<int, string>)labels, addEndingLines);
		}

		public IEnumerable<string[]> DisassembleChapter (byte[] rom, int offset, ILog log, bool addEndingLines)
		{
			List<int> intList = new List<int> ();

			SortedDictionary<int, Code> codeMap = new SortedDictionary<int, Code> ();
			SortedDictionary<int, string> labelMap = new SortedDictionary<int, string> ();

			labelMap [offset] = "PointerList";

			foreach (Tuple<int, Tuple<string, List<Priority>>> tuple in pointerList.SelectMany (x => x).Index ()) {
				int pointerOffset = offset + 4 * tuple.Item1;
				int pointer = BitConverter.ToInt32 (rom, pointerOffset);

				if (this.pointerMaker.IsAValidPointer (pointer)) {
					int codeOffset = pointerMaker.MakeOffset (pointer);
					intList.Add (codeOffset);

					if (codeOffset > 0 && !labelMap.ContainsKey (codeOffset)) {
						labelMap.Add (codeOffset, tuple.Item2.Item1);

						foreach (Code code in FindTemplatesUntil(rom, codeOffset, codeMap, tuple.Item2.Item2, x => x.EndingCode, log))
							codeMap [code.Offset] = code;
					}
				} else {
					log.AddError ("Invalid pointer {0} at offset {1} in pointer list.", new object[2] {
						pointer.ToHexString ("$"),
						pointerOffset
					});

					return Enumerable.Empty<string[]> ();
				}
			}

			this.FindPointedCodes (rom, (IEnumerable<Code>)codeMap.Values, codeMap, log);
			this.FindLables ((IDictionary<int, Code>)codeMap, (IDictionary<int, string>)labelMap);
			this.MergeRepeatableCodes (codeMap, (Predicate<int>)(x => !labelMap.ContainsKey (x)));
			this.AddPointerListCodes (offset, intList.ToArray (), codeMap, log);

			/*

			// Why was this a thing? To test stuff?
			// This breaks empty pointer lists (right now used for non-FE languages)

			KeyValuePair<int, Code> keyValuePair = sortedDictionary.Last ();

			int key = keyValuePair.Key;
			int length = keyValuePair.Value.Length;

			*/

			return this.GetLines (codeMap, labelMap, addEndingLines);
		}

		public IEnumerable<string[]> DisassembleToEnd (byte[] code, int offset, Priority priority, ILog log, bool addEndingLines)
		{
			SortedDictionary<int, Code> lines = new SortedDictionary<int, Code> ();
			ICollection<Priority> priorities = (ICollection<Priority>)new List<Priority> ();
			priorities.Add (priority);
			priorities.Add (Priority.low);
			foreach (Code code1 in this.FindTemplatesUntil(code, offset, lines, (IEnumerable<Priority>) priorities, (Predicate<ICodeTemplate>) (x => x.EndingCode), log))
				lines [code1.Offset] = code1;
			SortedDictionary<int, string> labels = new SortedDictionary<int, string> ();
			this.FindLables ((IDictionary<int, Code>)lines, (IDictionary<int, string>)labels);
			this.MergeRepeatableCodes (lines, (Predicate<int>)(x => !labels.ContainsKey (x)));
			return this.GetLines ((IEnumerable<KeyValuePair<int, Code>>)lines, (IDictionary<int, string>)labels, addEndingLines);
		}

		private string[][] GetEnderLines (int endingOffset)
		{
			return new string[3][] {
				"//The next line is to ensure that new events do not overwrite other data.".GetArray<string> (),
				"//Do not delete unless you are SURE you know what you are doing!".GetArray<string> (),
				new string[3] {
					"ASSERT",
					endingOffset.ToHexString ("$") + " -",
					"CURRENTOFFSET"
				}
			};
		}

		private IEnumerable<string[]> GetLines (IEnumerable<KeyValuePair<int, Code>> lines, IDictionary<int, string> lables, bool addEndingMessages)
		{
			string[] emptyLine = new string[0];
			bool addedLine = false;
			bool enderLineAdded = false;
			int latestOffset = 0;
			foreach (KeyValuePair<int, Code> line in lines) {
				int currentOffset = line.Key;
				Code code = line.Value;
				if (line.Key != latestOffset) {
					if (addEndingMessages && !enderLineAdded && latestOffset > 0) {
						if (!addedLine)
							yield return emptyLine;
						addedLine = true;
						foreach (string[] enderLine in this.GetEnderLines(latestOffset))
							yield return enderLine;
						yield return emptyLine;
						enderLineAdded = true;
					}
					if (!addedLine)
						yield return emptyLine;
					addedLine = true;
					yield return new string[2]{ "ORG", currentOffset.ToHexString ("$") };
				}
				string labelName;
				if (lables.TryGetValue (currentOffset, out labelName)) {
					if (!addedLine)
						yield return emptyLine;
					addedLine = true;
					yield return (labelName + ":").GetArray<string> ();
				}
				enderLineAdded = false;
				yield return code.ReplaceOffsetsWithLables (lables);
				if (code.Template.EndingCode) {
					yield return emptyLine;
					addedLine = true;
				} else
					addedLine = false;
				latestOffset = currentOffset + code.Length;
			}
			if (addEndingMessages && !enderLineAdded) {
				if (!addedLine)
					yield return emptyLine;
				foreach (string[] enderLine in this.GetEnderLines(latestOffset))
					yield return enderLine;
			}
		}

		private void AddPointerListCodes (int offset, int[] pointerList, SortedDictionary<int, Code> lines, ILog log)
		{
			CanCauseError<ICodeTemplate> template = this.codeStorage.FindTemplate ("POIN", Priority.pointer);
			if (template.CausedError) {
				log.AddError (template.ErrorMessage);
			} else {
				ICodeTemplate result = template.Result;
				int index1 = 0;
				for (int index2 = 0; index2 < this.pointerList.Length; ++index2) {
					List<string> stringList = new List<string> ();
					stringList.Add (result.Name);
					int offset1 = offset + 4 * index1;
					for (int index3 = 0; index3 < this.pointerList [index2].Length; ++index3) {
						stringList.Add (pointerList [index1].ToHexString ("$"));
						++index1;
					}
					lines [offset1] = new Code (stringList.ToArray (), result, this.pointerList [index2].Length * 4, offset1);
				}
			}
		}

		private void FindLables (IDictionary<int, Code> lines, IDictionary<int, string> lables)
		{
			foreach (KeyValuePair<int, Code> line in (IEnumerable<KeyValuePair<int, Code>>) lines) {
				foreach (Tuple<int, Priority> pointedOffset in line.Value.GetPointedOffsets()) {
					int key = pointedOffset.Item1;
					if (lines.ContainsKey (key) && !lables.ContainsKey (key))
						lables.Add (key, "label" + (object)(lables.Count + 1));
				}
			}
		}

		private IEnumerable<Code> FindTemplates (byte[] code, int offset, int lengthToDiss, SortedDictionary<int, Code> lines, IEnumerable<Priority> prioritiesToUse, ILog log)
		{
			int currOffset = offset;
			while (currOffset - lengthToDiss < offset) {
				Code ccode;
				if (!lines.TryGetValue (currOffset, out ccode)) {
					CanCauseError<Code> res = this.GetCode (code, currOffset, prioritiesToUse);
					if (res.CausedError)
						log.AddError (res.ErrorMessage);
					else {
						ccode = res.Result;
						yield return res.Result;
					}
				}
				currOffset += ccode.Length;
			}
		}

		private IEnumerable<Code> FindTemplatesUntil (byte[] code, int offset, SortedDictionary<int, Code> lines, IEnumerable<Priority> prioritiesToUse, Predicate<ICodeTemplate> predicate, ILog log)
		{
			while (offset < code.Length) {
				Code ccode;
				if (!lines.TryGetValue (offset, out ccode)) {
					CanCauseError<Code> res = this.GetCode (code, offset, prioritiesToUse);
					if (res.CausedError) {
						log.AddError (res.ErrorMessage);
						break;
					}
					ccode = res.Result;
					yield return res.Result;
				}
				if (predicate (ccode.Template))
					break;
				offset += ccode.Length;
			}
		}

		private CanCauseError<Code> GetCode (byte[] code, int currOffset, IEnumerable<Priority> prioritiesToUse)
		{
			CanCauseError<ICodeTemplate> template = this.codeStorage.FindTemplate (code, currOffset, prioritiesToUse);
			if (template.CausedError)
				return template.ConvertError<Code> ();
			ICodeTemplate result = template.Result;
			int lengthBytes = result.GetLengthBytes (code, currOffset);
			CanCauseError<string[]> assembly = result.GetAssembly (code, currOffset);
			if (assembly.CausedError)
				return assembly.ConvertError<Code> ();
			return (CanCauseError<Code>)new Code (assembly.Result, result, lengthBytes, currOffset);
		}

		private void FindPointedCodes (byte[] code, IEnumerable<Code> codesToSearch, SortedDictionary<int, Code> lines, ILog log)
		{
			SortedDictionary<int, Priority> source = new SortedDictionary<int, Priority> ();
			foreach (Code code1 in codesToSearch) {
				foreach (Tuple<int, Priority> pointedOffset in code1.GetPointedOffsets()) {
					if (!source.ContainsKey (pointedOffset.Item1))
						source.Add (pointedOffset.Item1, pointedOffset.Item2);
				}
			}
			IEnumerable<KeyValuePair<int, Priority>> keyValuePairs = source.Where<KeyValuePair<int, Priority>> ((Func<KeyValuePair<int, Priority>, bool>)(pointerOffset => {
				if (pointerOffset.Key >= this.minimumOffset && pointerOffset.Key < code.Length && this.HandlePriority (pointerOffset.Value))
					return !lines.ContainsKey (pointerOffset.Key);
				return false;
			}));
			SortedDictionary<int, Code> sortedDictionary = new SortedDictionary<int, Code> ();
			Priority[] priorityArray = new Priority[2]{ Priority.none, Priority.low };
			foreach (KeyValuePair<int, Priority> keyValuePair in keyValuePairs) {
				priorityArray [0] = keyValuePair.Value;
				foreach (Code code1 in this.FindTemplatesUntil(code, keyValuePair.Key, lines, (IEnumerable<Priority>) priorityArray, (Predicate<ICodeTemplate>) (x => x.EndingCode), log)) {
					sortedDictionary.Add (code1.Offset, code1);
					lines.Add (code1.Offset, code1);
				}
			}
			if (sortedDictionary.Count <= 0)
				return;
			this.FindPointedCodes (code, (IEnumerable<Code>)sortedDictionary.Values, lines, log);
		}

		private bool HandlePriority (Priority priority)
		{
			if (priority != Priority.pointer && priority != Priority.unknown && priority != Priority.ASM)
				return priority != Priority.reinforcementData;
			return false;
		}

		private void MergeRepeatableCodes (SortedDictionary<int, Code> lines, Predicate<int> isAllowed)
		{
			int[] array = lines.Keys.ToArray<int> ();
			for (int index1 = 0; index1 < array.Length; ++index1) {
				int offset = array [index1];
				Code code1 = lines [offset];
				int maxRepetition = code1.Template.MaxRepetition;
				if (maxRepetition > 1) {
					int key = offset + code1.Length;
					List<Code> codeList = new List<Code> ();
					while (lines.ContainsKey (key) && isAllowed (key) && (codeList.Count < maxRepetition - 1 && lines [key] == code1)) {
						Code code2 = lines [key];
						codeList.Add (code2);
						lines.Remove (key);
						key += code2.Length;
					}
					if (codeList.Count > 0) {
						List<string> stringList = new List<string> ((codeList.Count + 1) * (code1.Text.Length - 1) + 1);
						stringList.AddRange ((IEnumerable<string>)code1.Text);
						int length = code1.Length;
						foreach (Code code2 in codeList) {
							length += code2.Length;
							for (int index2 = 1; index2 < code2.Text.Length; ++index2)
								stringList.Add (code2.Text [index2]);
						}
						Code code3 = new Code (stringList.ToArray (), code1.Template, length, offset);
						lines [offset] = code3;
					}
					index1 += codeList.Count;
				}
			}
		}
	}
}
