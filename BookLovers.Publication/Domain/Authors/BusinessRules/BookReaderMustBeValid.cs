using System;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.BookReaders;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class BookReaderMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid book reader";

        private readonly BookReader _bookReader;

        public BookReaderMustBeValid(BookReader bookReader) =>
            this._bookReader = bookReader;

        public bool IsFulfilled() => this._bookReader.IsActive()
                                     && this._bookReader.Guid != Guid.Empty;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}