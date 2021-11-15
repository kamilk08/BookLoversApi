using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries.Librarians;
using LinqKit;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.Librarians
{
    internal class ManageableTicketsPageHandler :
        IQueryHandler<ManageableTicketsQuery, PaginatedResult<TicketDto>>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public ManageableTicketsPageHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<TicketDto>> HandleAsync(
            ManageableTicketsQuery query)
        {
            var predicate = PredicateBuilder
                .True<TicketReadModel>()
                .And(p => p.TicketStateValue == TicketState.InProgress.Value)
                .OrIf(p => p.TicketStateValue == TicketState.Solved.Value, query.Solved);

            var orderedQueryable = this._context.Tickets
                .AsNoTracking().AsExpandable()
                .Where(predicate)
                .OrderByDescending(p => p.Date)
                .ThenBy(p => p.TicketStateValue == TicketState.Solved.Value);

            var totalCountQuery = orderedQueryable.DeferredCount();

            var resultsQuery = orderedQueryable.Paginate(query.Page, query.Count);

            int totalCount = await totalCountQuery.ExecuteAsync();

            var results = await resultsQuery.ToListAsync();

            return new PaginatedResult<TicketDto>(this._mapper.Map<List<TicketReadModel>, List<TicketDto>>(results),
                query.Page, query.Count, totalCount);
        }
    }
}