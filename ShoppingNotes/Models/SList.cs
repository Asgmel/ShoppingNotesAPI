using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Models
{
    public class SList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        public ICollection<Note> Notes { get; set; } = new List<Note>();

        [Required]
        public int UserId { get; set; }

        [Required]
        public User? User { get; set; }
    }
}
