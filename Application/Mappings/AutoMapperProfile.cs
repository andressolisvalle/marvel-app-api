using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap <ComicResultDto, ComicDto>()
                .ForMember(dest => dest.imageUrl, opt => opt.MapFrom(src => $"{src.thumbnail.path}.{src.thumbnail.extension}"));

            CreateMap<FavoriteComic, FavoriteComicDto>();





        }
    }
}
