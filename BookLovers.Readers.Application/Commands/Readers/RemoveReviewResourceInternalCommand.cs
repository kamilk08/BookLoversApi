using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Readers
{
    internal class RemoveReviewResourceInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid Guid { get; private set; }

        private RemoveReviewResourceInternalCommand()
        {
        }

        public RemoveReviewResourceInternalCommand(Guid readerGuid, Guid reviewGuid)
        {
            ReaderGuid = readerGuid;
            ReviewGuid = reviewGuid;
            Guid = Guid.NewGuid();
        }
    }
}