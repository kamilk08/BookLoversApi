using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Domain.Tokens;
using BookLovers.Auth.Domain.Users;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Tokens
{
    internal class RevokeTokenHandler : ICommandHandler<RevokeTokenCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeTokenHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RevokeTokenCommand command)
        {
            var token = await this._unitOfWork.GetAsync<RefreshToken>(command.WriteModel.TokenGuid);
            var user = await this._unitOfWork.GetAsync<User>(token.TokenIdentification.UserGuid);

            token.Revoke(user);

            await this._unitOfWork.CommitAsync(token);
        }
    }
}