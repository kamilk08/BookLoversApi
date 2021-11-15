using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Queries
{
    public class GetBookcaseInboxMessagesQuery : IQuery<List<InBoxMessage>>
    {
    }
}