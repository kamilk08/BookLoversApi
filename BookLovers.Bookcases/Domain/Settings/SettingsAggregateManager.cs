using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Bookcases.Events.Settings;

namespace BookLovers.Bookcases.Domain.Settings
{
    internal class SettingsAggregateManager : IAggregateManager<SettingsManager>
    {
        private readonly List<Func<SettingsManager, IBusinessRule>> _rules;

        public SettingsAggregateManager()
        {
            _rules = new List<Func<SettingsManager, IBusinessRule>>();

            _rules.Add(manager => new AggregateMustExist(manager.BookcaseGuid));
            _rules.Add(manager => new AggregateMustBeActive(manager.AggregateStatus.Value));
        }

        public void Archive(SettingsManager aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new SettingsManagerArchived(aggregate.Guid));
        }
    }
}