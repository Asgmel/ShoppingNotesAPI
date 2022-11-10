using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for reading users
    /// </summary>
    public class UserReadDto
    {
        /// <summary>
        /// The users ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string? LastName { get; set; }
    }
}
