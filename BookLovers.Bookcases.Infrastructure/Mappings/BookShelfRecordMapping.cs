using AutoMapper;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Mappings
{
    public class BookShelfRecordMapping : Profile
    {
        public BookShelfRecordMapping()
        {
            CreateMap<ShelfRecordReadModel, ShelfRecordDto>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(p => p.Book.BookId))
                .ForMember(dest => dest.ShelfId, opt => opt.MapFrom(p => p.Shelf.Id));
        }
    }
}