using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public static class BudgetApiEndpoints
{
    public static void MapBudgetApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        var incomes = api.MapGroup("/incomes").WithTags("Incomes");
        incomes.MapGet("/", GetIncomes);
        incomes.MapGet("/{id:int}", GetIncomeById);
        incomes.MapPost("/", CreateIncome);
        incomes.MapPut("/{id:int}", UpdateIncome);
        incomes.MapDelete("/{id:int}", DeleteIncome);

        var savings = api.MapGroup("/savings").WithTags("Savings");
        savings.MapGet("/", GetSavings);
        savings.MapGet("/{id:int}", GetSavingsById);
        savings.MapPost("/", CreateSavings);
        savings.MapPut("/{id:int}", UpdateSavings);
        savings.MapDelete("/{id:int}", DeleteSavings);

        var monthlyPayments = api.MapGroup("/monthly-payments").WithTags("Monthly Payments");
        monthlyPayments.MapGet("/", GetMonthlyPayments);
        monthlyPayments.MapGet("/{id:int}", GetMonthlyPaymentById);
        monthlyPayments.MapPost("/", CreateMonthlyPayment);
        monthlyPayments.MapPut("/{id:int}", UpdateMonthlyPayment);
        monthlyPayments.MapDelete("/{id:int}", DeleteMonthlyPayment);
    }

    private static async Task<Ok<List<IncomeRow>>> GetIncomes(BudgetDbContext db) =>
        TypedResults.Ok(await db.Incomes.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id).ToListAsync());

    private static async Task<Results<Ok<IncomeRow>, NotFound>> GetIncomeById(int id, BudgetDbContext db)
    {
        var income = await db.Incomes.FindAsync(id);
        return income is null ? TypedResults.NotFound() : TypedResults.Ok(income);
    }

    private static async Task<Results<Created<IncomeRow>, ValidationProblem>> CreateIncome(IncomeRequest request, BudgetDbContext db)
    {
        var errors = ValidateIncomeRequest(request);
        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var income = new IncomeRow
        {
            Amount = request.Amount,
            TaxAmount = request.TaxAmount,
            Type = request.Type.Trim(),
            Date = request.Date.Date
        };

        db.Incomes.Add(income);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/api/incomes/{income.Id}", income);
    }

    private static async Task<Results<Ok<IncomeRow>, NotFound, ValidationProblem>> UpdateIncome(int id, IncomeRequest request, BudgetDbContext db)
    {
        var errors = ValidateIncomeRequest(request);
        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var income = await db.Incomes.FindAsync(id);
        if (income is null)
        {
            return TypedResults.NotFound();
        }

        income.Amount = request.Amount;
        income.TaxAmount = request.TaxAmount;
        income.Type = request.Type.Trim();
        income.Date = request.Date.Date;
        await db.SaveChangesAsync();

        return TypedResults.Ok(income);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteIncome(int id, BudgetDbContext db)
    {
        var income = await db.Incomes.FindAsync(id);
        if (income is null)
        {
            return TypedResults.NotFound();
        }

        db.Incomes.Remove(income);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    private static async Task<Ok<List<SavingsRow>>> GetSavings(BudgetDbContext db) =>
        TypedResults.Ok(await db.Savings.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id).ToListAsync());

    private static async Task<Results<Ok<SavingsRow>, NotFound>> GetSavingsById(int id, BudgetDbContext db)
    {
        var savingsRow = await db.Savings.FindAsync(id);
        return savingsRow is null ? TypedResults.NotFound() : TypedResults.Ok(savingsRow);
    }

    private static async Task<Results<Created<SavingsRow>, ValidationProblem>> CreateSavings(SavingsRequest request, BudgetDbContext db)
    {
        var errors = ValidateSavingsRequest(request);
        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var savingsRow = new SavingsRow
        {
            Description = request.Description.Trim(),
            Amount = request.Amount,
            AccountName = request.AccountName.Trim(),
            Date = request.Date.Date
        };

        db.Savings.Add(savingsRow);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/api/savings/{savingsRow.Id}", savingsRow);
    }

    private static async Task<Results<Ok<SavingsRow>, NotFound, ValidationProblem>> UpdateSavings(int id, SavingsRequest request, BudgetDbContext db)
    {
        var errors = ValidateSavingsRequest(request);
        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var savingsRow = await db.Savings.FindAsync(id);
        if (savingsRow is null)
        {
            return TypedResults.NotFound();
        }

        savingsRow.Description = request.Description.Trim();
        savingsRow.Amount = request.Amount;
        savingsRow.AccountName = request.AccountName.Trim();
        savingsRow.Date = request.Date.Date;
        await db.SaveChangesAsync();

        return TypedResults.Ok(savingsRow);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteSavings(int id, BudgetDbContext db)
    {
        var savingsRow = await db.Savings.FindAsync(id);
        if (savingsRow is null)
        {
            return TypedResults.NotFound();
        }

        db.Savings.Remove(savingsRow);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    private static async Task<Ok<List<MonthlyPaymentRow>>> GetMonthlyPayments(BudgetDbContext db) =>
        TypedResults.Ok(await db.MonthlyPayments.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id).ToListAsync());

    private static async Task<Results<Ok<MonthlyPaymentRow>, NotFound>> GetMonthlyPaymentById(int id, BudgetDbContext db)
    {
        var monthlyPayment = await db.MonthlyPayments.FindAsync(id);
        return monthlyPayment is null ? TypedResults.NotFound() : TypedResults.Ok(monthlyPayment);
    }

    private static async Task<Results<Created<MonthlyPaymentRow>, ValidationProblem>> CreateMonthlyPayment(MonthlyPaymentRequest request, BudgetDbContext db)
    {
        var errors = ValidateMonthlyPaymentRequest(request);
        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var monthlyPayment = new MonthlyPaymentRow
        {
            Name = request.Name.Trim(),
            Type = request.Type.Trim(),
            PaymentType = request.PaymentType.Trim(),
            Amount = request.Amount,
            Date = request.Date.Date
        };

        db.MonthlyPayments.Add(monthlyPayment);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/api/monthly-payments/{monthlyPayment.Id}", monthlyPayment);
    }

    private static async Task<Results<Ok<MonthlyPaymentRow>, NotFound, ValidationProblem>> UpdateMonthlyPayment(int id, MonthlyPaymentRequest request, BudgetDbContext db)
    {
        var errors = ValidateMonthlyPaymentRequest(request);
        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var monthlyPayment = await db.MonthlyPayments.FindAsync(id);
        if (monthlyPayment is null)
        {
            return TypedResults.NotFound();
        }

        monthlyPayment.Name = request.Name.Trim();
        monthlyPayment.Type = request.Type.Trim();
        monthlyPayment.PaymentType = request.PaymentType.Trim();
        monthlyPayment.Amount = request.Amount;
        monthlyPayment.Date = request.Date.Date;
        await db.SaveChangesAsync();

        return TypedResults.Ok(monthlyPayment);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteMonthlyPayment(int id, BudgetDbContext db)
    {
        var monthlyPayment = await db.MonthlyPayments.FindAsync(id);
        if (monthlyPayment is null)
        {
            return TypedResults.NotFound();
        }

        db.MonthlyPayments.Remove(monthlyPayment);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    private static Dictionary<string, string[]> ValidateIncomeRequest(IncomeRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            errors["type"] = ["Type is required."];
        }
        else if (request.Type.Trim().Length > 100)
        {
            errors["type"] = ["Type cannot exceed 100 characters."];
        }

        if (request.Amount < 0)
        {
            errors["amount"] = ["Amount must be greater than or equal to 0."];
        }

        if (request.TaxAmount < 0)
        {
            errors["taxAmount"] = ["TaxAmount must be greater than or equal to 0."];
        }

        return errors;
    }

    private static Dictionary<string, string[]> ValidateSavingsRequest(SavingsRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            errors["description"] = ["Description is required."];
        }
        else if (request.Description.Trim().Length > 200)
        {
            errors["description"] = ["Description cannot exceed 200 characters."];
        }

        if (request.Amount < 0)
        {
            errors["amount"] = ["Amount must be greater than or equal to 0."];
        }

        if (string.IsNullOrWhiteSpace(request.AccountName))
        {
            errors["accountName"] = ["AccountName is required."];
        }
        else if (request.AccountName.Trim().Length > 100)
        {
            errors["accountName"] = ["AccountName cannot exceed 100 characters."];
        }

        return errors;
    }

    private static Dictionary<string, string[]> ValidateMonthlyPaymentRequest(MonthlyPaymentRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors["name"] = ["Name is required."];
        }
        else if (request.Name.Trim().Length > 200)
        {
            errors["name"] = ["Name cannot exceed 200 characters."];
        }

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            errors["type"] = ["Type is required."];
        }
        else if (request.Type.Trim().Length > 100)
        {
            errors["type"] = ["Type cannot exceed 100 characters."];
        }

        if (string.IsNullOrWhiteSpace(request.PaymentType))
        {
            errors["paymentType"] = ["PaymentType is required."];
        }
        else if (request.PaymentType.Trim().Length > 100)
        {
            errors["paymentType"] = ["PaymentType cannot exceed 100 characters."];
        }

        if (request.Amount < 0)
        {
            errors["amount"] = ["Amount must be greater than or equal to 0."];
        }

        return errors;
    }
}

public sealed class IncomeRequest
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("taxAmount")]
    public decimal TaxAmount { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}

public sealed class SavingsRequest
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}

public sealed class MonthlyPaymentRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("paymentType")]
    public string PaymentType { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}
