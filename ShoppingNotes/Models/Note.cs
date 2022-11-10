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

        public bool IsCompleted { get; set; } = false;

        [Required]
        public int SListId { get; set; }

        [Required]
        public SList? SList { get; set; }
    }
}
