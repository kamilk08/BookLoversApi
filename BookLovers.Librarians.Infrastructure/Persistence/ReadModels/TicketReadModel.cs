using BookLovers.Base.Infrastructure.Queries;
using System;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class TicketReadModel : IReadModel<TicketReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid TicketObjectGuid { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public byte TicketStateValue { get; set; }

        public string TicketState { get; set; }

        public byte TicketConcernValue { get; set; }

        public string TicketConcern { get; set; }

        public byte DecisionValue { get; set; }

        public string Decision { get; set; }

        public Guid? LibrarianGuid { get; set; }

        public int TicketOwnerId { get; set; }

        public Guid TicketOwnerGuid { get; set; }

        public int Status { get; set; }
    }
}