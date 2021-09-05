// -----------------------------------------------------------------------
// <copyright file="CommentRemovingTextReader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class CommentRemovingTextReader : TextReader
    {
        TextReader mainReader;
        StringBuilder currentLine = new StringBuilder(64);
        int index = 0;
        int blockCommentDepth = 0;

        public CommentRemovingTextReader(TextReader mainReader)
        {
            this.mainReader = mainReader;
        }


        public override int Read()
        {
            while (index >= currentLine.Length)
            {
                ReadUncommentedLine();
            }

            if (index == -1)
            {
                return -1;
            }
            else
            {
                return currentLine[index++];
            }
        }

        public override int Peek()
        {
            while (index >= currentLine.Length)
            {
                ReadUncommentedLine();
            }

            return index == -1 ? -1 : currentLine[index];
        }

        private void ReadUncommentedLine()
        {
            var line = mainReader.ReadLine();
            if (line == null)
            {
                index = -1;
                return;
            }
            currentLine.Clear();
            currentLine.Append(line);

            Utility.Strings.Parser.RemoveComments(currentLine, ref blockCommentDepth);
            if (blockCommentDepth == 0)
            {
                currentLine.AppendLine();
            }
            index = 0;
        }
    }
}
