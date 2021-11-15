using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Newtonsoft.Json;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class BookHashTagsChangedProjection :
        IProjectionHandler<BookHashTagsChanged>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public BookHashTagsChangedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookHashTagsChanged @event)
        {
            var serializedHashTags = JsonConvert.SerializeObject(@event.HashTags);

            _context.Books.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookReadModel
                {
                    HashTags = serializedHashTags
                });
        }
    }
}