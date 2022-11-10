using AutoMapper;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;

namespace ShoppingNotes.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteReadDto>();
            CreateMap<NoteCreateDto, Note>();
            CreateMap<Note, NoteUpdateDto>();
            CreateMap<NoteUpdateDto, Note>();
        }
    }
}
