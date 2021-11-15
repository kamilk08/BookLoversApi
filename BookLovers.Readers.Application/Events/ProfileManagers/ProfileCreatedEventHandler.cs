using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Application.Events.ProfileManagers
{
    internal class ProfileCreatedEventHandler :
        IDomainEventHandler<ProfileCreated>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ProfileCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ProfileCreated @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new CreateProfilePrivacyManagerInternalCommand(@event.AggregateGuid));
        }
    }
}