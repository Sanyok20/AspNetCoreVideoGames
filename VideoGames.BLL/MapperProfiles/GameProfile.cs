using AutoMapper;
using VideoGames.BLL.Dtos.Game;
using VideoGames.BLL.Dtos.GameDto;
using VideoGames.DAL.Entities;

namespace VideoGames.BLL.MapperProfiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.Developer, opt => opt.MapFrom(src => src.Developer!.Name));

            CreateMap<CreateGameDto, Game>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<UpdateGameDto, Game>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}