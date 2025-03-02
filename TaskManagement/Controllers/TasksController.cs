using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Business.Models;
using TaskManagement.DataAccess.Entities;
using TaskManagement.DataAccess.Services;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TaskManagement.Controllers
{
    /// <summary>
    /// Controller for managing tasks.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator _validator;
        private const int maxTasksPageSize = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="TasksController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="taskItemRepository">The task item repository instance.</param>
        /// <param name="mapper">The mapper instance.</param>
        /// <param name="validator">The validator instance.</param>
        public TasksController(ILogger<TasksController> logger,
            ITaskItemRepository taskItemRepository,
            IMapper mapper,
            IValidator<TaskItemDto> validator)
        {
            _logger = logger;
            _taskItemRepository = taskItemRepository ?? throw new ArgumentNullException(nameof(taskItemRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="task">The task details to create.</param>
        /// <returns>The created task details.</returns>
        /// <response code="201">Returns the created task details.</response>
        /// <response code="400">If the task details are invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemDto task)
        {

            ValidationResult result = await _validator.ValidateAsync(new ValidationContext<TaskItemDto>(task));

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var newTask = await _taskItemRepository.CreateTaskAsync(_mapper.Map<TaskItem>(task));
            _logger.LogInformation("Task created: {Task}", task);
            return CreatedAtAction(nameof(CreateTask), new { id = newTask.TaskItemId }, _mapper.Map<TaskItemDto>(task));
        }


        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The ID of the task to be updated.</param>
        /// <param name="updatedTask">The updated task details.</param>
        /// <returns>No content if the task is successfully updated.</returns>
        /// <response code="204">If the task is successfully updated.</response>
        /// <response code="400">If the updated task details are invalid.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemDto updatedTask)
        {
            ValidationResult result = await _validator.ValidateAsync(new ValidationContext<TaskItemDto>(updatedTask));

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            await _taskItemRepository.UpdateTaskAsync(id, updatedTask);
            return NoContent();
        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="id">The ID of the task to be deleted.</param>
        /// <returns>No content if the task is successfully deleted.</returns>
        /// <response code="204">If the task is successfully deleted.</response>
        /// <response code="404">If the task is not found.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {

            await _taskItemRepository.DeleteTask(id);
            _logger.LogInformation("Task with ID deleted: {id}", id);
            return NoContent();

        }

        /// <summary>
        /// Gets a task by ID.
        /// </summary>
        /// <param name="id">The ID of the task to retrieve.</param>
        /// <returns>The task details.</returns>
        /// <response code="200">Returns the task details.</response>
        /// <response code="204">If the task is not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDetailsDto>> GetTask(int id)
        {
            var task = await _taskItemRepository.GetTaskAsync(id);
            if (task == null)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<TaskItemDetailsDto>(task));
        }

        /// <summary>
        /// Get tasks based on search criteria.
        /// </summary>
        /// <param name="userId">The ID of the user to filter tasks by.</param>
        /// <param name="searchQuery">The search query to filter tasks by.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <returns>A list of tasks that match the search criteria.</returns>
        /// <response code="200">Returns the list of tasks</response>
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<TaskItemDetailsDto>>> GetTasks(int? userId,
            string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {

            if (pageSize > maxTasksPageSize)
            {
                pageSize = maxTasksPageSize;
            }

            var tasks = await _taskItemRepository.SearchTaskAync(userId, searchQuery, pageNumber, pageSize);
            return Ok(_mapper.Map<IEnumerable<TaskItemDetailsDto>>(tasks));

        }

        /// <summary>
        /// Get all tasks.
        /// </summary>
        /// <returns>A list of tasks</returns>
        /// <response code="200">Returns the list of tasks</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDetailsDto>>> GetTasks()
        {
            var tasks = await _taskItemRepository.GetTasksAsync();
            return Ok(_mapper.Map<IEnumerable<TaskItemDetailsDto>>(tasks));
        }

        /// <summary>
        /// Assigns a task to a user.
        /// </summary>
        /// <param name="id">The ID of the task to be assigned.</param>
        /// <param name="userWithoutTaskDto">The user details to whom the task will be assigned.</param>
        /// <returns>An updated task item with the assigned user.</returns>
        /// <response code="200">Returns the updated task item</response>
        /// <response code="400">If the user details are null</response>
        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignTask(int id, [FromBody] UserWithoutTaskDto userWithoutTaskDto)
        {
            if (userWithoutTaskDto == null)
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var user = _mapper.Map<User>(userWithoutTaskDto);
            var updatedTask = await _taskItemRepository.AssignTaskAsync(id, user);
            _logger.LogInformation("Task {TaskId} assigned to user {UserId}", id, user.UserId);
            return Ok(_mapper.Map<TaskItemDetailsDto>(updatedTask));
        }
    }
}
