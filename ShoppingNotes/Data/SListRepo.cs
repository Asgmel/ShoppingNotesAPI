using Microsoft.EntityFrameworkCore;
using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
    public class SListRepo : ISListRepo
    {
        private readonly NotesDbContext _context;

        public SListRepo(NotesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateListAsync(SList sList)
        {
            if (sList == null)
            {
                throw new ArgumentNullException(nameof(sList));
            }

            await _context.Lists.AddAsync(sList);
        }

        public void DeleteList(SList sList)
        {
            if (sList == null)
            {
                throw new ArgumentNullException(nameof(sList));
            }

            _context.Lists.Remove(sList);
        }

        public void DeleteLists(IEnumerable<SList> sLists)
        {
            sLists.ToList().ForEach(l => _context.Lists.Remove(l));
        }

        public async Task<IEnumerable<SList>> GetAllListsAsync(int userId)
        {
            return await _context.Lists.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<SList?> GetListByIdAsync(int id, bool includeNotes = true)
        {
            return await _context.Lists.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
