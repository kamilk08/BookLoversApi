using AutoMapper;
using BookLovers.Publication.Events.PublisherCycles;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    public class PublisherCyclesMapping : Profile
    {
        public PublisherCyclesMapping()
        {
            this.CreateMap<PublisherCycleCreated, PublisherCycleReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.Cycle, opt => opt.MapFrom(p => p.CycleName))
                .ForMember(dest => dest.CycleBooks, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.CycleStatus));

            this.CreateMap<PublisherCycleReadModel, PublisherCycleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.CycleName, opt => opt.MapFrom(p => p.Cycle))
                .ForMember(
                    dest => dest.PublisherId,
                    opt => opt.MapFrom(p => p.Publisher.Id));
        }
    }
}