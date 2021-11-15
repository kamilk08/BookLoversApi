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
    internal class PublisherByNameHandler : IQueryHandler<PublisherByNameQuery, PublisherDto>
    {
        private readonly PublicationsContext _readContext;
        private readonly IMapper _mapper;

        public PublisherByNameHandler(PublicationsContext readContext, IMapper mapper)
        {
            this._readContext = readContext;
            this._mapper = mapper;
        }

        public async Task<PublisherDto> HandleAsync(PublisherByNameQuery query)
        {
            var publisher = await this._readContext.Publishers.AsNoTracking()
                .ActiveRecords()
                .FindPublisherWithExactName(query.Name);

            return this._mapper.Map<PublisherReadModel, PublisherDto>(publisher);
        }
    }
}