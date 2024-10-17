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

        [TestMethod]
        public async Task CreateClaim_ValidClaim_ReturnsRedirectToAction()
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

            // Mock the uploaded file if needed
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


        [TestMethod]
        public async Task CreateClaim_InvalidFileType_ReturnsViewResult()
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
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey("uploadedFile"));
        }

        [TestMethod]
        public async Task CreateClaim_NoFileUploaded_ReturnsRedirectToAction()
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
            Assert.AreEqual("Test claim description", ((Claim)viewResult.Model).ClaimDescription); 
        }
    }
}