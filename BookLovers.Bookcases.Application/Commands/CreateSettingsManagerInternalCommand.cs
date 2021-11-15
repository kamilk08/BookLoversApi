using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands
{
    internal class CreateSettingsManagerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid SettingsManagerGuid { get; private set; }

        private CreateSettingsManagerInternalCommand()
        {
        }

        public CreateSettingsManagerInternalCommand(Guid bookcaseGuid, Guid settingsManagerGuid)
        {
            Guid = Guid.NewGuid();
            BookcaseGuid = bookcaseGuid;
            SettingsManagerGuid = settingsManagerGuid;
        }
    }
}