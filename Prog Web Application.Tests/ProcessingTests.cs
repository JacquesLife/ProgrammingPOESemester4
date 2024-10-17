using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Prog_Web_Application.Controllers;
using Prog_Web_Application.Database;
using Prog_Web_Application.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Prog_Web_Application.Tests
{
    [TestClass]
    public class ProcessingControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<ILogger<ProcessingController>> _loggerMock;
        private ProcessingController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ClaimTestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<ProcessingController>>();
            _controller = new ProcessingController(_context, _loggerMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // Test method for the Index action method
        [TestMethod]
        public async Task Index_ReturnsViewResult_WithClaims()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim
                {
                    ClaimID = 1,
                    FullName = "John Doe",
                    Username = "john_doe",
                    Email = "john@example.com",
                    Phone = "123-456-7890",
                    ClaimDescription = "Test claim 1",
                    Status = ClaimStatus.Pending
                },
                new Claim
                {
                    ClaimID = 2,
                    FullName = "Jane Doe",
                    Username = "jane_doe",
                    Email = "jane@example.com",
                    Phone = "098-765-4321",
                    ClaimDescription = "Test claim 2",
                    Status = ClaimStatus.Pending
                }
            };
            await _context.Claims.AddRangeAsync(claims);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<Claim>));
            var model = result.Model as List<Claim>;
            // Check if the model contains the expected number of claims
            Assert.AreEqual(2, model.Count);
        }
    }
}
