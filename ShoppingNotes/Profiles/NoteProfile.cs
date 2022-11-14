using AutoMapper;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;

namespace ShoppingNotes.Profiles
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
