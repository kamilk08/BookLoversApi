using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Publishers
{
    internal class ArchivePublisherInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        private ArchivePublisherInternalCommand()
        {
        }

        public ArchivePublisherInternalCommand(Guid publisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherGuid = publisherGuid;
        }
    }
}