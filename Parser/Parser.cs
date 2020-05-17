using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParserCore
{
    public class Parser
    {
        private Dictionary<int, List<string>> sequenceOfActions = new Dictionary<int, List<string>>();
        private int depthLevel = 0;

        public Parser()
        {

        }

        public Dictionary<int, List<string>> Parse(string textToParse)
        {
            sequenceOfActions[depthLevel] = new List<string>() { textToParse };

            var expressionsToParse = GetExpressionsToParseOnCurrentLevel();
            while (expressionsToParse.Count > 0)
            {
                ++depthLevel;
                sequenceOfActions[depthLevel] = new List<string>();
                foreach (var expression in expressionsToParse)
                {
                    if (expression.Contains("("))
                    {
                        DivideByBrackets(expression, depthLevel);
                    }
                    else
                    {
                        DivideByOperators(expression, depthLevel);
                    }
                }


                expressionsToParse = GetExpressionsToParseOnCurrentLevel();
            }

            return new Dictionary<int, List<string>>(sequenceOfActions);
        }

        private List<string> GetExpressionsToParseOnCurrentLevel()
        {
            var expressionsToParse = new List<string>();
            var operators = new List<char>() { '+', '-', '*', '/', '^', '(' };

            foreach (var expression in sequenceOfActions[depthLevel])
            {
                var amountOfOperators = 0;
                foreach (var mo in operators)
                {
                    amountOfOperators += expression.Count(c => c == mo);
                }

                if (amountOfOperators > 1)
                {
                    expressionsToParse.Add(expression);
                }
            }

            return new List<string>(expressionsToParse);
        }

        private void DivideByBrackets(string textToParse, int deepingLevel)
        {
            var a = textToParse.IndexOf('(');
            var b = textToParse.IndexOf(')');

            while (true)
            {
                var actions = new List<string>();
                var textToVerify = textToParse.Substring(a, b - a + 1);
                if (IsProperNumberOfParenthesesInText(textToVerify))
                {
                    actions.Add(textToVerify.Substring(1, textToVerify.Length - 2));
                    var rest = textToParse.Split(textToVerify);
                    foreach (var r in rest)
                    {
                        if (!string.IsNullOrWhiteSpace(r))
                        {
                            actions.Add(PrepareRestExpression(r.Trim()));
                        }
                    }

                    sequenceOfActions[deepingLevel].AddRange(actions);
                    break;
                }
                else
                {
                    b = textToParse.IndexOf(')', ++b);
                }
            }
        }

        private void DivideByOperators(string expression, int depthLevel)
        {
            var o = Regex.Match(expression, "\\w+[*/]\\w+");
            
            
        }

        private bool IsProperNumberOfParenthesesInText(string textToVerify)
        {
            int numberOfLeftParentheses = textToVerify.Count(pl => pl == '(');
            int numberOfRightParentheses = textToVerify.Count(pr => pr == ')');

            return numberOfLeftParentheses == numberOfRightParentheses;
        }

        private string PrepareRestExpression(string rest)
        {
            var result = rest;
            foreach (var op in Operators.operators)
            {
                result = result.TrimStart(op).TrimEnd(op);
            }

            return result.Trim();
        }
    }
}
