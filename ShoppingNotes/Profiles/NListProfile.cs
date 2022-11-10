using AutoMapper;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;

namespace ShoppingNotes.Profiles
{
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
}
