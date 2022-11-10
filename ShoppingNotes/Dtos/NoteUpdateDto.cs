using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for updating Notes
    /// </summary>
    public class NoteUpdateDto
    {
        /// <summary>
        /// The note content
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Content { get; set; }

        /// <summary>
        /// The note status, true if completed, false if not
        /// </summary>
        public bool IsCompleted { get; set; } = false;
    }
}
