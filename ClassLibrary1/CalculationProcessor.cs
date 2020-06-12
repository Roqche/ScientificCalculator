using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class CalculationProcessor
    {
        private readonly Dictionary<int, List<string>> sequenceOfActions;

        public CalculationProcessor(Dictionary<int, List<string>> sequenceOfActions)
        {
            this.sequenceOfActions = sequenceOfActions;
        }

        public decimal Calculate()
        {
            var lastResult = 0m;

            while (sequenceOfActions.Count > 0)
            {
                var key = sequenceOfActions.Keys.Last();
                var expressions = sequenceOfActions[key];
                foreach (var expression in expressions)
                {
                    var result = 0m;

                    if (Regex.IsMatch(expression, @"[A-Za-z]+"))
                    {
                        result = ConvertFromWord(expression);
                    }
                    else
                    {
                        try
                        {
                            var calculatedExpression = new DataTable().Compute(expression, "");
                            result = decimal.Parse(calculatedExpression.ToString());
                        }
                        catch (Exception ex)
                        {
                            //result = decimal.Round((decimal)(Math.Sin(15) + Math.Sin(30)), 3);


                        }
                    }

                    lastResult = result;

                    for (int i = 0; i < key; i++)
                    {
                        sequenceOfActions[i] = sequenceOfActions[i].Select(s => s.Replace(expression, result.ToString())).ToList();
                    }
                }

                sequenceOfActions.Remove(key);

            }
            return lastResult;
        }

        private decimal ConvertFromWord(string expression)
        {
            var convertedResult = 0d;

            if (expression.Contains("sin"))
            {
                if (double.TryParse(expression.Remove(expression.IndexOf("sin"), 3), out double result))
                {
                    convertedResult = Math.Sin(result);
                }
            }

            if (expression.Contains("tg"))
            {
                var stringToParse = expression.Remove(expression.IndexOf("tg"), 2);
                if (double.TryParse(stringToParse, out double result))
                {
                    convertedResult = Math.Tan(result);
                }
            }

            if (expression.Contains("cos"))
            {
                if (double.TryParse(expression.Remove(expression.IndexOf("cos"), 3), out double result))
                {
                    convertedResult = Math.Cos(result);
                }
            }

            return (decimal)convertedResult;
        }
    }
}
