using AutoMapper;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    public class RatingMapping : Profile
    {
        public RatingMapping()
        {
            CreateMap<Rating, RatingReadModel>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(p => p.BookId))
                .ForMember(dest => dest.Stars, opt => opt.MapFrom(p => p.Stars))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId));

            CreateMap<RatingReadModel, Rating>();

            CreateMap<RatingReadModel, RatingDto>();
        }
    }
}