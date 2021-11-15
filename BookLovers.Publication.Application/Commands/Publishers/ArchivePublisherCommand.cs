using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Publishers
{
    public class ArchivePublisherCommand : ICommand
    {
        public Guid PublisherGuid { get; }

        public ArchivePublisherCommand(Guid publisherGuid)
        {
            this.PublisherGuid = publisherGuid;
        }
    }
}