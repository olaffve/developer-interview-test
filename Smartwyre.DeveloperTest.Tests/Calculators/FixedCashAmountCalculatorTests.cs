using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedCashAmountCalculatorTests
{
    private readonly FixedCashAmountCalculator _calculator = new FixedCashAmountCalculator();

    [Fact]
    public void IsValid_WhenAmountIsNotZero_ShouldReturnTrue()
    {
        var rebate = new Rebate { Amount = 500m };

        var result = _calculator.IsValid(rebate, new Product(), new CalculateRebateRequest());

        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenAmountIsZero_ShouldReturnFalse()
    {
        var rebate = new Rebate { Amount = 0m };

        var result = _calculator.IsValid(rebate, new Product(), new CalculateRebateRequest());

        Assert.False(result);
    }

    [Fact]
    public void Calculate_ShouldReturnRebateAmount()
    {
        var rebate = new Rebate { Amount = 500m };
        var request = new CalculateRebateRequest { Volume = 10m };

        var result = _calculator.Calculate(rebate, new Product(), request);

        var expected = rebate.Amount;

        Assert.Equal(expected, result);
    }

}
