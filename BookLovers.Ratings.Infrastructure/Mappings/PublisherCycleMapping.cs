using AutoMapper;
using BookLovers.Ratings.Domain.PublisherCycles;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    public class PublisherCycleMapping : Profile
    {
        public PublisherCycleMapping()
        {
            CreateMap<PublisherCycle, PublisherCycleReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.PublisherCycleGuid, opt => opt.MapFrom(p => p.Identification.CycleGuid))
                .ForMember(dest => dest.PublisherCycleId, opt => opt.MapFrom(p => p.Identification.CycleId))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(p => p.Books));

            CreateMap<PublisherCycleReadModel, PublisherCycleIdentification>()
                .ConstructUsing(p =>
                    new PublisherCycleIdentification(p.PublisherCycleGuid, p.PublisherCycleId));

            CreateMap<PublisherCycleReadModel, PublisherCycle>()
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(p => p))
                .ForMember(PublisherCycle.Relations.Books, opt => opt.MapFrom(p => p.Books));
        }
    }
}