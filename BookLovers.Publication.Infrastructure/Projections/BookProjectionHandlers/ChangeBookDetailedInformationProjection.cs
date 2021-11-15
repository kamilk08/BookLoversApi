using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class ChangeBookDetailedInformationProjection :
        IProjectionHandler<BookDetailsChanged>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public ChangeBookDetailedInformationProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookDetailsChanged @event)
        {
            _context.Books.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookReadModel
                {
                    Pages = @event.Pages.GetValueOrDefault(),
                    LanguageId = @event.Language.GetValueOrDefault(),
                });
        }
    }
}