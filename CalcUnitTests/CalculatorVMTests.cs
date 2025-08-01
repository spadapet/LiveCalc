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
        Assert.Equal("1", vm.DisplayValue);
        Assert.Equal(1m, vm.DisplayValue);
        Assert.Equal("0", vm.MemoryValue);
        Assert.Equal(0m, vm.MemoryValue);

        executeCommand.Execute(CalculatorCommand.Plus);
        executeCommand.Execute(CalculatorCommand.Digit1);

        Assert.Equal("1", vm.DisplayValue);
        Assert.Equal(1m, vm.DisplayValue);
        Assert.Equal("0", vm.MemoryValue);
        Assert.Equal(0m, vm.MemoryValue);
        executeCommand.Execute(CalculatorCommand.Equal);
        Assert.Equal("2", vm.DisplayValue);
        Assert.Equal(2m, vm.DisplayValue);
        Assert.Equal("0", vm.MemoryValue);
    }
}
