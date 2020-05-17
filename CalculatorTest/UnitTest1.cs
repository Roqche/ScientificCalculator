using Calculator;
using NUnit.Framework;
using ParserCore;
using System.Collections.Generic;
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
        [TestCase("sin15+sin30", ExpectedResult = -0.338)]
        [TestCase(".21-5", ExpectedResult = -4.79)]
        [TestCase("5", ExpectedResult = 5)]
        [TestCase("-5", ExpectedResult = -5)]
        [TestCase("(2+3) + 5", ExpectedResult = 10)]
        [TestCase("((2+3) - (2*2)) + (1+1)", ExpectedResult = 3)]
        [TestCase("sin(cos(tg(15+30)))", ExpectedResult = 3)]
        [TestCase("15+20*35*15*15/12-140+150+(2(15-16)\\2)", ExpectedResult = 3)]

        public decimal Test1(string textToParse)
        {
            var parser = new Parser();
            var actions = parser.Parse(textToParse);

            var calculator = new CalculationProcessor(actions);
            var result = calculator.Calculate();

            return result;
        }
    }
}