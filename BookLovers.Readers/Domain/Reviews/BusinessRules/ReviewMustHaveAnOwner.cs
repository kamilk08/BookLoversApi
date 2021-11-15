using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReviewMustHaveAnOwner : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review must have an owner.";

        private readonly ReviewIdentification _reviewIdentification;

        public ReviewMustHaveAnOwner(ReviewIdentification reviewIdentification)
        {
            _reviewIdentification = reviewIdentification;
        }

        public bool IsFulfilled() =>
            _reviewIdentification != null
            && _reviewIdentification.ReaderGuid != Guid.Empty;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}