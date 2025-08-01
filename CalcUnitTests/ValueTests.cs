using CalcCore;

namespace CalcUnitTests;

public class ValueTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithZero()
    {
        // Arrange & Act
        var value = new Value();

        // Assert
        Assert.Equal("0", value.Display);
        Assert.Equal(0m, value.Number);
    }

    [Theory]
    [InlineData("123", "123")]
    [InlineData("123.45", "123.45")]
    [InlineData("-123", "-123")]
    [InlineData("-123.45", "-123.45")]
    [InlineData("0", "0")]
    [InlineData("0.0", "0.0")]
    public void StringConstructor_WithValidInput_ShouldSetDisplay(string input, string expected)
    {
        // Arrange & Act
        var value = new Value(input);

        // Assert
        Assert.Equal(expected, value.Display);
    }

    [Theory]
    [InlineData("  123  ", "123")]
    [InlineData("abc123", "123")]
    [InlineData("123abc", "123")]
    [InlineData("1.2.3", "1.23")]
    [InlineData(".123", "0.123")]
    [InlineData(".", "0.")]
    [InlineData("", "0")]
    // [InlineData("--123", "123")]
    [InlineData("-", "0")]
    [InlineData("000123", "123")]
    [InlineData("000", "0")]
    public void StringConstructor_WithInvalidInput_ShouldFixStringInput(string input, string expected)
    {
        // Arrange & Act
        var value = new Value(input);

        // Assert
        Assert.Equal(expected, value.Display);
    }

    [Theory]
    [InlineData(123, "123")]
    [InlineData(123.45, "123.45")]
    [InlineData(-123, "-123")]
    [InlineData(-123.45, "-123.45")]
    [InlineData(0, "0")]
    public void DecimalConstructor_ShouldSetDisplay(decimal input, string expected)
    {
        // Arrange & Act
        var value = new Value(input);

        // Assert
        Assert.Equal(expected, value.Display);
        Assert.Equal(input, value.Number);
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("123.45", 123.45)]
    [InlineData("-123", -123)]
    [InlineData("-123.45", -123.45)]
    [InlineData("0", 0)]
    [InlineData("invalid", 0)]
    [InlineData("", 0)]
    public void Number_Get_ShouldReturnCorrectDecimal(string display, decimal expected)
    {
        // Arrange
        var value = new Value(display);

        // Act
        var result = value.Number;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(123, "123")]
    [InlineData(123.45, "123.45")]
    [InlineData(-123, "-123")]
    [InlineData(0, "0")]
    public void Number_Set_ShouldUpdateDisplay(decimal input, string expected)
    {
        // Arrange
        var value = new Value();

        // Act
        value.Number = input;

        // Assert
        Assert.Equal(expected, value.Display);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("0")]
    [InlineData("-45.67")]
    public void ToString_ShouldReturnDisplay(string display)
    {
        // Arrange
        var value = new Value(display);

        // Act
        var result = value.ToString();

        // Assert
        Assert.Equal(display, result);
    }

    [Theory]
    [InlineData(CalculatorCommand.Plus, "123", "123 +")]
    [InlineData(CalculatorCommand.Minus, "123", "123 -")]
    [InlineData(CalculatorCommand.Multiply, "123", "123 ×")]
    [InlineData(CalculatorCommand.Divide, "123", "123 ÷")]
    [InlineData(CalculatorCommand.None, "123", "")]
    [InlineData(CalculatorCommand.Equal, "123", "")]
    [InlineData(CalculatorCommand.ClearEntry, "123", "")]
    public void ToString_WithCommand_ShouldReturnFormattedString(CalculatorCommand command, string display, string expected)
    {
        // Arrange
        var value = new Value(display);

        // Act
        var result = value.ToString(command);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Display_PropertyChanged_ShouldNotifyNumberChanged()
    {
        // Arrange
        var value = new Value("10");
        var displayChangedCount = 0;
        var numberChangedCount = 0;

        value.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(Value.Display))
                displayChangedCount++;
            if (e.PropertyName == nameof(Value.Number))
                numberChangedCount++;
        };

        // Act
        value.Display = "20";

        // Assert
        Assert.Equal(1, displayChangedCount);
        Assert.Equal(1, numberChangedCount);
    }
}
