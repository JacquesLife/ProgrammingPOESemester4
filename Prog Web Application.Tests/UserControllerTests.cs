///<summary
/// This file contains the test methods for the UserController class.
/// It tests the CreateUser action method with different scenarios.
/// The test are intialised with an in-memory database and the controller is tested with valid and invalid models.
/// Tests are cleaned up by deleting the in-memory database after each test.
/// <remarks>
/// Keep it simple, stupid. (2023). Unit testing in C# .NET with MSTest & Moq. [online] YouTube. Available at: https://www.youtube.com/watch?v=7UFjv_l0nfo.
/// ncarandini (2023). Unit testing C# with MSTest and .NET - .NET. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest.
/// <remarks>
///<summary

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

        // Test method for the CreateUser action method
        [TestMethod]
        public async Task CreateUser_ValidModel_ReturnsRedirectToIndex()
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
            // Check if the action name is "Index" and the controller name is "Home"
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
        
        // Test method for the CreateUser action method with duplicate email
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
            // Check if the ModelState contains an error for the email field
            Assert.IsTrue(_controller.ModelState["Email"].Errors.Any()); 
        }
    }
}

// --------------------------------------------**End of File**--------------------------------------------------------