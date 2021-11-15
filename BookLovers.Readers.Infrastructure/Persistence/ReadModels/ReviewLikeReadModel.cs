using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ReviewLikeReadModel : IReadModel
    {
        public int Id { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }
    }
}