using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Readers;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Projections.BookcaseOwners
{
    internal class ReaderCreatedProjection :
        IProjectionHandler<BookcaseOwnerCreated>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public ReaderCreatedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookcaseOwnerCreated @event)
        {
            var readModel = _context.Bookcases.Single(p => p.Guid == @event.BookcaseGuid);

            readModel.ReaderId = @event.ReaderId;

            _context.Readers.Add(new ReaderReadModel
            {
                Guid = @event.AggregateGuid,
                ReaderGuid = @event.ReaderGuid,
                ReaderId = @event.ReaderId,
                Status = @event.Status,
                Bookcase = readModel
            });

            _context.SaveChanges();
        }
    }
}