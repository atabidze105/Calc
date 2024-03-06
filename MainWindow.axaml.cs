using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace TabCalc
{
    public partial class MainWindow : Window
    {
        string _numberNow = ""; //поле дл€ второго операнда
        string _numberPr = ""; //ѕоле дл€ первого операнда
        public MainWindow()
        {
            InitializeComponent();
        }

        public void inputNumber(object? sender, RoutedEventArgs args) //¬вод и удаление последнего символа
        {
            var button = (sender as Button)!;
            text.Text = text.Text == "0" || text.Text == _numberPr ? "" : text.Text;
            switch (button.Name)
            {
                case "But_1":
                    text.Text += "1";
                    break;
                case "But_2":
                    text.Text += "2";
                    break;
                case "But_3":
                    text.Text += "3";
                    break;
                case "But_4":
                    text.Text += "4";
                    break;
                case "But_5":
                    text.Text += "5";
                    break;
                case "But_6":
                    text.Text += "6";
                    break;
                case "But_7":
                    text.Text += "7";
                    break;
                case "But_8":
                    text.Text += "8";
                    break;
                case "But_9":
                    text.Text += "9";
                    break;
                case "But_0":
                    text.Text += "0";
                    break;
                case "But_pi":
                    text.Text = Convert.ToString(Math.PI);
                    break;
                case "But_euler":
                    text.Text = Convert.ToString(Math.E);
                    break;
                case "But_back"://”даление последнего символа
                    string numm = text.Text;
                    if (text.Text != "")
                    {
                        numm = numm.Remove(numm.Length - 1);
                        text.Text = numm;
                    }
                    break;
            }
            text.Text = text.Text == "" ? "0" : text.Text; //если строка пуста, отображаетс€ ноль
        }

        public void inputOther(object? sender, RoutedEventArgs args) //—тирание строк и кнопка зап€той
        {
            var button = (sender as Button)!;
            int dotCount = 0; //счетчик точек
            switch (button.Name)
            {
                case "But_CE":
                    {
                        text.Text = "0";
                    }
                    break;
                case "But_Clr":
                    {
                        text.Text = "0";
                        previously.Text = "";
                    }
                    break;
                case "But_fr": //»спользование этой кнопки удалит зап€тую или добавит ее в конце строки //≈сли добавить зап€тую после нул€, а затем добавить другие цифры и еще раз использовать кнопку (уже дл€ удалени€ зап€той), 0 в начале сохранитс€ (врочем, на корректность счета это не вли€ет)
                    {
                        if (text.Text.Contains(',') == true)
                        {
                            text.Text = text.Text.Replace(",", "");
                            dotCount++;
                        }
                        if (dotCount == 0) //ƒобавление зап€той
                        {
                            text.Text += text.Text == "" ? "0," : ","; //т.к. в большой строке "0" == "", исп-е кнопки заменит ее на "0,"
                        }
                    }
                    break;
            }
        }

        public void operation(object? sender, RoutedEventArgs args) //основные математические операции и равно
        {
            if ( previously.Text == "")
            {
                var button = (sender as Button)!;
                _numberPr = text.Text;
                switch (button.Name) //¬ычисллени€ происход€т в методе equal(), здесь только мен€етс€ мала€ строка, конечный символ которой вли€ет на выполнение операции 
                {
                    case "But_plus":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")+" : text.Text + "+";
                        }
                        break;
                    case "But_min":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")-" : text.Text + "-";
                        }
                        break;
                    case "But_mult":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")*" : text.Text + "*";
                        }
                        break;
                    case "But_slash":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")/" : text.Text + "/";
                        }
                        break;
                    case "But_equal": //¬ылет при нажатии на "=" при отсутствии символов в малой строке
                        {
                            _numberNow = _numberPr;
                            equal();
                        }
                        break;
                }
            }
            else //≈сли в малой строке есть символы, то нажатие на любую из кнопок операций вызовет метод equal()
            {
                var button = (sender as Button)!;
                switch (button.Name)
                {
                    default:
                        {
                            _numberNow = text.Text;
                            equal();
                        }
                        break;
                }
            }
        }

        private void symChange(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //смена знака перед числом
        {
            text.Text = text.Text == "0" ? text.Text : (text.Text.StartsWith('-') ? text.Text.Remove(0, 1) : '-' + text.Text);
        }

        private void persent(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_numberPr == "")
            {
                text.Text = Convert.ToString(Convert.ToDouble(text.Text)/1000); //ѕри нажатии на "%" с пустой малой строкой выполн€етс€ некорректное вычисление одного процента (одна тыс€чна€ вместо одной сотой)
            }
            else
            {
                if (previously.Text != "")
                {
                    double numFull = Convert.ToDouble(text.Text);
                    _numberNow = Convert.ToString(numFull / 100);
                    text.Text = Convert.ToString(_numberNow);
                }
                else
                {
                    double numFull = Convert.ToDouble(_numberPr);
                    _numberNow = Convert.ToString(numFull / 100);
                    text.Text = Convert.ToString(_numberNow);
                }
            }
        }

        private double factorial(double number) //метод дл€ факториалап
        {
            double numReturn = 1;
            if (number > 0)
            {
                for (int i = 1; i < number; i++) //при использовании факториал не дойдет до корректного значени€ из-за потери последнего множител€
                {
                    numReturn *= i;
                }
                return numReturn;
            }
            else if (number < 0) 
            {
                for (int i = -1; i >= number; i--) 
                {
                    numReturn *= i;
                }
                return numReturn;
            }
            else
            {
                return 1;
            }
        }

        public void function(object? sender, RoutedEventArgs args) //‘ункции примен€ютс€ только к тому, что находитс€ в большой строке
        {          
            var button = (sender as Button)!;
            switch (button.Name)
            {
                case "But_drob":
                    previously.Text = "1/(" + text.Text + ")";
                    text.Text = Convert.ToString(1 / Convert.ToDouble(text.Text));
                    break;
                case "But_Sq":
                    previously.Text = "sqr(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Pow(Convert.ToDouble(text.Text), 2));
                    break;
                case "But_Sqrt":
                    previously.Text = "sqrt(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Sqrt(Convert.ToDouble(text.Text)));
                    break;
                case "But_sin": //¬се следующие триготометрические ф-ии актуальны только дл€ градусов
                    previously.Text = "sin(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Sin(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_sinh":
                    previously.Text = "sinh(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Sinh(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_cos":
                    previously.Text = "cos(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Cos(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_cosh":
                    previously.Text = "cosh(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Cosh(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_tan":                
                    previously.Text = "tan(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Tan(Math.PI * Convert.ToDouble(text.Text) / 180));                    
                    break;                 
                case "But_tanh":                   
                    previously.Text = "tanh(" + text.Text + ")";text.Text = Convert.ToString(Math.Tanh(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_ln":
                    previously.Text = "ln(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Log(Convert.ToDouble(text.Text)));
                    break;
                case "But_log":
                    previously.Text = "log(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Log10(Convert.ToDouble(text.Text)));
                    break;
                case "But_fact":
                    previously.Text = "fact(" + text.Text + ")";
                    text.Text = Convert.ToString(factorial(Convert.ToDouble(text.Text)));
                    break; 
            }
        }

        private void equal()
        {
            double num1 = Convert.ToDouble(_numberPr);
            double num2 = Convert.ToDouble(_numberNow);
            switch (previously.Text.Substring(previously.Text.Length - 1)) //¬ зависимости от последнего символа в малой строке будет выполнена одна из следующих операций
            {
                case "+":
                    text.Text = Convert.ToString(num1 + num2);
                    break;
                case "-":
                    text.Text = Convert.ToString(num1 - num2);
                    break;
                case "*":
                    text.Text = Convert.ToString(num1 * num2);
                    break;
                case "/":
                    text.Text = Convert.ToString(num1 / num2);
                    break;
            }
            previously.Text = "";
            _numberPr = "";
        }
    }

}