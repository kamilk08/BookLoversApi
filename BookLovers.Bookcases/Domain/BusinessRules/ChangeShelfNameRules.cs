using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal sealed class ChangeShelfNameRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangeShelfNameRules(Bookcase bookcase, Shelf shelf)
        {
            FollowingRules.Add(new AggregateMustBeActive(bookcase?.AggregateStatus.Value ?? AggregateStatus.Archived.Value));
            FollowingRules.Add(new AggregateMustExist(bookcase.Guid));
            FollowingRules.Add(new BookcaseMustContainSelectedShelf(bookcase, shelf));
            FollowingRules.Add(new ShelfIsOfTypeCustom(shelf));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}