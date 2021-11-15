using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Queries
{
    public class BookFilterCriteria
    {
        public List<int> Categories { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public DateTime? From { get; set; }

        public DateTime? Till { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }
    }
}