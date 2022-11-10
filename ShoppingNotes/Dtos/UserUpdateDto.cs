using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for updating users
    /// </summary>
    public class UserUpdateDto
    {
        /// <summary>
        /// The users first name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
    }
}
