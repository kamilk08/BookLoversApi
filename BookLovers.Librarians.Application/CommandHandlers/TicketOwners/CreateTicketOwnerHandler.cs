using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.TicketOwners;
using BookLovers.Librarians.Domain.TicketOwners;

namespace BookLovers.Librarians.Application.CommandHandlers.TicketOwners
{
    internal class CreateTicketOwnerHandler : ICommandHandler<CreateTicketOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketOwnerHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateTicketOwnerInternalCommand command)
        {
            var ticketOwner = new TicketOwner(Guid.NewGuid(), command.TicketOwnerGuid,
                command.TicketOwnerId);

            return this._unitOfWork.CommitAsync(ticketOwner);
        }
    }
}