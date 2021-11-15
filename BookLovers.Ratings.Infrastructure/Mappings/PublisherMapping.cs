using AutoMapper;
using BookLovers.Ratings.Domain.Publisher;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    public class PublisherMapping : Profile
    {
        public PublisherMapping()
        {
            CreateMap<Publisher, PublisherReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.PublisherGuid, opt => opt.MapFrom(p => p.Identification.PublisherGuid))
                .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(p => p.Identification.PublisherId))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(p => p.Books))
                .ForMember(
                    dest => dest.PublisherCycles,
                    opt => opt.MapFrom(p => p.PublisherCycles));

            CreateMap<PublisherReadModel, PublisherIdentification>()
                .ConstructUsing(p => new PublisherIdentification(p.PublisherGuid, p.PublisherId));

            CreateMap<PublisherReadModel, Publisher>().ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(p => p))
                .ForMember(Publisher.Relations.Books, opt => opt.MapFrom(p => p.Books))
                .ForMember(Publisher.Relations.Cycles, opt => opt.MapFrom(p => p.PublisherCycles));

            CreateMap<PublisherReadModel, RatingsDto>()
                .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(p => p.PublisherId))
                .ForMember(dest => dest.Average, opt => opt.MapFrom(p => p.Average))
                .ForMember(dest => dest.RatingsCount, opt => opt.MapFrom(p => p.RatingsCount));

            CreateMap<PublisherReadModel, PublisherDto>();
        }
    }
}