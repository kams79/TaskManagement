using Microsoft.EntityFrameworkCore;
using TaskManagement.Entities;
using TaskManagement.Models;

namespace TaskManagement.DbContexts
{
    public class TaskItemContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public TaskItemContext(DbContextOptions<TaskItemContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // populate default data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId =  -1,
                    Username = "Not Assigned",
                    Email = "Not Assigned"
                }
            );

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    TaskItemId = 1,
                    Title = "Task 1",
                    Description = "Description for Task 1",
                    DueDate = new DateTime(2025, 3, 8, 10, 32, 32, 507, DateTimeKind.Local).AddTicks(5223),
                    Priority = Priority.Medium,
                    Status = Status.None,
                    UserId = -1
                },
                new TaskItem
                {
                    TaskItemId = 2,
                    Title = "Task 2",
                    Description = "Description for Task 2",
                    DueDate = new DateTime(2025, 3, 15, 10, 32, 32, 508, DateTimeKind.Local).AddTicks(7773),
                    Priority = Priority.High,
                    Status = Status.None,
                    UserId = -1
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
