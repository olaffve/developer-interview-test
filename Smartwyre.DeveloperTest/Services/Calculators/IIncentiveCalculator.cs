using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public interface IIncentiveCalculator
{
    IncentiveType IncentiveType { get; }
    SupportedIncentiveType SupportedIncentiveType { get; }
    bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request);
    decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}