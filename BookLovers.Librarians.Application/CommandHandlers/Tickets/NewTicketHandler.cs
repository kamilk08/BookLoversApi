using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Application.Extensions;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;

namespace BookLovers.Librarians.Application.CommandHandlers.Tickets
{
    internal class NewTicketHandler : ICommandHandler<NewTicketCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadContextAccessor _readContextAccessor;
        private readonly TicketFactorySetup _factorySetup;

        public NewTicketHandler(
            IUnitOfWork unitOfWork,
            IReadContextAccessor readContextAccessor,
            TicketFactorySetup factorySetup)
        {
            this._unitOfWork = unitOfWork;
            this._readContextAccessor = readContextAccessor;
            this._factorySetup = factorySetup;
        }

        public async Task HandleAsync(NewTicketCommand command)
        {
            var ticket = this._factorySetup
                .GetFactory(command.WriteModel.ConvertToTicketData()).Create();

            await this._unitOfWork.CommitAsync(ticket);

            command.WriteModel.TicketId = this._readContextAccessor.GetReadModelId(command.WriteModel.TicketGuid);
        }
    }
}