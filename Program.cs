using Microsoft.EntityFrameworkCore;
using EcommerceApp.Data;
using EcommerceApp.Controllers;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // <-- REQUIRED for MySQL configuration

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// -------------------------------------------------------------------
// KEY FIX: CONFIGURE DB CONTEXT FOR MYSQL
// -------------------------------------------------------------------

// 1. Retrieve the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Configure ApplicationDbContext to use the MySQL provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        // IMPORTANT: Specify the MySQL server version. You may need to adjust this number.
        // We use a common version for demonstration. You can also use ServerVersion.AutoDetect(connectionString)
        new MySqlServerVersion(new Version(8, 0, 31))
    )
);

// Add Cart Service to the dependency injection container
builder.Services.AddScoped<ICartService, EfCoreCartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}"); // <-- Assuming Product/Index is your main shop page

app.Run();