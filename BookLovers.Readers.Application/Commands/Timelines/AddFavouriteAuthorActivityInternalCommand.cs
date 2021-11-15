using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddFavouriteAuthorActivityInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        private AddFavouriteAuthorActivityInternalCommand()
        {
        }

        public AddFavouriteAuthorActivityInternalCommand(Guid readerGuid, Guid authorGuid)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            AuthorGuid = authorGuid;
        }
    }
}