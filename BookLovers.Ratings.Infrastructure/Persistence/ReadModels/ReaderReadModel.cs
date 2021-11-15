using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.Persistence.ReadModels
{
    public class ReaderReadModel : IReadModel<ReaderReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public int Status { get; set; }
    }
}