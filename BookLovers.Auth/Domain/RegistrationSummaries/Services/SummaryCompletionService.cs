using System;
using System.Collections.Generic;
using BookLovers.Auth.Domain.RegistrationSummaries.BusinessRules;
using BookLovers.Auth.Events;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.RegistrationSummaries.Services
{
    public class SummaryCompletionService
    {
        private readonly List<Func<RegistrationSummary, string, IBusinessRule>> _rules =
            new List<Func<RegistrationSummary, string, IBusinessRule>>();

        public SummaryCompletionService()
        {
            _rules.Add((summary, token) => new AggregateMustExist(summary?.Guid ?? Guid.Empty));
            _rules.Add((summary, token) => new AggregateMustBeActive(summary?.Status ?? AggregateStatus.Archived.Value));
            _rules.Add((summary, token) => new RegistrationCannotBeCompleted(summary.Completion));
            _rules.Add((summary, token) => new RegistrationCompletionCannotBeExpired(summary));
            _rules.Add((summary, token) => new RegistrationTokensMustBeEqual(summary, token));
        }

        public RegistrationSummary CompleteRegistration(RegistrationSummary summary, string token)
        {
            foreach (var rule in _rules)
            {
                if (!rule(summary, token).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(summary, token).BrokenRuleMessage);
            }

            var now = DateTime.UtcNow;

            summary.Completion = summary.Completion.Completed(now);

            summary.AddEvent(new RegistrationSummaryCompleted(summary.Guid, summary.Identification.UserGuid, now));

            return summary;
        }
    }
}