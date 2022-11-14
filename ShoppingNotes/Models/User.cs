using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Models
{
    /// <summary>
    /// The model for the user entity
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user ID, primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The users username
        /// </summary>
        [Required]
        public string? UserName { get; set; }


        /// <summary>
        /// The users password hash
        /// </summary>
        [Required]
        public byte[]? PasswordHash { get; set; }

        /// <summary>
        /// The users password hash
        /// </summary>
        [Required]
        public byte[]? PasswordSalt { get; set; }

        /// <summary>
        /// A collection of SLists that the user has created
        /// </summary>
        public ICollection<SList> SLists { get; set; } = new List<SList>();
    }
}
