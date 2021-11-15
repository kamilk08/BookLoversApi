using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Domain;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class ArchiveBookcaseHandler : ICommandHandler<ArchiveBookcaseInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Bookcase> _manager;

        public ArchiveBookcaseHandler(IUnitOfWork unitOfWork, IAggregateManager<Bookcase> manager)
        {
            _unitOfWork = unitOfWork;
            _manager = manager;
        }

        public async Task HandleAsync(ArchiveBookcaseInternalCommand command)
        {
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.BookcaseGuid);

            _manager.Archive(bookcase);

            await _unitOfWork.CommitAsync(bookcase);
        }
    }
}