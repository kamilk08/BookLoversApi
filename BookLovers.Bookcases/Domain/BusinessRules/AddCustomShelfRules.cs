using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal sealed class AddCustomShelfRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddCustomShelfRules(Bookcase bookcase, Shelf shelf)
        {
            FollowingRules.Add(new AggregateMustExist(bookcase.Guid));
            FollowingRules.Add(new AggregateMustBeActive(bookcase?.AggregateStatus.Value ?? AggregateStatus.Archived.Value));

            FollowingRules.Add(new CannotHaveDuplicatedCustomShelf(bookcase, shelf));
            FollowingRules.Add(new ShelfIsOfTypeCustom(shelf));
            FollowingRules.Add(new ShelfWithGivenNameAlreadyExists(bookcase, shelf.ShelfDetails.ShelfName));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}