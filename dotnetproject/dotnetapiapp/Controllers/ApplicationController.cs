using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreDBFirst.Models;

namespace BookStoreDBFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly JobApplicationDbContext _context;

        public ApplicationController(JobApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetAllApplications()
        {
            var applications = await _context.Applications.ToListAsync();
            return Ok(applications);
        }

        [HttpPost]
        public async Task<ActionResult> AddApplication(Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return detailed validation errors
            }
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid Application id");

            var application = await _context.Applications.FindAsync(id);
              _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
