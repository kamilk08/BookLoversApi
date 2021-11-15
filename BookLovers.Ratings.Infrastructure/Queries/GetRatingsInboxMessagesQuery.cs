﻿using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.Queries
{
    public class GetRatingsInboxMessagesQuery : IQuery<List<InBoxMessage>>
    {
    }
}