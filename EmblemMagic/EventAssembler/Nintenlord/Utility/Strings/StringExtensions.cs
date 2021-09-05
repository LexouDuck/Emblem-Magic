using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Nintenlord.Utility.Primitives;

namespace Nintenlord.Utility.Strings
{
    /// <summary>
    /// Extensions and helper methods to string class
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsLable(this string parameter)
        {
            //return parameter.StartsWith("@") || parameter.EndsWith(":");
            return parameter.EndsWith(":");
        }
        public static bool IsHexNumber(this string parameter)
        {
            return (parameter.StartsWith("0x") || parameter.StartsWith("$"));
        }
        public static bool IsHexByte(this string parameter)
        {
            return parameter.StartsWith("0x");
        }
        public static bool IsHexWord(this string parameter)
        {
            return parameter.StartsWith("$");
        }
        public static bool IsBinary(this string parameter)
        {
            return parameter.EndsWith("b", StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsValidNumber(this string parameter)
        {
            if (parameter.Length == 0)
            {
                return false;
            }
            bool hex = false;
            if (parameter.IsHexByte())
            {
                hex = true;
                parameter = parameter.Substring(2);
            }
            else if (parameter.IsHexWord())
            {
                hex = true;
                parameter = parameter.Substring(1);
            }

            if (hex)
            {
                for (int i = 0; i < parameter.Length; i++)
                {
                    if (!parameter[i].IsHexDigit())
                    {
                        return false;
                    }
                }
            }
            else if (parameter.IsBinary())
            {
                for (int i = 0; i < parameter.Length - 1; i++)
                {
                    if (!(parameter[i] == '0' || parameter[i] == '1'))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < parameter.Length; i++)
                {
                    if (!char.IsNumber(parameter, i))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool ContainsWhiteSpace(this string text)
        {
            return text.Contains(char.IsWhiteSpace);
        }
        public static bool ContainsNonWhiteSpace(this string text)
        {
            return text.Contains(x => !char.IsWhiteSpace(x));
        }
        public static bool ContainsAnyOf(this string text, char[] toContain)
        {
            return text.Contains(toContain.Contains);
        }
        public static bool Contains(this string text, Predicate<char> test)
        {
            return text.Any(t => test(t));
        }


        public static bool TryGetValue(this string s, out int value)
        {
            value = 0;
            if (s.IsValidNumber())
            {
                value = s.GetValue();
                return true;
            }
            else return false;
        }

        public static bool Same(string a, int index1, string b, int index2, int length)
        {
            if (index1 < 0 || index2 < 0 || index1 + length > a.Length || index2 + length > b.Length)
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = 0; i < length; i++)
            {
                if (a[index1 + i] != b[index2 + i])
                {
                    return false;
                }
            }
            return true;
        }

        public static int GetValue(this string parameter)
        {
            int code = 0;
            if (parameter.IsHexNumber())
            {
                if (parameter.StartsWith("$"))
                    parameter = parameter.Substring(1);
                
                code = Convert.ToInt32(parameter, 16);
            }
            else if (parameter.IsBinary())
            {
                code = Convert.ToInt32(parameter.Substring(0, parameter.Length - 1), 2);                
            }
            else
            {
                if (int.TryParse(parameter, 
                    System.Globalization.NumberStyles.Integer, 
                    System.Globalization.CultureInfo.InvariantCulture, out code))
                {
                    return code;
                }
                else
                {
                    throw new ArgumentException("parameter(" + parameter + ")");
                }
            }

            return code;
        }
        public static string GetLableName(this string parameter)
        {
            //return parameter.Trim('@', ':');
            return parameter.TrimEnd(':');
        }
        public static int[] GetIndexes(this string text, string toFind)
        {
            List<int> results = new List<int>(text.Length);
            int lastIndex = text.IndexOf(toFind);
            while (lastIndex >= 0)
            {
                results.Add(lastIndex);
                if (lastIndex == text.Length - 1)
                {
                    break;
                }
                lastIndex = text.IndexOf(toFind, lastIndex + 1);
            }
            return results.ToArray();
        }

        public static int IndexOf(this string text, Predicate<char> match)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (match(text[i]))
                {
                    return i;
                }
            }
            return -1;
        }
        public static int LastIndexOf(this string text, Predicate<char> match)
        {
            int i;
            for (i = text.Length - 1; i >= 0 && match(text[i]); i--) ;
            return i;
        }

        public static string[] Split(this string text, params int[] indexes)
        {
            string[] result = new string[indexes.Length + 1];

            result[0] = text.Substring(0, indexes[0]);
            for (int i = 0; i < indexes.Length - 1; i++)
            {
                result[i + 1] = text.Substring(indexes[i] + 1, indexes[i + 1] - indexes[i] - 1);
            }
            result[result.Length - 1] = text.Substring(indexes[indexes.Length - 1] + 1);

            return result;
        }
        public static string[] Split(this string line, char[] separators, char[][] uniters)
        {
            List<string> parameters = new List<string>();

            int begIndex = 0;
            Stack<int> uniterIndexs = new Stack<int>();
            
            for (int i = 0; i < line.Length; i++)
            {
                for (int j = 0; j < uniters.Length; j++)
                {
                    if (line[i] == uniters[j][0])
                    {
                        uniterIndexs.Push(j);
                        break;
                    }
                    else if (uniterIndexs.Count > 0 && line[i] == uniters[uniterIndexs.Peek()][1])
                    {
                        uniterIndexs.Pop();
                        break;
                    }
                }

                if (uniterIndexs.Count == 0 && 
                    separators.Contains(line[i]))
                {
                    parameters.Add(line.Substring(begIndex, i - begIndex));
                    begIndex = i + 1;
                }
            }

            if (begIndex < line.Length)
            {
                parameters.Add(line.Substring(begIndex));
            }

            parameters.RemoveAll(string.IsNullOrEmpty);

            return parameters.ToArray();
        }
        public static string[] Split(this string line, ICollection<char> separators, Dictionary<char,char> uniters)
        {
            int dontCare;
            return line.Split(separators, uniters, out dontCare);
        }
        public static string[] Split(this string line, ICollection<char> separators, Dictionary<char, char> uniters, out int nonClosedParenthesis)
        {
            List<string> parameters = new List<string>();
            int startIndex = 0;
            Stack<char> uniterStack = new Stack<char>();
            for (int j = 0; j < line.Length; j++)
            {
                char currChar = line[j];
                if (separators.Contains(currChar))
                {
                    if (uniterStack.Count == 0)
                    {
                        if (j - startIndex > 0)
                        {
                            parameters.Add(line.Substring(startIndex, j - startIndex));
                        }
                        startIndex = j + 1;
                    }
                }
                else if (uniters.ContainsKey(currChar) 
                    && !(uniters[currChar] == currChar
                        && uniterStack.Count > 0 && uniterStack.Peek() == currChar))
                {
                    uniterStack.Push(currChar);
                }
                else if (uniters.ContainsValue(currChar))
                {
                    if (uniterStack.Count > 0 && uniters[uniterStack.Peek()] == currChar)
                    {
                        uniterStack.Pop();
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
            if (startIndex < line.Length)
            {
                parameters.Add(line.Substring(startIndex, line.Length - startIndex));
            }

            nonClosedParenthesis = uniterStack.Count;
            parameters.RemoveAll(string.IsNullOrEmpty);
            return parameters.ToArray();
        }


        public static string ReplaceEach(this string text, string[] toReplace, string[] with)
        {
            if (toReplace.Length != with.Length)
            {
                throw new ArgumentException("toReplace and with need to contain same amount of items.");
            }

            //The laxy way
            for (int i = 0; i < toReplace.Length; i++)
            {
                text = text.Replace(toReplace[i], with[i]);
            }

            return text;
        }

        public static int AmountInTheBeginning(this string text, char value)
        {
            int i = 0;
            while (i < text.Length && text[i] == value)
            {
                i++;
            }
            return i;
        }

        public static int AmountInTheEnd(this string text, char value)
        {
            int i = text.Length - 1;
            while (i >= 0 && text[i] == value)
            {
                i--;
            }
            return text.Length - 1 - i;
        }


        public static int Amount(this string text, char value)
        {
            return text.Count(character => character == value);
        }

        public static int Amount(this string text, char value, int start, int length)
        {
            int result = 0;
            for (int i = start; i < start + length; i++)
            {
                if (text[i] == value)
                {
                    result++;
                }
            }
            return result;
        }

        public static string Repeat(this string text, int toLength)
        {
            char[] rawText = new char[toLength];
            for (int i = 0; i < toLength; i++)
            {
                rawText[i] = text[i % text.Length];
            }
            return new string(rawText);
        }

        public static string ToLinedString(this string[] lines)
        {
            StringBuilder builder = new StringBuilder(lines.Length * 40);

            foreach (string line in lines)
            {
                builder.AppendLine(line);
            }

            return builder.ToString();
        }
        
        public static int AmountOfLines(this string text)
        {
            int lineAmount = Math.Max(text.Amount('\r'), text.Amount('\n'));
            return lineAmount;
        }

        public static bool GetMathStringValue(this string s, out int result)
        {
            if (s.IsValidNumber())
            {
                result = s.GetValue();
                return true;
            }
            else
            {
                try
                {
                    string[] temp = Parser.ShuntingYardAlgorithm(s);
                    result = Parser.EvaluateReversePolishNotation(temp);
                    return true;
                }
                catch (Exception)
                {
                    result = 0;
                    return false;
                }             
            }
        }

        public static int? GetMathStringValue(this string s)
        {
            int result;
            if (s.GetMathStringValue(out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static StringBuilder Remove(this StringBuilder bldr, int length)
        {
            return bldr.Remove(0, length);
        }
        public static StringBuilder RemoveFromEnd(this StringBuilder bldr, int length)
        {
            return bldr.Remove(bldr.Length - length, length);
        }
        [Obsolete("Use stringBuilder.Clear() instead", true)]
        public static StringBuilder RemoveAll(this StringBuilder bldr)
        {
            return bldr.Remove(0, bldr.Length);
        }
        public static StringBuilder Substring(this StringBuilder bldr, int index, int length)
        {
            StringBuilder subBldr = new StringBuilder(length);
            for (int i = index; i < index + length; i++)
            {
                subBldr.Append(bldr[i]);
            }
            return subBldr;
        }

        public static bool FirstNonWhiteSpaceIs(this string s, char c)
        {
            return (from t in s 
                    where !Char.IsWhiteSpace(t) 
                    select t == c).FirstOrDefault();
        }
    }
}