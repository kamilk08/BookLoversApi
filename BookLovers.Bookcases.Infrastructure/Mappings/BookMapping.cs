using AutoMapper;
using BookLovers.Bookcases.Events.BookcaseBooks;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Mappings
{
    internal class BookMapping : Profile
    {
        public BookMapping()
        {
            CreateMap<BookcaseBookCreated, BookReadModel>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(p => p.BookId))
                .ForMember(dest => dest.AggregateGuid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.BookGuid, opt => opt.MapFrom(p => p.BookGuid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status));

            CreateMap<BookReadModel, BookDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.BookId));

            CreateMap<BookReadModel, int>().ConstructUsing(s => s.BookId);
        }
    }
}