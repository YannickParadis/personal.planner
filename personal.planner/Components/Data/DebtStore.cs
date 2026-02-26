using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public sealed class DebtStore
{
    private readonly BudgetDbContext _db;
    public event Action? Changed;

    public List<DebtRow> Debts { get; private set; } = [];

    public DebtStore(BudgetDbContext db)
    {
        _db = db;
        Refresh();
    }

    public void Add(string creditCardType, decimal amount, decimal minPayment, DateTime date)
    {
        _db.Debts.Add(new DebtRow
        {
            CreditCardType = creditCardType.Trim(),
            Amount = amount,
            MinPayment = minPayment,
            Date = date.Date
        });

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Update(int id, string creditCardType, decimal amount, decimal minPayment, DateTime date)
    {
        var debt = _db.Debts.FirstOrDefault(x => x.Id == id);
        if (debt is null)
        {
            return;
        }

        debt.CreditCardType = creditCardType.Trim();
        debt.Amount = amount;
        debt.MinPayment = minPayment;
        debt.Date = date.Date;

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Delete(int id)
    {
        var debt = _db.Debts.FirstOrDefault(x => x.Id == id);
        if (debt is null)
        {
            return;
        }

        _db.Debts.Remove(debt);
        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    private void Refresh()
    {
        Debts = _db.Debts
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToList();
    }
}

public sealed class DebtRow
{
    public int Id { get; set; }
    public string CreditCardType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal MinPayment { get; set; }
    public DateTime Date { get; set; }
}
