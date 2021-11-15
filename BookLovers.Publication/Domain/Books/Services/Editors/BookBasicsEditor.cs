using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.BusinessRules;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookBasicsEditor : IBookEditor
    {
        private readonly List<Func<Book, BookData, IBusinessRule>> _rules;

        public BookBasicsEditor(IsbnValidatorFactory isbnValidatorFactory)
        {
            _rules = new List<Func<Book, BookData, IBusinessRule>>();

            _rules.Add((book, data) => new AggregateMustExist(book.Guid));
            _rules.Add((book, data) => new AggregateMustBeActive(book.AggregateStatus.Value));
            _rules.Add((book, data) =>
                new IsbnNumberMustBeValid(
                    isbnValidatorFactory.GetValidator(data.BasicsData.Isbn),
                    data.BasicsData.Isbn));
            _rules.Add((book, data) => new BooksCategoryMustBeValid(data.BasicsData.BookCategory));
        }

        public Task EditBook(Book book, BookData bookData)
        {
            foreach (var rule in _rules)
            {
                if (!rule(book, bookData).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(book, bookData).BrokenRuleMessage);
            }

            var bookBasics = new BookBasics(bookData.BasicsData.Isbn, bookData.BasicsData.Title,
                bookData.BasicsData.PublicationDate, bookData.BasicsData.BookCategory);

            if (bookBasics == book.Basics)
                return Task.CompletedTask;

            var @event = BookBasicsChanged.Initialize()
                .WithAggregate(book.Guid)
                .WithTitleAndIsbn(bookBasics.Title, bookBasics.ISBN)
                .WithPublished(bookBasics.PublicationDate)
                .WithCategory(bookBasics.BookCategory.Category.Value, bookBasics.BookCategory.Category.Name)
                .WithSubCategory(bookBasics.BookCategory.SubCategory.Value, bookBasics.BookCategory.SubCategory.Name);

            book.ApplyChange(@event);

            return Task.CompletedTask;
        }
    }
}