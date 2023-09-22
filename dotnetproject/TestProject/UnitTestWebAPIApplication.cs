using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetapiapp.Controllers;
using dotnetapiapp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetapiapp.Tests
{
     [TestFixture]
    public class ApplicationControllerTests
    {
        private ApplicationController _ApplicationController;
        private JobApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<JobApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new JobApplicationDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Applications.AddRange(new List<Application>
            {
new Application { ApplicationID = 1, ApplicationName = "Applicant 1", ContactNumber = "9876543210", MailID = "mymail1@gmail.com",JobTitle="HR" },
new Application { ApplicationID = 2, ApplicationName = "Applicant 2", ContactNumber = "9876543217", MailID = "mymail2@gmail.com",JobTitle="IT Admin" },
new Application { ApplicationID = 3, ApplicationName = "Applicant 3", ContactNumber = "9876543216", MailID = "mymail3@gmail.com",JobTitle="HR" }
            });
            _context.SaveChanges();

            _ApplicationController = new ApplicationController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void ApplicationClassExists()
        {
            // Arrange
            Type ApplicationType = typeof(Application);

            // Act & Assert
            Assert.IsNotNull(ApplicationType, "Application class not found.");
        }
        [Test]
        public void Application_Properties_ApplicationName_ReturnExpectedDataTypes()
        {
            // Arrange
            Application application = new Application();
            PropertyInfo propertyInfo = application.GetType().GetProperty("ApplicationName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "ApplicationName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "ApplicationName property type is not string.");
        }
[Test]
        public void Application_Properties_JobTitle_ReturnExpectedDataTypes()
        {
            // Arrange
            Application application = new Application();
            PropertyInfo propertyInfo = application.GetType().GetProperty("JobTitle");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "JobTitle property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "JobTitle property type is not string.");
        }

        [Test]
        public async Task GetAllApplications_ReturnsOkResult()
        {
            // Act
            var result = await _ApplicationController.GetAllApplications();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllApplications_ReturnsAllApplications()
        {
            // Act
            var result = await _ApplicationController.GetAllApplications();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Application>>(okResult.Value);
            var applications = okResult.Value as IEnumerable<Application>;

            var ApplicationCount = applications.Count();
            Assert.AreEqual(3, ApplicationCount); // Assuming you have 3 Applications in the seeded data
        }


        [Test]
        public async Task AddApplication_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newApplication = new Application
            {
ApplicationName = "Applicant New", ContactNumber = "9877743210", MailID = "newmymail1@gmail.com",JobTitle="HR"
            };

            // Act
            var result = await _ApplicationController.AddApplication(newApplication);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteApplication_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new ApplicationsController(context);

                // Act
                var result = await _ApplicationController.DeleteApplication(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteApplication_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _ApplicationController.DeleteApplication(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Application id", result.Value);
        }
    }
}
