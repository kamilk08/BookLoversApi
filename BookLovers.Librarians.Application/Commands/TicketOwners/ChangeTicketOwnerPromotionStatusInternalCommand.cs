using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.TicketOwners
{
    internal class ChangeTicketOwnerPromotionStatusInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private ChangeTicketOwnerPromotionStatusInternalCommand()
        {
        }

        public ChangeTicketOwnerPromotionStatusInternalCommand(Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
        }
    }
}