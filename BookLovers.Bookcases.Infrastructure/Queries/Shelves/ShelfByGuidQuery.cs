using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Shelves
{
    public class ShelfByGuidQuery : IQuery<ShelfDto>
    {
        public Guid ShelfGuid { get; }

        public ShelfByGuidQuery(Guid shelfGuid)
        {
            ShelfGuid = shelfGuid;
        }
    }
}