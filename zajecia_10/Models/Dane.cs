using System.ComponentModel.DataAnnotations;

namespace LoginApp.Models
{
    public class Dane
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}