using AutoMapper;
using VideoGames.BLL.Dtos.Developer;
using VideoGames.DAL.Entities;

namespace VideoGames.BLL.MapperProfiles
{
    public class DeveloperProfile : Profile
    {
        public DeveloperProfile()
        {
            CreateMap<Developer, DeveloperDto>();

            CreateMap<CreateDeveloperDto, Developer>();

            CreateMap<UpdateDeveloperDto, Developer>();
        }
    }
}