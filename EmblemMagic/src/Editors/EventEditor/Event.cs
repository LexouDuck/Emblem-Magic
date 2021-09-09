using System;
using System.Collections.Generic;

namespace EmblemMagic.Editors
{
    public class Event
    {
        /// <summary>
        /// The command string (usually 4 letters, all caps)
        /// </summary>
        public String Command;
        /// <summary>
        /// The label/name for this event
        /// </summary>
        public String Label;
        /// <summary>
        /// The index at which this event is located in the code string
        /// </summary>
        public Int32 CodeLineNumber;
        /// <summary>
        /// Stores the arguments of the command, differents uses for different types of events
        /// </summary>
        public UInt32[] Arguments;

        public Event(String command, String label, Int32 line, UInt32[] args)
        {
            Command = command;
            Label = label;
            CodeLineNumber = line;
            Arguments = args;
        }



        /// <summary>
        /// Returns the line number for the given 'index' in the line-returned string 'code'
        /// </summary>
        public static Int32 GetLineNumber(String code, Int32 index)
        {
            Int32 line = 0;
            Int32 parse = 0;
            Int32 next;
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
        public static Int32 GetCodeIndex(String code, Int32 line_number)
        {
            Int32 line = 0;
            Int32 parse = 0;
            Int32 next;
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
        public static String GetWord(String code, Int32 index)
        {
            if (code[index] == ' ' || code[index] == '\n')
                return "";
            Int32 start = 0;
            Int32 end = code.Length;
            for (Int32 i = index; i >= 0; i--)
            {
                if (code[i] == ' ' || code[i] == '\n')
                {
                    start = i + 1;
                    break;
                }
            }
            for (Int32 i = index; i < code.Length; i++)
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
        public static String[] GetLine(String code, Int32 index)
        {
            Int32 start = 0;
            for (Int32 i = index; i >= 0; i--)
            {
                if (code[i] == '\n')
                {
                    start = i + 1;
                    break;
                }
            }
            Int32 end = 0;
            for (Int32 i = index; i < code.Length; i++)
            {
                if (code[i] == '\n' || code[i] == '\r')
                {
                    end = i;
                    break;
                }
            }
            return code.Substring(start, end - start).Split(
                new Char[] { ' ', ',', '[', ']', '\t' },
                StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Replaces every occurence of 'label' with 'newlabel' in 'code', checking for conflicting labels in 'events'
        /// </summary>
        public static void ApplyLabel(ref String code, List<Event> events, String label, ref String newlabel)
        {
            Int32 index = Event.GetNextLabelNumber(events, newlabel);
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
        public static Int32 GetLabelIndex(IEnumerable<Event> events, String label)
        {
            Int32 result = 0;
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
        public static Int32 GetNextLabelNumber(IEnumerable<Event> events, String label)
        {
            Int32 result = 0;
            Int32 number;
            foreach (Event item in events)
            {
                if (item.Label != null &&
                    item.Label.Length > label.Length &&
                    item.Label.Substring(0, label.Length).Equals(label))
                {
                    number = Int32.Parse(item.Label.Substring(label.Length + 1)) + 1;
                    if (result < number)
                        result = number;
                }
            }
            return result;
        }
    }
}
