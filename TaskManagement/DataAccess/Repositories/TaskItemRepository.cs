
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Business.Models;
using TaskManagement.DataAccess.DbContexts;
using TaskManagement.DataAccess.Entities;

namespace TaskManagement.DataAccess.Services
{
    /// <summary>
    /// Repository for managing TaskItem entities.
    /// </summary>
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly TaskItemContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItemRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TaskItemRepository(TaskItemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates a new task asynchronously.
        /// </summary>
        /// <param name="task">The task to create.</param>
        /// <returns>The created task.</returns>
        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            var entityEntry = await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        /// <summary>
        /// Deletes a task by its identifier.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        public async Task DeleteTask(int id)
        {
            var taskEntry = _context.Tasks.FirstOrDefault(task => task.TaskItemId == id)
                ?? throw new InvalidOperationException($"Task with Id {id} not found.");

            _context.Tasks.Remove(taskEntry);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a task by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>The task if found; otherwise, null.</returns>
        public async Task<TaskItem?> GetTaskAsync(int id)
        {
            return await _context.Tasks.Where(task => task.TaskItemId == id).Include(a => a.TaskOwner).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all tasks asynchronously.
        /// </summary>
        /// <returns>A collection of tasks.</returns>
        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            var taskEntries = await _context.Tasks.Include(a => a.TaskOwner).ToListAsync();
            return taskEntries;
        }

        /// <summary>
        /// Updates a task asynchronously.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <param name="taskItemDto">The task data transfer object.</param>
        /// <returns>The updated task.</returns>
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

        /// <summary>
        /// Searches for tasks asynchronously.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A collection of tasks.</returns>
        public async Task<IEnumerable<TaskItem>> SearchTaskAync(int? userId, string? searchQuery, int pageNumber, int pageSize)
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

        /// <summary>
        /// Assigns a task to a user asynchronously.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <param name="user">The user to assign the task to.</param>
        /// <returns>The assigned task.</returns>
        public async Task<TaskItem> AssignTaskAsync(int id, User user)
        {
            var taskEntry = await _context.Tasks.Where(task => task.TaskItemId == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Task with Id {id} not found.");
            taskEntry.TaskOwner = user;
            await _context.SaveChangesAsync();
            return taskEntry;
        }

        /// <summary>
        /// Saves changes asynchronously.
        /// </summary>
        /// <returns>True if changes were saved; otherwise, false.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
