using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.BookcaseBooks;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.Books
{
    internal class BookcaseBookArchivedProjection :
        IProjectionHandler<BookcaseBookArchived>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public BookcaseBookArchivedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookcaseBookArchived @event)
        {
            _context.Books
                .Where(p => p.BookId == @event.BookId)
                .Update(p => new BookReadModel
                {
                    Status = AggregateStatus.Archived.Value
                });
        }
    }
}