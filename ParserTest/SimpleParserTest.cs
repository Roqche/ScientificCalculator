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

        [TestCase("12+20", ExpectedResult = 32)]
        [TestCase("15+16*20", ExpectedResult = 335)]
        
        [TestCase(".21-5", ExpectedResult = 4.79)]
        [TestCase("5", ExpectedResult = 5)]
        [TestCase("-5", ExpectedResult = -5)]

        [TestCase("2++4", ExpectedResult = 6)]
        [TestCase("--84+1", ExpectedResult = 85)]
        [TestCase("--84++1", ExpectedResult = 85)]

        [TestCase("(2+3) + 5", ExpectedResult = 10)]
        [TestCase("((2+3) - (2*2)) + (1+1)", ExpectedResult = 3)]

        [TestCase("-2-4-6+5+3", ExpectedResult = -4)]
        [TestCase("-2+4-6+5-3", ExpectedResult = -2)]

        [TestCase("sin180+sin360", ExpectedResult = 0)]
        [TestCase("sin(-1/2)", ExpectedResult = 0)]
        [TestCase("sin(pi/2)", ExpectedResult = 1)]
        [TestCase("sin(pi/2)+cos(pi)", ExpectedResult = 0)]
        [TestCase("cos(pi)", ExpectedResult = -1)]


        public decimal MixedTest(string textToParse)
        {
            var parser = new Parser();
            var result = parser.Parse(textToParse);
            return result;
        }

        //[TestCase("(2+3)+5")]
        //public void Test2(string expression)
        //{
        //    List<string> result = new List<string>();
        //    List<string> tokens = new List<string>();

        //    tokens.Add("^\\(");// matches opening bracket
        //    tokens.Add("^([\\d.\\d]+)"); // matches floating point numbers
        //    tokens.Add("^[&|<=>!-+/*]+"); // matches operators and other special characters
        //    tokens.Add("^[\\w]+"); // matches words and integers
        //    tokens.Add("^[,]"); // matches ,
        //    tokens.Add("^[\\)]"); // matches closing bracket

        //    while (0 != expression.Length)
        //    {
        //        bool foundMatch = false;

        //        foreach (string token in tokens)
        //        {
        //            Match match = Regex.Match(expression, token);
        //            if (false == match.Success)
        //            {
        //                continue;
        //            }

        //            result.Add(match.Value);
        //            expression = Regex.Replace(expression, token, "");
        //            foundMatch = true;

        //            break;
        //        }

        //        if (false == foundMatch)
        //        {
        //            break;
        //        }
        //    }
        //}

        [TestCase("5^2", ExpectedResult = 25)]
        [TestCase("5^5", ExpectedResult = 3125)]
        [TestCase("(-5)^2", ExpectedResult = 25)]
        [TestCase("(-5)^3", ExpectedResult = -125)]
        [TestCase("(-5)^(-2)", ExpectedResult = 0.04)]
        [TestCase("(-5+2)^2", ExpectedResult = 9)]
        [TestCase("(-5+2)^(2)", ExpectedResult = 9)]
        [TestCase("-5^2", ExpectedResult = -25)]
        [TestCase("5^2+5", ExpectedResult = 30)]
        [TestCase("5-5^2", ExpectedResult = -20)]
        public decimal TestPower(string expression)
        {
            var parser = new Parser();
            var result = parser.Parse(expression);
            return result;
        }

        [TestCase("sqrt(2)(4)", ExpectedResult = 2)]
        [TestCase("sqrt(3)(27)", ExpectedResult = 3)]
        [TestCase("sqrt(2)(4*4)", ExpectedResult = 4)]
        [TestCase("sqrt(3.5)(4*15)", ExpectedResult = 3.2214)]
        [TestCase("2 + sqrt(2)(4)", ExpectedResult = 4)]
        [TestCase("2 + sqrt(2)(4)-2", ExpectedResult = 2)]
        [TestCase("sqrt(4)", ExpectedResult = 2)]
        public decimal TestRoots(string expression)
        {
            var parser = new Parser();
            var result = parser.Parse(expression);
            return decimal.Round(result, 4);
        }
    }
}