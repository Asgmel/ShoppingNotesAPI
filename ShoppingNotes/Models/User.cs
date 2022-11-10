using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public ICollection<SList> SLists { get; set; } = new List<SList>();
    }
}
