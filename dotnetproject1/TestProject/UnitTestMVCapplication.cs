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
    public class ApplicationControllerTests
    {
        private Mock<IApplicationService> mockApplicationService;
        private ApplicationController controller;
        [SetUp]
        public void Setup()
        {
            mockApplicationService = new Mock<IApplicationService>();
            controller = new ApplicationController(mockApplicationService.Object);
        }

        [Test]
        public void AddApplication_ValidData_SuccessfulAddition_RedirectsToIndex()
        {
            // Arrange
            var mockApplicationService = new Mock<IApplicationService>();
            mockApplicationService.Setup(service => service.AddApplication(It.IsAny<Application>())).Returns(true);
            var controller = new ApplicationController(mockApplicationService.Object);
            var application = new Application(); // Provide valid Application data

            // Act
            var result = controller.AddApplication(application) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public void AddApplication_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockApplicationService = new Mock<IApplicationService>();
            var controller = new ApplicationController(mockApplicationService.Object);
            Application invalidApplication = null; // Invalid Application data

            // Act
            var result = controller.AddApplication(invalidApplication) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Application data", result.Value);
        }
        [Test]
        public void AddApplication_FailedAddition_ReturnsViewWithModelError()
        {
            // Arrange
            var mockApplicationService = new Mock<IApplicationService>();
            mockApplicationService.Setup(service => service.AddApplication(It.IsAny<Application>())).Returns(false);
            var controller = new ApplicationController(mockApplicationService.Object);
            var application = new Application(); // Provide valid Application data

            // Act
            var result = controller.AddApplication(application) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            // Check for expected model state error
            Assert.AreEqual("Failed to add the Application. Please try again.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }


        [Test]
        public void AddApplication_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockApplicationService = new Mock<IApplicationService>();
            mockApplicationService.Setup(service => service.AddApplication(It.IsAny<Application>())).Returns(true);
            var controller = new ApplicationController(mockApplicationService.Object);
            var application = new Application();

            // Act
            var result = controller.AddApplication(application) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void AddApplication_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var mockApplicationService = new Mock<IApplicationService>();
            var controller = new ApplicationController(mockApplicationService.Object);
            controller.ModelState.AddModelError("error", "Error");
            var application = new Application();

            // Act
            var result = controller.AddApplication(application) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(application, result.Model);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockApplicationService = new Mock<IApplicationService>();
            mockApplicationService.Setup(service => service.GetAllApplications()).Returns(new List<Application>());
            var controller = new ApplicationController(mockApplicationService.Object);

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
            var mockApplicationService = new Mock<IApplicationService>();
            mockApplicationService.Setup(service => service.DeleteApplication(1)).Returns(true);
            var controller = new ApplicationController(mockApplicationService.Object);

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
            var mockApplicationService = new Mock<IApplicationService>();
            mockApplicationService.Setup(service => service.DeleteApplication(1)).Returns(false);
            var controller = new ApplicationController(mockApplicationService.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
