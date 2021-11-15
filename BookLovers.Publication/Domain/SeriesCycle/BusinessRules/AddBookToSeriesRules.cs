using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.SeriesCycle.BusinessRules
{
    internal class AddBookToSeriesRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddBookToSeriesRules(Series series, SeriesBook seriesBook)
        {
            this.FollowingRules.Add(new AggregateMustExist(series.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(series.AggregateStatus.Value));
            this.FollowingRules.Add(new SeriesCannotContainDuplicatedBooks(series, seriesBook));
            this.FollowingRules.Add(new PositionCannotBeLessThanLowestSeriesPosition(seriesBook.Position));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}