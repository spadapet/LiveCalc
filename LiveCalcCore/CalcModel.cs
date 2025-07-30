using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;

namespace CalcCore;

public partial class CalcModel : ObservableObject, IDisposable
{
    private Value currentValue = new();
    private Value memoryValue = new();
    private Value pushedValue = new();
    private Command pushedCommand = Command.None;

    public CalcModel()
    {
        this.currentValue.PropertyChanged += OnValuePropertyChanged;
        this.memoryValue.PropertyChanged += OnValuePropertyChanged;
        this.pushedValue.PropertyChanged += OnValuePropertyChanged;
    }

    public void Dispose()
    {
        this.currentValue.PropertyChanged -= OnValuePropertyChanged;
        this.memoryValue.PropertyChanged -= OnValuePropertyChanged;
        this.pushedValue.PropertyChanged -= OnValuePropertyChanged;
    }

    public string Display
    {
        get => this.currentValue.Display;
        set => this.currentValue.Display = value;
    }

    public string MemoryDisplay
    {
        get => this.memoryValue.Display;
        set => this.memoryValue.Display = value;
    }

    public decimal Number
    {
        get => this.currentValue.Number;
        set => this.currentValue.Number = value;
    }

    public decimal MemoryNumber
    {
        get => this.memoryValue.Number;
        set => this.memoryValue.Number = value;
    }

    public string Formula => ValueHelpers.ToString(this.pushedValue, this.pushedCommand);

    [RelayCommand]
    private void Execute(Command command)
    {
        switch (command)
        {
            case Command.Type0:
                this.Display += "0";
                break;

            case Command.Type1:
                this.Display += "1";
                break;

            case Command.Type2:
                this.Display += "2";
                break;

            case Command.Type3:
                this.Display += "3";
                break;

            case Command.Type4:
                this.Display += "4";
                break;

            case Command.Type5:
                this.Display += "5";
                break;

            case Command.Type6:
                this.Display += "6";
                break;

            case Command.Type7:
                this.Display += "7";
                break;

            case Command.Type8:
                this.Display += "8";
                break;

            case Command.Type9:
                this.Display += "9";
                break;

            case Command.DecimalPoint:
                this.Display += ".";
                break;

            case Command.Backspace:
                if (this.Display.Length > 0)
                {
                    this.Display = this.Display.Remove(this.Display.Length - 1);
                }
                break;

            case Command.ClearAll:
            case Command.ClearEntry:
                if (command == Command.ClearAll || this.Display == ValueHelpers.DefaultDisplay)
                {
                    this.pushedValue.Display = ValueHelpers.DefaultDisplay;
                    this.pushedCommand = Command.None;
                    this.OnPropertyChanged(nameof(this.Formula));
                }
                
                this.Display = ValueHelpers.DefaultDisplay;
                break;

            case Command.Plus:
            case Command.Minus:
            case Command.Multiply:
            case Command.Divide:
                this.pushedValue.Display = this.currentValue.Display;
                this.pushedCommand = command;
                this.OnPropertyChanged(nameof(this.Formula));
                this.Display = ValueHelpers.DefaultDisplay;
                break;

            case Command.Equal:
                if (ValueHelpers.Compute(this.pushedValue, this.pushedCommand, this.currentValue) is Value result)
                {
                    this.pushedValue.Display = ValueHelpers.DefaultDisplay;
                    this.pushedCommand = Command.None;
                    this.OnPropertyChanged(nameof(this.Formula));
                    this.Display = result.Display;
                }
                break;

            case Command.PlusMinus:
                if (this.Display.StartsWith('-'))
                {
                    this.Display = this.Display.Substring(1);
                }
                else
                {
                    this.Display = $"-{this.Display}";
                }
                break;

            case Command.Percent:
                this.Number *= 0.01m;
                break;

            case Command.SquareRoot:
                this.Number = this.Number > 0m
                    ? (decimal)Math.Sqrt((double)this.Number)
                    : 0m;
                break;

            case Command.Square:
                this.Number *= this.Number;
                break;

            case Command.Inverse:
                this.Number = this.Number != 0m ? 1m / this.Number : 0m;
                break;

            case Command.ConstantPi:
                this.Number = (decimal)Math.PI;
                break;

            case Command.MemoryClear:
                this.MemoryDisplay = ValueHelpers.DefaultDisplay;
                break;

            case Command.MemoryRecall:
                this.Display = this.MemoryDisplay;
                break;

            case Command.MemoryAdd:
                this.MemoryNumber += this.Number;
                break;

            case Command.MemorySubtract:
                this.MemoryNumber -= this.Number;
                break;

            case Command.MemoryStore:
                this.MemoryDisplay = this.Display;
                break;
        }
    }

    private void OnValuePropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        if (sender == this.currentValue)
        {
            this.OnPropertyChanged(nameof(Display));
            this.OnPropertyChanged(nameof(Number));
        }
        else if (sender == this.memoryValue)
        {
            this.OnPropertyChanged(nameof(MemoryDisplay));
            this.OnPropertyChanged(nameof(MemoryNumber));
        }
        else if (sender == this.pushedValue)
        {
            this.OnPropertyChanged(nameof(Formula));
        }
    }
}
