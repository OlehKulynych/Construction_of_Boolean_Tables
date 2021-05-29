using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DS_Lab5
{
    class Program
    {

        static int Calculate(IEnumerable<char> rpn, Dictionary<char, int> variableValue)
        {
            var stack = new Stack<bool>();

            foreach (var token in rpn)
                switch (token)
                {
                    case '-': stack.Push(!stack.Pop()); break;
                    case '+': stack.Push(stack.Pop() | stack.Pop()); break;
                    case '*': stack.Push(stack.Pop() & stack.Pop()); break;
                    case '>': stack.Push(stack.Pop() | !stack.Pop()); break;
                    case '=': stack.Push(stack.Pop() == stack.Pop()); break;
                    case '0': stack.Push(false); break;
                    case '1': stack.Push(true); break;
                    default: stack.Push(variableValue[token] == 1); break;
                }

            return stack.Pop() ? 1 : 0;
        }
        static IEnumerable GetAllCombinations(IList<char> variables, int index, Dictionary<char, int> varValues)
        {
            if (index >= variables.Count)
            {
                yield return null;
            }
            else
            {
                foreach (var val in Enumerable.Range(0, 2))
                {
                    varValues[variables[index]] = val;
                    foreach (var temp in GetAllCombinations(variables, index + 1, varValues))
                    {
                        yield return temp;
                    }
                }
            }
        }
        static IEnumerable<char> GetMyRPN(IEnumerable<char> expr)
        {
            Dictionary<char, int> priorityMap = new Dictionary<char, int>() {
                { '-', 5 },
                { '*', 4 },
                { '+', 3 },
                { '>', 2 },
                { '=', 1 }
            };
            var stack = new Stack<char>();
            foreach (char token in expr)
            {
                if (token == ')')
                {
                    while (stack.Peek() != '(')
                    {
                        yield return stack.Pop();
                    }
                    stack.Pop();
                }
                else if (!char.IsLetterOrDigit(token))
                {
                    if (token == '(')
                        stack.Push(token);
                    else
                    {
                        while (stack.Count > 0 && priorityMap.Keys.Contains(stack.Peek()) && priorityMap[stack.Peek()] > priorityMap[token])
                            yield return stack.Pop();
                        stack.Push(token);
                    }
                }
                else
                    yield return token;
            }
            foreach (char operation in stack)
            {
                yield return operation;
            }
        }


        static void Main(string[] args)
        {
            bool isCalculate = true;

            while (isCalculate)
            {
                Console.Write("Enter expression: ");

                var expr = Console.ReadLine();

                var list = new List<string>();

                foreach (var i in expr)
                {
                    if (Char.IsLetter(i))
                    {
                        list.Add(i.ToString());
                    }
                }

                list = list.Distinct().ToList();
                list.Sort();

                string str = "";

                foreach (var i in list)
                {
                    str += i;
                }

                try
                {
                    int count = 0;
                    var rpn = GetMyRPN(expr);
                    var varValues = new Dictionary<char, int>();
                    var headerShown = false;
                    foreach (var combination in GetAllCombinations(str.ToArray(), 0, varValues))
                    {
                        var res = Calculate(rpn, varValues);
                        if (!headerShown)
                        {
                            foreach (var var in varValues.Keys)
                            {
                                Console.Write(var + "\t");
                                count++;
                            }
                            
                            Console.WriteLine(expr);
                            headerShown = true;
                            Console.WriteLine(new String('-', count*8 + expr.Length));
                        }

                        foreach (var var in varValues.Values)
                        {
                            Console.Write(var + "\t");
                        }
                            

                        Console.WriteLine(res);
                    }
                    Console.WriteLine();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error expression!");
                }

                try
                {
                    Console.WriteLine("Do you want to continue? \n 1 — Yes \n Another number to exit.");
                    Console.Write("Enter number: ");
                    int number = Convert.ToInt32(Console.ReadLine());
                    if(number == 1)
                    {
                        isCalculate = true;
                    }
                    else
                    {
                        isCalculate = false;
                    }
                }
                catch(Exception)
                {
                    isCalculate = false;
                }
               
            }
        }
    }
}
