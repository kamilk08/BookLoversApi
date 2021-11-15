using BookLovers.Base.Infrastructure.Queries;
using System;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class PromotionWaiterReadModel : IReadModel<PromotionWaiterReadModel>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public int PromotionAvailability { get; set; }

        public int Status { get; set; }
    }
}