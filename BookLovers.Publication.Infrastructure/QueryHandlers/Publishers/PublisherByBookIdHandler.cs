using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Publishers
{
    internal class PublisherByBookIdHandler : IQueryHandler<PublisherByBookIdQuery, PublisherDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PublisherByBookIdHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PublisherDto> HandleAsync(PublisherByBookIdQuery query)
        {
            var publisher = await this._context.Publishers.AsNoTracking()
                .Include(p => p.Books)
                .Include(p => p.Cycles)
                .ActiveRecords()
                .FindPublisherWithBook(query.BookId);

            return this._mapper.Map<PublisherReadModel, PublisherDto>(publisher);
        }
    }
}