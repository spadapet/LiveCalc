using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LiveCalcCore
{
    public partial class CalcModel : ObservableObject
    {
        [ObservableProperty]
        private decimal result;

        [RelayCommand]
        private void Execute(Command command)
        {
            switch (command)
            {
                case Command.None:
                    break;

                case Command.Type0:
                    break;

                case Command.Type1:
                    break;

                case Command.Type2:
                    break;

                case Command.Type3:
                    break;

                case Command.Type4:
                    break;

                case Command.Type5:
                    break;

                case Command.Type6:
                    break;

                case Command.Type7:
                    break;

                case Command.Type8:
                    break;

                case Command.Type9:
                    break;

                case Command.DecimalPoint:
                    break;

                case Command.Backspace:
                    break;

                case Command.ClearAll:
                    break;

                case Command.ClearEntry:
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
                    break;

                case Command.Percent:
                    break;

                case Command.SquareRoot:
                    break;

                case Command.Square:
                    break;

                case Command.Inverse:
                    break;

                case Command.ConstantPi:
                    break;

                case Command.MemoryClear:
                    break;

                case Command.MemoryRecall:
                    break;

                case Command.MemoryAdd:
                    break;

                case Command.MemorySubtract:
                    break;

                case Command.MemoryStore:
                    break;

                default:
                    break;

            }
        }
    }
}
