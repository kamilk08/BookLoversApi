using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Users;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class DegradeToReaderHandler : ICommandHandler<DegradeToReaderInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DegradeToReaderHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DegradeToReaderInternalCommand command)
        {
            var user = await this._unitOfWork.GetAsync<User>(command.UserGuid);

            RoleManager.Degrade(user, Role.Librarian);

            await this._unitOfWork.CommitAsync(user);
        }
    }
}