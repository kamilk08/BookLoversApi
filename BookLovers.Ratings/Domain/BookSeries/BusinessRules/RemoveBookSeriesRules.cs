using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.BookSeries.BusinessRules
{
    internal class RemoveBookSeriesRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveBookSeriesRules(Series series, Book book)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustExist(series.Guid));
            this.FollowingRules.Add(new SeriesMustHaveSelectedBook(series, book));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}