namespace TaskManagement.Models
{
    public class TaskItemDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
    }
}
