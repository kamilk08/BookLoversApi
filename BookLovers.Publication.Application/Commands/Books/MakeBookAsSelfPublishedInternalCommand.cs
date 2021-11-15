using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class MakeBookAsSelfPublishedInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private MakeBookAsSelfPublishedInternalCommand()
        {
        }

        public MakeBookAsSelfPublishedInternalCommand(Guid publisherGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherGuid = publisherGuid;
            this.BookGuid = bookGuid;
        }
    }
}