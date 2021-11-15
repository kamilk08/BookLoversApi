using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Series
{
    public class SeriesByIdQuery : IQuery<SeriesDto>
    {
        public int SeriesId { get; set; }

        public SeriesByIdQuery()
        {
        }

        public SeriesByIdQuery(int seriesId)
        {
            this.SeriesId = seriesId;
        }
    }
}