using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.TicketOwners;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Application.CommandHandlers.TicketOwners
{
    internal class AddTicketToOwnerHandler : ICommandHandler<AddTicketToTheOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketOwnerRepository _ticketOwnerRepository;

        public AddTicketToOwnerHandler(
            IUnitOfWork unitOfWork,
            ITicketOwnerRepository ticketOwnerRepository)
        {
            this._unitOfWork = unitOfWork;
            this._ticketOwnerRepository = ticketOwnerRepository;
        }

        public async Task HandleAsync(AddTicketToTheOwnerInternalCommand command)
        {
            var ticket = await this._unitOfWork.GetAsync<Ticket>(command.TicketGuid);
            var ticketOwner = await this._ticketOwnerRepository.GetOwnerByReaderGuidAsync(command.TicketOwnerGuid);

            ticketOwner.AddTicket(ticket);

            await this._unitOfWork.CommitAsync(ticketOwner);
        }
    }
}