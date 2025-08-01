using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace CalcCore;

/// <summary>
/// Store all values as strings, to allow for arbitrary precision and formatting.
/// </summary>
public class Value : ObservableObject, 
    IEquatable<Value>, ICloneable
{
    private string _display;

    public Value()
        : this(ValueHelpers.DefaultDisplay)
    {}

    public Value(string value)
    {
        _display = ValueHelpers.FixStringInput(value);
    }

    public Value(decimal number)
    {
        _display = ValueHelpers.ToString(number);
    }

    object ICloneable.Clone() => Clone();

    public Value Clone() => new(Display);

    public bool Equals(Value other) 
        => other != null && Display == other.Display;
    
    public override bool Equals(object obj) 
        => obj is Value other && Equals(other);
    
    public override int GetHashCode() 
        => Display.GetHashCode();
    
    public override string ToString() => Display;

    public string Display
    {
        get => _display;
        set
        {
            value = ValueHelpers.FixStringInput(value);

            if (_display != value)
            {
                _display = value;

                OnPropertyChanged(nameof(Display));
                OnPropertyChanged(nameof(Number));
            }
        }
    }

    public decimal Number
    {
        get => ValueHelpers.ToNumber(Display);
        set => Display = ValueHelpers.ToString(value);
    }
}
