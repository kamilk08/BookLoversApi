using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Reviews
{
    public class ReviewByGuidQuery : IQuery<ReviewDto>
    {
        public Guid ReviewGuid { get; }

        public ReviewByGuidQuery(Guid reviewGuid)
        {
            this.ReviewGuid = reviewGuid;
        }
    }
}