using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands
{
    public class SuspendLibrarianCommand : ICommand
    {
        public Guid UserGuid { get; }

        public SuspendLibrarianCommand(Guid userGuid)
        {
            this.UserGuid = userGuid;
        }
    }
}