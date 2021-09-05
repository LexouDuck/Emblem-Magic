using System;
using System.Collections.Generic;
using System.Text;
using Nintenlord.Collections;
using Nintenlord.Utility.Primitives;

namespace Nintenlord.Utility.Strings
{
    public class Parser
    {
        static Dictionary<string, Func<int, int, int>> binaryOperators;
        static Dictionary<string, Func<int, int>> unaryOperators;

        static Parser()
        {
            binaryOperators = new Dictionary<string, Func<int, int, int>>();
            binaryOperators["+"] = (x, y) => x + y;
            binaryOperators["-"] = (x, y) => x - y;

            binaryOperators["*"] = (x, y) => x * y;
            binaryOperators["/"] = (x, y) => x / y;
            binaryOperators["%"] = (x, y) => x % y;

            binaryOperators["&"] = (x, y) => x & y;
            binaryOperators["|"] = (x, y) => x | y;
            binaryOperators["^"] = (x, y) => x ^ y;

            unaryOperators = new Dictionary<string, Func<int, int>>();
            unaryOperators["-"] = x => -x;
        }

        public static string[] ShuntingYardAlgorithm(string s)
        {
            return ShuntingYardAlgorithm(s, new OperatorComparer(), 
                x => binaryOperators.ContainsKey(x), x => x.IsHexDigit() || x == 'x');
        }

        public static string[] ShuntingYardAlgorithm(string s, IComparer<string> operatorComparer,
            Predicate<string> isOperator, Predicate<char> isValue)
        {
            Stack<string> operators = new Stack<string>();
            List<string> output = new List<string>();
            bool wasValue = false;
            bool isNegative = false;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (isValue(c))
                {
                    int length = 1;
                    while (i + length < s.Length && isValue(s[i + length]))
                    {
                        length++;
                    }
                    if (isNegative)
                    {
                        output.Add("-" + s.Substring(i, length));
                        isNegative = false;
                    }
                    else
                    {
                        output.Add(s.Substring(i, length));
                    }
                    i += length - 1;
                    wasValue = true;
                }
                else if (c == '(')
                {
                    operators.Push(s.Substring(i, 1));
                    wasValue = false;
                }
                else if (c == ')')
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    if (operators.Count > 0)
                    {
                        operators.Pop();
                    }
                    wasValue = true;
                }
                else if (isOperator(c.ToString()))
                {
                    if (!wasValue && c == '-')
                    {
                        isNegative = true;
                    }
                    else
                    {
                        while (operators.Count > 0 && operators.Peek() != "(" &&
                            operatorComparer.Compare(c.ToString(), operators.Peek()) <= 0)
                        {
                            output.Add(operators.Pop());
                        }
                        operators.Push(c.ToString());
                    }
                    wasValue = false;
                }
            }

            while (operators.Count > 0)
            {
                string c = operators.Pop();
                if (c != "(")
                {
                    output.Add(c);
                }
            }

            return output.ToArray();
        }
        
        private class OperatorComparer : IComparer<string>
        {
            #region IComparer<char> Members

            public int Compare(string x, string y)
            {
                return OperatorComparer.SCompare(x, y);
            }

            #endregion

            public static int SCompare(string x, string y)
            {
                if (IsLog(x))
                {
                    if (IsLog(y))
                        return 0;
                    else return -1;
                }
                else if (IsAdd(x))
                {
                    if (IsLog(y))
                        return 1;
                    if (IsAdd(y))
                        return 0;
                    else return -1;
                }
                else if (IsMul(x))
                {
                    if (IsMul(y))
                        return 0;
                    if (IsParent(y))
                        return -1;
                    else return 1;
                }
                else//x is ( or )
                {
                    if (IsParent(y))
                        return 0;
                    else return 1;
                }
            }
            private static bool IsParent(string c)
            {
                return c == "(" || c == ")";
            }

            private static bool IsAdd(string c)
            {
                return c == "+" || c == "-";
            }

            private static bool IsMul(string c)
            {
                return c == "*" || c == "/" || c == "%";
            }

            private static bool IsLog(string c)
            {
                return c == "&" || c == "|" || c == "^";
            }
        }
        

        public static int EvaluateReversePolishNotation(string[] s)
        {
            return EvaluateReversePolishNotation(s, 0, s.Length);
        }

        public static int EvaluateReversePolishNotation(string[] s, int index, int length)
        {
            return EvaluateReversePolishNotation(
                s, index, length, binaryOperators, unaryOperators, x => x.GetValue()
                );
        }

        private static TOutput EvaluateReversePolishNotation<TOutput, TInput>(
            TInput[] s, int index, int length,
            IDictionary<TInput, Func<TOutput, TOutput, TOutput>> binaryOperators,
            IDictionary<TInput, Func<TOutput, TOutput>> unaryOperators,
            Func<TInput, TOutput> conversion)
        {
            Stack<TOutput> values = new Stack<TOutput>();
            for (int i = index; i < index + length; i++)
            {
                if (binaryOperators.ContainsKey(s[i]))
                {
                    if (values.Count < 2)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        TOutput arg2 = values.Pop();
                        TOutput arg1 = values.Pop();
                        values.Push(binaryOperators[s[i]](arg1, arg2));
                    }
                }
                else if (unaryOperators.ContainsKey(s[i]))
                {
                    if (values.Count < 1)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        TOutput arg1 = values.Pop();
                        values.Push(unaryOperators[s[i]](arg1));
                    }
                }
                else
                {
                    values.Push(conversion(s[i]));
                }
            }


            if (values.Count != 1)
            {
                throw new ArgumentException();
            }
            return values.Pop();
        }



        public static string[] SplitToParameters(string code)
        {
            if (code.Length == 0)
            {
                return new string[] { };
            }
            List<string> elements = new List<string>();
            StringBuilder newElement = new StringBuilder(code.Length, code.Length);

            int parenthDepth = 0;
            bool quote = false;
            int vectorDepth = 0;

            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                bool split = false;
                if (c == '[') vectorDepth++;
                else if (c == ']') vectorDepth--;
                else if (c == '(') parenthDepth++;
                else if (c == ')') parenthDepth--;
                else if (c == '"') quote = !quote;
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

            if (newElement.Length > 0)
            {
                elements.Add(newElement.ToString());
            }

            return elements.ToArray();
        }

        private static bool RemoveComments(ref string line, ref int blockCommentDepth)
        {
            if (line.Length == 0)
                return true;

            bool cool = true;
            int[] commentDepths = new int[line.Length];
            int endOfLine = line.Length;

            {
                int i;
                for (i = 0; i < line.Length - 1; i++)
                {
                    if (line[i] == '/' && line[i + 1] == '*')
                        blockCommentDepth++;
                    else if (line[i] == '/' && line[i + 1] == '/')
                    {
                        endOfLine = i;
                        break;
                    }
                    commentDepths[i] = blockCommentDepth;
                    if (line[i] == '*' && line[i + 1] == '/')
                    {
                        i++;
                        commentDepths[i] = blockCommentDepth;
                        if (blockCommentDepth == 0)
                            cool = false;
                        else
                            blockCommentDepth--;
                    }
                }
                if (i < line.Length)
                {
                    commentDepths[i] = blockCommentDepth;
                }
            }

            if (Array.ConvertAll(commentDepths, x => x == 0).And())
            {
                //No block comments
                line = line.Substring(0, endOfLine);
            }
            else
            {
                StringBuilder newLine = new StringBuilder(endOfLine, line.Length);

                bool firstRemoved = true;
                for (int i = 0; i < endOfLine; i++)
                {
                    if (commentDepths[i] == 0) //Not in comment
                    {
                        newLine.Append(line[i]);
                        firstRemoved = true;
                    }
                    else if (firstRemoved) //First character to remove, insert whitespace
                    {
                        newLine.Append(' ');
                        firstRemoved = false;
                    }
                }
                line = newLine.ToString();
            }
            return cool;
        }
        
        public static bool ReplaceCommentsWith(StringBuilder line, char c, ref int blockCommentDepth)
        {
            if (line.Length == 0)
                return true;

            int[] commentDepths;
            int endOfLine;

            bool cool = FindComments(line, ref blockCommentDepth, out commentDepths, out endOfLine);

            int i = 0;
            for (; i < endOfLine; i++)
            {
                if (commentDepths[i] > 0)
                {
                    line[i] = c;
                }
            }
            for (; i < line.Length; i++)
            {
                line[i] = c;
            }

            return cool;
        }

        public static bool RemoveComments(StringBuilder line, ref int blockCommentDepth)
        {
            if (line.Length == 0)
                return true;

            int[] commentDepths;
            int endOfLine;

            bool cool = FindComments(line, ref blockCommentDepth, out commentDepths, out endOfLine);

            if (endOfLine < line.Length)
            {
                line.Remove(endOfLine,  line.Length - endOfLine);
            }
            for (int i = endOfLine - 1; i >= 0; i--)
            {
                if (commentDepths[i] > 0)
                {
                    line.Remove(i, 1);
                }
            }

            return cool;
        }

        private static bool FindComments(StringBuilder line, ref int blockCommentDepth, out int[] commentDepths, out int endOfLine)
        {
            bool cool = true;
            commentDepths = new int[line.Length];
            endOfLine = line.Length;

            int i;
            for (i = 0; i < line.Length - 1; i++)
            {
                if (line[i] == '/')
                {
                    if (line[i + 1] == '*')
                    {
                        blockCommentDepth++;
                    }
                    else if (line[i + 1] == '/' && blockCommentDepth == 0)
                    {
                        endOfLine = i;
                        break;
                    }
                }
                commentDepths[i] = blockCommentDepth;
                if (line[i] == '*' && line[i + 1] == '/')
                {
                    i++;
                    commentDepths[i] = blockCommentDepth;
                    if (blockCommentDepth == 0)
                        cool = false;
                    else
                        blockCommentDepth--;
                }
            }
            if (i < line.Length)
            {
                commentDepths[i] = blockCommentDepth;
            }
            return cool;
        }
    }
}