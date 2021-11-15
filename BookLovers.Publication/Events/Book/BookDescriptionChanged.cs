using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookDescriptionChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string Description { get; private set; }

        public string DescriptionSource { get; private set; }

        private BookDescriptionChanged()
        {
        }

        public BookDescriptionChanged(Guid bookGuid, string description, string descriptionSource)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.Description = description;
            this.DescriptionSource = descriptionSource;
        }
    }
}