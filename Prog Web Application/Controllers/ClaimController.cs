/// <summary>
/// This class creates a new claim in the database. 
/// It ensures the uploaded file is of the correct type and size, and saves the claim to the database.
/// There is lots of error handling in place to ensure that the user is informed of any issues that occur.
/// <remarks>
/// Khan, A. (2024). Working with SQL Lite Database in Asp.NET Core Web API. [online] C-sharpcorner.com. Available at: https://www.c-sharpcorner.com/article/working-with-sql-lite-database-in-asp-net-core-web-api/.
/// For more information on model validation in ASP.NET Core, see:
/// Rick-Anderson (2022). Model validation in ASP.NET Core MVC. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-6.0.
/// For more information on handling errors in ASP.NET Core, see:
/// tdykstra (2023). Handle errors in ASP.NET Core. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0.
/// </remarks>
/// </summary>

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Prog_Web_Application.Controllers
{
    [Authorize]
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClaimController> _logger;
        private readonly string[] allowedExtensions = { ".pdf", ".docx", ".xlsx", ".txt", ".md" };
        private const int maxHours = 70;
        private const int maxFileSizeBytes = 10 * 1024 * 1024; 

        public ClaimController(ApplicationDbContext context, ILogger<ClaimController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaim(Models.Claim claim, IFormFile? uploadedFile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                claim.FullName ??= user.FullName;
                claim.Email ??= user.Email;
                claim.Phone ??= user.PhoneNumber;
            }

            if (uploadedFile != null)
            {
                var fileExtension = Path.GetExtension(uploadedFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("uploadedFile", "Invalid file type.");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }

                if (uploadedFile.Length > maxFileSizeBytes)
                {
                    ModelState.AddModelError("uploadedFile", "File size exceeds 10 MB.");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }

                using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                claim.UploadedFile = memoryStream.ToArray();
                claim.FileName = uploadedFile.FileName;
            }

            claim.SubmissionDate = DateTime.Now;
            claim.Status = (claim.HoursWorked < 1 || claim.HoursWorked > maxHours || claim.TotalAmount > 100000) // Max amount is R100,000
                ? ClaimStatus.Rejected 
                : ClaimStatus.Pending;

            try
            {
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving claim");
                ModelState.AddModelError("", "Unable to save claim.");
                return View("/Views/Home/NewClaim.cshtml", claim);
            }
        }
    }
}