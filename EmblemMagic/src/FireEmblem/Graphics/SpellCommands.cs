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
        public String Name { get; }
        /// <summary>
        /// The ASM code this command is a macro for
        /// </summary>
        public String[] ASM_Code { get; }
        /// <summary>
        /// Whether or not to apply syntax coloring onto this command
        /// </summary>
        public Boolean Colored { get; }

        public SpellCommand(String command, String[] asm_code, Boolean color)
        {
            Name = command;
            ASM_Code = asm_code;
            Colored = color;
        }
    }



    public class SpellCommands
    {
        List<SpellCommand> Commands;

        public SpellCommands(String filepath)
        {
            String[] file = File.ReadAllLines(filepath);

            Commands = new List<SpellCommand>();
            for (Int32 i = 0; i < file.Length; i++)
            {
                if (file[i].Length == 0 || file[i][0] == '@')
                {
                    continue;
                }
                else
                {
                    String command;
                    Int32 length = file[i].IndexOf('@');
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
                    List<String> asmcode = new List<String>();
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

        public String Get(ASM.Instruction[] asm, ref Int32 index)
        {
            String result = "";
            List<String> arguments;
            Boolean match;
            foreach (var command in Commands)
            {
                match = false;
                arguments = new List<String>();
                for (Int32 i = 0; i < command.ASM_Code.Length; i++)
                {
                    for (Int32 j = 0; j < asm[index + i].Code.Length; j++)
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
                            String argument = "";
                            Int32 c = 0;
                            while (j + c < command.ASM_Code[i].Length && command.ASM_Code[i][j + c] == '_')
                            {
                                argument += asm[index + i].Code[j + c];
                                c += 1;
                            }
                            if (argument.Length <= 2)
                                arguments.Insert(0, argument);
                            else arguments.Add(argument);
                            j += c;
                        }
                        else goto Continue;
                    }
                }
                if (match)
                {
                    index += command.ASM_Code.Length - 1; // skip the correct amount of lines
                    result = command.Name;
                    if (arguments.Count == 0)
                        return result;
                    result += "(";
                    for (Int32 i = 0; i < arguments.Count; i++)
                    {
                        result += arguments[i];
                        if (i < arguments.Count - 1)
                            result += ", ";
                    }
                    result += ")";
                    return result;
                }
                Continue: continue;
            }
            return null;
        }



        public String GetRegex()
        {
            String result = "";

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
