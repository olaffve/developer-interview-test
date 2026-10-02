# Smartwyre Developer Test Instructions

You have been selected to complete our candidate coding exercise. Please follow the directions in this readme.

Clone, **DO NOT FORK**, this repository to your account on the online Git resource of your choosing (GitHub, BitBucket, GitLab, etc.). Your solution should retain previous commit history and you should utilize best practices for committing your changes to the repository.

You are welcome to use whatever tools you normally would when coding — including documentation, libraries, frameworks, or AI tools (such as ChatGPT or Copilot).

However, it is important that you fully understand your solution. As part of the interview process, we will review your code with you in detail. You should be able to:

- Explain the design choices you made.
- Walk us through how your solution works.
- Make modifications or extensions to your code during the review.

Please note: if your submission appears to have been generated entirely by an AI agent or another third party, without your own understanding or contribution, it will not meet our evaluation criteria.

# The Exercise

In the 'RebateService.cs' file you will find a method for calculating a rebate. At a high level the steps for calculating a rebate are:

 1. Lookup the rebate that the request is being made against.
 2. Lookup the product that the request is being made against.
 2. Check that the rebate and request are valid to calculate the incentive type rebate.
 3. Store the rebate calculation.

What we'd like you to do is refactor the code with the following things in mind:

 - Adherence to SOLID principles
 - Testability
 - Readability
 - Currently there are 3 known incentive types. In the future the business will want to add many more incentive types. Your solution should make it easy for developers to add new incentive types in the future.

We’d also like you to 
 - Add some unit tests to the Smartwyre.DeveloperTest.Tests project to show how you would test the code that you’ve produced 
 - Run the RebateService from the Smartwyre.DeveloperTest.Runner console application accepting inputs (either via command line arguments or via prompts is fine)

The only specific "rules" are:

- The solution must build
- All tests must pass

You are free to use any frameworks/NuGet packages that you see fit. You should plan to spend around 1 hour completing the exercise.

Feel free to use code comments to describe your changes. You are also welcome to update this readme with any important details for us to consider.

Once you have completed the exercise either ensure your repository is available publicly or contact the hiring manager to set up a private share.


---

## Solution Notes

### Issues found in the original code

- The switch on rebate.Incentive ran before the null check, so a missing rebate threw a NullReferenceException. The null checks inside each case were dead code.
- The FixedCashAmount case never checked whether the product was null, but still read product.SupportedIncentives.
- The data stores were created with new inside the method, so the service could not be unit tested.
- A second RebateDataStore instance was created just to save the result.
- The shared validation (rebate null, product null, supported incentive) was duplicated in every case.
- The method loaded data, validated, calculated and saved, all in one place.
- Adding a new incentive type required modifying the switch (Open/Closed violation).

### Design

- **Data store interfaces** (IRebateDataStore, IProductDataStore) are injected into RebateService through the constructor (Dependency Inversion).
- **One calculator per incentive type** (Services/Calculators). Each class implements IIncentiveCalculator and only contains its own validation rule and formula (Single Responsibility).
- **RebateService has no switch.** It receives all calculators, indexes them by incentive type in a dictionary, and runs the shared checks once using guard clauses. The dictionary also guarantees there is only one calculator per incentive type.
- Business rules were kept exactly the same as the original code. The only behavior change is that missing rebates or products now return Success = false instead of throwing.
- Added the [Flags] attribute to SupportedIncentiveType, since its values are combined.

### How to add a new incentive type

1. Add the value to IncentiveType.
2. Add the next flag to SupportedIncentiveType (1 << 3).
3. Create a new class that implements IIncentiveCalculator.
4. Register it in CreateRebateService in the Runner.

RebateService does not need to change.

### Tests

- Calculator tests verify each validation rule and formula in isolation.
- RebateService tests mock all dependencies with NSubstitute. Each test breaks one condition and checks that the result fails and nothing is stored.
- One test uses a real calculator to verify the pieces work together.

Run them with:

    dotnet test

### Running the console app

    dotnet run --project .\Smartwyre.DeveloperTest.Runner

The original data stores are placeholders that always return empty objects, so the Runner uses in-memory data stores with sample data (Runner/SampleData). This also shows how the service works with any implementation of the interfaces.

| Rebate   | Product  | Volume | Result                |
|----------|----------|--------|-----------------------|
| REB-CASH | PROD-001 | 10     | 500.00                |
| REB-RATE | PROD-001 | 10     | 50.00                 |
| REB-UOM  | PROD-001 | 300    | 600.00                |
| REB-UOM  | PROD-002 | 300    | 600.00                |
| REB-CASH | PROD-002 | 10     | Fails (not supported) |

After each calculation, press any key to run another one, or Esc to exit.