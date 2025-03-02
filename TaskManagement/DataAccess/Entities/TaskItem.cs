using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.Business.Models;

namespace TaskManagement.DataAccess.Entities
{
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; } = Priority.None;
        public Status Status { get; set; } = Status.None;

        [ForeignKey("UserId")]
        public User? TaskOwner { get; set; } = null;
        public int UserId { get; set; } = -1;

    }
}
