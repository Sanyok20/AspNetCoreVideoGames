using AutoMapper;
using VideoGames.BLL.Dtos.Genre;
using VideoGames.DAL.Entities;

namespace VideoGames.BLL.MapperProfiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreDto>();

            CreateMap<CreateGenreDto, Genre>();

            CreateMap<UpdateGenreDto, Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}