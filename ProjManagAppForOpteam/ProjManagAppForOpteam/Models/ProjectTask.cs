using ProjManagAppForOpteam.Model;

namespace ProjManagAppForOpteam.Models;

public class ProjectTask
{
    public int TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Priority { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }  // Not Started, In Progress, Completed

    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    // Navigation property
    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();

}