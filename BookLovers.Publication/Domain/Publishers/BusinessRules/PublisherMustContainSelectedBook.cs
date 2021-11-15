using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Publishers.BusinessRules
{
    internal class PublisherMustContainSelectedBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher does not have selected book.";

        private readonly Publisher _publisher;
        private readonly PublisherBook _publisherBook;

        public PublisherMustContainSelectedBook(Publisher publisher, PublisherBook publisherBook)
        {
            this._publisher = publisher;
            this._publisherBook = publisherBook;
        }

        public bool IsFulfilled() =>
            this._publisher.Books.Contains(this._publisherBook);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}