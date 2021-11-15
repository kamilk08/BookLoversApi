using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class ChangeBookPublisherInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        private ChangeBookPublisherInternalCommand()
        {
        }

        public ChangeBookPublisherInternalCommand(Guid bookGuid, Guid publisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
            this.PublisherGuid = publisherGuid;
        }
    }
}