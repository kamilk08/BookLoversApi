using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class DecisionMade : Enumeration
    {
        public static readonly DecisionMade Accepted = new DecisionMade(1, nameof(Accepted));
        public static readonly DecisionMade Dismissed = new DecisionMade(2, nameof(Dismissed));

        protected DecisionMade(int value, string name)
            : base(value, name)
        {
        }
    }
}