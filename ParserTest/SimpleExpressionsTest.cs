using NUnit.Framework;
using ParserCore;

namespace ParserTest
{
    public class SimpleExpressionsTest
    {
        [TestCase("12+20", ExpectedResult = 32)]
        [TestCase("15+16*20", ExpectedResult = 335)]

        [TestCase(".21-5", ExpectedResult = 4.79)]
        [TestCase("5", ExpectedResult = 5)]
        [TestCase("-5", ExpectedResult = -5)]

        //[TestCase("2++4", ExpectedResult = 6)] //Shouldn't work
        [TestCase("--84+1", ExpectedResult = 85)]
        //[TestCase("--84++1", ExpectedResult = 85)] //Shouldn't work
        [TestCase("--84--1", ExpectedResult = 85)]
        [TestCase("--84*-1", ExpectedResult = -84)]
        [TestCase("(2+3) + 5", ExpectedResult = 10)]
        [TestCase("(2+3) + 5", ExpectedResult = 10)]
        [TestCase("(2+3) * 5", ExpectedResult = 25)]
        [TestCase("0.21-5", ExpectedResult = -4.79)]
        [TestCase("5-0.21", ExpectedResult = 4.79)]
        [TestCase("(1*2*3)+1", ExpectedResult = 7)]
        public decimal MixedTest(string textToParse)
        {
            var parser = new Parser(textToParse);
            var result = parser.Parse();
            return result;
        }

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
            var parser = new Parser(expression);
            var result = parser.Parse();
            return result;
        }

        [TestCase("sqrt(2)(4)", ExpectedResult = 2)]
        [TestCase("sqrt(3)(27)", ExpectedResult = 3)]
        [TestCase("sqrt(2)(4*4)", ExpectedResult = 4)]
        [TestCase("sqrt(3.5)(4*15)", ExpectedResult = 3.2214)]
        [TestCase("2 + sqrt(2)(4)", ExpectedResult = 4)]
        [TestCase("2 + sqrt(2)(4)-2", ExpectedResult = 2)]
        [TestCase("sqrt(4)", ExpectedResult = 2)]
        [TestCase("sqrt(4*4)", ExpectedResult = 4)]
        public decimal TestRoots(string expression)
        {
            var parser = new Parser(expression);
            var result = parser.Parse();
            return decimal.Round(result, 4);
        }
        
        [TestCase("ln(e)", ExpectedResult = 1)]
        [TestCase("log(10)(10)", ExpectedResult = 1)]
        [TestCase("log(2)(4)", ExpectedResult = 2)]
        [TestCase("log(4)", ExpectedResult = 2)]

        [TestCase("2*ln(e)", ExpectedResult = 2)]
        [TestCase("log(10)(10) + 5", ExpectedResult = 6)]
        [TestCase("4 - log(2)(4)", ExpectedResult = 2)]
        [TestCase("8/log(4)", ExpectedResult = 4)]
        public decimal TestLogarithmic(string expression)
        {
            var parser = new Parser(expression);
            var result = parser.Parse();
            return decimal.Round(result, 4);
        }

        [TestCase("sin(pi/2)", ExpectedResult = 1)]
        [TestCase("sin(pi)", ExpectedResult = 0)]
        [TestCase("sin(pi/2)+cos(pi)", ExpectedResult = 0)]
        [TestCase("cos(pi)", ExpectedResult = -1)]
        [TestCase("cos(pi/2)", ExpectedResult = 0)]
        [TestCase("cos(0)", ExpectedResult = 1)]

        [TestCase("sin(pi/2)*2", ExpectedResult = 2)]
        [TestCase("5+sin(pi/2)+cos(pi) + 5", ExpectedResult = 10)]
        [TestCase("cos(pi)/3", ExpectedResult = -0.3333)]
        [TestCase("sin15+sin30", ExpectedResult = -0.3377)]
        public decimal TestTrygonometric(string expression)
        {
            var parser = new Parser(expression);
            var result = parser.Parse();
            return decimal.Round(result, 4);
        }
    }
}