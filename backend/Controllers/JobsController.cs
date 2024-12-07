using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobHuntBackend.Data;
using JobHuntBackend.Models.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace JobHuntBackend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  [EnableRateLimiting("GeneralLimit")]
  public class JobsController : ControllerBase
  {
    private readonly JobHuntDbContext _context;

    public JobsController(JobHuntDbContext context)
    {
      _context = context;
    }

    // GET: api/Jobs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      return await _context.Jobs
        .Where(job => job.UserId == userId)
        .ToListAsync();
    }

    // GET: api/Jobs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Job>> GetJob(int id)
    {
      var job = await _context.Jobs.FindAsync(id);

      if (job == null)
      {
        return NotFound();
      }

      return job;
    }

    // PUT: api/Jobs/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(int id, Job job)
    {
      if (id != job.Id)
      {
        return BadRequest();
      }

      _context.Entry(job).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!JobExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Jobs
    [HttpPost]
    public async Task<ActionResult<Job>> CreateJob(Job job)
    {
      job.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
          ?? throw new InvalidOperationException("User ID not found");
      _context.Jobs.Add(job);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetJob", new { id = job.Id }, job);
    }

    // DELETE: api/Jobs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
      var job = await _context.Jobs.FindAsync(id);
      if (job == null)
      {
        return NotFound();
      }

      _context.Jobs.Remove(job);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool JobExists(int id)
    {
      return _context.Jobs.Any(e => e.Id == id);
    }
  }
}
