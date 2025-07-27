using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Text;

namespace CalcCore
{
    public partial class CalcModel : ObservableObject
    {
        private const string DefaultDisplay = "0";
        private string display = CalcModel.DefaultDisplay;
        private string memoryDisplay = CalcModel.DefaultDisplay;

        public string Display
        {
            get => this.display;
            set
            {
                // Validate and fix input
                value = value.Trim();
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
                    sb.Append(CalcModel.DefaultDisplay);
                }

                value = $"{(sawNegative && sawNumber ? "-" : "")}{sb}";

                if (!string.Equals(this.display, value, StringComparison.Ordinal))
                {
                    this.display = value;
                    this.OnPropertyChanged(nameof(this.Display));
                    this.OnPropertyChanged(nameof(this.Number));
                }
            }
        }

        public decimal Number
        {
            get
            {
                if (decimal.TryParse(this.Display, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }

                return 0m;
            }

            set
            {
                this.Display = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string MemoryDisplay
        {
            get => this.memoryDisplay;
            set
            {
                if (!string.Equals(this.memoryDisplay, value, StringComparison.Ordinal))
                {
                    this.memoryDisplay = value;
                    this.OnPropertyChanged(nameof(this.MemoryDisplay));
                    this.OnPropertyChanged(nameof(this.MemoryNumber));
                }
            }
        }

        public decimal MemoryNumber
        {
            get
            {
                if (decimal.TryParse(this.MemoryDisplay, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }

                return 0m;
            }

            set
            {
                this.MemoryDisplay = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        [RelayCommand]
        private void Execute(Command command)
        {
            switch (command)
            {
                case Command.None:
                    break;

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
                    this.Display = CalcModel.DefaultDisplay;
                    break;

                case Command.ClearEntry:
                    this.Display = CalcModel.DefaultDisplay;
                    break;

                case Command.Plus:
                    break;

                case Command.Minus:
                    break;

                case Command.Multiply:
                    break;

                case Command.Divide:
                    break;

                case Command.Equal:
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
                    this.MemoryDisplay = CalcModel.DefaultDisplay;
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

                default:
                    break;

            }
        }
    }
}
