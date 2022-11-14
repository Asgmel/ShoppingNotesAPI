using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for creating lists
    /// </summary>
    public class SListCreateDto
    {
        /// <summary>
        /// The list name
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

    }
}
