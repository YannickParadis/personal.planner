using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public sealed class BudgetDbContext(DbContextOptions<BudgetDbContext> options) : DbContext(options)
{
    public DbSet<IncomeRow> Incomes => Set<IncomeRow>();
    public DbSet<SavingsRow> Savings => Set<SavingsRow>();
    public DbSet<MonthlyPaymentRow> MonthlyPayments => Set<MonthlyPaymentRow>();
    public DbSet<DebtRow> Debts => Set<DebtRow>();
    public DbSet<ExpenseRow> Expenses => Set<ExpenseRow>();
    public DbSet<ReminderRow> Reminders => Set<ReminderRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IncomeRow>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Type).HasMaxLength(100);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            entity.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<SavingsRow>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Description).HasMaxLength(200);
            entity.Property(x => x.AccountName).HasMaxLength(100);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<MonthlyPaymentRow>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200);
            entity.Property(x => x.Type).HasMaxLength(100);
            entity.Property(x => x.PaymentType).HasMaxLength(100);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<DebtRow>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CreditCardType).HasMaxLength(120);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            entity.Property(x => x.MinPayment).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<ExpenseRow>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(140);
            entity.Property(x => x.Description).HasMaxLength(260);
            entity.Property(x => x.PaidWith).HasMaxLength(100);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<ReminderRow>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(120);
            entity.Property(x => x.Description).HasMaxLength(300);
            entity.Property(x => x.Date);
        });
    }
}
