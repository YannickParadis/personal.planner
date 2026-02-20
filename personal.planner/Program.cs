using personal.planner.Components;
using personal.planner.Components.Data;
using personal.planner.Components.Layout;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<BudgetDbContext>(options =>
{
    var sqliteConnection = builder.Configuration.GetConnectionString("BudgetDb")
        ?? "Data Source=personal-planner.db";
    options.UseSqlite(sqliteConnection);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMudServices();
builder.Services.AddScoped<ThemeState>();
builder.Services.AddScoped<IncomeStore>();
builder.Services.AddScoped<MonthlyPaymentStore>();
builder.Services.AddScoped<SavingsStore>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
    BudgetDbInitializer.Initialize(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapBudgetApi();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
