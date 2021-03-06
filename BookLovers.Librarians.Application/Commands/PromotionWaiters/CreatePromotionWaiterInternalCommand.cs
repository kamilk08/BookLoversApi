using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.PromotionWaiters
{
    internal class CreatePromotionWaiterInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        private CreatePromotionWaiterInternalCommand()
        {
        }

        public CreatePromotionWaiterInternalCommand(Guid readerGuid, int readerId)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
        }
    }
}