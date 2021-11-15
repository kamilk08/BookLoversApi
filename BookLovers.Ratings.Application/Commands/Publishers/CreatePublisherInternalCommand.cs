using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Publishers
{
    internal class CreatePublisherInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public int PublisherId { get; private set; }

        private CreatePublisherInternalCommand()
        {
        }

        public CreatePublisherInternalCommand(Guid publisherGuid, int publisherId)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherGuid = publisherGuid;
            this.PublisherId = publisherId;
        }
    }
}