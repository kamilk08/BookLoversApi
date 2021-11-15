using AutoMapper;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    internal class QuotesMapping : Profile
    {
        public QuotesMapping()
        {
            this.CreateMap<BookQuoteAdded, QuoteReadModel>()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.QuoteState))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.QuoteLikes, opt => opt.Ignore())
                .ForMember(dest => dest.QuoteType, opt => opt.MapFrom(p => p.Type));

            this.CreateMap<AuthorQuoteAdded, QuoteReadModel>().ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.QuoteState))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.QuoteLikes, opt => opt.Ignore())
                .ForMember(dest => dest.QuoteType, opt => opt.MapFrom(p => p.Type));

            this.CreateMap<QuoteLiked, QuoteLikeReadModel>();

            this.CreateMap<QuoteReadModel, int>().ConstructUsing(p => p.Id);

            this.CreateMap<QuoteReadModel, QuoteDto>()
                .ForMember(
                    dest => dest.QuoteFromGuid,
                    opt => opt.MapFrom(p => p.AuthorId == null ? p.Book.Guid : p.Author.Guid))
                .ForMember(dest => dest.QuoteFromId, opt => opt.MapFrom(p => p.AuthorId ?? p.BookId))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId))
                .ForMember(dest => dest.ReaderGuid, opt => opt.MapFrom(p => p.ReaderGuid))
                .ForMember(dest => dest.QuoteLikes, opt => opt.MapFrom(p => p.QuoteLikes));

            this.CreateMap<QuoteLikeReadModel, QuoteLikeDto>();
        }
    }
}