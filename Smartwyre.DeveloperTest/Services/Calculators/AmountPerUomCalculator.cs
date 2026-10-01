using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class AmountPerUomCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.AmountPerUom;

    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Amount != 0 && request.Volume != 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Amount * request.Volume;
}