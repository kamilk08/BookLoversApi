using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Profile;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Profiles
{
    internal class AddressChangedProjection : IProjectionHandler<AddressChanged>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public AddressChangedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(AddressChanged @event)
        {
            this._context.Profiles
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ProfileReadModel
                {
                    City = @event.City,
                    Country = @event.Country
                });
        }
    }
}