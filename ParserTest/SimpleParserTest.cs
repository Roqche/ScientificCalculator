using Calculator;
using NUnit.Framework;
using ParserCore;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace ParserTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("12+20", ExpectedResult = true)]
        [TestCase("15+16*20", ExpectedResult = true)]
        [TestCase("sin15+sin30", ExpectedResult = true)]
        [TestCase(".21-5", ExpectedResult = true)]
        [TestCase("5", ExpectedResult = true)]
        [TestCase("-5", ExpectedResult = true)]

        [TestCase("2++4", ExpectedResult = false)]
        [TestCase("--84+1", ExpectedResult = false)]
        [TestCase("--84+1", ExpectedResult = false)]

        [TestCase("(2+3) + 5", ExpectedResult = true)]
        [TestCase("((2+3) - (2*2)) + (1+1)", ExpectedResult = true)]

        public bool Test1(string textToParse)
        {
            var parser = new Parser();
            var actions = parser.Parse(textToParse);
            var calculator = new CalculationProcessor(actions);
            calculator.Calculate();

            var result = false;
            return result;
        }

        [TestCase("(2+3)+5")]
        public void Test2(string expression)
        {
            List<string> result = new List<string>();
            List<string> tokens = new List<string>();

            tokens.Add("^\\(");// matches opening bracket
            tokens.Add("^([\\d.\\d]+)"); // matches floating point numbers
            tokens.Add("^[&|<=>!-+/*]+"); // matches operators and other special characters
            tokens.Add("^[\\w]+"); // matches words and integers
            tokens.Add("^[,]"); // matches ,
            tokens.Add("^[\\)]"); // matches closing bracket

            while (0 != expression.Length)
            {
                bool foundMatch = false;

                foreach (string token in tokens)
                {
                    Match match = Regex.Match(expression, token);
                    if (false == match.Success)
                    {
                        continue;
                    }

                    result.Add(match.Value);
                    expression = Regex.Replace(expression, token, "");
                    foundMatch = true;

                    break;
                }

                if (false == foundMatch)
                {
                    break;
                }
            }
        }
    }
}