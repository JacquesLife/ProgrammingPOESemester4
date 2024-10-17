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
                    ModelState.AddModelError("Email", "Email is already in use."); // Add error for duplicate email
                }
                else
                {
                    _context.Users.Add(user); // Add the user to the DbContext
                    await _context.SaveChangesAsync(); // Save changes to the database
                    return RedirectToAction("Index", "Home"); // Redirect to the home page after successful creation
                }
            }

            return View("/Views/Home/NewUser.cshtml", user); // If model state is invalid, return to the same view
        }

        // Optionally, you can add a method to list users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync(); 
            return View("/Views/Home/Index.cshtml", users); 
        }
    }
}
