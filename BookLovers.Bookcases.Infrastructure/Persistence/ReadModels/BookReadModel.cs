using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Persistence.ReadModels
{
    public class BookReadModel : IReadModel<BookReadModel>
    {
        public int Id { get; set; }

        public Guid AggregateGuid { get; set; }

        public Guid BookGuid { get; set; }

        public int BookId { get; set; }

        public int Status { get; set; }
    }
}