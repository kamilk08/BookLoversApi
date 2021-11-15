using System;

namespace BookLovers.Publication.Application.WriteModels.Publisher
{
    public class AddPublisherWriteModel
    {
        public int PublisherId { get; set; }

        public Guid PublisherGuid { get; set; }

        public string Name { get; set; }
    }
}