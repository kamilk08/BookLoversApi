using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class RemoveBookReviewInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private RemoveBookReviewInternalCommand()
        {
        }

        public RemoveBookReviewInternalCommand(Guid bookGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}