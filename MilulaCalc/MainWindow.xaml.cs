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
            try
            {
                var expression = new Expression(_ExpressionBox.Text);
                _ResultBox.Text = expression.Evaluate().ToString();
            }
            catch (Exception)
            {

            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Clipboard.SetText(_ResultBox.Text);
            }
        }

        private void OnHotKeyHandler(HotKey hotKey)
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
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
