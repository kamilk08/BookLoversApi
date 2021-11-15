using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ReviewEditReadModel : IReadModel
    {
        public int Id { get; set; }

        public Guid ReviewGuid { get; set; }

        public int ReviewId { get; set; }

        public DateTime EditedAt { get; set; }

        public string Review { get; set; }
    }
}