using System;

namespace BookLovers.Seed.Models
{
    public class SeedSeries
    {
        public Guid Guid { get; }

        public string Name { get; }

        public SeedSeries(Guid guid, string name)
        {
            this.Guid = guid;
            this.Name = name;
        }
    }
}