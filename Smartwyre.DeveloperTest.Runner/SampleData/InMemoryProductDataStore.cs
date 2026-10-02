using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner.SampleData;

// Sample data so the runner can demonstrate real calculations.
// The real ProductDataStore is a placeholder that always returns an empty product.
public class InMemoryProductDataStore : IProductDataStore
{
    private readonly Dictionary<string, Product> _products = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase)
    {
        {
            "PROD-001",
            new Product
            {
                Identifier = "PROD-001",
                Price = 100m,
                Uom = "Bag",
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
                    | SupportedIncentiveType.FixedRateRebate
                    | SupportedIncentiveType.AmountPerUom
            }
        },
        {
            "PROD-002",
            new Product
            {
                Identifier = "PROD-002",
                Price = 50m,
                Uom = "Gallon",
                SupportedIncentives = SupportedIncentiveType.AmountPerUom
            }
        }
    };

    public Product GetProduct(string productIdentifier)
        => _products.TryGetValue(productIdentifier, out var product) ? product : null;
}