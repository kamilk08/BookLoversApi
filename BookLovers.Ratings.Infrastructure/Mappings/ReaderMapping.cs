using AutoMapper;
using BookLovers.Ratings.Domain.RatingGivers;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    public class ReaderMapping : Profile
    {
        public ReaderMapping()
        {
            CreateMap<RatingGiver, ReaderReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id));

            CreateMap<ReaderReadModel, RatingGiver>();

            CreateMap<ReaderReadModel, ReaderDto>()
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId));
        }
    }
}