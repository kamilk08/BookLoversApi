using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Queries
{
    public class GetLibrariansInboxMessagesQuery : IQuery<List<InBoxMessage>>
    {
    }
}