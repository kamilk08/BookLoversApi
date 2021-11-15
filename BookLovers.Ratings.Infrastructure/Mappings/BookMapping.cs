using AutoMapper;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    public class BookMapping : Profile
    {
        public BookMapping()
        {
            CreateMap<Book, BookReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.BookGuid, opt => opt.MapFrom(p => p.Identification.BookGuid))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(p => p.Identification.BookId))
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(p => p.Authors))
                .ForMember(dest => dest.Ratings, opt => opt.MapFrom(p => p.Ratings));

            CreateMap<BookReadModel, BookIdentification>()
                .ConstructUsing(p => new BookIdentification(p.BookGuid, p.BookId));

            CreateMap<BookReadModel, Book>()
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(p => p))
                .ForMember(Book.Relations.Authors, opt => opt.MapFrom(p => p.Authors))
                .ForMember(Book.Relations.Ratings, opt => opt.MapFrom(p => p.Ratings));

            CreateMap<BookReadModel, BookDto>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(p => p.BookId))
                .ForMember(dest => dest.BookGuid, opt => opt.MapFrom(p => p.BookGuid))
                .ForMember(dest => dest.Ratings, opt => opt.MapFrom(p => p.Ratings));
        }
    }
}