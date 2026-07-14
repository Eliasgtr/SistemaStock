using Microsoft.EntityFrameworkCore;
using SistemaStock.Domain.Entities;
using SistemaStock.Application.ViewModels;
using SistemaStock.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var usePostgres = builder.Environment.IsProduction();

builder.Services.AddDbContext<SistemaStockContext>(options =>
{
    if (usePostgres)
    {
        // Try getting connection string from different common environment configurations
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
                               builder.Configuration.GetConnectionString("ConexionPostgres") ??
                               builder.Configuration.GetConnectionString("ConexionSQL");
                               
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La cadena de conexión a PostgreSQL no fue configurada.");
        }
        options.UseNpgsql(connectionString);
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

if (usePostgres)
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
