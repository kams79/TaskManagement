using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Business.Models;
using TaskManagement.Controllers;
using TaskManagement.DataAccess.Entities;
using TaskManagement.DataAccess.Services;
using TaskManagement.Test.Data;
using Xunit;

namespace TaskManagement.Test.ControllersTest
{
    public class TasksControllerTests
    {
        private readonly Mock<ILogger<TasksController>> _loggerMock;
        private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<TaskItemDto>> _validatorMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _loggerMock = new Mock<ILogger<TasksController>>();
            _taskItemRepositoryMock = new Mock<ITaskItemRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<TaskItemDto>>();
            _controller = new TasksController(_loggerMock.Object, _taskItemRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task CreateTask_ValidTask_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var taskDto = TestData.ValidTaskDto;
            var task = TestData.ValidTask;
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TaskItemDto>>(), default))
                .ReturnsAsync(new ValidationResult());
            _mapperMock.Setup(m => m.Map<TaskItem>(taskDto)).Returns(task);
            _taskItemRepositoryMock.Setup(r => r.CreateTaskAsync(task)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TaskItemDto>(task)).Returns(taskDto);

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TasksController.CreateTask), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task CreateTask_InvalidTask_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = TestData.ValidTaskDto;
            var validationResult = TestData.InvalidValidationResult;
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TaskItemDto>>(), default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateTask_ValidTask_ReturnsNoContent()
        {
            // Arrange
            var taskDto = TestData.UpdatedTaskDto;
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TaskItemDto>>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.UpdateTask(1, taskDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTask_InvalidTask_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = TestData.UpdatedTaskDto;
            var validationResult = TestData.InvalidValidationResult;
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TaskItemDto>>(), default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.UpdateTask(1, taskDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteTask_ValidId_ReturnsNoContent()
        {
            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetTask_ValidId_ReturnsOkObjectResult()
        {
            // Arrange
            var task = TestData.ValidTask;
            var taskDto = TestData.ValidTaskDetailsDto;
            _taskItemRepositoryMock.Setup(r => r.GetTaskAsync(1)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TaskItemDetailsDto>(task)).Returns(taskDto);

            // Act
            var result = await _controller.GetTask(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(taskDto, okResult.Value);
        }

        [Fact]
        public async Task GetTask_InvalidId_ReturnsNoContent()
        {
            // Arrange
            _taskItemRepositoryMock.Setup(r => r.GetTaskAsync(1)).ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _controller.GetTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task GetTasks_ReturnsOkObjectResult()
        {
            // Arrange
            var tasks = TestData.ValidTasks;
            var taskDtos = TestData.ValidTaskDetailsDtos;
            _taskItemRepositoryMock.Setup(r => r.GetTasksAsync()).ReturnsAsync(tasks);
            _mapperMock.Setup(m => m.Map<IEnumerable<TaskItemDetailsDto>>(tasks)).Returns(taskDtos);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(taskDtos, okResult.Value);
        }

        [Fact]
        public async Task GetTasks_WithSearchCriteria_ReturnsOkObjectResult()
        {
            // Arrange
            var tasks = TestData.ValidTasks;
            var taskDtos = TestData.ValidTaskDetailsDtos;
            _taskItemRepositoryMock.Setup(r => r.SearchTaskAync(null, null, 1, 10)).ReturnsAsync(tasks);
            _mapperMock.Setup(m => m.Map<IEnumerable<TaskItemDetailsDto>>(tasks)).Returns(taskDtos);

            // Act
            var result = await _controller.GetTasks(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(taskDtos, okResult.Value);
        }

        [Fact]
        public async Task AssignTask_ValidUser_ReturnsOkObjectResult()
        {
            // Arrange
            var userDto = TestData.ValidUserWithoutTaskDto;
            var user = TestData.ValidUser;
            var task = TestData.ValidTask;
            var taskDto = TestData.ValidTaskDetailsDto;
            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(user);
            _taskItemRepositoryMock.Setup(r => r.AssignTaskAsync(1, user)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TaskItemDetailsDto>(task)).Returns(taskDto);

            // Act
            var result = await _controller.AssignTask(1, userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(taskDto, okResult.Value);
        }

        [Fact]
        public async Task AssignTask_NullUser_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.AssignTask(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID cannot be null or empty.", badRequestResult.Value);
        }
    }
}
