using System.Text;

namespace CalcCore;

/// <summary>
///  Provides extension methods for value formatting, command string conversion, and calculation logic.
/// </summary>
internal static class ValueExtensions
{
    /// <summary>
    ///  The default display value for numbers.
    /// </summary>
    public const string DefaultDisplay = "0";

    /// <summary>
    ///  Returns a valid string representation of a number from any input, ensuring proper formatting.
    /// </summary>
    /// <param name="value">The input string to fix.</param>
    /// <returns>A valid number string.</returns>
    public static string FixStringInput(this string value)
    {
        value = value?.Trim() ?? string.Empty;
        StringBuilder sb = new(value.Length);

        bool sawNegative = false;
        bool sawNumber = false;
        bool sawDot = false;

        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];

            switch (c)
            {
                case '0':
                    if (sawNumber)
                    {
                        sb.Append(c);
                    }
                    break;

                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    sawNumber = true;
                    sb.Append(c);
                    break;

                case '.':
                    if (!sawDot)
                    {
                        if (!sawNumber)
                        {
                            sawNumber = true;
                            sb.Append('0');
                        }

                        sawDot = true;
                        sb.Append(c);
                    }
                    break;

                case '-':
                    if (i == 0)
                    {
                        sawNegative = true;
                    }
                    break;
            }
        }

        if (sb.Length == 0)
        {
            // Nit-picked quicker... :-)
            sb.Append(DefaultDisplay[0]);
        }

        value = $"{(sawNegative && sawNumber ? "-" : "")}{sb}";

        return value;
    }

    /// <summary>
    ///  Converts a calculator command and value to a string representation.
    /// </summary>
    /// <param name="command">The calculator command.</param>
    /// <param name="value">The value to display.</param>
    /// <returns>A string representing the command and value.</returns>
    public static string ToString(this CalculatorCommand command, Value value)
         => command switch
         {
             CalculatorCommand.Plus => $"{value} +",
             CalculatorCommand.Minus => $"{value} -",
             CalculatorCommand.Multiply => $"{value} ×",
             CalculatorCommand.Divide => $"{value} ÷",
             _ => string.Empty
         };

    /// <summary>
    ///  Computes the result of a binary calculator command on two values, or returns null if invalid.
    /// </summary>
    /// <param name="command">The calculator command.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The computed value, or null if the operation is invalid.</returns>
    public static Value? Compute(this CalculatorCommand command, Value left, Value right)
    {
        try
        {
            return command switch
            {
                CalculatorCommand.Plus => new Value(left.Number + right.Number),
                CalculatorCommand.Minus => new Value(left.Number - right.Number),
                CalculatorCommand.Multiply => new Value(left.Number * right.Number),
                CalculatorCommand.Divide => new Value(left.Number / right.Number),
                _ => null,
            };
        }
        catch
        {
            // Probably divide by zero
            return null;
        }
    }
}
