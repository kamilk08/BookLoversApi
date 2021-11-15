using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Settings;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.BookcaseSettings
{
    internal class PrivacyOptionChangedProjection :
        IProjectionHandler<PrivacyOptionChanged>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public PrivacyOptionChangedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(PrivacyOptionChanged @event)
        {
            _context.SettingsManagers
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new SettingsManagerReadModel
                {
                    Privacy = @event.Privacy
                });
        }
    }
}