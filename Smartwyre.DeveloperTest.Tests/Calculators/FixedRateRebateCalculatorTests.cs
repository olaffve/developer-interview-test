using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedRateRebateCalculatorTests
{
    private readonly FixedRateRebateCalculator  _calculator = new FixedRateRebateCalculator();
    
    [Fact]
    public void IsValid_WhenAllValuesAreNotZero_ShouldReturnTrue()
    {
        var rebate = new Rebate { Percentage = 0.05m };
        var product = new Product { Price = 100m };
        var request = new CalculateRebateRequest { Volume = 10m };

        var result = _calculator.IsValid(rebate, product, request);

        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 100, 10)]
    [InlineData(5, 0, 10)]
    [InlineData(5, 100, 0)]
    public void IsValid_WhenAnyValueIsZero_ShouldReturnFalse(decimal percentage, decimal price, decimal volume)
    {
        var rebate = new Rebate { Percentage = percentage };
        var product = new Product { Price = price };
        var request = new CalculateRebateRequest { Volume = volume };

        var result = _calculator.IsValid(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void Calculate_ReturnsPriceTimesPercentageTimesVolume()
    {
        var rebate = new Rebate { Percentage = 0.05m };
        var product = new Product { Price = 100m };
        var request = new CalculateRebateRequest { Volume = 10m };

        var result = _calculator.Calculate(rebate, product, request);

        var expected = rebate.Percentage * product.Price * request.Volume;
        Assert.Equal(expected, result);
    }
}