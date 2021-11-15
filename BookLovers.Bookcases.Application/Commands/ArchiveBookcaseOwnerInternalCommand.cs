using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands
{
    internal class ArchiveBookcaseOwnerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private ArchiveBookcaseOwnerInternalCommand()
        {
        }

        public ArchiveBookcaseOwnerInternalCommand(Guid readerGuid)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
        }
    }
}