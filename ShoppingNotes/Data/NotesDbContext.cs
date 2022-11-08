using Microsoft.EntityFrameworkCore;
using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
    public class NotesDbContext : DbContext
    {
        public DbSet<NList> Lists { get; set; } = null!;
        public DbSet<Note> Notes { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
        {

        }
    }
}
