using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public sealed class IncomeStore
{
    private readonly BudgetDbContext _db;
    public event Action? Changed;

    public List<IncomeRow> Incomes { get; private set; } = [];

    public IncomeStore(BudgetDbContext db)
    {
        _db = db;
        Refresh();
    }

    public void Add(decimal amount, decimal taxAmount, string type, DateTime date)
    {
        _db.Incomes.Add(new IncomeRow
        {
            Amount = amount,
            TaxAmount = taxAmount,
            Type = type.Trim(),
            Date = date.Date
        });

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Update(int id, decimal amount, decimal taxAmount, string type, DateTime date)
    {
        var income = _db.Incomes.FirstOrDefault(x => x.Id == id);
        if (income is null)
        {
            return;
        }

        income.Amount = amount;
        income.TaxAmount = taxAmount;
        income.Type = type.Trim();
        income.Date = date.Date;

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Delete(int id)
    {
        var income = _db.Incomes.FirstOrDefault(x => x.Id == id);
        if (income is null)
        {
            return;
        }

        _db.Incomes.Remove(income);
        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    private void Refresh()
    {
        Incomes = _db.Incomes
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToList();
    }
}

public sealed class IncomeRow
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
