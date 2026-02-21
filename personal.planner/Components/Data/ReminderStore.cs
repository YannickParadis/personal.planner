using Microsoft.EntityFrameworkCore;

namespace personal.planner.Components.Data;

public sealed class ReminderStore
{
    private readonly BudgetDbContext _db;
    public event Action? Changed;

    public List<ReminderRow> Reminders { get; private set; } = [];

    public ReminderStore(BudgetDbContext db)
    {
        _db = db;
        Refresh();
    }

    public void Add(string title, string description, DateTime date)
    {
        _db.Reminders.Add(new ReminderRow
        {
            Title = title.Trim(),
            Date = date.Date,
            Description = description.Trim()
        });

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Update(int id, string title, string description, DateTime date)
    {
        var reminder = _db.Reminders.FirstOrDefault(x => x.Id == id);
        if (reminder is null)
        {
            return;
        }

        reminder.Title = title.Trim();
        reminder.Description = description.Trim();
        reminder.Date = date.Date;

        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    public void Delete(int id)
    {
        var reminder = _db.Reminders.FirstOrDefault(x => x.Id == id);
        if (reminder is null)
        {
            return;
        }

        _db.Reminders.Remove(reminder);
        _db.SaveChanges();
        Refresh();
        Changed?.Invoke();
    }

    private void Refresh()
    {
        Reminders = _db.Reminders
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToList();
    }
}

public sealed class ReminderRow
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
}
