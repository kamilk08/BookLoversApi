using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class ChangeBookBasicInformationProjection :
        IProjectionHandler<BookBasicsChanged>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public ChangeBookBasicInformationProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookBasicsChanged @event)
        {
            _context.Books.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookReadModel
                {
                    Isbn = @event.Isbn,
                    Title = @event.Title,
                    PublicationDate = @event.PublicationDate,
                    SubCategory = @event.SubCategoryName,
                    SubCategoryId = @event.SubCategoryId,
                    Category = @event.CategoryName,
                    CategoryId = @event.CategoryId
                });
        }
    }
}