using TaskManagement.Business.Models;
using TaskManagement.DataAccess.Entities;

namespace TaskManagement.DataAccess.Services
{
    public interface ITaskItemRepository
    {
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<TaskItem> UpdateTaskAsync(int id, TaskItemDto taskItemDto);
        Task DeleteTask(int id);
        Task<TaskItem?> GetTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetTasksAsync();
        Task<IEnumerable<TaskItem>> SearchTaskAync(int? userId, string? searchQuery, int pageNumber, int pageSize);
        Task<TaskItem> AssignTaskAsync(int id, User user);
        Task<bool> SaveChangesAsync();
    }
}
