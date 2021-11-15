using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.SeriesCycle.BusinessRules
{
    internal class SeriesCannotContainDuplicatedBooks : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book series cannot contain duplciated book.";

        private readonly Series _series;
        private readonly SeriesBook _seriesBook;

        public SeriesCannotContainDuplicatedBooks(Series series, SeriesBook seriesBook)
        {
            this._series = series;
            this._seriesBook = seriesBook;
        }

        public bool IsFulfilled()
        {
            return !this._series.Books.Contains(this._seriesBook);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}