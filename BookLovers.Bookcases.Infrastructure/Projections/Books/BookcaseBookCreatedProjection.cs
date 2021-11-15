using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.BookcaseBooks;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Projections.Books
{
    internal class BookcaseBookCreatedProjection :
        IProjectionHandler<BookcaseBookCreated>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public BookcaseBookCreatedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookcaseBookCreated @event)
        {
            _context.Books.Add(new BookReadModel
            {
                BookGuid = @event.BookGuid,
                BookId = @event.BookId,
                AggregateGuid = @event.AggregateGuid,
                Status = @event.Status
            });

            _context.SaveChanges();
        }
    }
}