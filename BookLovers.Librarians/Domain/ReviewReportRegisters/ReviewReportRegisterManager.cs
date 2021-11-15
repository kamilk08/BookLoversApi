using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Librarians.Events;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters
{
    internal class ReviewReportRegisterManager : IAggregateManager<ReviewReportRegister>
    {
        private readonly List<Func<ReviewReportRegister, IBusinessRule>> _rules =
            new List<Func<ReviewReportRegister, IBusinessRule>>();

        public ReviewReportRegisterManager()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.Status));
        }

        public void Archive(ReviewReportRegister aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ArchiveAggregate();

            aggregate.AddEvent(new ReviewRegistrationReportArchived(aggregate.Guid));
        }
    }
}