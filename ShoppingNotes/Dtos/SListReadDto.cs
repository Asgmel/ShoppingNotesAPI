using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for reading lists
    /// </summary>
    public class SListReadDto
    {
        /// <summary>
        /// The list ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The list name
        /// </summary>
        public string? Name { get; set; }


        /// <summary>
        /// The ID of the user that created the list
        /// </summary>
        public int UserId { get; set; }
    }
}
