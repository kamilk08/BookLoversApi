using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Application.CommandHandlers.Tickets
{
    internal class SolveTicketInternalHandler : ICommandHandler<SolveTicketInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDecisionProvider _decisionProvider;

        public SolveTicketInternalHandler(IUnitOfWork unitOfWork, IDecisionProvider decisionProvider)
        {
            this._unitOfWork = unitOfWork;
            this._decisionProvider = decisionProvider;
        }

        public async Task HandleAsync(SolveTicketInternalCommand command)
        {
            var ticket = await this._unitOfWork.GetAsync<Ticket>(command.TicketGuid);
            var librarian = await this._unitOfWork.GetAsync<Librarian>(command.LibrarianGuid);

            ticket.SolveTicket(
                librarian,
                this._decisionProvider.GetDecision(command.DecisionId),
                command.Notification);

            await this._unitOfWork.CommitAsync(ticket);
        }
    }
}