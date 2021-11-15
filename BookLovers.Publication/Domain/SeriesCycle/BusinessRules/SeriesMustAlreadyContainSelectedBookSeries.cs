using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.SeriesCycle.BusinessRules
{
    internal class SeriesMustAlreadyContainSelectedBookSeries : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Series must contain selected book.";

        private readonly Series _series;
        private readonly SeriesBook _seriesBook;

        public SeriesMustAlreadyContainSelectedBookSeries(Series series, SeriesBook seriesBook)
        {
            this._series = series;
            this._seriesBook = seriesBook;
        }

        public bool IsFulfilled()
        {
            return this._series.Books.Any(a => a.BookGuid == this._seriesBook.BookGuid);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}