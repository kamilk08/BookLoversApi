using AutoMapper;
using BookLovers.Publication.Infrastructure.Dtos;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    public class BookReaderMapping : Profile
    {
        public BookReaderMapping()
        {
            this.CreateMap<ReaderReadModel, BookReaderDto>()
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId));
        }
    }
}