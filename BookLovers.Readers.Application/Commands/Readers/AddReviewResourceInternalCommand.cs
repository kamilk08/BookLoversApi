using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Readers
{
    internal class AddReviewResourceInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public DateTime Date { get; private set; }

        public Guid Guid { get; private set; }

        private AddReviewResourceInternalCommand()
        {
        }

        public AddReviewResourceInternalCommand(
            Guid readerGuid,
            Guid reviewGuid,
            Guid bookGuid,
            DateTime date)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            ReviewGuid = reviewGuid;
            BookGuid = bookGuid;
            Date = date;
        }
    }
}