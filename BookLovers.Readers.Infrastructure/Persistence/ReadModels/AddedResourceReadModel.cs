using System;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class AddedResourceReadModel
    {
        public int Id { get; set; }

        public Guid ResourceGuid { get; set; }
    }
}