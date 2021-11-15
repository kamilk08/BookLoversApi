using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.PublisherCycles
{
    public class PaginatedCyclesQuery : IQuery<PaginatedResult<PublisherCycleDto>>
    {
        public int Page { get; set; }

        public int Count { get; set; }

        public List<int> CyclesIds { get; set; }

        public PaginatedCyclesQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
            this.CyclesIds = CyclesIds ?? new List<int>();
        }

        public PaginatedCyclesQuery(int? page, int? count, List<int> cyclesIds)
        {
            this.CyclesIds = cyclesIds ?? new List<int>();
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}