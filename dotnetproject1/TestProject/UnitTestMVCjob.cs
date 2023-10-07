using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnetmvcapp.Controllers;
using dotnetmvcapp.Models;
using dotnetmvcapp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;

namespace dotnetmvcapp.Tests
{
     [TestFixture]
    public class JobControllerTests
    {
        private Mock<IJobService> mockJobService;
        private JobController controller;
        [SetUp]
        public void Setup()
        {
            mockJobService = new Mock<IJobService>();
            controller = new JobController(mockJobService.Object);
        }

        [Test]
        public void AddJob_ValidData_SuccessfulAddition_RedirectsToIndex()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            mockJobService.Setup(service => service.AddJob(It.IsAny<Job>())).Returns(true);
            var controller = new JobController(mockJobService.Object);
            var job = new Job(); // Provide valid Job data

            // Act
            var result = controller.AddJob(job) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public void AddJob_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            var controller = new JobController(mockJobService.Object);
            Job invalidJob = null; // Invalid Job data

            // Act
            var result = controller.AddJob(invalidJob) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Job data", result.Value);
        }
        [Test]
        public void AddJob_FailedAddition_ReturnsViewWithModelError()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            mockJobService.Setup(service => service.AddJob(It.IsAny<Job>())).Returns(false);
            var controller = new JobController(mockJobService.Object);
            var job = new Job(); // Provide valid Job data

            // Act
            var result = controller.AddJob(job) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            // Check for expected model state error
            Assert.AreEqual("Failed to add the Job. Please try again.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }


        [Test]
        public void AddJob_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            mockJobService.Setup(service => service.AddJob(It.IsAny<Job>())).Returns(true);
            var controller = new JobController(mockJobService.Object);
            var job = new Job();

            // Act
            var result = controller.AddJob(job) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void AddJob_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            var controller = new JobController(mockJobService.Object);
            controller.ModelState.AddModelError("error", "Error");
            var job = new Job();

            // Act
            var result = controller.AddJob(job) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(job, result.Model);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            mockJobService.Setup(service => service.GetAllJobs()).Returns(new List<Job>());
            var controller = new JobController(mockJobService.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void Delete_ValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            mockJobService.Setup(service => service.DeleteJob(1)).Returns(true);
            var controller = new JobController(mockJobService.Object);

            // Act
            var result = controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void Delete_InvalidId_ReturnsViewResult()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            mockJobService.Setup(service => service.DeleteJob(1)).Returns(false);
            var controller = new JobController(mockJobService.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
