using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Queries
{
    public class GetReadersOutboxMessagesQuery : IQuery<List<OutboxMessage>>
    {
    }
}