using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.BusinessRules;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookDetailsEditor : IBookEditor
    {
        private readonly List<Func<Book, BookData, IBusinessRule>> _rules;

        public BookDetailsEditor()
        {
            _rules = new List<Func<Book, BookData, IBusinessRule>>();

            _rules.Add((book, data) => new AggregateMustBeActive(book.AggregateStatus.Value));
            _rules.Add((book, data) => new LanguageTypeMustBeValid(data.DetailsData.Language));
        }

        public Task EditBook(Book book, BookData bookData)
        {
            foreach (var rule in _rules)
            {
                if (!rule(book, bookData).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(book, bookData).BrokenRuleMessage);
            }

            var bookDetails = new BookDetails(bookData.DetailsData.Pages, bookData.DetailsData.Language);
            if (book.Details == bookDetails)
                return Task.CompletedTask;

            var @event = new BookDetailsChanged(book.Guid, bookDetails.Pages,
                bookDetails.Language.Value);

            book.ApplyChange(@event);

            return Task.CompletedTask;
        }
    }
}