using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace CalcCore;

/// <summary>
///  Store all values as strings, to allow for arbitrary precision and formatting.
/// </summary>
public partial class Value : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Number))]
    private string _display;

    public Value()
        : this(ValueExtensions.DefaultDisplay)
    {}

    public Value(string value)
    {
        _display = value.FixStringInput();
    }

    public Value(decimal number)
    {
        _display = number.ToString();
    }

    partial void OnDisplayChanged(string? oldValue, string newValue)
    {
        _display = newValue.FixStringInput();
    }

    public decimal Number
    {
        get => ToNumber(Display);
        set => Display = value.ToString();
    }

    public override string ToString() 
        => Display;

    public string ToString(CalculatorCommand command)
        => command switch
        {
            CalculatorCommand.Plus => $"{this} +",
            CalculatorCommand.Minus => $"{this} -",
            CalculatorCommand.Multiply => $"{this} ×",
            CalculatorCommand.Divide => $"{this} ÷",
            _ => string.Empty
        };

    private static decimal ToNumber(string value)
    {
        if (decimal.TryParse(
            value,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out decimal result))
        {
            return result;
        }

        return 0m;
    }
}
