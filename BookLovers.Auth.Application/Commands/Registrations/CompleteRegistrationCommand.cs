using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Registrations
{
    public class CompleteRegistrationCommand : ICommand
    {
        public string Token { get; }

        public string Email { get; }

        public CompleteRegistrationCommand(string email, string token)
        {
            this.Email = email;
            this.Token = token;
        }
    }
}