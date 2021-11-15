using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Queries
{
    public class GetBooksOutboxMessagesQuery : IQuery<List<OutboxMessage>>
    {
    }
}