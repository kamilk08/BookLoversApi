using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands
{
    internal class ArchiveSettingsManagerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid SettingsManagerGuid { get; private set; }

        private ArchiveSettingsManagerInternalCommand()
        {
        }

        public ArchiveSettingsManagerInternalCommand(Guid settingsManagerGuid)
        {
            Guid = Guid.NewGuid();
            SettingsManagerGuid = settingsManagerGuid;
        }
    }
}