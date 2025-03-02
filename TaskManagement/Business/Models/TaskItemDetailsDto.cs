namespace TaskManagement.Business.Models
{
    public class TaskItemDetailsDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public UserDto? AssignedUser { get; set; }
    }
}
