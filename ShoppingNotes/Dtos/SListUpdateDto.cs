using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for updating lists
    /// </summary>
    public class SListUpdateDto
    {
        /// <summary>
        /// The list name
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}
