using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Publishers.Services;
using BookLovers.Publication.Events.Publishers;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class PublisherArchivedEventHandler :
        IDomainEventHandler<PublisherArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;
        private readonly ISelfPublisherService _selfPublisherService;

        public PublisherArchivedEventHandler(
            IInternalCommandDispatcher commandDispatcher,
            ISelfPublisherService selfPublisherService)
        {
            this._commandDispatcher = commandDispatcher;
            this._selfPublisherService = selfPublisherService;
        }

        public async Task HandleAsync(PublisherArchived @event)
        {
            var selfPublisher = await this._selfPublisherService.GetSelfPublisherAsync();

            foreach (var publisherBook in @event.PublisherBooks)
                await this._commandDispatcher.SendInternalCommandAsync(
                    new MakeBookAsSelfPublishedInternalCommand(selfPublisher.Guid, @event.AggregateGuid));
        }
    }
}