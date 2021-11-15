using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters.BusinessRules
{
    internal sealed class AddReviewReportRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddReviewReportRules(ReviewReportRegister reportRegister, ReportRegisterItem reportRegisterItem)
        {
            this.FollowingRules.Add(new AggregateMustExist(reportRegister.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(reportRegister.Status));
            this.FollowingRules.Add(new ReviewReportCannotBeDuplicated(reportRegister, reportRegisterItem));
            this.FollowingRules.Add(new ReviewReportRegistrationCannotBeCompleted(reportRegister));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}