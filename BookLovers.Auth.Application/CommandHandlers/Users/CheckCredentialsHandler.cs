using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class CheckCredentialsHandler : ICommandHandler<CheckCredentialsCommand>
    {
        private readonly IHashingService _hashingService;
        private readonly IUserRepository _repository;

        public CheckCredentialsHandler(
            IHashingService hashingService,
            IUserRepository repository,
            IUnitOfWork unitOfWork)
        {
            this._hashingService = hashingService;
            this._repository = repository;
        }

        public async Task HandleAsync(CheckCredentialsCommand command)
        {
            var user = await this._repository.GetUserByNickNameAsync(command.UserName) ??
                       await this._repository.GetUserByEmailAsync(command.UserName);

            var generatedHash = this._hashingService.GetHash(command.Password, user.Account.AccountSecurity.Salt);

            command.IsAuthenticated = user.IsPasswordCorrect(generatedHash);
        }
    }
}