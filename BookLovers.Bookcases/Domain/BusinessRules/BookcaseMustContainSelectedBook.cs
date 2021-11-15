using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class BookcaseMustContainSelectedBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Bookcase does not have selected book.";

        private readonly Bookcase _bookcase;
        private readonly Guid _bookGuid;

        public BookcaseMustContainSelectedBook(Bookcase bookcase, Guid bookGuid)
        {
            _bookcase = bookcase;
            _bookGuid = bookGuid;
        }

        public bool IsFulfilled() => _bookcase.ContainsBook(_bookGuid);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}