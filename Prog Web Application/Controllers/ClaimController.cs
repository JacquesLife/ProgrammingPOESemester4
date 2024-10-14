using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using System;
using System.IO; 
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Prog_Web_Application.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClaimController> _logger;

        public ClaimController(ApplicationDbContext context, ILogger<ClaimController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClaim(Claim claim, IFormFile? uploadedFile)
        {
            // Handle file upload
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                claim.uploadedFile = memoryStream.ToArray(); // Convert to byte array
            }
            else
            {
                claim.uploadedFile = null; // Explicitly set to null if no file is uploaded
            }

            // Set the submission date to the current date and time
            claim.submissionDate = DateTime.Now;

            // Set the initial status of the claim to "Pending"
            claim.Status = ClaimStatus.Pending;

            if (!ModelState.IsValid)
            {
                // Log and return the view with the model
                _logger.LogWarning("Invalid ModelState when creating claim");
                return View("/Views/Home/NewClaim.cshtml", claim);
            }

            try
            {
                // Add the claim to the database
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Claim saved successfully");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the claim");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View("/Views/Home/NewClaim.cshtml", claim);
            }
        }
    }
}