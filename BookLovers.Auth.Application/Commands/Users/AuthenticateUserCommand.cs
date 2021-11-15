using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    public class AuthenticateUserCommand : ICommand
    {
        public string Email { get; }

        public string Password { get; }

        public bool IsAuthenticated { get; internal set; }

        public AuthenticateUserCommand(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}