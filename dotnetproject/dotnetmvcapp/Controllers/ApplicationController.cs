using Microsoft.AspNetCore.Mvc;
using BookStoreApp.Models;
using BookStoreApp.Services;
using System;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _ApplicationService;

        public ApplicationController(IApplicationService ApplicationService)
        {
            _ApplicationService = ApplicationService;

        }

        public IActionResult AddApplication()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddApplication(Application application)
        {
            try
            {
                if (application == null)
                {
                    return BadRequest("Invalid Application data");
                }

                var success = _ApplicationService.AddApplication(application);

                if (success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Failed to add the Application. Please try again.");
                return View(application);
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                // Return an error response or another appropriate response
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
                return View(application);
            }
        }

        public IActionResult Index()
        {
            try
            {
                var listApplications = _ApplicationService.GetAllApplications();
                return View("Index", listApplications);
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error view or another appropriate response
                return View("Error"); // Assuming you have an "Error" view
            }
        }
        public IActionResult Delete(int id)
        {
            try
            {
                var success = _ApplicationService.DeleteApplication(id);

                if (success)
                {
                    // Check if the deletion was successful and return a view or a redirect
                    return RedirectToAction("Index"); // Redirect to the list of Applications, for example
                }
                else
                {
                    // Handle other error cases
                    return View("Error"); // Assuming you have an "Error" view
                }
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error view or another appropriate response
                return View("Error"); // Assuming you have an "Error" view
            }
        }
    }
}
