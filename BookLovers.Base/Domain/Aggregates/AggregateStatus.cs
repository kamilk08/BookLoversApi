using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Base.Domain.Aggregates
{
    public class AggregateStatus : Enumeration
    {
        public static readonly AggregateStatus Active = new AggregateStatus(1, nameof(Active));
        public static readonly AggregateStatus Archived = new AggregateStatus(2, nameof(Archived));

        protected AggregateStatus()
        {
        }

        protected AggregateStatus(int value, string name)
            : base(value, name)
        {
        }
    }
}