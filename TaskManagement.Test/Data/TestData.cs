using FluentValidation.Results;
using System.Collections.Generic;
using TaskManagement.Business.Models;
using TaskManagement.DataAccess.Entities;

namespace TaskManagement.Test.Data
{
    public static class TestData
    {
        public static TaskItemDto ValidTaskDto => new TaskItemDto
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = Priority.Medium,
            Status = Status.Open
        };

        public static TaskItemDto UpdatedTaskDto => new TaskItemDto
        {
            Title = "Updated Task",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow,
            Priority = Priority.High,
            Status = Status.InProgress
        };

        public static TaskItem ValidTask => new TaskItem
        {
            TaskItemId = 1,
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = Priority.Medium,
            Status = Status.Open
        };

        public static TaskItemDetailsDto ValidTaskDetailsDto => new TaskItemDetailsDto
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = "Medium",
            Status = "Open"
        };

        public static List<TaskItem> ValidTasks => new List<TaskItem>
        {
            new TaskItem
            {
                TaskItemId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = Priority.Medium,
                Status = Status.Open
            }
        };

        public static List<TaskItemDetailsDto> ValidTaskDetailsDtos => new List<TaskItemDetailsDto>
        {
            new TaskItemDetailsDto
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = "Medium",
                Status = "Open"
            }
        };

        public static UserWithoutTaskDto ValidUserWithoutTaskDto => new UserWithoutTaskDto
        {
            UserId = 1,
            Username = "TestUser",
            Email = "test@example.com"
        };

        public static User ValidUser => new User
        {
            UserId = 1,
            Username = "TestUser",
            Email = "test@example.com"
        };

        public static ValidationResult InvalidValidationResult => new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Error")
        });
    }
}
