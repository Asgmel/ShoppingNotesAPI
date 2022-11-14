using AutoMapper;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;

namespace ShoppingNotes.Profiles
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NListProfile : Profile
    {
        public NListProfile()
        {
            CreateMap<SList, SListReadDto>();
            CreateMap<SListCreateDto, SList>();
            CreateMap<SList, SListUpdateDto>();
            CreateMap<SListUpdateDto, SList>();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
