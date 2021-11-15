using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries.Tickets;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.Tickets
{
    internal class ApprovedTicketsHandler : IQueryHandler<ApprovedTicketsQuery, IList<TicketDto>>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public ApprovedTicketsHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<IList<TicketDto>> HandleAsync(ApprovedTicketsQuery query)
        {
            var tickets = await this._context.Tickets
                .AsNoTracking()
                .Where(p => p.DecisionValue == Decision.Approve.Value)
                .ToListAsync();

            return this._mapper.Map<List<TicketReadModel>, IList<TicketDto>>(tickets);
        }
    }
}