using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Bookcases.Events.Bookcases;

namespace BookLovers.Bookcases.Domain.Services
{
    internal class BookcaseManager : IAggregateManager<Bookcase>
    {
        private readonly List<Func<Bookcase, IBusinessRule>> _rules =
            new List<Func<Bookcase, IBusinessRule>>();

        public BookcaseManager()
        {
            _rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            _rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(Bookcase aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            var bookcaseArchived = BookcaseArchived.Initialize()
                .WithBookcase(aggregate.Guid)
                .WithReader(aggregate.Additions.ReaderGuid)
                .WithSettingsManager(aggregate.Additions.SettingsManagerGuid)
                .WithShelfTracker(aggregate.Additions.ShelfRecordTrackerGuid);

            aggregate.ApplyChange(bookcaseArchived);
        }
    }
}