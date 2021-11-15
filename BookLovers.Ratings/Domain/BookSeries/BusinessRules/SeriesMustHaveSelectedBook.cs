using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.BookSeries.BusinessRules
{
    internal class SeriesMustHaveSelectedBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Series must have selected book.";
        private readonly Series _series;
        private readonly Book _book;

        public SeriesMustHaveSelectedBook(Series series, Book book)
        {
            this._series = series;
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._series.Books.Contains(this._book);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}