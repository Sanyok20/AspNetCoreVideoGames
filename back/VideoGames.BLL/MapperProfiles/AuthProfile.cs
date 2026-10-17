using AutoMapper;
using VideoGames.BLL.Dtos.Auth;
using VideoGames.DAL.Entities;

namespace VideoGames.BLL.MapperProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}