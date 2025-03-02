
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Business.Models;
using TaskManagement.DataAccess.DbContexts;
using TaskManagement.DataAccess.Entities;

namespace TaskManagement.DataAccess.Services
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly TaskItemContext _context;

        public TaskItemRepository(TaskItemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            var entityEntry = await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task DeleteTask(int id)
        {
            var taskEntry = _context.Tasks.FirstOrDefault(task => task.TaskItemId == id)
                ?? throw new InvalidOperationException($"Task with Id {id} not found.");

            _context.Tasks.Remove(taskEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetTaskAsync(int id)
        {
            return await _context.Tasks.Where(task => task.TaskItemId == id).Include(a => a.TaskOwner).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            var taskEntries = await _context.Tasks.Include(a => a.TaskOwner).ToListAsync();

            return taskEntries;
        }

        public async Task<TaskItem> UpdateTaskAsync(int id, TaskItemDto taskItemDto)
        {
            var taskEntry = await _context.Tasks.Where(task => task.TaskItemId == id).FirstOrDefaultAsync() 
                ?? throw new KeyNotFoundException($"Task with Id {id} not found.");

            taskEntry.Title = taskItemDto.Title ?? throw new NullReferenceException();
            taskEntry.Description = taskItemDto.Description ?? throw new NullReferenceException();
            taskEntry.Status = taskItemDto.Status;
            taskEntry.DueDate = taskItemDto.DueDate;
            taskEntry.Priority = taskItemDto.Priority;
            await _context.SaveChangesAsync();
            return taskEntry;
        }

        public async Task<IEnumerable<TaskItem>> SearchTaskAync(int? userId, 
            string? searchQuery,
            int pageNumber,
            int pageSize)
        {
            var collection = _context.Tasks as IQueryable<TaskItem>;

            if (userId.HasValue)
            {
                collection = collection.Where(task => task.TaskOwner != null 
                    && task.TaskOwner.UserId == userId);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(task => task.Title.Contains(searchQuery)
                    || task.Description.Contains(searchQuery));
            }

            return await collection.OrderBy(task => task.Title)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<TaskItem> AssignTaskAsync(int id, User user)
        {
            var taskEntry = await _context.Tasks.Where(task => task.TaskItemId == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Task with Id {id} not found.");
            taskEntry.TaskOwner = user;
            await _context.SaveChangesAsync();
            return taskEntry;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
