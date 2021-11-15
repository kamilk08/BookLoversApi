using BookLovers.Base.Infrastructure.Queries;
using System;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class LibrarianReadModel : IReadModel<LibrarianReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public IList<ResolvedTicketReadModel> Tickets { get; set; }

        public int Status { get; set; }

        public LibrarianReadModel()
        {
            this.Tickets = new List<ResolvedTicketReadModel>();
        }
    }
}