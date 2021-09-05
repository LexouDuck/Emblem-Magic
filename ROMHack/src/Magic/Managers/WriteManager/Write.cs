using Magic.Editors;
using GBA;
using System;

namespace Magic
{
    /// <summary>
    /// Records all relevant information regarding a write of data to the ROM
    /// </summary>
    public class Write
    {
        /// <summary>
        /// Is true if the data of this Write is applied onto the last saved version of the ROM.
        /// </summary>
        public Boolean Patched { get; set; }
        /// <summary>
        /// Is true if this Write is in the last saved version of the FEH hack file.
        /// </summary>
        public Boolean IsSaved { get; set; }

        /// <summary>
        /// A string of the name of the Editor that made this Write.
        /// </summary>
        public String Author { get; private set; }
        /// <summary>
        /// A string description of this write (32 chars long) - auto-generated in some cases.
        /// </summary>
        public String Phrase { get; private set; }
        /// <summary>
        /// The time at which this write was done/created.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// The address in the ROM where data is (to be) written.
        /// </summary>
        public GBA.Pointer Address { get; set; }
        /// <summary>
        /// The data of this Write.
        /// </summary>
        public Byte[] Data { get; set; }

        

        public Write(Editor author, Pointer address, byte[] data, string description = "")
        {
            Author = (author == null) ? "Unknown Editor" : author.Text ?? "Unknown Editor";
            Load(DateTime.Now, address, data, description ?? "");
        }
        public Write(string author, Pointer address, byte[] data, string description = "")
        {
            Author = (author == null) ? "Unknown Editor" : author ?? "Unknown Editor";
            Load(DateTime.Now, address, data, description ?? "");
        }
        /// <summary>
        /// Constructor used for loading from file.
        /// </summary>
        public Write(string author, ulong time, string phrase, uint address, byte[] data)
        {
            Author = author ?? "Unknown Editor";
            string description = phrase ?? "";
            int length;
            for (length = description.Length; length > 0; length--)
            {
                if (description[length - 1] != ' ') break;
            }
            description = description.Substring(0, length);
            Load(DateTime.FromBinary((long)time), new Pointer(address), data, description);
        }

        /// <summary>
        /// private, sets some basic fields. 
        /// </summary>
        void Load(DateTime time, Pointer address, byte[] data, string phrase)
        {
            /*
            if (address < 0 || address + data.Length > App.ROM.FileSize)
                throw new Exception("Write goes outside the bounds of the current ROM file");
            */
            Time = time;
            Address = address;
            Data = data;
            Phrase = phrase;
        }



        /// <summary>
        /// Returns a fixed-length string of the Editor of this write
        /// </summary>
        /// <returns></returns>
        public string GetEditorString()
        {
            string result = Author;
            for (int i = 0; i < HackManager.LENGTH_Write_Author - Author.Length; i++)
            {
                result += " ";
            }
            return result;
        }
        /// <summary>
        /// Returns a fixed-length string of this write's description
        /// </summary>
        /// <returns></returns>
        public string GetPhraseString()
        {
            string result = Phrase;
            for (int i = 0; i < HackManager.LENGTH_Write_Phrase - Phrase.Length; i++)
            {
                result += " ";
            }
            return result;
        }
    }
}
