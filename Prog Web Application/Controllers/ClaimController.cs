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

        // Post method to create a new claim
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
                    // Add model error for invalid file type
                    ModelState.AddModelError("uploadedFile", "Invalid file type. Allowed types are: .pdf, .docx, .xlsx, .txt, .md");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }

                // Validate file size
                if (uploadedFile.Length > maxFileSizeBytes)
                {
                    // Add model error for file size exceeding the limit
                    ModelState.AddModelError("uploadedFile", "File size exceeds the limit of 10 MB.");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }
                try
                {   // Read the uploaded file into a memory stream
                    using var memoryStream = new MemoryStream();
                    await uploadedFile.CopyToAsync(memoryStream);
                    claim.uploadedFile = memoryStream.ToArray();
                    claim.FileName = uploadedFile.FileName;
                    // Log successful processing of the file
                    _logger.LogInformation($"File successfully processed. Size: {claim.uploadedFile.Length} bytes, Name: {claim.FileName}");
                }
                catch (Exception ex)
                {
                    // throw an error if there is an issue processing the file
                    _logger.LogError(ex, "Error occurred while processing uploaded file");
                    ModelState.AddModelError("", "Error occurred while processing the uploaded file. Please try again.");
                    return View("/Views/Home/NewClaim.cshtml", claim);
                }
            }
            else
            {
                // If no file is uploaded, set the uploaded file and file name to null
                claim.uploadedFile = null;
                claim.FileName = null;
                _logger.LogInformation("No file uploaded");
            }

            // Set the submission date to the current date and time
            claim.submissionDate = DateTime.Now;

            // Default status of the claim is set to "Pending"
            claim.Status = ClaimStatus.Pending;

            if (!ModelState.IsValid)
            {
                // Log model state errors if the model is invalid
                _logger.LogWarning("Invalid ModelState when creating claim");
                foreach (var modelState in ModelState.Values)
                {
                    // Log each error message
                    foreach (var error in modelState.Errors)
                    {    
                        _logger.LogWarning($"Model error: {error.ErrorMessage}");
                    }
                }
                // Return the view with the invalid claim model
                return View("/Views/Home/NewClaim.cshtml", claim);
            }

            try
            {
                // Add the claim to the database and save changes
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                // Log successful claim creation
                _logger.LogInformation($"Claim saved successfully. File name: {claim.FileName}");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log error if there is an issue saving the claim
                _logger.LogError(ex, "An error occurred while saving the claim");

                // Add model error for generic save error
                ModelState.AddModelError("", "Unable to save changes.");

                return View("/Views/Home/NewClaim.cshtml", claim);
            }
        }
    }
}