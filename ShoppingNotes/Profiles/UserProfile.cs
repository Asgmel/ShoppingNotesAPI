using AutoMapper;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;

namespace ShoppingNotes.Profiles
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
