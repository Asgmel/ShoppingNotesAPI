using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Models
{
    /// <summary>
    /// The model for the SList entity
    /// </summary>
    public class SList
    {
        /// <summary>
        /// The SList ID, primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the sList
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        /// <summary>
        /// The collection of notes that exist within the list
        /// </summary>
        public ICollection<Note> Notes { get; set; } = new List<Note>();

        /// <summary>
        /// The ID of the user that created the list
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// The user that created the list
        /// </summary>
        [Required]
        public User? User { get; set; }
    }
}
