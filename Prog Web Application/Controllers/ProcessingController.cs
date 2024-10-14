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

        public ProcessingController(ApplicationDbContext context, ILogger<ProcessingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Processing
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Processing Index action called");
            var claims = await _context.Claims.ToListAsync();
            if (claims == null || !claims.Any())
            {
                _logger.LogWarning("No claims found in the database");
            }
            else
            {
                _logger.LogInformation($"Retrieved {claims.Count} claims from the database");
            }
            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int claimId, string status)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null)
            {
                return Json(new { success = false, message = "Claim not found" });
            }

            if (Enum.TryParse<ClaimStatus>(status, out var claimStatus))
            {
                claim.Status = claimStatus;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Invalid status" });
            }
        }
    }
}