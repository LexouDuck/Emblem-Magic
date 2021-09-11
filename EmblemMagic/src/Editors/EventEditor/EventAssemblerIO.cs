using System;
using System.Collections.Generic;
using System.IO;
using GBA;
using Magic;
using Magic.Editors;
using EA = Nintenlord.Event_Assembler.Core;

namespace EmblemMagic.Editors
{
    public static class EventAssemblerIO
    {
        public static String ReplaceSpacesAndSpecialChars(String EMidentifier)
        {
            Boolean capitalize = true;
            Char[] str = EMidentifier.ToCharArray();
            List<Char> result = new List<Char>();
            for (Int32 i = 0; i < str.Length; i++)
            {
                if (str[i] == '(' ||
                    str[i] == ':' ||
                    str[i] == ';' ||
                    str[i] == '+' ||
                    str[i] == '-' ||
                    str[i] == '*' ||
                    str[i] == '/')
                    result.Add('_');
                else if (str[i] == ')' && (i + 1 < str.Length) && (str[i + 1] == ' '))
                    ++i;
                else if (Char.IsLetter(str[i]) && capitalize)
                    result.Add(Char.ToUpper(str[i]));
                else if (Char.IsLetterOrDigit(str[i]) || str[i] == '_')
                    result.Add(str[i]);

                capitalize = (str[i] == ' ');
            }
            return new String(result.ToArray());
        }

        static Int32 IndexOfCharset(String str, String charset, Int32 start = 0, Int32 count = 0)
        {
            Int32 i = 0;
            while (start + i < str.Length)
            {
                if (i == count) break;
                for (Int32 c = 0; c < charset.Length; c++)
                {
                    if (str[start + i] == charset[c])
                        return (start + i);
                }
                ++i;
            }
            return (-1);
        }

        static void LoadEventCode_ArrayDefinitionsForCommand(ref String code, String command, Int32 arg_index, ArrayFile definitions, String prefix = "")
        {
            const String separators = " \t\r\n,[]";
            String define;
            Byte value;
            Int32 index = 0;
            Int32 line_length;
            Int32 occurence;
            Int32 length;
            while ((occurence = code.IndexOf(command + " ", index)) > 0)
            {
                index = occurence + command.Length + 1;
                line_length = code.IndexOf("\r\n", index) - occurence;
                if (line_length < 0) line_length = code.Length - occurence;
                for (Int32 i = 1; i < arg_index; i++)
                {   // iteratively parse through the event code string, along the command's arguments, to  reach 'arg_index'
                    index = IndexOfCharset(code, separators, index, line_length);
                    if (index >= occurence + line_length) break;
                    if (index < 0) return;
                    index++;
                    while (separators.Contains(code[index].ToString()))
                        index++;
                }
                index = code.IndexOf("0x", index) + 2;
                if (index < 0) continue;
                length = 0;
                while (index + length < code.Length && Util.IsHexDigit(code[index + length]))
                    length++;
                value = (Byte)Util.HexToInt(code.Substring(index, length));
                index -= 2;
                length += 2;
                define = ReplaceSpacesAndSpecialChars(definitions[value]);
                if (define.Length > 1)
                {
                    code = code.Remove(index, length);
                    code = code.Insert(index, prefix + define);
                }
            }
        }
        public static void LoadEventCode_ArrayDefinitions(ref String code)
        {
            ArrayFile backgrounds = new ArrayFile("Dialog Background List.txt");
            ArrayFile cutscenes = new ArrayFile("Cutscene Screen List.txt");
            ArrayFile songs = new ArrayFile("Music List.txt");
            ArrayFile chapters = new ArrayFile("Chapter List.txt");
            ArrayFile characters = new ArrayFile("Character List.txt");
            ArrayFile classes = new ArrayFile("Class List.txt");
            ArrayFile items = new ArrayFile("Item List.txt");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT",  1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT",  2, classes   ); // "Class_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT",  8, items     ); // "Item_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT",  9, items     ); // "Item_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT", 10, items     ); // "Item_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT", 11, items     ); // "Item_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "MNCH",           1, chapters); // "Chapter_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "MNC2",           1, chapters); // "Chapter_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "MNC3",           1, chapters); // "Chapter_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "NEXTCH",         1, chapters); // "Chapter_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "NEXTCH_NOSAVE",  1, chapters); // "Chapter_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "NEXTCH_DUNGEON", 1, chapters); // "Chapter_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "LOMA",    1, chapters); // "Chapter_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "LOADMAP", 1, chapters); // "Chapter_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "LOEV", 1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "LOEV", 2, classes   ); // "Class_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "LOADUNIT", 1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "LOADUNIT", 2, classes   ); // "Class_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "MOVE", 1, characters); // "Character_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "DISA",            1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT_DISSAPPEAR", 1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "REPA",            1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "UNIT_REAPPEAR",   1, characters); // "Character_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "CAM1", 1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "CAM2", 1, characters); // "Character_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "CHAR", 3, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "CHAR", 4, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "CHAR", 5, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "CHAR", 6, characters); // "Character_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "CURF", 1, characters); // "Character_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "GOTO_IFCL", 1, characters); // "Character_");
            
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "ITGC",               1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "ITGC",               2, items     ); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "ITGV",               1, items     ); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "ITGM",               1, items     ); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "GIVEITEM",           1, characters); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "GIVEITEM",           2, items     ); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "GIVEITEM_TOCURRENT", 1, items     ); // "Character_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "GIVEITEM_TOMAIN",    1, items     ); // "Character_");


            LoadEventCode_ArrayDefinitionsForCommand(ref code, "BACG",   1, backgrounds); // "BG_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "SHOWBG", 1, backgrounds); // "BG_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "SHCG",   1, cutscenes); // "Cutscene_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "SHOWCG", 1, cutscenes); // "Cutscene_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "TEXTCG", 2, cutscenes); // "Cutscene_");

            LoadEventCode_ArrayDefinitionsForCommand(ref code, "MUSC", 1, songs); // "Music_");
            LoadEventCode_ArrayDefinitionsForCommand(ref code, "PLAY", 1, songs); // "Music_");
        }

        public static void LoadEventCode_HelperMacros(ref String code)
        {

        }
    }



    public class EA_Log : EA.IO.Logs.MessageLog
    {
        public override void PrintAll()
        {
            if (ErrorCount + MessageCount + WarningCount > 0)
                Prompt.ShowResult("Message logs:", "EventAssembler", this.GetText());
        }
    }



    public class ROM_Stream : Nintenlord.IO.ChangeStream
    {
        Editor owner;
        Int32 position;
        Nintenlord.Collections.DataChange.IDataChange<Byte> changes;

        public ROM_Stream(Editor parent) : base()
        {
            owner = parent;
            changes = new Nintenlord.Collections.DataChange.DataChange<Byte>();
        }

        public String Description;

        public override Boolean CanRead { get { return false; } }
        public override Boolean CanSeek { get { return true; } }
        public override Boolean CanWrite { get { return true; } }
        public override Int64 Length { get { return Core.App.Game.FileSize; } }
        public override Int64 Position
        {
            get { return position; }
            set { position = (Int32)value; }
        }

        public override void Flush() { }
        public override void SetLength(Int64 value) { }
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            throw new Exception("EA ROM_Stream: Reading not supported.");
        }
        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = (Int32)offset;
                    break;
                case SeekOrigin.Current:
                    position += (Int32)offset;
                    break;
                case SeekOrigin.End:
                    position = (Int32)Core.App.Game.FileSize + (Int32)offset;
                    break;
            }
            return position;
        }
        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            try
            {
                changes.AddChangedData(position, buffer, offset, count);
            }
            catch { }
            position += count;
        }

        public void WriteToROM(Boolean autoFreeSpace)
        {
            Pointer address = new Pointer();
            List<Byte[]> writes = new List<Byte[]>();
            Int32 length = 0;
            foreach (var item in (IEnumerable<KeyValuePair<Int32, Byte[]>>)changes)
            {
                if (length == 0)
                {
                    address = new Pointer((UInt32)item.Key);
                    writes.Add(item.Value);
                    length = item.Value.Length;
                }
                /*
                System.Windows.Forms.MessageBox.Show(
                    "address: " + address + ", length: " + length + "\r\n" +
                    "total: " + new Pointer(address + length) +
                    ", item.Key: " + new Pointer((uint)item.Key));
                */
                if (address + length == item.Key)
                {
                    writes.Add(item.Value);
                    length += item.Value.Length;
                }
                else
                {
                    WriteToROM_Concatenate(autoFreeSpace, address, writes, length);
                    writes.Clear();
                    address = new Pointer((UInt32)item.Key);
                    writes.Add(item.Value);
                    length = item.Value.Length;
                }
            }
            WriteToROM_Concatenate(autoFreeSpace, address, writes, length);
        }
        void WriteToROM_Concatenate(Boolean autoFreeSpace, Pointer address, List<Byte[]> writes, Int32 length)
        {
            Byte[] data = new Byte[length];
            length = 0;
            for (Int32 index = 0; index < writes.Count; index++)
            {
                for (Int32 i = 0; i < writes[index].Length; i++)
                {
                    data[length + i] = writes[index][i];
                }
                length += writes[index].Length;
            }

            String write_description = "???";
            if (length == 4 && data[0] == 0x08) write_description = "Repoint";
            else if (writes.Count > 1) write_description = "Compiled Events";

            if (autoFreeSpace)
            {
                address = Core.GetFreeSpace(data.Length);
                if (address % 4 != 0) address += (4 - (address % 4));
            }
            Core.WriteData(owner,
                address,
                data,
                Description + " - " + write_description);
        }
    }
}
