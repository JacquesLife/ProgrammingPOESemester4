using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using System.Threading.Tasks;

namespace Prog_Web_Application.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User/NewUser
        public IActionResult NewUser()
        {
            return View("/Views/Home/NewUser.cshtml"); 
        }

      // POST: User/CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid) // Validate the model
            {
                // Check if the email is already in use
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);
                
                if (existingUser != null)
                {
                    // Add a model error if the email is already in use
                    ModelState.AddModelError("Email", "Email is already in use."); 
                }
                else
                {
                    // Add the new user to the database
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync(); 

                    // Redirect to the Index action of the Home controller if successful
                    return RedirectToAction("Index", "Home"); 
                }
            }
            // Return the NewUser view with the user model if validation fails
            return View("/Views/Home/NewUser.cshtml", user); 
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            // Get all users from the database
            var users = await _context.Users.ToListAsync(); 
            return View("/Views/Home/Index.cshtml", users); 
        }
    }
}
