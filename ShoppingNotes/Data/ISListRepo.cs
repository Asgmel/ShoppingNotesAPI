using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
    public interface ISListRepo
    {
        Task<IEnumerable<SList>> GetAllListsAsync(int userId);
        Task<SList?> GetListByIdAsync(int id, bool includeNotes = true);
        Task CreateListAsync(SList sList);
        void DeleteList(SList sList);
        void DeleteLists(IEnumerable<SList> sLists);
        Task SaveChangesAsync();
    }
}
