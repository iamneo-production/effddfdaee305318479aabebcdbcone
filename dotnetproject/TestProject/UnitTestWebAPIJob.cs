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
    public class JobControllerTests
    {
        private JobController _JobController;
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
            _context.Jobs.AddRange(new List<Job>
            {
                new Job { JobID = 1, JobTitle = "Job 1", Department = "HR", Location = "Chennai",Responsibility="Job responsibility1",Qualification="BE",DeadLine = DateTime.Parse("2023-08-30"), },
                new Job { JobID = 2, JobTitle = "Job 2", Department = "Admin", Location = "Pune",Responsibility="Job responsibility2",Qualification="MBA",DeadLine=DateTime.Parse("2023-08-30") },
                new Job { JobID = 3, JobTitle = "Job 3", Department = "IT", Location = "Mumbai",Responsibility="Job responsibility3",Qualification="MSc",DeadLine=DateTime.Parse("2023-08-30") }
            });
            _context.SaveChanges();

            _JobController = new JobController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void JobClassExists()
        {
            // Arrange
            Type JobType = typeof(Job);

            // Act & Assert
            Assert.IsNotNull(JobType, "Job class not found.");
        }
        [Test]
        public void Job_Properties_JobTitle_ReturnExpectedDataTypes()
        {
            // Arrange
            Job job = new Job();
            PropertyInfo propertyInfo = job.GetType().GetProperty("JobTitle");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "JobTitle property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "JobTitle property type is not string.");
        }
[Test]
        public void Job_Properties_Department_ReturnExpectedDataTypes()
        {
            // Arrange
            Job job = new Job();
            PropertyInfo propertyInfo = job.GetType().GetProperty("Department");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "Department property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Department property type is not string.");
        }

        [Test]
        public async Task GetAllJobs_ReturnsOkResult()
        {
            // Act
            var result = await _JobController.GetAllJobs();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllJobs_ReturnsAllJobs()
        {
            // Act
            var result = await _JobController.GetAllJobs();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Job>>(okResult.Value);
            var Jobs = okResult.Value as IEnumerable<Job>;

            var JobCount = Jobs.Count();
            Assert.AreEqual(3, JobCount); // Assuming you have 3 Jobs in the seeded data
        }


        [Test]
        public async Task AddJob_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newJob = new Job
            {
JobTitle = "New Job Title", Department = "HR", Location = "Chennai",Responsibility="Job responsibility1",Qualification="BE",DeadLine=DateTime.Parse("2023-08-10")
            };

            // Act
            var result = await _JobController.AddJob(newJob);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteJob_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new JobsController(context);

                // Act
                var result = await _JobController.DeleteJob(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteJob_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _JobController.DeleteJob(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Job id", result.Value);
        }
    }
}
