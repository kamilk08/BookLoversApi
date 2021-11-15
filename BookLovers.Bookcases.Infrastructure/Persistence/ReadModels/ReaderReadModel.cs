using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Persistence.ReadModels
{
    public class ReaderReadModel : IReadModel<ReaderReadModel>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public int Status { get; set; }

        public BookcaseReadModel Bookcase { get; set; }

        public int BookCaseId { get; set; }
    }
}