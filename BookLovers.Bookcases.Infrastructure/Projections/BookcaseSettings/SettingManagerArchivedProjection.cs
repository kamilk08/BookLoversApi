using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Settings;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.BookcaseSettings
{
    internal class SettingManagerArchivedProjection :
        IProjectionHandler<SettingsManagerArchived>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public SettingManagerArchivedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(SettingsManagerArchived @event)
        {
            _context.SettingsManagers
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new SettingsManagerReadModel
                {
                    Status = AggregateStatus.Archived.Value
                });
        }
    }
}