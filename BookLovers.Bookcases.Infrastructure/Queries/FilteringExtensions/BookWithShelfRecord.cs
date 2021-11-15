using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Queries.FilteringExtensions
{
    internal sealed class BookWithShelfRecord
    {
        public BookReadModel Book { get; set; }

        public ShelfRecordReadModel ShelfRecord { get; set; }
    }
}