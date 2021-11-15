using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    public class CheckCredentialsCommand : ICommand
    {
        public string UserName { get; }

        public string Password { get; }

        public bool IsAuthenticated { get; internal set; }

        public CheckCredentialsCommand(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}