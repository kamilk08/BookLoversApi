using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Readers
{
    internal class ReaderByUserNameHandler : IQueryHandler<ReaderByUserNameQuery, ReaderDto>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderByUserNameHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ReaderDto> HandleAsync(ReaderByUserNameQuery query)
        {
            var reader = await _context.Readers
                .AsNoTracking()
                .ActiveRecords()
                .Where(p => p.UserName.ToUpper().StartsWith(query.UserName.ToUpper()))
                .SingleOrDefaultAsync();

            var dto = _mapper.Map<ReaderReadModel, ReaderDto>(reader);

            return dto;
        }
    }
}