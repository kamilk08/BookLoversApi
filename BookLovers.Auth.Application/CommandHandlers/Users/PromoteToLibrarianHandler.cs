using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Users;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class PromoteToLibrarianHandler : ICommandHandler<PromoteToLibrarianInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromoteToLibrarianHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(PromoteToLibrarianInternalCommand command)
        {
            var user = await this._unitOfWork.GetAsync<User>(command.ReaderGuid);

            RoleManager.Promote(user, Role.Librarian);

            await this._unitOfWork.CommitAsync(user);
        }
    }
}