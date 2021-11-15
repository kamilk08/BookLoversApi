using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Profile;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Profiles
{
    internal class IdentityChangedProjection : IProjectionHandler<IdentityChanged>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public IdentityChangedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(IdentityChanged @event)
        {
            this._context.Profiles.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ProfileReadModel
                {
                    FullName = @event.FullName,
                    Sex = @event.Sex,
                    SexName = @event.SexName,
                    BirthDate = @event.BirthDate
                });
        }
    }
}