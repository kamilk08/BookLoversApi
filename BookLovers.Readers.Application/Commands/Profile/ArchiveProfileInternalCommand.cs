using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Profile
{
    internal class ArchiveProfileInternalCommand : ICommand, IInternalCommand
    {
        public Guid ProfileGuid { get; private set; }

        public Guid Guid { get; private set; }

        private ArchiveProfileInternalCommand()
        {
        }

        public ArchiveProfileInternalCommand(Guid profileGuid)
        {
            ProfileGuid = profileGuid;
            Guid = Guid.NewGuid();
        }
    }
}