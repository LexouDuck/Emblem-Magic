using System;
using System.Collections.Generic;

namespace EmblemMagic.Editors
{
    public class Event
    {
        /// <summary>
        /// The command string (usually 4 letters, all caps)
        /// </summary>
        public string Command;
        /// <summary>
        /// The label/name for this event
        /// </summary>
        public string Label;
        /// <summary>
        /// The index at which this event is located in the code string
        /// </summary>
        public int CodeLineNumber;
        /// <summary>
        /// Stores the arguments of the command, differents uses for different types of events
        /// </summary>
        public uint[] Arguments;

        public Event(string command, string label, int line, uint[] args)
        {
            Command = command;
            Label = label;
            CodeLineNumber = line;
            Arguments = args;
        }



        /// <summary>
        /// Returns the line number for the given 'index' in the line-returned string 'code'
        /// </summary>
        public static int GetLineNumber(string code, int index)
        {
            int line = 0;
            int parse = 0;
            int next;
            while (parse < code.Length && parse < index)
            {
                next = code.IndexOf("\r\n", parse) + 2;
                if (parse < next)
                    parse = next;
                else break;
                line++;
            }
            return line;
        }
        /// <summary>
        /// Returns the character index for the given 'line_number' in the line-returned string 'code'
        /// </summary>
        public static int GetCodeIndex(string code, int line_number)
        {
            int line = 0;
            int parse = 0;
            int next;
            while (parse < code.Length && line < line_number)
            {
                next = code.IndexOf('\n', parse) + 1;
                if (parse < next)
                    parse = next;
                else break;
                line++;
            }
            return parse;
        }

        /// <summary>
        /// Returns the word around the given 'index' in the 'code' string
        /// </summary>
        public static string GetWord(string code, int index)
        {
            if (code[index] == ' ' || code[index] == '\n')
                return "";
            int start = 0;
            int end = code.Length;
            for (int i = index; i >= 0; i--)
            {
                if (code[i] == ' ' || code[i] == '\n')
                {
                    start = i + 1;
                    break;
                }
            }
            for (int i = index; i < code.Length; i++)
            {
                if (code[i] == ' ' || code[i] == '\n')
                {
                    end = i;
                    break;
                }
            }
            return code.Substring(start, end - start);
        }
        /// <summary>
        /// Returns the line surrounding the given 'index' in the 'code' string
        /// The line is a string array (split without separator chars like [ and ])
        /// Element 0 is the command, the following elements are each parameter of that line
        /// </summary>
        public static string[] GetLine(string code, int index)
        {
            int start = 0;
            for (int i = index; i >= 0; i--)
            {
                if (code[i] == '\n')
                {
                    start = i + 1;
                    break;
                }
            }
            int end = 0;
            for (int i = index; i < code.Length; i++)
            {
                if (code[i] == '\n' || code[i] == '\r')
                {
                    end = i;
                    break;
                }
            }
            return code.Substring(start, end - start).Split(
                new char[] { ' ', ',', '[', ']', '\t' },
                StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Replaces every occurence of 'label' with 'newlabel' in 'code', checking for conflicting labels in 'events'
        /// </summary>
        public static void ApplyLabel(ref string code, List<Event> events, string label, ref string newlabel)
        {
            int index = Event.GetNextLabelNumber(events, newlabel);
            if (index > 0)
            {
                newlabel = newlabel + "_" + index;
            }
            else
            {
                index = Event.GetLabelIndex(events, newlabel);
                if (index >= 0)
                {
                    code = code.Replace(events[index].Label, newlabel + "_1");
                    events[index].Label = newlabel + "_1";
                    newlabel = newlabel + "_2";
                }
            }
            code = code.Replace(label, newlabel);
        }
        /// <summary>
        /// Returns the index within the given collection of the event with the given label, or -1 if no such event was found
        /// </summary>
        public static int GetLabelIndex(IEnumerable<Event> events, string label)
        {
            int result = 0;
            foreach (Event item in events)
            {
                if (item.Label != null &&
                    item.Label.Equals(label))
                    return result;
                else result++;
            }
            return -1;
        }
        /// <summary>
        /// Returns the number to give for the next event label of this type, 0 if no such label was found
        /// </summary>
        public static int GetNextLabelNumber(IEnumerable<Event> events, string label)
        {
            int result = 0;
            int number;
            foreach (Event item in events)
            {
                if (item.Label != null &&
                    item.Label.Length > label.Length &&
                    item.Label.Substring(0, label.Length).Equals(label))
                {
                    number = int.Parse(item.Label.Substring(label.Length + 1)) + 1;
                    if (result < number)
                        result = number;
                }
            }
            return result;
        }
    }
}
