using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands;
using BookLovers.Librarians.Domain.Librarians;

namespace BookLovers.Librarians.Application.CommandHandlers
{
    internal class AssignSuperAdminToLibrarianInternalHandler :
        ICommandHandler<AssignSuperAdminToLibrarianInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignSuperAdminToLibrarianInternalHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(AssignSuperAdminToLibrarianInternalCommand command)
        {
            return this._unitOfWork.CommitAsync(
                new Librarian(command.WriteModel.LibrarianGuid, command.WriteModel.ReaderGuid), false);
        }
    }
}