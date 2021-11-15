using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Readers
{
    internal class ReaderByGuidHandler : IQueryHandler<ReaderByGuidQuery, ReaderDto>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderByGuidHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ReaderDto> HandleAsync(ReaderByGuidQuery query)
        {
            var reader = await this._context.Readers.AsNoTracking()
                .ActiveRecords()
                .SingleOrDefaultAsync(p => p.Guid == query.Guid);

            return this._mapper.Map<ReaderReadModel, ReaderDto>(reader);
        }
    }
}