using TaskManagement.Business.Models;
using TaskManagement.DataAccess.DbContexts;
using TaskManagement.DataAccess.Entities;

/// <summary>
/// Provides methods to seed initial data into the database context.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Initializes the database with seed data.
    /// </summary>
    /// <param name="context">The database context to seed data into.</param>
    public static void Initialize(TaskItemContext context)
    {
        context.Users.AddRange(
            new User
            {
                UserId = -1,
                Username = "Not Assigned",
                Email = "Not Assigned"
            }
        );

        context.Tasks.AddRange(
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
        context.SaveChanges();
    }
}
