using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries.PromotionWaiters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Librarians.Infrastructure.Queries.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.PromotionWaiters
{
    internal class PaginatedPromotionWaitersHandler :
        IQueryHandler<PaginatedPromotionWaitersQuery, PaginatedResult<PromotionWaiterDto>>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public PaginatedPromotionWaitersHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<PromotionWaiterDto>> HandleAsync(
            PaginatedPromotionWaitersQuery query)
        {
            var baseQuery = _context.PromotionWaiters
                .AsNoTracking()
                .ActiveRecords()
                .Where(p => query.Ids.Any(a => a == p.ReaderId))
                .SkipUnAvailable()
                .OrderBy(p => p.Id);

            var totalItemsQuery = baseQuery.DeferredCount();
            var itemsQuery = baseQuery.Paginate(query.Page, query.Count).Future();

            var totalItems = await totalItemsQuery.ExecuteAsync();
            var items = await itemsQuery.ToListAsync();

            if (totalItems == 0)
                return new PaginatedResult<PromotionWaiterDto>(query.Page);

            var mappedItems = _mapper.Map<List<PromotionWaiterReadModel>, List<PromotionWaiterDto>>(items);

            return new PaginatedResult<PromotionWaiterDto>(mappedItems, query.Page, query.Count, totalItems);
        }
    }
}