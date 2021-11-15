using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.TicketReasons;
using BookLovers.Librarians.Events.TicketOwners;

namespace BookLovers.Librarians.Domain.Librarians.DecisionNotifiers
{
    internal class AcceptBookDecisionNotifier : IDecisionNotifier
    {
        private readonly ITicketOwnerRepository _ticketOwnerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ITicketSummary TicketSummary => new BookTicketAccepted();

        public AcceptBookDecisionNotifier(
            ITicketOwnerRepository ticketOwnerRepository,
            IUnitOfWork unitOfWork)
        {
            this._ticketOwnerRepository = ticketOwnerRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task Notify(Ticket ticket, string justification)
        {
            var ticketOwner =
                await this._ticketOwnerRepository.GetOwnerByReaderGuidAsync(ticket.IssuedBy.TicketOwnerGuid);

            this.TicketSummary.SetNotification(justification);

            if (this.TicketSummary.IsValid())
            {
                var createdBookAccepted = CreatedBookAccepted.Initialize()
                    .WithAggregate(ticketOwner.Guid)
                    .AcceptedBy(ticket.SolvedBy.LibrarianGuid.Value)
                    .WithTicketOwner(ticketOwner.ReaderGuid)
                    .WithTicket(ticket.Guid, this.TicketSummary.Notification)
                    .WithBook(ticket.TicketContent.TicketObjectGuid, ticket.TicketContent.Content);

                ticketOwner.NotifyOwner(createdBookAccepted);
            }

            await this._unitOfWork.CommitAsync(ticketOwner);
        }
    }
}