using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.SeriesCycle.BusinessRules
{
    internal class PositionCannotBeAlreadyOccupied : IBusinessRule
    {
        private const string BrokenRuleErrorMessage =
            "Cannot add book to position that is already taken by other book.";

        private readonly Series _series;
        private readonly SeriesBook _seriesBook;

        public PositionCannotBeAlreadyOccupied(Series series, SeriesBook seriesBook)
        {
            this._series = series;
            this._seriesBook = seriesBook;
        }

        public bool IsFulfilled()
        {
            var currentBook = this._series.GetBook(this._seriesBook.Position);
            if (currentBook == null) return true;
            else return false;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}