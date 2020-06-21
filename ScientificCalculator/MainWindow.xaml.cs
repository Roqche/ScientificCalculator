using ParserCore;
using System.Windows;
using System.Windows.Controls;

namespace ScientificCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSumButton(object sender, RoutedEventArgs e)
        {
            var parser = new Parser();
            try
            {
                input.Text = parser.Parse(input.Text).ToString();
            }
            catch
            {
                input.Text = "Wrong expression";
            }

            input.CaretIndex = input.Text.Length;
            input.Focus();
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

            input.Text += buttonValue switch
            {
                "π" => "pi",
                "sqrt" => "sqrt()",
                "exponent" => "^",
                "log" => "log()()",
                "ln" => "ln()",
                "sin" => "sin()",
                "cos" => "cos()",
                "tg" => "tg()",
                "sinh" => "sinh()",
                "cosh" => "cosh()",
                "tgh" => "tgh()",
                "division" => "/",
                "multiplication" => "*",
                "substraction" => "-",
                "add"=>"+",
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

                
                "Factorial" => "(!x)",                
                "|x|" => "abs(x)",
                "mod" => "%",

                _ => buttonValue.ToString()
            };

            input.CaretIndex = input.Text.Length;
            input.Focus();
        }
    }
}
