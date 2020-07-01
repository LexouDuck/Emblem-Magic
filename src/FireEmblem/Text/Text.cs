using System;
using System.Collections.Generic;

namespace EmblemMagic.FireEmblem
{
    public static class Text
    {
        public static String[] RemoveBracketCodes(string[] text)
        {
            List<string> result = new List<string>();
            string line;
            for (int i = 0; i < text.Length; i++)
            {
                line = "";
                for (int j = 0; j < text[i].Length; j++)
                {
                    if (text[i][j] == '[')
                    {
                        while (text[i][j] != ']') j++;
                    }
                    else line += text[i][j];
                }
                if (line.Length != 0) result.Add(line);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Loads command codes on ASCII-encoded FE game text, and returns the final string
        /// </summary>
        public static String BytesToText(
            byte[] data, bool bytecodes,
            ArrayFile commands_list,
            ArrayFile commands_0x80,
            ArrayFile portrait_list)
        {
            string result = "";
            string command;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < 0x20)
                {
                    command = commands_list[data[i]].Trim(' ');
                    
                    if (i != 0 && data[i] >= 0x08 && data[i] < 0x10)
                        result += "\r\n";
                    if (data[i] == 0x10)
                    {
                        UInt16 index = Util.BytesToUInt16(new byte[2] { data[i + 1], data[i + 2] }, true);

                        if (index < 0x100) throw new Exception(
                            "Invalid portrait index read in the text data" +
                            " at byte " + i + ": " + Util.UInt32ToHex(index));

                        if (bytecodes || command.Length == 0)
                            result += "[0x10:0x" + Util.UInt16ToHex(index) + ']';
                        else
                        {
                            result += '[' + command + ':';
                            result += (index == 0xFFFF ? "Current" : portrait_list[(UInt16)(index - 0x100)]) + ']';
                        }
                        i += 2;
                    }
                    else if (command == "\\n")
                        result += "\r\n";
                    else if (bytecodes || command.Length == 0)
                        result += "[0x" + Util.ByteToHex(data[i]) + ']';
                    else
                        result += '[' + command + ']';
                }
                else if (data[i] == 0x80)
                {
                    i++;
                    command = commands_0x80[data[i]].Trim(' ');

                    if (bytecodes || command.Length == 0)
                         result += "[0x" + Util.ByteToHex(data[i]) + ']';
                    else result += '[' + command + ']';
                }
                else
                {
                    if (Core.CurrentROM.Version == GameVersion.JAP)
                    {
                        byte[] buffer = System.Text.Encoding.Convert(
                            System.Text.Encoding.GetEncoding(932), // Shift-JIS
                            System.Text.Encoding.Unicode,
                            data, i++, 2);
                        //UInt16 symbol = data.GetUInt16((uint)i++, false);
                        //Program.ShowMessage(Util.UInt16ToHex(symbol));
                        result += (char)(Util.BytesToNumber(buffer, true));
                    }
                    else result += (char)data[i];
                }
            }
            return result;
        }
        /// <summary>
        /// Takes text with command codes and returns the corresponding ASCII-encoded byte array
        /// </summary>
        public static byte[] TextToBytes(string text,
            ArrayFile commands_list,
            ArrayFile commands_0x80,
            ArrayFile portrait_list)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '[')
                {
                    i++;
                    int length = 0;
                    while (text[i + length] != ']') length++;
                    string command = text.Substring(i, length);
                    i += length;
                    
                    if (command.StartsWith(commands_list[0x10]))
                    {   // Load portrait command is pretty special
                        string portrait = command.Substring(commands_list[0x10].Length + 1);
                        result.Add(0x10);
                        if (portrait.Equals("Current"))
                        {
                            result.AddRange(new byte[2] { 0xFF, 0xFF });
                        }
                        else
                        {
                            uint index = portrait_list.FindEntry(portrait);
                            if (index == 0xFFFFFFFF) throw new Exception("Invalid portrait name: " + portrait);
                            result.AddRange(Util.UInt16ToBytes((UInt16)(index + 0x100), true));
                        }
                    }
                    else
                    {
                        uint index = commands_list.FindEntry(command);
                        if (index == uint.MaxValue)
                        {   // so it's not a regular byte command

                            index = commands_0x80.FindEntry(command);
                            if (index == uint.MaxValue)
                            {   // it wasn't found in the command lists

                                if (command.StartsWith("0x"))
                                {
                                    result.Add(Util.HexToByte(command.Substring(2)));
                                }
                                else throw new Exception("Invalid text command: [" + command + ']');
                            }
                            else
                            {
                                result.Add(0x80);
                                result.Add((byte)index);
                            }
                        }
                        else result.Add((byte)index);
                    }
                }
                else if (text[i] == '\n' || (text[i] == '\r' && text[++i] == '\n'))
                {
                    result.Add(0x01);
                }
                else result.Add((byte)text[i]);
            }
            return result.ToArray();
        }
    }
}