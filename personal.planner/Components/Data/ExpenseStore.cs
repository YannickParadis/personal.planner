using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public sealed class ExpenseStore
{
    private readonly BudgetDbContext _db;
    public event Action? Changed;

    public List<ExpenseRow> Expenses { get; private set; } = [];

    public ExpenseStore(BudgetDbContext db)
    {
        _db = db;
        Refresh();
    }

    public void Add(string name, string description, decimal amount, string paidWith, DateTime date)
    {
        _db.Expenses.Add(new ExpenseRow
        {
            Name = name.Trim(),
            Description = description.Trim(),
            Amount = amount,
            PaidWith = paidWith.Trim(),
            Date = date.Date
        });

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Update(int id, string name, string description, decimal amount, string paidWith, DateTime date)
    {
        var expense = _db.Expenses.FirstOrDefault(x => x.Id == id);
        if (expense is null)
        {
            return;
        }

        expense.Name = name.Trim();
        expense.Description = description.Trim();
        expense.Amount = amount;
        expense.PaidWith = paidWith.Trim();
        expense.Date = date.Date;

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Delete(int id)
    {
        var expense = _db.Expenses.FirstOrDefault(x => x.Id == id);
        if (expense is null)
        {
            return;
        }

        _db.Expenses.Remove(expense);
        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    private void Refresh()
    {
        Expenses = _db.Expenses
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToList();
    }
}

public sealed class ExpenseRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaidWith { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
