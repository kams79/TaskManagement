namespace TaskManagement.Models
{
    public class UserWithoutTaskDto
    {
        public required int UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }

    }
}
