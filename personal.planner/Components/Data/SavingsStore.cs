using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public sealed class SavingsStore
{
    private readonly BudgetDbContext _db;
    public event Action? Changed;

    public List<SavingsRow> Savings { get; private set; } = [];

    public SavingsStore(BudgetDbContext db)
    {
        _db = db;
        Refresh();
    }

    public void Add(string description, decimal amount, string accountName, DateTime date)
    {
        _db.Savings.Add(new SavingsRow
        {
            Description = description.Trim(),
            Amount = amount,
            AccountName = accountName.Trim(),
            Date = date.Date
        });

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Update(int id, string description, decimal amount, string accountName, DateTime date)
    {
        var savingsRow = _db.Savings.FirstOrDefault(x => x.Id == id);
        if (savingsRow is null)
        {
            return;
        }

        savingsRow.Description = description.Trim();
        savingsRow.Amount = amount;
        savingsRow.AccountName = accountName.Trim();
        savingsRow.Date = date.Date;

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Delete(int id)
    {
        var savingsRow = _db.Savings.FirstOrDefault(x => x.Id == id);
        if (savingsRow is null)
        {
            return;
        }

        _db.Savings.Remove(savingsRow);
        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    private void Refresh()
    {
        Savings = _db.Savings
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToList();
    }
}

public sealed class SavingsRow
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
