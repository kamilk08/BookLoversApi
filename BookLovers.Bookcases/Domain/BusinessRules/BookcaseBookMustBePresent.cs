using System;
using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain.BookcaseBooks;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class BookcaseBookMustBePresent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book does not exist.";

        private readonly BookcaseBook _bookcaseBook;

        public BookcaseBookMustBePresent(BookcaseBook bookcaseBook)
        {
            _bookcaseBook = bookcaseBook;
        }

        public bool IsFulfilled() => _bookcaseBook != null
                                     && _bookcaseBook.Guid != Guid.Empty;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}