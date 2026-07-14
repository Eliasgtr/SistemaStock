using Microsoft.EntityFrameworkCore;
using SistemaStock.Domain.Entities;
using SistemaStock.Application.ViewModels;
using SistemaStock.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var useSqlite = builder.Configuration.GetValue<bool>("UseSQLite") || 
                builder.Environment.IsProduction();

builder.Services.AddDbContext<SistemaStockContext>(options =>
{
    if (useSqlite)
    {
        var dbPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dbPath);
        var connectionString = $"Data Source={Path.Combine(dbPath, "stock.db")}";
        options.UseSqlite(connectionString);
    }
    else
    {
        var conString = builder.Configuration.GetConnectionString("ConexionSQL") ??
             throw new InvalidOperationException("Connection string 'ConexionSQL' not found.");
        options.UseSqlServer(conString);
    }
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (useSqlite)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<SistemaStockContext>();
        db.Database.EnsureCreated();
    }
}

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Registro}/{id?}")
    .WithStaticAssets();


app.Run();
