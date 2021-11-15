using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class SeriesDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string SeriesName { get; set; }

        public IList<int> Books { get; set; }
    }
}