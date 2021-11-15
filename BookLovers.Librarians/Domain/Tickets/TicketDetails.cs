using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class TicketDetails : ValueObject<TicketDetails>
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        private TicketDetails()
        {
        }

        public TicketDetails(string title, string description, DateTime date)
        {
            this.Title = title;
            this.Description = description;
            this.Date = date;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Date.GetHashCode();

            // hash = (hash * 23) + this.Description.GetHashCode();
            hash = (hash * 23) + this.Title.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(TicketDetails obj) =>
            this.Title == obj.Title && this.Date == obj.Date;
    }
}