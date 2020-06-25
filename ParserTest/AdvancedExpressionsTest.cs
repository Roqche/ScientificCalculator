using NUnit.Framework;
using ParserCore;

namespace ParserTest
{
    public  class AdvancedExpressionsTest
    {
        [TestCase("-2-4-6+5+3", ExpectedResult = -4)]
        [TestCase("-2+4-6+5-3", ExpectedResult = -2)]
        [TestCase("((2+3) - (2*2)) + (1+1)", ExpectedResult = 3)]
        [TestCase("15+20*35*15*15/12-140+150+(2*(15-16)/2)", ExpectedResult = 13149)]
        [TestCase("15+20*35*15*15/12-140+150", ExpectedResult = 13150)]
        [TestCase("((2+3) - (2*2)) + (1+1)", ExpectedResult = 3)]
        [TestCase("e^ln(e)", ExpectedResult = 2.7183)]
        [TestCase("cos(cos(pi))", ExpectedResult = 0.5403)]
        [TestCase("cos(cos(pi)+sin(pi))", ExpectedResult = 0.5403)]
        [TestCase("cos(log(10)(10))", ExpectedResult = 0.5403)]
        [TestCase("sin(cos(tg(15+30)))", ExpectedResult = 0.8415)]
        [TestCase("7*sqrt(4*4)+3", ExpectedResult = 31)]
        public decimal TestAdvancedExpression(string expression)
        {
            var parser = new Parser(expression);
            var result = parser.Parse();
            return decimal.Round(result, 4);
        }
    }
}
