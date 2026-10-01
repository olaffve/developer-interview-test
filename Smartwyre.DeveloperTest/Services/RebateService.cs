using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Services.Calculators;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{

    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly Dictionary<IncentiveType, IIncentiveCalculator> _calculators;
    public RebateService(
        IRebateDataStore rebateDataStore, 
        IProductDataStore productDataStore,
        IEnumerable<IIncentiveCalculator> calculators)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _calculators = calculators.ToDictionary(c => c.IncentiveType);
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {

        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate == null || product == null)
        {
            return Failure();
        }

        if (!_calculators.TryGetValue(rebate.Incentive, out var calculator))
        {
            return Failure();
        }

        if (!calculator.IsValid(rebate, product, request))
        {
            return Failure();
        }

        if (!product.SupportedIncentives.HasFlag(calculator.SupportedIncentiveType))
        {
            return Failure();
        }

        var rebateAmount = calculator.Calculate(rebate, product, request);
        _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return new CalculateRebateResult
        {
            Success = true
        };
    }

    private static CalculateRebateResult Failure() => new CalculateRebateResult
    {
        Success = false
    };
}
