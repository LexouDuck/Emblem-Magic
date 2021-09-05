using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Preprocessors;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.IO;
using Nintenlord.Collections.Lists;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code
{
	public class PreprocessingInputStream : IInputStream, IPositionableInputStream, IDisposable
	{
		private IPreprocessor preprocessor;
		private Stack<PreprocessingInputStream.PrimitiveStream> positions;
		private LinkedArrayList<string> unreadLines;

		public int LineNumber {
			get {
				return this.positions.Peek ().LineNumber;
			}
		}

		public string CurrentFile {
			get {
				return this.positions.Peek ().Name;
			}
		}

		public PreprocessingInputStream (TextReader reader, IPreprocessor preprocessor)
		{
			this.preprocessor = preprocessor;
			this.positions = new Stack<PreprocessingInputStream.PrimitiveStream> ();
			this.unreadLines = new LinkedArrayList<string> ();
			this.positions.Push (new PreprocessingInputStream.PrimitiveStream (reader));
		}

		public string ReadLine ()
		{
			if (this.positions.Count == 0)
				return (string)null;
			
			while (this.unreadLines.Count > 0) {
				string first = this.unreadLines.First;
				this.unreadLines.RemoveFirst ();

				// Skip blank lines
				if (first.Any(c => !Char.IsWhiteSpace(c)))
					return first;
			}

			PreprocessingInputStream.PrimitiveStream primitiveStream = this.positions.Peek ();

			string line;

			if (primitiveStream.ReadLine (out line)) {
				string[] strArray = this.preprocessor.Process (line, (IInputStream)this).Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);

				for (int index = 1; index < strArray.Length; ++index)
					this.unreadLines.Add (strArray [index]);

				if (strArray.Length != 0)
					return strArray [0];

				return this.ReadLine ();
			}

			this.positions.Pop ();

			if (this.positions.Count > 0)
				primitiveStream.Close ();
			
			return this.ReadLine ();
		}

		public string PeekOriginalLine ()
		{
			string originalLine = this.positions.Peek ().OriginalLine;

			if (originalLine != null)
				return originalLine;
			
			throw new InvalidOperationException ("No lines have been read or stream has passed the end.");
		}

		public void OpenSourceFile (string path)
		{
			if (this.unreadLines.Count > 0)
				throw new InvalidOperationException ();

			this.positions.Push (new PreprocessingInputStream.PrimitiveStream ((TextReader)new StreamReader (path)));
		}

		public void OpenBinaryFile (string path)
		{
			if (this.unreadLines.Count > 0)
				throw new InvalidOperationException ();
			
			this.AddBytes (File.ReadAllBytes (path));
		}

		public void AddBytes (byte[] data)
		{
			if (this.unreadLines.Count > 0)
				throw new InvalidOperationException ();

			// this.unreadLines.AddLast (((IEnumerable<string>)Array.ConvertAll<byte, string> (data, (Converter<byte, string>)(x => x.ToString ()))).ToElementWiseString<string> (" ", "BYTE ", ""));

			StringBuilder builder = new StringBuilder();

			builder.Append ("@b64");
			builder.Append (System.Convert.ToBase64String (data));
			builder.Append ("@");

			this.unreadLines.AddLast (builder.ToString ());
		}

		public void AddNewLine (string line)
		{
			this.unreadLines.AddLast (line);
		}

		public void AddNewLines (IEnumerable<string> lines)
		{
			foreach (string line in lines)
				this.unreadLines.AddLast (line);
		}

		public void Dispose ()
		{
			foreach (PreprocessingInputStream.PrimitiveStream position in this.positions)
				position.Close ();
			
			this.positions.Clear ();
		}

		private class PrimitiveStream
		{
			private IEnumerator<string> lines;

			public int LineNumber { get; private set; }

			public string OriginalLine { get; private set; }

			public string Name { get; private set; }

			private TextReader mySource;

			public PrimitiveStream (TextReader reader)
				: this (reader.LineEnumerator (), reader.GetReaderName ())
			{
				mySource = reader;
			}

			public PrimitiveStream (IEnumerable<string> reader, string name)
			{
				this.Name = name;
				this.lines = reader.GetEnumerator ();
				this.LineNumber = 0;
		
				mySource = null;
			}

			public string ReadLine ()
			{
				++this.LineNumber;

				bool flag = this.lines.MoveNext ();
				this.OriginalLine = this.lines.Current;

				if (!flag)
					return (string)null;

				return this.lines.Current;
			}

			public bool ReadLine (out string line)
			{
				++this.LineNumber;

				bool flag = this.lines.MoveNext ();
				line = this.lines.Current;
				this.OriginalLine = this.lines.Current;

				return flag;
			}

			public void Close ()
			{
				this.lines.Dispose ();
				this.OriginalLine = (string)null;
				this.LineNumber = -1;
		
				if (mySource != null)
					mySource.Close ();
			}

			public void Reset ()
			{
				this.lines.Reset ();
				this.LineNumber = 0;
				this.OriginalLine = (string)null;
			}
		}
	}
}
