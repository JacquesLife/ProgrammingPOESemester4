using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using Prog_Web_Application.Controllers;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Construct the connection string for SQLite using a relative path
string connectionString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mydatabase.db");

// Add DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={connectionString}"));

// Register ProcessingController as a service
builder.Services.AddScoped<ProcessingController>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection(); // this will redirect all HTTP requests to HTTPS
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();