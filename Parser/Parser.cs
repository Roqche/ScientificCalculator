using Calculator;
using System.Collections.Generic;
using System.Linq;
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

            var collection = Regex.Matches(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\-+\s*\(?\w+[,.]?\w*\)?");
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

        private bool IsProperNumberOfParenthesesInText(string textToVerify)
        {
            int numberOfLeftParentheses = textToVerify.Count(pl => pl == '(');
            int numberOfRightParentheses = textToVerify.Count(pr => pr == ')');

            return numberOfLeftParentheses == numberOfRightParentheses;
        }
    }
}
