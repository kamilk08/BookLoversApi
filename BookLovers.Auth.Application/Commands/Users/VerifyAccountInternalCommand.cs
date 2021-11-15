using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    internal class VerifyAccountInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid UserGuid { get; private set; }

        public DateTime ConfirmedAt { get; private set; }

        private VerifyAccountInternalCommand()
        {
        }

        public VerifyAccountInternalCommand(Guid userGuid, DateTime confirmedAt)
        {
            this.Guid = Guid.NewGuid();
            this.UserGuid = userGuid;
            this.ConfirmedAt = confirmedAt;
        }
    }
}