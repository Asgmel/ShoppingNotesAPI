using ShoppingNotes.Models;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for reading notes
    /// </summary>
    public class NoteReadDto
    {
        /// <summary>
        /// The note ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The note contents
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The note status, true if completed, false if not
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// The ID of the list where the note lives
        /// </summary>
        public int SListId { get; set; }
    }
}
