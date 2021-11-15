using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Publishers
{
    public class PublisherRatingsQuery : IQuery<RatingsDto>
    {
        public int PublisherId { get; set; }

        public PublisherRatingsQuery()
        {
        }

        public PublisherRatingsQuery(int publisherId)
        {
            this.PublisherId = publisherId;
        }
    }
}