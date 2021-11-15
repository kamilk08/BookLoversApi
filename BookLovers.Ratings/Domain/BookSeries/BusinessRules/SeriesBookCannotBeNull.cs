using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.BookSeries.BusinessRules
{
    internal class SeriesBookCannotBeNull : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Series book cannot be null";
        private readonly Book _book;

        public SeriesBookCannotBeNull(Book book)
        {
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._book != null;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}