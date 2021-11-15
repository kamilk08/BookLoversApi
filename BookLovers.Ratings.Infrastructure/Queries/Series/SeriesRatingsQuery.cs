using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Series
{
    public class SeriesRatingsQuery : IQuery<RatingsDto>
    {
        public int SeriesId { get; set; }

        public SeriesRatingsQuery()
        {
        }

        public SeriesRatingsQuery(int seriesId)
        {
            this.SeriesId = seriesId;
        }
    }
}