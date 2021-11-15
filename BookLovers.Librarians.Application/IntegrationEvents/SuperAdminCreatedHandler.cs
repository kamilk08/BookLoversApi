using System;
using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Application.Commands;
using BookLovers.Librarians.Application.Commands.TicketOwners;
using BookLovers.Librarians.Application.WriteModels;

namespace BookLovers.Librarians.Application.IntegrationEvents
{
    internal class SuperAdminCreatedHandler :
        IIntegrationEventHandler<SuperAdminCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SuperAdminCreatedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(SuperAdminCreatedIntegrationEvent @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new CreateTicketOwnerInternalCommand(@event.ReaderGuid, @event.ReaderId));

            await this._commandDispatcher.SendInternalCommandAsync(
                new AssignSuperAdminToLibrarianInternalCommand(
                    new CreateLibrarianWriteModel
                    {
                        LibrarianGuid = Guid.NewGuid(),
                        ReaderGuid = @event.ReaderGuid
                    }));
        }
    }
}