using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjManagAppForOpteam.Models;
using ProjManagAppForOpteam.Repositories;

namespace ProjManagAppForOpteam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ProjectManagementContext _context;

        public TaskController(ProjectManagementContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> GetTasks(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tasks = await _context.Tasks
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

                return Ok(tasks);
            }
            catch
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> GetProjectTask(int id)
        {
            try
            {
                var task = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.TaskId == id);

                if (task is null)
                    return NotFound();

                return task;
            }
            catch
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("{projectId}/Tasks")]
        public async Task<ActionResult<List<ProjectTask>>> GetTasksByProjectId(int projectId)
        {
            try
            {
                var tasks = await _context.Tasks
                                            .Where(pt => pt.ProjectId == projectId)
                                            .Include(pt => pt.Project)
                                            .Select(pt => new
                                            {
                                                pt.TaskId,
                                                pt.TaskName,
                                                pt.Description,
                                                pt.AssignedTo,
                                                pt.StartDate,
                                                pt.EndDate,
                                                pt.Priority,
                                                pt.Status,
                                                pt.ProjectId
                                            })
                                            .ToListAsync();

                if (tasks is null || tasks.Count == 0)
                    return NotFound();

                return Ok(tasks);
            }
            catch
            {
                throw;
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult<ProjectTask>> CreateProjectTask(ProjectTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectTask), new { id = task.TaskId }, task);
        }

        [Authorize(Roles = "Manager,Employee")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectTask(int id, ProjectTask task)
        {
            if (id != task.TaskId)
                return BadRequest();

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTaskExists(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task is null)
                    return NotFound();

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                throw;
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("assign/{id}")]
        public async Task<IActionResult> AssignTaskToUser(int id, [FromBody] string assignedTo)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task is null) return NotFound();

            task.AssignedTo = assignedTo;
            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTaskExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        private bool ProjectTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}