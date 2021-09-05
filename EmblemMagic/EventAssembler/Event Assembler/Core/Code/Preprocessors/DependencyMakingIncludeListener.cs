using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nintenlord.Event_Assembler.Core.IO.Logs;

namespace Nintenlord.Event_Assembler.Core.Code.Preprocessors
{
	public class DependencyMakingIncludeListener : IIncludeListener
	{
		private List<string> fileList = new List<string> ();

		public void GenerateMakeDependencies(ILog log) {
			if (File.Exists (Program.RunConfig.ppDepOutput))
				File.Delete (Program.RunConfig.ppDepOutput);

			foreach (string file in fileList)
				if (file.Contains (' '))
					log.AddWarning ("[Make Dependencies] Included file path `{0}` contains spaces. Ignoring.", file);

			using (FileStream fileStream = File.OpenWrite (Program.RunConfig.ppDepOutput)) {
				using (StreamWriter output = new StreamWriter (fileStream)) {
					output.Write (Program.RunConfig.outputFile);

					foreach (string target in Program.RunConfig.ppDepTargets) {
						output.Write (' ');
						output.Write (target);
					}

					output.Write (": ");
					output.Write (IO.IOHelpers.GetRelativePath (Environment.CurrentDirectory, Path.GetFullPath(Program.RunConfig.inputFile)));

					foreach (string file in this.fileList) {
						if (file.Contains (' '))
							continue;
						
						output.Write (' ');
						output.Write (file);
					}

					output.WriteLine ();

					if (Program.RunConfig.ppDepAddEmptyTargets) {
						foreach (string file in this.fileList) {
							if (file.Contains (' '))
								continue;
							
							output.Write (file);
							output.WriteLine (':');
						}
					}
				}
			}
		}

		public void RegisterIncludeFile(string fPath) {
			if (Program.RunConfig.ppDepIgnoreSystemFiles)
				if (Path.GetFullPath (fPath).StartsWith (AppDomain.CurrentDomain.BaseDirectory))
					return; // File is part of the program distribution, so we ignore it.
			
			string path = IO.IOHelpers.GetRelativePath (Environment.CurrentDirectory, fPath);

			if (!fileList.Contains(path))
				fileList.Add (path);
		}

		public bool AllowsMissingFiles() {
			return Program.RunConfig.ppDepIgnoreMissingFiles;
		}

		public void IncludeTextFile(string fPath) {
			RegisterIncludeFile (fPath);
		}

		public void IncludeBinaryFile(string fPath) {
			RegisterIncludeFile (fPath);
		}
	}
}

