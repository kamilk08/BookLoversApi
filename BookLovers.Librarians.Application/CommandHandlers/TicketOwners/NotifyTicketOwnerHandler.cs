using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.TicketOwners;
using BookLovers.Librarians.Domain.Librarians.DecisionNotifiers;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Application.CommandHandlers.TicketOwners
{
    internal class NotifyTicketOwnerHandler : ICommandHandler<NotifyTicketOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketDecisionNotifier _notifier;

        public NotifyTicketOwnerHandler(IUnitOfWork unitOfWork, TicketDecisionNotifier notifier)
        {
            this._unitOfWork = unitOfWork;
            this._notifier = notifier;
        }

        public async Task HandleAsync(NotifyTicketOwnerInternalCommand command)
        {
            var ticket = await this._unitOfWork.GetAsync<Ticket>(command.TicketGuid);

            await this._notifier.Notify(ticket, command.Notification);
        }
    }
}