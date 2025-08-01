using CalcCore;
using System.Windows.Input;

namespace CalcUnitTests;

public class CalculatorVMTests
{
    [Fact]
    public void DefaultConstructor_InitializationTest()
    {
        CalculatorViewModel vm = new();

        ICommand executeCommand = vm.ExecuteCommand;
        executeCommand.Execute(CalculatorCommand.Digit1);
        Assert.Equal("1", vm.Display);
        Assert.Equal(1m, vm.Number);
        Assert.Equal("0", vm.MemoryDisplay);
        Assert.Equal(0m, vm.MemoryNumber);

        executeCommand.Execute(CalculatorCommand.Plus);
        executeCommand.Execute(CalculatorCommand.Digit1);

        Assert.Equal("1", vm.Display);
        Assert.Equal(1m, vm.Number);
        Assert.Equal("0", vm.MemoryDisplay);
        Assert.Equal(0m, vm.MemoryNumber);
        executeCommand.Execute(CalculatorCommand.Equal);
        Assert.Equal("2", vm.Display);
        Assert.Equal(2m, vm.Number);
        Assert.Equal("0", vm.MemoryDisplay);
    }
}
