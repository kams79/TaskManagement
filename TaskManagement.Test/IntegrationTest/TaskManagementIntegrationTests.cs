using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManagement.Business.Models;

public class TaskManagementIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TaskManagementIntegrationTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task GetAllTaskItems_ReturnsOkResponse()
    {
        var response = await _client.GetAsync("/api/tasks");
        response.EnsureSuccessStatusCode();
        var taskItems = await response.Content.ReadFromJsonAsync<IEnumerable<TaskItemDetailsDto>>();
        taskItems.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTaskItemById_ReturnsOkResponse()
    {
        var response = await _client.GetAsync("/api/tasks/1");
        response.EnsureSuccessStatusCode();
        var taskItem = await response.Content.ReadFromJsonAsync<TaskItemDetailsDto>();
        taskItem.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateTaskItem_ReturnsCreatedResponse()
    {
        var newTaskItem = new TaskItemDto { Title = "Test Task", Description = "Test Description", DueDate = DateTime.UtcNow.AddDays(1), Priority = Priority.Medium, Status = Status.Open };
        var response = await _client.PostAsJsonAsync("/api/tasks", newTaskItem);
        response.EnsureSuccessStatusCode();
        var createdTaskItem = await response.Content.ReadFromJsonAsync<TaskItemDto>();
        createdTaskItem.Should().NotBeNull();
        createdTaskItem.Title.Should().Be("Test Task");
    }

    [Fact]
    public async Task UpdateTaskItem_ReturnsNoContentResponse()
    {
        var updatedTaskItem = new TaskItemDto { Title = "Updated Task", Description = "Updated Description", DueDate = DateTime.UtcNow.AddDays(1), Priority = Priority.High, Status = Status.InProgress };
        var response = await _client.PutAsJsonAsync("/api/tasks/1", updatedTaskItem);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTaskItem_ReturnsNoContentResponse()
    {
        var response = await _client.DeleteAsync("/api/tasks/1");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}

