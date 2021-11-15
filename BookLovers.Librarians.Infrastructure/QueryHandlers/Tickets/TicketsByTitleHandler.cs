using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries.Tickets;
using LinqKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Librarians.Infrastructure.Queries.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.Tickets
{
    internal class TicketsByTitleHandler :
        IQueryHandler<TicketsByTitleQuery, PaginatedResult<TicketDto>>
    {
        private readonly LibrariansContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public TicketsByTitleHandler(
            LibrariansContext context,
            IHttpContextAccessor contextAccessor,
            IMapper mapper)
        {
            this._context = context;
            this._contextAccessor = contextAccessor;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<TicketDto>> HandleAsync(
            TicketsByTitleQuery query)
        {
            var predicate = PredicateBuilder.New<TicketReadModel>()
                .And(p => p.TicketOwnerGuid == this._contextAccessor.UserGuid)
                .AndIf(p => p.TicketStateValue == TicketState.InProgress.Value, query.Solved)
                .AndIf(p => p.TicketStateValue == TicketState.Solved.Value
                            && p.TicketOwnerGuid == this._contextAccessor.UserGuid, query.Solved)
                .AndWithTitle(query.Phrase);

            var queryable = this._context.Tickets.AsNoTracking().Where(predicate);
            var totalCountQuery = queryable.DeferredCount();
            var paginatedResultsQuery = queryable.OrderByDescending(p => p.Date)
                .Paginate(query.Page, query.Count).Future();

            int totalCount = await totalCountQuery.ExecuteAsync();

            var results = await paginatedResultsQuery.ToListAsync();

            return new PaginatedResult<TicketDto>(
                this._mapper.Map<List<TicketReadModel>, List<TicketDto>>(results), query.Page,
                query.Count, totalCount);
        }
    }
}