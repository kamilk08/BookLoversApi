using System.Linq;
using AutoMapper;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Mappings
{
    public class BookcaseMapping : Profile
    {
        public BookcaseMapping()
        {
            CreateMap<BookcaseCreated, BookcaseReadModel>()
                .ForMember(dest => dest.ReaderGuid, opt => opt.MapFrom(p => p.ReaderGuid))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.Shelves, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.BookcaseStatus));

            CreateMap<BookcaseReadModel, BookcaseDto>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId))
                .ForMember(
                    dest => dest.BooksCount,
                    opt => opt.MapFrom(p => p.Shelves.SelectMany(sm => sm.Books).Count()))
                .ForMember(dest => dest.Shelves, opt => opt.MapFrom(p => p.Shelves));
        }
    }
}