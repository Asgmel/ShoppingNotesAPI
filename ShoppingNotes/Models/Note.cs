using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Content { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
