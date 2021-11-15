using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.PasswordResets
{
    public class CreateTokenPasswordCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid UserGuid { get; private set; }

        public string Email { get; private set; }

        private CreateTokenPasswordCommand()
        {
        }

        public CreateTokenPasswordCommand(Guid userGuid, string email)
        {
            this.Guid = Guid.NewGuid();
            this.UserGuid = userGuid;
            this.Email = email;
        }
    }
}