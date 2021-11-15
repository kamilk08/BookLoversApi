using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Publishers
{
    public class PublisherByGuidQuery : IQuery<PublisherDto>
    {
        public Guid PublisherGuid { get; }

        public PublisherByGuidQuery(Guid publisherGuid)
        {
            this.PublisherGuid = publisherGuid;
        }
    }
}