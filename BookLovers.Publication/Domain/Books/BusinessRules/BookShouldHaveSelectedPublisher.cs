using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BookShouldHaveSelectedPublisher : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Created book should have selected publisher";

        private readonly BookPublisher _bookPublisher;

        public BookShouldHaveSelectedPublisher(BookPublisher bookPublisher)
        {
            this._bookPublisher = bookPublisher;
        }

        public bool IsFulfilled()
        {
            return this._bookPublisher != null;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}