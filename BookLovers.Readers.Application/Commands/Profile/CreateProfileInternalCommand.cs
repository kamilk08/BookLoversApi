using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Profile
{
    internal class CreateProfileInternalCommand : ICommand, IInternalCommand
    {
        public Guid ProfileGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid Guid { get; private set; }

        public string Email { get; private set; }

        private CreateProfileInternalCommand()
        {
        }

        public CreateProfileInternalCommand(Guid profileGuid, Guid readerGuid, string email)
        {
            Email = email;
            ProfileGuid = profileGuid;
            ReaderGuid = readerGuid;
            Guid = Guid.NewGuid();
        }
    }
}