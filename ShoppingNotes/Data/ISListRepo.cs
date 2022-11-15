using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ISListRepo
    {
        Task<IEnumerable<SList>> GetAllListsAsync(int userId);
        Task<SList?> GetListByIdAsync(int id);
        Task CreateListAsync(SList sList);
        void DeleteList(SList sList);
        void DeleteLists(IEnumerable<SList> sLists);
        Task SaveChangesAsync();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
