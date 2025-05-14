using System.ComponentModel.DataAnnotations;

namespace LoginApp.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}

