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

        // Define allowed file extensions and max file size
        private readonly string[] allowedExtensions = { ".pdf", ".docx", ".xlsx", ".txt", ".md" };
        private const int maxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        public ClaimController(ApplicationDbContext context, ILogger<ClaimController> logger)
        {
            _context = context;
            _logger = logger;
        }

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

            // Set the initial status of the claim to "Pending"
            claim.Status = ClaimStatus.Pending;

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

            try
            {
                // Add the claim to the database
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Claim saved successfully. File name: {claim.FileName}");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the claim");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View("/Views/Home/NewClaim.cshtml", claim);
            }
        }

        // Existing DownloadFile method remains unchanged
    }
}