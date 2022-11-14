using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Models
{
    /// <summary>
    /// The model for the Note entity 
    /// </summary>
    public class Note
    {
        /// <summary>
        /// The note ID, primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The text content of the note
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Content { get; set; }

        /// <summary>
        /// Boolean to check if the note is checked as completed or not
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// The ID of the list that contains the note
        /// </summary>
        [Required]
        public int SListId { get; set; }

        /// <summary>
        /// The list that contains the note
        /// </summary>
        [Required]
        public SList? SList { get; set; }
    }
}
