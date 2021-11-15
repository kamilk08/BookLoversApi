using BookLovers.Base.Infrastructure.Queries;
using System;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class TicketOwnerReadModel : IReadModel<TicketOwnerReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public IList<CreatedTicketReadModel> Tickets { get; set; }

        public int Status { get; set; }

        public TicketOwnerReadModel()
        {
            this.Tickets = new List<CreatedTicketReadModel>();
        }
    }
}