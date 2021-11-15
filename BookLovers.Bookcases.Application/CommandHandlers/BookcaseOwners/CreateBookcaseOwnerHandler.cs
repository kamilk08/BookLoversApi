using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands;
using BookLovers.Bookcases.Domain.BookcaseOwners;

namespace BookLovers.Bookcases.Application.CommandHandlers.BookcaseOwners
{
    internal class CreateBookcaseOwnerHandler : ICommandHandler<CreateBookcaseOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookcaseOwnerHandler(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public Task HandleAsync(CreateBookcaseOwnerInternalCommand command)
        {
            var owner = new BookcaseOwner(Guid.NewGuid(), command.ReaderGuid,
                command.ReaderId, command.BookcaseGuid);

            return _unitOfWork.CommitAsync(owner);
        }
    }
}