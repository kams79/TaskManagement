using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Business.Models;
using TaskManagement.DataAccess.DbContexts;
using TaskManagement.DataAccess.Entities;
using TaskManagement.DataAccess.Services;
using Xunit;

namespace TaskManagement.Test.Repositories
{
    public class TaskItemRepositoryTests
    {
        private readonly Mock<TaskItemContext> _mockContext;
        private readonly TaskItemRepository _repository;

        public TaskItemRepositoryTests()
        {
            _mockContext = new Mock<TaskItemContext>(new DbContextOptions<TaskItemContext>());
            _repository = new TaskItemRepository(_mockContext.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldAddTask()
        {
            var task = new TaskItem { Title = "Test Task", Description = "Test Description" };
            var mockSet = new Mock<DbSet<TaskItem>>();
            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            await _repository.CreateTaskAsync(task);

            mockSet.Verify(m => m.AddAsync(task, default), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_ShouldRemoveTask()
        {
            var task = new TaskItem { TaskItemId = 1, Title = "Test Task", Description = "Test Description" };
            var mockSet = new Mock<DbSet<TaskItem>>();
            mockSet.Setup(m => m.FirstOrDefault(It.IsAny<Func<TaskItem, bool>>())).Returns(task);
            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            await _repository.DeleteTask(1);

            mockSet.Verify(m => m.Remove(task), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetTaskAsync_ShouldReturnTask()
        {
            var task = new TaskItem { TaskItemId = 1, Title = "Test Task", Description = "Test Description" };
            var mockSet = new Mock<DbSet<TaskItem>>();
            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);
            mockSet.Setup(m => m.IgnoreQueryFilters()).Returns(mockSet.Object);
            mockSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<TaskItem, bool>>(), default)).ReturnsAsync(task);
            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var result = await _repository.GetTaskAsync(1);

            Assert.Equal(task, result);
        }

        [Fact]
        public async Task GetTasksAsync_ShouldReturnTasks()
        {
            var tasks = new List<TaskItem>
            {
                new TaskItem { TaskItemId = 1, Title = "Test Task 1", Description = "Test Description 1" },
                new TaskItem { TaskItemId = 2, Title = "Test Task 2", Description = "Test Description 2" }
            };
            var mockSet = new Mock<DbSet<TaskItem>>();
            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);
            mockSet.Setup(m => m.ToListAsync(default)).ReturnsAsync(tasks);
            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var result = await _repository.GetTasksAsync();

            Assert.Equal(tasks, result);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTask()
        {
            var task = new TaskItem { TaskItemId = 1, Title = "Test Task", Description = "Test Description" };
            var taskDto = new TaskItemDto { Title = "Updated Task", Description = "Updated Description", DueDate = DateTime.Now, Priority = Priority.High, Status = Status.InProgress };
            var mockSet = new Mock<DbSet<TaskItem>>();
            mockSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<TaskItem, bool>>(), default)).ReturnsAsync(task);
            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var result = await _repository.UpdateTaskAsync(1, taskDto);

            Assert.Equal(taskDto.Title, result.Title);
            Assert.Equal(taskDto.Description, result.Description);
            Assert.Equal(taskDto.DueDate, result.DueDate);
            Assert.Equal(taskDto.Priority, result.Priority);
            Assert.Equal(taskDto.Status, result.Status);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SearchTaskAync_ShouldReturnFilteredTasks()
        {
            var tasks = new List<TaskItem>
            {
                new TaskItem { TaskItemId = 1, Title = "Test Task 1", Description = "Test Description 1", TaskOwner = new User { UserId = 1 } },
                new TaskItem { TaskItemId = 2, Title = "Test Task 2", Description = "Test Description 2", TaskOwner = new User { UserId = 2 } }
            }.AsQueryable();
            var mockSet = new Mock<DbSet<TaskItem>>();
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.Provider).Returns(tasks.Provider);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.Expression).Returns(tasks.Expression);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());
            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var result = await _repository.SearchTaskAync(1, "Test", 1, 10);

            Assert.Single(result);
        }

        [Fact]
        public async Task AssignTaskAsync_ShouldAssignUserToTask()
        {
            var task = new TaskItem { TaskItemId = 1, Title = "Test Task", Description = "Test Description" };
            var user = new User { UserId = 1, Username = "TestUser", Email = "test@example.com" };
            var mockSet = new Mock<DbSet<TaskItem>>();
            mockSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<TaskItem, bool>>(), default)).ReturnsAsync(task);
            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var result = await _repository.AssignTaskAsync(1, user);

            Assert.Equal(user, result.TaskOwner);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldReturnTrue_WhenChangesSaved()
        {
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _repository.SaveChangesAsync();

            Assert.True(result);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldReturnFalse_WhenNoChangesSaved()
        {
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(0);

            var result = await _repository.SaveChangesAsync();

            Assert.False(result);
        }
    }
}
