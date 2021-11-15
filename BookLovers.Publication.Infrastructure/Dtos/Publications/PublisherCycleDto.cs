using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class PublisherCycleDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string CycleName { get; set; }

        public int PublisherId { get; set; }

        public Guid PublisherGuid { get; set; }

        public IList<int> CycleBooks { get; set; }
    }
}