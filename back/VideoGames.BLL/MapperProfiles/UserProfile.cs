using AutoMapper;
using VideoGames.DAL.Entities;
using VideoGames.BLL.Dtos.User;

namespace VideoGames.BLL.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}