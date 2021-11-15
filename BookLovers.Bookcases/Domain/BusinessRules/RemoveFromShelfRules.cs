using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain.BookcaseBooks;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal sealed class RemoveFromShelfRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveFromShelfRules(Bookcase bookcase, Shelf shelf, BookcaseBook book)
        {
            FollowingRules.Add(new AggregateMustExist(bookcase.Guid));
            FollowingRules.Add(new AggregateMustExist(book.BookGuid));
            FollowingRules.Add(
                new AggregateMustBeActive(bookcase?.AggregateStatus.Value ?? AggregateStatus.Archived.Value));
            FollowingRules.Add(new BookcaseMustContainSelectedShelf(bookcase, shelf));
            FollowingRules.Add(new BookcaseMustContainSelectedBook(bookcase, book.BookGuid));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}