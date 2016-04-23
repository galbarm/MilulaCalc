using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Expression = NCalc.Expression;


namespace MilulaCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _ExpressionBox.Focus();

            new HotKey(Key.Space, KeyModifier.Alt, OnHotKeyHandler);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RemoveRedundantChars();
            
            try
            {
                var expression = new Expression(_ExpressionBox.Text);
                object resultObj = expression.Evaluate();
                double result = Convert.ToDouble(resultObj);
                result = Math.Round(result, 2, MidpointRounding.AwayFromZero);
                var str = result.ToString();
                
                if (result > 0 & result < 1)
                {
                    str += " (" + ((decimal)result).ToString("P") + ")";
                }


                _ResultBox.Text = str;
            }
            catch (Exception)
            {
            }
        }

        private void RemoveRedundantChars()
        {
            string originalText = _ExpressionBox.Text;

            if (!String.IsNullOrEmpty(originalText))
            {
                int originalIndex = _ExpressionBox.CaretIndex;

                var newText = originalText.Replace(",", "");
                newText = newText.Replace("$", "");
                newText = newText.Replace("₪", "");
                newText = newText.Replace(" ", "");
                _ExpressionBox.Text = newText;

                _ExpressionBox.CaretIndex = originalIndex - (originalText.Length - newText.Length);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string str = _ResultBox.Text;
                int index = _ResultBox.Text.IndexOf("(");

                if (index >= 0)
                {
                    str = str.Substring(0, index);
                }

                Clipboard.SetText(str);

                WindowState = WindowState.Minimized;
            }
        }

        private void OnHotKeyHandler(HotKey hotKey)
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
            InputLanguageManager.SetInputLanguage(_ExpressionBox, CultureInfo.CreateSpecificCulture("en"));
            _ExpressionBox.SelectAll();
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Visibility = Visibility.Hidden;
            }
        }
    }
}
