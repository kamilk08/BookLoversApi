using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class ChangeBookDescriptionProjection :
        IProjectionHandler<BookDescriptionChanged>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public ChangeBookDescriptionProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookDescriptionChanged @event)
        {
            _context.Books.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookReadModel
                {
                    Description = @event.Description,
                    DescriptionSource = @event.DescriptionSource
                });
        }
    }
}