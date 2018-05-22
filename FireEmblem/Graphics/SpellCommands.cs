using GBA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace EmblemMagic.FireEmblem
{
    class SpellCommand
    {
        /// <summary>
        /// The name of this command
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The ASM code this command is a macro for
        /// </summary>
        public string[] ASM_Code { get; }
        /// <summary>
        /// Whether or not to apply syntax coloring onto this command
        /// </summary>
        public bool Colored { get; }

        public SpellCommand(string command, string[] asm_code, bool color)
        {
            Name = command;
            ASM_Code = asm_code;
            Colored = color;
        }
    }
    public class SpellCommands
    {
        List<SpellCommand> Commands;

        public SpellCommands(string filepath)
        {
            string[] file = File.ReadAllLines(Core.Path_Structs + filepath);

            Commands = new List<SpellCommand>();
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i].Length == 0 || file[i][0] == '@')
                {
                    continue;
                }
                else
                {
                    string command;
                    int length = file[i].IndexOf('@');
                    if (length == -1)
                    {
                        command = file[i].TrimEnd(' ');
                    }
                    else
                    {
                        while (file[i][length - 1] == ' ' ||
                               file[i][length - 1] == '\t') length--;
                        command = file[i].Substring(0, length);
                    }
                    List<string> asmcode = new List<string>();
                    i++;
                    while (i < file.Length)
                    {
                        if (file[i].Length == 0)
                            break;
                        else
                        {
                            length = file[i].IndexOf('@');
                            if (length == -1)
                            {
                                asmcode.Add(file[i].TrimEnd(' '));
                            }
                            else
                            {
                                while (file[i][length - 1] == ' ' ||
                                       file[i][length - 1] == '\t') length--;
                                asmcode.Add(file[i].Substring(0, length));
                            }
                            i++;
                        }
                    }
                    Commands.Add(new SpellCommand(command, asmcode.ToArray(), (command.Length > 1)));
                }
            }
        }

        public string Get(ASM.Instruction[] asm, ref int index)
        {
            string result = "";
            string argument = "";
            bool match;
            foreach (var command in Commands)
            {
                match = false;
                argument = "";
                for (int i = 0; i < command.ASM_Code.Length; i++)
                {
                    for (int j = 0; j < asm[index + i].Code.Length; j++)
                    {
                        if (asm[index + i].Code[j] == command.ASM_Code[i][j])
                        {
                            match = true;
                        }
                        else if (match && command.ASM_Code[i][j] == '*')
                        {
                            match = true;
                        }
                        else if (match && command.ASM_Code[i][j] == '_')
                        {
                            argument += asm[index + i].Code[j];
                        }
                        else goto Continue;
                    }
                }
                if (match)
                {
                    index += command.ASM_Code.Length - 1; // skip the correct amount of lines
                    result = command.Name;
                    if (argument.Length != 0)
                        result += "(0x" + argument + ")";
                    return result;
                }
                Continue: continue;
            }
            return null;
        }



        public string GetRegex()
        {
            string result = "";

            foreach (var command in Commands)
            {
                if (command.Colored)
                {
                    result += command.Name;
                    result += '|';
                }
            }

            return result.Substring(0, result.Length - 1);
        }
    }
}
