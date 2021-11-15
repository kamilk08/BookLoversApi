using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.QueryHandlers
{
    internal class BookReaderQueryHandler : IQueryHandler<BookReaderByIdQuery, BookReaderDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public BookReaderQueryHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<BookReaderDto> HandleAsync(BookReaderByIdQuery query)
        {
            var bookReader = await this._context.Readers.AsNoTracking()
                .SingleOrDefaultAsync(p => p.ReaderId == query.ReaderId);

            return this._mapper.Map<ReaderReadModel, BookReaderDto>(bookReader);
        }
    }
}