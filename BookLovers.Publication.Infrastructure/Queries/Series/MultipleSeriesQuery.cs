using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Series
{
    public class MultipleSeriesQuery : IQuery<List<SeriesDto>>
    {
        public List<int> Ids { get; set; }

        public MultipleSeriesQuery()
        {
            this.Ids = this.Ids ?? new List<int>();
        }

        public MultipleSeriesQuery(List<int> ids)
        {
            this.Ids = ids;
        }
    }
}