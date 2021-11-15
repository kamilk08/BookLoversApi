using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.SeriesCycle.Services;

namespace BookLovers.Publication.Domain.SeriesCycle.BusinessRules
{
    internal class SeriesMustBeUnique : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Series is not unique.";

        private readonly ISeriesUniquenessChecker _checker;
        private readonly Series _series;

        public SeriesMustBeUnique(ISeriesUniquenessChecker checker, Series series)
        {
            this._checker = checker;
            this._series = series;
        }

        public bool IsFulfilled()
        {
            return this._checker.IsUnique(this._series.Guid, this._series.SeriesName);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}