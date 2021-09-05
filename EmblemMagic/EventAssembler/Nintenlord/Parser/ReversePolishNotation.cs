using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Parser
{
    public static class ReversePolishNotation
    {
        public enum Associativity
        {
            Left,
            Right
        }

        public struct OperatorInfo<T>
        {
            public int Precedance;
            public Associativity Associativity;
            public int ArgumentCount;
            public Func<T[], T> Function;
        }


        public static List<T> ShuntingYardAlgorithm<T>(
            IEnumerable<T> tokens, 
            IDictionary<T, T> parenthesis, 
            IDictionary<T, OperatorInfo<T>> operators,
            Predicate<T> isFunction,
            Predicate<T> isParameterSeparator,
            IEqualityComparer<T> eq)
        {
            Stack<T> operatorStack = new Stack<T>();
            List<T> output = new List<T>();
            foreach (var token in tokens)
            {
                if (parenthesis.ContainsKey(token))
                {
                    operatorStack.Push(token);
                }
                else if (parenthesis.Values.Contains(token))
                {
                    bool found = false;
                    while (operatorStack.Count > 0)
                    {
                        T temp;
                        if (parenthesis.TryGetValue(operatorStack.Peek(), out temp))
                        {
                            if (eq.Equals(temp, token))
                            {
                                found = true;
                                break;
                            }
                        }
                        else
                        {
                            output.Add(operatorStack.Pop());
                        }
                    }
                    if (!found)
                    {
                        throw new ArgumentException("Missmatched parenthesis.");
                    }

                    if (operatorStack.Count > 0 && 
                        isFunction(operatorStack.Peek()))
                    {
                        output.Add(operatorStack.Pop());
                    } 
                }
                else
                {
                    OperatorInfo<T> tempInfo;
                    if (operators.TryGetValue(token, out tempInfo))
                    {
                        while (operatorStack.Count > 0)
                        {
                            T operatorToken = operatorStack.Peek();
                            OperatorInfo<T> info;
                            if (operators.TryGetValue(operatorToken, out info))
                            {
                                switch (tempInfo.Associativity)
                                {
                                    case Associativity.Left:
                                        if (tempInfo.Precedance <= info.Precedance)
                                        {
                                            output.Add(operatorStack.Pop());
                                        }
                                        break;
                                    case Associativity.Right:
                                        if (tempInfo.Precedance < info.Precedance)
                                        {
                                            output.Add(operatorStack.Pop());
                                        }
                                        break;
                                }
                            }
                        }
                        operatorStack.Push(token);
                    }
                    else if (isParameterSeparator(token))
                    {
                        bool found = false;
                        while (operatorStack.Count > 0)
                        {
                            if (parenthesis.ContainsKey(operatorStack.Peek()))
                            {
                                found = true;
                            }
                            else
                            {
                                output.Add(operatorStack.Pop());
                            }
                        }
                        if (!found)
                        {
                            throw new ArgumentException("Missmatched parenthesis.");
                        }
                    }
                    else
                    {
                        output.Add(token);
                    }
                }
            }

            foreach (var item in operatorStack)
            {
                if (parenthesis.ContainsKey(item))
                {
                    throw new ArgumentException("Missmatched parenthesis.");
                }
                output.Add(item);
            }

            return output;
        }

        public static T Evaluate<T>(IEnumerable<T> rpnValue, IDictionary<T, OperatorInfo<T>> operators)
        {
            Stack<T> stack = new Stack<T>(10);
            OperatorInfo<T> info;
            foreach (var token in rpnValue)
            {
                if (operators.TryGetValue(token, out info))
                {
                    if (info.ArgumentCount > stack.Count)
                    {
                        throw new ArgumentException("Not enough parameters for a function");
                    }
                    T[] parameters = new T[info.ArgumentCount];
                    for (int i = stack.Count - 1; i >= 0; i--)
                    {
                        parameters[i] = stack.Pop();
                    }
                    stack.Push(info.Function(parameters));
                }
                else
                {
                    stack.Push(token);
                }
            }

            if (stack.Count == 1)
            {
                return stack.Pop();
            }
            else if (stack.Count > 1)
            {
                throw new ArgumentException("Too many values left.");                
            }
            else
            {
                throw new ArgumentException("0 values left.");
            }
        }
    }
}
