using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace CalcCore;

public record PropertyCascadeMessage(object Source, string PropertyName, object? NewValue);

public partial class CalculatorViewModel : ObservableObject
{
    private readonly Value _currentValue = new();
    private readonly Value _memoryValue = new();
    private readonly Value _pushedValue = new();

    private CalculatorCommand _pushedCommand = CalculatorCommand.None;

    public CalculatorViewModel()
    {
        _currentValue.PropertyChanged += OnValuePropertyChanged;
        _memoryValue.PropertyChanged += OnValuePropertyChanged;
        _pushedValue.PropertyChanged += OnValuePropertyChanged;
    }

    public string Display
    {
        get => _currentValue.Display;
        set => _currentValue.Display = value;
    }

    public string MemoryDisplay
    {
        get => _memoryValue.Display;
        set => _memoryValue.Display = value;
    }

    public decimal Number
    {
        get => _currentValue.Number;
        set => _currentValue.Number = value;
    }

    public decimal MemoryNumber
    {
        get => _memoryValue.Number;
        set => _memoryValue.Number = value;
    }

    public string Formula 
        => _pushedCommand.ToString(_pushedValue);

    [RelayCommand]
    private void Execute(CalculatorCommand command)
    {
        switch (command)
        {
            case CalculatorCommand.Digit0:
                Display += "0";
                break;

            case CalculatorCommand.Digit1:
                Display += "1";
                break;

            case CalculatorCommand.Digit2:
                Display += "2";
                break;

            case CalculatorCommand.Digit3:
                Display += "3";
                break;

            case CalculatorCommand.Digit4:
                Display += "4";
                break;

            case CalculatorCommand.Digit5:
                Display += "5";
                break;

            case CalculatorCommand.Digit6:
                Display += "6";
                break;

            case CalculatorCommand.Digit7:
                Display += "7";
                break;

            case CalculatorCommand.Digit8:
                Display += "8";
                break;

            case CalculatorCommand.Digit9:
                Display += "9";
                break;

            case CalculatorCommand.DecimalPoint:
                Display += ".";
                break;

            case CalculatorCommand.Backspace:
                if (Display.Length > 0)
                {
                    Display = Display[..(Display.Length - 1)];
                }

                break;

            case CalculatorCommand.ClearAll:
            case CalculatorCommand.ClearEntry:
                if (command == CalculatorCommand.ClearAll || Display == ValueExtensions.DefaultDisplay)
                {
                    _pushedValue.Display = ValueExtensions.DefaultDisplay;
                    _pushedCommand = CalculatorCommand.None;
                    OnPropertyChanged(nameof(Formula));
                }
                
                Display = ValueExtensions.DefaultDisplay;
                break;

            case CalculatorCommand.Plus:
            case CalculatorCommand.Minus:
            case CalculatorCommand.Multiply:
            case CalculatorCommand.Divide:
                _pushedValue.Display = _currentValue.Display;
                _pushedCommand = command;
                OnPropertyChanged(nameof(Formula));
                Display = ValueExtensions.DefaultDisplay;

                break;

            case CalculatorCommand.Equal:
                if (_pushedCommand.Compute(_pushedValue, _currentValue) is Value result)
                {
                    _pushedValue.Display = ValueExtensions.DefaultDisplay;
                    _pushedCommand = CalculatorCommand.None;
                    OnPropertyChanged(nameof(Formula));
                    Display = result.Display;
                }

                break;

            case CalculatorCommand.PlusMinus:
                if (Display.StartsWith('-'))
                {
                    Display = Display[1..];
                }
                else
                {
                    Display = $"-{Display}";
                }
                break;

            case CalculatorCommand.Percent:
                Number *= 0.01m;
                break;

            case CalculatorCommand.SquareRoot:
                Number = Number > 0m
                    ? (decimal)Math.Sqrt((double)Number)
                    : 0m;
                break;

            case CalculatorCommand.Square:
                Number *= Number;
                break;

            case CalculatorCommand.Inverse:
                Number = Number != 0m ? 1m / Number : 0m;
                break;

            case CalculatorCommand.ConstantPi:
                Number = (decimal)Math.PI;
                break;

            case CalculatorCommand.MemoryClear:
                MemoryDisplay = ValueExtensions.DefaultDisplay;
                break;

            case CalculatorCommand.MemoryRecall:
                Display = MemoryDisplay;
                break;

            case CalculatorCommand.MemoryAdd:
                MemoryNumber += Number;
                break;

            case CalculatorCommand.MemorySubtract:
                MemoryNumber -= Number;
                break;

            case CalculatorCommand.MemoryStore:
                MemoryDisplay = Display;
                break;
        }
    }

    private void OnValuePropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (sender == _currentValue)
        {
            OnPropertyChanged(nameof(Display));
            OnPropertyChanged(nameof(Number));
        }
        else if (sender == _memoryValue)
        {
            OnPropertyChanged(nameof(MemoryDisplay));
            OnPropertyChanged(nameof(MemoryNumber));
        }
        else if (sender == _pushedValue)
        {
            OnPropertyChanged(nameof(Formula));
        }
    }
}
