namespace ProjManagAppForOpteam.Models;

public class Project
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double Budget { get; set; }
    public string Owner { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }  // Not Started, In Progress, Completed

    // Navigation property
    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

}