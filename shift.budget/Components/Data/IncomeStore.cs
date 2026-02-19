namespace shift.budget.Components.Data;

public sealed class IncomeStore
{
    private int _nextId = 6;

    public List<IncomeRow> Incomes { get; } =
    [
        new() { Id = 1, Amount = 4200m, TaxAmount = 840m, Type = "Salary", Date = new DateTime(2026, 1, 31) },
        new() { Id = 2, Amount = 750m, TaxAmount = 112.50m, Type = "Freelance", Date = new DateTime(2026, 2, 5) },
        new() { Id = 3, Amount = 320m, TaxAmount = 48m, Type = "Dividends", Date = new DateTime(2026, 2, 12) },
        new() { Id = 4, Amount = 1500m, TaxAmount = 300m, Type = "Bonus", Date = new DateTime(2026, 2, 15) },
        new() { Id = 5, Amount = 580m, TaxAmount = 87m, Type = "Rental", Date = new DateTime(2026, 2, 18) }
    ];

    public void Add(decimal amount, decimal taxAmount, string type, DateTime date)
    {
        Incomes.Add(new IncomeRow
        {
            Id = _nextId++,
            Amount = amount,
            TaxAmount = taxAmount,
            Type = type.Trim(),
            Date = date.Date
        });
    }

    public void Update(int id, decimal amount, decimal taxAmount, string type, DateTime date)
    {
        var income = Incomes.FirstOrDefault(x => x.Id == id);
        if (income is null)
        {
            return;
        }

        income.Amount = amount;
        income.TaxAmount = taxAmount;
        income.Type = type.Trim();
        income.Date = date.Date;
    }

    public void Delete(int id)
    {
        var income = Incomes.FirstOrDefault(x => x.Id == id);
        if (income is null)
        {
            return;
        }

        Incomes.Remove(income);
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
