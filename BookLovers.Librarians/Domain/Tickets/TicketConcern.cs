using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class TicketConcern : Enumeration
    {
        public static TicketConcern Book = new TicketConcern(1, nameof(Book));
        public static TicketConcern Author = new TicketConcern(2, nameof(Author));

        protected TicketConcern()
        {
        }

        public TicketConcern(int value, string name)
            : base(value, name)
        {
        }
    }
}