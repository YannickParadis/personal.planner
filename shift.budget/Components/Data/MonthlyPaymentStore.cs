using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public sealed class MonthlyPaymentStore
{
    private readonly BudgetDbContext _db;
    public event Action? Changed;

    public List<MonthlyPaymentRow> MonthlyPayments { get; private set; } = [];

    public MonthlyPaymentStore(BudgetDbContext db)
    {
        _db = db;
        Refresh();
    }

    public void Add(string name, string type, string paymentType, decimal amount, DateTime date)
    {
        _db.MonthlyPayments.Add(new MonthlyPaymentRow
        {
            Name = name.Trim(),
            Type = type.Trim(),
            PaymentType = paymentType.Trim(),
            Amount = amount,
            Date = date.Date
        });

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Update(int id, string name, string type, string paymentType, decimal amount, DateTime date)
    {
        var monthlyPayment = _db.MonthlyPayments.FirstOrDefault(x => x.Id == id);
        if (monthlyPayment is null)
        {
            return;
        }

        monthlyPayment.Name = name.Trim();
        monthlyPayment.Type = type.Trim();
        monthlyPayment.PaymentType = paymentType.Trim();
        monthlyPayment.Amount = amount;
        monthlyPayment.Date = date.Date;

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Delete(int id)
    {
        var monthlyPayment = _db.MonthlyPayments.FirstOrDefault(x => x.Id == id);
        if (monthlyPayment is null)
        {
            return;
        }

        _db.MonthlyPayments.Remove(monthlyPayment);
        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    private void Refresh()
    {
        MonthlyPayments = _db.MonthlyPayments
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToList();
    }
}

public sealed class MonthlyPaymentRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string PaymentType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
