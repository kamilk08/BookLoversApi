using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class AddBookReviewInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        private AddBookReviewInternalCommand()
        {
        }

        public AddBookReviewInternalCommand(Guid bookGuid, Guid readerGuid, Guid reviewGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
            this.ReaderGuid = readerGuid;
            this.ReviewGuid = reviewGuid;
        }
    }
}