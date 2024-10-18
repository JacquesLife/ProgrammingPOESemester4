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
using System.Linq;

namespace Prog_Web_Application.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClaimController> _logger;

        // List of allowed file extensions and maximum file size
        private readonly string[] allowedExtensions = { ".pdf", ".docx", ".xlsx", ".txt", ".md" };
        private const int maxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        // Constructor with ApplicationDbContext and ILogger parameters
        public ClaimController(ApplicationDbContext context, ILogger<ClaimController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// This method is responsible for creating a new claim in the database. 
        /// It receives a Claim object and an optional IFormFile object representing an uploaded file.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateClaim(Claim claim, IFormFile? uploadedFile)
        {
            _logger.LogInformation("CreateClaim method called");

            if (uploadedFile != null)
            {
                _logger.LogInformation($"File uploaded: Name = {uploadedFile.FileName}, Size = {uploadedFile.Length} bytes");

                // Validate file extension
                var fileExtension = Path.GetExtension(uploadedFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("uploadedFile", "Invalid file type. Allowed types are: .pdf, .docx, .xlsx, .txt, .md");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }

                // Validate file size
                if (uploadedFile.Length > maxFileSizeBytes)
                {
                    ModelState.AddModelError("uploadedFile", "File size exceeds the limit of 10 MB.");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }

                try
                {
                    // Read the uploaded file into a memory stream
                    using var memoryStream = new MemoryStream();
                    await uploadedFile.CopyToAsync(memoryStream);
                    claim.uploadedFile = memoryStream.ToArray();
                    claim.FileName = uploadedFile.FileName;
                    _logger.LogInformation($"File successfully processed. Size: {claim.uploadedFile.Length} bytes, Name: {claim.FileName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing uploaded file");
                    ModelState.AddModelError("", "Error occurred while processing the uploaded file. Please try again.");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }
            }
            else
            {
                claim.uploadedFile = null;
                claim.FileName = null;
                _logger.LogInformation("No file uploaded");
            }

            // Set the submission date to the current date and time
            claim.submissionDate = DateTime.Now;

            // Default status of the claim is set to "Pending"
            claim.Status = ClaimStatus.Pending;

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid ModelState when creating claim");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning($"Model error: {error.ErrorMessage}");
                    }
                }
                return View("/Views/Home/NewClaim.cshtml", claim);
            }

            // Check for duplicate email before saving
            var emailExists = await _context.Claims.AnyAsync(c => c.Email == claim.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(claim.Email), "This email is already in use.");
                return View("/Views/Home/NewClaim.cshtml", claim);
            }

            // Check for duplicate phone number before saving
            var phoneExists = await _context.Claims.AnyAsync(c => c.Phone == claim.Phone);
            if (phoneExists)
            {
                ModelState.AddModelError(nameof(claim.Phone), "This phone number is already in use.");
                return View("/Views/Home/NewClaim.cshtml", claim);
            }

            try
            {
                // Add the claim to the database and save changes
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Claim saved successfully. File name: {claim.FileName}");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the claim");
                ModelState.AddModelError("", "Unable to save changes.");
                return View("/Views/Home/NewClaim.cshtml", claim);
            }
        }
    }
}

// --------------------------------------------**End of File**--------------------------------------------------------
