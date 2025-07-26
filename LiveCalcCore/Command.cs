namespace CalcCore
{
    public enum Command
    {
        None,

        // Typing
        Type0,
        Type1,
        Type2,
        Type3,
        Type4,
        Type5,
        Type6,
        Type7,
        Type8,
        Type9,
        DecimalPoint,
        Backspace,
        ClearAll,
        ClearEntry,

        // Binary operations
        Plus,
        Minus,
        Multiply,
        Divide,

        // Unary operations
        Equal,
        PlusMinus,
        Percent,
        SquareRoot,
        Square,
        Inverse,

        // Constants
        ConstantPi,

        // Memory operations
        MemoryClear,
        MemoryRecall,
        MemoryAdd,
        MemorySubtract,
        MemoryStore,
    }
}
