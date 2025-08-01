using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;

namespace CalcCore;

/// <summary>
///  Store all values as strings, to allow for arbitrary precision and formatting.
/// </summary>
public partial class Value : ObservableObject, ICloneable
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
        get => this;
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

    public void Add(string v)
        => Display += v.FixStringInput();

    public Value Clone()
        => new(Display);

    object ICloneable.Clone() => new Value(Display);

    public static implicit operator string(Value value) 
        => value.Display;

    public static implicit operator decimal(Value value)
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

    public static implicit operator double(Value value)
        => (double)(decimal)value;
}
