using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for authenticating users
    /// </summary>
    public class UserAuthDto
    {
        /// <summary>
        /// The users user name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        /// <summary>
        /// The users password
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }


    }
}
