using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Prog_Web_Application.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Database;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Prog_Web_Application.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<User> userManager,
            ILogger<UserController> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(UserViewModel model, string password)
        {
            if (ModelState.IsValid)
            {
                var validRoles = new[] { "Lecturer", "Academic Manager", "Program Coordinator" };
                if (model.Role == null || !validRoles.Contains(model.Role))
                {
                    model.Role = "Lecturer"; // Default to Lecturer
                }

                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View("/Views/Home/NewUser.cshtml", model);
                }

                var newUser = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.Phone ?? "",
                    FullName = model.FullName,
                    Role = model.Role
                };

                var result = await _userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    // Ensure the role exists
                    if (!await _roleManager.RoleExistsAsync(newUser.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(newUser.Role));
                    }

                    await _userManager.AddToRoleAsync(newUser, newUser.Role);
                    _logger.LogInformation("New user created: {Email}", newUser.Email);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("/Views/Home/NewUser.cshtml", model);
        }
    }
}