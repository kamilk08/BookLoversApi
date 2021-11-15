using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddBookActivityInternalCommand : ICommand, IInternalCommand
    {
        public Guid BookGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public DateTime AddedAt { get; private set; }

        public Guid Guid { get; private set; }

        private AddBookActivityInternalCommand()
        {
        }

        public AddBookActivityInternalCommand(Guid bookGuid, Guid readerGuid, DateTime addedAt)
        {
            BookGuid = bookGuid;
            ReaderGuid = readerGuid;
            AddedAt = addedAt;
            Guid = Guid.NewGuid();
        }
    }
}