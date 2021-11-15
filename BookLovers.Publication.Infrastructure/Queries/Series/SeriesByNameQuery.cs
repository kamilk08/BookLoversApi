using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Series
{
    public class SeriesByNameQuery : IQuery<SeriesDto>
    {
        public string Name { get; set; }

        public SeriesByNameQuery()
        {
        }

        public SeriesByNameQuery(string name)
        {
            this.Name = name;
        }
    }
}