using System.Linq;
using AutoMapper;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    internal class PublisherMapping : Profile
    {
        public PublisherMapping()
        {
            this.CreateMap<PublisherReadModel, int>()
                .ConstructUsing(p => p.Id);

            this.CreateMap<PublisherReadModel, PublisherDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(p => p.Publisher))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(p => p.Books))
                .ForMember(
                    dest => dest.Cycles,
                    opt => opt.MapFrom(p => p.Cycles.Select(s => s.Id)));
        }
    }
}