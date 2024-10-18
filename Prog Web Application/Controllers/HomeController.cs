///<summary>
/// This is a standard home controller it is responsible for rendering the views for the home page, 
/// new claim page, processing page, new user page and error page.
/// It also has a method to get the claims from the processing controller and pass them to the processing view.
/// The processing view will display the claims in a table.
///</summary>

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prog_Web_Application.Models;
using Prog_Web_Application.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Prog_Web_Application.Controllers;

public class HomeController : Controller
{
    // Add a logger and a ProcessingController to the HomeController
    private readonly ILogger<HomeController> _logger;
    private readonly ProcessingController _processingController;

    // Constructor with ILogger and ProcessingController parameters
    public HomeController(ILogger<HomeController> logger, ProcessingController processingController)
    {
        _logger = logger;
        _processingController = processingController;
    }

    // GET: Home/Index
    public IActionResult Index()
    {
        return View();
    }

    // GET: Home/NewClaim
    public IActionResult NewClaim()
    {
        return View();
    }

    // GET: Home/Processing
    public async Task<IActionResult> Processing()
    {
        // Get the claims from the ProcessingController
        var result = await _processingController.Index() as ViewResult;
        var claims = result?.Model as IEnumerable<Claim>;

        // Pass the claims to our Processing view
        return View(claims);
    }

    // GET: Home/NewUser
    public IActionResult NewUser()
    {
        return View();
    }

    // GET: Home/Error
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

// --------------------------------------------**End of File**--------------------------------------------------------