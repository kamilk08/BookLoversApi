using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddAuthorActivityInternalCommand : ICommand, IInternalCommand
    {
        public Guid AuthorGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public DateTime AddedAt { get; private set; }

        public Guid Guid { get; private set; }

        private AddAuthorActivityInternalCommand()
        {
        }

        public AddAuthorActivityInternalCommand(Guid authorGuid, Guid readerGuid, DateTime addedAt)
        {
            AuthorGuid = authorGuid;
            ReaderGuid = readerGuid;
            AddedAt = addedAt;
            Guid = Guid.NewGuid();
        }
    }
}