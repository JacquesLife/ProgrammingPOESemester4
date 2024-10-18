///<summary
/// This file contains the test methods for the ClaimController class. 
/// It tests the CreateClaim action method with different scenarios.
/// Moq is used to mock the IFormFile object for testing file uploads. 
/// The test are initialized with an in-memory database and the controller is tested with valid and invalid models.
/// Tests are cleaned up by deleting the in-memory database after each test.
/// <remarks>
/// Keep it simple, stupid. (2023). Unit testing in C# .NET with MSTest & Moq. [online] YouTube. Available at: https://www.youtube.com/watch?v=7UFjv_l0nfo.
/// ncarandini (2023). Unit testing C# with MSTest and .NET - .NET. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest.
/// <remarks>
///<summary

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Prog_Web_Application.Controllers;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;

namespace Prog_Web_Application.Tests
{
    [TestClass]
    public class ClaimControllerTests
    {
        private ApplicationDbContext _context;
        private ClaimController _controller;
        private Mock<ILogger<ClaimController>> _loggerMock;

        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ClaimTestDatabase")
                .Options;
    
            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<ClaimController>>();
            _controller = new ClaimController(_context, _loggerMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // Test method for the CreateClaim action method
        [TestMethod]
        public async Task Create_ValidClaim_RedirectsToIndex()
        {
            // Arrange
            var claim = new Claim
            {
                FullName = "John Doe",
                Username = "johndoe",
                Email = "johndoe@example.com",
                Phone = "1234567890",
                ClaimDescription = "Claim for testing",
                HoursWorked = 10,
                HourlyRate = 50,
                submissionDate = DateTime.UtcNow,
                FileName = "test.pdf"
            };

            // Mock the IFormFile object for testing file uploads
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.pdf");
            fileMock.Setup(f => f.Length).Returns(1024); // 1 KB file size
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateClaim(claim, fileMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Test method for the CreateClaim action method with an invalid file type
        [TestMethod]
        public async Task CreateClaim_InvalidFileType_ReturnsView()
        {
            // Arrange
            var claim = new Claim
            {
                FullName = "John Doe",
                Email = "johndoe@example.com",
                ClaimDescription = "Claim for testing",
                HoursWorked = 10,
                HourlyRate = 50
            };

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.exe"); // Invalid file type
            fileMock.Setup(f => f.Length).Returns(1024); // 1 KB
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateClaim(claim, fileMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual("/Views/Home/NewClaim.cshtml", viewResult.ViewName);
            // Check if the ModelState contains an error for the uploaded file
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey("uploadedFile"));
        }

        // Test method for the CreateClaim action method with a file size exceeding the limit
        [TestMethod]
        public async Task CreateClaim_NoFileUploaded_ReturnsRedirectToIndex()
        {
            // Arrange
            var controller = new ClaimController(_context, _loggerMock.Object);
            var claim = new Claim
            {
                FullName = "John Doe",
                Username = "johndoe",
                Email = "johndoe@example.com",
                Phone = "1234567890",
                ClaimDescription = "Test claim",
                HoursWorked = 10,
                HourlyRate = 50.0m
            };

            // Act
            var result = await controller.CreateClaim(claim, null); 

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        // Test method for the CreateClaim action method with a file size exceeding the limit
        [TestMethod]
        public async Task CreateClaim_FileSizeExceedsLimit_ReturnsViewResult()
        {
            // Arrange
            var claim = new Claim
            {
                FullName = "John Doe",
                Username = "johndoe",
                Email = "johndoe@example.com",
                Phone = "1234567890",
                ClaimDescription = "Test claim description",
                HoursWorked = 10,
                HourlyRate = 100
            };

            // Create a mock file with a size larger than the allowed limit
            var fileMock = new Mock<IFormFile>();
            var fileName = "largefile.pdf";
            var fileSize = 15 * 1024 * 1024; // 15 MB
            var stream = new MemoryStream(new byte[fileSize]); // Create a stream with 15 MB of data

            fileMock.Setup(f => f.Length).Returns(fileSize);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((s, c) => stream.CopyTo(s));

            // Create a new instance of the controller and pass in the necessary dependencies
            var controller = new ClaimController(_context, _loggerMock.Object);

            // Act
            var result = await controller.CreateClaim(claim, fileMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Expected a ViewResult to be returned.");
            var viewResult = (ViewResult)result;
            // Check if the view name is correct
            Assert.AreEqual("Test claim description", ((Claim)viewResult.Model).ClaimDescription); 
        }
    }
}

// --------------------------------------------**End of File**--------------------------------------------------------