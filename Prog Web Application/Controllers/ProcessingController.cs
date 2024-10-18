/// <summary>
/// This controller is responsible for processing claims. It retrieves claims from the database and allows the user to update the status of a claim.
/// <remarks>
/// tdykstra (2023). Handle errors in ASP.NET Core. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0.
/// Hassan, Z.U. (2024). Action Result In ASP.NET MVC. [online] C-sharpcorner.com. Available at: https://www.c-sharpcorner.com/article/action-result-in-asp-net-mvc/.
/// </remarks>
/// </summary>

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Prog_Web_Application.Controllers
{
    public class ProcessingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProcessingController> _logger;

        // Constructor with ApplicationDbContext and ILogger parameters
        public ProcessingController(ApplicationDbContext context, ILogger<ProcessingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// This method retrieves a list of claims from the database and passes them to the Processing view.
        /// <remarks>
        /// Hassan, Z.U. (2024). Action Result In ASP.NET MVC. [online] C-sharpcorner.com. Available at: https://www.c-sharpcorner.com/article/action-result-in-asp-net-mvc/.
        /// </remarks>
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Log information about the action
            _logger.LogInformation("Processing Index action called");
            var claims = await _context.Claims.ToListAsync();

            // Log the number of claims retrieved from the database
            if (claims == null || !claims.Any())
            {
                // Log a warning if no claims are found
                _logger.LogWarning("No claims found in the database"); 
            }
            else
            {
                // Log the number of claims retrieved
                _logger.LogInformation($"Retrieved {claims.Count} claims from the database"); 
            }
            return View(claims);
        }

        // POST: Processing/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int claimId, string status)
        {
            // Find the claim by ID
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null)
            {
                // Return an error message if the claim is not found
                return Json(new { success = false, message = "Claim not found" });
            }

            // Update the claim status if the status is valid
            if (Enum.TryParse<ClaimStatus>(status, out var claimStatus))
            {
                claim.Status = claimStatus;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                // Return an error message if the status is invalid
                return Json(new { success = false, message = "Invalid status" });
            }
        }
    }
}

// --------------------------------------------**End of File**--------------------------------------------------------