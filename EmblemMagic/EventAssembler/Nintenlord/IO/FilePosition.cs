using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.IO
{
    public struct FilePosition
    {
        readonly string file;
        readonly int line;
        readonly int column;

        public int Line
        {
            get { return line; }
        }
        public int Column
        {
            get { return column; }
        }
        public string File
        {
            get { return file; }
        }

        public FilePosition(string file, int line, int column)
        {
            this.file = file;
            this.line = line;
            this.column = column;
        }
        
        public static FilePosition BeginningPosition(string file)
        {
            return new FilePosition(file, 1, 1);
        }

        public override string ToString()
        {
            return string.Format("File {0}, Line {1}, Column {2}", System.IO.Path.GetFileName(file), line, column);
        }
    }
}
