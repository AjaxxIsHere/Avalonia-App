using Avalonia.Controls;

namespace MyApp;

public partial class MainWindow : Window
{
    private string _currentInput = "";
    private string _operator = "";
    private double? _firstOperand = null;
    private bool _resetInput = false;

    public MainWindow()
    {
        InitializeComponent();
        Display.Text = "0";
    }

    private void Digit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            string digit = btn.Content?.ToString() ?? "";
            if (_resetInput)
            {
                _currentInput = "";
                _resetInput = false;
            }
            if (digit == ".")
            {
                if (_currentInput.Contains(".")) return;
                if (string.IsNullOrEmpty(_currentInput))
                    _currentInput = "0";
            }
            _currentInput += digit;
            Display.Text = _currentInput;
        }
    }

    private void Operator_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                _firstOperand = value;
                _operator = btn.Content?.ToString() ?? "";
                _resetInput = true;
                // Update the display to show the first operand
                Display.Text = _firstOperand?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "0";
            }
        }
    }

    private void Equals_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_firstOperand.HasValue && !string.IsNullOrEmpty(_operator) && double.TryParse(_currentInput, out double secondOperand))
        {
            // divide by zero check
            if (_operator == "/" && secondOperand == 0)
            {
                Display.Text = ">:( Error";
                _currentInput = "0";
                _firstOperand = null;
                _operator = "";
                _resetInput = true;
                return;
            }

            double result = 0;
            switch (_operator)
            {
                case "+": result = _firstOperand.Value + secondOperand; break;
                case "-": result = _firstOperand.Value - secondOperand; break;
                case "*": result = _firstOperand.Value * secondOperand; break;
                case "/": result = _firstOperand.Value / secondOperand; break;
            }

            _currentInput = result.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Display.Text = _currentInput;
            _firstOperand = null;
            _operator = "";
            _resetInput = true;
        }
    }

    private void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(_currentInput))
        {
            _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            Display.Text = string.IsNullOrEmpty(_currentInput) ? "0" : _currentInput;
        }
    }

    private void ClearAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _currentInput = "";
        _firstOperand = null;
        _operator = "";
        _resetInput = false;
        Display.Text = "0";
    }
}