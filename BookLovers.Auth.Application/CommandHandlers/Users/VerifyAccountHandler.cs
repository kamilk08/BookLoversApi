using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class VerifyAccountHandler : ICommandHandler<VerifyAccountInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VerifyAccountHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(VerifyAccountInternalCommand command)
        {
            var user = await this._unitOfWork.GetAsync<User>(command.UserGuid);

            user.ConfirmAccount(command.ConfirmedAt);

            await this._unitOfWork.CommitAsync(user);
        }
    }
}