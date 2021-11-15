using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Readers
{
    internal class ChangeReaderEmailInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public string Email { get; private set; }

        private ChangeReaderEmailInternalCommand()
        {
        }

        public ChangeReaderEmailInternalCommand(Guid guid, string email)
        {
            Guid = guid;
            Email = email;
        }
    }
}