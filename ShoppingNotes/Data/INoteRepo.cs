using ShoppingNotes.Models;

namespace ShoppingNotes.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface INoteRepo
    {
        Task<IEnumerable<Note>> GetAllNotesAsync(int userId);
        Task<IEnumerable<Note>> GetAllNotesForListAsync(int listId);
        Task<Note?> GetNoteByIdAsync(int id);
        Task CreateNoteAsync(Note note);
        void DeleteNote(Note note);
        void DeleteNotes(IEnumerable<Note> notes);
        Task SaveChangesAsync();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
