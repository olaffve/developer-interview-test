using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class FixedRateRebateCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedRateRebate;

    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Percentage != 0 && product.Price != 0 && request.Volume != 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Percentage * product.Price * request.Volume;
}