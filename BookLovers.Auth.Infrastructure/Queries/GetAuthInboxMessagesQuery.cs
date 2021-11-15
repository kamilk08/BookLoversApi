using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries
{
    public class GetAuthInboxMessagesQuery : IQuery<List<InBoxMessage>>
    {
    }
}