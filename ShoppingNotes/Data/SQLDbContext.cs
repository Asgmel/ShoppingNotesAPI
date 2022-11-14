using Microsoft.EntityFrameworkCore;
using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SQLDbContext : DbContext
    {
        public DbSet<SList> Lists { get; set; } = null!;
        public DbSet<Note> Notes { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public SQLDbContext(DbContextOptions<SQLDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set the username to be unique
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });
        }
    }
}
