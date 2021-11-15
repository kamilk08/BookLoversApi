using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Application.CommandHandlers.Tickets
{
    internal class ResolveTicketHandler : ICommandHandler<ResolveTicketCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDecisionProvider _decisionProvider;

        public ResolveTicketHandler(IUnitOfWork unitOfWork, IDecisionProvider decisionProvider)
        {
            this._unitOfWork = unitOfWork;
            this._decisionProvider = decisionProvider;
        }

        public async Task HandleAsync(ResolveTicketCommand command)
        {
            var librarian = await this._unitOfWork.GetAsync<Librarian>(command.WriteModel.LibrarianGuid);
            var ticket = await this._unitOfWork.GetAsync<Ticket>(command.WriteModel.TicketGuid);

            if (ticket == null || librarian == null)
                throw new BusinessRuleNotMetException($"Either ticket or librarian does not exist.");

            var justification =
                new DecisionJustification(command.WriteModel.DecisionJustification, command.WriteModel.Date);

            librarian.ResolveTicket(
                ticket,
                this._decisionProvider.GetDecision(command.WriteModel.DecisionType),
                justification,
                this._decisionProvider as IDecisionChecker);

            await this._unitOfWork.CommitAsync(librarian);
        }
    }
}