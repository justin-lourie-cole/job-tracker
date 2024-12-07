using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobHuntBackend.Data;
using JobHuntBackend.Models.Jobs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobHuntBackend.Controllers
{
  [Route("api/jobs/{jobId}/[controller]")]
  [ApiController]
  [Authorize]
  public class InterviewsController : ControllerBase
  {
    private readonly JobHuntDbContext _context;

    public InterviewsController(JobHuntDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Interview>>> GetInterviews(int jobId)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId && j.UserId == userId);

      if (job == null)
      {
        return NotFound("Job not found");
      }

      return await _context.Interviews
          .Where(i => i.JobId == jobId)
          .OrderBy(i => i.ScheduledAt)
          .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Interview>> GetInterview(int jobId, int id)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var interview = await _context.Interviews
          .Include(i => i.Job)
          .FirstOrDefaultAsync(i => i.Id == id && i.JobId == jobId && i.Job.UserId == userId);

      if (interview == null)
      {
        return NotFound();
      }

      return interview;
    }

    [HttpPost]
    public async Task<ActionResult<Interview>> CreateInterview(int jobId, Interview interview)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId && j.UserId == userId);

      if (job == null)
      {
        return NotFound("Job not found");
      }

      interview.JobId = jobId;
      _context.Interviews.Add(interview);

      // Update job status and next interview date
      if (job.Status == JobStatus.Applied)
      {
        job.Status = JobStatus.Interview;
      }
      job.NextInterviewDate = interview.ScheduledAt;

      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetInterview),
          new { jobId = jobId, id = interview.Id }, interview);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInterview(int jobId, int id, Interview interview)
    {
      if (id != interview.Id || jobId != interview.JobId)
      {
        return BadRequest();
      }

      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var existingInterview = await _context.Interviews
          .Include(i => i.Job)
          .FirstOrDefaultAsync(i => i.Id == id && i.Job.UserId == userId);

      if (existingInterview == null)
      {
        return NotFound();
      }

      _context.Entry(existingInterview).CurrentValues.SetValues(interview);

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!InterviewExists(id))
        {
          return NotFound();
        }
        throw;
      }

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInterview(int jobId, int id)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var interview = await _context.Interviews
          .Include(i => i.Job)
          .FirstOrDefaultAsync(i => i.Id == id && i.JobId == jobId && i.Job.UserId == userId);

      if (interview == null)
      {
        return NotFound();
      }

      _context.Interviews.Remove(interview);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool InterviewExists(int id)
    {
      return _context.Interviews.Any(e => e.Id == id);
    }
  }
}