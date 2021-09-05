//Added by Crazycolorz5
using System;
using System.Collections.Generic;
using System.Text;

namespace Crazycolorz5
{
    public static class Parser
    {
        public static string[] SplitToParameters(string code)
        {
            //Edited, revamped version of nintenlord's to allow for escaping.
            if (code.Length == 0)
            {
                return new string[0] { };
            }
            List<string> elements = new List<string>();
            StringBuilder newElement = new StringBuilder(code.Length, code.Length);

            int parenthDepth = 0;
            bool quote = false, escape = false;
            int vectorDepth = 0;

            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                bool split = false;
                if(!escape)
                {
                    if (c == '[') vectorDepth++;
                    else if (c == ']') vectorDepth--;
                    else if (c == '(') parenthDepth++;
                    else if (c == ')') parenthDepth--;
                    else if (c == '"') quote = !quote;
                    else if (c == '\\') escape = true;
                    else if (Char.IsWhiteSpace(c) &&
                        parenthDepth <= 0 &&
                        !quote &&
                        vectorDepth <= 0)
                    {
                        split = true;
                    }

                    if (split)
                    {
                        if (newElement.Length > 0)
                        {
                            elements.Add(newElement.ToString());
                            newElement.Remove(0, newElement.Length);
                        }
                    }
                    else if (c != '"')
                    {
                        newElement.Append(c);
                    }
                }
                else
                {
                    newElement.Append(c);
                    escape = false;
                }
            }

            if (newElement.Length > 0)
            {
                elements.Add(newElement.ToString());
            }

            return elements.ToArray();
        }
    }
}
