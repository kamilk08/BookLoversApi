using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Registrations
{
    internal class CreateRegistrationSummaryInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid UserGuid { get; private set; }

        public string Email { get; private set; }

        public string Token { get; private set; }

        private CreateRegistrationSummaryInternalCommand()
        {
        }

        public CreateRegistrationSummaryInternalCommand(Guid userGuid, string email, string token)
        {
            this.Guid = Guid.NewGuid();
            this.UserGuid = userGuid;
            this.Email = email;
            this.Token = token;
        }
    }
}