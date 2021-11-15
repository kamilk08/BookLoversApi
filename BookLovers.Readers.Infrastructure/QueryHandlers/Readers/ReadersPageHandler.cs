using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Readers.Infrastructure.Queries.Readers;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Readers
{
    internal class ReadersPageHandler : IQueryHandler<ReadersPageQuery, PaginatedResult<ReaderDto>>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReadersPageHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<ReaderDto>> HandleAsync(
            ReadersPageQuery query)
        {
            var baseQuery = _context.Readers
                .AsNoTracking()
                .ActiveRecords()
                .UserNameStartsWith(query.Value)
                .OrderByUserName();

            var readersQuery = baseQuery
                .Paginate(query.Page, query.Count)
                .Future();

            var totalReadersQuery = baseQuery.DeferredCount();

            var totalReaders = await totalReadersQuery.ExecuteAsync();
            var readers = await readersQuery.ToListAsync();

            var results = _mapper.Map<List<ReaderReadModel>, List<ReaderDto>>(readers);

            return new PaginatedResult<ReaderDto>(
                results, query.Page, query.Count, totalReaders);
        }
    }
}