using AutoMapper;
using VideoGames.BLL.Dtos.Role;
using VideoGames.DAL.Entities;

namespace VideoGames.BLL.MapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>();

            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()));

            CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()));
        }
    }
}