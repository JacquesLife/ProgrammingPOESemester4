using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Controllers;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Prog_Web_Application.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private ApplicationDbContext _context;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _controller = new UserController(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task CreateUser_ValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var user = new User
            {
                Name = "John Doe",
                Username = "johndoe123",
                Password = "SecurePassword123!",
                Email = "johndoe@example.com",
                Phone = "1234567890",
                Role = "Lecturer"
            };

            // Act
            var result = await _controller.CreateUser(user);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
        
    
        [TestMethod]
        public async Task CreateUser_Duplicate_Email_Returns_Error()
        {
            // Arrange
            var existingUser = new User
            {
                Name = "Existing User",
                Username = "existinguser",
                Password = "SecurePassword123!",
                Email = "test@example.com",
                Phone = "1234567890",
                Role = "Lecturer"
            };
            await _context.Users.AddAsync(existingUser);
            await _context.SaveChangesAsync();

            var newUser = new User
            {
                Name = "New User",
                Username = "newuser",
                Password = "SecurePassword123!",
                Email = "test@example.com", // Duplicate email
                Phone = "0987654321",
                Role = "Lecturer"
            };

            // Act
            var result = await _controller.CreateUser(newUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual("/Views/Home/NewUser.cshtml", viewResult.ViewName);
            Assert.IsFalse(_controller.ModelState.IsValid); 
            Assert.IsTrue(_controller.ModelState["Email"].Errors.Any()); 
        }
    }
}