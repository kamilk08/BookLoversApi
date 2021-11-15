using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.BookcaseBooks
{
    internal class ArchiveBookcaseBookInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        private ArchiveBookcaseBookInternalCommand()
        {
        }

        public ArchiveBookcaseBookInternalCommand(Guid bookGuid)
        {
            Guid = Guid.NewGuid();
            BookGuid = bookGuid;
        }
    }
}