using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands
{
    internal class SuspendLibrarianInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid UserGuid { get; private set; }

        private SuspendLibrarianInternalCommand()
        {
        }

        public SuspendLibrarianInternalCommand(Guid userGuid)
        {
            this.Guid = Guid.NewGuid();
            this.UserGuid = userGuid;
        }
    }
}