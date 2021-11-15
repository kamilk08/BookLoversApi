using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Audiences
{
    public class AuthenticateAudienceCommand : ICommand
    {
        public string AudienceId { get; }

        public string SecretKey { get; }

        public bool IsAuthenticated { get; internal set; }

        public AuthenticateAudienceCommand(string audienceId, string secretKey)
        {
            this.AudienceId = audienceId;
            this.SecretKey = secretKey;
        }
    }
}