using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.Publishers;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Publishers
{
    internal class PublisherByIdHandler : IQueryHandler<PublisherByIdQuery, PublisherDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PublisherByIdHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PublisherDto> HandleAsync(PublisherByIdQuery query)
        {
            var publisher = await this._context.Publishers
                .Include(p => p.Books)
                .Include(p => p.Cycles)
                .AsNoTracking()
                .ActiveRecords().WithId(query.PublisherId)
                .SingleOrDefaultAsync();

            return this._mapper.Map<PublisherReadModel, PublisherDto>(publisher);
        }
    }
}