using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.ProfileManagers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.ProfileManagers
{
    internal class PrivacyManagerCreatedProjection :
        IProjectionHandler<ProfilePrivacyManagerCreated>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public PrivacyManagerCreatedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ProfilePrivacyManagerCreated @event)
        {
            var profile = this._context.Profiles
                .Include(p => p.Reader)
                .Single(p => p.Guid == @event.ProfileGuid);

            this._context.PrivacyManagers.Add(new ProfilePrivacyManagerReadModel()
            {
                Guid = @event.AggregateGuid,
                ProfileGuid = @event.ProfileGuid,
                ProfileId = profile.Id,
                ReaderId = profile.Reader.ReaderId,
                Status = @event.Status
            });

            this._context.SaveChanges();
        }
    }
}