using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    public class NListCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}
