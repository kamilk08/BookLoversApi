using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Publisher.BusinessRules
{
    internal class PublisherCannotHaveDuplicatedBooks : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher cannot have multiple same books.";

        private readonly Publisher _publisher;
        private readonly Book _book;

        public PublisherCannotHaveDuplicatedBooks(Publisher publisher, Book book)
        {
            _publisher = publisher;
            _book = book;
        }

        public bool IsFulfilled()
        {
            return !_publisher.Books.Contains(_book);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}