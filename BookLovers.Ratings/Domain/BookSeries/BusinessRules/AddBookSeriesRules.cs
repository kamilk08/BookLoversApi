using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.BookSeries.BusinessRules
{
    internal class AddBookSeriesRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddBookSeriesRules(Series series, Book book)
        {
            this.FollowingRules.Add(new AggregateMustExist(series.Guid));
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(series.Status));
            this.FollowingRules.Add(new SeriesBookCannotBeNull(book));
            this.FollowingRules.Add(new SeriesCannotHaveDuplicatedBooks(series, book));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}