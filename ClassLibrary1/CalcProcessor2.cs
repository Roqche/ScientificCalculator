using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class CalcProcessor2
    {
        private readonly List<string> expressions;
        private readonly CalculationType calculationType;

        public CalcProcessor2(List<string> expressions, CalculationType calculationType)
        {
            this.expressions = ReplaceConstants(expressions);
            this.calculationType = calculationType;
        }

        public List<string> Calculate()
        {
            //var listToReturn = 0m;
            var listToReturn = new List<string>();
            if (expressions.Count > 0)
            {
                switch (calculationType)
                {
                    case CalculationType.Exponentation:
                        listToReturn.AddRange(CalculateExponentation(expressions));
                        break;
                    case CalculationType.Root:
                        listToReturn.AddRange(CalculateRoot(expressions));
                        break;
                    case CalculationType.TrygoFunc:
                        listToReturn.AddRange(CalculateTrygonometric(expressions));
                        break;
                    case CalculationType.Logarithm:
                        listToReturn.AddRange(CalculateLogarithm(expressions));
                        break;
                    default:
                        listToReturn.AddRange(BaseCalculation(expressions));
                        break;
                }
            }

            return listToReturn;
        }



        private List<string> CalculateTrygonometric(List<string> expressions)
        {
            var calculatedExpressions = new List<string>();

            foreach (var expression in expressions)
            {
                var angle = double.Parse(Regex.Matches(expression, @"\d+[,.]?\d*")[0].ToString());
                if (expression.StartsWith("sin"))
                {
                    var result = Math.Sin(angle);
                    calculatedExpressions.Add(result.ToString());
                }
                else if (expression.StartsWith("cos"))
                {
                    var result = Math.Cos(angle);
                    calculatedExpressions.Add(result.ToString());
                }
                else if (expression.StartsWith("tan"))
                {
                    var result = Math.Tan(angle);
                    calculatedExpressions.Add(result.ToString());
                }
                else if (expression.StartsWith("sinh"))
                {
                    var result = Math.Sinh(angle);
                    calculatedExpressions.Add(result.ToString());
                }
                else if (expression.StartsWith("cosh"))
                {
                    var result = Math.Cosh(angle);
                    calculatedExpressions.Add(result.ToString());
                }
                else if (expression.StartsWith("tanh"))
                {
                    var result = Math.Tanh(angle);
                    calculatedExpressions.Add(result.ToString());
                }
            }

            return new List<string>(calculatedExpressions);
        }

        private List<string> BaseCalculation(List<string> expressions)
        {
            var calculatedExpressions = new List<string>();

            foreach (var expression in expressions)
            {
                var result = string.Empty;

                try
                {
                    var calculatedExpression = new DataTable().Compute(expression, "");
                    result = calculatedExpression.ToString();
                }
                catch (Exception ex)
                {
                }

                calculatedExpressions.Add(result);
            }

            return new List<string>(calculatedExpressions);
        }

        private List<string> CalculateRoot(List<string> expressions)
        {
            var calculatedExpressions = new List<string>();

            foreach (var expression in expressions)
            {
                var getIndexAndRadicand = Regex.Matches(expression, @"\d+[,.]?\d*");
                var index = 2d;
                var radicand = 0d;
                if (getIndexAndRadicand.Count == 2)
                {
                    index = double.Parse(getIndexAndRadicand[0].ToString());
                    radicand = double.Parse(getIndexAndRadicand[1].ToString());
                }
                else
                {
                    radicand = double.Parse(getIndexAndRadicand[0].ToString());
                }

                if (radicand < 0)
                {
                    throw new OverflowException("Cannot calculate square root from a negative number");
                }

                var result = Math.Pow(radicand, 1.0 / index);
                calculatedExpressions.Add(result.ToString());
            }

            return new List<string>(calculatedExpressions);
        }

        private List<string> CalculateExponentation(List<string> expressions)
        {
            var calculatedExpressions = new List<string>();

            foreach (var expression in expressions)
            {
                var getNumberAndPower = expression.Split('^');
                var number = double.Parse(getNumberAndPower[0].TrimStart('(').TrimEnd(')'));
                var power = double.Parse(getNumberAndPower[1].TrimStart('(').TrimEnd(')'));

                var result = Math.Pow(number, power);
                calculatedExpressions.Add(result.ToString());
            }

            return new List<string>(calculatedExpressions);
        }

        private List<string> CalculateLogarithm(List<string> expressions)
        {
            var calculatedExpressions = new List<string>();

            foreach (var expression in expressions)
            {
                var baseAndExponent = Regex.Matches(expression, @"\d+[,.]?\d*");
                double exponent;
                var result = 0d;

                if (expression.StartsWith("ln"))
                {
                    exponent = double.Parse(baseAndExponent[0].ToString());
                    result = Math.Log(exponent);

                }
                else if (expression.StartsWith("log"))
                {
                    var logarithmBase = 2d;

                    if (baseAndExponent.Count == 2)
                    {
                        logarithmBase = double.Parse(baseAndExponent[0].ToString());
                        exponent = double.Parse(baseAndExponent[1].ToString());
                        result = Math.Log(exponent, logarithmBase);
                    }
                    else
                    {
                        exponent = double.Parse(baseAndExponent[0].ToString());
                        result = Math.Log(exponent, logarithmBase);
                    }
                }


                calculatedExpressions.Add(result.ToString());

            }
            return new List<string>(calculatedExpressions);
        }

        private List<string> ReplaceConstants(List<string> expressions)
        {
            var replacedExpressions = new List<string>();
            foreach (var expression in expressions)
            {
                var properExpression = expression
                                            .Replace("pi", Math.PI.ToString())
                                            .Replace("e", Math.E.ToString());

                replacedExpressions.Add(properExpression);
            }

            return new List<string>(replacedExpressions);
        }
    }
}
