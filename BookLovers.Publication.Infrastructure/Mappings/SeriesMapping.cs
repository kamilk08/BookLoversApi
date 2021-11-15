using AutoMapper;
using BookLovers.Publication.Events.SeriesCycle;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    public class SeriesMapping : Profile
    {
        public SeriesMapping()
        {
            this.CreateMap<SeriesCreated, SeriesReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(p => p.SeriesName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.SeriesStatus))
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            this.CreateMap<SeriesReadModel, SeriesDto>()
                .ForMember(dest => dest.SeriesName, opt => opt.MapFrom(p => p.Name));
        }
    }
}