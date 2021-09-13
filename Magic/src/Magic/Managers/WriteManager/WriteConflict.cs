using Magic.Editors;
using GBA;
using System;

namespace Magic
{
    public class WriteConflict
    {
        /// <summary>
        /// The conflicting write.
        /// </summary>
        public Write Write { get; }
        /// <summary>
        /// The Range within this write that is to be cut
        /// </summary>
        public Range Portion { get; }

        public WriteConflict(Write conflict, Range cutPortion)
        {
            if (conflict == null)
                throw new Exception("cannot instantiate a null write conflict.");
            if (cutPortion.Start < 0 || cutPortion.End > conflict.Data.Length)
                throw new Exception("conflict portion is outside its write.");

            this.Write = conflict;
            this.Portion = cutPortion;
        }
        /// <summary>
        /// Returns the new write(s) with its 'Portion' cut off
        /// </summary>
        /// <returns></returns>
        public Tuple<Write, Write> GetNewWrite()
        {
            String editor = this.Write.Author;
            String phrase = this.Write.Phrase;
            Pointer address = this.Write.Address;
            if (this.Portion.Start == 0)
            {
                if (this.Portion.End == this.Write.Data.Length)
                {
                    return Tuple.Create(
                        (Write)null,
                        (Write)null);
                }
                else
                {
                    Byte[] data = new Byte[this.Write.Data.Length - this.Portion.End];
                    Array.Copy(this.Write.Data, this.Portion.End, data, 0, data.Length);
                    return Tuple.Create(
                        (Write)null,
                        new Write(editor, address + new Pointer(this.Portion.End), data, phrase));
                }
            }
            else
            {
                if (this.Portion.End == this.Write.Data.Length)
                {
                    Byte[] data = new Byte[this.Portion.Start];
                    Array.Copy(this.Write.Data, 0, data, 0, data.Length);
                    return Tuple.Create(
                        new Write(editor, address, data, phrase),
                        (Write)null);
                }
                else
                {
                    Byte[] data1 = new Byte[this.Portion.Start];
                    Array.Copy(this.Write.Data, 0, data1, 0, data1.Length);
                    Byte[] data2 = new Byte[this.Write.Data.Length - this.Portion.End];
                    Array.Copy(this.Write.Data, this.Portion.End, data2, 0, data2.Length);
                    return Tuple.Create(
                        new Write(editor, address, data1, phrase),
                        new Write(editor, address + new Pointer(this.Portion.End), data2, phrase));
                }
            }
        }
    }
}
