using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using NSubstitute;
using Xunit;


namespace Smartwyre.DeveloperTest.Tests.Services;

public class RebateServiceTests
{
    private readonly IRebateDataStore _rebateDataStore = Substitute.For<IRebateDataStore>();
    private readonly IProductDataStore _productDataStore = Substitute.For<IProductDataStore>();
    private readonly IIncentiveCalculator _calculator = Substitute.For<IIncentiveCalculator>();
    private readonly RebateService _service;
    private readonly Rebate _rebate;
    private readonly Product _product;
    private readonly CalculateRebateRequest _request;

    public RebateServiceTests()
    {
        _rebate = new Rebate{Identifier = "REB-1", Incentive = IncentiveType.FixedCashAmount, Amount = 500m};
        _product = new Product{Identifier = "PROD-1", SupportedIncentives = SupportedIncentiveType.FixedCashAmount};
        _request = new CalculateRebateRequest{RebateIdentifier = "REB-1", ProductIdentifier= "PROD-1", Volume = 10m};

        _rebateDataStore.GetRebate("REB-1").Returns(_rebate);
        _productDataStore.GetProduct("PROD-1").Returns(_product);

        _calculator.IncentiveType.Returns(IncentiveType.FixedCashAmount);
        _calculator.SupportedIncentiveType.Returns(SupportedIncentiveType.FixedCashAmount);
        _calculator.IsValid(_rebate, _product, _request).Returns(true);
        _calculator.Calculate(_rebate, _product, _request).Returns(500m);

        _service = new RebateService(_rebateDataStore, _productDataStore, [_calculator]);

    }

    [Fact]
    public void CalculateRebate_WhenAllChecksPass_StoreResultAndReturnsSuccess()
    {
        var expected = 500m;
        var result = _service.Calculate(_request);
        
        Assert.True(result.Success);
        _rebateDataStore.Received(1).StoreCalculationResult(_rebate, expected);
    }

    [Fact]
    public void Calculate_WhenRebateIsNotFound_ReturnsFailureAndDoesNotStore()
    {
        _rebateDataStore.GetRebate("REB-1").Returns((Rebate)null);

        var result = _service.Calculate(_request);

        Assert.False(result.Success);
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());

    }

    [Fact]
    public void Calculate_WhenProductIsNotFound_ReturnsFailureAndDoesNotStore()
    {
        _productDataStore.GetProduct("PROD-1").Returns((Product)null);

        var result = _service.Calculate(_request);

        Assert.False(result.Success);
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Calculate_WhenNoCalculatorExistsForIncentiveType_ReturnsFailureAndDoesNotStore()
    {
        _rebate.Incentive = IncentiveType.FixedRateRebate;

        var result = _service.Calculate(_request);

        Assert.False(result.Success);
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Calculate_WhenProductDoesNotSupportIncentiveType_ReturnsFailureAndDoesNotStore()
    {
        _product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;

        var result = _service.Calculate(_request);

        Assert.False(result.Success);
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }   

    [Fact]
    public void Calculate_WhenCalculatorRejectsRequest_ReturnsFailureAndDoesNotStore()
    {
        _calculator.IsValid(_rebate, _product, _request).Returns(false);

        var result = _service.Calculate(_request);

        Assert.False(result.Success);
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }
}