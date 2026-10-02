using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner.SampleData;

// Sample data so the runner can demonstrate real calculations.
// The real RebateDataStore is a placeholder that always returns an empty rebate.
public class InMemoryRebateDataStore : IRebateDataStore
{
    private readonly Dictionary<string, Rebate> _rebates = new Dictionary<string, Rebate>(StringComparer.OrdinalIgnoreCase)
    {
        { "REB-CASH", new Rebate { Identifier = "REB-CASH", Incentive = IncentiveType.FixedCashAmount, Amount = 500m } },
        { "REB-RATE", new Rebate { Identifier = "REB-RATE", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.05m } },
        { "REB-UOM", new Rebate { Identifier = "REB-UOM", Incentive = IncentiveType.AmountPerUom, Amount = 2m } }
    };

    public Rebate GetRebate(string rebateIdentifier)
        => _rebates.TryGetValue(rebateIdentifier, out var rebate) ? rebate : null;

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
        => Console.WriteLine($"Stored rebate amount {rebateAmount:0.00} for rebate {account.Identifier}.");
}