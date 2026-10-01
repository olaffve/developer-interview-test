using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

public class InMemoryRebateDataStore : IRebateDataStore
{
    private readonly Dictionary<string, Rebate> _rabate = new Dictionary<string, Rebate>(StringComparer.OrdinalIgnoreCase)
    {
        { "REB-CASH", new Rebate { Identifier = "REB-CASH", Incentive = IncentiveType.FixedCashAmount, Amount = 500m } },
        { "REB-RATE", new Rebate { Identifier = "REB-RATE", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.5m } },
        { "REB-UOM", new Rebate { Identifier = "REB-UOM", Incentive = IncentiveType.AmountPerUom, Amount = 2m } }
    };

    public Rebate GetRebate(string rebateIdentifier) 
        => _rabate.TryGetValue(rebateIdentifier, out var rebate) ? rebate : null;

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
        => Console.WriteLine($"Stored rebate amount {rebateAmount} for rebate {account.Identifier}.");
}