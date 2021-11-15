using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.PublisherCycles
{
    public class CycleByIdQuery : IQuery<PublisherCycleDto>
    {
        public int Id { get; set; }

        public CycleByIdQuery()
        {
        }

        public CycleByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}