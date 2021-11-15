using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal sealed class AddToShelfRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddToShelfRules(Bookcase bookcase, Shelf shelf, Guid bookGuid)
        {
            FollowingRules.Add(new AggregateMustBeActive(bookcase?.AggregateStatus.Value ?? AggregateStatus.Archived.Value));
            FollowingRules.Add(new AggregateMustExist(bookcase?.Guid ?? Guid.Empty));

            FollowingRules.Add(new BookcaseMustContainSelectedShelf(bookcase, shelf));
            FollowingRules.Add(new BookCannotBeAlreadyOnShelfThatIsNotCustom(bookcase, shelf, bookGuid));
            FollowingRules.Add(new ShelfCannotHaveMultipleSameBooks(shelf, bookGuid));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}