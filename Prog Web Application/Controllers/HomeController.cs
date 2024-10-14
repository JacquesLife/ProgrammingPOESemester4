using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prog_Web_Application.Models;
using Prog_Web_Application.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Prog_Web_Application.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProcessingController _processingController;

    public HomeController(ILogger<HomeController> logger, ProcessingController processingController)
    {
        _logger = logger;
        _processingController = processingController;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult NewClaim()
    {
        return View();
    }

    public async Task<IActionResult> Processing()
    {
        // Get the claims from the ProcessingController
        var result = await _processingController.Index() as ViewResult;
        var claims = result?.Model as IEnumerable<Claim>;
        
        // Pass the claims to our Processing view
        return View(claims);
    }

    public IActionResult NewUser()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
