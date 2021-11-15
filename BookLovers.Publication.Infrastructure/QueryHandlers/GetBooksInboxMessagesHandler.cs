﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.QueryHandlers
{
    internal class GetBooksInboxMessagesHandler :
        IQueryHandler<GetBooksInboxMessagesQuery, List<InBoxMessage>>
    {
        private readonly PublicationsContext _context;

        public GetBooksInboxMessagesHandler(PublicationsContext context)
        {
            this._context = context;
        }

        public Task<List<InBoxMessage>> HandleAsync(GetBooksInboxMessagesQuery query)
        {
            return this._context.InBoxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}