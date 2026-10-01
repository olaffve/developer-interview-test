using System;
using Smartwyre.DeveloperTest.Runner.SampleData;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var rebateService = CreateRebateService();

        do
        {
            RunCalculation(rebateService);

            Console.WriteLine("Press any key to calculate another rebate, or ESC to exit.");
        }
        while (Console.ReadKey(intercept: true).Key != ConsoleKey.Escape);
    }

    private static void RunCalculation(IRebateService rebateService)
    {
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = ReadRequiredText("Rebate Identifier: "),
            ProductIdentifier = ReadRequiredText("Product Identifier: "),
            Volume = ReadDecimal("Volume: ")
        };

        var result = rebateService.Calculate(request);

        Console.WriteLine(result.Success
            ? "Rebate calculated and stored successfully."
            : "Rebate could not be calculated for this request.");
    }

    private static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }
            Console.WriteLine("Invalid input. Please enter a valid text value.");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (decimal.TryParse(input, out var value))
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a valid decimal number.");
        }
    }

    private static IRebateService CreateRebateService()
    {
        var calculators = new IIncentiveCalculator[] 
        { 
            new FixedCashAmountCalculator(),
            new FixedRateRebateCalculator(),
            new AmountPerUomCalculator() 
        };

        return new RebateService(new InMemoryRebateDataStore(), new InMemoryProductDataStore(), calculators);
    }
}
