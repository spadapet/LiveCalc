namespace CalcCore;

public enum CalculatorCommand
{
    None,

    // Number input Commands
    Digit0,
    Digit1,
    Digit2,
    Digit3,
    Digit4,
    Digit5,
    Digit6,
    Digit7,
    Digit8,
    Digit9,
    DecimalPoint,
    Backspace,
    ClearAll,
    ClearEntry,

    // Binary operation Commands
    Plus,
    Minus,
    Multiply,
    Divide,

    // Unary operation Commands
    Equal,
    PlusMinus,
    Percent,
    SquareRoot,
    Square,
    Inverse,

    // Math Constants Command
    ConstantPi,

    // Memory operations Commands
    MemoryClear,
    MemoryRecall,
    MemoryAdd,
    MemorySubtract,
    MemoryStore,
}
