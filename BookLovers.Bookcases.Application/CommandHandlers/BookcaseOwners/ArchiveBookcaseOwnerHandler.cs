using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands;
using BookLovers.Bookcases.Application.Contracts;
using BookLovers.Bookcases.Domain.BookcaseOwners;

namespace BookLovers.Bookcases.Application.CommandHandlers.BookcaseOwners
{
    internal class ArchiveBookcaseOwnerHandler : ICommandHandler<ArchiveBookcaseOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookcaseOwnerAccessor _bookcaseOwnerAccessor;
        private readonly IAggregateManager<BookcaseOwner> _manager;

        public ArchiveBookcaseOwnerHandler(
            IUnitOfWork unitOfWork,
            IBookcaseOwnerAccessor bookcaseOwnerAccessor,
            IAggregateManager<BookcaseOwner> manager)
        {
            _unitOfWork = unitOfWork;
            _bookcaseOwnerAccessor = bookcaseOwnerAccessor;
            _manager = manager;
        }

        public async Task HandleAsync(ArchiveBookcaseOwnerInternalCommand command)
        {
            var ownerGuid = await _bookcaseOwnerAccessor.GetOwnerAggregateGuidAsync(command.ReaderGuid);

            var owner = await _unitOfWork.GetAsync<BookcaseOwner>(ownerGuid);

            _manager.Archive(owner);

            await _unitOfWork.CommitAsync(owner);
        }
    }
}