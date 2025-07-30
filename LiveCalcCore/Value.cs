using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace CalcCore;

/// <summary>
/// Store all values as strings, to allow for arbitrary precision and formatting.
/// </summary>
public class Value : ObservableObject, IEquatable<Value>, ICloneable
{
    private string display;

    public Value()
        : this(ValueHelpers.DefaultDisplay)
    {}

    public Value(string value)
    {
        this.display = ValueHelpers.FixStringInput(value);
    }

    public Value(decimal number)
    {
        this.display = ValueHelpers.ToString(number);
    }

    object ICloneable.Clone() => this.Clone();
    public Value Clone() => new(this.Display);
    public bool Equals(Value other) => other != null && this.Display == other.Display;
    public override bool Equals(object obj) => obj is Value other && this.Equals(other);
    public override int GetHashCode() => this.Display.GetHashCode();
    public override string ToString() => this.Display;

    public string Display
    {
        get => this.display;
        set
        {
            value = ValueHelpers.FixStringInput(value);
            if (this.display != value)
            {
                this.display = value;
                this.OnPropertyChanged(nameof(this.Display));
                this.OnPropertyChanged(nameof(this.Number));
            }
        }
    }

    public decimal Number
    {
        get => ValueHelpers.ToNumber(this.Display);
        set => this.Display = ValueHelpers.ToString(value);
    }
}
