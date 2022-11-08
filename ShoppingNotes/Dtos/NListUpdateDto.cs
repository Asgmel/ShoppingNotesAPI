using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    public class NListUpdateDto
    {
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}
