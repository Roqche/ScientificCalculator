using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Calculator
{
    public class CalcProcessor2
    {
        private readonly List<string> expressions;

        public CalcProcessor2(List<string> expressions)
        {
            this.expressions = expressions;
        }

        public List<string> Calculate()
        {
            //var listToReturn = 0m;
            var listToReturn = new List<string>();
            if (expressions.Count > 0)
            {
                foreach (var expression in expressions)
                {
                    var result = string.Empty;

                    //if (Regex.IsMatch(expression, @"[A-Za-z]+"))
                    //{
                    //    result = ConvertFromWord(expression);
                    //}
                    //else
                    //{
                    try
                    {
                        var calculatedExpression = new DataTable().Compute(expression, "");
                        result = calculatedExpression.ToString();
                    }
                    catch (Exception ex)
                    {
                        //result = decimal.Round((decimal)(Math.Sin(15) + Math.Sin(30)), 3);


                    }
                    //}

                    listToReturn.Add(result);
                }
            }
            return listToReturn;
        }
    }
}
