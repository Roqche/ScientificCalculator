using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
                    try
                    {
                        var calculatedExpression = new DataTable().Compute(expression, "");
                        result = decimal.Parse(calculatedExpression.ToString());
                    }
                    catch (Exception ex)
                    {
                        //result = decimal.Round((decimal)(Math.Sin(15) + Math.Sin(30)), 3);
                        

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
    }
}
