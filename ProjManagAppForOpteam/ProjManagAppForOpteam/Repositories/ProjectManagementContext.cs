using Microsoft.EntityFrameworkCore;
using ProjManagAppForOpteam.Model;
using ProjManagAppForOpteam.Models;

namespace ProjManagAppForOpteam.Repositories
{
    public class ProjectManagementContext(DbContextOptions<ProjectManagementContext> options)
        : DbContext(options)
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Project entity
            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectId); // primary key

            modelBuilder.Entity<Project>()
                .Property(p => p.ProjectName)
                .IsRequired();

            // Configure ProjectTask entity
            modelBuilder.Entity<ProjectTask>()
                .HasKey(pt => pt.TaskId); // primary key

            modelBuilder.Entity<ProjectTask>()
                .Property(pt => pt.TaskName)
                .IsRequired();

            // Configure the relationship between Project and ProjectTask
            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(pt => pt.ProjectId); // foreign key

            // Configure TaskComment entity
            modelBuilder.Entity<TaskComment>()
                .HasKey(tc => tc.CommentId); // primary key

            modelBuilder.Entity<TaskComment>()
                .Property(tc => tc.CommentText)
                .IsRequired();

            // Configure the relationship between TaskComment and ProjectTask
            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Task)
                .WithMany(pt => pt.Comments)
                .HasForeignKey(tc => tc.TaskId); // foreign key

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId); // the primary key

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired(); // Example of required property

            // Configure the enum for TaskStatus
            modelBuilder.Entity<ProjectTask>()
                .Property(pt => pt.Status)
                .HasConversion<string>(); // Store TaskStatus as string in the database

            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>(); // Store TaskStatus as string in the database
        }
    }
}