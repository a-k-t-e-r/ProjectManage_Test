using ProjManagAppForOpteam.Models;

namespace ProjManagAppForOpteam.Model;

public class TaskComment
{
    public int CommentId { get; set; }
    public string CommentText { get; set; } = string.Empty;
    public string CommentedBy { get; set; } = string.Empty;
    public DateTime CommentedAt { get; set; }

    public int TaskId { get; set; }
    public ProjectTask Task { get; set; }
}