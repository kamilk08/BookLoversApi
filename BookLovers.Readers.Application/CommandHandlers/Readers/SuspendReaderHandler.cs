using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class SuspendReaderHandler : ICommandHandler<SuspendReaderInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Reader> _manager;

        public SuspendReaderHandler(IUnitOfWork unitOfWork, IAggregateManager<Reader> manager)
        {
            _unitOfWork = unitOfWork;
            _manager = manager;
        }

        public async Task HandleAsync(SuspendReaderInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            _manager.Archive(reader);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}