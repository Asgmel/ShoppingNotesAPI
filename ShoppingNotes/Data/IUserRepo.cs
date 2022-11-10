using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
    public interface IUserRepo
    {
        Task<User?> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        void DeleteUser(User user);
        Task SaveChangesAsync();
    }
}
