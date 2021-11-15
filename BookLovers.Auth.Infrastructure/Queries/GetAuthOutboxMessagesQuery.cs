using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries
{
    public class GetAuthOutboxMessagesQuery : IQuery<List<OutboxMessage>>
    {
    }
}