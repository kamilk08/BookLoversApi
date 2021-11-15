using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.PublisherCycles
{
    public class CycleByNameQuery : IQuery<PublisherCycleDto>
    {
        public string Name { get; set; }

        public CycleByNameQuery()
        {
        }

        public CycleByNameQuery(string name)
        {
            this.Name = name;
        }
    }
}