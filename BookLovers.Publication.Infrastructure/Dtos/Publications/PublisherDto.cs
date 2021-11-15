using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class PublisherDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }

        public byte Status { get; set; }

        public IList<int> Books { get; set; }

        public IList<int> Cycles { get; set; }
    }
}