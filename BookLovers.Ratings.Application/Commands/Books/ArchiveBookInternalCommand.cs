using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Books
{
    internal class ArchiveBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid BookGuid { get; private set; }

        public Guid Guid { get; private set; }

        private ArchiveBookInternalCommand()
        {
        }

        public ArchiveBookInternalCommand(Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
        }
    }
}