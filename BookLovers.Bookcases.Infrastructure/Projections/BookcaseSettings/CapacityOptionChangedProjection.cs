using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Settings;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.BookcaseSettings
{
    internal class CapacityOptionChangedProjection :
        IProjectionHandler<ShelfCapacityChanged>
    {
        private readonly BookcaseContext _context;

        public CapacityOptionChangedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(ShelfCapacityChanged @event)
        {
            _context.SettingsManagers
                .Where(p => p.BookcaseGuid == @event.BookcaseGuid)
                .Update(p => new SettingsManagerReadModel
                {
                    Capacity = @event.Capacity
                });
        }
    }
}