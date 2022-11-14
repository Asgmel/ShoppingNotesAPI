using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IUserRepo
    {
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<User?> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        void DeleteUser(User user);
        Task SaveChangesAsync();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
