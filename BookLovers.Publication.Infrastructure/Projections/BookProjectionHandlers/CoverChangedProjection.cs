﻿using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class CoverChangedProjection : IProjectionHandler<CoverChanged>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public CoverChangedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(CoverChanged @event)
        {
            _context.Books.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookReadModel
                {
                    CoverSource = @event.PictureSource
                });
        }
    }
}