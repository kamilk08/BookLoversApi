using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Publishers
{
    public class PublisherByNameQuery : IQuery<PublisherDto>
    {
        public string Name { get; set; }

        public PublisherByNameQuery()
        {
        }

        public PublisherByNameQuery(string name)
        {
            this.Name = name;
        }
    }
}