using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CalcCore;

public partial class CalculatorViewModel : ObservableObject
{
    [NotifyPropertyChangedFor(nameof(Formula))]
    [ObservableProperty]
    private CalculatorCommand _pushedCommand = CalculatorCommand.None;

    [NotifyPropertyChangedFor(nameof(Formula))]
    [ObservableProperty]
    public Value _pushedValue = new();

    [ObservableProperty]
    private Value _displayValue = new();

    [ObservableProperty]
    private Value _memoryValue = new();

    public string Formula 
        => PushedCommand.ToString(PushedValue);

    [RelayCommand]
    private void Execute(CalculatorCommand command)
    {
        switch (command)
        {
            case CalculatorCommand.Digit0:
                DisplayValue.Add("0");
                break;

            case CalculatorCommand.Digit1:
                DisplayValue.Add("1");
                break;

            case CalculatorCommand.Digit2:
                DisplayValue.Add("2");
                break;

            case CalculatorCommand.Digit3:
                DisplayValue.Add("3");
                break;

            case CalculatorCommand.Digit4:
                DisplayValue.Add("4");
                break;

            case CalculatorCommand.Digit5:
                DisplayValue.Add("5");
                break;

            case CalculatorCommand.Digit6:
                DisplayValue.Add("6");
                break;

            case CalculatorCommand.Digit7:
                DisplayValue.Add("7");
                break;

            case CalculatorCommand.Digit8:
                DisplayValue.Add("8");
                break;

            case CalculatorCommand.Digit9:
                DisplayValue.Add("9");
                break;

            case CalculatorCommand.DecimalPoint:
                DisplayValue.Add(".");
                break;

            case CalculatorCommand.Backspace:
                if (DisplayValue.Display.Length > 0)
                {
                    DisplayValue.Display = DisplayValue.Display[..(DisplayValue.Display.Length - 1)];
                }
                break;

            case CalculatorCommand.ClearAll:
            case CalculatorCommand.ClearEntry:
                if (command == CalculatorCommand.ClearAll || DisplayValue == ValueExtensions.DefaultDisplay)
                {
                    PushedValue.Display = ValueExtensions.DefaultDisplay;
                    PushedCommand = CalculatorCommand.None;
                }
                
                DisplayValue.Display = ValueExtensions.DefaultDisplay;
                break;

            case CalculatorCommand.Plus:
            case CalculatorCommand.Minus:
            case CalculatorCommand.Multiply:
            case CalculatorCommand.Divide:
                PushedValue.Display = DisplayValue.Display;
                PushedCommand = command;
                DisplayValue.Display = ValueExtensions.DefaultDisplay;
                break;

            case CalculatorCommand.Equal:
                if (PushedCommand.Compute(PushedValue, DisplayValue) is Value result)
                {
                    PushedValue.Display = ValueExtensions.DefaultDisplay;
                    PushedCommand = CalculatorCommand.None;
                    DisplayValue.Display = result;
                }
                break;

            case CalculatorCommand.PlusMinus:
                if (DisplayValue.Display.StartsWith('-'))
                {
                    DisplayValue.Display = DisplayValue.Display[1..];
                }
                else
                {
                    DisplayValue.Display = $"-{DisplayValue}";
                }
                break;

            case CalculatorCommand.Percent:
                DisplayValue.Number *= 0.01m;
                break;

            case CalculatorCommand.SquareRoot:
                DisplayValue.Number = DisplayValue > 0m
                    ? (decimal)Math.Sqrt(DisplayValue)
                    : 0m;
                break;

            case CalculatorCommand.Square:
                DisplayValue.Number *= DisplayValue;
                break;

            case CalculatorCommand.Inverse:
                DisplayValue.Number = DisplayValue != 0m ? 1m / DisplayValue : 0m;
                break;

            case CalculatorCommand.ConstantPi:
                DisplayValue.Number = (decimal)Math.PI;
                break;

            case CalculatorCommand.MemoryClear:
                MemoryValue.Display = ValueExtensions.DefaultDisplay;
                break;

            case CalculatorCommand.MemoryRecall:
                DisplayValue = MemoryValue.Clone();
                break;

            case CalculatorCommand.MemoryAdd:
                MemoryValue.Number += DisplayValue;
                break;

            case CalculatorCommand.MemorySubtract:
                MemoryValue.Number -= DisplayValue;
                break;

            case CalculatorCommand.MemoryStore:
                MemoryValue = DisplayValue.Clone();
                break;
        }
    }
}
