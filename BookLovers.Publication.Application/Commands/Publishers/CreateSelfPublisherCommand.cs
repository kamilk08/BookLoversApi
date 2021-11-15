using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Publishers
{
    public class CreateSelfPublisherCommand : ICommand
    {
        public Guid PublisherGuid { get; private set; }

        private CreateSelfPublisherCommand()
        {
        }

        public CreateSelfPublisherCommand(Guid publisherGuid)
        {
            this.PublisherGuid = publisherGuid;
        }
    }
}