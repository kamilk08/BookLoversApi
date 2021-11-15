using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Profile
{
    internal class CreateProfilePrivacyManagerInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        private CreateProfilePrivacyManagerInternalCommand()
        {
        }

        public CreateProfilePrivacyManagerInternalCommand(Guid profileGuid)
        {
            Guid = Guid.NewGuid();
            ProfileGuid = profileGuid;
        }
    }
}