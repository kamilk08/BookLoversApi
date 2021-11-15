using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.PromotionWaiters
{
    internal class ChangePromotionWaiterStatusInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int PromotionWaiterStatus { get; private set; }

        private ChangePromotionWaiterStatusInternalCommand()
        {
        }

        public ChangePromotionWaiterStatusInternalCommand(Guid readerGuid, int promotionWaiterStatus)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
            this.PromotionWaiterStatus = promotionWaiterStatus;
        }
    }
}