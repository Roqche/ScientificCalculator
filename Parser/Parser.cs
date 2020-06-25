using Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParserCore
{
    public class Parser
    {
        private string textToParse;
        private int depthLevel = 0;

        public Parser(string textToParse)
        {
            this.textToParse = textToParse;
        }

        public decimal Parse()
        {
            // If the number contains a fraction, it is required that it does not start with decimal point.
            //CompleteNumbersToCorrectFormat(textToParse);

            var doSomeParsing = textToParse;

            do
            {
                var textToProcess = textToParse;

                while (textToProcess.Contains('(') && 
                    (!Regex.IsMatch(textToProcess, @"\(-?\w+[,.]?\w*\)") 
                    || Regex.IsMatch(textToProcess, @"\((\(*-*\w+[,.]?\w*\)*\s*[-+*\/\^]\s*)+\w+[,.]?\w*\)") 
                    || Regex.IsMatch(textToProcess, @"\(\w+(\(-?\w+[,.]?\w*\))+\)") 
                    || Regex.IsMatch(textToProcess, @"\(\w+\(.*\)\s*[-+*\/\^]\s*\w+\(.*\)\)")))
                {
                    textToProcess = DivideByBrackets(textToProcess);
                }

                while (depthLevel < 8)
                {
                    ++depthLevel;
                    textToProcess = DivideByOperatorsPriority(textToProcess);
                }

                depthLevel = 0;
            } while (textToParse.Contains('(') && textToParse != doSomeParsing);

            return decimal.Parse(textToParse);
        }

        private void CompleteNumbersToCorrectFormat(string textToParse)
        {
            throw new NotImplementedException();
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
                        var standardNumber = @"\(?-?\w+[,.]?\w*\)?";

                        expressionsToCalculate = DefaultDivider(
                            textToProcess,
                            $@"sin{standardNumber}|cos{standardNumber}|tg{standardNumber}|sinh{standardNumber}|cosh{standardNumber}|tgh{standardNumber}"
                        );

                        expressionType = CalculationType.TrygoFunc;
                        break;
                    case 2:
                        expressionsToCalculate = DefaultDivider(textToProcess, @"ln\(\w+[,.]?\w*\)|log\(\w+[,.]?\w*\)(\(\w+[,.]?\w*\))?");
                        expressionType = CalculationType.Logarithm;
                        break;
                    case 3:
                        expressionsToCalculate = DefaultDivider(textToProcess, @"sqrt\(\w+[,.]?\w*\)(\(\w+[,.]?\w*\))?");
                        expressionType = CalculationType.Root;
                        break;
                    case 4:
                        expressionsToCalculate = ToPower(textToProcess);
                        expressionType = CalculationType.Exponentation;
                        break;
                    case 5:
                        expressionsToCalculate = DefaultDivider(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\/\s*\(?\w+[,.]?\w*\)?");
                        expressionType = CalculationType.Division;
                        break;
                    case 6:
                        expressionsToCalculate = DefaultDivider(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\*\s*\(?-*\w+[,.]?\w*\)?");
                        expressionType = CalculationType.Multiplication;
                        break;
                    case 7:
                        expressionsToCalculate = DefaultDivider(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\-+\s*\(?\w+[,.]?\w*\)?");
                        expressionType = CalculationType.Substraction;
                        break;
                    case 8:
                        expressionsToCalculate = DefaultDivider(textToProcess, @"\(?-*\w+[,.]?\w*\)?\s*\+\s*\(?-*\w+[,.]?\w*\)?");
                        expressionType = CalculationType.Addition;
                        break;
                }

                if (expressionsToCalculate.Count == 0)
                {
                    break;
                }

                var calculator = new CalculationProcessor(expressionsToCalculate, expressionType);
                var result = calculator.Calculate();

                while (expressionsToCalculate.Count > 0)
                {
                    textToProcess = textToProcess.Replace(expressionsToCalculate.First(), result.First());
                    textToParse = textToParse.Replace(expressionsToCalculate.First(), result.First());
                    result.RemoveAt(0);
                    expressionsToCalculate.RemoveAt(0);
                }
            }

            return textToProcess;
        }

        private List<string> DefaultDivider(string textToProcess, string pattern)
        {
            var matchedExpression = new List<string>();

            var collection = Regex.Matches(textToProcess, pattern);
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
