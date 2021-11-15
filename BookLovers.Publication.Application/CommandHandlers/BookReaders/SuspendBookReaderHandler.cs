using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.BookReaders;
using BookLovers.Publication.Domain.BookReaders;

namespace BookLovers.Publication.Application.CommandHandlers.BookReaders
{
    internal class SuspendBookReaderHandler : ICommandHandler<SuspendBookReaderInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<BookReader> _aggregateManager;
        private readonly IBookReaderAccessor _accessor;

        public SuspendBookReaderHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<BookReader> aggregateManager,
            IBookReaderAccessor accessor)
        {
            this._unitOfWork = unitOfWork;
            this._aggregateManager = aggregateManager;
            this._accessor = accessor;
        }

        public async Task HandleAsync(SuspendBookReaderInternalCommand command)
        {
            var aggregateGuid = await this._accessor.GetAggregateGuidAsync(command.ReaderGuid);

            var bookReader =
                await this._unitOfWork.GetAsync<BookReader>(aggregateGuid);

            this._aggregateManager.Archive(bookReader);

            await this._unitOfWork.CommitAsync(bookReader);
        }
    }
}