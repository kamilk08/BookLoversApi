using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Settings;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Projections.BookcaseSettings
{
    internal class SettingManagerCreatedProjection :
        IProjectionHandler<SettingsManagerCreated>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public SettingManagerCreatedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(SettingsManagerCreated @event)
        {
            var bookcaseReadModel = _context.Bookcases
                .Single(p => p.Guid == @event.BookcaseGuid);

            _context.SettingsManagers.Add(new SettingsManagerReadModel
            {
                Guid = @event.AggregateGuid,
                BookcaseGuid = @event.BookcaseGuid,
                Privacy = @event.Privacy,
                Capacity = @event.Capacity,
                BookcaseId = bookcaseReadModel.Id,
                Status = @event.Status
            });

            _context.SaveChanges();
        }
    }
}