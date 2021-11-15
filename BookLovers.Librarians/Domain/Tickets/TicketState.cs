using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class TicketState : Enumeration
    {
        public static readonly TicketState Solved = new TicketState(1, nameof(Solved));
        public static readonly TicketState InProgress = new TicketState(2, nameof(InProgress));

        protected TicketState()
        {
        }

        public TicketState(int value, string name)
            : base(value, name)
        {
        }
    }
}