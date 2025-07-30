using System.Globalization;
using System.Text;

namespace CalcCore;

internal static class ValueHelpers
{
    public const string DefaultDisplay = "0";

    public static decimal ToNumber(string value)
    {
        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
        {
            return result;
        }

        return 0m;
    }

    public static string ToString(decimal value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Always returns a valid string representation of a number, given any number-like input.
    /// </summary>
    public static string FixStringInput(string value)
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
            sb.Append(ValueHelpers.DefaultDisplay);
        }

        value = $"{(sawNegative && sawNumber ? "-" : "")}{sb}";

        return value;
    }

    public static string ToString(Value value, Command command) => command switch
    {
        Command.Plus => $"{value} +",
        Command.Minus => $"{value} -",
        Command.Multiply => $"{value} ×",
        Command.Divide => $"{value} ÷",
        _ => string.Empty
    };

    /// <summary>
    /// Returns null if the operation cannot be computed (e.g., divide by zero).
    /// </summary>
    public static Value Compute(Value left, Command command, Value right)
    {
        try
        {
            switch (command)
            {
                case Command.Plus:
                    return new Value(left.Number + right.Number);

                case Command.Minus:
                    return new Value(left.Number - right.Number);

                case Command.Multiply:
                    return new Value(left.Number * right.Number);

                case Command.Divide:
                    return new Value(left.Number / right.Number);

                default:
                    return null;
            }
        }
        catch
        {
            // Probably divide by zero
            return null;
        }
    }
}
