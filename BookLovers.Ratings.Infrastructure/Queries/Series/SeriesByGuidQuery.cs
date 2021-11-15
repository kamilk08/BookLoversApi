using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Series
{
    public class SeriesByGuidQuery : IQuery<SeriesDto>
    {
        public Guid SeriesGuid { get; }

        public SeriesByGuidQuery(Guid seriesGuid)
        {
            this.SeriesGuid = seriesGuid;
        }
    }
}