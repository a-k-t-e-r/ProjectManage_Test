using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjManagAppForOpteam.Models;
using ProjManagAppForOpteam.Repositories;

namespace ProjManagAppForOpteam.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController(ProjectManagementContext context) : ControllerBase
{
    private readonly ProjectManagementContext _context = context;

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var projects = await _context.Projects
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            return Ok(projects);
        }
        catch
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        try
        {
            var project = await _context.Projects.FindAsync(id);

            if (project is null)
                return NotFound();

            return project;
        }
        catch
        {
            throw;
        }
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        try
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
        }
        catch
        {
            throw;
        }
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, Project project)
    {
        if (id != project.ProjectId)
            return BadRequest();

        _context.Entry(project).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProjectExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
            return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<ProjectTask>>> GetOverdueTasks()
    {
        try
        {
            var overdueTasks = await _context.Tasks
                                               .Where(t => t.EndDate < DateTime.Now && t.Status != Models.TaskStatus.Completed)
                                               .ToListAsync();

            if (overdueTasks.Count == 0)
                return NoContent();

            return Ok(overdueTasks);
        }
        catch
        {
            throw;
        }
    }

    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }
}