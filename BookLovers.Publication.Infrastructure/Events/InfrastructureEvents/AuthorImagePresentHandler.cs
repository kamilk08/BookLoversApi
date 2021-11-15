using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;

namespace BookLovers.Publication.Infrastructure.Events.InfrastructureEvents
{
    internal class AuthorImagePresentHandler : IInfrastructureEventHandler<AuthorImageSaved>
    {
        private readonly PublicationsContext _context;

        public AuthorImagePresentHandler(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task HandleAsync(AuthorImageSaved @event)
        {
            this._context.AuthorImages.AddOrUpdate(
                p => p.AuthorGuid,
                new AuthorImageReadModel()
                {
                    AuthorPictureUrl = @event.ImageUrl,
                    FileName = @event.FileName,
                    MimeType = @event.MimeType,
                    AuthorGuid = @event.AuthorGuid
                });

            await this._context.SaveChangesAsync();
        }
    }
}