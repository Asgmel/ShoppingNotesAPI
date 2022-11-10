using ShoppingNotes.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for creating notes
    /// </summary>
    public class NoteCreateDto
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

        /// <summary>
        /// The ID of the list where the note lives
        /// </summary>
        [Required]
        public int SListId { get; set; }
    }
}
