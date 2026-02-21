using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public static class BudgetDbInitializer
{
    public static void Initialize(BudgetDbContext db)
    {
        db.Database.EnsureCreated();
        EnsureRemindersSchema(db);
        db.Database.ExecuteSqlRaw("""
            CREATE TABLE IF NOT EXISTS Reminders (
                Id INTEGER NOT NULL CONSTRAINT PK_Reminders PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL DEFAULT '',
                Description TEXT NOT NULL,
                Date TEXT NOT NULL DEFAULT '2026-02-20'
            );
            """);

        if (!db.Incomes.Any())
        {
            db.Incomes.AddRange(
                new IncomeRow { Amount = 4200m, TaxAmount = 840m, Type = "Salary", Date = new DateTime(2026, 1, 31) },
                new IncomeRow { Amount = 750m, TaxAmount = 112.50m, Type = "Freelance", Date = new DateTime(2026, 2, 5) },
                new IncomeRow { Amount = 320m, TaxAmount = 48m, Type = "Dividends", Date = new DateTime(2026, 2, 12) },
                new IncomeRow { Amount = 1500m, TaxAmount = 300m, Type = "Bonus", Date = new DateTime(2026, 2, 15) },
                new IncomeRow { Amount = 580m, TaxAmount = 87m, Type = "Rental", Date = new DateTime(2026, 2, 18) }
            );
        }

        if (!db.Savings.Any())
        {
            db.Savings.AddRange(
                new SavingsRow { Description = "Emergency Fund", Amount = 5000m, AccountName = "High Yield Savings", Date = new DateTime(2026, 1, 15) },
                new SavingsRow { Description = "Vacation Savings", Amount = 1200m, AccountName = "Travel Account", Date = new DateTime(2026, 1, 30) },
                new SavingsRow { Description = "Home Down Payment", Amount = 8600m, AccountName = "Main Savings", Date = new DateTime(2026, 2, 5) },
                new SavingsRow { Description = "Car Maintenance Reserve", Amount = 750m, AccountName = "Auto Fund", Date = new DateTime(2026, 2, 10) },
                new SavingsRow { Description = "Education Fund", Amount = 2400m, AccountName = "Future Goals", Date = new DateTime(2026, 2, 18) }
            );
        }

        if (!db.MonthlyPayments.Any())
        {
            db.MonthlyPayments.AddRange(
                new MonthlyPaymentRow { Name = "Apartment Rent", Type = "Housing", PaymentType = "Bank Transfer", Amount = 1450m, Date = new DateTime(2026, 2, 1) },
                new MonthlyPaymentRow { Name = "Electricity Bill", Type = "Utilities", PaymentType = "Credit Card", Amount = 120.35m, Date = new DateTime(2026, 2, 3) },
                new MonthlyPaymentRow { Name = "Internet", Type = "Utilities", PaymentType = "Auto Debit", Amount = 69.99m, Date = new DateTime(2026, 2, 5) },
                new MonthlyPaymentRow { Name = "Car Insurance", Type = "Insurance", PaymentType = "Credit Card", Amount = 185m, Date = new DateTime(2026, 2, 8) },
                new MonthlyPaymentRow { Name = "Gym Membership", Type = "Health", PaymentType = "Auto Debit", Amount = 45m, Date = new DateTime(2026, 2, 10) }
            );
        }

        if (!db.Reminders.Any())
        {
            db.Reminders.AddRange(
                new ReminderRow { Title = "Pay Bills", Description = "Pay utility bills before the due date.", Date = new DateTime(2026, 2, 21) },
                new ReminderRow { Title = "Review Subscriptions", Description = "Review monthly subscriptions this weekend.", Date = new DateTime(2026, 2, 22) }
            );
        }

        db.SaveChanges();
    }

    private static void EnsureRemindersSchema(BudgetDbContext db)
    {
        var connection = db.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "PRAGMA table_info('Reminders');";
        using var reader = checkCommand.ExecuteReader();

        var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        while (reader.Read())
        {
            columns.Add(reader.GetString(1));
        }
        reader.Close();

        if (!columns.Contains("Title"))
        {
            db.Database.ExecuteSqlRaw("ALTER TABLE Reminders ADD COLUMN Title TEXT NOT NULL DEFAULT '';");
        }

        if (!columns.Contains("Date"))
        {
            db.Database.ExecuteSqlRaw("ALTER TABLE Reminders ADD COLUMN Date TEXT NOT NULL DEFAULT '2026-02-20';");
        }
    }
}
