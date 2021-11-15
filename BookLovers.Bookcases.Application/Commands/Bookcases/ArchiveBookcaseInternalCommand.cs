using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.Bookcases
{
    internal class ArchiveBookcaseInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        private ArchiveBookcaseInternalCommand()
        {
        }

        public ArchiveBookcaseInternalCommand(Guid bookcaseGuid)
        {
            Guid = Guid.NewGuid();
            BookcaseGuid = bookcaseGuid;
        }
    }
}