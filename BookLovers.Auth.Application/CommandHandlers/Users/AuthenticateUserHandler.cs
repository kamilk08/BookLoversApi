using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class AuthenticateUserHandler : ICommandHandler<AuthenticateUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly UserAuthenticationService _authenticationService;

        public AuthenticateUserHandler(
            IUserRepository repository,
            UserAuthenticationService authenticationService)
        {
            this._repository = repository;
            this._authenticationService = authenticationService;
        }

        public async Task HandleAsync(AuthenticateUserCommand command)
        {
            var user = await this._repository.GetUserByEmailAsync(command.Email);

            command.IsAuthenticated = this._authenticationService.Authenticate(user, command.Password);
        }
    }
}