using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Infrastructure.Queries.Quotes
{
    public class QuotesOrder : Enumeration
    {
        public static readonly QuotesOrder ById = new QuotesOrder(1, "By Id");
        public static readonly QuotesOrder ByLikes = new QuotesOrder(2, "By Likes");

        protected QuotesOrder(int value, string name)
            : base(value, name)
        {
        }
    }
}