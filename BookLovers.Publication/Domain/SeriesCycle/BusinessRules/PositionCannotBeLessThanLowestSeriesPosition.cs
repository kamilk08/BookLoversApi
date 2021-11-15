using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.SeriesCycle.BusinessRules
{
    internal class PositionCannotBeLessThanLowestSeriesPosition : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book position cannot be less than one.";
        private readonly int _position;

        public PositionCannotBeLessThanLowestSeriesPosition(int position)
        {
            this._position = position;
        }

        public bool IsFulfilled()
        {
            return this._position >= 1;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}