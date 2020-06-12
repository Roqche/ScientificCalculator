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
        [TestCase("0.21-5", ExpectedResult = -4.79)]
        [TestCase("5-0.21", ExpectedResult = -4.79)]
        [TestCase("5", ExpectedResult = 5)]
        [TestCase("-5", ExpectedResult = -5)]
        [TestCase("(2+3) + 5", ExpectedResult = 10)]
        [TestCase("(2+3) * 5", ExpectedResult = 25)]
        [TestCase("((2+3) - (2*2)) + (1+1)", ExpectedResult = 3)]
        [TestCase("sin(cos(tg(15+30)))", ExpectedResult = 3)]
        [TestCase("15+20*35*15*15/12-140+150+(2(15-16)/2)", ExpectedResult = 13149)]
        [TestCase("15+20*35*15*15/12-140+150", ExpectedResult = 13150)]
        [TestCase("7*sqrt(4+4)+3", ExpectedResult = 24)]
        [TestCase("sqrt(4+4)", ExpectedResult = 24)]

        public decimal Test1(string textToCalculate)
        {
            var parser = new Parser();
            var result = parser.Parse(textToCalculate);
            return result;
        }

        [TestCase("sqrt(4)", ExpectedResult = 2)]
        public decimal TestSingleEquation(string textToCalculate)
        {
            //var parser = new Parser();
            //var actions = parser.Parse(textToCalculate);

            //var calculator = new CalculationProcessor(actions);
            //var result = calculator.Calculate();

            return default;
        }
    }
}