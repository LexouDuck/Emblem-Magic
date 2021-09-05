using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nintenlord.IO.Scanners
{
    public sealed class CharacterScanner : IScanner<char>
    {
        Stream stream;
        char current;

        public CharacterScanner(Stream stream)
        {
            this.stream = stream;
            int val = stream.ReadByte();
            if (val != -1)
            {
                current = Convert.ToChar(val);
            }
        }

        #region IScanner<char> Members

        public bool IsAtEnd
        {
            get { return stream.IsAtEnd(); }
        }

        public long Offset
        {
            get
            {
                return stream.Position;
            }
            set
            {
                stream.Position = value;
            }
        }

        public bool MoveNext()
        {
            int val = stream.ReadByte();
            if (val != -1)
            {
                current = Convert.ToChar(val);
            }
            return val == -1;
        }

        public char Current
        {
            get
            {
                return current;
            }
        }

        public bool CanSeek
        {
            get { return true; }
        }
        
        public IEnumerable<char> Substring(long Offset, int Length)
        {
            throw new NotSupportedException();
        }
        
        public bool CanTakeSubstring
        {
            get { return false; }
        }

        #endregion
    }
}
