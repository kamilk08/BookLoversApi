using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Publishers
{
    public class PublisherByIdQuery : IQuery<PublisherDto>
    {
        public int PublisherId { get; set; }

        public PublisherByIdQuery()
        {
        }

        public PublisherByIdQuery(int publisherId)
        {
            this.PublisherId = publisherId;
        }
    }
}