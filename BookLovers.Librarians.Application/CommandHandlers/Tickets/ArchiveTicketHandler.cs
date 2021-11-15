using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Application.CommandHandlers.Tickets
{
    internal class ArchiveTicketHandler : ICommandHandler<ArchiveTicketInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Ticket> _manager;

        public ArchiveTicketHandler(IUnitOfWork unitOfWork, IAggregateManager<Ticket> manager)
        {
            this._unitOfWork = unitOfWork;
            this._manager = manager;
        }

        public async Task HandleAsync(ArchiveTicketInternalCommand command)
        {
            var ticket = await this._unitOfWork.GetAsync<Ticket>(command.TicketGuid);

            this._manager.Archive(ticket);

            await this._unitOfWork.CommitAsync(ticket);
        }
    }
}