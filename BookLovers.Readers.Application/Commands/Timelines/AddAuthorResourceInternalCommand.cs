using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddAuthorResourceInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public int AuthorId { get; private set; }

        public DateTime Date { get; private set; }

        public Guid Guid { get; private set; }

        public AddAuthorResourceInternalCommand(
            Guid readerGuid,
            Guid authorGuid,
            int authorId,
            DateTime date)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            AuthorGuid = authorGuid;
            AuthorId = authorId;
            Date = date;
        }
    }
}