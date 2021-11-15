using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Events.SeriesCycle;

namespace BookLovers.Publication.Domain.SeriesCycle.Services
{
    public class SeriesArchiver : IAggregateManager<Series>
    {
        private readonly List<Func<Series, IBusinessRule>> _rules =
            new List<Func<Series, IBusinessRule>>();

        public SeriesArchiver()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(Series aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            var seriesArchived =
                new SeriesArchived(aggregate.Guid, aggregate.Books.Select(s => s.BookGuid).AsEnumerable());

            aggregate.ApplyChange(seriesArchived);
        }
    }
}