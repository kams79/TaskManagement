using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.DataAccess.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
    }
}
