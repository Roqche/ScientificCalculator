using Calculator;
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

        public decimal Parse(string textToParse)
        {
            // If the number contains a fraction, it is required that it does not start with decimal point.
            CompleteNumbersToCorrectFormat(textToParse);

            sequenceOfActions[depthLevel] = new List<string>() { textToParse };
            var doWhile = 0;
            while (doWhile <= 4)
            {
                doWhile++;
                ++depthLevel;
                var expressionsToParse = DivideByOperatorsPriority();
            }

            return decimal.Parse(sequenceOfActions[0][0]);
        }

        private void CompleteNumbersToCorrectFormat(string textToParse)
        {
            //var a = Regex.Matches(textToParse, @"[,.]");
            var a = Regex.Split(textToParse, @"[,.]");
            //var a = Regex.Matches(textToParse, @"(?!\d[.,])");

        }

        private List<string> DivideByOperatorsPriority()
        {
            var expressionsToParse = new List<string>();
            var expressionsToCalculate = new List<string>();

            while (true)
            {
                switch (depthLevel)
                {
                    case 1:
                        expressionsToCalculate = DivideByDivision(1);
                        break;
                    case 2:
                        expressionsToCalculate = DivideByMultiplication(1);
                        break;
                    case 3:
                        expressionsToCalculate = DivideByMinus(1);
                        break;
                    case 4:
                        expressionsToCalculate = DivideByPlus(1);
                        break;
                }

                if (expressionsToCalculate.Count == 0)
                {
                    break;
                }

                var calculator = new CalcProcessor2(expressionsToCalculate);
                var result = calculator.Calculate();

                for (int i = 0; i < expressionsToCalculate.Count; i++)
                {
                    sequenceOfActions[0] = sequenceOfActions[0].Select(s => s.Replace(expressionsToCalculate.First(), result.First())).ToList();
                    result.RemoveAt(0);
                    expressionsToCalculate.RemoveAt(0);
                }
            }


            return expressionsToParse;
        }

        private List<string> DivideByMinus(int priority)
        {
            var matchedExpression = new List<string>();
            //var sign = '-';
            foreach (var a in sequenceOfActions[priority - 1])
            {
                var collection = Regex.Matches(a, @"(-*\w+[,.]?\w*\-\w+[,.]?\w*)");
                foreach (var m in collection)
                {
                    matchedExpression.Add(m.ToString());
                }
            }

            return matchedExpression;
        }

        private List<string> DivideByPlus(int priority)
        {
            var matchedExpression = new List<string>();
            //var sign = '+';
            foreach (var a in sequenceOfActions[priority - 1])
            {
                var collection = Regex.Matches(a, @"(-*\w+[,.]?\w*\+\w+[,.]?\w*)");
                foreach (var m in collection)
                {
                    matchedExpression.Add(m.ToString());
                }
            }

            return matchedExpression;
        }

        private List<string> DivideByMultiplication(int priority)
        {
            var matchedExpression = new List<string>();
            //var sign = '+';
            foreach (var a in sequenceOfActions[priority - 1])
            {
                var collection = Regex.Matches(a, @"(-*\w+[,.]?\w*\*\w+[,.]?\w*)");
                foreach (var m in collection)
                {
                    matchedExpression.Add(m.ToString());
                }
            }

            return matchedExpression;
        }

        private List<string> DivideByDivision(int priority)
        {
            var matchedExpression = new List<string>();
            //var sign = '+';
            foreach (var a in sequenceOfActions[priority - 1])
            {
                var collection = Regex.Matches(a, @"(-*\w+[,.]?\w*\/\w+[,.]?\w*)");
                foreach (var m in collection)
                {
                    matchedExpression.Add(m.ToString());
                }
            }

            return matchedExpression;
        }

        //public Dictionary<int, List<string>> Parse(string textToParse)
        //{
        //    sequenceOfActions[depthLevel] = new List<string>() { textToParse };

        //    var expressionsToParse = GetExpressionsToParseOnCurrentLevel();
        //    while (expressionsToParse.Count > 0)
        //    {
        //        ++depthLevel;
        //        sequenceOfActions[depthLevel] = new List<string>();
        //        foreach (var expression in expressionsToParse)
        //        {
        //            //if(Regex.IsMatch(expression, @"[a-zA-Z]+\(.+\)"))
        //            //{
        //            //    DivideByWords(expression, depthLevel);
        //            //}
        //            if (expression.Contains("("))
        //            {
        //                DivideByBrackets(expression, depthLevel);
        //            }
        //            else
        //            {
        //                DivideByOperators(expression, depthLevel);
        //            }
        //        }


        //        expressionsToParse = GetExpressionsToParseOnCurrentLevel();
        //    }

        //    return new Dictionary<int, List<string>>(sequenceOfActions);
        //}

        //private List<string> GetExpressionsToParseOnCurrentLevel()
        //{
        //    var expressionsToParse = new List<string>();
        //    var operators = new List<char>() { '+', '-', '*', '/', '^', '(' };

        //    foreach (var expression in sequenceOfActions[depthLevel])
        //    {
        //        var amountOfOperators = 0;
        //        foreach (var mo in operators)
        //        {
        //            amountOfOperators += expression.Count(c => c == mo);
        //        }

        //        if (amountOfOperators > 1)
        //        {
        //            expressionsToParse.Add(expression);
        //        }
        //    }

        //    return new List<string>(expressionsToParse);
        //}

        //private void DivideByBrackets(string textToParse, int deepingLevel)
        //{
        //    var a = textToParse.IndexOf('(');
        //    var b = textToParse.IndexOf(')');

        //    while (true)
        //    {
        //        var actions = new List<string>();
        //        var textToVerify = textToParse.Substring(a, b - a + 1);
        //        if (IsProperNumberOfParenthesesInText(textToVerify))
        //        {
        //            actions.Add(textToVerify.Substring(1, textToVerify.Length - 2));
        //            var rest = textToParse.Split(textToVerify);
        //            foreach (var r in rest)
        //            {
        //                if (!string.IsNullOrWhiteSpace(r))
        //                {
        //                    actions.Add(PrepareRestExpression(r.Trim()));
        //                }
        //            }

        //            sequenceOfActions[deepingLevel].AddRange(actions);
        //            break;
        //        }
        //        else
        //        {
        //            b = textToParse.IndexOf(')', ++b);
        //        }
        //    }
        //}

        //private void DivideByOperators(string expression, int depthLevel)
        //{
        //    var power = Regex.Matches(expression, @"\w+[\^]\w+");

        //    var productQuotient = Regex.Matches(expression, @"\w+[*/]\w+");

        //}

        //private void DivideByWords(string expression, int depthLevel)
        //{
        //    var roots = Regex.Matches(expression, @"sqrt\(.+\)");

        //    var actions = new List<string>();
        //    foreach (var match in roots)
        //    {

        //    }
        //}

        //private bool IsProperNumberOfParenthesesInText(string textToVerify)
        //{
        //    int numberOfLeftParentheses = textToVerify.Count(pl => pl == '(');
        //    int numberOfRightParentheses = textToVerify.Count(pr => pr == ')');

        //    return numberOfLeftParentheses == numberOfRightParentheses;
        //}

        //private string PrepareRestExpression(string rest)
        //{
        //    var result = rest;
        //    foreach (var op in Operators.operators)
        //    {
        //        result = result.TrimStart(op).TrimEnd(op);
        //    }

        //    return result.Trim();
        //}
    }
}
