using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    internal class SendEmailConfirmationInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public string Email { get; private set; }

        private SendEmailConfirmationInternalCommand()
        {
        }

        public SendEmailConfirmationInternalCommand(Guid guid, string email)
        {
            this.Guid = guid;
            this.Email = email;
        }
    }
}