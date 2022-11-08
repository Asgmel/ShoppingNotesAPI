using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Models
{
    public class NList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}
