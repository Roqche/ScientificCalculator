using Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ParserCore
{
    public class Parser
    {
        private string TextToParse;
        private int depthLevel = 0;

        public Parser()
        {

        }

        public decimal Parse(string textToParse)
        {
            // If the number contains a fraction, it is required that it does not start with decimal point.
            //CompleteNumbersToCorrectFormat(textToParse);
            TextToParse = textToParse;

            do
            {
                var textToProcess = TextToParse;
                //foreach (var singleNumberInBrackets in Regex.Matches(textToProcess, @"\(-?\w+[,.]?\w*\)"))
                //{
                //    var index = textToProcess.IndexOf(singleNumberInBrackets.ToString());
                //    textToProcess = textToProcess.Remove(index, singleNumberInBrackets.ToString().Length);
                //}

                while (textToProcess.Contains('(') && (!Regex.IsMatch(textToProcess, @"\(-?\w+[,.]?\w*\)") || Regex.IsMatch(textToProcess, @"\(-*?\w+[,.]?\w*\s*[-+*\/\^]\s*\w+[,.]?\w*\)")))
                {
                    textToProcess = DivideByBrackets(textToProcess);
                }

                while (depthLevel < 8)
                {
                    ++depthLevel;
                    textToProcess = DivideByOperatorsPriority(textToProcess);
                }

                depthLevel = 0;
            } while (TextToParse.Contains('('));

            return decimal.Parse(TextToParse);
        }

        private void CompleteNumbersToCorrectFormat(string textToParse)
        {
            //var a = Regex.Matches(textToParse, @"[,.]");
            //var a = Regex.Split(textToParse, @"[,.]");
            //var a = Regex.Matches(textToParse, @"(?!\d[.,])");

        }

        private string DivideByOperatorsPriority(string textToProcess)
        {
            var expressionsToCalculate = new List<string>();
            var expressionType = CalculationType.Addition;

            while (true)
            {
                switch (depthLevel)
                {
                    case 1:
                        expressionsToCalculate = TrygonometricFunctions(textToProcess);
                        expressionType = CalculationType.TrygoFunc;
                        break;
                    case 2:
                        expressionsToCalculate = Logarithm(textToProcess);
                        expressionType = CalculationType.Logarithm;
                        break;
                    case 3:
                        expressionsToCalculate = Roots(textToProcess);
                        expressionType = CalculationType.Root;
                        break;
                    case 4:
                        expressionsToCalculate = ToPower(textToProcess);
                        expressionType = CalculationType.Exponentation;
                        break;
                    case 5:
                        expressionsToCalculate = DivideByDivision(textToProcess);
                        expressionType = CalculationType.Division;
                        break;
                    case 6:
                        expressionsToCalculate = DivideByMultiplication(textToProcess);
                        expressionType = CalculationType.Multiplication;
                        break;
                    case 7:
                        expressionsToCalculate = DivideByMinus(textToProcess);
                        expressionType = CalculationType.Substraction;
                        break;
                    case 8:
                        expressionsToCalculate = DivideByPlus(textToProcess);
                        expressionType = CalculationType.Addition;
                        break;
                }

                if (expressionsToCalculate.Count == 0)
                {
                    break;
                }

                var calculator = new CalcProcessor2(expressionsToCalculate, expressionType);
                var result = calculator.Calculate();

                while (expressionsToCalculate.Count > 0)
                {
                    textToProcess = textToProcess.Replace(expressionsToCalculate.First(), result.First());
                    TextToParse = TextToParse.Replace(expressionsToCalculate.First(), result.First());
                    result.RemoveAt(0);
                    expressionsToCalculate.RemoveAt(0);
                }
            }

            return textToProcess;
        }

        private List<string> DivideByMinus(string textToProcess)
        {
            var matchedExpression = new List<string>();
            //var sign = '-';

            var collection = Regex.Matches(textToProcess, @"\(?-*?\w+[,.]?\w*\)?\s*\-+\s*\(?\w+[,.]?\w*\)?");
            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
            }

            return matchedExpression;
        }

        private List<string> DivideByPlus(string textToProcess)
        {
            var matchedExpression = new List<string>();
            //var sign = '+';

            var collection = Regex.Matches(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\+\s*\(?-*\w+[,.]?\w*\)?");
            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
            }

            return matchedExpression;
        }

        private List<string> DivideByMultiplication(string textToProcess)
        {
            var matchedExpression = new List<string>();
            //var sign = '+';

            var collection = Regex.Matches(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\*\s*\(?-*\w+[,.]?\w*\)?");
            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
            }

            return matchedExpression;
        }

        private List<string> DivideByDivision(string textToProcess)
        {
            var matchedExpression = new List<string>();
            //var sign = '+';

            var collection = Regex.Matches(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\/\s*\(?\w+[,.]?\w*\)?");
            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
            }

            return matchedExpression;
        }

        private List<string> ToPower(string textToProcess)
        {
            var matchedExpression = new List<string>();
            //When Base is negative or/and index is negative, for example (-5)^(-2)
            var collection = Regex.Matches(textToProcess, @"(\(-\w+[,.]?\w*\)\s*\^\s*\(-\w+[,.]?\w*\))|(\(-\w+[,.]?\w*\)\s*\^\s*\(?\w+[,.]?\w*\)?)|(\(?\w+[,.]?\w*\)?\s*\^\s*\(-\w+[,.]?\w*\))");
            if (collection?.Count == 0)
            {

                collection = Regex.Matches(textToProcess, @"\(?\w+[,.]?\w*\)?\s*\^\s*\(?\w+[,.]?\w*\)?");
            }
            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
            }

            return matchedExpression;
        }

        private List<string> Roots(string textToProcess)
        {
            var matchedExpression = new List<string>();

            var collection = Regex.Matches(textToProcess, @"sqrt\(\w+[,.]?\w*\)(\(\w+[,.]?\w*\))?");

            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
            }

            return matchedExpression;
        }

        private List<string> TrygonometricFunctions(string textToProcess)
        {
            var matchedExpression = new List<string>();
            var standardNumber = @"\(?-?\w+[,.]?\w*\)?";

            var collection = Regex.Matches(textToProcess, 
                $@"sin{standardNumber}|cos{standardNumber}|tg{standardNumber}|sinh{standardNumber}|cosh{standardNumber}|tgh{standardNumber}");

            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
            }

            return matchedExpression;
        }

        private List<string> Logarithm(string textToProcess)
        {
            var matchedExpression = new List<string>();

            var collection = Regex.Matches(textToProcess, @"ln\(\w+[,.]?\w*\)|log\(\w+[,.]?\w*\)(\(\w+[,.]?\w*\))?");

            foreach (var m in collection)
            {
                matchedExpression.Add(m.ToString());
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

        private string DivideByBrackets(string textToParse)
        {
            var a = textToParse.IndexOf('(');
            var b = textToParse.IndexOf(')');
            var textToVerify = textToParse.Substring(a, b - a + 1);

            if (Regex.IsMatch(textToVerify, @"^\(-?\w+[,.]?\w*\)$"))
            {
                a = textToParse.IndexOf('(', b++);
                b = textToParse.IndexOf(')', b++);
                textToVerify = textToParse.Substring(a, b - a + 1);
            }

            while (!IsProperNumberOfParenthesesInText(textToVerify))
            {
                b = textToParse.IndexOf(')', ++b);
                textToVerify = textToParse.Substring(a, b - a + 1);
            }

            return textToVerify.Substring(1, textToVerify.Length - 2);
        }

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
