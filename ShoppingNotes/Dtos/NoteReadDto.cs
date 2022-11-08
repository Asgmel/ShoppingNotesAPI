namespace ShoppingNotes.Dtos
{
    public class NoteReadDto
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public int OwnerId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
