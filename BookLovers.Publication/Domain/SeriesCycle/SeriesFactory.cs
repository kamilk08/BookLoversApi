using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.SeriesCycle.BusinessRules;
using BookLovers.Publication.Domain.SeriesCycle.Services;

namespace BookLovers.Publication.Domain.SeriesCycle
{
    public class SeriesFactory
    {
        private readonly List<Func<Series, IBusinessRule>> _rules =
            new List<Func<Series, IBusinessRule>>();

        public SeriesFactory(ISeriesUniquenessChecker uniquenessChecker)
        {
            this._rules.Add(series => new AggregateMustBeActive(series.AggregateStatus.Value));
            this._rules.Add(series => new SeriesMustBeUnique(uniquenessChecker, series));
        }

        public Series Create(Guid seriesGuid, string name)
        {
            var series = new Series(seriesGuid, name);

            foreach (var rule in this._rules)
            {
                if (!rule(series).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(series)
                        .BrokenRuleMessage);
            }

            return series;
        }
    }
}