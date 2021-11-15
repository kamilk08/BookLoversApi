using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Bookcases.Domain.BusinessRules;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Events.Shelf;

namespace BookLovers.Bookcases.Domain.Services
{
    public class BookcaseFactory
    {
        private readonly IDictionary<string, int> _coreShelves =
            new Dictionary<string, int>();

        private readonly List<Func<Bookcase, IBusinessRule>> _businessRules =
            new List<Func<Bookcase, IBusinessRule>>();

        public BookcaseFactory()
        {
            _businessRules.Add(bookcase => new AggregateMustExist(bookcase.Guid));
            _businessRules.Add(bookcase => new AggregateMustBeActive(bookcase.AggregateStatus.Value));
            _businessRules.Add(bookcase => new BookcaseMustContainCoreShelves(bookcase));

            _coreShelves.Add(ShelfCategory.Read.Name, ShelfCategory.Read.Value);
            _coreShelves.Add(ShelfCategory.NowReading.Name, ShelfCategory.NowReading.Value);
            _coreShelves.Add(ShelfCategory.WantToRead.Name, ShelfCategory.WantToRead.Value);
        }

        public Bookcase Create(Guid bookcaseGuid, Guid readerGuid, int readerId)
        {
            var bookcase = new Bookcase(bookcaseGuid, BookcaseAdditions.Create(readerGuid));

            var bookcaseCreated = BookcaseCreated
                .Initialize()
                .WithBookcase(bookcaseGuid)
                .WithReader(readerGuid, readerId)
                .WithSettingsManager(bookcase.Additions.SettingsManagerGuid)
                .WithShelfRecordTracker(bookcase.Additions.ShelfRecordTrackerGuid);

            bookcase.ApplyChange(bookcaseCreated);

            _coreShelves.ForEach(shelf => bookcase.ApplyChange(
                CoreShelfCreated.Initialize()
                    .WithBookcase(bookcaseGuid)
                    .WithShelf(shelf.Key, shelf.Value)));

            foreach (var businessRule in _businessRules)
            {
                if (!businessRule(bookcase).IsFulfilled())
                    throw new BusinessRuleNotMetException(businessRule(bookcase).BrokenRuleMessage);
            }

            return bookcase;
        }
    }
}