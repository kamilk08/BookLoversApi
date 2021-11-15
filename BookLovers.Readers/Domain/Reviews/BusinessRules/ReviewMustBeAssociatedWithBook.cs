using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReviewMustBeAssociatedWithBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review must have a book.";

        private readonly ReviewIdentification _reviewIdentification;

        public ReviewMustBeAssociatedWithBook(ReviewIdentification reviewIdentification)
        {
            _reviewIdentification = reviewIdentification;
        }

        public bool IsFulfilled() => _reviewIdentification != null && _reviewIdentification.BookGuid != Guid.Empty;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}