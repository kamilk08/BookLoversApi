using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries.Tickets;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.Tickets
{
    internal class TicketByGuidHandler : IQueryHandler<TicketByGuidQuery, TicketDto>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public TicketByGuidHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<TicketDto> HandleAsync(TicketByGuidQuery query)
        {
            var ticket = await this._context.Tickets.AsNoTracking()
                .SingleOrDefaultAsync(p => p.Guid == query.TicketGuid);

            return this._mapper.Map<TicketReadModel, TicketDto>(ticket);
        }
    }
}