using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.TicketOwners;
using BookLovers.Librarians.Domain.TicketOwners;

namespace BookLovers.Librarians.Application.CommandHandlers.TicketOwners
{
    internal class SuspendTicketOwnerHandler : ICommandHandler<SuspendTicketOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketOwnerRepository _ticketOwnerRepository;
        private readonly IAggregateManager<TicketOwner> _manager;

        public SuspendTicketOwnerHandler(
            IUnitOfWork unitOfWork,
            ITicketOwnerRepository ticketOwnerRepository,
            IAggregateManager<TicketOwner> manager)
        {
            this._unitOfWork = unitOfWork;
            this._ticketOwnerRepository = ticketOwnerRepository;
            this._manager = manager;
        }

        public async Task HandleAsync(SuspendTicketOwnerInternalCommand command)
        {
            var ticketOwner = await this._ticketOwnerRepository.GetOwnerByReaderGuidAsync(command.TicketOwnerGuid);

            this._manager.Archive(ticketOwner);

            await this._unitOfWork.CommitAsync(ticketOwner);
        }
    }
}