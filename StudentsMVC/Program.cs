using Microsoft.EntityFrameworkCore;
using StudentsMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Отримуємо рядок підключення з файлу конфігурації
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Додаємо контекст StudentContext як сервіс у програму
builder.Services.AddDbContext<StudentContext>(options => options.UseSqlServer(connection));

// Додаємо сервіси MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles(); // Обробляє запити до статичних файлів у папці wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();