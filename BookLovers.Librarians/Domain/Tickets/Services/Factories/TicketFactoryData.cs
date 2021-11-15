using System;

namespace BookLovers.Librarians.Domain.Tickets.Services.Factories
{
    public class TicketFactoryData
    {
        public Guid TicketGuid { get; private set; }

        public Guid TicketObjectGuid { get; private set; }

        public TicketContentData TicketContentData { get; private set; }

        public TicketDetailsData TicketDetailsData { get; private set; }

        private TicketFactoryData()
        {
        }

        public static TicketFactoryData Initialize()
        {
            return new TicketFactoryData();
        }

        public TicketFactoryData WithGuid(Guid ticketGuid)
        {
            this.TicketGuid = ticketGuid;

            return this;
        }

        public TicketFactoryData WithTicketObject(Guid ticketObjectGuid)
        {
            this.TicketObjectGuid = ticketObjectGuid;

            return this;
        }

        public TicketFactoryData WithContent(TicketContentData ticketContentData)
        {
            this.TicketContentData = ticketContentData;

            return this;
        }

        public TicketFactoryData WithDetails(TicketDetailsData detailsData)
        {
            this.TicketDetailsData = detailsData;

            return this;
        }
    }
}