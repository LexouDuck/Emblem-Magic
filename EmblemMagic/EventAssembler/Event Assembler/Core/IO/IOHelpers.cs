using Nintenlord.Event_Assembler.Core.Collections;
using System;
using System.IO;

namespace Nintenlord.Event_Assembler.Core.IO
{
	internal static class IOHelpers
	{
		public static string FindFile (string currentFile, string newFile)
		{
			newFile = newFile.Trim ('"');

			if (File.Exists (newFile))
				return Path.GetFullPath (newFile);

			string path = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, newFile);

			if (File.Exists (path))
				return path;
			
			if (!string.IsNullOrEmpty (currentFile)) {
				path = Path.Combine (Path.GetDirectoryName (currentFile), newFile);

				if (File.Exists (path))
					return path;
			}

			return string.Empty;
		}

		public static string GetPrefferedFileName(string currentFile, string newFile) {
			if (!string.IsNullOrEmpty (currentFile))
				return Path.Combine (Path.GetDirectoryName (currentFile), newFile);

			return Path.GetFullPath (newFile);
		}

		public static string GetRelativePath(string basePath, string path) {
			// from https://stackoverflow.com/questions/703281/getting-path-relative-to-the-current-working-directory
			// because I'm scrub

			// Folders must end in a slash
			if (!basePath.EndsWith (Path.DirectorySeparatorChar.ToString ()))
				basePath += Path.DirectorySeparatorChar;
			
			Uri pathUri = new Uri(Path.GetFullPath (path));
			Uri baseUri = new Uri(Path.GetFullPath (basePath));

			return Uri.UnescapeDataString(baseUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
		}

		public static void DefineFile (string path, IDefineCollection defCol)
		{
			StreamReader streamReader = new StreamReader (path);

			while (!streamReader.EndOfStream) {
				if (streamReader.ReadLine ().Length > 0) {
					string[] strArray = (string[])null;

					for (int index = 1; index < strArray.Length; ++index)
						defCol.Add (strArray [index], strArray [0]);
				}
			}

			streamReader.Close ();
		}

		public static char? ReadCharacter (this TextReader reader)
		{
			int num = reader.Read ();

			if (num == -1)
				return new char? ();

			return new char? (Convert.ToChar (num));
		}

		public static char? PeekCharacter (this TextReader reader)
		{
			int num = reader.Peek ();

			if (num == -1)
				return new char? ();

			return new char? (Convert.ToChar (num));
		}
	}
}
