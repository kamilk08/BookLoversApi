using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.ProfileManagers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.ProfileManagers
{
    internal class ProfilePrivacyOptionAddedProjection :
        IProjectionHandler<ProfilePrivacyOptionAdded>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ProfilePrivacyOptionAddedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ProfilePrivacyOptionAdded @event)
        {
            var manager = this._context.PrivacyManagers
                .Include(p => p.PrivacyOptions)
                .Single(p => p.Guid == @event.AggregateGuid);

            manager.PrivacyOptions.Add(new ProfilePrivacyOptionReadModel()
            {
                PrivacyTypeId = @event.PrivacyTypeId,
                PrivacyTypeName = @event.PrivacyTypeName,
                PrivacyOptionName = @event.PrivacyOptionName,
                PrivacyTypeOptionId = @event.PrivacyOptionId
            });

            this._context.PrivacyManagers.AddOrUpdate(p => p.Id, manager);

            this._context.SaveChanges();
        }
    }
}