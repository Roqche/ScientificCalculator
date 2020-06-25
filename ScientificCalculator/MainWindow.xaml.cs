using ParserCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScientificCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<string> calculationHistory = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Calculate()
        {
            var equation = input.Text + " = ";
            var parser = new Parser(input.Text);
            var result = string.Empty;

            try
            {
                var task = Task.Run(() =>
                {
                    result = parser.Parse().ToString();
                });

                if (!task.Wait(TimeSpan.FromSeconds(3)))
                {
                    throw new Exception("Timed out");
                }
            }
            catch
            {
                result = "Wrong expression";
            }

            equation += result;
            RedrawCalculationHistory(equation);         

            input.Text = result;
            input.CaretIndex = result.Length;
            input.Focus();
        }

        private void OnSumButton(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void OnClearButton(object sender, RoutedEventArgs e)
        {
            input.Text = string.Empty;
            input.CaretIndex = 0;
            input.Focus();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var buttonValue = ((Button)sender).Name;

            var textToInsert = buttonValue switch
            {
                "π" => "pi",
                "exponent" => "^",
                "division" => "/",
                "multiplication" => "*",
                "substraction" => "-",
                "add" => "+",
                "_1" => "1",
                "_2" => "2",
                "_3" => "3",
                "_4" => "4",
                "_5" => "5",
                "_6" => "6",
                "_7" => "7",
                "_8" => "8",
                "_9" => "9",
                "_0" => "0",
                "dot" => ".",
                _ => buttonValue.ToString()
            };

            var newCaretIndex = input.CaretIndex + textToInsert.Length;

            input.Text = input.Text.Insert(input.CaretIndex, textToInsert);

            input.CaretIndex = newCaretIndex;
            input.Focus();
        }

        private void OnButtonWithBracketsClick(object sender, RoutedEventArgs e)
        {
            var buttonValue = ((Button)sender).Name;

            var textToInsert = buttonValue switch
            {
                "sqrt" => "sqrt()",
                "log" => "log()()",
                "ln" => "ln()",
                "sin" => "sin()",
                "cos" => "cos()",
                "tg" => "tg()",
                "sinh" => "sinh()",
                "cosh" => "cosh()",
                "tgh" => "tgh()",
                _ => buttonValue.ToString()
            };

            int newCaretIndex;
            if (buttonValue == "log")
            {
                newCaretIndex = input.CaretIndex + textToInsert.Length - 3;
            }
            else
            {
                newCaretIndex = input.CaretIndex + textToInsert.Length - 1;
            }

            input.Text = input.Text.Insert(input.CaretIndex, textToInsert);

            input.CaretIndex = newCaretIndex;
            input.Focus();
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Calculate();
            }
        }

        private void RedrawCalculationHistory(string equation)
        {
            lock (calculationHistory)
            {
                calculationHistory.Add(equation);

                if (calculationHistory.Count > 5)
                {
                    calculationHistory.RemoveAt(0);
                }

                CalculationHistory.Text = string.Join("\n", calculationHistory);
            }
        }
    }
}
