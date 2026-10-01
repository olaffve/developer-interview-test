using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class FixedCashAmountCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedCashAmount;

    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Amount != 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Amount;
}