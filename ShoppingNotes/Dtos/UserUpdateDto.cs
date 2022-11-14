using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    /// <summary>
    /// A DTO for updating users passwords
    /// </summary>
    public class UserUpdateDto
    {
        /// <summary>
        /// The users current password
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? CurrentPassword { get; set; }

        /// <summary>
        /// The users new password
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? NewPassword { get; set; }

        /// <summary>
        /// The users new password repeated
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? RepeatNewPassword { get; set; }
    }
}
