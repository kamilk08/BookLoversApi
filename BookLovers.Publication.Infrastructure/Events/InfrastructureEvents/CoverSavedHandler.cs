using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Events.InfrastructureEvents
{
    public class CoverSavedHandler : IInfrastructureEventHandler<BookCoverSaved>
    {
        private readonly PublicationsContext _context;

        public CoverSavedHandler(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task HandleAsync(BookCoverSaved @event)
        {
            this._context.BookCovers.AddOrUpdate(p => p.BookGuid, new BookCoverReadModel()
            {
                BookGuid = @event.BookGuid,
                CoverUrl = @event.ImageUrl,
                MimeType = @event.MimeType,
                FileName = @event.FileName
            });

            await this._context.SaveChangesAsync();
        }
    }
}