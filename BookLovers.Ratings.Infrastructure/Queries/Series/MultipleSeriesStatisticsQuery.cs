using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Series
{
    public class MultipleSeriesStatisticsQuery : IQuery<List<RatingsDto>>
    {
        public List<int> SeriesIds { get; set; }

        public MultipleSeriesStatisticsQuery()
        {
            this.SeriesIds = this.SeriesIds ?? new List<int>();
        }

        public MultipleSeriesStatisticsQuery(List<int> seriesIds)
        {
            this.SeriesIds = seriesIds;
        }
    }
}