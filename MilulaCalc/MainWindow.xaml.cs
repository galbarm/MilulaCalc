using System;
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

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RemoveRedundantChars();

            try
            {
                var expression = new Expression(_ExpressionBox.Text);
                object resultObj = expression.Evaluate();
                double result = Convert.ToDouble(resultObj);
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
            string original = _ExpressionBox.Text;

            if (!String.IsNullOrEmpty(original))
            {
                original = original.Replace(",", "");
                original = original.Replace("$", "");
                original = original.Replace("₪", "");
                original = original.Replace(" ", "");
                _ExpressionBox.Text = original;
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
