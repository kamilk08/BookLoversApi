using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal sealed class ChangeShelfRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangeShelfRules(Bookcase bookcase, Shelf oldShelf, Shelf newShelf, Guid bookGuid)
        {
            FollowingRules.Add(new AggregateMustBeActive(bookcase?.AggregateStatus.Value ?? AggregateStatus.Archived.Value));
            FollowingRules.Add(new AggregateMustExist(bookcase.Guid));
            FollowingRules.Add(new BookcaseMustContainSelectedShelf(bookcase, oldShelf));
            FollowingRules.Add(new BookcaseMustContainSelectedShelf(bookcase, newShelf));
            FollowingRules.Add(new BookcaseMustContainSelectedBook(bookcase, bookGuid));
            FollowingRules.Add(new ShelvesMustBeDifferent(oldShelf, newShelf));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}