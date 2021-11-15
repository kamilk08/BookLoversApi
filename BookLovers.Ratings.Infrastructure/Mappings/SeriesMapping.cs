using AutoMapper;
using BookLovers.Ratings.Domain.BookSeries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    internal class SeriesMapping : Profile
    {
        public SeriesMapping()
        {
            CreateMap<Series, SeriesReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.SeriesGuid, opt => opt.MapFrom(p => p.Identification.SeriesGuid))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(p => p.Identification.SeriesId))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(p => p.Books));

            CreateMap<SeriesReadModel, SeriesIdentification>()
                .ConstructUsing(p => new SeriesIdentification(p.SeriesGuid, p.SeriesId));

            CreateMap<SeriesReadModel, Series>()
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(p => p))
                .ForMember(Series.Relations.Books, opt => opt.MapFrom(p => p.Books));

            CreateMap<SeriesReadModel, SeriesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.SeriesGuid, opt => opt.MapFrom(p => p.SeriesGuid))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(p => p.SeriesId));

            CreateMap<SeriesReadModel, RatingsDto>()
                .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(p => p.SeriesId))
                .ForMember(dest => dest.Average, opt => opt.MapFrom(p => p.Average))
                .ForMember(dest => dest.RatingsCount, opt => opt.MapFrom(p => p.RatingsCount));
        }
    }
}