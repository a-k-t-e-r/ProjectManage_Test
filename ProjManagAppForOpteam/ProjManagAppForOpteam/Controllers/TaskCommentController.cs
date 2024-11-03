using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjManagAppForOpteam.Model;
using ProjManagAppForOpteam.Repositories;

namespace ProjManagAppForOpteam.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskCommentController(ProjectManagementContext context) : ControllerBase
{
    private readonly ProjectManagementContext _context = context;

    [Authorize]
    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<TaskComment>>> GetCommentsForTask(int taskId)
    {
        return await _context.TaskComments
            .Where(c => c.TaskId == taskId)
            .ToListAsync();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TaskComment>> AddComment(TaskComment comment)
    {
        comment.CommentedAt = DateTime.Now;
        _context.TaskComments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCommentsForTask), new { taskId = comment.TaskId }, comment);
    }
}