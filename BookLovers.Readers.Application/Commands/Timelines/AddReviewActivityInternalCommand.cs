using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddReviewActivityInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReviewGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public DateTime AddedAt { get; private set; }

        public Guid Guid { get; private set; }

        private AddReviewActivityInternalCommand()
        {
        }

        public AddReviewActivityInternalCommand(
            Guid reviewGuid,
            Guid readerGuid,
            Guid bookGuid,
            DateTime addedAt)
        {
            ReviewGuid = reviewGuid;
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            AddedAt = addedAt;
            Guid = Guid.NewGuid();
        }
    }
}