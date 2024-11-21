/// <summary>
/// This file contains the main entry point for the application.
/// It configures the application and sets up the database connection.
/// <remarks>
/// <remarks>
/// TutorialBrain (2022). How to Run SQLITE in Visual Studio Code. [online] YouTube. Available at: https://www.youtube.com/watch?v=JrAiefGNUq8.
/// <summary>
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using Prog_Web_Application.Controllers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Construct the connection string for SQLite using a relative path
string connectionString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mydatabase.db");

// Add DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={connectionString}"));

// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false; 
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.Cookie.Name = "ProgWebAppCookie";
});

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAcademicManagerRole", policy => 
        policy.RequireRole("Academic Manager"));
    options.AddPolicy("RequireProgramCoordinatorRole", policy => 
        policy.RequireRole("Program Coordinator"));
    options.AddPolicy("RequireLecturerRole", policy => 
        policy.RequireRole("Lecturer"));
});

// Register ProcessingController as a service
builder.Services.AddScoped<ProcessingController>();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Initialize Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        // Create roles if they don't exist
        var roles = new[] { "Academic Manager", "Program Coordinator", "Lecturer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Remove invalid roles (like "User")
        var invalidRoles = new[] { "User" };
        foreach (var invalidRole in invalidRoles)
        {
            var role = await roleManager.FindByNameAsync(invalidRole);
            if (role != null)
            {
                await roleManager.DeleteAsync(role);
            }
        }

        // Optionally create a default admin user
        var adminEmail = "admin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var admin = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, "Admin123!"); // Default password
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Academic Manager");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles and users.");
    }
}

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

// Enable HTTPS Redirection
app.UseHttpsRedirection();

// Serve static files
app.UseStaticFiles();

// Use routing middleware
app.UseRouting();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure routing for controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
