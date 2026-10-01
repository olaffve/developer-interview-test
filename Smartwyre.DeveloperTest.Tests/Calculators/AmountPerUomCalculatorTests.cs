using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class AmountPerUomCalculatorTests
{
    private readonly AmountPerUomCalculator _calculator = new AmountPerUomCalculator();

    [Fact]
    public void IsValid_WhenAllValuesAreNotZero_ShouldReturnTrue()
    {
        var rebate = new Rebate { Amount = 10m };
        var request = new CalculateRebateRequest { Volume = 300m };

        var result = _calculator.IsValid(rebate, new Product(), request);

        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 300)]
    [InlineData(2, 0)]
    public void IsValid_WhenAmountOrVolumeIsZero_ReturnsFalse(int amount, int volume)
    {
        var rebate = new Rebate { Amount = amount };
        var request = new CalculateRebateRequest { Volume = volume };

        var result = _calculator.IsValid(rebate, new Product(), request);

        Assert.False(result);
    }

    [Fact]
    public void Calculate_ReturnsAmountPerUomTimesVolume()
    {
        var rebate = new Rebate { Amount = 2m };
        var request = new CalculateRebateRequest { Volume = 300m };

        var result = _calculator.Calculate(rebate, new Product(), request);

        var expected = rebate.Amount * request.Volume;
        Assert.Equal(expected, result);
    }
}