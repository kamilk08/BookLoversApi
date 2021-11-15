using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.BusinessRules;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Domain.ShelfCategories;

namespace BookLovers.Bookcases.Domain.Services
{
    public class BookcaseService : IDomainService<Bookcase>
    {
        private readonly List<Func<SettingsManager, Shelf, BookcaseBook, IBusinessRule>> _rules =
            new List<Func<SettingsManager, Shelf, BookcaseBook, IBusinessRule>>();

        private Bookcase _bookcase;
        private SettingsManager _settingsManager;

        public BookcaseService()
        {
            _rules.Add((manager, shelf, bookcaseBook) => new AggregateMustExist(manager.Guid));
            _rules.Add((manager, shelf, bookcaseBook) => new BookcaseBookMustBePresent(bookcaseBook));
            _rules.Add((manager, shelf, book) => new ShelfMustHaveEnoughSpace(manager, shelf));
        }

        public void AddBook(BookcaseBook bookcaseBook, Shelf shelf)
        {
            foreach (var rule in _rules)
            {
                if (!rule(_settingsManager, shelf, bookcaseBook).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(_settingsManager, shelf, bookcaseBook)
                        .BrokenRuleMessage);
            }

            _bookcase.AddToShelf(bookcaseBook.BookGuid, shelf);
        }

        public void ChangeShelf(BookcaseBook bookcaseBook, Shelf oldShelf, Shelf newShelf)
        {
            foreach (var rule in _rules)
            {
                if (!rule(_settingsManager, newShelf, bookcaseBook).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(_settingsManager, newShelf, bookcaseBook)
                        .BrokenRuleMessage);
            }

            _bookcase.ChangeShelf(bookcaseBook.BookGuid, oldShelf, newShelf);
        }

        public void SetBookcaseWithSettings(Bookcase bookcase, SettingsManager settingsManager)
        {
            _bookcase = _bookcase == null || settingsManager == null
                ? bookcase
                : throw new InvalidOperationException("Bookcase with settings already set.");

            _settingsManager = settingsManager;
        }

        public bool IsShelfOfType(Shelf shelf, ShelfCategory shelfCategory) =>
            shelf.ShelfDetails.Category.Value == shelfCategory.Value;
    }
}